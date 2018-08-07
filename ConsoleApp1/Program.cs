using System;


namespace ConsoleApp1
{
  class Program
  {
    static void Main(string[] args)
    {
      //BitcoinClientSamples.Run();
      PurelyFunctional.SamplesRunner.Run();
      //new PurelyFunctional.CrockfordClosures().Run();
      Sha256Sample();
    }

    private static void Sha256Sample()
    {
      string text = "I am Satoshi Nakamoto";
      Console.WriteLine(text.ToSha256());

      for (int nonce = 0; nonce < 20; nonce++)
      {
        string input = text + nonce;
        var hash = input.ToSha256();
        Console.WriteLine($"{input}=>{hash}");
      }

      Sha256HashAlgorithms.ProofOfWorkSample();
    }
  }
}
