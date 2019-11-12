using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ret)]
    public class Ret : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
                                    if (meth.IsFunc)
                        {
                            context.Asm("pop eax");
                        }

                        context.Asm("leave");
                        context.Asm("ret");

        }
    }
}