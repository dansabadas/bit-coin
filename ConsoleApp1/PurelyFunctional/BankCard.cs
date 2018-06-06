using System;

namespace ConsoleApp1.PurelyFunctional
{
  public class BankCard : IMoney
  {
    public DateTime ValidBefore { get; }

    public BankCard(Month validBefore)
    {
      this.ValidBefore = validBefore;
    }
  }
}
