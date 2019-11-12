using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Arglist)]
    public class Arglist : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
                                    Logger.Debug($"Not Implemented: {instruction.OpCode.Code}");
                        //@Incomplete need to create an embeded resource that stores this information to return

        }
    }
}