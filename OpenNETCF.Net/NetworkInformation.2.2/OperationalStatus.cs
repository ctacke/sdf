using System;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// Specifies the operational state of a network interface.
	/// </summary>
	public enum OperationalStatus
	{
		/// <summary>
		/// The network interface is not in a condition to transmit data packets; it is waiting for an external event.
		/// </summary>
		Dormant			= 5,
		/// <summary>
		/// The network interface is unable to transmit data packets.
		/// </summary>
		Down			= 2,
		/// <summary>
		/// The network interface is unable to transmit data packets because it runs on top of one or more other interfaces, and at least one of these "lower layer" interfaces is down.
		/// </summary>
		LowerLayerDown	= 7,
		/// <summary>
		/// The network interface is unable to transmit data packets because of a missing component, typically a hardware component.
		/// </summary>
		NotPresent		= 6,
		/// <summary>
		/// The network interface is running tests.
		/// </summary>
		Testing			= 3,
		/// <summary>
		/// The network interface status is not known.
		/// </summary>
		Unknown			= 4,
		/// <summary>
		/// The network interface is up; it can transmit data packets.
		/// </summary>
		Up				= 1
	}
}
