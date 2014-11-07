using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.IO
{
    /// <summary>
    /// CreateFile file flags
    /// </summary>
    [CLSCompliant(false)]
    public enum FileFlags : uint
    {
        /// <summary>
        /// Instructs the system to write through any intermediate cache and go directly to disk.
        /// The system can still cache write operations, but cannot lazily flush them.
        /// </summary>
        WriteThrough = 0x80000000,
        /// <summary>
        /// This flag is not supported; however, multiple read/write operations pending on a device at a time are allowed.
        /// </summary>
        Overlapped = 0x40000000,
        /// <summary>
        /// Indicates that the file is accessed randomly.
        /// The system can use this as a hint to optimize file caching.
        /// </summary>
        RandomAccess = 0x10000000,
        /// <summary>
        /// 
        /// </summary>
        SequentialScan = 0x08000000,
        /// <summary>
        /// 
        /// </summary>
        DeleteOnClose = 0x04000000
    }
}
