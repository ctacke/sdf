using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Specifies a route type as defined in RFC 1354.
    /// </summary>
    public enum RouteType
    {
        /// <summary>
        /// The next hop is not the final destination (remote route).
        /// </summary>
        Intermediate = 4,
        /// <summary>
        /// The next hop is the final destination (local route). 
        /// </summary>
        Final = 3,
        /// <summary>
        /// The route is invalid. 
        /// </summary>
        Invalid = 2,
        /// <summary>
        /// Other
        /// </summary>
        Other = 1
    }
}
