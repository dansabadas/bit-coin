using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.PurelyFunctional
{
  public static class SamplesRunner
  {
    public static (IMoney final, Amount added) Add(IMoney money, Amount amount, DateTime at)
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
  }
}
