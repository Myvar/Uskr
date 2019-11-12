using dnlib.DotNet.Emit;

namespace Uskr.IR
{
    /// <summary>
    /// Method or Func
    /// </summary>
    public class IRMethod : IntermediateRepresentation
    {
        public string Identifier { get; set; }
        public string BaseType { get; set; }
        public CilBody Body { get; set; }
        public bool IsFunc { get; set; }
        public bool IsStatic { get; set; }
        public bool IsVirtual { get; set; }
        public int ParamsCount { get; set; }
        public bool CCall { get; set; }
    }
}