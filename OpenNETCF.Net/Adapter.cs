using System;
using System.Data;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Microsoft.Win32;
using OpenNETCF.IO;
using OpenNETCF.Runtime.InteropServices;
using System.IO;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Class representing a single instance of a network
	/// adapter, which might include PCMCIA cards, USB network
	/// cards, built-in Ethernet chips, etc.
	/// </summary>
    [Obsolete("This class is obsolete and will be removed in a future version of the SDF.  Consider using OpenNETCF.Net.NetworkInformation.NetworkInterface or one of its subclasses instead", false)]
    public class Adapter : StreamInterfaceDriver
	{
		private const string DEVICE_NAME = "UIO1:";

		private void Open()
		{
            base.Open(FileAccess.ReadWrite, FileShare.None);  
		}

		internal String	name;
		/// <summary>
		/// The NDIS/driver assigned adapter name.
		/// </summary>
		public String	Name
		{
			get { return name; }
		}
		internal String	description;
		/// <summary>
		/// The descriptive name of the adapter.
		/// </summary>
		public String	Description
		{
			get { return description; }
		}
		internal int	index;
		/// <summary>
		/// The index in NDIS' list of adapters where this
		/// adapter is found.
		/// </summary>
		public int	Index
		{
			get { return index; }
		}
		internal int	type;
		/// <summary>
		/// The adapter type.  Adapters can be standard
		/// Ethernet, RF Ethernet, loopback, dial-up, etc.
		/// </summary>
		public AdapterType	Type
		{
			get { return (AdapterType)type; }
		}
		internal byte[]	hwaddress;
		/// <summary>
		/// The hardware address associated with the adapter.
		/// For Ethernet-based adapters, including RF Ethernet
		/// adapters, this is the Ethernet address.
		/// </summary>
		public byte[]	MacAddress
		{
			get { return hwaddress; }
		}

		internal bool	dhcpenabled;
		/// <summary>
		/// Indicator of whether DHCP (for IP address 
		/// assignment from a server), is enabled for the
		/// adapter.
		/// </summary>
		public bool	DhcpEnabled
		{
			get { return dhcpenabled; }
			set
			{
				// Update the local copy of the state.
				// Well, on second thought, maybe we should have to
				// be refreshed to get this updated.
//				dhcpenabled = value;

				// Modify the registry keys associated with this
				// adapter to enable DHCP.  Once that is done, we
				// have to rebind the network adapter to actually
				// make the change from static to DHCP.  We try to
				// only do this if there has been a change.
				string	regName = "\\comm\\" + this.Name + "\\Parms\\Tcpip";

				// Open the base key for the adapter.
				RegistryKey tcpipkey = Registry.LocalMachine.OpenSubKey( regName, true );

				// Get the current value of the DHCPEnabled value.  If
				// it already matches the value we're trying to set, we
				// don't have to change it.
                object o = tcpipkey.GetValue("EnableDHCP");
                bool oldVal = value;
                if (o != null)
                {
                    oldVal = (int)o == 1;
                }

				if ( oldVal != value )
				{
					// Make the change.
					tcpipkey.SetValue("EnableDHCP", value ? 1 : 0 );
				}

				tcpipkey.Close();
			}
		}

		internal string	currentIp;
		/// <summary>
		/// The currently active IP address of the adapter.
		/// </summary>
		public string	CurrentIpAddress
		{
			get { return currentIp; }
			set
			{
				// Update the local copy of the state.
				// Well, on second thought, maybe we should have to
				// be refreshed to get this updated.
				//				currentIp = value;

				// Modify the registry keys associated with this
				// adapter to set the new static IP.  Once that is done, we
				// have to rebind the network adapter to actually
				// make the change.  
				string	regName = "\\comm\\" + this.Name + "\\Parms\\Tcpip";

				// Open the base key for the adapter.
				RegistryKey tcpipkey = Registry.LocalMachine.OpenSubKey( regName, true );

				// Get the current value of the IpAddress value.  If
				// it already matches the value we're trying to set, we
				// don't have to change it.
                object oldVal = tcpipkey.GetValue("IpAddress");
                string oldIP = oldVal == null ? null : ((string[])oldVal)[0];

				if ((oldIP == null) || (oldIP != value))
				{
					// Make the change.
					tcpipkey.SetValue( "IpAddress", value );
				}

				tcpipkey.Close();
			}
		}

		internal string	currentsubnet;
		/// <summary>
		/// The currently active subnet mask address of the 
		/// adapter.
		/// </summary>
		public string	CurrentSubnetMask
		{
			get { return currentsubnet; }
			set
			{
				// Update the local copy of the state.
				// Well, on second thought, maybe we should have to
				// be refreshed to get this updated.
				//				currentsubnet = value;

				// Modify the registry keys associated with this
				// adapter to set the new subnet mask.  Once that is done, we
				// have to rebind the network adapter to actually
				// make the change.  
				string	regName = "\\comm\\" + this.Name + "\\Parms\\Tcpip";

				// Open the base key for the adapter.
				RegistryKey tcpipkey = Registry.LocalMachine.OpenSubKey( regName, true );

				// Get the current value of the SubnetMask value.  If
				// it already matches the value we're trying to set, we
				// don't have to change it.
                object oldVal = tcpipkey.GetValue("SubnetMask");
                string oldMask = oldVal == null ? null : ((string[])oldVal)[0];

                if ((oldMask == null) || (oldMask != value))
                {
                    // Make the change.
                    tcpipkey.SetValue("SubnetMask", value);
                }

				tcpipkey.Close();
			}
		}

		internal string	gateway;
		/// <summary>
		/// The active gateway address.
		/// </summary>
		public string	Gateway
		{
			get { return gateway; }
			set
			{
				// Update the local copy of the state.
				// Well, on second thought, maybe we should have to
				// be refreshed to get this updated.
				//				gateway = value;

				// Modify the registry keys associated with this
				// adapter to set the new gateway.  Once that is done, we
				// have to rebind the network adapter to actually
				// make the change.  
				string	regName = "\\comm\\" + this.Name + "\\Parms\\Tcpip";

				// Open the base key for the adapter.
				RegistryKey tcpipkey = Registry.LocalMachine.OpenSubKey( regName, true );

				// Get the current value of the DefaultGateway value.  If
				// it already matches the value we're trying to set, we
				// don't have to change it.
                object oldVal = tcpipkey.GetValue("DefaultGateway");
                string oldGateway = oldVal == null ? null : ((string[])oldVal)[0];

                if ((oldGateway == null) || (oldGateway != value))
                {
                    // Make the change.
                    tcpipkey.SetValue("DefaultGateway", value);
                }

				tcpipkey.Close();
			}
		}

		internal string	dhcpserver;
		/// <summary>
		/// The DHCP server from which the IP address was
		/// last assigned.
		/// </summary>
		public string	DhcpServer
		{
			get { return dhcpserver; }
		}

		/// <summary>
        /// Gets the WEP status for the current adapter
        /// </summary>
        public WEPStatus WEPStatus
        {
            get
            {
                INTF_ENTRY entry = new INTF_ENTRY();
                entry.Guid = Name;
                INTFFlags flags = 0;
                WEPStatus status;
                try
                {
                    WZCPInvokes.WZCQueryInterface(null, INTFFlags.INTF_ALL, ref entry, out flags);
                    status = entry.nWepStatus;
                }
                finally
                {
                    WZCPInvokes.WZCDeleteIntfObj(ref entry);
                }

                return status;
            }
        }

        /// <summary>
        /// Gets the AuthenticationMode for the current adapter
        /// </summary>
        public AuthenticationMode AuthMode
        {
            get
            {                
                INTF_ENTRY entry = new INTF_ENTRY();
                entry.Guid = Name;
                INTFFlags flags = 0;
                AuthenticationMode mode;

                try
                {
                    WZCPInvokes.WZCQueryInterface(null, INTFFlags.INTF_ALL, ref entry, out flags);
                    mode = entry.nAuthMode;
                }
                finally
                {
                    WZCPInvokes.WZCDeleteIntfObj(ref entry);
                }

                return mode;
            }
        }

		/// <summary>
		/// Enables or disables WZC Fallback for the current adapter
		/// </summary>
		/// <returns>
		/// Returns true/false if WZC Fallback is enabled for the current adapter
		/// </returns>
		public bool WZCFallbackEnabled
		{
			set
			{
				if ( Type != AdapterType.Ethernet )
					return ;
				// Create the entry that will be passed to WZCSetInterface.
				INTF_ENTRY entry = new INTF_ENTRY();
				entry.Guid = Name;
				if (value == false)
					entry.dwCtlFlags &= ~(uint) INTFFlags.INTF_FALLBACK;
				else
					entry.dwCtlFlags |= (uint) INTFFlags.INTF_FALLBACK;
				int uret = WZCPInvokes.WZCSetInterface(null, INTFFlags.INTF_FALLBACK, ref entry, null);
				if ( uret > 0 )
					throw new AdapterException(uret, "WZCSetInterface");
				entry.Dispose();
			}

			get
			{
				if ( Type != AdapterType.Ethernet )
					return false;
				INTF_ENTRY entry = new INTF_ENTRY();
				entry.Guid = Name;
				INTFFlags dwOutFlags;
                try
                {
                    int uret = WZCPInvokes.WZCQueryInterface(null,
                        INTFFlags.INTF_FALLBACK, ref entry, out dwOutFlags);
                    if (uret > 0 || (entry.dwCtlFlags & (uint)INTFFlags.INTF_FALLBACK) == 0)
                        return false;
                }
                finally
                {
                    WZCPInvokes.WZCDeleteIntfObj(ref entry);
                }
				return true;
			}
		}

		internal bool	havewins;
		/// <summary>
		/// Indicates the presence of WINS server addresses
		/// for the adapter.
		/// </summary>
		public bool	HasWins
		{
			get { return havewins; }
		}

		internal string	primarywinsserver;
		/// <summary>
		/// The IP address of the primary WINS server for the
		/// adapter.
		/// </summary>
		public string	PrimaryWinsServer
		{
			get { return primarywinsserver; }
		}
		internal string	secondarywinsserver;
		/// <summary>
		/// The IP address of the secondary WINS server for the
		/// adapter.
		/// </summary>
		public string	SecondaryWinsServer
		{
			get { return secondarywinsserver; }
		}

		internal DateTime	leaseobtained;
		/// <summary>
		/// The date/time at which the IP address lease was
		/// obtained from the DHCP server.
		/// </summary>
		public DateTime	LeaseObtained
		{
			get { return leaseobtained; }
		}
		internal DateTime	leaseexpires;
		/// <summary>
		/// The date/time at which the IP address lease from
		/// the DHCP server will expire (at which time the
		/// adapter will have to contact the server to renew
		/// the lease or stop using the IP address).
		/// </summary>
		public DateTime	LeaseExpires
		{
			get { return leaseexpires; }
		}

		/// <summary>
		/// Field, if set, is used, if the NDISUIO method
		/// fails, to get the RF signal strength.  You might 
		/// use this on an OS earlier than 4.0, when NDISUIO
		/// became available.  You'd usually create your own
		/// subclass of StrengthAddon, then assign an instance
		/// of that subclass to this property, then ask for
		/// the signal strength.
		/// </summary>
		internal StrengthAddon StrengthFetcher = null;

		/// <summary>
		/// Method called on unbound adapter (maybe when handling
		/// changing *both* the IP/subnet/gateway *and* the wireless
		/// settings).  This method notifies NDIS to bind the 
		/// adapter to all protocols indicated in the registry, in 
		/// effect causing the current registry settings to be 
		/// applied rather than those which the adapter is currently
		/// using.  Since we are binding, not *re*-binding the
		/// protocols, we are implying that the adapter is not
		/// currently bound to anything.  When making this call,
		/// we must refresh any adapter list that we might have,
		/// to retrieve the current state of all adapters.  
		/// Changes to things like the IP address, subnet mask, 
		/// etc. are not immediately returned.
		/// </summary>
		public void BindAdapter()
		{
			// Rather than telling NDISUIO to rebind, we actually open
			// NDIS itself and send the message there.  So, rather than
			// calling this.Open() to open NDISUIO, we need to open the
			// driver directly.
			IntPtr	ndisAccess = FileHelper.CreateFile( 
				NDISPInvokes.NDIS_DEVICE_NAME,
                FileAccess.ReadWrite, 
				FileShare.None,
				FileCreateDisposition.OpenExisting,
				NDISUIOPInvokes.FILE_ATTRIBUTE_NORMAL | NDISUIOPInvokes.FILE_FLAG_OVERLAPPED );

			// Send the device command.
			UInt32	xcount = 0;
			byte[]	namebytes = new byte[ (this.Name.Length+1)*2 ];

			// Zero the byte array.  Since the default value for a byte
			// should be zero, this should not be necessary.

			// Get the bytes forming the Unicode string which is the
			// adapter name.
			Encoding.Unicode.GetBytes( this.Name, 0, this.Name.Length, 
				namebytes, 0);

			// Tell NDIS to bind the adapter.
			if ( !NDISUIOPInvokes.DeviceIoControl( ndisAccess, 
				NDISPInvokes.IOCTL_NDIS_BIND_ADAPTER,
				namebytes, namebytes.Length,
				null, 0, ref xcount, IntPtr.Zero ) )
			{
				// Handle error.
				throw new AdapterException(Marshal.GetLastWin32Error(), 
					"DeviceIoControl( IOCTL_NDIS_BIND_ADAPTER )");
			}

			FileHelper.CloseHandle( ndisAccess );
		}

		/// <summary>
		/// Method called after making some changes to the current
		/// IP address, subnet mask, etc.  This method notifies NDIS
		/// to rebind the adapter to all protocols, in effect causing
		/// the current registry settings to be applied rather than
		/// those which the current configuration represents.  Once you
		/// have rebound an adapter, to get its new configuration, you
		/// must regenerate the list of adapters.  Changes to things
		/// like the IP address, subnet mask, etc. are not immediately 
		/// returned.
		/// </summary>
		public void RebindAdapter()
		{
			// Open the NDIS driver.
			IntPtr	ndisAccess = FileHelper.CreateFile( 
				NDISPInvokes.NDIS_DEVICE_NAME,
                FileAccess.ReadWrite, 
				FileShare.None,
				FileCreateDisposition.OpenExisting,
				NDISUIOPInvokes.FILE_ATTRIBUTE_NORMAL | NDISUIOPInvokes.FILE_FLAG_OVERLAPPED );

			// Send the device command.
			UInt32	xcount = 0;

            // +2 here because some devices actually require it to be a MUTI_SZ (per the docs) and both trailing nulls must be there
			byte[]	namebytes = new byte[ (this.Name.Length + 2) * 2 ];

			// Get the bytes forming the Unicode string which is the
			// adapter name.
			Encoding.Unicode.GetBytes( this.Name, 0, this.Name.Length, 
				namebytes, 0);

			// Tell NDIS to rebind the adapter.
			if ( !NDISUIOPInvokes.DeviceIoControl( ndisAccess, 
				NDISPInvokes.IOCTL_NDIS_REBIND_ADAPTER,
				namebytes, namebytes.Length,
				null, 0, ref xcount, IntPtr.Zero ) )
			{
				// Handle error.
                throw new AdapterException(Marshal.GetLastWin32Error(), 
					"DeviceIoControl( IOCTL_NDIS_REBIND_ADAPTER )");
			}

			FileHelper.CloseHandle( ndisAccess );
		}

		/// <summary>
		/// Method called to unbind a given adapter.  You might
		/// perform this operation before attempting to change
		/// *both* the protocol configuration of an adapter (IP,
		/// subnet, gateway), *and* the wireless configuration of
		/// the same adapter (WEP, SSID, etc.)  To do that, first
		/// unbind the adapter, then change the settings, then
		/// bind the adapter (UnbindAdapter(), make changes,
		/// BindAdapter()).  Once you have bound/unbound an 
		/// adapter, to get its new configuration, you must 
		/// regenerate the list of adapters.  Changes to things
		/// like the IP address, subnet mask, etc. are not 
		/// immediately returned.
		/// </summary>
		public void UnbindAdapter()
		{
			// Open the NDIS driver.
			IntPtr	ndisAccess = FileHelper.CreateFile( 
				NDISPInvokes.NDIS_DEVICE_NAME,
                FileAccess.ReadWrite, 
				FileShare.None,
				FileCreateDisposition.OpenExisting,
				NDISUIOPInvokes.FILE_ATTRIBUTE_NORMAL | NDISUIOPInvokes.FILE_FLAG_OVERLAPPED );

			// Send the device command.
			UInt32	xcount = 0;
			byte[]	namebytes = new byte[ (this.Name.Length+1)*2 ];

			// Zero the byte array.  Since the default value for a byte
			// should be zero, this should not be necessary.

			// Get the bytes forming the Unicode string which is the
			// adapter name.
			Encoding.Unicode.GetBytes( this.Name, 0, this.Name.Length, 
				namebytes, 0);

			// Tell NDIS to unbind the adapter.
			if ( !NDISUIOPInvokes.DeviceIoControl( ndisAccess, 
				NDISPInvokes.IOCTL_NDIS_UNBIND_ADAPTER,
				namebytes, namebytes.Length,
				null, 0, ref xcount, IntPtr.Zero ) )
			{
				// Handle error.
				throw new AdapterException(Marshal.GetLastWin32Error(), 
					"DeviceIoControl( IOCTL_NDIS_UNBIND_ADAPTER )");
			}

			FileHelper.CloseHandle( ndisAccess );
		}

		internal void FromIP_ADAPTER_INFO( IP_ADAPTER_INFO info )
		{
			// Copy the name, description, index, etc.
			name = info.AdapterName;
			description = info.Description;
			index = info.Index;

			// The adapter type should not change, so we
			// can store that.
			type = info.Type;

			// The hardware address should not change, so
			// we can store that, too.
			hwaddress = info.PhysAddress;

			// Get the flag concerning whether DHCP is enabled
			// or not.
			dhcpenabled = info.DHCPEnabled;

			// Get the current IP address and subnet mask.
			currentIp = info.CurrentIpAddress;
			currentsubnet = info.CurrentSubnetMask;

			// Get the gateway address and the DHCP server.
			gateway = info.Gateway;
			dhcpserver = info.DHCPServer;

			// Get the WINS information.
			havewins = info.HaveWINS;
			primarywinsserver = info.PrimaryWINSServer;
			secondarywinsserver = info.SecondaryWINSServer;

			// DHCP lease information.
			leaseobtained = info.LeaseObtained;
			leaseexpires = info.LeaseExpires;
		}

		internal Adapter( IP_ADAPTER_INFO info ) : base(DEVICE_NAME)
		{
			this.FromIP_ADAPTER_INFO( info );
		}

		/// <summary>
		/// Returns a Boolean indicating if the adapter is
		/// an RF Ethernet adapter.
		/// </summary>
		/// <returns>
		/// true if adapter is RF Ethernet; false otherwise
		/// </returns>
		public bool IsWireless
		{
            // AF: It appears that on WM5 when the device is cradled
            // the wireless adapter(s) are unboumd from WZC in a way that 
            // WZCQueryInterface fails with STATUS_NOT_FOUND
            // We now do this by doing any 802.11-related 
            // NDISUIO Query. When attempted on a non-wireless 
            // adapter, it will fail with ERROR_NOT_SUPPORTED

			// Deciding if the adapter is RF Ethernet is a
			// little more complicated than just looking at
			// a bit somewhere.
			// The original scheme is below:
			// get {return ( (Type == AdapterType.Ethernet) && (SignalStrengthInDecibels != 0) ); }
			// I figured that, if you can get a signal strength,
			// it's a wireless card.  However, there are a fair
			// number of errors that would cause that strength
			// to be zero.  So, now we try to use WZC to do this.
			get
			{
				if ( Type != AdapterType.Ethernet )
					return false;

                // Get NDISUIO: handle
                IntPtr ndisAccess = FileHelper.CreateFile(
                    NDISUIOPInvokes.NDISUIO_DEVICE_NAME,
                    FileAccess.Read,
                    FileShare.ReadWrite,
                    FileCreateDisposition.OpenExisting,
                    NDISUIOPInvokes.FILE_ATTRIBUTE_NORMAL | NDISUIOPInvokes.FILE_FLAG_OVERLAPPED);
                bool retval = true;
                // Allocate a buffer. Sise does not matter as we are asking for a DWORD
                NDISQueryOid queryOID = new NDISQueryOid(6000);

                unsafe
                {
                    uint cb = 0;
                    byte[] namestr = Encoding.Unicode.GetBytes(Name + '\0');
                    fixed (byte* name = &namestr[0])
                    {
                        // Get Signal strength
                        queryOID.ptcDeviceName = name;
                        queryOID.Oid = NDISUIOPInvokes.OID_802_11_WEP_STATUS; // 0x0D010217

                        retval = NDISUIOPInvokes.DeviceIoControl(ndisAccess,
                            NDISUIOPInvokes.IOCTL_NDISUIO_QUERY_OID_VALUE,	// 0x00120804
                            queryOID,
                            queryOID.Size,
                            queryOID,
                            queryOID.Size,
                            ref cb,
                            IntPtr.Zero);
                    }
                }

                FileHelper.CloseHandle(ndisAccess);

				return retval;
			}
		}

		/// <summary>
		/// Returns a Boolean indicating if the adapter is
		/// supported by WZC.
		/// </summary>
		/// <returns>
		/// true if adapter is supported by WZC; false otherwise
		/// </returns>
		public bool IsWirelessZeroConfigCompatible
		{
			// Deciding if the adapter is working with WZC
			// requires call call to WZCQueryInterface().
			get 
			{ 
				if ( !this.IsWireless )
					return false;
				
				// Attempt to get the status of the indicated
				// interface by calling WZCQueryInterface.  If
				// it works, we return true; if not, false.
				// Note that the first parameter, the WZC server,
				// is set to null, apparently indicating that the
				// local machine is the target.
				INTF_ENTRY entry = new INTF_ENTRY();
				entry.Guid = this.Name;
				INTFFlags dwOutFlags;

				try
				{
					int uret = WZCPInvokes.WZCQueryInterface(null, INTFFlags.INTF_ALL, ref entry, out dwOutFlags);
					if (uret > 0)
						return false;
				}
				finally
				{
					WZCPInvokes.WZCDeleteIntfObj(ref entry);
				}

				return true;
			}
		}

		/// <summary>
		/// Converts the Adapter to a string representation.  We 
		/// use the adapter's name for this.
		/// </summary>
		/// <returns>
		/// string representing the adapter
		/// </returns>
		public override string ToString()
		{
			return this.Name;
		}

		#region -------------- Changing connection parameters --------------

		/// <summary>
		/// This routine is used, internally, to make entries
		/// for the preferred SSID list.  It centralizes the
		/// mapping of key data into the structure, etc.
		/// </summary>
		/// <param name="SSID">
		/// The SSID string for the new entry
		/// </param>
		/// <param name="bInfrastructure">
		/// Set to true for infrastucture mode; false for
		/// ad hoc mode
		/// </param>
		/// <param name="Key">
		/// WEP key material
		/// </param>
		/// <param name="keyIndex">
		/// Key index.  Valid values are 1-4
		/// </param>
		/// <param name="authMode">
		/// Authentication mode for the connection
		/// </param>
		/// <param name="privacyMode">
		/// Privacy (encryption) mode for the connection
		/// </param>
		/// <returns>
		/// New WZC_WLAN_CONFIG object or null on failure
		/// </returns>
		private WLANConfiguration MakeSSIDEntry(string SSID, bool bInfrastructure, 
			byte[] Key, int keyIndex, 
			AuthenticationMode authMode, WEPStatus privacyMode)
		{
			WLANConfiguration	thisConfig = new WLANConfiguration();

			// Set the length.
			thisConfig.Length = thisConfig.Data.Length;

			// Set the MAC address.
			thisConfig.MACAddress = this.MacAddress;

			// Set the SSID.
			thisConfig.SSID = SSID;

			// Save the privacy mode.
			thisConfig.Privacy = privacyMode;

			// Set the key index.  Note that, since the 'first'
			// key is key #1 in all of the wireless specifications,
			// we have to decrement the value for WZC, which expects
			// it to be 0.
			thisConfig.KeyIndex = keyIndex - 1;

			// Proceed with configuration.
			byte [] arrKey = null;
			if ( Key != null )
			{
					// Key size has already been checked (this
					// is an entry invariant).

					arrKey = Key.Clone() as byte[];
					thisConfig.KeyLength = arrKey.Length;
					thisConfig.CtlFlags |= WZCControl.WEPKPresent | WZCControl.WEPKXFormat;
					if ( arrKey.Length == 10 )
						thisConfig.CtlFlags |= WZCControl.WEPK40Bit;
					byte[] chFakeKeyMaterial = new byte[]{0x56, 0x09, 0x08, 0x98, 0x4D, 0x08, 0x11, 0x66, 0x42, 0x03, 0x01, 0x67, 0x66};
					for( int i = 0; i < arrKey.Length; i ++ )
						arrKey[i] ^= chFakeKeyMaterial[(7*i)%13];
					thisConfig.KeyMaterial = arrKey;
			}
			else
			{
				// Clear the key material, as well as setting
				// the length to zero.
				byte[]	key = new byte[] 
					{ 0, 0, 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0, 0, 0, 
					  0, 0, 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0, 0, 0 };

				thisConfig.KeyMaterial = key;
				thisConfig.KeyLength = 0;
			}
			thisConfig.AuthenticationMode = authMode;

			// ???? do the right thing, based on the mode.

			// If we have no key, we should probably set this to WEP Off.
			thisConfig.InfrastructureMode = bInfrastructure? InfrastructureMode.Infrastructure: InfrastructureMode.AdHoc;

			return thisConfig;
		}

		/// <summary>
		/// The KeyType enumeration tells the Adapter what
		/// sort of key material is being passed.  There are
		/// several types for various forms of WEP and WPA
		/// keys.  External callers of Adapter methods should
		/// set it to None, however.
		/// </summary>
		internal enum KeyType 
		{ 
			None, 
			WPAPassphrase, 
			WPABinary, 
			WEP 
		}

        /// <summary>
        /// The ProcessKey routine makes necessary modifications
		/// to the key material of a WPA key before it is passed
		/// to WZC routines.  The processing done to it depends
		/// on how it was generated.
		/// </summary>
		/// <param name="kt">
		/// The key type, indicating how the key material in
		/// the structure was originally generated
		/// </param>
		/// <param name="config">
		/// The configuration being changed
		/// </param>
		/// <param name="passphrase">
		/// For WPA-PSK passphrase type, the passphrase.
		/// </param>
		internal void ProcessKey( KeyType kt, ref WLANConfiguration config,
			string passphrase )
		{
			// Define fake key material for 'encrypting' the
			// keys.
			byte[]	chFakeKeyMaterial = new byte[] {0x56, 0x09, 0x08, 0x98, 0x4D, 0x08, 0x11, 0x66, 0x42, 0x03, 0x01, 0x67, 0x66};
			byte[]	key;
			uint	i;

			switch ( kt )
			{
				case KeyType.WPAPassphrase:
					// We set this explicitly here.  It was set
					// out of line in the NetUI code.
					config.Privacy = WEPStatus.Ndis802_11Encryption2Enabled;

                    config.CtlFlags |= WZCControl.WEPKXFormat | WZCControl.ONEXEnabled;
    
					WZCPInvokes.WZCPassword2Key(
						ref config, passphrase);

					// Tell it to just encrypt and no conversion.
					config.KeyLength = WLANConfiguration.WZCCTL_MAX_WEPK_MATERIAL;
					config.CtlFlags |= WZCControl.WEPKPresent;    		

					// Note that, since the config structure doesn't
					// actually have a byte[] for key material, we
					// can't modify bytes of that 'array' in-place.
					key = config.KeyMaterial;
					for (i = 0; i < WLANConfiguration.WZCCTL_MAX_WEPK_MATERIAL; i++)
						key[i] ^= chFakeKeyMaterial[(7*i)%13];
					config.KeyMaterial = key;
					break;
				case KeyType.WPABinary:
					// We set this explicitly here.  It was set
					// out of line in the NetUI code.
					config.Privacy = WEPStatus.Ndis802_11Encryption2Enabled;

					config.KeyLength = WLANConfiguration.WZCCTL_MAX_WEPK_MATERIAL;
					config.CtlFlags |= WZCControl.WEPKPresent;

					// Note that, since the config structure doesn't
					// actually have a byte[] for key material, we
					// can't modify bytes of that 'array' in-place.
					key = config.KeyMaterial;
					for (i = 0; i < WLANConfiguration.WZCCTL_MAX_WEPK_MATERIAL; i++)
						config.KeyMaterial[i] ^= chFakeKeyMaterial[(7*i)%13];
					config.KeyMaterial = key;
					break;
			}
		}

		/// <summary>
		/// The CheckKeySize routine checks the incoming 
		/// WEP or WPA key from the user and throws an 
		/// exception of an appropriate type, if the key
		/// size is wrong or if it contains invalid
		/// characters.  In this case, it also maps the
		/// string key into a byte array of the binary
		/// values corresponding to the string, which should
		/// represent the hex values in the key.
		/// </summary>
		/// <param name="sKey">
		/// The string key to be used.  Must consist of a
		/// string of hexadecimal digits, for WEP keys and
		/// WPA binary keys.  May be a passphrase for WPA-PSK,
		/// though, which can be 8-63 characters long.
		/// </param>
		/// <param name="authMode">
		/// Can be any of the authentication mode types, 
		/// including WEP and WPA-PSK.
		/// </param>
		/// <param name="Key">
		/// Reference parameter into which the returned
		/// binary key value is written.  Will be set to
		/// null if the input string key is empty.
		/// </param>
		/// <returns>
		/// KeyType value indicating the type of the key. Note:
		/// for WPAPassphrase type, the caller will still have
		/// to encode the password before using it (Key is not
		/// set).  Note: for WPABinary type, the caller will
		/// still have to encrypt the key, although it is
		/// converted from hex string to binary by this routine.
		/// </returns>
		internal KeyType CheckKeySize( string sKey, AuthenticationMode authMode,
			ref byte[] Key )
		{
			Key = null;

			// If the key is empty, just return.
			if ( ( sKey == null ) || ( sKey.Length == 0 ) )
				return KeyType.None;

			// Handle two cases: WEP key and WPA-PSK
			// password/binary key.
			if ( ( authMode == AuthenticationMode.Ndis802_11AuthModeWPAPSK ) ||
				( authMode == AuthenticationMode.Ndis802_11AuthModeWPANone ) )
			{        
				//  User can only enter either 64 hex entries, or 8/63 any ASCII entries which is always
				//  converted to XFORMAT.
				if ( ( sKey.Length >= 8 ) && ( sKey.Length <= 63 ))
				{
					return KeyType.WPAPassphrase;
				}
				else if (sKey.Length == 64)
				{
					Key = new byte[sKey.Length >> 1];
					try
					{
						for ( int i = 0; i < (sKey.Length >> 1); i++ )
							Key[i] = byte.Parse(sKey.Substring(i*2, 2), System.Globalization.NumberStyles.HexNumber);
					}
					catch
					{
						throw new ArgumentException("Key may contain hexadecimal digits only");
					}

					return KeyType.WPABinary;
				}
				else
				{
					// Throw exception: Invalid WPA key
					// material.
					throw new ArgumentException("WPA key must contain more than 16 and less than 128 hex digits or exactly 64 bytes of binary key data", "sKey");
				}
			}
			else	// WEP
			{
				if ( sKey.Length != 10 && sKey.Length != 26 ) 
					throw new ArgumentException("Key must contain 10 or 26 hexadecimal digits", "sKey");

				Key = new byte[sKey.Length >> 1];
				try
				{
					for ( int i = 0; i < (sKey.Length >> 1) ; i++ )
						Key[i] = byte.Parse(sKey.Substring(i*2, 2), System.Globalization.NumberStyles.HexNumber);
				}
				catch
				{
					throw new ArgumentException("Key may contain hexadecimal digits only");
				}

				return KeyType.WEP;
			}
		}

		/// <summary>
		/// The CheckKeySize routine checks the incoming 
		/// WEP or WPA key from the user and throws an 
		/// exception of an appropriate type, if the key
		/// size is wrong.
		/// </summary>
		/// <param name="Key">
		/// The binary key to be used.
		/// </param>
		/// <param name="authMode">
		/// Can be any of the authentication mode types, 
		/// including WEP and WPA-PSK.
		/// </param>
		internal KeyType CheckKeySize( byte[] Key, AuthenticationMode authMode )
		{
			if ( ( Key == null ) || ( Key.Length == 0 ) )
				return KeyType.None;

			// Handle two cases: WEP key and WPA-PSK
			// password/binary key.
			if ( ( authMode == AuthenticationMode.Ndis802_11AuthModeWPAPSK ) ||
				( authMode == AuthenticationMode.Ndis802_11AuthModeWPANone ) )
			{
				// The only binary key material that you can send here is
				// 32 bytes long.
				if (Key.Length != 32)
				{
					throw new ArgumentException("Key must contain 32 bytes of key data", "Key");
				}

				return KeyType.WPABinary;
			}
			else
			{
				// Length must be 5 or 13.
				if ( ( Key.Length != 5 ) && ( Key.Length != 13 ) )
				{
					throw new ArgumentException("Key must contain 5 or 13 bytes of key data", "Key");
				}

				return KeyType.WEP;
			}
		}

		/// <summary>
		/// Modifies wireless settings associated with a given interface and AP
		/// </summary>
		/// <param name="SSID">Target SSID to connect</param>
		/// <param name="bInfrastructure">Is infrastructure</param>
		/// <param name="sKey">WEP key</param>
		/// <returns>True if successful</returns>
		[Obsolete("OpenNETCF.Net.Adapter.SetWirelessSettings is deprecated. Please use OpenNETCF.Net.Adapter.SetWirelessSettingsAddEx()", true)]
		public bool SetWirelessSettings(string SSID, bool bInfrastructure, string sKey)
		{
			if ( sKey.Length != 10 && sKey.Length != 26 ) 
				throw new ArgumentException("Key must contain 10 or 26 hexadecimal digits", "sKey");

			byte [] arrKey = new byte[sKey.Length >> 1];
			try
			{
				for ( int i = 0; i < (sKey.Length >> 1) ; i++ )
					arrKey[i] = byte.Parse(sKey.Substring(i*2, 2), System.Globalization.NumberStyles.HexNumber);
			}
			catch
			{
				throw new ArgumentException("Key may contain hexadecimal digits only");
			}
			return SetWirelessSettings(SSID, bInfrastructure, arrKey);			
		}

		/// <summary>
		/// Modifies wireless settings associated with a given interface and AP
		/// </summary>
		/// <param name="SSID">Target SSID to connect</param>
		/// <param name="bInfrastructure">Is infrastructure</param>
		/// <param name="Key">binary wep key - 5 or 13 bytes</param>
		/// <returns>true if succeeded</returns>
		[Obsolete("OpenNETCF.Net.Adapter.SetWirelessSettings is deprecated. Please use OpenNETCF.Net.Adapter.SetWirelessSettingsAddEx()", true)]
		public bool SetWirelessSettings(string SSID, bool bInfrastructure, byte[] Key)
		{
			// First, we need to get an INTF_ENTRY for this adapter.
			INTF_ENTRY entry = new INTF_ENTRY();
			entry.Guid = this.Name;
			INTFFlags dwOutFlags;
			int uret = WZCPInvokes.WZCQueryInterface(null, 
				INTFFlags.INTF_ALL, 
				ref entry, out dwOutFlags);
			if ( uret > 0 )
			{
				// As you can see, we presently don't support
				// total configuration of the adapter with no
				// WZC intervention at all.  Somehow, you have
				// to set things up, other than SSID value, 
				// infrastructure mode, and WEP key, so that we
				// have a starting place.
				throw new AdapterException(uret, "WZCQueryInterface");
			}
			else
			{
				// Perform the 'standard' WZC stuff to set the entry's 
				// configuration.
				int cConfig = BitConverter.ToInt32( entry.rdBSSIDList.lpData, 0 );
				int Index = 8;
				WLANConfiguration thisConfig = null;
				for( int i = 0; i < cConfig; i ++ )
				{
					WLANConfiguration cfg = new WLANConfiguration();
					int cbCfg = BitConverter.ToInt32( entry.rdBSSIDList.lpData, Index );
					Buffer.BlockCopy(entry.rdBSSIDList.lpData, Index, cfg.Data, 0, cbCfg);
					Index += cbCfg;
					if ( cfg.SSID == SSID )
						thisConfig = cfg;
					cfg = null;
				}

				// There are a couple of things going on here:
				//	1. It might be that the user is trying to associate
				//		with an access point that we don't know about.
				//		For now, we don't allow this.
				//	2. It is also possible that the adapter is to be 
				//		placed in ad hoc mode and it just so happens that
				//		this is the first adapter to be enabled with the
				//		SSID.  We need to allow this.
				if ( ( thisConfig == null ) && ( bInfrastructure ) )
				{
					WZCPInvokes.WZCDeleteIntfObj(ref entry);
					entry.Dispose(); 
					return false;
				}
				

				// If the config is null, but we are going to continue,
				// we have to create a new one and set it up for us to
				// use.
				if ( thisConfig == null )
				{
					thisConfig = new WLANConfiguration();

					// Set the length.
					thisConfig.Length = thisConfig.Data.Length;

					// Set the MAC address.
					thisConfig.MACAddress = this.MacAddress;

					//				thisConfig.NetworkTypeInUse = NetworkType.????;

					// Set the SSID.
					thisConfig.SSID = SSID;
				}

				// Proceed with configuration.
				byte [] arrKey = null;
				if ( Key != null )
				{
					arrKey = Key.Clone() as byte[];
					thisConfig.KeyLength = arrKey.Length;
					thisConfig.CtlFlags |= WZCControl.WEPKPresent | WZCControl.WEPKXFormat;
					if ( arrKey.Length == 10 )
						thisConfig.CtlFlags |= WZCControl.WEPK40Bit;
					byte[] chFakeKeyMaterial = new byte[]{0x56, 0x09, 0x08, 0x98, 0x4D, 0x08, 0x11, 0x66, 0x42, 0x03, 0x01, 0x67, 0x66};
					for( int i = 0; i < arrKey.Length; i ++ )
						arrKey[i] ^= chFakeKeyMaterial[(7*i)%13];
					thisConfig.KeyMaterial = arrKey;
				}
				else
				{
					thisConfig.KeyLength = 0;
				}
				thisConfig.AuthenticationMode = AuthenticationMode.Ndis802_11AuthModeOpen;

				// If we have no key, we should probably set this to WEP Off.
				thisConfig.Privacy = ( thisConfig.KeyLength > 0 )? WEPStatus.Ndis802_11WEPEnabled : WEPStatus.Ndis802_11WEPDisabled;
				thisConfig.InfrastructureMode = bInfrastructure? InfrastructureMode.Infrastructure: InfrastructureMode.AdHoc;
				byte [] FullConfig = new byte[thisConfig.Data.Length + 8 ];
				FullConfig[0] = 1; 
				thisConfig.Data.CopyTo(FullConfig, 8);
				RAW_DATA dt = new RAW_DATA(FullConfig);
				entry.rdStSSIDList = dt;
				uret = WZCPInvokes.WZCSetInterface(null, INTFFlags.INTF_PREFLIST, ref entry, null);
				if ( uret > 0 )
					throw new AdapterException(uret, "WZCSetInterface");

				WZCPInvokes.WZCDeleteIntfObj(ref entry);
				entry.Dispose();
				return true;
			}
		}

		/// <summary>
		/// Sets wireless settings associated with a given 
		/// interface and AP.  This version of the method is
		/// designed for the case where *all* of the options
		/// are going to be set, where no initial configuration
		/// exists at all.
		/// </summary>
		/// <param name="SSID">Target SSID to connect</param>
		/// <param name="bInfrastructure">Is infrastructure</param>
		/// <param name="sKey">wep key string representing hex string (each two characters are converted to a single byte)</param>
		/// <param name="authMode">Authentication mode for the connection</param>
		/// <returns>true if succeeded</returns>
		[Obsolete("OpenNETCF.Net.Adapter.SetWirelessSettingsEx is deprecated. Please use OpenNETCF.Net.Adapter.SetWirelessSettingsAddEx()", true)]
		public bool SetWirelessSettingsEx(string SSID, bool bInfrastructure, 
			string sKey, AuthenticationMode authMode)
		{
			if ( sKey.Length != 10 && sKey.Length != 26 ) 
				throw new ArgumentException("Key must contain 10 or 26 hexadecimal digits", "sKey");

			byte [] arrKey = new byte[sKey.Length >> 1];
			try
			{
				for ( int i = 0; i < (sKey.Length >> 1) ; i++ )
					arrKey[i] = byte.Parse(sKey.Substring(i*2, 2), System.Globalization.NumberStyles.HexNumber);
			}
			catch
			{
				throw new ArgumentException("Key may contain hexadecimal digits only");
			}
			return SetWirelessSettingsEx(SSID, bInfrastructure, arrKey, authMode);			
		}

		/// <summary>
		/// Sets wireless settings associated with a given 
		/// interface and AP.  This version of the method is
		/// designed for the case where *all* of the options
		/// are going to be set, where no initial configuration
		/// exists at all.
		/// </summary>
		/// <param name="SSID">Target SSID to connect</param>
		/// <param name="bInfrastructure">Is infrastructure</param>
		/// <param name="Key">binary wep key - 5 or 13 bytes</param>
		/// <param name="authMode">Authentication mode for the connection</param>
		/// <returns>true if succeeded</returns>
		[Obsolete("OpenNETCF.Net.Adapter.SetWirelessSettingsEx is deprecated. Please use OpenNETCF.Net.Adapter.SetWirelessSettingsAddEx()", true)]
		public bool SetWirelessSettingsEx(string SSID, bool bInfrastructure, 
			byte[] Key, AuthenticationMode authMode)
		{
			// Unlike the other SetWirelessSettings versions,
			// we *don't* get the current configuration here;
			// our parameters will set that.
			int			uret;
			WLANConfiguration thisConfig;

			thisConfig = new WLANConfiguration();

			// Set the length.
			thisConfig.Length = thisConfig.Data.Length;

			// Set the MAC address.
			thisConfig.MACAddress = this.MacAddress;

			// Set the SSID.
			thisConfig.SSID = SSID;

			// Proceed with configuration.
			byte [] arrKey = null;
			if ( Key != null )
			{
				arrKey = Key.Clone() as byte[];
				thisConfig.KeyLength = arrKey.Length;
				thisConfig.CtlFlags |= WZCControl.WEPKPresent | WZCControl.WEPKXFormat;
				if ( arrKey.Length == 10 )
					thisConfig.CtlFlags |= WZCControl.WEPK40Bit;
				byte[] chFakeKeyMaterial = new byte[]{0x56, 0x09, 0x08, 0x98, 0x4D, 0x08, 0x11, 0x66, 0x42, 0x03, 0x01, 0x67, 0x66};
				for( int i = 0; i < arrKey.Length; i ++ )
					arrKey[i] ^= chFakeKeyMaterial[(7*i)%13];
				thisConfig.KeyMaterial = arrKey;
			}
			else
			{
				thisConfig.KeyLength = 0;
			}
			thisConfig.AuthenticationMode = authMode;

			// ???? do the right thing, based on the mode.

			// If we have no key, we should probably set this to WEP Off.
			thisConfig.Privacy = ( thisConfig.KeyLength > 0 )? WEPStatus.Ndis802_11WEPEnabled : WEPStatus.Ndis802_11WEPDisabled;
			thisConfig.InfrastructureMode = bInfrastructure? InfrastructureMode.Infrastructure: InfrastructureMode.AdHoc;
			byte [] FullConfig = new byte[thisConfig.Data.Length + 8 ];
			FullConfig[0] = 1; 
			thisConfig.Data.CopyTo(FullConfig, 8);
			RAW_DATA dt = new RAW_DATA(FullConfig);

			// Create the entry that will be passed to WZCSetInterface.
			INTF_ENTRY entry = new INTF_ENTRY();
			entry.Guid = this.Name;
			entry.rdStSSIDList = dt;
			uret = WZCPInvokes.WZCSetInterface(null, INTFFlags.INTF_PREFLIST, ref entry, null);
			if ( uret > 0 )
				throw new AdapterException(uret, "WZCSetInterface");
			entry.Dispose();
			return true;
		}

		#endregion

		#region -------------- Methods for control over associations with wireless SSIDs --------------

		/// <summary>
		/// Attaches to an existing SSID value which *must* already be
		/// on the preferred list.  Settings are copied from when the 
		/// AP was added to the preferred list.  This allows you to 
		/// connect to a preconfigured SSID value without respecifying
		/// the WEP key, etc.
		/// </summary>
		/// <param name="SSID">Target SSID to connect</param>
		/// <returns>true if succeeded</returns>
		public bool SetWirelessSettings(string SSID)
		{
			// First, we need to get an INTF_ENTRY for this adapter.
			INTF_ENTRY entry = new INTF_ENTRY();
			entry.Guid = this.Name;
			INTFFlags dwOutFlags;
			int uret = WZCPInvokes.WZCQueryInterface(null, 
				INTFFlags.INTF_ALL, 
				ref entry, out dwOutFlags);
			if ( uret > 0 )
			{
				// As you can see, we presently don't support
				// total configuration of the adapter with no
				// WZC intervention at all.  Somehow, you have
				// to set things up, other than SSID value, 
				// infrastructure mode, and WEP key, so that we
				// have a starting place.
				throw new AdapterException(uret, "WZCQueryInterface");
			}
			else
			{
				// We need to push the indicated item to the top of the
				// preferred list.  Once we do that and call WZCSetInterface
				// the connection will be established to that SSID.
				// The preferred list is in the rdStSSIDList field.
				RAW_DATA				rdold = entry.rdStSSIDList;
				WLANConfigurationList	prefl = new WLANConfigurationList( rdold );

				// Start at the bottom of the list.  If the current item
				// is the one we want to copy, save it and start copying
				// items down in the list.  
				WLANConfiguration	targetItem = null;
				int				i;
				for ( i = (int)prefl.NumberOfItems-1; i >= 0; i-- )
				{
					targetItem = prefl.Item( i );
					if ( targetItem.SSID == SSID )
					{
						break;
					}
				}

				// If we get no match for our SSID value, the item is *not*
				// in the preferred list.  Return false.
				if ( targetItem == null )
					return false;

				// If the SSID is already first in the list, we're done.
				if ( i > 0 )
				{
					// Now, copy the rest of the items one place down in the
					// list.
					for ( int j = i; j >= 1; j-- )
					{
						// Copy old list item j-1 to new list item j.
						prefl.SetItem( j, prefl.Item( j-1 ) );
					}

					// Put the saved target item in index 0 in the new list.
					prefl.SetItem( 0, targetItem );
				}

				// Finally, we are ready to select the new SSID as our 
				// primary preferred connection.
				uret = WZCPInvokes.WZCSetInterface(null, INTFFlags.INTF_ALL_FLAGS | INTFFlags.INTF_PREFLIST, ref entry, null);
				if ( uret > 0 )
					throw new AdapterException(uret, "WZCSetInterface");
				entry.Dispose();
				return true;
			}
		}

		/// <summary>
		/// Sets wireless settings associated with a given 
		/// interface and AP, adding to, rather than replacing
		/// the preferred list of APs.  This version of the 
		/// method is designed for the case where *all* of 
		/// the options are going to be set, where no initial 
		/// configuration exists at all and where existing
		/// items in the preferred list should be maintained.
		/// After this method executes, if it is successful,
		/// the specified SSID will be at the top, highest-
		/// priority, end of the preferred list.
		/// </summary>
		/// <param name="SSID">
		/// Target SSID to connect
		/// </param>
		/// <param name="bInfrastructure">
		/// Is infrastructure
		/// </param>
		/// <param name="sKey">
		/// WEP key string representing hex string (each 
		/// two characters are converted to a single byte)
		/// </param>
		/// <param name="keyIndex">
		/// Index of the WEP key.  Valid values are 1-4
		/// </param>
		/// <param name="authMode">
		/// Authentication mode for the connection
		/// </param>
		/// <param name="privacyMode">
		/// Privacy (encryption) mode for the connection
		/// </param>
		/// <param name="eapParams">
		/// Parameters describing how the connection should use EAP
		/// to authenticate the user to the network
		/// </param>
		/// <returns>true if succeeded</returns>
		public bool SetWirelessSettingsAddEx(string SSID, bool bInfrastructure, 
			string sKey, int keyIndex, 
			AuthenticationMode authMode, WEPStatus privacyMode,
			EAPParameters eapParams )
		{
			// Check key length and fire exception if 
			// out-of-range.
			byte[]	arrKey = null;
			KeyType	kt = CheckKeySize( sKey, authMode, ref arrKey );

			return SetWirelessSettingsAddEx(SSID, bInfrastructure, arrKey, keyIndex, authMode, privacyMode, kt, sKey, eapParams);
		}

		/// <summary>
		/// Sets wireless settings associated with a given 
		/// interface and AP, adding the new SSID to the
		/// list of preferred SSID values, rather than 
		/// replacing the whole list.  This version of the 
		/// method is designed for the case where *all* of 
		/// the options are going to be set, where no 
		/// initial configuration exists at all and where the
		/// current SSID values in the preferred list should
		/// be retained.  After this call completes, if it
		/// is successful, the new SSID is at the top, highest-
		/// priority, end of the preferred list.
		/// </summary>
		/// <param name="SSID">
		/// Target SSID to connect
		/// </param>
		/// <param name="bInfrastructure">
		/// Is infrastructure
		/// </param>
		/// <param name="Key">
		/// WEP or WPA key.  Key is in binary format. 
		/// For WEP, it should contain 5 or 13 bytes of data.
		/// The number of bytes for WPA depends on whether a
		/// passphrase was used to generate it or whether it
		/// is the actual 32-byte binary key
		/// </param>
		/// <param name="keyIndex">
		/// Index of the key.  Valid values are 1-4
		/// </param>
		/// <param name="authMode">
		/// Authentication mode for the connection
		/// </param>
		/// <param name="privacyMode">
		/// Privacy (encryption) mode for the connection
		/// </param>
		/// <param name="eapParams">
		/// Parameters describing how the connection should use EAP
		/// to authenticate the user to the network
		/// </param>
		/// <returns>true if succeeded</returns>
		public bool SetWirelessSettingsAddEx(string SSID, bool bInfrastructure, 
			byte[] Key, int keyIndex, 
			AuthenticationMode authMode, WEPStatus privacyMode,
			EAPParameters eapParams )
		{
			// Verify that key size is valid for authentication
			// mode.  This is only done if this routine was
			// called from an external source.  If it was called
			// from another overload of SetWirelessSettingsAddEx(),
			// it should have a value other than none, which we
			// use.
			KeyType	kt = CheckKeySize( Key, authMode );

			return SetWirelessSettingsAddEx( SSID, bInfrastructure,
				Key, keyIndex, authMode, privacyMode, kt, null, eapParams);
		}

		/// <summary>
		/// Sets wireless settings associated with a given 
		/// interface and AP, adding the new SSID to the
		/// list of preferred SSID values, rather than 
		/// replacing the whole list.  This version of the 
		/// method is designed for the case where *all* of 
		/// the options are going to be set, where no 
		/// initial configuration exists at all and where the
		/// current SSID values in the preferred list should
		/// be retained.  After this call completes, if it
		/// is successful, the new SSID is at the top, highest-
		/// priority, end of the preferred list.
		/// </summary>
		/// <param name="SSID">
		/// Target SSID to connect
		/// </param>
		/// <param name="bInfrastructure">
		/// Is infrastructure
		/// </param>
		/// <param name="Key">
		/// WEP or WPA key.  Key is in binary format. 
		/// For WEP, it should contain 5 or 13 bytes of data.
		/// The number of bytes for WPA depends on whether a
		/// passphrase was used to generate it or whether it
		/// is the actual 32-byte binary key
		/// </param>
		/// <param name="keyIndex">
		/// Index of the key.  Valid values are 1-4
		/// </param>
		/// <param name="authMode">
		/// Authentication mode for the connection
		/// </param>
		/// <param name="privacyMode">
		/// Privacy (encryption) mode for the connection
		/// </param>
		/// <param name="kt">
		/// Key type of the key.  Should be KeyType.None for 
		/// external callers
		/// </param>
		/// <param name="wpaPassphrase">
		/// This is a bad way to have to do things, but, short
		/// of duplicating the code to do the processing for
		/// the string passphrase case and the binary key case,
		/// I need to get a WPA passphrase here.  If the key type,
		/// kt, is not KeyType.WPAPassphrase, the string isn't 
		/// used, so you can set it to null.
		/// </param>
		/// <param name="eapParams">
		/// Parameters describing how the connection should use EAP
		/// to authenticate the user to the network
		/// </param>
		/// <returns>true if succeeded</returns>
		internal bool SetWirelessSettingsAddEx(string SSID, bool bInfrastructure, 
			byte[] Key, int keyIndex,
			AuthenticationMode authMode, WEPStatus privacyMode,
			KeyType kt, string wpaPassphrase, 
			EAPParameters eapParams )
		{
			// We may yet need to do some processing on the key,
			// if it is a WPA key.  We need a WZC_WLAN_CONFIG 
			// structure to pass to the WZC routine that does 
			// this processing, however, so that is done below.

			// Get the current preferred list of SSID values.
			// First, we need to get an INTF_ENTRY for this adapter.
			INTF_ENTRY entry = new INTF_ENTRY();
			entry.Guid = this.Name;
			INTFFlags dwOutFlags;
			int uret = WZCPInvokes.WZCQueryInterface(null, 
				INTFFlags.INTF_ALL, 
				ref entry, out dwOutFlags);
			if ( uret > 0 )
			{
				// No preferred list is not a valid starting point 
				// for this call.
				throw new AdapterException(uret, "No preferred list found");
			}
			else
			{
				// We need to push the indicated item to the top of the
				// preferred list.  Once we do that and call WZCSetInterface
				// the connection will be established to that SSID.
				// The preferred list is in the rdStSSIDList field.
				RAW_DATA				rdold = entry.rdStSSIDList;
				WLANConfigurationList	prefl = new WLANConfigurationList( rdold );

				// Start at the bottom of the list.  If the current item
				// is the one we want to copy, save it and start copying
				// items down in the list.  
				WLANConfiguration	targetItem = null;
				int				i;
				for ( i = (int)prefl.NumberOfItems-1; i >= 0; i-- )
				{
					targetItem = prefl.Item( i );
					if ( targetItem.SSID == SSID )
					{
						break;
					}
				}

				// If we get no match for our SSID value, the item 
				// is *not* in the preferred list, so we can
				// skip removing it.
				if ( i >= 0 )
				{
					// Now, copy the items before i on the
					// list down to cover i.  This leaves
					// position 0 in the list as a copy of
					// position 1.  We'll fill in position 0
					// with the new most-preferred SSID.
					for ( int j = i; j >= 1; j-- )
					{
						// Copy old list item j-1 to new list item j.
						prefl.SetItem( j, prefl.Item( j-1 ) );
					}
				}
				else
				{
					// The item was not in the list.  We have
					// to expand the list and move all of
					// the original items down one spot.
					WLANConfigurationList	prefl2 = new WLANConfigurationList( prefl.NumberOfItems+1 );
					for ( int j = 0; j < (int)prefl.NumberOfItems; j++ )
					{
						// Copy from old list to new list.
						prefl2.SetItem( j+1, prefl.Item( j ) );
					}

					// Replace the old list with the new one
					// for the rest of the code.  Entry #0
					// is unset.
					prefl = prefl2;
				}

				// Create a new item and put that in the list
				// at item #0, which presently exists but
				// doesn't mean anything (it's either a 
				// totally blank item, if the SSID was not
				// in the list before the call, or it's the
				// old first item in the list).

				// Unlike the other SetWirelessSettings versions,
				// we *don't* get the current configuration here;
				// our parameters will set that.
				WLANConfiguration thisConfig = this.MakeSSIDEntry( SSID, bInfrastructure, Key, keyIndex, authMode, privacyMode );

				// Turn on the 802.1x enable flag, if indicated
				// by the caller.
				if ( ( eapParams != null ) && eapParams.Enable8021x )
				{
					thisConfig.CtlFlags |= WZCControl.ONEXEnabled;

					// Copy the EAP parameters to the new configuration.
					thisConfig.EapolParams = eapParams;
				}

				// Process the key for the target item, based
				// on the key type.  Note that we have to do this
				// before the item gets copied to the list.  We
				// can't get a real reference to any of the data
				// items in the list, only copies of thier data.
				ProcessKey( kt, ref thisConfig, wpaPassphrase );

				// OK, finally, set the item in the preferred
				// list according to the parameters to this
				// call.
				prefl.SetItem( 0, thisConfig );

				// Must now copy the new preferred list to the entry that
				// we will sent with WZCSetInterface.
				entry.rdStSSIDList = prefl.rawData;

				// Finally, we are ready to select the new SSID as our 
				// primary preferred connection.
				uret = WZCPInvokes.WZCSetInterface(null, INTFFlags.INTF_PREFLIST, ref entry, null);
				if ( uret > 0 )
					throw new AdapterException(uret, "WZCSetInterface");
				entry.Dispose();
				return ( uret <= 0 );
			}
		}

		/// <summary>
		/// Removes preferred connection with indicated SSID
		/// value from preferred connection list for the
		/// adapter.  This is roughly equivalent to using
		/// the Advanced settings in the WZC dialogs to 
		/// delete an item from the preferred list.
		/// </summary>
		/// <param name="SSID">
		/// SSID value of list item to remove.
		/// </param>
		/// <returns>
		/// true if successful; false otherwise
		/// </returns>
		public bool RemoveWirelessSettings(string SSID)
		{
			// Get the current preferred list of SSID values.
			// First, we need to get an INTF_ENTRY for this adapter.
			INTF_ENTRY entry = new INTF_ENTRY();
			entry.Guid = this.Name;
			INTFFlags dwOutFlags;
			int uret = WZCPInvokes.WZCQueryInterface(null, 
				INTFFlags.INTF_ALL, 
				ref entry, out dwOutFlags);
			if ( uret > 0 )
			{
				// There is no list.  Return false.
				return false;
			}

			// Find the indicated item and remove it from
			// the list by creating a new list and setting
			// that as the preferred list.
			RAW_DATA				rdold = entry.rdStSSIDList;
			WLANConfigurationList	prefl = new WLANConfigurationList( rdold );

			// If there are no items in the list, return false.
			if ( prefl.NumberOfItems == 0 )
			{
				return false;
			}

			// Start at the top of the old list and copy items
			// from old to new, until we find the item to be
			// removed.
			int	j = 0;
			int	i;
            bool found = false;
            for (i = 0; i < prefl.NumberOfItems; i++)
                if (found = prefl.Item(i).SSID == SSID)
                    break;

            if (!found) // Nothing to do
                return false;

            // Build the new list.
            WLANConfigurationList prefnew = new WLANConfigurationList(prefl.NumberOfItems - 1);

            for (i = 0; i < prefl.NumberOfItems; i++)
			{
				WLANConfiguration	item = prefl.Item( i );

				if ( item.SSID != SSID )
					prefnew.SetItem( j++, item );
			}

			// Replace the old list with the new one
			// for the rest of the code.  Entry #0
			// is unset.
			prefl = prefnew;

			// Must now copy the new preferred list to the entry that
			// we will sent with WZCSetInterface.
			entry.rdStSSIDList = prefl.rawData;          

			// Finally, we are ready to select the new SSID as our 
			// primary preferred connection.
            INTFFlags flags;
			uret = WZCPInvokes.WZCSetInterface(null, INTFFlags.INTF_ALL_FLAGS | INTFFlags.INTF_PREFLIST, ref entry, out flags);
			if ( uret > 0 )
				throw new AdapterException(uret, "WZCSetInterface");
			entry.Dispose();
			return ( uret <= 0 );
		}

		#endregion

		/// <summary>
		/// Returns the currently-attached SSID for RF
		/// Ethernet adapters.
		/// </summary>
		/// <returns>
		/// Instance of SSID class (or null if not associated).
		/// </returns>
		public unsafe String AssociatedAccessPoint
		{
			get 
			{
				// Are we wireless?
				if(!IsWireless)
					throw new AdapterException("Wired NICs are not associated with Access Points");
				
				String	ssid = null;

				// If we are running on an OS version of 4.0 or
				// higher (Windows CE.NET), then attempt to use
				// NDISUIO to get the SSID.  If we are running on 
				// an earlier version of the OS, we call a virtual 
				// method to get it.  If you have a PPC or other 
				// 3.0-based device, you can override this method 
				// to get the SSID in some other way.
				if ( System.Environment.OSVersion.Version.Major >= 4 )
				{
					NDISQueryOid	queryOID;

					// Attach to NDISUIO.
					try
					{
						this.Open();
					}
					catch(Exception)
					{
						return null;
					}

					// Pin unsafely-accessed items in memory.
					byte[]	namestr = System.Text.Encoding.Unicode.GetBytes(this.Name+'\0');
					fixed (byte *name = &namestr[ 0 ])
					{
						// Get Signal strength
						queryOID = new NDISQueryOid( 36 );	// The data is a four-byte length plus 32-byte ASCII string
						queryOID.ptcDeviceName = name;
						queryOID.Oid = NDISUIOPInvokes.OID_802_11_SSID; // 0x0D010102

						try
						{
							this.DeviceIoControl(
								(int)NDISUIOPInvokes.IOCTL_NDISUIO_QUERY_OID_VALUE,	// 0x00120804
								queryOID, 
								queryOID);
						}
						catch(Exception)
						{
                            //jsm - Must close Adapter handle if call to DeviceIoControl fails
                            this.Close();
							return null;
						}
					}

					// Convert the data to a string.
					byte[]	ssdata = queryOID.Data;
					int		len	= BitConverter.ToInt32( ssdata, 0 );
					if ( len > 0 )
					{
						// Convert the string from ASCII to
						// Unicode.
						ssid = System.Text.Encoding.ASCII.GetString( ssdata, 4, len );
					}

					this.Close();
				}

				// If there is still no signal indication,
				// give the add-on method a chance.
				if ( ( ssid == null ) && ( this.StrengthFetcher != null ) )
					ssid = this.StrengthFetcher.RFSSID( this );

				return ssid;
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
		public unsafe int SignalStrengthInDecibels
		{
			get 
			{
				int	db = 0;	// 0 indicates not an RF adapter or error.

				// If we are running on an OS version of 4.0 or
				// higher (Windows CE.NET), then attempt to use
				// NDISUIO to get the RF signal strenth.  If we
				// are running on an earlier version of the OS, 
				// we call a virtual method to get it.  If you
				// have a PPC or other 3.0-based device, you can
				// override this method to get the signal
				// strength in some other way.
				if ( System.Environment.OSVersion.Version.Major >= 4 )
				{
					NDISQueryOid	queryOID;

					// Attach to NDISUIO.
					try
					{
						this.Open();
					}
					catch(Exception)
					{
						return 0;
					}

					// Pin unsafely-accessed items in memory.
					byte[]	namestr = System.Text.Encoding.Unicode.GetBytes(this.Name+'\0');
					fixed (byte *name = &namestr[ 0 ])
					{
						// Get Signal strength
						queryOID = new NDISQueryOid( 4 );
						queryOID.ptcDeviceName = name;
						queryOID.Oid = NDISUIOPInvokes.OID_802_11_RSSI; // 0x0d010206

						try
						{
							this.DeviceIoControl(
								(int)NDISUIOPInvokes.IOCTL_NDISUIO_QUERY_OID_VALUE,	// 0x00120804
								queryOID, 
								queryOID);
						}
						catch(Exception)
						{
                            //jsm - Bug 77 - Must close Adapter handle if call to DeviceIoControl fails
                            this.Close();
							return 0;
						}						
					}

					byte[]	ssdata = queryOID.Data;
					db = BitConverter.ToInt32( ssdata, 0 );

					this.Close();
				}

				// If there is still no signal indication,
				// give the add-on method a chance.
				if ( ( db == 0 ) && ( this.StrengthFetcher != null ) )
					db = this.StrengthFetcher.RFSignalStrengthDB( this );

				return db;
			}
		}

		/// <summary>
		/// Returns a SignalStrength class representing the current strength
		/// of the signal.
		/// </summary>
		/// <returns>
		///	SignalStrength
		/// </returns>
		public SignalStrength SignalStrength
		{
			get 
			{
				// Check if its a 802.11 adapter first...
				if(!IsWireless)
					throw new AdapterException("Signal strength is not a property of a wired NIC adapter");

				// Get the signal strength code and just convert
				// it to a string.
				return ( new SignalStrength(this.SignalStrengthInDecibels) );
			}
		}

		/// <summary>
		/// Returns a list of the SSID values which the 
		/// adapter can currently 'hear'.
		/// </summary>
		/// <returns>
		/// SSIDList instance containing the SSIDs.
		/// </returns>
		public AccessPointCollection NearbyAccessPoints
		{
			get { return ( new AccessPointCollection( this ) ); }
		}

		/// <summary>
		/// Returns the list of preferred SSID values which the 
		/// adapter is currently assigned.  Note that none of
		/// these might be within range, however.
		/// </summary>
		/// <returns>
		/// SSIDList instance containing the preferred SSIDs.
		/// </returns>
		public AccessPointCollection PreferredAccessPoints
		{
			get { return ( new AccessPointCollection( this, false ) ); }
		}

		/// <summary>
		/// Returns the list of preferred SSID values which the 
		/// adapter is currently assigned, but also updates the
		/// signal strengths to their current values.  Otherwise,
		/// the signal strengths are not really valid.
		/// </summary>
		/// <returns>
		/// SSIDList instance containing the preferred SSIDs.
		/// </returns>
		public AccessPointCollection NearbyPreferredAccessPoints
		{
			get { return ( new AccessPointCollection( this, true ) ); }
		}

		/// <summary>
		/// Releases the Adapter's DHCP lease
		/// </summary>
		public void DhcpRelease()
		{
			IP_ADAPTER_INDEX_MAP map = new IP_ADAPTER_INDEX_MAP();
			map.Name = this.Name;
			map.Index = this.Index;

			AdapterPInvokes.IpReleaseAddress(map);
		}

		/// <summary>
		/// Renews the Adapter's DHCP lease
		/// </summary>
		public void DhcpRenew()
		{
			IP_ADAPTER_INDEX_MAP map = new IP_ADAPTER_INDEX_MAP();
			map.Name = this.Name;
			map.Index = this.Index;

			AdapterPInvokes.IpRenewAddress(map);
		}
	}

	#region ---------- P/Invokes ----------

	// P/Invoke declarations.
	internal class AdapterPInvokes
	{
		[DllImport ("iphlpapi.dll", SetLastError=true)]
		public static extern int GetAdaptersInfo( byte[] ip, ref int size );

		[DllImport ("iphlpapi.dll", SetLastError=true)]
		public static extern uint IpReleaseAddress(byte[] adapterInfo);

		[DllImport ("iphlpapi.dll", SetLastError=true)]
		public static extern uint IpRenewAddress(byte[] adapterInfo);
	}
	
	internal class IP_ADAPTER_INDEX_MAP
	{
		byte[] m_bytes = new byte[260]; // 4 + (128 * 2)

		public IP_ADAPTER_INDEX_MAP() {}
		
		public IP_ADAPTER_INDEX_MAP(byte[] data) 
		{
			m_bytes = data;
		}

		public static implicit operator byte[] (IP_ADAPTER_INDEX_MAP map)
		{
			return map.m_bytes;
		}

		public static implicit operator IP_ADAPTER_INDEX_MAP(byte[] data)
		{
			return new IP_ADAPTER_INDEX_MAP(data);
		}

		public int Index
		{
			get { return BitConverter.ToInt32(m_bytes, 0); }
			set { Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_bytes, 0, 4); }
		}

		public string Name
		{
			get { return BitConverter.ToString(m_bytes, 4); }
			set 
			{
				byte[] bytes = Encoding.Unicode.GetBytes(value);
				Buffer.BlockCopy(bytes, 0, m_bytes, 4, bytes.Length); 
			}
		}
	}

	internal class NDISPInvokes
	{
		// This name might be used when rebinding an adapter to all
		// of its protocols (when changing its IP address, for example).
		public const String NDIS_DEVICE_NAME = "NDS0:";

		// Pass this value, with the name of the adapter to be bound,
		// to bind an unbound adapter to all of its protocols, 
		// effectively grabbing its setup from the registry.
		public const uint IOCTL_NDIS_BIND_ADAPTER = 0x00170032;

		// Pass this value, with the name of the adapter to be rebound,
		// to rebind an adapter to all of its protocols, effectively
		// grabbing its setup again from the registry.
		public const uint IOCTL_NDIS_REBIND_ADAPTER = 0x0017002e;

		// Pass this value, with the name of the adapter to be unbound,
		// to unbind an unbound adapter from all of its protocols, 
		// effectively 'disconnecting' it.
		public const uint IOCTL_NDIS_UNBIND_ADAPTER = 0x00170036;

	}

	internal class NDISUIOPInvokes
	{
		public const String NDISUIO_DEVICE_NAME = "UIO1:";

		[DllImport("coredll.dll", SetLastError = true)]
		public static extern bool DeviceIoControl(
			IntPtr hDevice, UInt32 dwIoControlCode,
			byte[] lpInBuffer, Int32 nInBufferSize,
			byte[] lpOutBuffer, Int32 nOutBufferSize,
			ref UInt32 lpBytesReturned,
			IntPtr lpOverlapped);

		public const Int32 FILE_ATTRIBUTE_NORMAL = 0x00000080;
		public const Int32	FILE_FLAG_OVERLAPPED = 0x40000000;

		public const UInt32 ERROR_SUCCESS = 0x0;
		public const UInt32 E_FAIL = 0x80004005;

		public const uint OID_802_11_BSSID                        = 0x0D010101;
		public const uint OID_802_11_SSID                         = 0x0D010102;
		public const uint OID_802_11_NETWORK_TYPES_SUPPORTED      = 0x0D010203;
		public const uint OID_802_11_NETWORK_TYPE_IN_USE          = 0x0D010204;
		public const uint OID_802_11_TX_POWER_LEVEL               = 0x0D010205;
		public const uint OID_802_11_RSSI                         = 0x0D010206;
		public const uint OID_802_11_RSSI_TRIGGER                 = 0x0D010207;
		public const uint OID_802_11_INFRASTRUCTURE_MODE          = 0x0D010108;
		public const uint OID_802_11_FRAGMENTATION_THRESHOLD      = 0x0D010209;
		public const uint OID_802_11_RTS_THRESHOLD                = 0x0D01020A;
		public const uint OID_802_11_NUMBER_OF_ANTENNAS           = 0x0D01020B;
		public const uint OID_802_11_RX_ANTENNA_SELECTED          = 0x0D01020C;
		public const uint OID_802_11_TX_ANTENNA_SELECTED          = 0x0D01020D;
		public const uint OID_802_11_SUPPORTED_RATES              = 0x0D01020E;
		public const uint OID_802_11_DESIRED_RATES                = 0x0D010210;
		public const uint OID_802_11_CONFIGURATION                = 0x0D010211;
		public const uint OID_802_11_STATISTICS                   = 0x0D020212;
		public const uint OID_802_11_ADD_WEP                      = 0x0D010113;
		public const uint OID_802_11_REMOVE_WEP                   = 0x0D010114;
		public const uint OID_802_11_DISASSOCIATE                 = 0x0D010115;
		public const uint OID_802_11_POWER_MODE                   = 0x0D010216;
		public const uint OID_802_11_BSSID_LIST                   = 0x0D010217;
		public const uint OID_802_11_AUTHENTICATION_MODE          = 0x0D010118;
		public const uint OID_802_11_PRIVACY_FILTER               = 0x0D010119;
		public const uint OID_802_11_BSSID_LIST_SCAN              = 0x0D01011A;
		public const uint OID_802_11_WEP_STATUS                   = 0x0D01011B;
		// Renamed to support more than just WEP encryption
		public const uint OID_802_11_ENCRYPTION_STATUS            = OID_802_11_WEP_STATUS;
		public const uint OID_802_11_RELOAD_DEFAULTS              = 0x0D01011C;

		public const uint IOCTL_NDISUIO_QUERY_OID_VALUE = 0x120804;
		public const uint IOCTL_NDISUIO_SET_OID_VALUE = 0x120814;
		public const uint IOCTL_NDISUIO_REQUEST_NOTIFICATION = 0x12081c;
		public const uint IOCTL_NDISUIO_CANCEL_NOTIFICATION = 0x120820;

		// Adapter notification flags.
		public const uint NDISUIO_NOTIFICATION_RESET_START					= 0x00000001;
		public const uint NDISUIO_NOTIFICATION_RESET_END					= 0x00000002;
		public const uint NDISUIO_NOTIFICATION_MEDIA_CONNECT				= 0x00000004;
		public const uint NDISUIO_NOTIFICATION_MEDIA_DISCONNECT				= 0x00000008;			
		public const uint NDISUIO_NOTIFICATION_BIND							= 0x00000010;
		public const uint NDISUIO_NOTIFICATION_UNBIND						= 0x00000020;
		public const uint NDISUIO_NOTIFICATION_MEDIA_SPECIFIC_NOTIFICATION  = 0x00000040;
	}

	/// <summary>
	/// P/Invoke definitions for WZC API
	/// </summary>
	internal class WZCPInvokes
	{
		[DllImport("wzcsapi.dll")]
		public static extern int
			WZCQueryInterface(
			string              pSrvAddr,
			INTFFlags           dwInFlags,
			ref INTF_ENTRY      pIntf,
			out INTFFlags		pdwOutFlags);

        [DllImport("wzcsapi.dll")]
		public static extern uint
			WZCEnumInterfaces(
			string           pSrvAddr,
			ref INTFS_KEY_TABLE pIntfs);

        [DllImport("wzcsapi.dll")]
		public static extern int
			WZCSetInterface(
			string              pSrvAddr,
			INTFFlags           dwInFlags,
			ref INTF_ENTRY      pIntf,
			object		pdwOutFlags);

        [DllImport("wzcsapi.dll")]
        public static extern int
            WZCSetInterface(
            string pSrvAddr,
            INTFFlags dwInFlags,
            ref INTF_ENTRY pIntf,
            out INTFFlags pdwOutFlags);

		//---------------------------------------
		// WZCDeleteIntfObj: cleans an INTF_ENTRY object that is
		// allocated within any RPC call.
		// 
		// Parameters
		// pIntf
		//     [in] pointer to the INTF_ENTRY object to delete
        [DllImport("wzcsapi.dll")]
		public static extern void
			WZCDeleteIntfObj(
			ref INTF_ENTRY Intf);

        [DllImport("wzcsapi.dll")]
		public static extern void
			WZCDeleteIntfObj(
			IntPtr p);


		//---------------------------------------
		// WZCPassword2Key: Translates a user password (8 to 63 ascii chars)
		// into a 256 bit network key)  Note that the second parameter is the
		// key string, but unlike most strings, this one is using ASCII, not
		// Unicode.  We export a Unicode version and do the mapping inside
		// that.
		[DllImport("wzcsapi.dll", EntryPoint="WZCPassword2Key")]
		protected static extern void
			WZCPassword2KeyCE(
			byte[] pwzcConfig,
			byte[] cszPassword); 

		public static void
			WZCPassword2Key(
			ref WLANConfiguration pwzcConfig,
			string cszPassword )
		{
			// Convert string from Unicode to Ascii.
			byte[] ascii = Encoding.ASCII.GetBytes(cszPassword + '\0');

			// Pass Ascii string and configuration structure
			// to the external call.
			WZCPassword2KeyCE( pwzcConfig.Data, ascii );
		}
	}

	#endregion

}
