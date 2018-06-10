using System;

namespace ConsoleApp1.PurelyFunctional.Common
{
  public static class ObjectExtensions
  {
    public static TResult Map<T, TResult>(this T obj, Func<T, TResult> map)
      =>
        map(obj);
  }
}
