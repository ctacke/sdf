using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Define authentication types for an adapter.
    /// </summary>
    public enum AuthenticationMode
    {
        /// <summary>
        /// Specifies IEEE 802.11 Open System authentication mode. In this mode, there are no checks performed during the 802.11 authentication
        /// </summary>
        Open = 0,
        /// <summary>
        /// Specifies IEEE 802.11 Shared Key authentication mode. This mode requires the use of a pre-shared Wired Equivalent Privacy (WEP) key for the 802.11 authentication
        /// </summary>
        Shared = 1,
        /// <summary>
        /// Specifies auto-switch mode. When using auto-switch mode, the device tries IEEE 802.11 Shared Key authentication mode first. If Shared Key authentication fails, the device attempts to use IEEE 802.11 Open System authentication mode.
        /// <remarks><b>Note</b> The use of this setting is not recommended</remarks> 
        /// </summary>
        AutoSwitch = 2,
        /// <summary>
        /// Specifies WPA version 1 security for infrastructure mode. Authentication is performed between the supplicant, authenticator, and authentication server over IEEE 802.1X. Encryption keys are dynamic and are derived through the authentication process
        /// While in this authentication mode, the device will only associate with an access point whose beacon or probe responses contain the authentication suite of type 1 (802.1X) within the WPA information element (IE).
        /// This authentication mode is only valid for infrastructure network modes. The driver must return NDIS_STATUS_NOT_ACCEPTED if its network mode is set to ad hoc.
        /// </summary>
        WPA = 3,
        /// <summary>
        /// Specifies WPA version 1 security for infrastructure mode. Authentication is made between the supplicant and authenticator over IEEE 802.1X. Encryption keys are dynamic and are derived through a pre-shared key used on both the supplicant and authenticator.
        /// While in this authentication mode, the device will only associate with an access point whose beacon or probe responses contain the authentication suite of type 2 (pre-shared key) within the WPA information element (IE).
        /// This authentication mode is only valid for infrastructure network modes. The driver must return NDIS_STATUS_NOT_ACCEPTED if its network mode is set to ad hoc.
        /// </summary>
        WPAPSK = 4,
        /// <summary>
        /// Specifies WPA version 1 security for ad hoc mode. This setting specifies the use of a pre-shared key without IEEE 802.1X authentication. Encryption keys are static and are derived through the pre-shared key.
        /// This authentication mode is only valid for ad hoc network modes. The driver must return NDIS_STATUS_NOT_ACCEPTED if its network mode is set to infrastructure.
        /// </summary>
        WPAAdHoc = 5,
        /// <summary>
        /// The authentication mode cannot be determined.
        /// </summary>
        Unknown = -1
    }
}
