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
using System.Text;
using System.Net.Sockets;

namespace OpenNETCF.Net.NetworkInformation
{
    internal static class NativeMethods
    {
        internal const int NO_ERROR = 0;
        internal const int NOT_SUPPORTED = 0x32;
        internal const int NO_DATA = 0xE8;
        internal const int INVALID_ARGUMENT = 0x57;

        [DllImport("coredll.dll", SetLastError = true)]
        internal static unsafe extern int SetIpNetEntry(byte[] pArpEntry);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static unsafe extern int DeleteIpNetEntry(byte[] pArpEntry);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static unsafe extern int CreateIpNetEntry(byte[] pArpEntry);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static unsafe extern int WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int NotifyRouteChange(ref IntPtr Handle, IntPtr overlapped);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int NotifyAddrChange(ref IntPtr Handle, IntPtr overlapped);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int SetTcpEntry(ref MIB_TCPROW pTcpRow);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int SendARP(uint DestIP, uint SrcIP, out uint pMacAddr, out uint PhyAddrLen);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int DeleteIpForwardEntry(byte* pRoute);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int DeleteIpForwardEntry(byte[] pRoute);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int CreateIpForwardEntry(byte[] pRoute);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int SetIpForwardEntry(byte[] pRoute);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int CreateProxyArpEntry(uint dwAddress, uint dwMask, int dwIfIndex);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int DeleteProxyArpEntry(uint dwAddress, uint dwMask, int dwIfIndex);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetBestInterface(uint DestIpAddress, ref int pdwBestIfIndex);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetRTTAndHopCount(uint DestIpAddress, ref int HopCount, int MaxHops, ref int RTT);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetIpForwardTable(IntPtr pIpForwardTable, ref int pdwSize, int bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetIpForwardTable(byte[] pIpForwardTable, ref int pdwSize, int bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int FlushIpNetTable(int dwIfIndex);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetIpNetTable(IntPtr pIpNetTable, ref int pdwSize, int bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetIpNetTable(byte[] pIpNetTable, ref int pdwSize, int bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetUdpStatisticsEx(byte[] pStats, AddressFamily dwFamily);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetTcpStatisticsEx(byte[] pStats, AddressFamily dwFamily);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetIcmpStatisticsEx(byte[] pStats, AddressFamily dwFamily);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetUdpTable(byte[] pUdpTable, ref uint pdwSize, int bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetUdpTable(IntPtr pUdpTable, ref uint pdwSize, int bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetTcpTable(byte[] pTcpTable, ref uint pdwSize, int bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetTcpTable(IntPtr pTcpTable, ref uint pdwSize, int bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetNetworkParams(byte[] pFixedInfo, ref uint dwOutBufLen);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetNetworkParams(IntPtr pFixedInfo, ref uint dwOutBufLen);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static extern int SetIpStatistics(ref MibIpStats stats);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static extern int GetIpStatisticsEx(out MibIpStats stats, AddressFamily dwFamily);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static unsafe extern int GetInterfaceInfo(byte* pIfTable, out uint dwOutBufLen);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static extern int GetInterfaceInfo(IntPtr pIfTable, out uint dwOutBufLen);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public static extern int GetAdaptersInfo(IntPtr pAdapterInfo, ref uint pOutBufLen);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public static extern int GetAdaptersInfo(byte[] pAdapterInfo, ref uint pOutBufLen);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static extern int GetNumberOfInterfaces(out int intpdwNumIf);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static extern int GetIfTable(IntPtr pIfTable, ref int pdwSize, bool bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static extern int GetIfTable(byte[] pIfTable, ref int pdwSize, bool bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal static extern int GetIfEntry(byte[] pIfRow);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal unsafe static extern int GetPerAdapterInfo(int IfIndex, byte* pPerAdapterInfo, ref int pOutBufLen);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        internal unsafe static extern int GetAdaptersAddresses(
            AddressFamily Family,
            AdapterAddressFlags Flags,
            IntPtr Reserved,
            byte* pAdapterAddresses,
            ref int pOutBufLen);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public static extern uint IpReleaseAddress(byte[] adapterInfo);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        public static extern uint IpRenewAddress(byte[] adapterInfo);

        [DllImport("wzcsapi.dll")]
        public static extern int
            WZCQueryInterface(
            string pSrvAddr,
            INTF_FLAGS dwInFlags,
            ref INTF_ENTRY pIntf,
            out INTF_FLAGS pdwOutFlags);

        [DllImport("wzcsapi.dll")]
        public static extern uint
            WZCEnumInterfaces(
            string pSrvAddr,
            ref INTFS_KEY_TABLE pIntfs);

        [DllImport("wzcsapi.dll")]
        public static extern int
            WZCSetInterface(
            string pSrvAddr,
            INTF_FLAGS dwInFlags,
            ref INTF_ENTRY pIntf,
            object pdwOutFlags);


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
        [DllImport("wzcsapi.dll", EntryPoint = "WZCPassword2Key")]
        internal static extern void
            WZCPassword2KeyCE(
            byte[] pwzcConfig,
            byte[] cszPassword);

        public static void
            WZCPassword2Key(
            ref WLANConfiguration pwzcConfig,
            string cszPassword)
        {
            // Convert string from Unicode to Ascii.
            byte[] ascii = Encoding.ASCII.GetBytes(cszPassword + '\0');

            // Pass Ascii string and configuration structure
            // to the external call.
            WZCPassword2KeyCE(pwzcConfig.Data, ascii);
        }
    }
}
