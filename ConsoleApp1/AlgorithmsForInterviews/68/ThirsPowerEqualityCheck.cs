using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.Algorithms._68
{
    /// <summary>
    /// Print all positive integer solutions to the equation a3 + b3 = c3 + d3 where a, b, c and d are integers between 1 and 1000.
    /// </summary>
    public static class ThirdPowerEqualityCheck
    {
        public static void Run()
        {
            var cachedSums = new Dictionary<int, List<Tuple<int, int>>>();
            for (int a = 1; a <= 1000; a++)
            {
                for (int b = 1; b <= 1000; b++)
                {
                    int result = a * a * a + b * b * b;
                    var currentTuple = Tuple.Create(a, b);
                    if (!cachedSums.TryGetValue(result, out var cachedResult))
                    {
                        cachedResult = new List<Tuple<int, int>>();
                        cachedSums.Add(result, cachedResult);
                    }

                    cachedResult.Add(currentTuple);
                }
            }

            //var bigSol = cachedSums.Values.AsEnumerable().Where(list => list.Count > 2).ToList();
            //Console.WriteLine(Math.Pow(428, 3) + Math.Pow(346, 3));
            //Console.WriteLine(Math.Pow(492, 3) + Math.Pow(90, 3));
            //Console.WriteLine(Math.Pow(493, 3) + Math.Pow(11, 3));
            int currentIteratedSolution = 0;
            foreach (var keyValuePair in cachedSums)
            {
                foreach (var (a, b) in keyValuePair.Value)
                {
                    foreach (var (c, d) in keyValuePair.Value)
                    {
                        Console.WriteLine($"{++currentIteratedSolution}:({a},{b},{c},{d})");
                    }
                }
            }

            //for (int c = 1; c <= 1000; c++)
            //{
            //    for (int d = 1; d <= 1000; d++)
            //    {
            //        int result = c * c * c + d * d * d;
            //        if (cachedSums.TryGetValue(result, out var cachedResult))
            //        {
            //            foreach (var (a, b) in cachedResult)
            //            {
            //                Console.WriteLine($"{++currentIteratedSolution}:({a},{b},{c},{d})");
            //            }
            //        }
            //    }
            //}
        }
    }
}
