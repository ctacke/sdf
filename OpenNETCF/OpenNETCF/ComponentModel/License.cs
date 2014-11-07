using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.ComponentModel
{
    /// <summary>
    /// Provides the abstract base class for all licenses. A license is granted to a specific instance of a component.
    /// </summary>
    public abstract class License
    {
        /// <summary>
        /// Initializes a new instance of the License class.
        /// </summary>
        protected License()
        {
        }

        public abstract string LicenseKey { get; }

        public abstract void Dispose();

        public abstract bool IsValid { get; }

    }
}
