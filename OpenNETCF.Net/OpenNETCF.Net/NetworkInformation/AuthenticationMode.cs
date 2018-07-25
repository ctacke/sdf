#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
// typedef enum NDIS_802_11_AUTHENTICATION_MODE {
//    Ndis802_11AuthModeOpen,
//    Ndis802_11AuthModeShared,
//    Ndis802_11AuthModeAutoSwitch,
//    Ndis802_11AuthModeWPA,
//    Ndis802_11AuthModeWPAPSK,
//    Ndis802_11AuthModeWPANone,
//    Ndis802_11AuthModeWPA2,
//    Ndis802_11AuthModeWPA2PSK,
//    Ndis802_11AuthModeMax
// } NDIS_802_11_AUTHENTICATION_MODE;
    
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
        WPA2 = 6,
        WPA2PSK = 7,
        /// <summary>
        /// The authentication mode cannot be determined.
        /// </summary>
        Unknown = -1
    }
}
