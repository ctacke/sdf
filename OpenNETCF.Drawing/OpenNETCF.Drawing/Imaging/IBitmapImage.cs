using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;

namespace OpenNETCF.Drawing.Imaging
{
  [ComImport, Guid("327ABDAA-072B-11D3-9D7B-0000F81EF32E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [CLSCompliant(false)]
  public interface IBitmapImage
  {
    /// <summary>
    /// Get bitmap dimensions pixels
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    int GetSize(
      out Size size
      );

    /// <summary>
    /// Get bitmap pixel format
    /// </summary>
    /// <param name="pixelFormat"></param>
    /// <returns></returns>
    int GetPixelFormatID(
      out PixelFormat pixelFormat
      );

    /// <summary>
    /// Access bitmap data the specified pixel format
    ///  must support at least PIXFMT_DONTCARE and
    ///  the canonical formats.
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="flags"></param>
    /// <param name="pixelFormat"></param>
    /// <param name="lockedBitmapData"></param>
    /// <returns></returns>
    int LockBits(
      RECT rect,
      uint flags,
      PixelFormat pixelFormat,
      ref BitmapDataInternal lockedBitmapData
      );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lockedBitmapData"></param>
    /// <returns></returns>
    int UnlockBits(
      ref BitmapDataInternal lockedBitmapData
      );

    /// <summary>
    /// Set/get palette associated with the bitmap image
    /// Do not use directly. Use <c>ImageUtils.GetBitmapPalette</c> instead
    /// </summary>
    /// <param name="palette"></param>
    /// <returns></returns>
    int GetPalette(
      /*out ColorPalette palette*/
          out IntPtr pPalette
      );

    /// <summary>
    /// Do not use directly. Use <c>ImageUtils.SetBitmapPalette</c> instead
    /// </summary>
    /// <param name="palette"></param>
    /// <returns></returns>
    int SetPalette(
      /*ref ColorPalette palette*/
          IntPtr pPalette
      );

  }
}
