using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.PurelyFunctional.Common
{
  public class Reiterable<T> : IEnumerable<T>
  {
    private IEnumerable<T> Data { get; }

    private Reiterable(IEnumerable<T> data)
    {
      this.Data = data;
    }

    public IEnumerator<T> GetEnumerator() => this.Data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public static implicit operator Reiterable<T>(List<T> data) =>
        new Reiterable<T>(data);

    public static implicit operator Reiterable<T>(T[] data) =>
        new Reiterable<T>(data);

    public static Reiterable<T> From(IEnumerable<T> data)
    {
      switch (data)
      {
        case List<T> list: return list;
        case T[] array: return array;
        case Reiterable<T> reiterable: return reiterable;
        default: return new Reiterable<T>(data.ToList());
      }
    }
  }

  public static class ReiterableExtensions
  {
    public static Reiterable<T> AsReiterable<T>(this IEnumerable<T> data) =>
        Reiterable<T>.From(data);
  }
}
