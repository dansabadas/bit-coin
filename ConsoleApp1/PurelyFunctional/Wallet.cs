using ConsoleApp1.PurelyFunctional.Common;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ConsoleApp1.PurelyFunctional
{
  public class Wallet
  {
    public Currency BaseCurrency { get; }
    public ImmutableList<IMoney> Moneys { get; }
    public Wallet(Currency baseCurrency, ImmutableList<IMoney> moneys)
    {
      this.BaseCurrency = baseCurrency;
      this.Moneys = moneys;
    }
  }
}
