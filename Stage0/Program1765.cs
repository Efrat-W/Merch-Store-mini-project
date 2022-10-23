using System;

namespace Stage0
    {
        partial class Program
    {
        static void Main(string[] args)
        {
            welcome1765();
            welcome6768();
            Console.ReadKey();
        }

        static partial void welcome6768();
        private static void welcome1765()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}