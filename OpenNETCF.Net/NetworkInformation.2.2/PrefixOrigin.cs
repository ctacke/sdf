using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Specifies how an IP address network prefix was located.
    /// </summary>
    public enum PrefixOrigin
    {
        /// <summary>
        /// The prefix was located using an unspecified source.
        /// </summary>
        Other = 0,
        /// <summary>
        /// The prefix was manually configured.
        /// </summary>
        Manual = 1,
        /// <summary>
        /// The prefix is a well-known prefix. Well-known prefixes are specified in standard-track
        /// Request for Comments (RFC) documents and assigned by the Internet Assigned
        /// Numbers Authority (IANA) or an address registry. Such prefixes are reserved
        /// for special purposes.
        /// </summary>
        WellKnown = 2,
        /// <summary>
        /// The prefix was supplied by a Dynamic Host Configuration Protocol (DHCP) server.
        /// </summary>
        Dhcp = 3,
        /// <summary>
        /// The prefix was supplied by a router advertisement.
        /// </summary>
        RouterAdvertisement = 4,
    }
}
