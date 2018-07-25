using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Specifies how an IP address host suffix was located.
    /// </summary>
    public enum SuffixOrigin
    {
        /// <summary>
        /// The suffix was located using an unspecified source.
        /// </summary>
        Other = 0,
        /// <summary>
        /// The suffix was manually configured.
        /// </summary>
        Manual = 1,
        /// <summary>
        /// The suffix is a well-known suffix. Well-known suffixes are specified in standard-track
        /// Request for Comments (RFC) documents and assigned by the Internet Assigned
        /// Numbers Authority (IANA) or an address registry. Such suffixes are reserved
        /// for special purposes.
        /// </summary>
        WellKnown = 2,
        /// <summary>
        /// The suffix was supplied by a Dynamic Host Configuration Protocol (DHCP) server.
        /// </summary>
        OriginDhcp = 3,
        /// <summary>
        /// The suffix is a link-local suffix.
        /// </summary>
        LinkLayerAddress = 4,
        /// <summary>
        /// The suffix was randomly assigned.
        /// </summary>
        Random = 5,
    }
}
