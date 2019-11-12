using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading;
using dnlib.DotNet;
using Uskr.Core;
using Uskr.IR;
using Uskr.PipeLine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Uskr
{
    public static class UskrEngine
    {
        private static List<IPipeLine> PipeLine { get; set; } = new List<IPipeLine>()
        {
            new IRBuilder(),
            new StaticBuilder()
        };


        public static void Run()
        {
            Logger.Debug($"Running");

            var ctx = new UskrContext();
            ctx.UserAssembly = Assembly.GetCallingAssembly();

            Logger.Hook = ctx.Comment;

            foreach (var pipe in PipeLine)
            {
                pipe.Run(ref ctx);
            }


            //first we need to load up the cfg file

            var names = Assembly.GetCallingAssembly().GetManifestResourceNames();
            var name = names.First(x => x.ToLower().EndsWith("build.yaml"));

            var input = new StringReader(Utils.GetResourceFileNoPath(name, Assembly.GetCallingAssembly()));

            var deserializer = new DeserializerBuilder()
                .Build();

            var cfg = deserializer.Deserialize<BuildCfg>(input);


            if (!Directory.Exists("Uskr")) Directory.CreateDirectory("Uskr");
            Directory.CreateDirectory(Path.Combine("Uskr", "img"));

            File.WriteAllText(Path.Combine("Uskr", "usercode.asm"), ctx.Nasm.ToString());

            File.WriteAllText(Path.Combine("Uskr", "linker.ld"), Utils.GetResourceFile("linker.ld"));
            File.WriteAllText(Path.Combine("Uskr", "code.c"), Utils.GetResourceFile("code.c"));
            File.WriteAllText(Path.Combine("Uskr", "boot.asm"),
                Utils.GetResourceFile("boot.asm").Replace("{{CALL}}",
                    Utils.MD5(ctx.UserSpace.Methods.First(x => x.Identifier == "Main").Namespace)));
            
            

            var wrkDir = Path.GetFullPath("Uskr");

            //now nasm the build and user code
            if (!RunNasm("-f elf -o ./boot.o .\\boot.asm", wrkDir, cfg)) Environment.Exit(0);
            if (!RunNasm("-f elf -o ./kernel.o .\\usercode.asm", wrkDir, cfg)) Environment.Exit(0);

            RunGcc(
                "-masm=intel -m32 -std=gnu99 -fno-builtin " +
                $"-ffreestanding -w -O2 -Wall -Wextra -nostartfiles -nostdlib -fno-stack-protector -c \"{Path.GetFullPath(Path.Combine(wrkDir, "code.c"))}\"" +
                $" -o \"{Path.GetFullPath(Path.Combine(wrkDir, "code.o"))}\" ", cfg.GccRoot, cfg);//;) Environment.Exit(0);


            //link it
            if (!RunLinker("-T ./linker.ld -o ./img/kernel.bin ./kernel.o ./code.o ./boot.o ", wrkDir, cfg)) Environment.Exit(0);

          //  RunObjCopy("-O elf32-i386 ./kernel.bin ./img/kernel.bin", wrkDir, cfg);
            
            //create boot image
            //first we need to make sure the tool exists

            if (!File.Exists("./Uskr/GrubImgTool.exe"))
            {
                var zip = Path.GetFullPath("GrubImgTool.zip");
                if (File.Exists(zip)) File.Delete(zip);
                using (var wc = new WebClient())
                {
                    wc.DownloadFile("https://github.com/Myvar/GrubImgTool/releases/download/V0.2/GrubImgTool.zip",
                        zip);
                }

                ZipFile.ExtractToDirectory(zip, wrkDir);
                File.Delete(zip);
            }

            var img = Path.Combine("Uskr", "os.img");

            if (File.Exists(img)) File.Delete(img);

            RunGrubImageTool("-i ./img/ -o ./os.img", wrkDir, cfg);

            bool whileOpen = true;


            ThreadPool.QueueUserWorkItem((x) =>
            {
                //  Thread.Sleep(1000);

                var client = new TcpClient();
                client.Connect(IPAddress.Parse("127.0.0.1"), 4444);

                var ns = client.GetStream();

                var last = 0;

                while (whileOpen && client.Connected)
                {
                    if (ns.DataAvailable)
                    {
                        var buf = new byte[4096];
                        int size = ns.Read(buf, 0, buf.Length);
                        Array.Resize(ref buf, size);

                        foreach (var b in buf)
                        {
                            if (last == (byte) '\n')
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("[");
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write("KERNEL");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("] ");
                                Console.ResetColor();
                            }

                            Console.Write((char) b);
                            last = b;
                        }
                    }
                }
            });

            RunQemu(" -fda os.img -serial tcp:localhost:4444,server,nowait", wrkDir, cfg);

            whileOpen = false;

            Environment.Exit(0);
        }

        //@Crossplatfrom exe is hard coded thats bad for now
        public static bool RunGcc(string args, string workingdir, BuildCfg cfg) =>
            StartProcess(Path.Combine(cfg.GccRoot, "gcc.exe"), workingdir, args);

        public static bool RunObjCopy(string args, string workingdir, BuildCfg cfg) =>
            StartProcess(Path.Combine(cfg.GccRoot, "objcopy.exe"), workingdir, args);
        
        public static bool RunNasm(string args, string workingdir, BuildCfg cfg) =>
            StartProcess(Path.Combine(cfg.NasmExe, "nasm.exe"), workingdir, args);

        public static bool RunLinker(string args, string workingdir, BuildCfg cfg) =>
            StartProcess(Path.Combine(cfg.GccRoot, "ld.exe"), workingdir, args);

        public static bool RunGrubImageTool(string args, string workingdir, BuildCfg cfg) =>
            StartProcess("./Uskr/GrubImgTool.exe", workingdir, args);

        public static bool RunQemu(string args, string workingdir, BuildCfg cfg) =>
            StartProcess(Path.Combine(cfg.QemuRoot, "qemu-system-x86_64"), workingdir, args, true, false);


        public static bool StartProcess(string pname, string workingdir, string args, bool WaitForExit = true,
            bool UseShellExecute = false)
        {
            var p = new Process();

            p.StartInfo.FileName = pname;
            p.StartInfo.WorkingDirectory = workingdir;
            p.StartInfo.Arguments = args;
            p.StartInfo.UseShellExecute = UseShellExecute;
            p.StartInfo.CreateNoWindow = !UseShellExecute;
            p.StartInfo.RedirectStandardError = !UseShellExecute;
            p.StartInfo.RedirectStandardOutput = !UseShellExecute;

            p.Start();
            if (WaitForExit)
            {
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                p.OutputDataReceived += (sender, eventArgs) =>
                {
                    if (eventArgs == null || eventArgs.Data == null) return;
                    if (eventArgs.Data.ToLower().Contains("warn"))
                    {
                        Logger.Warn(eventArgs.Data);
                    }
                    else
                    {
                        Logger.Log(eventArgs.Data);
                    }
                };

                p.ErrorDataReceived += (sender, eventArgs) =>
                {
                    if (eventArgs == null || eventArgs.Data == null) return;
                    if (eventArgs.Data.ToLower().Contains("warn"))
                    {
                        Logger.Warn(eventArgs.Data);
                    }
                    else
                    {
                        Logger.Log(eventArgs.Data);
                    }
                };

                p.WaitForExit();

                // LogStream(p.StandardOutput);
                // ErrorStream(p.StandardError);
            }
            else
            {
                return true;
            }

            return p.ExitCode == 0;
        }

        public static void LogStream(StreamReader s)
        {
            var ln = s.ReadLine();
            while (ln != null)
            {
                if (ln.ToLower().Contains("warn"))
                {
                    Logger.Warn(ln);
                }
                else
                {
                    Logger.Log(ln);
                }

                ln = s.ReadLine();
            }
        }

        public static void ErrorStream(StreamReader s)
        {
            var ln = s.ReadLine();
            while (ln != null)
            {
                if (ln.ToLower().Contains("warn"))
                {
                    Logger.Warn(ln);
                }
                else
                {
                    Logger.Error(ln);
                }

                ln = s.ReadLine();
            }
        }
    }
}