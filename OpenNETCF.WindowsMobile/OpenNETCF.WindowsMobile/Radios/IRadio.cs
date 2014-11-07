using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.WindowsMobile
{
  public interface IRadio
  {
    RadioType RadioType { get; }
    RadioState RadioState { get; set; }
    string DeviceName { get; }
    string DisplayName { get; }
  }
}
