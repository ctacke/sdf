using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// Provides information about network interfaces that support Internet Protocol version 4 (IPv4) or Internet Protocol version 6 (IPv6).
	/// </summary>
	public partial class IPInterfaceProperties
	{
        internal IP_ADAPTER_INFO m_info;
        UnicastIPAddressInformationCollection m_unicastAddresses;
        IPAddressInformationCollection m_anycastAddresses;
        MulticastIPAddressInformationCollection m_multicastAddresses;
        int m_mtu = 0;

        internal IPInterfaceProperties(IP_ADAPTER_INFO info, MibIfRow mibifRow)
        {
            m_info = info;
            m_mtu = mibifRow.MTU;
        }

        /// <summary>
        /// Gets the addresses of Dynamic Host Configuration Protocol (DHCP) servers
        /// for this interface.
        /// </summary>
        public  IPAddressCollection DhcpServerAddresses 
        {
            get { return (IPAddressCollection)m_info.DHCPServerList; }
        }

        /// <summary>
        /// Gets the addresses of Domain Name System (DNS) servers for this interface.
        /// </summary>
        public IPAddressCollection DnsAddresses 
        {
            get 
            {
                this.RefreshPerAdapterInfo();
                return m_dnsList; 
            } 
        }

        /// <summary>
        /// Gets the network gateway addresses for this interface.
        /// </summary>
        public GatewayIPAddressInformationCollection GatewayAddresses
        {
            get { return m_info.GatewayList; }
        }

        /// <summary>
        /// Gets a System.Boolean value that indicates whether this interface is configured
        /// to send name resolution queries to a Domain Name System (DNS) server.
        /// </summary>
        public bool IsDnsEnabled 
        {
            get
            {
                this.RefreshPerAdapterInfo();
                return DnsAddresses.Count > 0;
            }
        }

        /// <summary>
        /// Gets the unicast addresses assigned to this interface.
        /// </summary>
        public UnicastIPAddressInformationCollection UnicastAddresses 
        {
            get
            {
                if (m_unicastAddresses == null)
                {
                    m_unicastAddresses = new UnicastIPAddressInformationCollection();

                    GetAddressesForAdapter(m_info.Index, ref m_unicastAddresses);
                }

                return m_unicastAddresses;
            }
        }

        /// <summary>
        /// Gets the anycast IP addresses assigned to this interface.
        /// </summary>
        public IPAddressInformationCollection AnycastAddresses
        {
            get
            {
                if (m_anycastAddresses == null)
                {
                    m_anycastAddresses = new IPAddressInformationCollection();

                    GetAddressesForAdapter(m_info.Index, ref m_anycastAddresses);
                }

                return m_anycastAddresses;
            }
        }

        /// <summary>
        /// Gets the multicast addresses assigned to this interface.
        /// </summary>
        public MulticastIPAddressInformationCollection MulticastAddresses
        {
            get
            {
                if (m_multicastAddresses == null)
                {
                    m_multicastAddresses = new MulticastIPAddressInformationCollection();

                    GetAddressesForAdapter(m_info.Index, ref m_multicastAddresses);
                }

                return m_multicastAddresses;
            }
        }

        /// <summary>
        /// Gets the addresses of Windows Internet Name Service (WINS) servers.
        /// </summary>
        public IPAddressCollection WinsServersAddresses 
        {
            get
            {
                IPAddressCollection coll = new IPAddressCollection();

                foreach (IPAddress ui in m_info.PrimaryWINSServer)
                {
                    coll.InternalAdd(ui);
                }
                foreach (IPAddress ui in m_info.SecondaryWINSServer)
                {
                    coll.InternalAdd(ui);
                }

                return coll;
            }
        }

        /// <summary>
        /// Provides Internet Protocol version 4 (IPv4) configuration data for this network
        /// interface.
        /// </summary>
        /// <returns>
        /// An System.Net.NetworkInformation.IPv4InterfaceProperties object that contains
        /// IPv4 configuration data, or null if no data is available for the interface.
        /// </returns>
        public IPv4InterfaceProperties GetIPv4Properties()
        {
            return new IPv4InterfaceProperties(this, m_mtu);
        }

        /// <summary>
        /// Provides Internet Protocol version 6 (IPv6) configuration data for this network
        /// interface.
        /// </summary>
        /// <returns>
        /// An System.Net.NetworkInformation.IPv6InterfaceProperties object that contains
        /// IPv6 configuration data.
        /// </returns>
        public IPv6InterfaceProperties GetIPv6Properties()
        {
            return new IPv6InterfaceProperties(this, m_mtu);
        }

        #region ---- PerAdapterInfo ----

        internal bool m_autoConfigEnabled;
        internal bool m_autoConfigActive;
        private IPAddressCollection m_dnsList = new IPAddressCollection();

        internal unsafe void RefreshPerAdapterInfo()
        {
            int size = 0;

            m_dnsList.InternalClear();

            NativeMethods.GetPerAdapterInfo(m_info.Index, null, ref size);

            byte[] data = new byte[size];

            fixed (byte* pdata = data)
            {
                byte* p = pdata;

                NativeMethods.GetPerAdapterInfo(m_info.Index, p, ref size);

                m_autoConfigEnabled = (int)*((int*)p) != 0;
                p += 4;
                m_autoConfigActive = (int)*((int*)p) != 0;
                p += 8; // skip CurrentDnsServer as it's unsupported

                m_dnsList = (IPAddressCollection)IPUtility.ParseAddressesFromPointer(p);
            } // fixed
        }
        /*
            typedef struct _IP_PER_ADAPTER_INFO {
          UINT AutoconfigEnabled;
          UINT AutoconfigActive;
          PIP_ADDR_STRING CurrentDnsServer;
          IP_ADDR_STRING DnsServerList;
        } IP_PER_ADAPTER_INFO, *PIP_PER_ADAPTER_INFO;

            typedef struct _IP_ADDR_STRING {
          struct _IP_ADDR_STRING* Next;
          IP_ADDRESS_STRING IpAddress;
          IP_MASK_STRING IpMask;
          DWORD Context;
        } IP_ADDR_STRING, *PIP_ADDR_STRING;


        typedef struct {
          char String[4 * 4];
        } IP_ADDRESS_STRING
         */
        #endregion
    }
}
