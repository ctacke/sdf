using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OpenNETCF
{
  public static partial class Extensions
  {
    /// <summary>
    /// Determines if the file is writable or not
    /// </summary>
    /// <param name="fi">A FileInfo instance</param>
    /// <returns><b>true</b> if the ReadObly attribute flag is cleared, otherwise <b>false</b></returns>
    public static bool IsWritable(this FileInfo fi)
    {
      return !((fi.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);
    }

    /// <summary>
    /// Clears a FileInfo's ReadOnly attribute flag
    /// </summary>
    /// <param name="fi">A FileInfo instance</param>
    public static void MakeWritable(this FileInfo fi)
    {
      fi.Attributes &= ~FileAttributes.ReadOnly;
    }
  }
}
