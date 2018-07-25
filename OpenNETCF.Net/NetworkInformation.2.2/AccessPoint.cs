using System;
using System.Collections;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// The SSID class represents a single SSID value which
	/// an adapter might be receiving data from.  It can be
	/// queried for SSID-specific information for the
	/// associated adapter such as signal strength.
	/// </summary>
	public class AccessPoint
	{

        private int m_channel;

        public int Channel
        {
            get { return m_channel; }
        }

        internal AccessPoint(BSSID bssid)
        {
            name = bssid.SSID;
            macaddr = bssid.MacAddress;
            privacy = bssid.Privacy;

            m_channel = bssid.Configuration.Frequency;
            if (m_channel > 14)
            {
                m_channel = (m_channel - 2407000) / 5000;
            }

            // see if the rssi is in the HIWORD or LOWORD
            uint ssi = (uint)bssid.Rssi;
            if (((ssi & 0xFFFF0000) > 0) && ((ssi & 0xffff) == 0))
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
        public PhysicalAddress PhysicalAddress
		{
            get { return new PhysicalAddress(macaddr); }
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
		internal int SignalStrengthInDecibels
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
        /// An array of supported rates in kilobits per second
		/// </returns>
		public int[] SupportedRates
		{
			get
			{
                ArrayList list = new ArrayList(supportedrates.Length);

                for (int i = 0; i < supportedrates.Length; i++)
                {
                    if (supportedrates[i] > 0)
                    {
                        list.Add(supportedrates[i] * 500);
                    }
                }
                return (int[])list.ToArray(typeof(int)); ;
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
