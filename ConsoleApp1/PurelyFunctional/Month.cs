using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.PurelyFunctional
{
  public class Month
  {
    private DateTime Value { get; }

    public Month(int year, int month)
    {
      this.Value = new DateTime(year, month, 1);
    }

    public static implicit operator DateTime(Month month) => month.Value;
  }
}
