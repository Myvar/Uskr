using System;
using dnlib.DotNet.Emit;

namespace Uskr.Attributes
{
    public class HandlerAttribute : Attribute
    {
        public HandlerAttribute(Code opcode)
        {
            Opcode = opcode;
        }

        public Code Opcode { get; set; }
    }
}