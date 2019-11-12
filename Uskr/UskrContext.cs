using System.Collections.Generic;
using System.Reflection;
using System.Text;
using dnlib.DotNet;
using Uskr.IR;

namespace Uskr
{
    public class UskrContext
    {
        public IRAssembly RuntimeKernel { get; set; }
        public IRAssembly UserSpace { get; set; }
        public Assembly UserAssembly { get; set; }

        public StringBuilder Nasm { get; set; } = new StringBuilder();

        public Dictionary<string, string> GlobalsExtra { get; set; } = new Dictionary<string, string>();
        public List<TypeDef> VirtualTypes { get; set; } = new List<TypeDef>();

        public void Comment(string s)
        {
            Nasm.AppendLine("; " + s);
        }

        public void Comment()
        {
            Nasm.AppendLine();
        }

        public void Asm(string s)
        {
            Nasm.AppendLine(s);
        }
    }
}