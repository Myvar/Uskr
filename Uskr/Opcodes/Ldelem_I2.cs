using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ldelem_I2)]
    public class Ldelem_I2 : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            context.Asm($"pop eax"); //index
            context.Asm($"pop ecx"); //array
            context.Asm($"add ecx, 4"); //array
            
            context.Asm($"mov edx, 4"); //array

            context.Asm($"mul edx"); 
            context.Asm($"add ecx, eax");
            context.Asm($"mov ebx, [ecx]"); //array
            context.Asm("push ebx");

        }
    }
}