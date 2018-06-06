using System;

namespace ConsoleApp1.PurelyFunctional
{
  static class Hashing
  {
    private static int GetHashEntry<T>(this Element<T>[] set, T value)
    {
      if (object.ReferenceEquals(value, null))
        return 0;

      int code = value.GetHashCode();
      int entry = code % set.Length;
      if (entry < 0) entry += set.Length;
      return entry;
    }

    //private static int GetHashEntry(this Element<int>[] set, int value)
    //{
    //  int entry = value % set.Length;
    //  if (entry < 0) entry += set.Length;
    //  return entry;
    //}

    public static void Add<T>(this Element<T>[] set, T value)
    {
      int entry = set.GetHashEntry(value);
      Console.WriteLine($"Adding {value} to set[{entry}]");

      Element<T> element = new Element<T>()
      {
        Content = value,
        Next = set[entry]
      };

      set[entry] = element;
    }

    //public static void Add(this Element<int>[] set, int value)
    //{
    //  int entry = set.GetHashEntry(value);
    //  Console.WriteLine($"Adding {value} to set[{entry}]");

    //  Element<int> element = new Element<int>()
    //  {
    //    Content = value,
    //    Next = set[entry]
    //  };

    //  set[entry] = element;
    //}

    public static bool Contains<T>(this Element<T>[] set, T value)
    {
      int entry = set.GetHashEntry(value);
      Console.WriteLine($"Searching for {value} in set[{entry}]");

      for (Element<T> cur = set[entry]; cur != null; cur = cur.Next)
      {
        if (cur.Content.Equals(value))
        {
          return true;
        }
      }

      return false;
    }

    //public static bool Contains(this Element<int>[] set, int value)
    //{
    //  int entry = set.GetHashEntry(value);
    //  Console.WriteLine($"Searching for {value} in set[{entry}]");

    //  for (Element<int> cur = set[entry]; cur != null; cur = cur.Next)
    //  {
    //    if (cur.Content == value)
    //    {
    //      return true;
    //    }
    //  }

    //  return false;
    //}
  }
}
