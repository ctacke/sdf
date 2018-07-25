using System;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// Provides information about network interfaces that support Internet Protocol version 4 (IPv4) or Internet Protocol version 6 (IPv6).
	/// </summary>
	public abstract class IPInterfaceProperties
	{
        /// <summary>
        /// Default class contructor
        /// </summary>
		protected IPInterfaceProperties()
		{
		}

		/// <summary>
		/// Provides Internet Protocol (IP) statistical data for the network interface.
		/// </summary>
		/// <returns>An <see cref="IPInterfaceStatistics"/> object that provides traffic statistics for this network interface.</returns>
		public abstract IPInterfaceStatistics GetIPInterfaceStatistics();
	}
}
