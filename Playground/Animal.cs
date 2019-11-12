using System;

namespace Playground
{
    public class Animal
    {
        public string Name;

        public void SayYourName(string pre)
        {
            Console.WriteLine(pre);
            Console.WriteLine(Name);
        }

        public virtual void Speak()
        {
            Console.WriteLine("Generic animal sound");
        }
    }

    public class Dog : Animal
    {
        public Dog()
        {
            Name = "I am a Dog";
        }

        public override void Speak()
        {
            Console.WriteLine("Barking sounds");
            base.Speak();
        }
    }

    public class Cat : Animal
    {
        public Cat()
        {
            Name = "I am a Cat";
        }

        public override void Speak()
        {
            Console.WriteLine("Sad cat sounds");
        }
    }
}