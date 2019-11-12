using Uskr.Attributes;

namespace Uskr.Runtime
{
    [RK]
    public unsafe static class KSerial
    {
        public static int Port;

        public static void Serial_Init()
        {
            KIO.outpb((ushort) (Port + 1), 0x00);
            KIO.outpb((ushort) (Port + 3), 0x80);
            KIO.outpb((ushort) (Port + 0), 0x03);
            KIO.outpb((ushort) (Port + 1), 0x00);
            KIO.outpb((ushort) (Port + 3), 0x03);
            KIO.outpb((ushort) (Port + 2), 0xC7);
            KIO.outpb((ushort) (Port + 4), 0x0B);
        }

        public static void Serial_SetPort(int p)
        {
            Port = p;
        }

        public static int Serial_Received()
        {
            return KIO.inportb((short) (Port + 5)) & 1;
        }

        public static byte Read_Serial()
        {
            while (Serial_Received() == 0) ;

            return KIO.inportb((short) Port);
        }

        public static int is_transmit_empty()
        {
            return KIO.inportb((short) (Port + 5)) & 0x20;
        }

        public static void Write_Serial(byte a)
        {
             while (is_transmit_empty() == 0) ;

            KIO.outpb((ushort) Port, a);
        }

        public static void Write_Serial_Str(string a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                Write_Serial((byte) a[i]);
            }
        }
    }
}