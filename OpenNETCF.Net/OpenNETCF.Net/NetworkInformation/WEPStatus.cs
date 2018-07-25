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
TKIP    Ndis802_11Encryption2Enabled,
        Ndis802_11Encryption2KeyAbsent,
AES     Ndis802_11Encryption3Enabled,
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
