using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Cpobj)]
    public class Cpobj : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
                                    //@NotStaticPipe

                        Logger.Debug($"Not Implemented: {instruction.OpCode.Code}");

        }
    }
}