using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ldloca_S)]
    public class Ldloca_S : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
                                    context.Asm($"mov eax, ebp");
                        context.Asm($"sub eax, {4 * ((instruction.Operand as Local).Index + 1)}");
                        context.Asm($"push eax");


        }
    }
}