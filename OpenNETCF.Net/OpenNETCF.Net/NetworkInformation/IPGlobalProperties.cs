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
using System.Net.Sockets;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Provides information about the network connectivity of the local computer.
    /// </summary>
    public class IPGlobalProperties
    {
        private const int MAX_HOSTNAME_LEN = 128;
        private const int MAX_DOMAIN_NAME_LEN = 128;
        private const int MAX_SCOPE_ID_LEN = 256;

        private const int HOSTNAME_OFFSET = 0;
        private const int DOMAIN_OFFSET = 132;
        private const int SCOPE_OFFSET = 312;
        private const int STRUCT_LENGTH = 1024;

        private string m_hostName;
        private string m_domainName;
        private string m_scopeId;

        private IPGlobalProperties()
        {
            /*
            #define MAX_HOSTNAME_LEN                128 // arb.
            #define MAX_DOMAIN_NAME_LEN             128 // arb.
            #define MAX_SCOPE_ID_LEN                256 // arb.

             typedef struct {
              000 char HostName [MAX_HOSTNAME_LEN + 4];
              132 char DomainName [MAX_DOMAIN_NAME_LEN + 4];
              264 PIP_ADDR_STRING CurrentDnsServer;
              268 IP_ADDR_STRING DnsServerList; 
              308 UINT NodeType;
              312 char ScopeId [MAX_SCOPE_ID_LEN + 4];
              UINT EnableRouting;
              UINT EnableProxy;
              UINT EnableDns;
            } FIXED_INFO, *PFIXED_INFO;
            */

            uint length = STRUCT_LENGTH;
            byte[] data = new byte[length];

            int errorCode = NativeMethods.GetNetworkParams(data, ref length);

            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }

            m_hostName = Encoding.ASCII.GetString(data, HOSTNAME_OFFSET, MAX_HOSTNAME_LEN);
            int nullIndex = m_hostName.IndexOf('\0');
            if (nullIndex >= 0)
            {
                m_hostName = m_hostName.Substring(0, nullIndex);
            }

            m_domainName = Encoding.ASCII.GetString(data, DOMAIN_OFFSET, MAX_DOMAIN_NAME_LEN);
            nullIndex = m_domainName.IndexOf('\0');
            if (nullIndex >= 0)
            {
                m_domainName = m_domainName.Substring(0, nullIndex);
            }

            m_scopeId = Encoding.ASCII.GetString(data, SCOPE_OFFSET, MAX_SCOPE_ID_LEN);
            nullIndex = m_scopeId.IndexOf('\0');
            if (nullIndex >= 0)
            {
                m_scopeId = m_scopeId.Substring(0, nullIndex);
            }
        }

        /// <summary>
        /// Provides Internet Protocol version 4 (IPv4) statistical data for the local
        /// computer.
        /// </summary>
        /// <returns>
        /// An OpenNETCF.Net.NetworkInformation.IPGlobalStatistics object that provides
        /// IPv4 traffic statistics for the local computer.
        /// </returns>
        /// <exception cref="OpenNETCF.Net.NetworkInformation">The call to the Win32 function GetIpStatistics failed.</exception>
        public IPGlobalStatistics GetIPv4GlobalStatistics()
        {
            MibIpStats stats = new MibIpStats();
            int errorCode = NativeMethods.GetIpStatisticsEx(out stats, System.Net.Sockets.AddressFamily.InterNetwork);

            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }

            return new IPGlobalStatistics(stats);
        }

        /// <summary>
        /// Provides Internet Protocol version 6 (IPv6) statistical data for the local
        ///  computer.
        /// </summary>
        /// <returns>
        /// An OpenNETCF.Net.NetworkInformation.IPGlobalStatistics object that provides
        /// IPv6 traffic statistics for the local computer.
        /// </returns>
        /// <exception cref="OpenNETCF.Net.NetworkInformation.NetworkInformationException">The call to the Win32 function GetIpStatistics failed.</exception>
        /// <exception cref="System.PlatformNotSupportedException">The local computer is not running an operating system that supports IPv6.</exception>
        public IPGlobalStatistics GetIPv6GlobalStatistics()
        {
            MibIpStats stats = new MibIpStats();
            int errorCode = NativeMethods.GetIpStatisticsEx(out stats, System.Net.Sockets.AddressFamily.InterNetworkV6);

            if (errorCode == NativeMethods.NOT_SUPPORTED)
            {
                throw new System.PlatformNotSupportedException("The local device does not support IPv6");
            }

            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }

            return new IPGlobalStatistics(stats);
        }

        /// <summary>
        /// Gets an object that provides information about the local computer's network
        /// connectivity and traffic statistics.
        /// </summary>
        /// <returns>
        /// A OpenNETCF.Net.NetworkInformation.IPGlobalProperties object that contains information
        /// about the local computer.
        /// </returns>
        public static IPGlobalProperties GetIPGlobalProperties()
        {
            return new IPGlobalProperties();
        }

        /// <summary>
        /// Gets the Dynamic Host Configuration Protocol (DHCP) scope name.
        /// </summary>
        public string DhcpScopeName
        {
            get { return m_scopeId; }
        }

        /// <summary>
        /// Gets the domain in which the local computer is registered.
        /// </summary>
        public string DomainName
        {
            get { return m_domainName; }
        }

        /// <summary>
        /// Gets the host name for the local computer.
        /// </summary>
        public string HostName
        {
            get { return m_hostName; }
        }

        /// <summary>
        /// Sets the state for a specified TCP connection
        /// </summary>
        /// <param name="localAddress">Local IP address for the connection</param>
        /// <param name="localPort">Local port for the connection</param>
        /// <param name="remoteAddress">Remote IP address for the connection</param>
        /// <param name="remotePort">Remote port for the connection</param>
        /// <param name="newState">New state for the connection</param>
        public void SetTcpState(IPAddress localAddress, int localPort, IPAddress remoteAddress, int remotePort, TcpState newState)
        {
            MIB_TCPROW row = new MIB_TCPROW();
            row.dwLocalAddr = BitConverter.ToUInt32(localAddress.GetAddressBytes(), 0);
            row.dwLocalPort = (uint)localPort;
            row.dwRemoteAddr = BitConverter.ToUInt32(remoteAddress.GetAddressBytes(), 0);
            row.dwRemotePort = (uint)remotePort;
            row.dwState = newState;

            int errorCode = NativeMethods.SetTcpEntry(ref row);

            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }
        }

        /// <summary>
        /// Returns information about the Internet Protocol version 4 (IPV4) Transmission
        /// Control Protocol (TCP) connections on the local computer.
        /// </summary>
        /// <returns>
        /// An OpenNETCF.Net.NetworkInformation.TcpConnectionInformation array that contains
        /// objects that describe the active TCP connections, or an empty array if no
        /// active TCP connections are detected.
        /// </returns>
        /// <exception cref="OpenNETCF.Net.NetworkInformation.NetworkInformationException">
        /// The Win32 function GetTcpTable failed.
        /// </exception>
        public TcpConnectionInformation[] GetActiveTcpConnections()
        {
            byte[] data;
            uint size = 0;

            NativeMethods.GetTcpTable(IntPtr.Zero, ref size, 1);
            data = new byte[size];
            int errorCode = NativeMethods.GetTcpTable(data, ref size, 1);

            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }

            /*
            typedef struct _MIB_TCPTABLE {
              DWORD dwNumEntries; 
              MIB_TCPROW table[ANY_SIZE]; 
            } MIB_TCPTABLE, *PMIB_TCPTABLE;
            */
            int entries = BitConverter.ToInt32(data, 0);
            TcpConnectionInformation[] connectionList = new TcpConnectionInformation[entries];

            int offset = 4;

            for (int i = 0; i < entries; i++)
            {
                connectionList[i] = new TcpConnectionInformation();
                Buffer.BlockCopy(data, offset, connectionList[i], 0, TcpConnectionInformation.NATIVE_SIZE);
                offset += TcpConnectionInformation.NATIVE_SIZE;
            }

            return connectionList;
        }

        /// <summary>
        /// Returns information about the Internet Protocol version 4 (IPv4) User Datagram
        /// Protocol (UDP) listeners on the local computer.
        /// </summary>
        /// <returns>
        /// An System.Net.IPEndPoint array that contains objects that describe the UDP
        /// listeners, or an empty array if no UDP listeners are detected.
        /// </returns>
        /// <exception cref="OpenNETCF.Net.NetworkInformation.NetworkInformationException">
        /// The call to the Win32 function GetUdpTable failed.
        /// </exception>
        public IPEndPoint[] GetActiveUdpListeners()
        {
            byte[] data;
            uint size = 0;

            NativeMethods.GetUdpTable(IntPtr.Zero, ref size, 1);
            data = new byte[size];
            int errorCode = NativeMethods.GetUdpTable(data, ref size, 1);

            if (errorCode != NativeMethods.NO_ERROR)
            {
                throw new NetworkInformationException(errorCode);
            }
            /*
            typedef struct _MIB_UDPTABLE {
              DWORD dwNumEntries; 
              MIB_UDPROW table[ANY_SIZE]; 
            } MIB_UDPTABLE, *PMIB_UDPTABLE;

            typedef struct _MIB_UDPROW {
              DWORD dwLocalAddr; 
              DWORD dwLocalPort; 
            } MIB_UDPROW, *PMIB_UDPROW;
            */
            int entries = BitConverter.ToInt32(data, 0);
            IPEndPoint[] connectionList = new IPEndPoint[entries];

            int offset = 4;

            for (int i = 0; i < entries; i++)
            {
                IPAddress address = new IPAddress(BitConverter.ToUInt32(data, offset));
                offset += 4;
                int port = BitConverter.ToInt32(data, offset);
                offset += 4;
                connectionList[i] = new IPEndPoint(address, port);
            }

            return connectionList;
        }

        /// <summary>
        /// Provides Internet Control Message Protocol (ICMP) version 4 statistical data
        /// for the local computer.
        /// </summary>
        /// <returns>
        /// An OpenNETCF.Net.NetworkInformation.IcmpV4Statistics object that provides ICMP
        /// version 4 traffic statistics for the local computer.
        /// </returns>
        /// <exception cref="OpenNETCF.Net.NetworkInformation.NetworkInformationException">
        /// The Win32 function GetIcmpStatistics failed.
        /// </exception>
        public IcmpV4Statistics GetIcmpV4Statistics()
        {
            return new IcmpV4Statistics();
        }

        /// <summary>
        /// Provides Transmission Control Protocol/Internet Protocol version 4 (TCP/IPv4)
        /// statistical data for the local computer.
        /// </summary>
        /// <returns>
        /// A OpenNETCF.Net.NetworkInformation.TcpStatistics object that provides TCP/IPv4
        /// traffic statistics for the local computer.
        /// </returns>
        /// <exception cref="OpenNETCF.Net.NetworkInformation.NetworkInformationException">
        /// The call to the Win32 function GetTcpStatistics failed.
        /// </exception>
        public TcpStatistics GetTcpIPv4Statistics()
        {
            return new TcpStatistics(AddressFamily.InterNetwork);
        }

        /// <summary>
        /// Provides Transmission Control Protocol/Internet Protocol version 6 (TCP/IPv6)
        /// statistical data for the local computer.
        /// </summary>
        /// <returns>
        /// A OpenNETCF.Net.NetworkInformation.TcpStatistics object that provides TCP/IPv6
        /// traffic statistics for the local computer.
        /// </returns>
        /// <exception cref="OpenNETCF.Net.NetworkInformation.NetworkInformationException">
        /// The call to the Win32 function GetTcpStatistics failed.
        /// </exception>
        /// <exception cref="System.PlatformNotSupportedException">The local computer is not running an operating system that supports IPv6.</exception>
        public TcpStatistics GetTcpIPv6Statistics()
        {
            return new TcpStatistics(AddressFamily.InterNetworkV6);
        }

        /// <summary>
        /// Provides User Datagram Protocol/Internet Protocol version 4 (UDP/IPv4) statistical
        /// data for the local computer.
        /// </summary>
        /// <returns>
        /// An OpenNETCF.Net.NetworkInformation.UdpStatistics object that provides UDP/IPv4
        /// traffic statistics for the local computer.
        /// </returns>
        /// <exception cref="OpenNETCF.Net.NetworkInformation.NetworkInformationException">The call to the Win32 function GetUdpStatistics failed.</exception>
        public UdpStatistics GetUdpIPv4Statistics()
        {
            return new UdpStatistics(AddressFamily.InterNetwork);
        }

        /// <summary>
        /// Provides User Datagram Protocol/Internet Protocol version 6 (UDP/IPv6) statistical
        /// data for the local computer.
        /// </summary>
        /// <returns>
        /// An OpenNETCF.Net.NetworkInformation.UdpStatistics object that provides UDP/IPv6
        /// traffic statistics for the local computer.
        /// </returns>
        /// <exception cref="OpenNETCF.Net.NetworkInformation.NetworkInformationException">The call to the Win32 function GetUdpStatistics failed.</exception>
        /// <exception cref="System.PlatformNotSupportedException">The local computer is not running an operating system that supports IPv6.</exception>
        public UdpStatistics GetUdpIPv6Statistics()
        {
            return new UdpStatistics(AddressFamily.InterNetworkV6);
        }

        /*
                //
                // Summary:
                //     Returns endpoint information about the Internet Protocol version 4 (IPV4)
                //     Transmission Control Protocol (TCP) listeners on the local computer.
                //
                // Returns:
                //     A System.Net.IPEndPoint array that contains objects that describe the active
                //     TCP listeners, or an empty array, if no active TCP listeners are detected.
                //
                // Exceptions:
                //   OpenNETCF.Net.NetworkInformation.NetworkInformationException:
                //     The Win32 function GetTcpTable failed.
                public abstract IPEndPoint[] GetActiveTcpListeners();
                //
                // Summary:
                //     Provides Internet Control Message Protocol (ICMP) version 6 statistical data
                //     for the local computer.
                //
                // Returns:
                //     An OpenNETCF.Net.NetworkInformation.IcmpV6Statistics object that provides ICMP
                //     version 6 traffic statistics for the local computer.
                //
                // Exceptions:
                //   System.PlatformNotSupportedException:
                //     The local computer's operating system is not Windows XP or later.
                //
                //   OpenNETCF.Net.NetworkInformation.NetworkInformationException:
                //     The Win32 function GetIcmpStatisticsEx failed.
                public abstract IcmpV6Statistics GetIcmpV6Statistics();

        */
    }
}
