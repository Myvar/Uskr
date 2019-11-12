namespace Uskr.IR
{
    /// <summary>
    /// Field or Propertie
    /// </summary>
    public class IRMember : IntermediateRepresentation
    {
        public bool IsField { get; set; }
        public string Identifier { get; set; }

        public uint Size { get; set; }

        public byte[] InitValue { get; set; }
    }
}