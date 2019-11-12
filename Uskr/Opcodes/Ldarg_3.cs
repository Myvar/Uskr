using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ldarg_3)]
    public class Ldarg_3 : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
                                    context.Asm($"mov eax, [ebp+{(meth.ParamsCount * 4) - (4 * 3) + 4}]");
                        context.Asm("push eax");

        }
    }
}