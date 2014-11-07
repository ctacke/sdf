using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Specifies the type of an ARP entry
    /// </summary>
    public enum ArpEntryType
    {
        /// <summary>
        /// A static entry
        /// </summary>
        Static = 4,
        /// <summary>
        /// A dynamic entry
        /// </summary>
        Dynamic = 3,
        /// <summary>
        /// An invalid entry
        /// </summary>
        Invalid = 2,
        /// <summary>
        /// Entry is non-determinable
        /// </summary>
        Other = 1
    }
}
