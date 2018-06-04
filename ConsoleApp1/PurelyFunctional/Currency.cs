using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.PurelyFunctional
{
  public class Currency
  {
    public string Symbol { get; }
    public Currency(string symbol)
    {
      Symbol = !string.IsNullOrEmpty(symbol)
        ? symbol
        : throw new ArgumentException();
    }
  }
}
