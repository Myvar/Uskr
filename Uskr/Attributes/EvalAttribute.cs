using System;
using Uskr.Enums;

namespace Uskr.Attributes
{
    public class EvalAttribute : Attribute
    {
        public EvalConstraint EvalConstraint { get; set; }

        public EvalAttribute(EvalConstraint evalConstraint)
        {
            EvalConstraint = evalConstraint;
        }
    }
}