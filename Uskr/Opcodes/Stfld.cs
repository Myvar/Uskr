using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Stfld)]
    public class Stfld : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            context.Asm($"pop eax"); // value
            context.Asm($"pop ebx"); // object pointer

            if (instruction.Operand is FieldDef fd)
            {
                context.Asm($"add ebx, {4 + fd.DeclaringType.Fields.IndexOf(fd) * 4}");
                context.Asm($"mov [ebx], eax");
            }
        }
    }
}