using System;

namespace ConsoleApp1.PurelyFunctional.Trainings
{
    static class FuncExtensionMethods
    {
        static Func<A, C> Compose<A, B, C>(this Func<A, B> f, Func<B, C> g)
          => (n) => g(f(n));
    }
}
