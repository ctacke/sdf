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
using OpenNETCF.Runtime.InteropServices.ComTypes;
using System.Drawing.Imaging;

namespace OpenNETCF.Drawing.Imaging
{
  [Guid("327ABDA7-072B-11D3-9D7B-0000F81EF32E")]
  [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [CLSCompliant(false)]
  public interface IImagingFactory
  {
    /// <summary>
    /// Create an image object from an input stream
    ///  stream doesn't have to seekable
    ///  caller should Release the stream if call is successful
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    int CreateImageFromStream(
      IStream stream,
      out IImage image
      );

    /// <summary>
    /// Create an image object from a file
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    int CreateImageFromFile(
      string filename,
      out IImage image
      );

    /// <summary>
    /// Create an image object from a memory buffer
    /// </summary>
    /// <param name="buf"></param>
    /// <param name="size"></param>
    /// <param name="disposalFlag"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    int CreateImageFromBuffer(
      byte[] buf,
      uint size,
      BufferDisposalFlag disposalFlag,
      out IImage image
      );

    /// <summary>
    /// Create a new bitmap image object
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="pixelFormat"></param>
    /// <param name="bitmap"></param>
    /// <returns></returns>
    int CreateNewBitmap(
      uint width,
      uint height,
      PixelFormat pixelFormat,
      out IBitmapImage bitmap
      );

    /// <summary>
    /// Create a bitmap image from an IImage object
    /// </summary>
    /// <param name="image"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="pixelFormat"></param>
    /// <param name="hints"></param>
    /// <param name="bitmap"></param>
    /// <returns></returns>
    int CreateBitmapFromImage(
      IImage image,
      uint width,
      uint height,
      PixelFormat pixelFormat,
      InterpolationHint hints,
      out IBitmapImage bitmap
      );

    /// <summary>
    /// Create a new bitmap image object on user-supplied memory buffer
    /// </summary>
    /// <param name="bitmapData"></param>
    /// <param name="bitmap"></param>
    /// <returns></returns>
    int CreateBitmapFromBuffer(
      ref BitmapDataInternal bitmapData,
      out IBitmapImage bitmap
      );

    /// <summary>
    /// Create an image decoder object to process the given input stream
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="flags"></param>
    /// <param name="decoder"></param>
    /// <returns></returns>
    int CreateImageDecoder(
      IStream stream,
      DecoderInitFlag flags,
      out IImageDecoder decoder
      );

    /// <summary>
    /// Create an image encoder object that can output data the
    /// specified image file format.
    /// </summary>
    /// <param name="clsid"></param>
    /// <param name="stream"></param>
    /// <param name="encoder"></param>
    /// <returns></returns>
    int CreateImageEncoderToStream(
      ref Guid clsid,
      IStream stream,
      out IImageEncoder encoder
      );

    int CreateImageEncoderToFile(
      ref Guid clsid,
      string filename,
      out IImageEncoder encoder
      );

    /// <summary>
    /// Get a list of all currently installed image decoders
    /// </summary>
    /// <param name="count"></param>
    /// <param name="decoders"></param>
    /// <returns></returns>
    int GetInstalledDecoders(
      out uint count,
      out IntPtr /*out ImageCodecInfo*/ decoders
      );

    /// <summary>
    /// Get a list of all currently installed image decoders
    /// </summary>
    /// <param name="count"></param>
    /// <param name="encoders"></param>
    /// <returns></returns>
    int GetInstalledEncoders(
      out uint count,
      //[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)]
      //out ImageCodecInfo[] encoders
      out IntPtr /*out ImageCodecInfo[]*/ encoders
      );

    /// <summary>
    /// Install an image encoder / decoder
    ///  caller should do the regular COM component
    ///  installation before calling this method
    /// </summary>
    /// <param name="codecInfo"></param>
    /// <returns></returns>
    int InstallImageCodec(
      ref ImageCodecInfo codecInfo
      );

    /// <summary>
    /// Uninstall an image encoder / decoder
    /// </summary>
    /// <param name="codecName"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    int UninstallImageCodec(
      string codecName,
      uint flags
      );

  }
}
