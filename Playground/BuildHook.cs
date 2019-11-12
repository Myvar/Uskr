using System.Text;
using Uskr.Attributes;
using Uskr.Core;
using Uskr.Enums;
using Uskr.IR;

namespace Playground
{
    //@Uncompleted
    [UskrIgnore]
    [Eval(EvalConstraint.IRBuilder)]
    public class BuildHook : IRuntimeExecution
    {
        /// <summary>
        /// This will execute at compile time allowing the compiler to imbed/change things on the host operating system(Linux,Win)
        /// </summary>
        /// <param name="assembly"></param>
        public void Fire(ref IRAssembly assembly)
        {
            assembly.EmbeddedResources.Add(new IREmbedded()
            {
                Data = Encoding.ASCII.GetBytes("Bob the builder"),
                Namespace = "foo"
            });
        }
    }
}