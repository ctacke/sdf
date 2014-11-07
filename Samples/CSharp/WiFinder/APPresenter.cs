using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Net.NetworkInformation;
using System.Threading;

namespace WiFinder
{
  internal class APPresenter : IAPPresenter
  {
    private NetworkInterfaceWatcher m_interfaceWatcher;
    private IAdapterStateMachine m_stateMachine;

    public IWirelessNetworkInterface WirelessInterface { get; set; }
    public IAPView View { get; private set; }

    public APPresenter()
    {
      // fire up a watcher to see if the NIC is powered on/off and changes availability
      m_interfaceWatcher = new NetworkInterfaceWatcher();
      m_interfaceWatcher.InterfaceGained += new InterfaceEventDelegate(m_interfaceWatcher_InterfaceGained);
      m_interfaceWatcher.InterfaceLost += new InterfaceEventDelegate(m_interfaceWatcher_InterfaceLost);
      m_interfaceWatcher.InterfaceStatusChange += new InterfaceStatusDelegate(m_interfaceWatcher_InterfaceStatusChange);
    }

    public void Initialize(IAPView view)
    {
      View = view;
      View.Presenter = this;

      // find all wireless interfaces - for now assume the first is the only one we'll deal with
      WirelessInterface = FindWirelessInterface();

      if (WirelessInterface != null)
      {
        View.OnInterfaceGained();
        
        // start up our state machine
        CreateStateMachine();
      }
    }

    public bool IsPreferredAP(AccessPoint ap)
    {
      if (WirelessInterface is WirelessZeroConfigNetworkInterface)
      {
        return (WirelessInterface as WirelessZeroConfigNetworkInterface).PreferredAccessPoints.Contains(ap);
      }
      else
      {
        // no access to the preferred list on non-WZC interfaces
        return false;
      }
    }

    private void CreateStateMachine()
    {
      if(WirelessInterface is WirelessZeroConfigNetworkInterface)
      {
        m_stateMachine = new WZCAdapterStateMachine(WirelessInterface as WirelessZeroConfigNetworkInterface);
      }
      else
      {
        m_stateMachine = new WirelessAdapterStateMachine(WirelessInterface);
      }

      m_stateMachine.AttachedAPChanged += new AttachedAPChangeHandler(m_stateMachine_AttachedAPChanged);
      m_stateMachine.APInRange += new APChangeHandler(m_stateMachine_APInRange);
      m_stateMachine.APOutOfRange += new APChangeHandler(m_stateMachine_APOutOfRange);
      m_stateMachine.APInfoChange += new APChangeHandler(m_stateMachine_APInfoChange);
      m_stateMachine.Start();
    }

    void m_stateMachine_APInfoChange(AccessPoint ap)
    {
      View.OnAPInfoChange(ap);
    }

    void m_stateMachine_APOutOfRange(OpenNETCF.Net.NetworkInformation.AccessPoint ap)
    {
      View.RemoveNearbyAP(ap);
    }

    void m_stateMachine_APInRange(OpenNETCF.Net.NetworkInformation.AccessPoint ap)
    {
      View.AddNearbyAP(ap);
    }

    void m_stateMachine_AttachedAPChanged(string apName, string apMAC)
    {
      View.OnAttachedAPChange(apName, apMAC);
    }

    public void Dispose()
    {
      m_interfaceWatcher.Dispose();

      if (m_stateMachine != null)
      {
        m_stateMachine.Stop();
      }
    }

    private IWirelessNetworkInterface FindWirelessInterface()
    {
      foreach (INetworkInterface intf in NetworkInterface.GetAllNetworkInterfaces())
      {
        if ((intf is WirelessNetworkInterface) || (intf is WirelessZeroConfigNetworkInterface))
        {
          return intf as IWirelessNetworkInterface;
        }
      }

      return null;
    }

    public bool APStateMachineEnabled 
    {
      get { return m_stateMachine.Enabled; }
      set { m_stateMachine.Enabled = value; }
    }

    void m_interfaceWatcher_InterfaceStatusChange(string interfaceName, InterfaceStatus newStatus)
    {
      if (
        (WirelessInterface != null) &&
        (interfaceName == WirelessInterface.Name))
      {
        View.OnInterfaceStatusChange(newStatus);
      }
    }

    void m_interfaceWatcher_InterfaceLost(string interfaceName)
    {
      if (
        (WirelessInterface != null) && 
        (interfaceName == WirelessInterface.Name))
      {
        View.OnInterfaceLost();
      }
    }

    void m_interfaceWatcher_InterfaceGained(string interfaceName)
    {
      if(WirelessInterface == null)
      {
        // see if the new interface is wireless
        WirelessInterface = FindWirelessInterface();
      }
      
      if((WirelessInterface != null) &&
        (interfaceName == WirelessInterface.Name))
      {
        View.OnInterfaceGained();
      }
    }
  }
}
