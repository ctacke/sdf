using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Runtime.InteropServices.ComTypes
{
    /// <summary>
    /// This structure is a 64-bit value representing the number of 100-nanosecond intervals since January 1, 1601.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FILETIME
    {
        /// <summary>
        /// Specifies the low 32 bits of the FILETIME.
        /// </summary>
        public int dwLowDateTime;
        /// <summary>
        /// Specifies the high 32 bits of the FILETIME.
        /// </summary>
        public int dwHighDateTime;
    }
}
