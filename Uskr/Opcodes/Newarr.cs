using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Newarr)]
    public class Newarr : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            context.Asm($"pop edi"); // count of elements
            context.Asm($"mov eax, edi"); // count of elements
          

            context.Asm("mov edx, 0");
            context.Asm($"mov ecx, 4");
            context.Asm("mul ecx");
            context.Asm($"add eax, 4"); // add padding of 4 to store leng
            context.Asm("push eax");

            
            context.Asm($"call _1EC80A85A7C365C7432628F0BD1DC116");
            context.Asm($"sub esp, 4");
            context.Asm($"mov [eax], edi");
            context.Asm($"push eax");
        }
    }
}