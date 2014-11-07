using System;

namespace OpenNETCF.WindowsMobile
{
  public sealed class BluetoothRadio : BaseRadio
  {
    internal BluetoothRadio(RadioDevice device, Radios parent)
      : base(device, parent)
    {
    }

    public override RadioType RadioType
    {
      get { return RadioType.Bluetooth; }
    }
  }
}
