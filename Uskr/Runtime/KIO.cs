using Uskr.Attributes;

namespace Uskr.Runtime
{
    [RK]
    public static class KIO
    {
        [UskrIgnore]
        public static void outpw(ushort _port, ushort _data)
        {
        }
        
        [UskrIgnore]
        public static void outpb(ushort _port, byte _data)
        {
        }

        [UskrIgnore]
        public static byte inportb(short _port)
        {
            return 0;
        }
    }
}