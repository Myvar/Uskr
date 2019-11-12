using System.Collections.Generic;

namespace Uskr.IR
{
    public class IRAssembly : IntermediateRepresentation
    {
        public List<IRMethod> Methods { get; set; } = new List<IRMethod>();
        public List<IRMember> Members { get; set; } = new List<IRMember>();
        public List<IREmbedded> EmbeddedResources { get; set; } = new List<IREmbedded>();
    }
}