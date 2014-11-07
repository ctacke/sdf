using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Drawing.Imaging
{
  [StructLayout(LayoutKind.Sequential)]
  public sealed class EncoderParameter : IDisposable
  {
    [MarshalAs(UnmanagedType.Struct)]
    private Guid parameterGuid;
    private int numberOfValues;
    private int parameterValueType;
    private IntPtr parameterValue;
    public void Dispose() { }
    //public IEncoder Encoder { get; set; }
    /*
    public EncoderParameterValueType Type { get; }
    public EncoderParameterValueType ValueType { get; }
    public int NumberOfValues { get; }
    private void Dispose(bool disposing);
    */
    /*
    public EncoderParameter(Encoder encoder, byte value);
    public EncoderParameter(Encoder encoder, byte value, bool undefined);
    public EncoderParameter(Encoder encoder, short value);
    public EncoderParameter(Encoder encoder, long value);
    public EncoderParameter(Encoder encoder, int numerator, int demoninator);
    public EncoderParameter(Encoder encoder, long rangebegin, long rangeend);
    public EncoderParameter(Encoder encoder, int numerator1, int demoninator1, int numerator2, int demoninator2);
    public EncoderParameter(Encoder encoder, string value);
    public EncoderParameter(Encoder encoder, byte[] value);
    public EncoderParameter(Encoder encoder, byte[] value, bool undefined);
    public EncoderParameter(Encoder encoder, short[] value);
    public EncoderParameter(Encoder encoder, long[] value);
    public EncoderParameter(Encoder encoder, int[] numerator, int[] denominator);
    public EncoderParameter(Encoder encoder, long[] rangebegin, long[] rangeend);
    public EncoderParameter(Encoder encoder, int[] numerator1, int[] denominator1, int[] numerator2, int[] denominator2);
    public EncoderParameter(Encoder encoder, int NumberOfValues, int Type, int Value);
    private static IntPtr Add(IntPtr a, int b);
    private static IntPtr Add(int a, IntPtr b);
     * */
  }
}
