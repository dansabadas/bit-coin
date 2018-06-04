using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.PurelyFunctional
{
  public class Gift : IMoney
  {
    public decimal Value { get; }
    public Currency Currency { get; }
    public DateTime ValidBefore { get; }

    public Gift(decimal value, Currency currency, DateTime validBefore)
    {
      Value = value;
      Currency = currency ?? throw new ArgumentException(nameof(currency));
      ValidBefore = validBefore;
    }
  }
}
