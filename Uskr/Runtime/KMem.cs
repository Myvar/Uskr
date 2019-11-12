using System;
using Uskr.Attributes;

namespace Uskr.Runtime
{
    [RK]
    public static unsafe class KMem
    {
        //for now we will make a sudo fake mem manager to test oop shit
        private static byte* _raw;
        private static int _offset;

        public static void Init()
        {
        }

        public static void* Kmalloc(int size)
        {
            var x = _raw + _offset;
            _offset += size;
            return x;
        }

        [Plug(
            "System.Void System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray(System.Array,System.RuntimeFieldHandle)")]
        public static void InitArray(int a, int b)
        {
            // Console.WriteLine("InitializeArray");
        }

        public static void* Memcpy(void* dest, void* src, int count)
        {
            var sp = (byte*) src;
            var dp = (byte*) dest;
            for (int i = 0; i < count; i++)
            {
                dp[i] = sp[i];
            }

            return dest;
        }

        public static void* Memset(void* dest, byte val, int count)
        {
            var temp = (byte*) dest;

            for (int i = 0; i < count; i++)
            {
                temp[i] = val;
            }

            return dest;
        }

        public static ushort* Memsetw(ushort* dest, ushort val, int count)
        {
            var temp = (ushort*) dest;
            for (int i = 0; i < count; i++)
            {
                temp[i] = val;
            }

            return dest;
        }
    }
}