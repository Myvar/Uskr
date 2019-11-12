using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Add)]
    public class Add : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            context.Asm("pop eax");
            context.Asm("pop ebx");
            context.Asm("add ebx, eax");
            context.Asm("push ebx");
        }
    }
}