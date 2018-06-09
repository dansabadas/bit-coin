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
  }
}
