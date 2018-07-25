using System;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Different NIC adapter types
	/// </summary>
	public enum AdapterType
	{
		/// <summary>
        /// Unknown or unresolvable adapter type
        /// </summary>
        Unknown = 0,

		/// <summary>
		/// Adapter type not known at compile time
		/// </summary>
		Other = 1,

		/// <summary>
		/// Ethernet adapter.  Also includes RF Ethernet
		/// </summary>
		Ethernet = 6,

		/// <summary>
		/// Token ring adapter
		/// </summary>
		TokenRing = 9,

		/// <summary>
		/// Fiber optic adapter
		/// </summary>
		FDDI = 15,

		/// <summary>
		/// Dial-up/serial adapter using PPP protocol
		/// </summary>
		PPP = 23,

		/// <summary>
		/// Loopback adapter
		/// </summary>
		Loopback = 24,

		/// <summary>
		/// Dial-up/serial adapter using SLIP protocol
		/// </summary>
		SLIP = 28,
	}
}
