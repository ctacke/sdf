
using System;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// Specifies types of network interfaces.
	/// </summary>
	public enum NetworkInterfaceType
	{
        /// <summary>
        /// The network interface uses an Asymmetric Digital Subscriber Line (ADSL). 
        /// </summary>
		AsymmetricDsl				= 0x5e,
        /// <summary>
        /// The network interface uses asynchronous transfer mode (ATM) for data transmission
        /// </summary>
		Atm							= 0x25,
        /// <summary>
        /// The network interface uses a basic rate interface Integrated Services Digital Network (ISDN) connection. ISDN is a set of standards for data transmission over telephone lines
        /// </summary>
		BasicIsdn					= 20,
        /// <summary>
        /// The network interface uses an Ethernet connection. Ethernet is defined in IEEE standard 802.3
        /// </summary>
		Ethernet					= 0x06,
        /// <summary>
        /// #Mb Ethernet
        /// </summary>
		Ethernet3MegaBit			= 0x1a,
        /// <summary>
        /// The network interface uses a Fast Ethernet connection over optical fiber. This type of connection is also known as 100BaseFX
        /// </summary>
		FastEthernetFx				= 0x45,
        /// <summary>
        /// The network interface uses a Fast Ethernet connection. Fast Ethernet provides a data rate of 100 megabits per second, known as 100BaseT
        /// </summary>
		FastEthernetT				= 0x3e,
        /// <summary>
        /// The network interface uses a Fiber Distributed Data Interface (FDDI) connection. FDDI is a set of standards for data transmission on fiber optic lines in a local area network
        /// </summary>
		Fddi						= 15,
        /// <summary>
        /// The network interface uses a modem
        /// </summary>
		GenericModem				= 0x30,
        /// <summary>
        /// 
        /// </summary>
		GigaBitEthernet				= 0x75,
        /// <summary>
        /// The network interface uses a High Performance Serial Bus
        /// </summary>
		HighPerformanceSerialBus	= 0x90,
        /// <summary>
        /// The network interface uses Internet Protocol (IP) in combination with asynchronous transfer mode (ATM) for data transmission
        /// </summary>
		IPOverAtm					= 0x72,
        /// <summary>
        /// The network interface uses a connection configured for ISDN and the X.25 protocol. X.25 allows computers on public networks to communicate using an intermediary computer
        /// </summary>
		Isdn						= 0x3f,
        /// <summary>
        /// The network interface is a loopback adapter. Such interfaces are used primarily for testing; no traffic is sent
        /// </summary>
		Loopback					= 0x18,
        /// <summary>
        /// The network interface uses a Multirate Digital Subscriber Line
        /// </summary>
		MultiRateSymmetricDsl		= 0x8f,
        /// <summary>
        /// The network interface uses a Point-To-Point protocol (PPP) connection. PPP is a protocol for data transmission using a serial device
        /// </summary>
		Ppp							= 0x17,
        /// <summary>
        /// The network interface uses a primary rate interface Integrated Services Digital Network (ISDN) connection. ISDN is a set of standards for data transmission over telephone lines
        /// </summary>
		PrimaryIsdn					= 0x15,
        /// <summary>
        /// The network interface uses a Rate Adaptive Digital Subscriber Line (ADSL). 
        /// </summary>
		RateAdaptDsl				= 0x5f,
        /// <summary>
        /// The network interface uses a Serial Line Internet Protocol (SLIP) connection. SLIP is defined in IETF RFC 1055
        /// </summary>
		Slip						= 0x1c,
        /// <summary>
        /// The network interface uses a Symmetric Digital Subscriber Line (SDSL). 
        /// </summary>
		SymmetricDsl				= 0x60,
        /// <summary>
        /// The network interface uses a Token-Ring connection. Token-Ring is defined in IEEE standard 802.5
        /// </summary>
		TokenRing					= 0x09,
        /// <summary>
        /// The network interface uses a tunnel connection
        /// </summary>
		Tunnel						= 0x83,
        /// <summary>
        /// The interface type is not known
        /// </summary>
		Unknown						= 0x01,
        /// <summary>
        /// The network interface uses a Very High Data Rate Digital Subscriber Line (VDSL). 
        /// </summary>
		VeryHighSpeedDsl			= 0x61,
        /// <summary>
        /// The network interface uses a wireless LAN connection (IEEE 802.11 standard). 
        /// </summary>
		Wireless80211				= 0x47
	}
}
