using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Specifies the current state of an IP address.
    /// </summary>
    public enum DuplicateAddressDetectionState
    {
        /// <summary>
        /// The address is not valid. A nonvalid address is expired and no longer assigned
        /// to an interface; applications should not send data packets to it.
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// The duplicate address detection procedure's evaluation of the address has
        /// not completed successfully. Applications should not use the address because
        /// it is not yet valid and packets sent to it are discarded.
        /// </summary>
        Tentative = 1,
        /// <summary>
        /// The address is not unique. This address should not be assigned to the network
        /// interface.
        /// </summary>
        Duplicate = 2,
        /// <summary>
        /// The address is valid, but it is nearing its lease lifetime and should not
        /// be used by applications.
        /// </summary>
        Deprecated = 3,
        /// <summary>
        /// The address is valid and its use is unrestricted.
        /// </summary>
        Preferred = 4,
    }
}
