using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ceq)]
    public class Ceq : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
                                    context.Asm("pop eax");
                        context.Asm("pop ebx");
                        context.Asm($"cmp eax, ebx");
                        context.Asm(
                            $"je _{Utils.MD5(meth.Namespace)}_{instruction.Offset}_f");
                        context.Asm("push 0");
                        context.Asm($"jmp _{Utils.MD5(meth.Namespace)}_{instruction.Offset}_t");
                        context.Asm(
                            $"_{Utils.MD5(meth.Namespace)}_{instruction.Offset}_f:");
                        context.Asm("push 1");
                        context.Asm(
                            $"_{Utils.MD5(meth.Namespace)}_{instruction.Offset}_t:");

        }
    }
}