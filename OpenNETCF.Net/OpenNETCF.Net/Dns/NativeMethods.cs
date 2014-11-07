using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;
using System.Net;

namespace OpenNETCF.Net
{
    // from: http://msdn.microsoft.com/en-us/library/aa921060.aspx
    internal static partial class NativeMethods
    {       
        //DNS_STATUS WINAPI DnsReplaceRecordSetW(
        //  PDNS_RECORD pNewSet,
        //  DWORD Options,
        //  HANDLE hContext,
        //  PIP_ARRAY pServerList,
        //  PVOID pReserved
        //);

        // -- use for CE --
        [DllImport("dnsapi.dll", SetLastError = true)]
        internal static extern int DnsReplaceRecordSetW(IntPtr pNewSet, DnsUpdateSecurity Options, ref SEC_WINNT_AUTH_IDENTITY hContext, IP4_ARRAY pServerList, IntPtr pReserved);

        // -- use on desktop --
        [DllImport("dnsapi.dll", SetLastError = true)]
        internal static extern int DnsReplaceRecordSetW(IntPtr pNewSet, DnsUpdateSecurity Options, IntPtr hContext, IP4_ARRAY pServerList, IntPtr pReserved);

        //DNS_STATUS
        //WINAPI
        //DnsQuery_W(
        //    IN      PCWSTR          pszName,
        //    IN      WORD            wType,
        //    IN      DWORD           Options,                         
        //    IN      PIP4_ARRAY      aipServers            OPTIONAL,
        //    IN OUT  PDNS_RECORD *   ppQueryResults        OPTIONAL,
        //    IN OUT  PVOID *         pReserved             OPTIONAL
        //    );
        [DllImport("dnsapi.dll", SetLastError = true)]
        internal static extern int DnsQuery_W([MarshalAs(UnmanagedType.LPWStr)] string pszName, DNS_TYPE wType, QueryOptions Options, IP4_ARRAY aipServers, ref IntPtr ppQueryResults, IntPtr pReserved);

        //void WINAPI DnsRecordListFree(
        //  PDNS_RECORD pRecordList,
        //  DNS_FREE_TYPE FreeType
        //);    
        [DllImport("dnsapi.dll", SetLastError = true)]
        internal static extern int DnsRecordListFree(IntPtr pRecordList, DNS_FREE_TYPE FreeType);

        // desktop only
        [DllImport("dnsapi.dll", SetLastError = true, EntryPoint = "DnsAcquireContextHandle_W")]
        internal static extern int DnsAcquireContextHandle(uint CredentialFlags, ref SEC_WINNT_AUTH_IDENTITY Credentials, out IntPtr pContext);

        // desktop only
        [DllImport("dnsapi.dll", SetLastError = true, EntryPoint = "DnsAcquireContextHandle_W")]
        internal static extern int DnsReleaseContextHandle(IntPtr pContext);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct DNS_SRV_DATA
    {
        public string pNameTarget;
        public short wPriority;
        public short wWeight;
        public short wPort;
        public short wPad;

        //typedef struct {
        //  PWSTR pNameTarget;
        //  WORD  wPriority;
        //  WORD  wWeight;
        //  WORD  wPort;
        //  WORD  Pad;
        //} DNS_SRV_DATA, 
        // *PDNS_SRV_DATA;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    internal struct DNS_SOA_DATA
    {
        public string pNamePrimaryServer;
        public string pNameAdministrator;
        public uint dwSerialNo;
        public uint dwRefresh;
        public uint dwRetry;
        public uint dwExpire;
        public uint dwDefaultTtl;

        //typedef struct {
        //  LPTSTR pNamePrimaryServer;
        //  LPTSTR pNameAdministrator;
        //  DWORD dwSerialNo;
        //  DWORD dwRefresh;
        //  DWORD dwRetry;
        //  DWORD dwExpire;
        //  DWORD dwDefaultTtl;
        //} DNS_SOA_DATA, *PDNS_SOA_DATA;
    }
}
