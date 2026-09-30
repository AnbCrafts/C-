using System;

namespace MyFirstProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== DEMO: Console.Read() ==========\n");

            Console.Write("Step 1: Enter a character and press Enter: ");

            int var1 = Console.Read();

            Console.WriteLine("\n\nConsole.Read() execution completed.");
            Console.WriteLine($"Value stored in var1 = {var1}");
            Console.WriteLine($"Character represented by var1 = {(char)var1}");

            Console.WriteLine("\nExplanation:");
            Console.WriteLine("- Console.Read() returns an INT.");
            Console.WriteLine("- That INT is the ASCII/Unicode value of the character.");
            Console.WriteLine("- Example: A = 65, a = 97, 1 = 49");



            Console.WriteLine("\n\n========== DEMO: Console.ReadKey() ==========\n");

            Console.Write("Step 2: Press any key: ");

            ConsoleKeyInfo var2 = Console.ReadKey();

            Console.WriteLine("\n\nConsole.ReadKey() execution completed.");

            Console.WriteLine("\nInspecting the ConsoleKeyInfo object:");

            Console.WriteLine($"var2.Key      = {var2.Key}");
            Console.WriteLine($"var2.KeyChar  = {var2.KeyChar}");
            Console.WriteLine($"ASCII Value   = {(int)var2.KeyChar}");

            Console.WriteLine("\nExplanation:");
            Console.WriteLine($"Key     -> {var2.Key}");
            Console.WriteLine($"KeyChar -> {var2.KeyChar}");



            Console.WriteLine("\n\n========== COMPARISON ==========");

            Console.WriteLine(@"
Key:
-----
Represents the keyboard key.
Examples:
A
Enter
Escape
F1

KeyChar:
---------
Represents the actual character generated.
Examples:
a
A
1
!
");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}