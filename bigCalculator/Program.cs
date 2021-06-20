using System;

namespace bigCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("It's a simple calculator for really big numbers, so it can operate only with non-negative numbers :(");
            Console.WriteLine("Enter expression to solve:\t\t(Available operations: + - * /)");
            string exp = Console.ReadLine();
            //try
            //{
                calculator c = new calculator(exp);
                Console.WriteLine($"Answer: {c}");
            /*}
            catch (Exception e)
            {
                Console.WriteLine($"Something went wrong......\n{e.Message}");
            }*/
            
        }
    }
}
