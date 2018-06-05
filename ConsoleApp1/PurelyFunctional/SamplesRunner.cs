using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.PurelyFunctional
{
  public static class SamplesRunner
  {
    static void PrintPrices2(int from, int to, Func<int, Amount> priceOf) =>
            Enumerable.Range(from, to - from + 1)
                .Select(quantity => (quantity, price: priceOf(quantity)))
                .Select(tuple => $"{tuple.quantity}\t{tuple.price}")
                .Join(Environment.NewLine)
                .WriteLine();

    public static void Run()
    {
      //Tuple<IMoney, Amount> toopie = Add(new Cash(0, Currency.USD), new Amount(0, Currency.USD), DateTime.Now).ToTuple();
      //ValueTuple<IMoney, Amount> toopie2 = Add(new Cash(0, Currency.USD), new Amount(0, Currency.USD), DateTime.Now);

      //PrintPrices(
      //          new Product("Steering wheel",
      //              new Amount(20, new Currency("USD"))),
      //          1, 10);


      Product product =
          new Product("Steering wheel",
              new Amount(20, Currency.USD));

      Func<Product, Amount, Amount> priceCalculator =
          (prod, price) => prod.CalculateTax(price);
      Amount priceCalculator2(Product prod, Amount price) => prod.CalculateTax(price);
      Amount priceCalculator3(Product prod, Amount price)
      {
        return prod.CalculateTax(price);
      }

      PrintPrices2(10, 19,
                product.TotalPriceCalculator(
                    (prod, price) => prod.CalculateTax(price)));
      PrintPrices2(10, 19, product.TotalPriceCalculator(priceCalculator3));

      void log(object messages)
      {
        if (messages == null)
        {
          Console.WriteLine("NULL");
        }

        Console.WriteLine(messages);
      }

      Func<int, int, int> scale = (factor, x) => factor * x;
      Func<int, int> @double = x => scale(x, 2);

      int scale2(int factor, int x) => factor * x;
      int @double2(int x) => scale2(x, 2);

      Action<Func<int, int>> doSomething = (Func<int, int> inputFunc) => { };
      doSomething(@double);
      doSomething(@double2);

      int factor2 = 2;
      int scale3(int x) => factor2 * x;

      void Work(Func<int, int> scaleF)
      {
        int y = scaleF(5);
        log(y);
      }

      factor2 = 3;
      Work(scale3);

      HashSet<int> set = new HashSet<int>();
      set.Add(22);
      set.Add(33);
      set.Add(27);
      set.Add(54);

      var contains33 = set.Contains(33);
      log(contains33);

      Element<int>[] set2 = new Element<int>[7];
      set2.Add(22);
      set2.Add(33);
      set2.Add(27);
      set2.Add(54);

      contains33 = set2.Contains(33);
      log(contains33);

      Currency eur = Currency.EUR;
      Element<Currency>[] set3 = new Element<Currency>[7];

      set3.Add(eur);
      if (set3.Contains(eur))
      {
        Console.WriteLine("Suspect found!");
      }
      else
      {
        Console.WriteLine("Suspect NOT found!");
      }

      Console.ReadLine();
    }
  }
}
