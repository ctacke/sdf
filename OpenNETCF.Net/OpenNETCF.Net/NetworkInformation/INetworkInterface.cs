using System;

namespace OpenNETCF.Net.NetworkInformation
{
  public interface INetworkInterface
  {
    void Bind();
    System.Net.IPAddress CurrentIpAddress { get; set; }
    System.Net.IPAddress CurrentSubnetMask { get; set; }
    string Description { get; }
    DateTime DhcpLeaseExpires { get; }
    DateTime DhcpLeaseObtained { get; }
    void DhcpRelease();
    void DhcpRenew();
    OpenNETCF.Net.NetworkInformation.IPInterfaceProperties GetIPProperties();
    OpenNETCF.Net.NetworkInformation.IPv4InterfaceStatistics GetIPv4Statistics();
    OpenNETCF.Net.NetworkInformation.PhysicalAddress GetPhysicalAddress();
    string Id { get; }
    OpenNETCF.Net.NetworkInformation.InterfaceOperationalStatus InterfaceOperationalStatus { get; }
    string Name { get; }
    OpenNETCF.Net.NetworkInformation.NetworkInterfaceType NetworkInterfaceType { get; }
    OpenNETCF.Net.NetworkInformation.OperationalStatus OperationalStatus { get; }
    void Rebind();
    void Refresh();
    int Speed { get; }
    string ToString();
    void Unbind();
  }
}
