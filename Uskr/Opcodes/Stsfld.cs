using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Stsfld)]
    public class Stsfld : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            context.Asm($"pop eax");
            context.Asm($"mov [_{Utils.MD5((instruction.Operand as FieldDef).FullName)}], eax");
        }
    }
}