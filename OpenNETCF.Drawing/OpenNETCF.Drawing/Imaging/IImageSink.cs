using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace OpenNETCF.Drawing.Imaging
{
  /// <summary>
  /// This interface allows an image source, such as an image decoder, and an image sink to exchange data. 
  /// The most important interaction between the source and the sink happens when they negotiate data transfer 
  /// parameters during the call to the IImageSink::BeginSink method
  /// </summary>
  [ComImport, Guid("327ABDAE-072B-11D3-9D7B-0000F81EF32E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [CLSCompliant(false)]
  public interface IImageSink
  {
    /// <summary>
    /// Begin the sink process
    /// </summary>
    /// <param name="imageInfo"></param>
    /// <param name="subarea"></param>
    /// <returns></returns>
    int BeginSink(
      out ImageInfo imageInfo,
      out RECT subarea
      );

    /// <summary>
    /// End the sink process
    /// </summary>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    int EndSink(
      int statusCode
      );

    /// <summary>
    /// Pass the color palette to the image sink
    /// </summary>
    /// <param name="palette"></param>
    /// <returns></returns>
    int SetPalette(
      ref ColorPalette palette
      );

    /// <summary>
    /// Ask the sink to allocate pixel data buffer
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="pixelFormat"></param>
    /// <param name="lastPass"></param>
    /// <param name="bitmapData"></param>
    /// <returns></returns>
    int GetPixelDataBuffer(
      RECT rect,
      PixelFormat pixelFormat,
      bool lastPass,
      out BitmapDataInternal bitmapData
      );

    /// <summary>
    /// Give the sink pixel data and release data buffer
    /// </summary>
    /// <param name="bitmapData"></param>
    /// <returns></returns>
    int ReleasePixelDataBuffer(
      ref BitmapDataInternal bitmapData
      );

    /// <summary>
    /// Push pixel data
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="bitmapData"></param>
    /// <param name="lastPass"></param>
    /// <returns></returns>
    int PushPixelData(
      RECT rect,
      ref BitmapDataInternal bitmapData,
      bool lastPass
      );

    /// <summary>
    /// Push raw image data
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="bufsize"></param>
    /// <returns></returns>
    int PushRawData(
      IntPtr buffer,
      uint bufsize
      );

    /// <summary>
    /// This method is used by an image source to determine whether an image 
    /// sink should rotate an image and by how much it should rotate the image
    /// </summary>
    /// <param name="rotation"></param>
    /// <returns></returns>
    int NeedTransform(
      out uint rotation
      );

    /// <summary>
    /// This method is used by an image source to determine whether an image sink can accept raw properties
    /// </summary>
    /// <returns></returns>
    int NeedRawProperty(
      );

    /// <summary>
    /// This method is used by an image source to send raw information about an image to an image sink
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    int PushRawInfo(
      byte[] info
      );

    /// <summary>
    /// Retrieves a property as a blob
    /// </summary>
    /// <param name="uiTotalBufferSize"></param>
    /// <param name="ppBuffer"></param>
    /// <returns></returns>
    int GetPropertyBuffer(
          int uiTotalBufferSize,
          IntPtr ppBuffer
      );

    int PushPropertyItems(
      uint numOfItems,
      uint uiTotalBufferSize,
      PropertyItem[] item
      );
  }
}
