using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.Algorithms._70
{
    /// <summary>
    /// Given a smaller string a and a bigger string b, design an algorithm to find all permutations
    /// of the shorter string within the longer one. Print the location of each permutation.
    /// </summary>
    public static class StringPermutationsFinder
    {
        public static void Run()
        {
            string shortstr1 = "abbc";
            string longsstr2 = "cbabadcbbabbcbabaabccbabc";

            var shortStrCharsStatisticsDictionary = shortstr1
                .ToCharArray()
                .GroupBy(c => c)
                .Select(group => new {Value = group.Key, Count = group.Count()})
                .ToDictionary(a => a.Value, a => a.Count);

            var shortStrLength = shortstr1.Length;
            int matches = 0;
            for (int i = 0; i <= longsstr2.Length - shortStrLength; i++)
            {
                var currentlyIteratedShortChunkStatisticsList = longsstr2
                    .Substring(i, shortStrLength)
                    .ToCharArray()
                    .GroupBy(c => c)
                    .Select(group => new { Value = group.Key, Count = group.Count() })
                    .ToList();

                if (shortStrCharsStatisticsDictionary.Count != currentlyIteratedShortChunkStatisticsList.Count)
                {
                    continue;
                }

                bool mismatchDetected = false;
                foreach (var charCount in currentlyIteratedShortChunkStatisticsList)
                {
                    if (!shortStrCharsStatisticsDictionary.ContainsKey(charCount.Value))
                    {
                        mismatchDetected = true;
                        break;
                    }

                    if (shortStrCharsStatisticsDictionary[charCount.Value] != charCount.Count)
                    {
                        mismatchDetected = true;
                        break;
                    }
                }

                if (!mismatchDetected)
                {
                    matches++;
                }
            }

            Console.WriteLine($"matches={matches}");
        }
    }
}
