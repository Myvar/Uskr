using System;
using Uskr.Attributes;

namespace Uskr.Runtime
{
    [RK]
    public static class KIrq
    {
        [CCall]
        public static void KeyboardIrq(char c)
        {
            KConsole.LogReadLine((byte) c);
        }
    }
}