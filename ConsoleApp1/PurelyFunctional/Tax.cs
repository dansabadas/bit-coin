namespace ConsoleApp1.PurelyFunctional
{
  static class Tax
  {
    public static Amount DefaultTax(this Amount basePrice) =>
        basePrice.RelativeTax(.20M);

    public static Amount RelativeTax(this Amount basePrice, decimal factor) =>
        basePrice.Scale(factor);

    public static Amount CalculateTax(this Product product, Amount basePrice) =>
        basePrice.DefaultTax();
  }
}
