using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.PurelyFunctional
{
  public class Amount
  {
    public decimal Value { get; }
    public Currency Currency { get; }

    public Amount(decimal value, Currency currency)
    {
      Value = value;
      Currency = currency ?? throw new ArgumentException(nameof(currency));
    }

    public override string ToString() => $"{this.Value} {this.Currency}";
  }
}
