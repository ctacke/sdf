using System;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// Provides configuration and statistical information for a network interface.
	/// </summary>
	public abstract class NetworkInterface
	{
		protected NetworkInterface()
		{
			
		}

		/// <summary>
		/// Returns objects that describe the network interfaces on the local computer.
		/// </summary>
		/// <returns></returns>
		public static NetworkInterface[] GetAllNetworkInterfaces()
		{
			return SystemNetworkInterface.GetNetworkInterfaces();
		}

		/// <summary>
		/// Returns an object that describes the configuration of this network interface.
		/// </summary>
		/// <returns></returns>
		public abstract IPInterfaceProperties GetIPInterfaceProperties();

//		// Properties
//		public abstract string Description { get; }
//		public abstract bool IsReceiveOnly { get; }
//		public abstract string Name { get; }
//		public abstract OperationalStatus OperationalStatus { get; }
//		public abstract long Speed { get; }
//		public abstract bool SupportsMulticast { get; }
//		public abstract InterfaceType Type { get; }
	}
}
