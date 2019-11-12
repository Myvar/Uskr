using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ldsflda)]
    public class Ldsflda : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
                                    context.Asm($"mov eax, _{Utils.MD5((instruction.Operand as FieldDef).FullName)}");
                        context.Asm($"push eax");

        }
    }
}