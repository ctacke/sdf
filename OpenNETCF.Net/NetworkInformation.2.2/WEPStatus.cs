using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /*
    typedef enum _NDIS_802_11_WEP_STATUS
    {
        Ndis802_11WEPEnabled,
        Ndis802_11Encryption1Enabled = Ndis802_11WEPEnabled,    
        Ndis802_11WEPDisabled,
        Ndis802_11EncryptionDisabled = Ndis802_11WEPDisabled,    
        Ndis802_11WEPKeyAbsent,
        Ndis802_11Encryption1KeyAbsent = Ndis802_11WEPKeyAbsent,
        Ndis802_11WEPNotSupported,
        Ndis802_11EncryptionNotSupported = Ndis802_11WEPNotSupported,
        Ndis802_11Encryption2Enabled,
        Ndis802_11Encryption2KeyAbsent,
        Ndis802_11Encryption3Enabled,
        Ndis802_11Encryption3KeyAbsent
    } NDIS_802_11_WEP_STATUS, *PNDIS_802_11_WEP_STATUS,
      NDIS_802_11_ENCRYPTION_STATUS, *PNDIS_802_11_ENCRYPTION_STATUS;
    */
    /// <summary>
    /// Define WEP authentication state for the adapter.
    /// </summary>
    public enum WEPStatus
    {
        /// <summary>
        /// WEP encryption enabled
        /// </summary>
        WEPEnabled = 0,
        /// <summary>
        /// No WEP encryption
        /// </summary>
        WEPDisabled = 1,
        /// <summary>
        /// WEP, TKIP and AES are disabled. A transmit key is not available.
        /// </summary>
        WEPKeyAbsent = 2,
        /// <summary>
        /// WEP is not supported
        /// </summary>
        WEPNotSupported = 3,
        /// <summary>
        /// TKIP and WEP are enabled; AES is disabled. A transmit key is available
        /// </summary>
        TKIPEnabled = 4,
        /// <summary>
        /// TKIP and WEP are enabled; AES is disabled. A transmit key is not available
        /// </summary>
        TKIPKeyAbsent = 5,
        /// <summary>
        /// AES, TKIP, and WEP are enabled, and a transmit key is available.
        /// </summary>
        AESEnabled = 6,
        /// <summary>
        /// AES, TKIP, and WEP are enabled. A transmit keys is not available.
        /// </summary>
        AESKeyAbsent = 7,
        /// <summary>
        /// Unknown or unresolvable WEP Status
        /// </summary>
        Unknown = -1
    }
}
