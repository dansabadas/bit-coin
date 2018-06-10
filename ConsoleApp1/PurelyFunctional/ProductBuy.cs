using System;

namespace ConsoleApp1.PurelyFunctional
{
  public static class ProductBuy
  {
    private static TResult Mapp<T, TResult>(this T obj, Func<T, TResult> map) => map(obj);

    private static InvoiceLine
      ToInvoiceLine(this (Amount basePrice, Amount tax) tuple, string label) => new InvoiceLine(label, tuple.basePrice, tuple.tax);

    internal static InvoiceLine Buy(this Product product, int quantity, Func<Product, Amount, Amount> taxCalculator) =>
      product
        .GetPriceSpecification(quantity, taxCalculator)
        .ToInvoiceLine(product.Name);

    private static Amount GetBasePrice(this Product product, int quantity) => product.UnitPrice.Scale(quantity);

    public static (Amount basePrice, Amount tax)
            GetPriceSpecification(this Product product, int quantity, Func<Product, Amount, Amount> taxCalculator) =>
            product.GetBasePrice(quantity)
                .Mapp(basePrice => (basePrice, taxCalculator(product, basePrice)));

    public static Func<int, Amount> TotalPriceCalculator(
            this Product product, Func<Product, Amount, Amount> taxCalculator) =>
            quantity => product.GetPriceSpecification(quantity, taxCalculator)
                .Mapp(tuple => tuple.basePrice.Add(tuple.tax.Value));

    public static Func<int, Amount> TotalPriceCalculator(this Product product) =>
            product.TotalPriceCalculator((prod, price) => price.DefaultTax());
  }
}
