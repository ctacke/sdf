#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



namespace OpenNETCF
{
  using System;

  public static partial class Extensions
  {
    /// <summary>
    /// Converts an int to IEEE standard binary-coded decimal (BCD) format
    /// </summary>
    /// <param name="value">integer value</param>
    /// <returns>BCD encoded value</returns>
    public static int ToBCD(this int value)
    {
      byte[] data = new byte[4];

      int tempval = value;
      data[3] = (byte)(value / 1000);
      tempval %= 1000;
      data[2] = (byte)(tempval / 100);
      tempval %= 100;
      data[1] = (byte)(tempval / 10);
      data[0] = (byte)(tempval % 10);

      return BitConverter.ToInt32(data, 0);
    }

    /// <summary>
    /// Converts an int to IEEE standard binary-coded decimal (BCD) format
    /// </summary>
    /// <param name="bcdValue">BCD encoded value</param>
    /// <returns>integer value</returns>
    public static int FromBCD(this int bcdValue)
    {
      byte[] data = BitConverter.GetBytes(bcdValue);

      int value = data[3] * 1000
        + data[2] * 100
        + data[1] * 10
        + data[0];

      return value;
    }

    public static ushort LoWord(this int src)
    {
      return (ushort)(src & ushort.MaxValue);
    }

    public static ushort HiWord(this int src)
    {
      return (ushort)(src >> 16);
    }

    public static ushort LoWord(this uint src)
    {
      return (ushort)(src & ushort.MaxValue);
    }

    public static ushort HiWord(this uint src)
    {
      return (ushort)(src >> 16);
    }

    public static ushort LoWord(this IntPtr src)
    {
      return src.ToInt32().LoWord();
    }

    public static ushort HiWord(this IntPtr src)
    {
      return src.ToInt32().HiWord();
    }
  }
}
