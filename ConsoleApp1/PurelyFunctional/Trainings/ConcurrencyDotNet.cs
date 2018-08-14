using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1.PurelyFunctional.Trainings
{
    public static class ConcurrencyDotNet
    {
        public static void Run()
        {
            FList<int> list1 = FList<int>.Empty;
            FList<int> list2 = list1.Cons(1).Cons(2).Cons(3);
            FList<int> list3 = FList<int>.Cons(1, FList<int>.Empty);
            FList<int> list4 = list2.Cons(2).Cons(3);

            Func<int, bool> isPrime = n => {
                if (n == 1) return false;
                if (n == 2) return true;
                var boundary = (int)Math.Floor(Math.Sqrt(n));
                for (int i = 2; i <= boundary; ++i)
                    if (n % i == 0) return false;
                return true;
            };

            int len = 10000000, count = 0;
            long total = 0;

            Parallel.For(0, len,
            //i => {
            //if (isPrime(i))
            //{
            //    total += i;
            //    count += 1;
            //}});
            () => 0,
            (int i, ParallelLoopState loopState, long tlsValue) => isPrime(i) ? tlsValue += i : tlsValue,
            value =>
            {
                Interlocked.Add(ref total, value);
                if (value > 0)
                {
                    Interlocked.Increment(ref count);
                }
            });// TODO: how do I calculate rigorously the total in the same parallel enumeration?


            Console.WriteLine($"total={total} count={count}");

            Console.WriteLine(Greeting ("Richard"));
            Thread.Sleep(2000);
            Console.WriteLine(Greeting ("Paul"));
            Thread.Sleep(2000);
            Console.WriteLine(Greeting ("Richard"));

            Func<string, string> grFunc = (name) => $"Warm greetings {name}, the time is {DateTime.Now.ToString("hh:mm:ss")}";
            var greetingMemoize = grFunc.Memoize(); // FuncExtensionMethods.Memoize<string, string>(Greeting);
            Console.WriteLine(greetingMemoize ("Richard"));
            Thread.Sleep(2000);
            Console.WriteLine(greetingMemoize ("Paul"));
            Thread.Sleep(2000);
            Console.WriteLine(greetingMemoize("Richard"));

            var greetingMemoize2 = grFunc.MemoizeLazyThreadSafe();
            Console.WriteLine(greetingMemoize2 ("Richard"));
            Thread.Sleep(2000);
            Console.WriteLine(greetingMemoize2 ("Paul"));
            Thread.Sleep(2000);
            Console.WriteLine(greetingMemoize2("Richard"));
        }

        private static string Greeting(string name)
        {
            return $"Warm greetings {name}, the time is {DateTime.Now.ToString("hh:mm:ss")}";
        }
    }
}
