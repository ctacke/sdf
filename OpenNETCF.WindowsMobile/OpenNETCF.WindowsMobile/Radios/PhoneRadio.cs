using System;

namespace OpenNETCF.WindowsMobile
{
  public sealed class PhoneRadio : BaseRadio
  {
    internal PhoneRadio(RadioDevice device, Radios parent)
      : base(device, parent)
    {
    }

    public override RadioType RadioType
    {
      get { return RadioType.Phone; }
    }
  }
}
