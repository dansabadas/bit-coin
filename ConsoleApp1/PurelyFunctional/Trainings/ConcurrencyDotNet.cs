using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
=======
using System.Threading;
using System.Threading.Tasks;
>>>>>>> c8a37aed0a9a5b077b5ea28d62b94c57b806bbcb

namespace ConsoleApp1.PurelyFunctional.Trainings
{
    public static class ConcurrencyDotNet
    {
        public static void Run()
        {
<<<<<<< HEAD
            var data = new int[1000000];
            for (int i = 0; i < data.Length; i++)
                data[i] = i;

            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            long total =
                data.AsParallel()
                    .Where(n => n % 2 == 0)
                    .Select(n => n + n)
                    .Sum(x => (long)x);
            stopwatch.Stop();

            // Write result.
            Console.WriteLine($"Time elapsed for normal: {stopwatch.Elapsed} with result = {total}");

            stopwatch.Start();
            long total2 = data.AsParallel()
                .Aggregate(0L, (acc, n) => n % 2 == 0 ? acc + (n + n) : acc);
            stopwatch.Stop();

            // Write result.
            Console.WriteLine($"Time elapsed for deforested: {stopwatch.Elapsed} with result = {total2}");

            Console.WriteLine(Greeting("Richard"));
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine(Greeting("Paul"));
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine(Greeting("Richard"));
=======
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
>>>>>>> c8a37aed0a9a5b077b5ea28d62b94c57b806bbcb

            Func<string, string> grFunc = (name) => $"Warm greetings {name}, the time is {DateTime.Now:hh:mm:ss}";
            var greetingMemoize = grFunc.Memoize(); // FuncExtensionMethods.Memoize<string, string>(Greeting);
<<<<<<< HEAD
            Console.WriteLine(greetingMemoize("Richard"));
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine(greetingMemoize("Paul"));
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine(greetingMemoize("Richard"));

            var greetingMemoize2 = grFunc.MemoizeLazyThreadSafe();
            Console.WriteLine(greetingMemoize2("Richard"));
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine(greetingMemoize2("Paul"));
            System.Threading.Thread.Sleep(2000);
=======
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
>>>>>>> c8a37aed0a9a5b077b5ea28d62b94c57b806bbcb
            Console.WriteLine(greetingMemoize2("Richard"));
        }

        private static string Greeting(string name) => $"Warm greetings {name}, the time is {DateTime.Now:hh:mm:ss}";

        public static double AreaCircle(int radius) => Math.Pow(radius, 2) * Math.PI;

        public static int Add(int x, int y) => x + y;

        public static Dictionary<string, int> WordsCounter(string source)
        {
            var contentFiles = Directory
                .GetFiles(source, "*.txt")
                .Select(File.ReadLines);
            var partitionedResult = PureWordsPartitioner(contentFiles);

            var wordsCount =
                (from filePath in
                        Directory.GetFiles(source, "*.txt")
                            .AsParallel()
                    from line in File.ReadLines(filePath)
                    from word in line.Split(' ')
                    select word.ToUpper())
                .GroupBy(w => w)
                .OrderByDescending(v => v.Count())
                .Take(10);

            return wordsCount.ToDictionary(k => k.Key, v => v.Count());
        }

        static Dictionary<string, int> PureWordsPartitioner
            (IEnumerable<IEnumerable<string>> content) =>
            (from lines in content.AsParallel()
                from line in lines
                from word in line.Split(' ')
                select word.ToUpper())
            .GroupBy(w => w)
            .OrderByDescending(v => v.Count())
            .Take(10)
            .ToDictionary(k => k.Key, v => v.Count());

        static Dictionary<string, int> WordsPartitioner(string sourceFolder)
        {
            var contentFiles =
                (from filePath in Directory.GetFiles(sourceFolder, "*.txt")
                    let lines = File.ReadLines(filePath)
                    select lines);
            return PureWordsPartitioner(contentFiles);
        }
    }
}
