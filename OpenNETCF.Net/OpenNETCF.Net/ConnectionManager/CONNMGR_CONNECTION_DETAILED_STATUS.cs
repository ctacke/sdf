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
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{

    internal class CONNMGR_CONNECTION_DETAILED_STATUS
    {
        public IntPtr pNext;

        public int dwVer;                // @field Structure version; current is CONNMGRDETAILEDSTATUS_VERSION.
        public DetailStatusParam dwParams;             // @field Combination of CONNMGRDETAILEDSTATUS_PARAM_* values.

        public ConnectionType dwType;               // @field One of CM_CONNTYPE_* values.
        //NOTE: the actual subtype is retrevied in ConnectionDetail 
        public int dwSubtype;            // @field One of CM_CONNSUBTYPE_* values.

        public ConnectionOption dwFlags;              // @field Combination of CM_DSF_* flags.
        public int dwSecure;             // @field Secure level (0 == not-secure) of connection.

        public Guid guidDestNet;           // @field GUID of destination network.
        public Guid guidSourceNet;         // @field GUID of source network.

        [MarshalAs(UnmanagedType.LPTStr)]
        public string szDescription;       // @field Name of connection, 0-terminated string or NULL if N/A.
        [MarshalAs(UnmanagedType.LPTStr)]
        public string szAdapterName;       // @field Name of adapter, 0-terminated or NULL if N/A.

        public ConnectionStatus dwConnectionStatus;   // @field One of CONNMGR_STATUS_*.
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public short[] LastConnectTime; // @field Time connection was last established.
        public int dwSignalQuality;      // @field Signal quality normalized in the range 0-255.

        public IntPtr pIPAddr; // @field Available IP addrs, or NULL if N/A.
        // End of version 1 fields.
    }
}
