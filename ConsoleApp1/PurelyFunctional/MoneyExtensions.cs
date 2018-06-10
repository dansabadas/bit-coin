using ConsoleApp1.PurelyFunctional.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.PurelyFunctional
{
  public static class MoneyExtensions
  {
    public static Option<IMoney> PayableAt(this IMoney money, DateTime time) =>
            money.IsPayableAt(time)
                ? (Option<IMoney>)new Some<IMoney>(money)
                : None.Value;

    public static (Amount subtracted, IMoney remaining) Subtract(
            this IMoney from, Amount amount)
    {
      switch (from)
      {
        case Cash cash:
          return new Amount(cash.Value, cash.Currency)
              .Subtract(amount)
              .Map(pair => (pair.subtracted, new Cash(pair.final.Value, pair.final.Currency)));
        case Gift gift:
          return gift.Value
              .Subtract(amount)
              .Map(pair => (pair.subtracted, new Gift(pair.final, gift.ValidBefore)));
        case BankCard card:
          return (amount, card);
        default: throw new ArgumentException("Unsupported money type.");
      }
    }

    private static bool IsPayableAt(this IMoney money, DateTime time)
    {
      switch (money)
      {
        case Cash _:
        case Gift gift when time < gift.ValidBefore:
        case BankCard card when time < card.ValidBefore:
          return true;
        default:
          return false;
      }
    }

    public static IEnumerable<IMoney> PayableAt(
            this IEnumerable<IMoney> moneys, DateTime at) =>
            moneys.All(money => money.IsPayableAt(at))
                ? moneys
                : moneys.Where(money => money.IsPayableAt(at));

    public static IEnumerable<(Amount subtractedRunningSum, IMoney remaining)>
            Subtract(this IEnumerable<IMoney> moneys, Amount total)
    {
      Amount toPay = total;

      foreach (IMoney money in moneys)
      {
        (Amount paid, IMoney remaining) = money.Subtract(toPay);

        toPay = toPay.Subtract(paid).final;

        yield return (paid, remaining);
      }
    }
  }
}
