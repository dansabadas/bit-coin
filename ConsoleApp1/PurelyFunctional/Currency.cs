using System;

namespace ConsoleApp1.PurelyFunctional
{
  public sealed class Currency : IEquatable<Currency>
  {
    private string Symbol { get; }
    private Currency(string symbol)
    {
      Symbol = !string.IsNullOrEmpty(symbol)
        ? symbol
        : throw new ArgumentException();
    }

    public static Currency USD => new Currency("USD");
    public static Currency EUR => new Currency("EUR");
    public static Currency JPY => new Currency("JPY");

    private static Random Rng { get; } = new Random(709);

    public override int GetHashCode() =>
        Symbol.GetHashCode(); // or Symbol.GetHashCode(); Rng.Next() // random is not good, should be the same value on subsequent calls

    public override string ToString() =>
        this.Symbol;

    public override bool Equals(object obj) => this.Equals((Currency)obj);

    public bool Equals(Currency obj) =>
            !ReferenceEquals(obj, null) && obj.Symbol == this.Symbol;

    public static bool operator ==(Currency a, Currency b) =>
        ReferenceEquals(a, b) ||
        (!ReferenceEquals(a, null) && a.Equals(b));

    public static bool operator !=(Currency a, Currency b) => !(a == b);
  }
}
