using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Net.NetworkInformation;

namespace WiFinder
{
  internal class WZCAdapterStateMachine : WirelessAdapterStateMachine
  {
    private List<AccessPoint> m_nearbyAPs;

    public WirelessZeroConfigNetworkInterface WZCInterface
    {
      get { return WirelessInterface as WirelessZeroConfigNetworkInterface; }
    }

    public WZCAdapterStateMachine(WirelessZeroConfigNetworkInterface intf)
      : base(intf)
    {
    }

    public override void StateFunction()
    {
      this.Enabled = false;

      base.StateFunction();

      // WZC-specific stuff

      if (m_nearbyAPs == null)
      {
        m_nearbyAPs = new List<AccessPoint>();
        m_nearbyAPs.AddRange(WZCInterface.NearbyAccessPoints);

        foreach (AccessPoint ap in m_nearbyAPs)
        {
          RaiseAPInRange(ap);
        }
      }
      else
      {
        AccessPointCollection testList = WZCInterface.NearbyAccessPoints;

        // check for new APs
        foreach (AccessPoint ap in testList)
        {
          if (!m_nearbyAPs.Contains(ap))
          {
            m_nearbyAPs.Add(ap);
            RaiseAPInRange(ap);
          }
          else
          {
            // for now assume that the signal has probably changed, so every tick of the state machine will cause a refresh of the AP list
            RaiseAPInfoChange(ap);
          }
        }

        // check for lost APs
        foreach (AccessPoint ap in m_nearbyAPs)
        {
          if (!testList.Contains(ap))
          {
            m_nearbyAPs.Remove(ap);
            RaiseAPOutOfRange(ap);
          }
        }
      }

      this.Enabled = true;
    }
  }
}
