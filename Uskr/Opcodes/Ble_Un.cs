using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ble_Un)]
    public class Ble_Un : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
                                    context.Asm("pop eax");
                        context.Asm("pop ebx");
                        context.Asm($"cmp eax, ebx");
                        context.Asm(
                            $"jle _{Utils.MD5(meth.Namespace)}_{(instruction.Operand as Instruction).Offset}");

        }
    }
}