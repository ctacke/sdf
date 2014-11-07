using System;
namespace OpenNETCF.Net.NetworkInformation
{
  public interface IWirelessNetworkInterface : INetworkInterface
	{
		string AssociatedAccessPoint { get; }
		PhysicalAddress AssociatedAccessPointMAC { get; }
		AuthenticationMode AuthenticationMode { get; set; }
		void Connect();
		void Connect(string SSID);
		InfrastructureMode InfrastructureMode { get; set; }
		NetworkType NetworkType { get; set; }
		RadioConfiguration RadioConfiguration { get; }
		SignalStrength SignalStrength { get; }
		int[] SupportedRates { get; }
		WEPStatus WEPStatus { get; }

		NDISProperties NicStatistics { get; }
	}
}
