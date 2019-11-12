using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Ldarg)]
    public class Ldarg : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            // context.Asm($"mov eax, [ebp+{instruction.}]");
            //     context.Asm("push eax");
            throw new Exception(); //@need an test case to see wtf
        }
    }
}