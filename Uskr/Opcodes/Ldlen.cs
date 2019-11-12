using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ldlen)]
    public class Ldlen : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            context.Asm("pop eax"); //the array
            context.Asm("mov ebx, [eax]");
            context.Asm("push ebx");
            
        }
    }
}