﻿using System;

namespace ConsoleApp1.PurelyFunctional
{
  public class Gift : IMoney
  {
    public Amount Value { get; }
    public DateTime ValidBefore { get; }

    public Gift(Amount value, Date validBefore)
    {
      this.Value = value;
      this.ValidBefore = validBefore;
    }
  }
}
