using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Net.NetworkInformation;

namespace WiFinder
{
  public interface IAPPresenter : IDisposable
  {
    IWirelessNetworkInterface WirelessInterface { get; set; }
    void Initialize(IAPView view);
    IAPView View { get; }
    bool APStateMachineEnabled { get; set; }
    bool IsPreferredAP(AccessPoint ap);
  }
}
