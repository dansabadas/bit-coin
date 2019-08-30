using System;

namespace ConsoleApp1.Algorithms._73
{
    /// <summary>
    /// Given two sorted arrays, find the number of elements in common.
    /// The arrays are the same length and each has all distinct elements.
    /// </summary>
    public static class BCR
    {
        public static void Run()
        {
            //int[] A = {13, 27, 35, 40, 49, 55, 59};
            //int[] B = {17, 35, 39, 40, 55, 58, 60};

            //int[] A = { 13, 27, 35, 40, 49, 55, 60 };
            //int[] B = { 17, 35, 39, 40, 55, 58, 60 };

            int[] A = { 13, 27, 35, 40, 49, 55, 60 };
            int[] B = { 13, 35, 39, 40, 55, 58, 60 };

            int i = 0, j = 0, foundMatchedCount = 0;
            while (i < A.Length && j < A.Length)
            {
                if (A[i] < B[j])
                {
                    i++;
                    continue;
                }

                if (A[i] > B[j])
                {
                    j++;
                    continue;
                }

                i++;
                j++;
                foundMatchedCount++;
            }

            Console.WriteLine($"found {foundMatchedCount} matches.");
        }
    }
}
