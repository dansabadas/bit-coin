using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.PurelyFunctional.Trainings
{
    public static class ConcurrencyDotNet
    {
        public static void Run()
        {
            Console.WriteLine(Greeting ("Richard"));
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine(Greeting ("Paul"));
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine(Greeting ("Richard"));

            Func<string, string> grFunc = (name) => $"Warm greetings {name}, the time is {DateTime.Now.ToString("hh:mm:ss")}";
            var greetingMemoize = grFunc.Memoize(); // FuncExtensionMethods.Memoize<string, string>(Greeting);
            Console.WriteLine(greetingMemoize ("Richard"));
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine(greetingMemoize ("Paul"));
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine(greetingMemoize("Richard"));

            var greetingMemoize2 = grFunc.MemoizeLazyThreadSafe();
            Console.WriteLine(greetingMemoize2 ("Richard"));
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine(greetingMemoize2 ("Paul"));
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine(greetingMemoize2("Richard"));
        }

        private static string Greeting(string name)
        {
            return $"Warm greetings {name}, the time is {DateTime.Now.ToString("hh:mm:ss")}";
        }
    }
}
