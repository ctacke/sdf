using System;
using System.Collections;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// Encapsulates a non-WZC wireless network interface.  
	/// </summary>
	public class WirelessNetworkInterface : NetworkInterface, IWirelessNetworkInterface
	{
		private NICStatistics nicStat;
		bool nicStatFilled = false;

		internal WirelessNetworkInterface(int index, string name)
			: base(index, name)
		{
			nicStat = new NICStatistics();
		}

		// Class finalizer
		~WirelessNetworkInterface()
		{
			nicStat.Dispose();
		}

		/// <summary>
		/// Attempts to connect to any available preferred access point
		/// </summary>
		public virtual void Connect()
		{
			Connect("");
		}

		/// <summary>
		/// Attempts to connect to a specific Access Point
		/// </summary>
		/// <param name="SSID">The Service Set Identified or the Access Point to which a connection should be made</param>
		public virtual void Connect(string SSID)
		{
			if (SSID.Length > 32)
			{
				throw new ArgumentException("SSID Max Length is 32 characters");
			}

			if (!NDISUIO.SetOID(NDIS_OID.SSID, this.Name, Encoding.ASCII.GetBytes(SSID)))
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Returns the currently-attached SSID for RF
		/// Ethernet adapters.
		/// </summary>
		/// <returns>
		/// Instance of SSID class (or null if not associated).
		/// </returns>
		public unsafe virtual string AssociatedAccessPoint
		{
			get
			{
				string ssid = "";

				// PGT: Check for LINK status.  No LINK, no association, regardless
				// what the wireless card driver says.  The way Microsoft wrote the
				// WZC docs, the adapter can return the last associated SSID value,
				// if it wants to.

				nicStat.DeviceName = this.Name;
				nicStatFilled = false;

				try
				{
					NDISUIO.IoControl(NDISUIO.IOCTL_NDISUIO_NIC_STATISTICS,
						null, nicStat);
				}
				catch
				{
					// no associated AP
					return null;
				}



				// Check the MediaState field.  If it is MEDIA_STATE_CONNECTED,
				// add one link.
				if (nicStat.MediaState == NDISMediaStates.MEDIA_STATE_CONNECTED)
				{
					// We are linked, more or less (the actual state for each
					// given state depends on the type of association going on).

					byte[] data = null;
					try
					{
						data = NDISUIO.QueryOID(NDIS_OID.SSID, this.Name);
					}
					catch
					{
						// no associated AP
						return null;
					}

					int len = BitConverter.ToInt32(data, 0);
					if (len > 0)
					{
						ssid = System.Text.Encoding.ASCII.GetString(data, 4, len);
					}

					nicStatFilled = true;

					return ssid;
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Returns the strength of the RF Ethernet signal
		/// being received by the adapter, in dB.
		/// </summary>
		/// <returns>
		/// integer strength in dB; zero, if adapter is not
		/// an RF adapter or an error occurred
		/// </returns>
		private unsafe int SignalStrengthInDecibels
		{
			get
			{
				int db = 0;

				if (System.Environment.OSVersion.Version.Major < 4)
				{
					throw new NotSupportedException("CE 3.0 and earlier not supported");
				}

				byte[] data = NDISUIO.QueryOID(NDIS_OID.RSSI, this.Name);
				if (data.Length > 0)
				{
					db = BitConverter.ToInt32(data, 0);
				}

				return db;
			}
		}

		private NICStatistics CurrentNICStats
		{
			get
			{
				if (nicStatFilled)
					return nicStat;
				else
				{
					try
					{
						nicStat.DeviceName = this.Name;
						NDISUIO.IoControl(NDISUIO.IOCTL_NDISUIO_NIC_STATISTICS,
							null, nicStat);
						nicStatFilled = true;
					}
					catch
					{
						// no associated AP
					}

					return nicStat;
				}
			}
		}

		/// <summary>
		/// Returns a SignalStrength class representing the current strength
		/// of the signal.
		/// </summary>
		/// <returns>
		///	SignalStrength
		/// </returns>
		public virtual SignalStrength SignalStrength
		{
			get
			{
				return (new SignalStrength(this.SignalStrengthInDecibels));
			}
		}

		/// <summary>
		/// Sets or gets how this adapter connects to the network. Setting this will also reset 
		/// the network association algorithm.
		/// </summary>
		public virtual InfrastructureMode InfrastructureMode
		{
			get
			{
				byte[] data = NDISUIO.QueryOID(NDIS_OID.INFRASTRUCTURE_MODE, this.Name);
				return (InfrastructureMode)BitConverter.ToInt32(data, 0);
			}
			set
			{
				if (!NDISUIO.SetOID(NDIS_OID.INFRASTRUCTURE_MODE, this.Name, BitConverter.GetBytes((uint)value)))
				{
					throw new NotSupportedException();
				}
			}
		}

		/// <summary>
		/// Sets the IEEE 802.11 authentication mode.
		/// </summary>
		public virtual AuthenticationMode AuthenticationMode
		{
			get
			{
				byte[] data = NDISUIO.QueryOID(NDIS_OID.AUTHENTICATION_MODE, this.Name);
				return (AuthenticationMode)BitConverter.ToInt32(data, 0);
			}
			set
			{
				if (!NDISUIO.SetOID(NDIS_OID.AUTHENTICATION_MODE, this.Name, BitConverter.GetBytes((uint)value)))
				{
					throw new NotSupportedException();
				}
			}
		}

		/// <summary>
		/// Gets the MAC address of the currently associated Access point
		/// </summary>
		public PhysicalAddress AssociatedAccessPointMAC
		{
			get
			{
				byte[] data = null;

				try
				{
					data = NDISUIO.QueryOID(NDIS_OID.BSSID, this.Name);
				}
				catch
				{
					// this happens if there is no associated AP
					return PhysicalAddress.None;
				}

				if (data == null)
					return PhysicalAddress.None;

				if (data.Length > 6)
				{
					byte[] shortenedData = new byte[6];
					Array.Copy(data, shortenedData, 6);
					return new PhysicalAddress(shortenedData);
				}

				return new PhysicalAddress(data);
			}
		}

		/// <summary>
		/// Gets the currently connected network type or sets the network type the driver should use on the next connection
		/// </summary>
		public NetworkType NetworkType
		{
			get
			{
				byte[] data = NDISUIO.QueryOID(NDIS_OID.NETWORK_TYPE_IN_USE, this.Name);
				if (data == null)
				{
					throw new NotSupportedException();
				}
				return (NetworkType)BitConverter.ToInt32(data, 0);
			}
			set
			{
				if (!NDISUIO.SetOID(NDIS_OID.NETWORK_TYPE_IN_USE, this.Name, BitConverter.GetBytes((uint)value)))
				{
					throw new NotSupportedException();
				}
			}
		}

		/// <summary>
		/// Gets an array defining the data rates, in kilobits per second (kbps), that the radio is capable of running at
		/// </summary>
		public int[] SupportedRates
		{
			get
			{
				byte[] data = NDISUIO.QueryOID(NDIS_OID.SUPPORTED_RATES, this.Name);

				if (data == null)
				{
					throw new NotSupportedException();
				}

				ArrayList list = new ArrayList(data.Length);

				for (int i = 0; i < data.Length; i++)
				{
					if (data[i] > 0)
					{
						list.Add(data[i] * 500);
					}
				}
				return (int[])list.ToArray(typeof(int)); ;
			}
		}

		/// <summary>
		/// Gets the configuration information for the radio
		/// </summary>
		public RadioConfiguration RadioConfiguration
		{
			get
			{
				byte[] data = NDISUIO.QueryOID(NDIS_OID.CONFIGURATION, this.Name);

				if (data == null)
				{
					throw new NotSupportedException();
				}

				RadioConfiguration config = new RadioConfiguration(data, 0);

				return config;
			}
		}

		/// <summary>
		/// Gets the interface's WEP status
		/// </summary>
		public virtual WEPStatus WEPStatus
		{
			get
			{
				byte[] data = NDISUIO.QueryOID(NDIS_OID.WEP_STATUS, this.Name);
				if (data == null)
				{
					throw new NotSupportedException();
				}
				return (WEPStatus)BitConverter.ToInt32(data, 0);
			}
		}

		/// <summary>
		/// This structure shows the return of a NIC statistics query through IOCTL_NDISUIO_NIC_STATISTICS.
		/// </summary>
		public virtual NDISProperties NicStatistics
		{
			get
			{
				NDISProperties props;

				props.deviceState = OpenNETCF.Enum2.GetName(typeof(NDISDeviceStates), CurrentNICStats.DeviceState.ToString());
				props.linkSpeed = System.Convert.ToInt32(CurrentNICStats.LinkSpeed);
				props.mediaState = OpenNETCF.Enum2.GetName(typeof(NDISMediaStates), CurrentNICStats.MediaState);
				props.mediaType = OpenNETCF.Enum2.GetName(typeof(NDISMediaTypes), CurrentNICStats.MediaType);
				props.physicalMediaType = OpenNETCF.Enum2.GetName(typeof(NDISPhysicalMedium), CurrentNICStats.PhysicalMediaType);

				return props;
			}
		}

	}


}