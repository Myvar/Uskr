using System;
using System.IO;
using System.Net.Http.Headers;
using Uskr.Attributes;

namespace Uskr.Runtime
{
    [RK]
    public unsafe static class KConsole
    {
        private static byte* TerminalLfb;
        private static int X;
        private static int Y;

        private static int MaxWidth;
        private static int MaxHeight;

        private static int CursorStart;
        private static int CursorEnd;
        private static int CursorOffsetOld;

        private static byte ColorBack;
        private static byte ColorFront;
        private static bool CursorEnabled;
        private static byte* Buf;

        public static string ItohMap;

        public static void Init()
        {
            ItohMap = "0123456789ABCDEF";
            TerminalLfb = (byte*) 0xB8000;
            X = 0;
            Y = 0;
            MaxWidth = 80;
            MaxHeight = 25;
            ColorBack = 0x0;
            ColorFront = 0xF;
            CursorEnabled = true;
            CursorOffsetOld = 0;

            KSerial.Serial_SetPort(0x3F8);
            KSerial.Serial_Init();
            KSerial.Write_Serial_Str("Started Serial Log");
            KSerial.Write_Serial_Str("\n");

            Console.CursorVisible = false;

            KIO.outpb(0x3D4, 0x0F);
            KIO.outpb(0x3D5, 0);
            KIO.outpb(0x3D4, 0x0E);
            KIO.outpb(0x3D5, 0);

            // KIO.outpb(0x3d4, 0xa);
            //  KIO.outpb(0x3d5, 0x0);

            Buf = (byte*) KMem.Kmalloc(255);
        }

        public static void LogReadLine(byte c)
        {
            Console.Write((char) c);

            Buf[*((int*) Buf)] = c;
            *((int*) Buf) += 1;
        }

        private static void UpdateCursor()
        {
            // TerminalLfb[CursorOffsetOld + 1] = 0x0F;
            // if (!CursorEnabled) return;


            /*var pos = (((y) * MaxWidth) + (x));

            KIO.outpb(0x3D4, 0x0F);
            KIO.outpb(0x3D5, (byte) (pos & 0xFF));
            KIO.outpb(0x3D4, 0x0E);
            KIO.outpb(0x3D5, (byte) ((pos >> 8) & 0xFF));*/


            // var offset = ((y * MaxWidth) + x) * 2;
            // TerminalLfb[offset + 1] = 0xF0;
            // CursorOffsetOld = offset;
        }

        [Plug("System.Void System.Console::WriteLine(System.String)")]
        private static void WriteLine(string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                Console.Write(value[i]);
            }

            Console.Write('\n');
        }

/*

        [Plug("System.Void System.Console::WriteLine(System.String,System.Object)")]
        private static void WriteLine(string format, object arg0)
        {
        }

        [Plug("System.Void System.Console::WriteLine(System.String,System.Object,System.Object)")]
        private static void WriteLine(string format, object arg0, object arg1)
        {
        }

        [Plug("System.Void System.Console::WriteLine(System.String,System.Object,System.Object,System.Object)")]
        private static void WriteLine(string format, object arg0, object arg1, object arg2)
        {
        }

        [Plug("System.Void System.Console::WriteLine(System.String,System.Object[])")]
        private static void WriteLine(string format, object[] arg)
        {
        }

        [Plug("System.Void System.Console::Write(System.String,System.Object)")]
        private static void Write(string format, object arg0)
        {
        }

        [Plug("System.Void System.Console::Write(System.String,System.Object,System.Object)")]
        private static void Write(string format, object arg0, object arg1)
        {
        }

        [Plug("System.Void System.Console::Write(System.String,System.Object,System.Object,System.Object)")]
        private static void Write(string format, object arg0, object arg1, object arg2)
        {
        }

        [Plug("System.Void System.Console::Write(System.String,System.Object[])")]
        private static void Write(string format, object[] arg)
        {
        }
        
*/
        [Plug("System.Void System.Console::Write(System.Boolean)")]
        private static void Write(bool value)
        {
            if (value)
            {
                Console.Write("True");
            }
            else
            {
                Console.Write("False");
            }
        }

        private static void ScrollUp()
        {
            for (int y = 1; y < 20; y++)
            {
                for (int x = 0; x < MaxWidth; x++)
                {
                    var offsetPrev = (((y - 1) * MaxWidth) + x) * 2;
                    var offset = ((y * MaxWidth) + x) * 2;

                    TerminalLfb[offsetPrev] = TerminalLfb[offset];
                }
            }

            for (int x = 0; x < MaxWidth; x++)
            {
                var offset = ((19 * MaxWidth) + x) * 2;
                TerminalLfb[offset] = 0;
            }
        }


        [Plug("System.Void System.Console::Write(System.Char)")]
        private static void Write(char value)
        {
            KSerial.Write_Serial((byte) value);

            if (value == '\n')
            {
                UpdateCursor();
                X = 0;
                Y++;

                if (Y >= 20)
                {
                    Y--;
                    ScrollUp();
                }

                return;
            }

            if (value == '\t')
            {
                X += 4;
            }

            if (value == '\b')
            {
                var bOffset = ((Y * MaxWidth) + X - 1) * 2;
                TerminalLfb[bOffset] = (byte) ' ';
                TerminalLfb[bOffset + 1] = (byte) (ColorFront | ColorBack << 8);
                ;

                X--;
            }


            var offset = ((Y * MaxWidth) + X) * 2;
            TerminalLfb[offset] = (byte) value;
            TerminalLfb[offset + 1] = (byte) (ColorFront | ColorBack << 8);

            X++;
            if (X > MaxWidth)
            {
                X = 0;
                Y++;

                if (Y >= 20)
                {
                    Y--;
                    ScrollUp();
                }
            }
        }


        [Plug("System.Void System.Console::Write(System.Char[])")]
        private static void Write(char[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                Console.Write(buffer[i]);
            }
        }


        [Plug("System.Void System.Console::Write(System.Char[],System.Int32,System.Int32)")]
        private static void Write(char[] buffer, int index, int count)
        {
            for (int i = index; i < count; i++)
            {
                Console.Write(buffer[i]);
            }
        }


/*
        [Plug("System.Void System.Console::Write(System.Double)")]
        private static void Write(double value)
        {
        }

        [Plug("System.Void System.Console::Write(System.Decimal)")]
        private static void Write(decimal value)
        {
        }

        [Plug("System.Void System.Console::Write(System.Single)")]
        private static void Write(short value)
        {
        }
*/
        [Plug("System.Void System.Console::Write(System.Int32)")]
        private static void Write(int value)
        {
            int n;
            int b;

            int s = 8;
            for (n = (s - 1);
                n > -1;
                --n)
            {
                b = (value >> (n * 4)) & 0xf;

                Console.Write(ItohMap[b]);
            }
        }

        [Plug("System.Void System.Console::Write(System.UInt32)")]
        private static void Write(uint value)
        {
            int n;
            int b;

            int s = 8;
            for (n = (s - 1);
                n > -1;
                --n)
            {
                b = (int) ((value >> (n * 4)) & 0xf);

                Console.Write(ItohMap[b]);
            }
        }

        [Plug("System.Void System.Console::Write(System.Int64)")]
        private static void Write(long value)
        {
            int n;
            int b;

            int s = 8;
            for (n = (s - 1);
                n > -1;
                --n)
            {
                b = (int) ((value >> (n * 4)) & 0xf);

                Console.Write(ItohMap[b]);
            }
        }

        [Plug("System.Void System.Console::Write(System.UInt64)")]
        private static void Write(ulong value)
        {
            int n;
            int b;

            int s = 8;
            for (n = (s - 1);
                n > -1;
                --n)
            {
                b = (int) ((value >> (n * 4)) & 0xf);

                Console.Write(ItohMap[b]);
            }
        }


        [Plug("System.Void System.Console::Write(System.Object)")]
        private static void Write(object value)
        {
            Console.Write(value.ToString());
        }

        [Plug("System.Void System.Console::Write(System.String)")]
        private static void Write(string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                Console.Write(value[i]);
            }
        }


/*
        [Plug("System.Boolean System.Console::get_KeyAvailable()")]
        private static bool get_KeyAvailable()
        {
        }

        [Plug("System.ConsoleKeyInfo System.Console::ReadKey()")]
        private static consolekeyinfo ReadKey()
        {
        }

        [Plug("System.ConsoleKeyInfo System.Console::ReadKey(System.Boolean)")]
        private static consolekeyinfo ReadKey(bool intercept)
        {
        }*/


        /*
        [Plug("System.Int32 System.Console::get_CursorSize()")]
        private static int get_CursorSize()
        {
            return -1;
        }

        [Plug("System.Void System.Console::set_CursorSize(System.Int32)")]
        private static void set_CursorSize(int value)
        {
        }
*/

        [Plug("System.ConsoleColor System.Console::get_BackgroundColor()")]
        private static ConsoleColor get_BackgroundColor()
        {
            return (ConsoleColor) (ColorBack);
        }

        [Plug("System.Void System.Console::set_BackgroundColor(System.ConsoleColor)")]
        private static void set_BackgroundColor(ConsoleColor value)
        {
            ColorBack = (byte) value;
        }

        [Plug("System.ConsoleColor System.Console::get_ForegroundColor()")]
        private static ConsoleColor get_ForegroundColor()
        {
            return (ConsoleColor) ColorFront;
        }

        [Plug("System.Void System.Console::set_ForegroundColor(System.ConsoleColor)")]
        private static void set_ForegroundColor(ConsoleColor value)
        {
            ColorFront = (byte) value;
        }

        [Plug("System.Void System.Console::ResetColor()")]
        private static void ResetColor()
        {
            ColorBack = 0;
            ColorFront = 0xF;
        }

        [Plug("System.Int32 System.Console::get_BufferWidth()")]
        private static int get_BufferWidth()
        {
            return MaxWidth;
        }

        [Plug("System.Void System.Console::set_BufferWidth(System.Int32)")]
        private static void set_BufferWidth(int value)
        {
            //lol you wish dude
        }

        [Plug("System.Int32 System.Console::get_BufferHeight()")]
        private static int get_BufferHeight()
        {
            return MaxHeight;
        }

        [Plug("System.Void System.Console::set_BufferHeight(System.Int32)")]
        private static void set_BufferHeight(int value)
        {
            //lol you wish dude
        }

/*
        [Plug("System.Void System.Console::SetBufferSize(System.Int32,System.Int32)")]
        private static void SetBufferSize(int width, int height)
        {
        }

        [Plug("System.Int32 System.Console::get_WindowLeft()")]
        private static int get_WindowLeft()
        {
            return -1;
        }

        [Plug("System.Void System.Console::set_WindowLeft(System.Int32)")]
        private static void set_WindowLeft(int value)
        {
        }

        [Plug("System.Int32 System.Console::get_WindowTop()")]
        private static int get_WindowTop()
        {
            return -1;
        }

        [Plug("System.Void System.Console::set_WindowTop(System.Int32)")]
        private static void set_WindowTop(int value)
        {
        }
*/
        [Plug("System.Int32 System.Console::get_WindowWidth()")]
        private static int get_WindowWidth()
        {
            return MaxWidth;
        }

        [Plug("System.Void System.Console::set_WindowWidth(System.Int32)")]
        private static void set_WindowWidth(int value)
        {
            //lol you wish dude
        }

        [Plug("System.Int32 System.Console::get_WindowHeight()")]
        private static int get_WindowHeight()
        {
            return MaxHeight;
        }

        [Plug("System.Void System.Console::set_WindowHeight(System.Int32)")]
        private static void set_WindowHeight(int value)
        {
            //lol you wish dude
        }

        [Plug("System.Void System.Console::SetWindowPosition(System.Int32,System.Int32)")]
        private static void SetWindowPosition(int left, int top)
        {
        }

        [Plug("System.Void System.Console::SetWindowSize(System.Int32,System.Int32)")]
        private static void SetWindowSize(int width, int height)
        {
        }

        [Plug("System.Int32 System.Console::get_LargestWindowWidth()")]
        private static int get_LargestWindowWidth()
        {
            return MaxWidth;
        }

        [Plug("System.Int32 System.Console::get_LargestWindowHeight()")]
        private static int get_LargestWindowHeight()
        {
            return MaxHeight;
        }

        [Plug("System.Boolean System.Console::get_CursorVisible()")]
        private static bool get_CursorVisible()
        {
            return CursorEnabled;
        }

        [Plug("System.Void System.Console::set_CursorVisible(System.Boolean)")]
        private static void set_CursorVisible(bool value)
        {
            CursorEnabled = value;
            if (value)
            {
                KIO.outpb(0x3D4, 0x0A);
                KIO.outpb(0x3D5, (byte) ((KIO.inportb(0x3D5) & 0xC0) | CursorStart));

                KIO.outpb(0x3D4, 0x0B);
                KIO.outpb(0x3D5, (byte) ((KIO.inportb(0x3D5) & 0xE0) | CursorEnd));
            }
            else
            {
                KIO.outpb(0x3D4, 0x0A);
                KIO.outpb(0x3D5, 0x20);
            }
        }
/*
        [Plug("System.Int32 System.Console::get_CursorLeft()")]
        private static int get_CursorLeft()
        {
            return -1;
        }

        [Plug("System.Void System.Console::set_CursorLeft(System.Int32)")]
        private static void set_CursorLeft(int value)
        {
        }

        [Plug("System.Int32 System.Console::get_CursorTop()")]
        private static int get_CursorTop()
        {
            return -1;
        }

        [Plug("System.Void System.Console::set_CursorTop(System.Int32)")]
        private static void set_CursorTop(int value)
        {
        }
*/

        [Plug("System.Void System.Console::Beep()")]
        private static void Beep()
        {
        }

        [Plug("System.Void System.Console::Beep(System.Int32,System.Int32)")]
        private static void Beep(int frequency, int duration)
        {
        }


        [Plug("System.Void System.Console::Clear()")]
        private static void Clear()
        {
            Console.ResetColor();
            X = 0;
            Y = 0;
            UpdateCursor();
            for (int ax = 0; ax < MaxWidth; ax++)
            {
                for (int ay = 0; ay < MaxHeight; ay++)
                {
                    var offset = ((ay * MaxWidth) + ax) * 2;
                    TerminalLfb[offset] = (byte) ' ';
                    TerminalLfb[offset + 1] = (byte) (ColorFront | ColorBack << 8);
                }
            }
        }

        /*[Plug("System.Void System.Console::SetCursorPosition(System.Int32,System.Int32)")]
        private static void SetCursorPosition(int left, int top)
        {
        }


        [Plug("System.Int32 System.Console::Read()")]
        private static int Read()
        {
            return -1;
        }
*/
        [Plug("System.String System.Console::ReadLine()")]
        private static byte* ReadLine()
        {
            *((int*) Buf) = 4;

            while (Buf[*((int*) Buf) - 1] != (byte) '\n')
            {
            }

            *((int*) Buf) -= 4;
            return Buf;
        }

        [Plug("System.Void System.Console::WriteLine()")]
        private static void WriteLine()
        {
            Console.Write('\n');
        }

        [Plug("System.Void System.Console::WriteLine(System.Boolean)")]
        private static void WriteLine(bool value)
        {
            if (value)
            {
                Console.WriteLine("True");
            }
            else
            {
                Console.WriteLine("False");
            }
        }


        [Plug("System.Void System.Console::WriteLine(System.Char)")]
        private static void WriteLine(char value)
        {
            Console.Write(value);
            Console.WriteLine();
        }

        [Plug("System.Void System.Console::WriteLine(System.Char[])")]
        private static void WriteLine(char[] buffer)
        {
            Console.Write(buffer);
            Console.WriteLine();
        }

        [Plug("System.Void System.Console::WriteLine(System.Char[],System.Int32,System.Int32)")]
        private static void WriteLine(char[] buffer, int index, int count)
        {
            Console.Write(buffer, index, count);
            Console.WriteLine();
        }

/*
        [Plug("System.Void System.Console::WriteLine(System.Decimal)")]
        private static void WriteLine(decimal value)
        {
        }

        [Plug("System.Void System.Console::WriteLine(System.Double)")]
        private static void WriteLine(double value)
        {
        }

        [Plug("System.Void System.Console::WriteLine(System.Single)")]
        private static void WriteLine(short value)
        {
        }
*/
        [Plug("System.Void System.Console::WriteLine(System.Int32)")]
        private static void WriteLine(int value)
        {
            Console.Write(value);
            Console.WriteLine();
        }

        [Plug("System.Void System.Console::WriteLine(System.UInt32)")]
        private static void WriteLine(uint value)
        {
            Console.Write(value);
            Console.WriteLine();
        }

        [Plug("System.Void System.Console::WriteLine(System.Int64)")]
        private static void WriteLine(long value)
        {
            Console.Write(value);
            Console.WriteLine();
        }

        [Plug("System.Void System.Console::WriteLine(System.UInt64)")]
        private static void WriteLine(ulong value)
        {
            Console.Write(value);
            Console.WriteLine();
        }

        [Plug("System.Void System.Console::WriteLine(System.Object)")]
        private static void WriteLine(object value)
        {
            Console.Write(value);
            Console.WriteLine();
        }
    }
}