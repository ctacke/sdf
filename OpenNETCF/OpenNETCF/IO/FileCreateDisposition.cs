using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.IO
{
    /// <summary>
    /// Specifies which action to take on files that exist, and which action to take when files do not exist.
    /// </summary>
    public enum FileCreateDisposition : int
    {
        /// <summary>
        /// Creates a new file.
        /// The function fails if the specified file already exists.
        /// </summary>
        CreateNew = 1,
        /// <summary>
        /// Creates a new file.
        /// If the file exists, the function overwrites the file and clears the existing attributes.
        /// </summary>
        CreateAlways = 2,
        /// <summary>
        /// Opens the file.
        /// The function fails if the file does not exist.
        /// </summary>
        OpenExisting = 3,
        /// <summary>
        /// Opens the file, if it exists.
        /// If the file does not exist, the function creates the file as if dwCreationDisposition were <b>CreateNew</b>.
        /// </summary>
        OpenAlways = 4,
        /// <summary>
        /// Opens the file.
        /// Once opened, the file is truncated so that its size is zero bytes. The calling process must open the file with at least Write access.
        /// </summary>
        TruncateExisting = 5,
        // <summary>
        // Not supported.
        // </summary>
        //OpenForLoader = 6,
    }
}
