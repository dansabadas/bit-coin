using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.PurelyFunctional
{
  public class Cash : IMoney
  {
    public decimal Value { get; }
    public Currency Currency { get; }

    public Cash(decimal value, Currency currency)
    {
      Value = value;
      Currency = currency ?? throw new ArgumentException(nameof(currency));
    }
  }
}
