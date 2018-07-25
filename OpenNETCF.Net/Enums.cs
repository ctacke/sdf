using System;

namespace OpenNETCF.Net
{

	/// <summary>
	/// Enumeration returned in the NetworkTypeInUse property.
	/// Indicates the general type of radio network in use.
	/// </summary>
    [Obsolete("This enum is obsolete and will be removed in a future version of the SDF.  Consider using OpenNETCF.Net.NetworkInformation.NetworkType instead", false)]
    public enum NetworkType
	{
		/// <summary>
		/// Indicates the physical layer of the frequency hopping spread-spectrum radio
		/// </summary>
		FH,
		/// <summary>
		/// Indicates the physical layer of the direct sequencing spread-spectrum radio
		/// </summary>
		DS,
		/// <summary>
		/// Indicates the physical layer for 5-GHz Orthagonal Frequency Division Multiplexing radios
		/// </summary>
		OFDM5,
		/// <summary>
		/// Indicates the physical layer for 24-GHz Orthagonal Frequency Division Multiplexing radios
		/// </summary>
		OFDM24
	}

	/// <summary>
	/// Define the general network infrastructure mode in
	/// which the selected network is presently operating.
	/// </summary>
    [Obsolete("This enum is obsolete and will be removed in a future version of the SDF.  Consider using OpenNETCF.Net.NetworkInformation.InfrastructureMode instead", false)]
    public enum InfrastructureMode
	{
		/// <summary>
		/// Specifies the independent basic service set (IBSS) mode. This mode is also known as ad hoc mode
		/// </summary>
		AdHoc,
		/// <summary>
		/// Specifies the infrastructure mode.
		/// </summary>
		Infrastructure,
		/// <summary>
		/// The infrastructure mode is either set to automatic or cannot be determined.
		/// </summary>
		AutoUnknown
	}

	/// <summary>
	/// Define authentication types for an adapter.
	/// </summary>
    [Obsolete("This enum is obsolete and will be removed in a future version of the SDF.  Consider using OpenNETCF.Net.NetworkInformation.AuthenticationMode instead", false)]
    public enum AuthenticationMode
	{
        /// <summary>
        /// Specifies IEEE 802.11 Open System authentication mode. In this mode, there are no checks performed during the 802.11 authentication
        /// </summary>
		Ndis802_11AuthModeOpen,
        /// <summary>
        /// Specifies IEEE 802.11 Shared Key authentication mode. This mode requires the use of a pre-shared Wired Equivalent Privacy (WEP) key for the 802.11 authentication
        /// </summary>
		Ndis802_11AuthModeShared,
        /// <summary>
        /// Specifies auto-switch mode. When using auto-switch mode, the device tries IEEE 802.11 Shared Key authentication mode first. If Shared Key authentication fails, the device attempts to use IEEE 802.11 Open System authentication mode.
        /// <remarks><b>Note</b> The use of this setting is not recommended</remarks> 
        /// </summary>
		Ndis802_11AuthModeAutoSwitch,
        /// <summary>
        /// Specifies WPA version 1 security for infrastructure mode. Authentication is performed between the supplicant, authenticator, and authentication server over IEEE 802.1X. Encryption keys are dynamic and are derived through the authentication process
        /// While in this authentication mode, the device will only associate with an access point whose beacon or probe responses contain the authentication suite of type 1 (802.1X) within the WPA information element (IE).
        /// This authentication mode is only valid for infrastructure network modes. The driver must return NDIS_STATUS_NOT_ACCEPTED if its network mode is set to ad hoc.
        /// </summary>
		Ndis802_11AuthModeWPA,
        /// <summary>
        /// Specifies WPA version 1 security for infrastructure mode. Authentication is made between the supplicant and authenticator over IEEE 802.1X. Encryption keys are dynamic and are derived through a pre-shared key used on both the supplicant and authenticator.
        /// While in this authentication mode, the device will only associate with an access point whose beacon or probe responses contain the authentication suite of type 2 (pre-shared key) within the WPA information element (IE).
        /// This authentication mode is only valid for infrastructure network modes. The driver must return NDIS_STATUS_NOT_ACCEPTED if its network mode is set to ad hoc.
        /// </summary>
		Ndis802_11AuthModeWPAPSK,
        /// <summary>
        /// Specifies WPA version 1 security for ad hoc mode. This setting specifies the use of a pre-shared key without IEEE 802.1X authentication. Encryption keys are static and are derived through the pre-shared key.
        /// This authentication mode is only valid for ad hoc network modes. The driver must return NDIS_STATUS_NOT_ACCEPTED if its network mode is set to infrastructure.
        /// </summary>
		Ndis802_11AuthModeWPANone,
        /// <summary>
        /// Upper bound of Modes - not a real mode
        /// </summary>
		Ndis802_11AuthModeMax
	}

	/// <summary>
	/// Define WEP authentication state for the adapter.
	/// </summary>
    [Obsolete("This enum is obsolete and will be removed in a future version of the SDF.  Consider using OpenNETCF.Net.NetworkInformation.WEPStatus instead", false)]
    public enum WEPStatus
	{
		/// <summary>
		/// WEP encryption enabled
		/// </summary>
		Ndis802_11WEPEnabled,
		/// <summary>
		/// WEP encryption enabled
		/// </summary>
		Ndis802_11Encryption1Enabled = Ndis802_11WEPEnabled,    
		/// <summary>
		/// No WEP encryption
		/// </summary>
		Ndis802_11WEPDisabled,
		/// <summary>
		/// No WEP encryption
		/// </summary>
		Ndis802_11EncryptionDisabled = Ndis802_11WEPDisabled,    
        /// <summary>
        /// WEP key is missing
        /// </summary>
		Ndis802_11WEPKeyAbsent,
        /// <summary>
        /// WEP, TKIP and AES are disabled. A transmit key is not available.
        /// </summary>
		Ndis802_11Encryption1KeyAbsent = Ndis802_11WEPKeyAbsent,
        /// <summary>
        /// WEP is not supported
        /// </summary>
		Ndis802_11WEPNotSupported,
        /// <summary>
        /// Encryption using the WEP, TKIP, and AES cipher suites is not supported
        /// </summary>
		Ndis802_11EncryptionNotSupported = Ndis802_11WEPNotSupported,
        /// <summary>
        /// TKIP and WEP are enabled; AES is disabled. A transmit key is available
        /// </summary>
		Ndis802_11Encryption2Enabled,
        /// <summary>
        /// TKIP and WEP are enabled; AES is disabled. A transmit key is not available
        /// </summary>
		Ndis802_11Encryption2KeyAbsent,
        /// <summary>
        /// AES, TKIP, and WEP are enabled, and a transmit key is available.
        /// </summary>
		Ndis802_11Encryption3Enabled,
        /// <summary>
        /// AES, TKIP, and WEP are enabled. A transmit keys is not available.
        /// </summary>
		Ndis802_11Encryption3KeyAbsent
	}

	/// <summary>
	/// Control flags for Windows Zero Config
	/// </summary>
    [Obsolete("This enum is obsolete and will be removed in a future version of the SDF.  Consider using OpenNETCF.Net.NetworkInformation.WZCControl instead", false)]
    [Flags]
	public enum WZCControl
	{
		/// <summary>
		/// specifies whether the configuration includes or not a WEP key
		/// </summary>
		WEPKPresent        =0x0001, 
		/// <summary>
		/// the WEP Key material (if any) is entered as hexadecimal digits
		/// </summary>
		WEPKXFormat        =0x0002,  
		/// <summary>
		/// this configuration should not be stored
		/// </summary>
		Volatile            =0x0004,  
		/// <summary>
		/// this configuration is enforced by the policy
		/// </summary>
		Policy              =0x0008,  
		/// <summary>
		/// for this configuration 802.1X should be enabled
		/// </summary>
		ONEXEnabled        =0x0010,  
		/// <summary>
		/// Key is 40 bit
		/// </summary>
		WEPK40Bit         =0x8000

	}

	/// <summary>
	/// The EAP_TYPE enumeration is used when configuring a
	/// connection to an EAP-enabled network.
	/// </summary>
    [Obsolete("This enum is obsolete and will be removed in a future version of the SDF.  Consider using OpenNETCF.Net.NetworkInformation.EAPType instead", false)]
    public enum EAPType
	{
		/// <summary>
		/// MD5 authentication
		/// </summary>
		MD5                    = 4,

		/// <summary>
		/// EAP-TLS authentication
		/// </summary>
		TLS                    = 13,

		/// <summary>
		/// PEAP authentication
		/// </summary>
		PEAP                   = 25,

		/// <summary>
		/// MS-CHAP version 2 authentication
		/// </summary>
		MSCHAPv2               = 26
	}

	/// <summary>
	/// Flags used for controlling how EAP is used
	/// </summary>
    [Obsolete("This enum is obsolete and will be removed in a future version of the SDF.  Consider using OpenNETCF.Net.NetworkInformation.EAPFlags instead", false)]
    [FlagsAttribute]
	public enum EAPFlags : int
	{
		/// <summary>
		/// No EAP
		/// </summary>
		Disabled                  = 0,

		/// <summary>
		/// EAP is on
		/// </summary>
		Enabled                   = -2147483648,	// 0x80000000,

		/// <summary>
		/// EAP authentication by user
		/// </summary>
		MachineAuthenticationDisabled     = 0,

		/// <summary>
		/// EAP authentication by machine (as opposed to user,
		/// presumably)
		/// </summary>
		MachineAuthenticationEnabled      = 0x40000000,

		/// <summary>
		/// EAP guest login not allowed
		/// </summary>
		GuestAuthenticationDisabled       = 0,

		/// <summary>
		/// EAP gues login allowed
		/// </summary>
		GuestAuthenticationEnabled        = 0x20000000,

		/// <summary>
		/// This is the default configuration for all connections
		/// in WZC
		/// </summary>
		DefaultState             = Enabled,

		/// <summary>
		/// This is the default state of the machine authentication
		/// flag in WZC
		/// </summary>
		DefaultMachineAuthentication      = MachineAuthenticationEnabled,

		/// <summary>
		/// This is the default state of the guest authentication
		/// flag in WZC
		/// </summary>
		DefaultGuestAuthentication        = GuestAuthenticationDisabled,

		/// <summary>
		/// This is the default set of flags set by WZC for 
		/// connections
		/// </summary>
		DefaultWZCFlags               = (DefaultState | DefaultMachineAuthentication | DefaultGuestAuthentication),
	}

    [Flags]
    internal enum INTFFlags : uint
    {
        INTF_ALL = 0xffffffff,

        INTF_ALL_FLAGS = 0x0000ffff,
        /// <summary>
        /// mask for the configuration mode (NDIS_802_11_NETWORK_INFRASTRUCTURE value)
        /// </summary>
        INTF_CM_MASK = 0x00000007,
        /// <summary>
        /// zero conf enabled for this interface
        /// </summary>
        INTF_ENABLED = 0x00008000,
        /// <summary>
        /// attempt to connect to visible non-preferred networks also
        /// </summary>
        INTF_FALLBACK = 0x00004000,
        /// <summary>
        /// 802.11 OIDs are supported by the driver/firmware (can't be set)
        /// </summary>
        INTF_OIDSSUPP = 0x00002000,
        /// <summary>
        /// the service parameters are volatile.
        /// </summary>
        INTF_VOLATILE = 0x00001000,
        /// <summary>
        /// the service parameters are enforced by the policy.
        /// </summary>
        INTF_POLICY = 0x00000800,

        INTF_DESCR = 0x00010000,
        INTF_NDISMEDIA = 0x00020000,
        INTF_PREFLIST = 0x00040000,
        INTF_CAPABILITIES = 0x00080000,


        INTF_ALL_OIDS = 0xfff00000,
        INTF_HANDLE = 0x00100000,
        INTF_INFRAMODE = 0x00200000,
        INTF_AUTHMODE = 0x00400000,
        INTF_WEPSTATUS = 0x00800000,
        INTF_SSID = 0x01000000,
        INTF_BSSID = 0x02000000,
        INTF_BSSIDLIST = 0x04000000,
        INTF_LIST_SCAN = 0x08000000,
        INTF_ADDWEPKEY = 0x10000000,
        INTF_REMWEPKEY = 0x20000000,
        /// <summary>
        /// reload the default WEP_KEY
        /// </summary>
        INTF_LDDEFWKEY = 0x40000000,
    }

}
