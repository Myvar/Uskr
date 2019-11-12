using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Shr_Un)]
    public class Shr_Un : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
                                    context.Asm("pop ecx"); //bits
                        context.Asm("pop ebx"); //val
                        context.Asm($"shr ebx, cl");
                        context.Asm("push ebx");

        }
    }
}