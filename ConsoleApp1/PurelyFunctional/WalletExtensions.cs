using ConsoleApp1.PurelyFunctional.Common;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace ConsoleApp1.PurelyFunctional
{
  public static class WalletExtensions
  {
    public static Wallet PayableAt(this Wallet wallet, DateTime at) =>
            new Wallet(
                wallet.BaseCurrency,
                wallet.Moneys.PayableAt(at).ToImmutableList());

    public static Wallet With(this Wallet wallet, Currency baseCurrency) =>
            new Wallet(baseCurrency, wallet.Moneys);

    public static Wallet Add(this Wallet wallet, IMoney money) =>
        new Wallet(
            wallet.BaseCurrency,
            wallet.Moneys.Add(money));

    public static (Amount subtracted, Wallet remaining) Subtract(
            this Wallet wallet, Amount amount)
    {
      Amount subtracted = amount.Currency.Zero();
      ImmutableList<IMoney>.Builder listBuilder =
          ImmutableList.CreateBuilder<IMoney>();

      foreach ((Amount paid, IMoney remaining) in wallet.Moneys.Subtract(amount))
      {
        subtracted = subtracted.Add(paid.Value);
        listBuilder.Add(remaining);
      }

      return (
          subtracted,
          new Wallet(wallet.BaseCurrency, listBuilder.ToImmutable()));
    }

    private static void Demo(Wallet wallet)
    {
      Amount bill = new Amount(10, Currency.USD);
      var (paid, newWallet) = wallet.PayableAt(DateTime.UtcNow).Subtract(bill);

      if (paid == bill)
      {
        // persist newWallet
        // return OK
      }
      else
      {
        Amount missing = bill.Subtract(paid).final;
        // fail and return missing
      }
    }
  }
}
