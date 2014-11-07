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

    }
}
