using System.Text;

namespace Uskr.IR
{
    public abstract class IntermediateRepresentation
    {
        public bool Static { get; set; }
        public string Class { get; set; }
        public string Namespace { get; set; }
    }
}