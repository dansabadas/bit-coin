﻿using System;
using System.Linq;

namespace ConsoleApp1.PurelyFunctional
{
  public static class SamplesRunner
  {
    static (IMoney final, Amount added) Add(IMoney money, Amount amount, DateTime at)
    {
      switch (money)
      {
        case Cash cash when amount.Currency == cash.Currency:
          return (new Cash(cash.Value + amount.Value, cash.Currency), amount);
        case Cash _:
          return (money, new Amount(0, amount.Currency));
        case Gift gift when at < gift.ValidBefore && gift.Currency == amount.Currency:
          return (new Gift(gift.Value + amount.Value, gift.Currency, gift.ValidBefore), amount);
        case Gift _:
          return (money, new Amount(0, amount.Currency));
        default:
          throw new ArgumentException();
      }
    }

    //static void PrintPrices(Product product, int from, int to) =>
    //        Enumerable.Range(from, to - from + 1)
    //            .Select(quantity => (quantity, totalPrice: product.Buy(quantity).TotalPrice))
    //            .Select(tuple => $"{tuple.quantity}\t{tuple.totalPrice}")
    //            .Join(Environment.NewLine)
    //            .WriteLine();

    static void PrintPrices2(int from, int to, Func<int, Amount> priceOf) =>
            Enumerable.Range(from, to - from + 1)
                .Select(quantity => (quantity, price: priceOf(quantity)))
                .Select(tuple => $"{tuple.quantity}\t{tuple.price}")
                .Join(Environment.NewLine)
                .WriteLine();

    public static void Run()
    {
      Tuple<IMoney, Amount> toopie = Add(new Cash(0, new Currency("usd")), new Amount(0, new Currency("usd")), DateTime.Now).ToTuple();
      ValueTuple<IMoney, Amount> toopie2 = Add(new Cash(0, new Currency("usd")), new Amount(0, new Currency("usd")), DateTime.Now);

      //PrintPrices(
      //          new Product("Steering wheel",
      //              new Amount(20, new Currency("USD"))),
      //          1, 10);


      Product product =
          new Product("Steering wheel",
              new Amount(20, new Currency("USD")));

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

      var closure = new Closure();
      Console.ReadLine();
    }

    class Closure
    {
      public object environment;
      public object Function(object arg) => null;
    }
  }
}
