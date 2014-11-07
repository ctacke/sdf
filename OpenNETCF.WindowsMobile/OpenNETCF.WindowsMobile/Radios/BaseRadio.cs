using System;

namespace OpenNETCF.WindowsMobile
{
  public abstract class BaseRadio : IRadio
  {
    private RadioState m_state;
    private Radios m_parent;

    public abstract RadioType RadioType { get; }

    internal BaseRadio(RadioDevice device, Radios parent)
    {
      DeviceName = device.DeviceName;
      DisplayName = device.DisplayName;
      m_state = device.ActualState ? RadioState.On : RadioState.Off;
      m_parent = parent;
    }

    public RadioState RadioState
    {
      get { return m_state; }
      set { m_parent.SetState(this, value); }
    }

    public string DeviceName { get; private set; }
    public string DisplayName { get; private set; }
  }
}
