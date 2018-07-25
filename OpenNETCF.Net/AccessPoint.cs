using System;

namespace OpenNETCF.Net
{
	/// <summary>
	/// The SSID class represents a single SSID value which
	/// an adapter might be receiving data from.  It can be
	/// queried for SSID-specific information for the
	/// associated adapter such as signal strength.
	/// </summary>
    [Obsolete("This class is obsolete and will be removed in a future version of the SDF.  Consider using OpenNETCF.Net.NetworkInformation.AccessPoint instead", false)]
	public class AccessPoint
	{
		internal AccessPoint( BSSID bssid )
		{
			name = bssid.SSID;
			macaddr = bssid.MacAddress;
			privacy = bssid.Privacy;

			// see if the rssi is in the HIWORD or LOWORD
			uint ssi = (uint)bssid.Rssi;
			if(((ssi & 0xFFFF0000) > 0) && ((ssi & 0xffff) == 0 ))
			{
				// hiword
				rssi = bssid.Rssi >> 16;
			}
			else
			{
				// loword
				rssi = bssid.Rssi;
			}

			supportedrates = bssid.SupportedRates;
			networktypeinuse = bssid.NetworkTypeInUse;
			infrastructuremode = bssid.InfrastructureMode;

			//jsm - Bug 121 - Enhancement to support Channel information per AP
			channel = bssid.Channel;

			// ToDo: For now, the configuration is not returned.
			//			configuration = bssid.Configuration;
		}

		internal String name;
		/// <summary>
		/// The SSID name string.
		/// </summary>
		public String Name
		{
			get { return name; }
		}

		internal byte[] macaddr;
		/// <summary>
		/// The hardware address for the network adapter.
		/// </summary>
		public byte[] MacAddress
		{
			get { return macaddr; }
		}

		internal int privacy;
		/// <summary>
		/// The privacy mask for the adapter.
		/// </summary>
		public int Privacy
		{
			get { return privacy; }
		}

		internal int rssi;

		/// <summary>
		/// Returns the strength of the RF Ethernet signal
		/// being received by the adapter for the SSID, in dB.
		/// </summary>
		/// <returns>
		/// integer strength in dB; zero, if adapter is not
		/// an RF adapter or an error occurred
		/// </returns>
		public int SignalStrengthInDecibels
		{
			get
			{
				return rssi;
			}
		}

		/// <summary>
		/// Returns the strength of the RF Ethernet signal
		/// being received by the adapter for the SSID, in dB.
		/// </summary>
		/// <returns>
		/// SignalStrength instance containing the strength
		/// </returns>
		public SignalStrength SignalStrength
		{
			get
			{
				return new SignalStrength( rssi );
			}
		}

		internal byte[] supportedrates;
		/// <summary>
		/// Returns the list of supported signaling rates for
		/// the adapter.  Each value indicates a single rate.
		/// </summary>
		/// <returns>
		/// array of bytes, each of which represents a rate.
		/// The units are 0.5Mbps.  Rates that belong to the
		/// 'basic rate set' have their high bits set to 1
		/// (they are OR-ed with 0x80).  Rates which are not
		/// in the basic rate set, have this bit clear.
		/// So, a value of 0x96, after clearing the
		/// high bit, is 0x16 or 22d.  Multiplying by 0.5Mbps 
		/// gives a rate of 11Mbps.  Since the high bit was
		/// set, this rate is in the basic rate set.
		/// </returns>
		public byte[] SupportedRates
		{
			get
			{
				return supportedrates;
			}
		}

		internal NetworkType networktypeinuse;
		/// <summary>
		/// Returns the current type of network in use in
		/// the form of an element of the 
		/// Ndis80211NetworkType enumeration.
		/// </summary>
		/// <returns>
		/// Ndis80211NetworkType network type
		/// </returns>
		public NetworkType NetworkTypeInUse
		{
			get
			{
				return networktypeinuse;
			}
		}

		internal InfrastructureMode infrastructuremode;
		/// <summary>
		/// Returns the current infrastructure in use by the
		/// adapter.
		/// </summary>
		/// <returns>
		/// Ndis80211NetworkInfrastructure type
		/// </returns>
		public InfrastructureMode InfrastructureMode
		{
			get
			{
				return infrastructuremode;
			}
		}

		internal int channel;
		/// <summary>
		/// Returns the active channel of the RF Ethernet signal
		/// being received by the adapter for the SSID
		/// </summary>
		/// <returns>
		/// Active channel of AP
		/// </returns>
		public int Channel
		{
			get
			{
				return channel;
			}
		}

		/// <summary>
		/// Return the name of the AccessPoint
		/// </summary>
		/// <returns>
		/// string name of the access point
		/// </returns>
		public override string ToString()
		{
			return this.Name;
		}
	}
}
