using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Brtrue_S)]
    public class Brtrue_S : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
                                    context.Asm("pop eax");
                        context.Asm("mov ebx, 1");
                        context.Asm($"cmp eax, ebx");
                        context.Asm(
                            $"je _{Utils.MD5(meth.Namespace)}_{(instruction.Operand as Instruction).Offset}");

        }
    }
}