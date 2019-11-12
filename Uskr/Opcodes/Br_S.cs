using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Br_S)]
    public class Br_S : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
                                    context.Asm(
                            $"jmp _{Utils.MD5(meth.Namespace)}_{(instruction.Operand as Instruction).Offset}");

        }
    }
}