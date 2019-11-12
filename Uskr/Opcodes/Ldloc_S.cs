using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ldloc_S)]
    public class Ldloc_S : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            context.Asm($"mov eax, [ebp-{4 + (4 * (instruction.Operand as Local).Index)}]");
            context.Asm("push eax");

        }
    }
}