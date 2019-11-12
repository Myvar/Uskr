using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Callvirt)]
    public class Callvirt : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            
            //fml time for vmt shit crap
            context.Asm("mov ebx, 1");
            context.Asm("call _" + Utils.MD5(instruction.Operand.ToString()));

            if (instruction.Operand is MemberRef mr)
            {
                var argsc = mr.FullName.Split('(').Last().TrimEnd(')').Split(',').Length;
                var size = (argsc * 4) ;


                context.Asm("add esp, " + size);
                if (!mr.ReturnType.FullName.Contains("Void"))
                {
                    context.Asm("push eax");
                }
            }
            else if (instruction.Operand is MethodDef md)
            {
                int size = !md.IsStatic ? 4 : 0; // non static always get the 'this' value are arg 0

                foreach (var parameter in md.Parameters)
                {
                    size += 4;
                    // context.Asm("pop ebx");
                }

                context.Asm("add esp, " + size);
                if (!md.ReturnType.FullName.Contains("Void"))
                {
                    context.Asm("push eax");
                }
            }
        }
    }
}