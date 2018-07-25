using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Define the general network infrastructure mode in
    /// which the selected network is presently operating.
    /// </summary>
    public enum InfrastructureMode
    {
        /// <summary>
        /// Specifies the independent basic service set (IBSS) mode. This mode is also known as ad hoc mode
        /// </summary>
        AdHoc,
        /// <summary>
        /// Specifies the infrastructure mode.
        /// </summary>
        Infrastructure,
        /// <summary>
        /// The infrastructure mode is either set to automatic or cannot be determined.
        /// </summary>
        AutoUnknown
    }
}
