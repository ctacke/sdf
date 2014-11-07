using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Net.NetworkInformation;

namespace WiFinder
{
  public interface IAPView
  {
    IAPPresenter Presenter { get; set; }

    void OnInterfaceGained();
    void OnInterfaceLost();
    void OnInterfaceStatusChange(InterfaceStatus newStatus);
    void RemoveNearbyAP(AccessPoint ap);
    void AddNearbyAP(AccessPoint ap);
    void OnAPInfoChange(AccessPoint ap);

    void OnAttachedAPChange(string apName, string apMAC);
  }
}
