using System;

namespace ConsoleApp1.PurelyFunctional
{
  /// <summary>
  /// Restricts the DateTime to a Date, so the time part is set to 0
  /// </summary>
  public class Date
  {
    private DateTime Value { get; }

    public Date(int year, int month, int day)
    {
      Value = new DateTime(year, month, day);
    }

    public static implicit operator DateTime(Date date) => date.Value;

    public static implicit operator Date(DateTime dateTime) => new Date(dateTime.Year, dateTime.Month, dateTime.Day);
  }
}
