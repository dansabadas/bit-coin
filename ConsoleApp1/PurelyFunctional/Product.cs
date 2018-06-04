using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.PurelyFunctional
{
  public class Product
  {
    public string Name { get; }
    public Amount UnitPrice { get; }

    public Product(string name, Amount unitPrice)
    {
      Name = !string.IsNullOrEmpty(name)
        ? name
        : throw new ArgumentException(nameof(name));

      UnitPrice = unitPrice ?? throw new ArgumentException(nameof(unitPrice));
    }
  }
}
