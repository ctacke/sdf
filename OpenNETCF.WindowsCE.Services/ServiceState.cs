using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.WindowsCE.Services
{
    public enum ServiceState
    {
        /// <summary>
        /// The service is turned off. 
        /// </summary>
        Stopped = 0,
        /// <summary>
        /// The service is turned on.
        /// </summary>
        Running = 1,
        /// <summary>
        /// The service is in the process of starting up. 
        /// </summary>
        Starting = 2,
        /// <summary>
        /// The service is in the process of shutting down. 
        /// </summary>
        Stopping = 3,
        /// <summary>
        /// The service is in the process of unloading. 
        /// </summary>
        Unloading = 4,
        /// <summary>
        /// The service is not initialized. 
        /// </summary>
        NotInitialized = 5,
        /// <summary>
        /// The state of the service is unknown. 
        /// </summary>
        Unknown = -1
    }
}
