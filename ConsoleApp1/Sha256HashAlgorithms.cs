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
        static readonly BigInteger MAX_NONCE;

        static Sha256HashAlgorithms()
        {
            MAX_NONCE = (BigInteger)2.Pow(32);
        }

        public static double Pow(this int baseOfPower, int exponent)
        {
            return Math.Pow(baseOfPower, exponent);
        }

        public static BigInteger ToBigInteger(this string hexString)
        {
            return BigInteger.Parse(0+hexString, NumberStyles.AllowHexSpecifier);
        }

        public static Tuple<string, int, BigInteger> ProofOfWork(string eachMinerBlockHeaderHash, int difficultyBits)
        {
            var target = (BigInteger)2.Pow(256 - difficultyBits);
            for (int nonce = 0; nonce < MAX_NONCE; nonce++)
            {
                string hashResult = (eachMinerBlockHeaderHash + nonce).ToSha256();
                var result = hashResult.ToBigInteger();
                if (result < target)
                {
                    return Tuple.Create(hashResult, nonce, target);
                }
            }

            return Tuple.Create(string.Empty, 0, new BigInteger());
        }

        public static void ProofOfWorkSample()
        {
            Stopwatch stopWatch = new Stopwatch();
            string previousHashResult = string.Empty;
            for (int difficultyBits = 0; difficultyBits < 32; difficultyBits++)
            {
                var difficulty = 2.Pow(difficultyBits);
                Console.WriteLine($"Difficulty: {difficulty}, {difficultyBits}");
                string newBlock = "test block with transactions" + previousHashResult;
                Console.WriteLine($"Starting search for block {newBlock}...");

                stopWatch.Start();

                var (hashResult, nonce, target) = ProofOfWork(newBlock, difficultyBits);
                previousHashResult = hashResult;

                stopWatch.Stop();

                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;
                if (ts > TimeSpan.Zero)
                {
                    Console.WriteLine($"Success with nonce {nonce}.\nHash is {hashResult}.\nTarget is {target}");
                    string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds/10:00}";
                    Console.WriteLine($"Elapsed Time {elapsedTime}");

                    var hashPower = (int)(nonce/ts.TotalSeconds);
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
