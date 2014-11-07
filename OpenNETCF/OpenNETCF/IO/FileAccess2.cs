using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.IO
{
    /// <summary>
    /// CreateFile file access flags
    /// </summary>
    [Flags()]//, CLSCompliant(false)]
    internal enum FileAccess2 : uint
    {
        /// <summary>
        /// Read access to the file.  Data can be read from the file.
        /// </summary>
        Read = 0x80000000,
        /// <summary>
        /// Write access to the file.  Data can be written to the file.
        /// </summary>
        Write = 0x40000000,
        /// <summary>
        /// Execute permission. The file can be executed.
        /// </summary>
        Execute = 0x20000000,
        /// <summary>
        /// All permissions.
        /// </summary>
        All = 0xF0000000
    }
}
