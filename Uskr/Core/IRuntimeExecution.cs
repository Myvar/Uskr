using Uskr.IR;

namespace Uskr.Core
{
    public interface IRuntimeExecution
    {
        void Fire(ref IRAssembly assembly);
    }
}