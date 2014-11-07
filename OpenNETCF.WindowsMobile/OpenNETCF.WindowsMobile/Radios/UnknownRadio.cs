using System;

namespace OpenNETCF.WindowsMobile
{
  public sealed class UnknownRadio : BaseRadio
  {
    internal UnknownRadio(RadioDevice device, Radios parent)
      : base(device, parent)
    {
    }

    public override RadioType RadioType
    {
      get { return RadioType.Unknown; }
    }
  }
}
