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
using System.Net;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{
    public static class Dns
    {
        /// <summary>
        /// Retrieves an A-record from a DNS server
        /// </summary>
        /// <param name="dnsServer"></param>
        /// <param name="name"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static ARecord[] QueryARecord(IPAddress dnsServer, string name, QueryOptions options)
        {
            IntPtr dnsRecord = IntPtr.Zero;
            IP4_ARRAY server = new IP4_ARRAY();
            server.AddrCount = 1;
            server.AddrArray = dnsServer.GetAddressBytes();
            int result = NativeMethods.DnsQuery_W(name, DNS_TYPE.TypeA, options, server, ref dnsRecord, IntPtr.Zero);

            if (result != 0) throw DnsException.FromErrorCode(result);

            using (DNS_RECORD_LIST records = DNS_RECORD_LIST.FromIntPtr(dnsRecord))
            {
                List<ARecord> list = new List<ARecord>();

                foreach (DNS_RECORD record in records)
                {
                    ARecord r = new ARecord();
                    r.Address = new IPAddress(record.data);
                    r.Name = record.pName;
                    r.TTL = (int)record.dwTtl;
                    list.Add(r);
                }

                return list.ToArray();
            }
        }

        /// <summary>
        /// Replaces an A-record on a DNS server
        /// </summary>
        /// <param name="dnsServer"></param>
        /// <param name="newRecord"></param>
        /// <param name="security"></param>
        /// <param name="username"></param>
        /// <param name="domain"></param>
        /// <param name="password"></param>
        public static void ReplaceARecord(IPAddress dnsServer, ARecord newRecord, DnsUpdateSecurity security, string username, string domain, string password)
        {
            DNS_RECORD record = new DNS_RECORD(newRecord);

            SEC_WINNT_AUTH_IDENTITY sec = new SEC_WINNT_AUTH_IDENTITY();
            sec.User = username;
            sec.UserLength = username == null ? 0 : username.Length;
            sec.Password = password;
            sec.PasswordLength = password == null ? 0 : password.Length;
            sec.Domain = domain;
            sec.DomainLength = domain == null ? 0 : domain.Length;
            sec.Flags = 2;

            IP4_ARRAY server = new IP4_ARRAY();
            server.AddrCount = 1;
            server.AddrArray = dnsServer.GetAddressBytes();

            IntPtr pRecord = record.Pin();

            try
            {
                int result = 0;

                if (Environment.OSVersion.Platform == PlatformID.WinCE)
                {
                    result = NativeMethods.DnsReplaceRecordSetW(pRecord, security, ref sec, server, IntPtr.Zero);
                }
                else
                {
                    IntPtr pCtx;
                    NativeMethods.DnsAcquireContextHandle(1, ref sec, out pCtx);
                    result = NativeMethods.DnsReplaceRecordSetW(pRecord, security, pCtx, server, IntPtr.Zero);
                    NativeMethods.DnsReleaseContextHandle(pCtx);
                }

                if (result != 0) throw DnsException.FromErrorCode(result);
            }
            finally
            {
                record.Free();
            }
        }
    }
}
