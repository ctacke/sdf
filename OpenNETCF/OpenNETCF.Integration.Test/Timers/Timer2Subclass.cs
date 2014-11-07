using System;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Timers;

namespace OpenNETCF.Integration.Test
{
  internal class Timer2Subclass : Timer2
  {
    public bool CallbackHasBeenCalled { get; set; }

    public Timer2Subclass(int interval)
      : base(interval)
    {
    }

    public override void TimerCallback()
    {
      CallbackHasBeenCalled = true;
    }
  }
}
