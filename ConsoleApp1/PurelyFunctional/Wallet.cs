using ConsoleApp1.PurelyFunctional.Common;
using System;
using System.Collections.Generic;

namespace ConsoleApp1.PurelyFunctional
{
  public class Wallet
  {
    public IEnumerable<IMoney> Moneys { get; }
    //public IEnumerable<IMoney> Moneys => this.Materialized;
    //private List<IMoney> Materialized { get; }

    public Wallet(Common.Reiterable<IMoney> moneys)
    {
      //this.Materialized = moneys.ToList();
      this.Moneys = moneys;
    }
  }

  public static class WalletExtensions
  {
    public static Wallet PayableAt(this Wallet wallet, DateTime at) =>
        new Wallet(wallet.Moneys
            .SelectOptional(money => money.PayableAt(at))
            .AsReiterable());
  }
}
