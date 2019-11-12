using System;
using Uskr.Attributes;

namespace Uskr.Runtime
{
    [RK]
    public static class RKernel
    {
        public static void Init()
        {
            KMem.Init(); //then we init kMem
            
            KConsole.Init(); //First console so Kmem can print debug messages
            //Now init the rest
            KInt.Init();
        }

        [CCall]
        public static void Panic()
        {
            Console.WriteLine("kErNeL pAnIc");
            while (true)
            {
            }
        }
    }
}