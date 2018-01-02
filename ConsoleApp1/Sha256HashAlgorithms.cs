using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp1
{
    public static class Sha256HashAlgorithms
    {
        static readonly long MAX_NONCE;

        static Sha256HashAlgorithms()
        {
            MAX_NONCE = (long)2.Pow(32);
        }

        public static double Pow(this int baseOfPower, int exponent)
        {
            return Math.Pow(baseOfPower, exponent);
        }

        public static BigInteger ToBigInteger(this string hexString)
        {
            return BigInteger.Parse(0+hexString, NumberStyles.AllowHexSpecifier);
        }

        public static Tuple<string, int> ProofOfWork(string header, int difficultyBits)
        {
            var target = (BigInteger)(2.Pow(256 - difficultyBits));
            for (int nonce = 0; nonce < MAX_NONCE; nonce++)
            {
                string hashResult = (header + nonce).ToSha256();
                var result = hashResult.ToBigInteger();
                if (result < target)
                {
                    Console.WriteLine($"Success with nonce {nonce}. Hash is {hashResult}");
                    return Tuple.Create(hashResult, nonce);
                }
            }

            return null;
        }

        public static void ProofOfWorkSample()
        {
            Stopwatch stopWatch = new Stopwatch();
            string previousHashResult = string.Empty;
            for (int difficultyBits = 0; difficultyBits < 32; difficultyBits++)
            {
                var difficulty = 2.Pow(difficultyBits);
                Console.WriteLine($"Difficulty: {difficulty}, {difficultyBits}");
                Console.WriteLine("Starting search...");
                stopWatch.Start();

                string newBlock = "test block with transactions" + previousHashResult;
                var (hashResult, nonce) = ProofOfWork(newBlock, difficultyBits);
                previousHashResult = hashResult;

                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;
                if (ts > TimeSpan.Zero)
                {
                    string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds/10:00}";
                    Console.WriteLine($"Elapsed Time {elapsedTime}");

                    var hashPower = nonce/ts.TotalSeconds;
                    Console.WriteLine($"Hashing Power: {hashPower} hashes per second.");
                }
            }
        }

        public static string ToSha256(this string value)
        {
            var sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }


    }
}
