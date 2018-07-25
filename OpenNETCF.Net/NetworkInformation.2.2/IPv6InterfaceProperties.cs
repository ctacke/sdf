using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Provides information about network interfaces that support Internet Protocol
    /// version 6 (IPv6).
    /// </summary>
    public class IPv6InterfaceProperties
    {
        private IPInterfaceProperties m_props;
        private int m_mtu = 0;

        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.IPv6InterfaceProperties
        /// class.
        /// </summary>
        /// <param name="props"></param>
        /// <param name="mtu"></param>
        internal IPv6InterfaceProperties(IPInterfaceProperties props, int mtu)
        {
            m_props = props;
            m_mtu = mtu;
        }

        /// <summary>
        /// Gets the interface index for the Internet Protocol version 6 (IPv6) address.
        /// </summary>
        public int Index
        {
            get { return m_props.m_info.Index; }
        }

        /// <summary>
        /// Gets the maximum transmission unit (MTU) for this network interface.
        /// </summary>
        public int Mtu
        {
            get { return m_mtu; }
        }
    }
}
