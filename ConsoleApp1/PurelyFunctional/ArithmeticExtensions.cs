using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.PurelyFunctional
{
  static class Arithmetic
  {
    public static Amount Scale(this Amount amount, decimal factor) =>
        new Amount(amount.Value * factor, amount.Currency);

    public static Amount Add(this Amount amount, decimal value) =>
        new Amount(amount.Value + value, amount.Currency);
  }
}
