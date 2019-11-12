using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ldelema)]
    public class Ldelema : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            context.Asm($"pop eax"); // the array
            context.Asm($"pop ebx"); // the index
            context.Asm($"add eax, ebx"); // the index
            context.Asm($"push eax"); // the index
        }
    }
}