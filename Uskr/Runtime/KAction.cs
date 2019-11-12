using System;
using Uskr.Attributes;

namespace Uskr.Runtime
{
    [RK]
    public unsafe static class KAction
    {
        [UskrIgnore]
        public static ulong GetFuncPtr(Action a)
        {
            return 0;
        }

        [Plug("System.UInt64 Uskr.Runtime.KAction::GetFuncPtr(System.Action)")]
        public static void* GetFuncPtrPlug(void* ptr)
        {
            return ptr;
        }
    }
}