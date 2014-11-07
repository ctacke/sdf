using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /*
        typedef struct
        {
    04        ULONG                               Length;             // Length of this structure
    04        DWORD                               dwCtlFlags;         // control flags (NON-NDIS) see WZC_WEPK* constants
            // fields from the NDIS_WLAN_BSSID structure
    06        NDIS_802_11_MAC_ADDRESS             MacAddress;         // BSSID
    02        UCHAR                               Reserved[2];
    36        NDIS_802_11_SSID                    Ssid;               // SSID
    04        ULONG                               Privacy;            // WEP encryption requirement
    04        NDIS_802_11_RSSI                    Rssi;               // receive signal strength in dBm
    04        NDIS_802_11_NETWORK_TYPE            NetworkTypeInUse;
    32        NDIS_802_11_CONFIGURATION           Configuration;
    04        NDIS_802_11_NETWORK_INFRASTRUCTURE  InfrastructureMode;
    04        NDIS_802_11_RATES                   SupportedRates;
            // fields from NDIS_802_11_WEP structure
    04        ULONG   KeyIndex;                               // 0 is the per-client key, 1-N are the global keys
    04        ULONG   KeyLength;                              // length of key in bytes
    32        UCHAR   KeyMaterial[WZCCTL_MAX_WEPK_MATERIAL];  // variable length depending on above field
            // aditional field for the Authentication mode
    04        NDIS_802_11_AUTHENTICATION_MODE     AuthenticationMode;
    08        RAW_DATA                            rdUserData;         // upper level buffer, attached to this config

        #ifdef UNDER_CE
    20        WZC_EAPOL_PARAMS                    EapolParams;		// 802.1x parameters
        #endif

    08        RAW_DATA                            rdNetworkData;      // upper level buffer, attached to this config
    04        ULONG                               WPAMCastCipher;     // Multicast Cipher Suite for WPA. 
    04        ULONG                               ulMediaType;        // 

        } WZC_WLAN_CONFIG, *PWZC_WLAN_CONFIG;
    
    total size = 196 bytes
    */
    /// <summary>
    /// Wireless LAN config descriptor
    /// </summary>
    internal class WLANConfiguration : SelfMarshalledStruct
    {
        private const int LENGTH_OFFSET = 0;
        private const int LENGTH_LENGTH = 4;
        private const int CTL_FLAGS_OFFSET = LENGTH_OFFSET + LENGTH_LENGTH;
        private const int CTL_FLAGS_LENGTH = 4;
        private const int MAC_OFFSET = CTL_FLAGS_OFFSET + CTL_FLAGS_LENGTH;
        private const int MAC_LENGTH = 6;
        private const int RESERVED_OFFSET = MAC_OFFSET + MAC_LENGTH;
        private const int RESERVED_LENGTH = 2;
        private const int SSID_OFFSET = RESERVED_OFFSET + RESERVED_LENGTH;
        private const int SSID_LENGTH = 36;

        private const int RADIO_CONFIG_OFFSET = 64;
        private const int CAST_CIPHER_OFFSET = 192;

        public WLANConfiguration()
            : this(SizeOf)
        {
        }

        public WLANConfiguration(int size)
            : base(size)
        {
            Length = this.Data.Length;
        }

        /// <summary>
        /// Length of this structure
        /// </summary>
        public int Length { get { return GetInt32(LENGTH_OFFSET); } set { SetInt32(LENGTH_OFFSET, value); } }
        /// <summary>
        /// control flags (NON-NDIS) see WZC_WEPK* constants
        /// </summary>
        public WZCControl CtlFlags { get { return (WZCControl)GetUInt32(CTL_FLAGS_OFFSET); } set { SetUInt32(CTL_FLAGS_OFFSET, (uint)value); } }
        /// <summary>
        /// MAC Address
        /// </summary>
        public byte[] MACAddress { get { byte[] ret = new byte[MAC_LENGTH]; Buffer.BlockCopy(data, MAC_OFFSET, ret, 0, MAC_LENGTH); return ret; } set { value.CopyTo(data, MAC_OFFSET); } }
        /// <summary>
        /// SSID
        /// </summary>
        public string SSID
        {
            get
            {
                int cb = GetInt32(16);
                if (cb > 32)
                {
                    return Encoding.ASCII.GetString(data, SSID_OFFSET, 32).TrimEnd('\0');
                }
                else
                {
                    return Encoding.ASCII.GetString(data, SSID_OFFSET + 4, cb);
                }
            }
            set
            {
                if (value.Length > 32) value = value.Substring(32);
                SetInt32(16, value.Length);
                Buffer.BlockCopy(Encoding.ASCII.GetBytes(value), 0, data, SSID_OFFSET + 4, value.Length);
            }
        }
        /// <summary>
        /// WEP status
        /// </summary>
        public WEPStatus Privacy { get { return (WEPStatus)GetInt32(52); } set { SetInt32(52, (int)value); } }
        /// <summary>
        /// Receive signal strength in dBm
        /// </summary>
        public int Rssi { get { return GetInt32(56); } set { SetInt32(56, value); } }
        /// <summary>
        /// Network type
        /// </summary>
        public NetworkType NetworkTypeInUse { get { return (NetworkType)GetInt32(60); } set { SetInt32(60, (int)value); } }
        /// <summary>
        /// Infrastructure mode
        /// </summary>
        public InfrastructureMode InfrastructureMode { get { return (InfrastructureMode)GetInt32(96); } set { SetInt32(96, (int)value); } }
        /// <summary>
        /// Supported data rates
        /// </summary>
        public byte[] Rates { get { byte[] ret = new byte[8]; Buffer.BlockCopy(data, 100, ret, 0, 8); return ret; } }
        /// <summary>
        /// Data rates in Mbps
        /// </summary>
        public float[] RatesConverted
        {
            get
            {
                int cnt = 0;
                foreach (byte b in Rates) cnt += b == 0 ? 0 : 1;
                float[] arrRates = new float[cnt];
                for (int i = 0; i < cnt; i++) arrRates[i] = (float)(Rates[i] & 0x7f) / 2;
                return arrRates;
            }
        }
        /// <summary>
        /// Current connection speed (data rate)
        /// </summary>
        public float CurrentDataRate
        {
            get
            {
                float ret = 0;
                foreach (byte b in Rates)
                    if ((b & 0x80) != 0)
                        ret = (float)(b & 0x7f) / 2;
                return ret;
            }
        }
        /// <summary>
        /// Selected key index
        /// 0 is the per-client key, 1-N are the global keys
        /// </summary>
        public int KeyIndex { get { return GetInt32(108); } set { SetInt32(108, value); } }
        /// <summary>
        /// Key length
        /// </summary>
        public int KeyLength { get { return GetInt32(112); } set { SetInt32(112, value); } }
        /// <summary>
        /// Key data
        /// </summary>
        public byte[] KeyMaterial
        {
            get
            {
                byte[] ret = new byte[KeyLength];
                Buffer.BlockCopy(data, 116, ret, 0, KeyLength);
                return ret;
            }
            set
            {
                KeyLength = Math.Min(WZCCTL_MAX_WEPK_MATERIAL, value.Length);
                Buffer.BlockCopy(value, 0, data, 116, KeyLength);
            }
        }

        /// <summary>
        /// Authentication mode
        /// </summary>
        public AuthenticationMode AuthenticationMode { get { return (AuthenticationMode)GetUInt32(148); } set { SetUInt32(148, (uint)value); } }

        // 152 rdUserData

        /// <summary>
        /// EAP parameters
        /// </summary>
        public EAPParameters EapolParams
        {
            get
            {
                return new EAPParameters(data, 160);
            }
            set
            {
                // Copy the content of the incoming value
                // over the top.
                Buffer.BlockCopy(value.Data, 0, data, 160, EAPParameters.SizeOf);
            }
        }

        public RadioConfiguration Configuration
        {
            get
            {
                return new RadioConfiguration(data, RADIO_CONFIG_OFFSET);
            }
        }

        public int WPAMCastCipher { get { return GetInt32(CAST_CIPHER_OFFSET); } set { SetInt32(CAST_CIPHER_OFFSET, value); } }


        public override string ToString()
        {
            return SSID;
        }

        public BSSID ToBssid(bool isWzc)
        {
            // Create a new BSSID structure from the information
            // in our own structure.  It's in the right order and 
            // everything.  Note that the signal strength does not
            // represent the signal strength currently, but when
            // the SSID was added to the preferred list, if that's
            // what we're constructing here.

            // First, since our entry list doesn't perfectly match
            // the layout (the length field is not there), we have
            // to build a data array.  We'll copy everything, starting
            // where the length would be (it's a flags field, in our
            // case), then rewrite the Length.
            /*
            4	ULONG                               Length;             // Length of this structure
            6	NDIS_802_11_MAC_ADDRESS             MacAddress;         // BSSID
            2	UCHAR                               Reserved[2];
            36	NDIS_802_11_SSID                    Ssid;               // SSID
            4	ULONG                               Privacy;            // WEP encryption requirement
            4	NDIS_802_11_RSSI                    Rssi;               // receive signal
            // strength in dBm
            4	NDIS_802_11_NETWORK_TYPE            NetworkTypeInUse;
            32	NDIS_802_11_CONFIGURATION           Configuration;
                    {
                    ULONG           Length;             // Length of structure
                    ULONG           BeaconPeriod;       // units are Kusec
                    ULONG           ATIMWindow;         // units are Kusec
                    ULONG           DSConfig;           // Frequency, units are kHz
                    NDIS_802_11_CONFIGURATION_FH    FHConfig;
                        {
                            ULONG           Length;             // Length of structure
                            ULONG           HopPattern;         // As defined by 802.11, MSB set
                            ULONG           HopSet;             // to one if non-802.11
                            ULONG           DwellTime;          // units are Kusec
                        } 
                    }
            4	NDIS_802_11_NETWORK_INFRASTRUCTURE  InfrastructureMode;
            16	NDIS_802_11_RATES                   SupportedRates;
            */
            int len = 4 + 6 + 2 + 36 + 4 + 4 + 4 + 32 + 4 + 8;
            
            if (isWzc) len += 4;

            byte[] d = new byte[len];

            Buffer.BlockCopy(data, 4 /* MacAddressOffset-4, effectively */,
                d, 0, d.Length);

            Buffer.BlockCopy(BitConverter.GetBytes(len), 0,
                d, 0, 4);

            // Build the NDIS_WLAN_BSSID from this array we've just
            // created.
            return new BSSID(d, 0, false);
        }

        internal const int WZCCTL_MAX_WEPK_MATERIAL = 32;
        internal const int SizeOf = 196;
    }
}
