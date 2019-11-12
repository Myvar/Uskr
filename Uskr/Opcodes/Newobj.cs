using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.IR;

namespace Uskr.Opcodes
{
    [Handler(Code.Newobj)]
    public class Newobj : IOpcodeProcessor
    {
        public void Handel(IRAssembly assembly, IRMethod meth, UskrContext context, Instruction instruction)
        {
            if (instruction.Operand is MethodDef md)
            {
                context.Asm($"push {4 + CalcMaxFields(md.DeclaringType) * 4}");
                context.Asm($"call _1EC80A85A7C365C7432628F0BD1DC116 ; call to kmalloc");
                context.Asm($"add esp, 4");
                context.Asm($"mov [eax], dword {context.VirtualTypes.IndexOf(md.DeclaringType)}"); //store in instance number
                context.Asm($"push eax");

                context.Asm($"call _{Utils.MD5(md.FullName)}");
                //dont add to esp here because we need to dup it any way`
            }
        }

        private int CalcMaxFields(TypeDef td)
        {
            if (td.BaseType.Name.ToLower().Contains("object")) return td.Fields.Count;
            return td.Fields.Count + CalcMaxFields(td.BaseType as TypeDef);
        }
    }
}