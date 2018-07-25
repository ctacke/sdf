using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Provides information about a network interface address.
    /// </summary>
    public class IPAddressInformation
    {
        internal IPAddress m_address;
        internal PerAdapterFlags m_adapterFlags;

        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.IPAddressInformation
        /// class.
        /// </summary>
        protected internal IPAddressInformation() {}

        /// <summary>
        /// Gets the Internet Protocol (IP) address.
        /// </summary>
        public IPAddress Address
        {
            get { return m_address; }
            internal set { m_address = value; }
        }

        /// <summary>
        /// Gets a System.Boolean value that indicates whether the Internet Protocol
        /// (IP) address is legal to appear in a Domain Name System (DNS) server database.
        /// </summary>
        public bool IsDnsEligible
        {
            get { return (m_adapterFlags | PerAdapterFlags.DnsEligible) == PerAdapterFlags.DnsEligible; }
        }

        /// <summary>
        /// Gets a System.Boolean value that indicates whether the Internet Protocol
        /// (IP) address is transient (a cluster address).
        /// </summary>
        public bool IsTransient
        {
            get { return (m_adapterFlags | PerAdapterFlags.Transient) == PerAdapterFlags.Transient; }
        }

        internal PerAdapterFlags AdapterFlags
        {
            set { m_adapterFlags = value; }
        }

    }
}
