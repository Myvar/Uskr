using dnlib.DotNet.Emit;
using Uskr.IR;

namespace Uskr.Opcodes
{
    public interface IOpcodeProcessor
    {
        void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction);
    }
}