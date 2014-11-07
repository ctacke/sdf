using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Drawing.Imaging
{
  /// <summary>
  /// Property tag types
  /// </summary>
  public enum TAG_TYPE
  {
    /// <summary>
    /// 8-bit unsigned int
    /// </summary>
    BYTE = 1,
    /// <summary>
    /// 8-bit byte containing one 7-bit ASCII code.
    /// NULL terminated.
    /// </summary>
    ASCII = 2,
    /// <summary>
    /// 16-bit unsigned int
    /// </summary>
    SHORT = 3,
    /// <summary>
    /// 32-bit unsigned int
    /// </summary>
    LONG = 4,
    /// <summary>
    /// Two LONGs.  The first LONG is the numerator,
    /// the second LONG expresses the denominator.
    /// </summary>
    RATIONAL = 5,
    /// <summary>
    /// 8-bit byte that can take any value depending
    /// on field definition
    /// </summary>
    UNDEFINED = 7,
    /// <summary>
    /// 32-bit singed integer (2's complement notation)
    /// </summary>
    SLONG = 9,
    /// <summary>
    /// Two SLONGs. First is numerator, second is the denominator
    /// </summary>
    SRATIONAL = 10,
  }
}
