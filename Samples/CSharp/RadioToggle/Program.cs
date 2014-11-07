using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.WindowsMobile;
using System.Threading;
using System.Diagnostics;

namespace RadioToggle
{
  class Program
  {
    static void Main(string[] args)
    {
      Radios radios = Radios.GetRadios();

      Debug.WriteLine("\nBefore\r\n--------");
      foreach (IRadio radio in radios)
      {
        Debug.WriteLine(string.Format("Name: {0}, Type: {1}, State: {2}", radio.DeviceName, radio.RadioType.ToString(), radio.RadioState.ToString()));

        // toggle all radio states
        radio.RadioState = (radio.RadioState == RadioState.On) ? RadioState.Off : RadioState.On;
      }

      // give the radios enough time to change state - some (like BT) seem to be slow
      Thread.Sleep(1000);

      radios.Refresh();

      // display again
      Debug.WriteLine("\r\nAfter\r\n--------");
      foreach (IRadio radio in radios)
      {
        Debug.WriteLine(string.Format("Name: {0}, Type: {1}, State: {2}", radio.DeviceName, radio.RadioType.ToString(), radio.RadioState.ToString()));
      }
      Debug.WriteLine("\r\n\n");
      Thread.Sleep(100);
    }
  }
}
