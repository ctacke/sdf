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
    /// <summary>
    /// Operational statii for a NetworkInterface
    /// </summary>
    public enum InterfaceOperationalStatus
    {
        #region --- native definitions ---
        /*
        #define MIB_IF_OPER_STATUS_NON_OPERATIONAL      0
        #define MIB_IF_OPER_STATUS_UNREACHABLE          1
        #define MIB_IF_OPER_STATUS_DISCONNECTED         2
        #define MIB_IF_OPER_STATUS_CONNECTING           3
        #define MIB_IF_OPER_STATUS_CONNECTED            4
        #define MIB_IF_OPER_STATUS_OPERATIONAL          5
        */
        #endregion
        /// <summary>
        /// LAN adapter has been disabled, for example because of an address conflict.
        /// </summary>
        NonOperational = 0,
        /// <summary>
        /// WAN adapter that is not connected.
        /// </summary>
        Unreachable = 1,
        /// <summary>
        /// For LAN adapters: network cable disconnected. For WAN adapters: no carrier
        /// </summary>
        Disconnected = 2,
        /// <summary>
        /// WAN adapter that is in the process of connecting.
        /// </summary>
        Connecting = 3,
        /// <summary>
        /// WAN adapter that is connected to a remote peer.
        /// </summary>
        Connected = 4,
        /// <summary>
        /// Default status for LAN adapters.
        /// </summary>
        Operational = 5
    }
}
