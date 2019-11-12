using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Add_Ovf)]
    public class Add_Ovf : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            //@Incomplete need to do overflow cheack
            context.Asm("pop eax");
            context.Asm("pop ebx");
            context.Asm("add ebx, eax");
            context.Asm("push ebx");
        }
    }
}