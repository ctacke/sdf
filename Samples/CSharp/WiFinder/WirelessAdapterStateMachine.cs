using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Timers;
using OpenNETCF.Net.NetworkInformation;

namespace WiFinder
{
  internal delegate void AttachedAPChangeHandler(string apNAme, string apMAC);
  internal delegate void APChangeHandler(AccessPoint ap);

  internal class WirelessAdapterStateMachine : Timer2, IAdapterStateMachine
  {
    public event AttachedAPChangeHandler AttachedAPChanged;
    public event APChangeHandler APInRange;
    public event APChangeHandler APOutOfRange;
    public event APChangeHandler APInfoChange;

    public IWirelessNetworkInterface WirelessInterface { get; private set; }

    private PhysicalAddress m_attachedAP;

    public WirelessAdapterStateMachine(IWirelessNetworkInterface intf)
      : base(2000)
    {
      WirelessInterface = intf;

      this.UseCallback = true;
      this.AutoReset = true;
    }

    protected void RaiseAPInRange(AccessPoint ap)
    {
      if (APInRange != null)
      {
        APInRange(ap);
      }
    }

    protected void RaiseAPOutOfRange(AccessPoint ap)
    {
      if (APOutOfRange != null)
      {
        APOutOfRange(ap);
      }
    }

    protected void RaiseAPInfoChange(AccessPoint ap)
    {
      if (APInfoChange != null)
      {
        APInfoChange(ap);
      }
    }

    public override void TimerCallback()
    {
      StateFunction();
    }

    public virtual void StateFunction()
    {
      this.Enabled = false;

      try
      {
        // Wireless and WZC-common stuff
        if ((m_attachedAP == null) || (!m_attachedAP.Equals(WirelessInterface.AssociatedAccessPointMAC)))
        {
          m_attachedAP = WirelessInterface.AssociatedAccessPointMAC;

          if (AttachedAPChanged != null)
          {
            if ((WirelessInterface.AssociatedAccessPoint == null) || (WirelessInterface.AssociatedAccessPointMAC == null))
            {
              AttachedAPChanged(null, null);
            }
            else
            {
              AttachedAPChanged(WirelessInterface.AssociatedAccessPoint, WirelessInterface.AssociatedAccessPointMAC.ToString());
            }
          }
        }
      }
      finally
      {
        this.Enabled = true;
      }
    }
  }
}
