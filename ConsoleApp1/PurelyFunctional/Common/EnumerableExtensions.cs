using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.PurelyFunctional.Common
{
  public static class EnumerableExtensions
  {
    public static IEnumerable<TResult> SelectOptional<T, TResult>(
        this IEnumerable<T> sequence, Func<T, Option<TResult>> map) =>
        sequence.Select(map)
            .OfType<Some<TResult>>()
            .Select(some => some.Content);
  }
}
