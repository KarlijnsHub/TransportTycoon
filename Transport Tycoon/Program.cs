using System;
using System.Text.RegularExpressions;

namespace Transport_Tycoon
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = string.Empty;

            Console.WriteLine("Hello, what is the tranport plan?");
            input = Console.ReadLine();

            while (!Regex.IsMatch(input, @"^[a-bA-B]+$"))
            {
                Console.WriteLine("That doesn't sound right. What is the tranport plan?");
                input = Console.ReadLine();
            }

            RouteManager routeManager = new RouteManager(input);
            var result = routeManager.ExecuteRoute();
            Console.WriteLine(result);
        }
    }
}
