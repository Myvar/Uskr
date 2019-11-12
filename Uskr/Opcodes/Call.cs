using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Call)]
    public class Call : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            context.Asm("mov ebx, 0");
            context.Asm("call _" + Utils.MD5(instruction.Operand.ToString()));


            if (instruction.Operand is MethodDef md)
            {
                int size = !md.IsStatic ? 4 : 0; // non static always get the 'this' value are arg 0

                foreach (var parameter in md.Parameters)
                {
                    size += 4;
                    // context.Asm("pop ebx");
                }

                context.Asm("add esp, " + size);
                if (!md.ReturnType.FullName.Contains("Void"))
                {
                    context.Asm("push eax");
                }
            }
            else if (instruction.Operand is MemberRef mr)
            {
                if (!mr.ReturnType.FullName.Contains("Void"))
                {
                    context.Asm("push eax");
                }
            }
        }
    }
}