using ConsoleApp1.IL;
using ConsoleApp1.PurelyFunctional;
using ConsoleApp1.PurelyFunctional.Trainings;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //IlCSharpSamples.Run();
            //PurelyFunctional.SamplesRunner.Run();
            //BitcoinClientSamples.Run();
            //new PurelyFunctional.CrockfordClosures().Run();
            Sha256HashAlgorithms.Sha256Sample();

            //ConcurrencyDotNet.Run();
            new CrockfordClosures().Run();
        }
    }
}
