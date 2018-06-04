using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.PurelyFunctional
{
  static class StringExtensions
  {
    public static string Join(this IEnumerable<string> sequence, string separator) =>
           string.Join(separator, sequence.ToArray());
  }
}
