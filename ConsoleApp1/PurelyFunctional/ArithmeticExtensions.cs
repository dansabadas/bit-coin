namespace ConsoleApp1.PurelyFunctional
{
  static class Arithmetic
  {
    public static Amount Scale(this Amount amount, decimal factor) =>
        new Amount(amount.Value * factor, amount.Currency);

    public static Amount Add(this Amount amount, decimal value) =>
        new Amount(amount.Value + value, amount.Currency);

    public static (Amount subtracted, Amount final) Subtract(this Amount from, Amount amount) =>
            from.Currency != amount.Currency 
              ? (amount.Currency.Zero(), from)
              : from.Value <= amount.Value 
                ? (amount, from.Currency.Zero())
                : (amount, new Amount(from.Value - amount.Value, from.Currency));
  }
}
