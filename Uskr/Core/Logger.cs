using System;
using System.Diagnostics;

namespace Uskr.Core
{
    public static class Logger
    {
        public static Action<string> Hook { get; set; } = (x) => { };


        public static void Log(string s)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("LOG");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            Console.ResetColor();

            Console.WriteLine(s);

            Hook($"[LOG] {s}");
        }

        public static void Warn(string s)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("WARNING");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            Console.ResetColor();

            Console.WriteLine(s);
            
            Hook($"[Warn] {s}");
        }

        public static void Error(string s)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ERROR");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            Console.ResetColor();
            Console.WriteLine(s);
            
            Hook($"[Error] {s}");
        }

        public static void Debug(string s)
        {
            try
            {
                var stackTrace = new StackTrace();
                var frame = stackTrace.GetFrame(1);
                var meth = frame.GetMethod();

                var args = "";

                foreach (var info in meth.GetParameters())
                {
                    args += info.ParameterType.Name + ",";
                }

                s = "[" + meth.DeclaringType.Name + "::" + meth.Name + "(" +
                    args.Trim(',') + ")]" + s;
            }
            catch (Exception)
            {
            }


            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("DEBUG");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            Console.ResetColor();
            Console.WriteLine(s);
            
            Hook($"[DEBUG] {s}");
        }
    }
}