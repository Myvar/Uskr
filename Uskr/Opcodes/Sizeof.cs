using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Sizeof)]
    public class Sizeof : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {

            long size = 0;

            if (instruction.Operand is TypeDef td)
            {
                foreach (var field in td.Fields)
                {
                    size += field.GetFieldSize();
                }
            }
            
            
            context.Asm($"push {size}");
        }
    }
}