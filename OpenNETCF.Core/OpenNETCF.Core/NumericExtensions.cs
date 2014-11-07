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
