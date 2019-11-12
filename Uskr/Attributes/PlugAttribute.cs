using System;

namespace Uskr.Attributes
{
    public class PlugAttribute : Attribute
    {
        public PlugAttribute(string overideName)
        {
            OverideName = overideName;
        }

        public string OverideName { get; set; }
    }
}