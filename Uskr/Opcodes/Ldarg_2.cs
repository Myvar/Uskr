using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ldarg_2)]
    public class Ldarg_2 : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
                                    context.Asm($"mov eax, [ebp+{(meth.ParamsCount * 4) - (4 * 2) + 4}]");
                        context.Asm("push eax");

        }
    }
}