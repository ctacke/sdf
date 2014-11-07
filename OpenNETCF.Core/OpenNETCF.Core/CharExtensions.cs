using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF
{
  /// <summary>
  /// Provides Extension Methods for commonly used base classes
  /// </summary>
  public static partial class Extensions
  {
    /// <summary>
    /// Determines if a character is a digit
    /// </summary>
    /// <param name="c">character to test</param>
    /// <returns><b>true</b> if the character is a digit, otherwise <b>false</b></returns>
    public static bool IsDigit(this char c)
    {
      return char.IsDigit(c);
    }
  }
}
