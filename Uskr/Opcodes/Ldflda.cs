using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ldflda)]
    public class Ldflda : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            context.Asm($"pop eax"); // object pointer

            if (instruction.Operand is FieldDef fd)
            {
                context.Asm($"add eax, {4 + fd.DeclaringType.Fields.IndexOf(fd) * 4}");
                context.Asm($"mov ebx, [eax]");
                context.Asm($"push ebx");
            }
        }
    }
}