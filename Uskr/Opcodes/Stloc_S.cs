using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Stloc_S)]
    public class Stloc_S : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            context.Asm("pop eax");
            context.Asm($"mov [ebp-{4 + 4 * ((instruction.Operand as Local).Index)}], eax");
        }
    }
}