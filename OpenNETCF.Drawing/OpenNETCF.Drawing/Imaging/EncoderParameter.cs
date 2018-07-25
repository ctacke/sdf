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
