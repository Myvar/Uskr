using System;
using Uskr;
using Uskr.Attributes;
using Uskr.Runtime;

namespace Playground
{
    public unsafe class Program
    {
#if DEBUG
        [UskrIgnore]
        static Program() => UskrEngine.Run();
#endif

        public static void Main()
        {
            RKernel.Init(); // use build in kernel

            Console.Clear();
            Console.WriteLine("Booting ...");

          
            
            while (true)
            {
                Console.Write(">");
                var x = Console.ReadLine();
                Console.WriteLine(x);
            }


            /*
             * TODO:
             * 
             * Imbedded files
             * BuildHooks
             * HardDrive BuildHook (maby a sync folder)
             * Recursive VMT
             * Nuget Packages
             * Cross Platfrom stuff
             */

            /*//vmt test
            var a = new Dog();
            var b = new Cat();
            a.SayYourName("A"); //make sure vmt flags dont scew over reading instance fields
            b.SayYourName("B");

            a.Speak();
            b.Speak();
            var c = (Animal) b;
            c.Speak();
            
            Log("YEET !!!");*/

            /*
            //PANIC
            var x = 0;
            var y = 0 / x;
            Console.WriteLine(y);

            //screen test
            var screen = new VBEDriver();
            screen.VBESet(800, 600, 32);
            int oy = 10;
            int ox = 10;

            // var pixels = (uint*) 0xE0000000; //Virtual Box
            var pixels = (uint*) 0xFD000000; //qemu
            
            for (int i = 0; i < 800 * 600; i++)
            {
                pixels[i] = 0x00_FF_FF_FF;
            }*/
        }
    }
}