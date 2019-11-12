using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ldarg_1)]
    public class Ldarg_1 : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
                                    context.Asm($"mov eax, [ebp+{(meth.ParamsCount * 4) - (4 * 1) + 4}]");
                        context.Asm("push eax");

        }
    }
}