using Uskr.Attributes;

namespace Uskr.Runtime
{
    [RK]
    public static unsafe class KString
    {
        [Plug("System.Char System.String::get_Chars(System.Int32)")]
        public static byte GetChar(byte* ptr, byte offset)
        {
            return ptr[offset + 4];
        }

        [Plug("System.Int32 System.String::get_Length()")]
        public static int Length(int* str)
        {
            return *str;
        }
    }
}