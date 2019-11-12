using System.Diagnostics;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ldc_I4)]
    public class Ldc_I4 : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            var nexti = meth.Body.Instructions.IndexOf(instruction);
            var next = meth.Body.Instructions[nexti + 1];
            var val = instruction.GetLdcI4Value();
            switch (next.OpCode.Code)
            {
                case Code.Conv_U:
                    context.Asm($"push {(uint) val}");
                    break;
                default:
                    context.Asm($"push {val}");
                    break;
            }
        }
    }
}