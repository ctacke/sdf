using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace OpenNETCF.Drawing.Imaging
{
  /// <summary>
  /// This is the basic interface to an image object. It allows applications to do the following: 
  /// Display the image onto a destination graphics context  
  /// Push image data into an image sink 
  /// Access image properties and metadata Decoded image objects and in-memory bitmap image objects support the IImage interface
  /// </summary>
  [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("327ABDA9-072B-11D3-9D7B-0000F81EF32E")]
  [CLSCompliant(false)]
  public interface IImage
  {
    /// <summary>
    /// Get the device-independent physical dimension of the image
    ///  unit of 0.01mm
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    int GetPhysicalDimension(
      out Size size
      );

    /// <summary>
    /// Get basic image info
    /// </summary>
    /// <param name="imageInfo"></param>
    /// <returns></returns>
    int GetImageInfo(
      out ImageInfo imageInfo
      );

    /// <summary>
    /// Set image flags
    /// </summary>
    /// <param name="flags"></param>
    /// <returns></returns>
    int SetImageFlags(
      ImageFlags flags
      );

    /// <summary>
    /// Display the image a GDI device context
    /// </summary>
    /// <param name="hdc"></param>
    /// <param name="dstRect"></param>
    /// <param name="srcRect"></param>
    /// <returns></returns>
    int Draw(
      IntPtr hdc,
      RECT dstRect,
      RECT srcRect
      );

    /// <summary>
    /// Push image data into an IImageSink
    /// </summary>
    /// <param name="sink"></param>
    /// <returns></returns>
    int PushIntoSink(
      IImageSink sink
      );

    /// <summary>
    /// Get a thumbnail representation for the image object
    /// </summary>
    /// <param name="thumbWidth"></param>
    /// <param name="thumbHeight"></param>
    /// <param name="thumbImage"></param>
    /// <returns></returns>
    int GetThumbnail(
      uint thumbWidth,
      uint thumbHeight,
      out IImage thumbImage
      );
  }
}
