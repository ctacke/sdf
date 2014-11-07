using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace OpenNETCF.Drawing.Imaging
{
  /// <summary>
  /// 
  /// </summary>
  [ComImport, Guid("327ABDAF-072B-11D3-9D7B-0000F81EF32E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [CLSCompliant(false)]
  public interface IBasicBitmapOps
  {
    /// <summary>
    /// Clone an area of the bitmap image
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="outbmp"></param>
    /// <param name="bNeedCloneProperty"></param>
    /// <returns></returns>
    int Clone(
        RECT rect,
        out IBitmapImage outbmp,
        bool bNeedCloneProperty
        );

    /// <summary>
    /// Flip the bitmap image in x- and/or y-direction
    /// </summary>
    /// <param name="flipX"></param>
    /// <param name="flipY"></param>
    /// <param name="outbmp"></param>
    /// <returns></returns>
    int Flip(
        bool flipX,
        bool flipY,
        out IBitmapImage outbmp
        );


    /// <summary>
    /// Resize the bitmap image
    /// </summary>
    /// <param name="newWidth"></param>
    /// <param name="newHeight"></param>
    /// <param name="pixelFormat"></param>
    /// <param name="hints"></param>
    /// <param name="outbmp"></param>
    /// <returns></returns>
    int Resize(
        uint newWidth,
        uint newHeight,
        PixelFormat pixelFormat,
        InterpolationHint hints,
        out IBitmapImage outbmp
        );

    /// <summary>
    /// Rotate the bitmap image by the specified angle
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="hints"></param>
    /// <param name="outbmp"></param>
    /// <returns></returns>
    int Rotate(
        float angle,
        InterpolationHint hints,
        out IBitmapImage outbmp
        );

    /// <summary>
    /// Adjust the brightness of the bitmap image
    /// </summary>
    /// <param name="percent"></param>
    /// <returns></returns>
    int AdjustBrightness(
        float percent
        );


    /// <summary>
    /// This method adjusts the contrast of an image by rescaling the pixel values in the range from 0 to 1 to the new range from shadow to highlight.
    /// The old pixel value 0 is mapped to the new pixel value of shadow and the old pixel value 1 is mapped to the new pixel value of highlight. All other old pixel values are mapped into the new range through linear interpolation.
    /// </summary>
    /// <param name="shadow">A FLOAT value that defines the value for pixels that are currently 0.</param>
    /// <param name="highlight">A FLOAT value that defines the value for pixels that are currently 1.</param>
    /// <returns></returns>
    int AdjustContrast(
        float shadow,
        float highlight
        );


    /// <summary>
    /// Adjust the gamma of the bitmap image
    /// </summary>
    /// <param name="gamma"></param>
    /// <returns></returns>
    int AdjustGamma(
        float gamma
        );
  }
}
