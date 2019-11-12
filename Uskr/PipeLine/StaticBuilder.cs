using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;
using Uskr.Opcodes;

namespace Uskr.PipeLine
{
    public class StaticBuilder : IPipeLine
    {
        public static Dictionary<Code, IOpcodeProcessor> Handlers { get; set; } =
            new Dictionary<Code, IOpcodeProcessor>();

        static StaticBuilder()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetInterface("IOpcodeProcessor") != null)
                {
                    if (type.CustomAttributes.Count() > 0)
                    {
                        var att = type.GetCustomAttribute<HandlerAttribute>();
                        if (att != null)
                        {
                            Handlers.Add(att.Opcode, (IOpcodeProcessor) Activator.CreateInstance(type));
                        }
                    }
                }
            }
        }

        public void Run(ref UskrContext context)
        {
            context.Asm("BITS 32");
            context.Asm("extern _70750F0AFB2995CC120C9F760274C2F9");
            context.Asm("extern _D271CF90018F771C18C5300730392647");
            context.Asm("extern _753F891548C9A78650968C6000A46A20");
            context.Asm("extern _097AAAA262E390E5C65A88581F0AA86C");
            context.Asm("extern _D45553A71DE6C0DA793315EA0AE8046F");
            context.Asm("extern _79A0D7AA956F5CBEDDED49D524FEF1A9");
            context.Asm("extern _C90C9B011121E670A95DB0CFD8646F31");
            context.Asm("extern _82A2D11E45ADDFD2AFAF15C2B0B66DFB");
            context.Asm("extern _C0F99D210869122397E9119A6525F919");
            context.Asm("extern _895EED2DB3B152639CA74BFD32090CA8");
            context.Asm("extern _B1437D3A3811567E68E42B4C936884D3");
            context.Asm("extern _824A1F21C8BF8B473BD2718A24E19467");
            context.Asm("extern _F6C910BB9661F7C73F659FA7555BF805");
            context.Asm("extern _06503F685ABE5A9A77C932F5BCBA1EB5");
            context.Asm("extern _D298A2D4E5657EEA7EB17452D6F2E617");
            context.Asm("extern _0519DBB93E219BC38CA0EA97FA3F7735");
            context.Asm("extern _A8E19AA8CAD25F13836DC967107BF5E1");
            context.Asm("extern _0068756F97648B1BD3982A80BE6B637C");
            context.Asm("extern _12DB08848180744E59A76B7AC4068D25");
            context.Asm("extern _9FDF3242FCB51E0F4FDEE7487ABCD2D9");
            context.Asm("extern _71565D88FA0B0839698FCAD8CFA49FCE");
            context.Asm("extern _BE6A5B585E77DC19CD2D96A4D32CBBE3");
            context.Asm("extern _E4910B1D8FA2DD7EF9141D6AD4F9A2A3");
            context.Asm("extern _EC2DCD7CA533B263D96736F1FA0EFD77");
            context.Asm("extern _4117CFF8D48D289B1BA88743B2A387E1");
            context.Asm("extern _9A40F757A64B441EEAE1E4F27A634412");
            context.Asm("extern _72E6006D3F84380F9874EDDC378FD0D4");
            context.Asm("extern _5D9DD1B69A761CF46A2B56AA4F3F8746");
            context.Asm("extern _C564E70E69C96050C36C7C021C5F3884");
            context.Asm("extern _6C0EB5335A22AD8A2D73C656610D7512");
            context.Asm("extern _2DADF0725911C3D2A9971653F781E74B");
            context.Asm("extern _A1B50173998F9101ACCCDADC83142A77");
            context.Asm("extern _0E7A761469DD4A340DFAFA47A84C312C");
            context.Asm("extern _F0B4BF4803A02357ABCAEE06F52FF195");
            context.Asm("extern _38C16E7E545AF70C2BD394833111F831");
            context.Asm("extern _229AA65FCE166B248E8ED157A3B0FF0A");
            /*   context.Asm(
                   "global start\nstart:\n  mov [_FAAF0DE554FAF3091BE39267E23BF3B8], dword start_of_heap\n mov esp, sys_stack     ; This points the stack to our new stack area\n    jmp stublet\n\n; This part MUST be 4byte aligned, so we solve that issue using 'ALIGN 4'\nALIGN 4\nmboot:\n    ; Multiboot macros to make a few lines later more readable\n    MULTIBOOT_PAGE_ALIGN    equ 1<<0\n    MULTIBOOT_MEMORY_INFO   equ 1<<1\n    MULTIBOOT_AOUT_KLUDGE   equ 1<<16\n    MULTIBOOT_HEADER_MAGIC  equ 0x1BADB002\n    MULTIBOOT_HEADER_FLAGS  equ MULTIBOOT_PAGE_ALIGN | MULTIBOOT_MEMORY_INFO | MULTIBOOT_AOUT_KLUDGE\n    MULTIBOOT_CHECKSUM  equ -(MULTIBOOT_HEADER_MAGIC + MULTIBOOT_HEADER_FLAGS)\n    EXTERN code, bss, end\n\n    ; This is the GRUB Multiboot header. A boot signature\n    dd MULTIBOOT_HEADER_MAGIC\n    dd MULTIBOOT_HEADER_FLAGS\n    dd MULTIBOOT_CHECKSUM\n    \n    ; AOUT kludge - must be physical addresses. Make a note of these:\n    ; The linker script fills in the data for these ones!\n    dd mboot\n    dd code\n    dd bss\n    dd end\n    dd start\n\n; This is an endless loop here. Make a note of this: Later on, we\n; will insert an 'extern _main', followed by 'call _main', right\n; before the 'jmp $'.\nstublet:\n    extern kmain\n    call _E5718F1DA4621EC9DDB27C7EF95C7BAE\n    jmp $");
   
   */
            Logger.Log($"Static Emmit: Kernel");
            EmmitStatic(context.RuntimeKernel, context);

            Logger.Log($"Static Emmit: UserSpace");
            EmmitStatic(context.UserSpace, context);

            //must be at top to prevent section overlaping
            context.Comment();
            context.Asm("section .data");
            context.Comment();


            EmmitGlobals(context.RuntimeKernel, context);
            EmmitGlobals(context.UserSpace, context);

            foreach (var gl in context.GlobalsExtra)
            {
                context.Asm($"global {gl.Key}");
                context.Asm($"{gl.Key}: db {gl.Value}");
            }

            /*    context.Asm("SECTION .bss\n    resb 8192               ; This reserves 8KBytes of memory here\nsys_stack:");
                context.Asm(
                    "SECTION .heap\n start_of_heap: \n    resb 8192               ; This reserves 8KBytes of memory here");*/
        }

        private void EmmitGlobals(IRAssembly assembly, UskrContext context)
        {
            foreach (var member in assembly.Members)
            {
                if (member.IsField && member.Static)
                {
                    context.Asm($"global _{Utils.MD5(member.Namespace)}");

                    var s = "";

                    if (member.InitValue == null)
                    {
                        context.Asm($"_{Utils.MD5(member.Namespace)}: db 0,0,0,0 ;{member.Namespace}");
                    }
                    else
                    {
                        foreach (var val in member.InitValue)
                        {
                            s += val + ",";
                        }

                        context.Asm($"{Utils.MD5(member.Namespace)} db {s.Trim().TrimEnd(',')}");
                    }
                }
            }
        }

        private void EmmitStatic(IRAssembly assembly, UskrContext context)
        {
            context.Comment();
            foreach (var method in assembly.Methods)
            {
                context.Comment(method.Identifier);
                context.Comment($"{method.Namespace}: ");


                if (method.CCall)
                {
                    context.Asm($"Global _{method.Identifier}");
                    context.Asm($"_{method.Identifier}: ");
                }
                
                context.Asm($"Global _{Utils.MD5(method.Namespace)}");
                context.Asm($"_{Utils.MD5(method.Namespace)}: ");

                context.Asm("push ebp");
                context.Asm("mov ebp,esp");
                context.Asm(
                    $"sub esp, {16 + (method.Body.Variables.Count * 4 > 16 ? (method.Body.Variables.Count * 4) - 16 : 0)}");

                //vmt logic
                if (method.IsVirtual)
                {
                    context.Asm($"cmp ebx, 0");
                    context.Asm($"je _{Utils.MD5(method.Namespace)}_start");

                    for (var i = 0; i < context.VirtualTypes.Count; i++)
                    {
                        var type = context.VirtualTypes[i];
                        context.Comment(type.Name);
                        context.Comment();
                        if (type.BaseType.Name == method.BaseType)
                        {
                            context.Asm($"mov eax, [ebp+8]");
                            context.Asm($"mov ebx, [eax]");
                            context.Asm($"cmp ebx, {i}");
                            context.Asm($"jne _{Utils.MD5(method.Namespace)}_{type.Name}_false");
                            context.Asm($"_{Utils.MD5(method.Namespace)}_{type.Name}_true: ");

                            foreach (var def in type.Methods)
                            {
                                if (def.Name == method.Identifier)
                                {
                                    context.Comment(def.FullName);
                                    context.Asm($"mov ebx, 0");
                                    context.Asm($"call _{Utils.MD5(def.FullName)}");
                                    break;
                                }
                            }

                            context.Asm($"jmp _{Utils.MD5(method.Namespace)}_exit");
                            context.Asm($"_{Utils.MD5(method.Namespace)}_{type.Name}_false: ");
                        }
                    }


                    context.Asm($"jmp _{Utils.MD5(method.Namespace)}_exit");

                    context.Asm($"_{Utils.MD5(method.Namespace)}_start: ");
                }

                EmmitStaticIL(assembly, method, context);

                if (method.IsVirtual)
                {
                    context.Asm($"_{Utils.MD5(method.Namespace)}_exit: ");
                    context.Asm($"leave");
                    context.Asm($"ret");
                }

                context.Comment();
            }
        }

        private void EmmitStaticIL(IRAssembly assembly, IRMethod meth, UskrContext context)
        {
            foreach (var instruction in meth.Body.Instructions)
            {
                context.Comment();
                context.Comment(instruction.ToString());

                context.Asm($"_{Utils.MD5(meth.Namespace)}_{instruction.Offset}: ");

                if (Handlers.ContainsKey(instruction.OpCode.Code))
                {
                    Handlers[instruction.OpCode.Code].Handel(assembly, meth, context, instruction);
                }
                else
                {
                    Logger.Error($"Missing Opcode Handler: {instruction}");
                }
            }
        }
    }
}