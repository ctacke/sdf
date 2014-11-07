using System;

namespace OpenNETCF.WindowsMobile
{
  public sealed class WiFiRadio : BaseRadio
  {
    internal WiFiRadio(RadioDevice device, Radios parent)
      : base(device, parent)
    {
    }

    public override RadioType RadioType
    {
      get { return RadioType.WiFi; }
    }
  }
}
