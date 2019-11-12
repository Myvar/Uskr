using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Stelem_I)]
    public class Stelem_I : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            context.Asm($"pop ebx"); //value
            context.Asm($"pop eax"); //index
            context.Asm($"pop ecx"); //array
            context.Asm($"add ecx, 4"); //array

            context.Asm($"mov edx, 4"); //array

            context.Asm($"mul edx"); //array

            context.Asm($"add ecx, eax"); //array
            context.Asm($"mov [ecx], ebx"); //array
        }
    }
}