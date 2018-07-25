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
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;

namespace OpenNETCF.Drawing.Imaging
{
  /// <summary>
  /// High-level image manipulation routines
  /// </summary>
  public class ImageUtils
  {
    /// <summary>
    /// Rotate and/or flip bitmap
    /// </summary>
    /// <param name="bitmap">Image</param>
    /// <param name="type">Operation</param>
    /// <returns>Modified bitmap</returns>
    static public Bitmap RotateFlip(Bitmap bitmap, RotationAngle angle, FlipAxis axis)
    {
      if (bitmap == null) throw new ArgumentNullException();

      IBitmapImage image = BitmapToIImageBitmap(bitmap);
      try
      {
        IBitmapImage imageBitmap = BitmapToIImageBitmap(bitmap);
        IBasicBitmapOps ops = (IBasicBitmapOps)imageBitmap;
        if (angle != 0)
        {
          ops.Rotate((float)angle, InterpolationHint.InterpolationHintDefault, out imageBitmap);
          Marshal.FinalReleaseComObject(ops);
          ops = (IBasicBitmapOps)imageBitmap;
        }
        if (axis != FlipAxis.None)
        {
          ops.Flip((axis & FlipAxis.X) == FlipAxis.X, (axis & FlipAxis.Y) == FlipAxis.Y, out imageBitmap);
          Marshal.FinalReleaseComObject(ops);
          ops = (IBasicBitmapOps)imageBitmap;
        }
        return IBitmapImageToBitmap(imageBitmap);
      }
      finally
      {
        Marshal.FinalReleaseComObject(image);
      }
    }

    /// <summary>
    /// Rotate and/or flip bitmap
    /// </summary>
    /// <param name="bitmap">Image</param>
    /// <param name="type">Operation</param>
    /// <returns>Modified bitmap</returns>
    [Obsolete("Deprecated.  Using the overload that takes a RotationAngle parameter.", false)]
    // Deprecated in v 2.3 with no error
    static public Bitmap RotateFlip(Bitmap bitmap, RotateFlipType type)
    {
      if (bitmap == null) throw new ArgumentNullException();

      RotationAngle angle = RotationAngle.Zero;
      FlipAxis axis = FlipAxis.None;

      switch (type)
      {
        case RotateFlipType.RotateNoneFlipX:
          axis = FlipAxis.X;
          break;
        case RotateFlipType.Rotate90FlipX:
          axis = FlipAxis.X;
          angle = RotationAngle.Clockwise90;
          break;
        case RotateFlipType.Rotate270FlipX:
          angle = RotationAngle.Clockwise270;
          axis = FlipAxis.X;
          break;
        case RotateFlipType.RotateNoneFlipXY:
          axis = FlipAxis.X | FlipAxis.Y;
          break;
        case RotateFlipType.Rotate90FlipXY:
          angle = RotationAngle.Clockwise90;
          axis = FlipAxis.X | FlipAxis.Y;
          break;
        case RotateFlipType.Rotate180FlipXY:
          angle = RotationAngle.Clockwise180;
          axis = FlipAxis.X | FlipAxis.Y;
          break;
        case RotateFlipType.Rotate270FlipXY:
          angle = RotationAngle.Clockwise270;
          axis = FlipAxis.X | FlipAxis.Y;
          break;
        case RotateFlipType.RotateNoneFlipY:
          axis = FlipAxis.Y;
          break;
      }

      return RotateFlip(bitmap, angle, axis);
    }

    /// <summary>
    /// Rotates image by specified amount of degrees
    /// </summary>
    /// <param name="bitmap">Image</param>
    /// <param name="angle">Amount of degrees to rotate image by. Must be 90, 180, or 270</param>
    /// <returns>Rotated image</returns>
    static public Bitmap Rotate(Bitmap bitmap, RotationAngle angle)
    {
      if (bitmap == null) throw new ArgumentNullException();

      IBitmapImage imageBitmap = BitmapToIImageBitmap(bitmap);
      try
      {
        return Rotate(imageBitmap, (float)angle);
      }
      finally
      {
        Marshal.FinalReleaseComObject(imageBitmap);
      }
    }

    private static Bitmap Rotate(IBitmapImage image, float angle)
    {
      IBitmapImage imageBitmap;
      Bitmap bmRet;
      IBasicBitmapOps ops = (IBasicBitmapOps)image;
      ops.Rotate(angle, InterpolationHint.InterpolationHintDefault, out imageBitmap);
      try
      {
        bmRet = IBitmapImageToBitmap(imageBitmap);
      }
      finally
      {
        Marshal.FinalReleaseComObject(imageBitmap);
      }
      return bmRet;
    }

    /// <summary>
    /// Rotates image by specified amount of degrees
    /// </summary>
    /// <param name="bitmap">Image</param>
    /// <param name="angle">Amount of degrees to rotate image by. Must be 90, 180, or 270</param>
    /// <returns>Rotated image</returns>
    [Obsolete("Deprecated.  Using the overload that takes a RotationAngle parameter.", false)]
    // Deprecated in v 2.3 with no error
    static public Bitmap Rotate(Bitmap bitmap, float angle)
    {
      if (bitmap == null) throw new ArgumentNullException();

      if (angle < 0)
        angle += 360;
      if (angle != 0 && angle != 90 && angle != 180 && angle != 270)
        throw new ArgumentException("Only -90, 0, 90, 180 or 270 degrees rotation is supported", "angle");
      IBitmapImage imageBitmap = BitmapToIImageBitmap(bitmap);
      try
      {
        return Rotate(imageBitmap, angle);
      }
      finally
      {
        Marshal.FinalReleaseComObject(imageBitmap);
      }
    }

    /// <summary>
    /// Flips image around X and/or Y axes
    /// </summary>
    /// <param name="bitmap">Image</param>
    /// <param name="axis">Axis or axes to flip on</param>
    static public Bitmap Flip(Bitmap bitmap, FlipAxis axis)
    {
      if (bitmap == null) throw new ArgumentNullException();

      IBitmapImage image = BitmapToIImageBitmap(bitmap);
      try
      {
        return Flip(image, axis);
      }
      finally
      {
        Marshal.FinalReleaseComObject(image);
      }
    }

    /// <summary>
    /// Flips image around X and/or Y axes
    /// </summary>
    /// <param name="bitmap">Image</param>
    /// <param name="axis">Axis or axes to flip on</param>
    private static Bitmap Flip(IBitmapImage image, FlipAxis axis)
    {
      Bitmap bmRet;
      IBitmapImage imageBitmap;
      IBasicBitmapOps ops = (IBasicBitmapOps)image;
      ops.Flip((axis & FlipAxis.X) == FlipAxis.X, (axis & FlipAxis.Y) == FlipAxis.Y, out imageBitmap);
      try
      {
        bmRet = IBitmapImageToBitmap(imageBitmap);
      }
      finally
      {
        Marshal.FinalReleaseComObject(imageBitmap);
      }
      return bmRet;
    }

      /// <summary>
    /// Flips image around X and/or Y axes
    /// </summary>
    /// <param name="bitmap">Image</param>
    /// <param name="flipX">Whether to flip around X axis</param>
    /// <param name="flipY">Whether to flip around Y axis</param>
    /// <returns>Flipped image</returns>
    [Obsolete("Deprecated.  Using the overload that takes a FlipAxis parameter.", false)]
    // Deprecated in v 2.3 with no error
    static public Bitmap Flip(Bitmap bitmap, bool flipX, bool flipY)
    {
      if (bitmap == null) throw new ArgumentNullException();

      FlipAxis axis = FlipAxis.None;
      if (flipX) axis |= FlipAxis.X;
      if (flipY) axis |= FlipAxis.Y;

      return Flip(bitmap, axis);
    }

    /// <summary>
    /// Converts Imaging API IBitmapImage object to .NET Bitmap object
    /// </summary>
    /// <param name="imageBitmap">Source IImageBitmap object</param>
    /// <returns>Bitmap object</returns>
    [CLSCompliant(false)]
    static public Bitmap IBitmapImageToBitmap(IBitmapImage imageBitmap)
    {
      if (imageBitmap == null) throw new ArgumentNullException();

      Size szRotated;
      imageBitmap.GetSize(out szRotated);
      RECT rcLock = new RECT(0, 0, szRotated.Width, szRotated.Height);
      BitmapDataInternal bdi = new BitmapDataInternal();
      imageBitmap.LockBits(rcLock, 0, PixelFormat.Format24bppRgb, ref bdi);
      Bitmap bitmap = new Bitmap(bdi.Width, bdi.Height, bdi.PixelFormat);
      System.Drawing.Imaging.BitmapData bd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bdi.PixelFormat);
      CopyMemory(bd.Scan0, bdi.Scan0, bdi.Height * bdi.Stride);
      imageBitmap.UnlockBits(ref bdi);
      bitmap.UnlockBits(bd);
      return bitmap;
    }

    /// <summary>
    /// Creates a thumbnail of a specified size out of a (compressed image)
    /// </summary>
    /// <param name="stream">Stream containing the image</param>
    /// <param name="size">requested thumbnail size</param>
    /// <returns>Thumbnail image</returns>
    [CLSCompliant(false)]
    static public IBitmapImage CreateThumbnail(Stream stream, Size size)
    {
      if (stream == null) throw new ArgumentNullException();

      ImagingFactory factory = new ImagingFactoryClass();
      IImage image, imageThumb;
      factory.CreateImageFromStream(new StreamOnFile(stream), out image);
      image.GetThumbnail((uint)size.Width, (uint)size.Height, out imageThumb);
      IBitmapImage imageBitmap = null;
      ImageInfo ii;
      image.GetImageInfo(out ii);
      factory.CreateBitmapFromImage(image, (uint)size.Width, (uint)size.Height, ii.PixelFormat, InterpolationHint.InterpolationHintDefault, out imageBitmap);
      return imageBitmap;
    }

    /// <summary>
    /// Converts .NET Bitmap object to Imaging API IBitmapImage object
    /// </summary>
    /// <param name="bitmap">Source Bitmap object</param>
    /// <returns>IImageBitmap object</returns>
    [CLSCompliant(false)]
    static public IBitmapImage BitmapToIImageBitmap(Bitmap bitmap)
    {
      if (bitmap == null) throw new ArgumentNullException();

      ImagingFactory factory = new ImagingFactoryClass();
      System.Drawing.Imaging.BitmapData bd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
      IBitmapImage imageBitmap = null;
      factory.CreateNewBitmap((uint)bd.Width, (uint)bd.Height, bd.PixelFormat, out imageBitmap);
      BitmapDataInternal bdi = new BitmapDataInternal();
      RECT rc = new RECT(0, 0, bd.Width, bd.Height);
      imageBitmap.LockBits(rc, (int)ImageLockMode.WriteOnly, bd.PixelFormat, ref bdi);
      int cb = bdi.Stride * bdi.Height;
      CopyMemory(bdi.Scan0, bd.Scan0, cb);
      bitmap.UnlockBits(bd);
      imageBitmap.UnlockBits(ref bdi);
      return imageBitmap;
    }

    static internal void CopyMemory(IntPtr pDst, IntPtr pSrc, int cbSize)
    {
      if ((pDst == IntPtr.Zero)
        || (pSrc == IntPtr.Zero)
        || (cbSize <= 0)
        || (pDst.ToInt32() == -1)
        || (pSrc.ToInt32() == -1))
      {
        throw new ArgumentException();
      }

      const int cb = 65536;
      byte[] buffer = new byte[cb];
      while (cbSize > cb)
      {
        Marshal.Copy(pSrc, buffer, 0, cb);
        Marshal.Copy(buffer, 0, pDst, cb);
        cbSize -= cb;
        pDst = (IntPtr)((int)pDst + cb);
        pSrc = (IntPtr)((int)pSrc + cb);
      }
      Marshal.Copy(pSrc, buffer, 0, cbSize);
      Marshal.Copy(buffer, 0, pDst, cbSize);
    }

    /// <summary>
    /// Loads specified image property from the given image
    /// </summary>
    /// <param name="imagePath">path to the image file</param>
    /// <param name="property">Property tag</param>
    /// <returns>Property</returns>
    public static ImageProperty LoadProperty(string imagePath, ImageTag property)
    {
      if (imagePath == null) throw new ArgumentNullException();

      StreamOnFile st = new StreamOnFile(imagePath);
      IImageDecoder decoder = null;
      ImagingFactory factory = new ImagingFactoryClass();
      factory.CreateImageDecoder(st, DecoderInitFlag.DecoderInitFlagNone, out decoder);
      return LoadProperty(decoder, property);
    }

    /// <summary>
    /// Loads specified image property from an ImageDecoder object
    /// </summary>
    /// <param name="decoder"><c>IImageDecoder</c> object</param>
    /// <param name="property">Property tag</param>
    /// <returns>Property</returns>
    [CLSCompliant(false)]
    public static ImageProperty LoadProperty(IImageDecoder decoder, ImageTag property)
    {
      if (decoder == null) throw new ArgumentNullException();
      PropertyItem item = new PropertyItem();
      uint cbProp;
      decoder.GetPropertyItemSize(property, out cbProp);
      IntPtr pProp = Marshal.AllocHGlobal((int)cbProp);
      try
      {
        if (decoder.GetPropertyItem(property, cbProp, pProp) != 0)
          return null;
        item = (PropertyItem)Marshal.PtrToStructure(pProp, typeof(PropertyItem));
        return LoadProperty(item);
      }
      finally
      {
        Marshal.FreeHGlobal(pProp);
      }
    }

    /// <summary>
    /// Loads managed <c>ImageProperty</c> object from an unmanaged <c>PropertyItem</c>
    /// </summary>
    /// <param name="item"><c>PropertyItem</c></param>
    /// <returns><c>ImageProperty</c></returns>
    public static ImageProperty LoadProperty(PropertyItem item)
    {
      ImageProperty prop = new ImageProperty();
      prop.Id = item.Id;
      prop.Len = item.Len;
      prop.Type = item.Type;
      if (prop.Len > 0)
      {
        byte[] data = new byte[item.Len];
        Marshal.Copy(item.Value, data, 0, item.Len);
        prop.FromByteArray(data);
      }
      return prop;
    }

    /// <summary>
    /// Loads all image tags for the image
    /// </summary>
    /// <param name="decoder"><c>IImageDecoder</c> object</param>
    /// <returns>Array of all tags</returns>
    [CLSCompliant(false)]
    public static ImageTag[] GetAllTags(IImageDecoder decoder)
    {
      if (decoder == null) throw new ArgumentNullException();

      uint cProps;
      decoder.GetPropertyCount(out cProps);
      ImageTag[] propTags = new ImageTag[cProps];
      decoder.GetPropertyIdList(cProps, propTags);
      return propTags;
    }

    /// <summary>
    /// Loads all image properties for the image
    /// </summary>
    /// <param name="decoder"><c>IImageDecoder</c> object</param>
    /// <returns>Array of all properties</returns>
    [CLSCompliant(false)]
    public static ImageProperty[] GetAllProperties(IImageDecoder decoder)
    {
      if (decoder == null) throw new ArgumentNullException();

      ImageTag[] propTags = GetAllTags(decoder);
      List<ImageProperty> allProps = new List<ImageProperty>(propTags.Length);
      uint cbAllProps; uint numProps;
      decoder.GetPropertySize(out cbAllProps, out numProps);
      IntPtr pProps = Marshal.AllocHGlobal((int)cbAllProps);
      PropertyItem[] allItems = new PropertyItem[numProps];
      decoder.GetAllPropertyItems(cbAllProps, numProps, pProps);
      IntPtr pProp = pProps;
      for (uint i = 0; i < numProps; i++)
      {
        allItems[i] = (PropertyItem)Marshal.PtrToStructure(pProp, typeof(PropertyItem));
        pProp = (IntPtr)(pProp.ToInt32() + Marshal.SizeOf(typeof(PropertyItem)));
      }
      foreach (PropertyItem pi in allItems)
        allProps.Add(LoadProperty(pi));
      Marshal.FreeHGlobal(pProps);
      return allProps.ToArray();
    }

    /// <summary>
    /// Retrieves bitmap palette, or null on error
    /// </summary>
    /// <param name="bitmap">Bitmap whose palette to retireve</param>
    /// <returns>Palette</returns>
    [CLSCompliant(false)]
    public static ColorPalette GetBitmapPalette(IBitmapImage bitmap)
    {
      if (bitmap == null) throw new ArgumentNullException();

      IntPtr pPalette;
      if (bitmap.GetPalette(out pPalette) != 0)
        return null;
      ColorPalette palette = new ColorPalette();
      palette.ConvertFromMemory(pPalette);
      Marshal.FreeCoTaskMem(pPalette);
      return palette;
    }

    /// <summary>
    /// Modifies bitmap palette
    /// </summary>
    /// <param name="bitmap">Bitmap</param>
    /// <param name="palette">New palette</param>
    /// <returns>Success indicator</returns>
    [CLSCompliant(false)]
    public static bool SetBitmapPalette(IBitmapImage bitmap, ColorPalette palette)
    {
      if (bitmap == null) throw new ArgumentNullException();
      return bitmap.SetPalette(palette.ConvertToMemory()) == 0;
    }

    private delegate int GetCodecDelegate(out uint count, out IntPtr ptr);

    /// <summary>
    /// Gets all of the installed imaging encoders on the device
    /// </summary>
    /// <returns></returns>
    public static ImageCodecInfo[] GetInstalledEncoders()
    {
      ImagingFactory factory = new ImagingFactoryClass();
      return GetInstalledCodecs(factory.GetInstalledEncoders);
    }

    /// <summary>
    /// Gets all of the installed imaging decoders on the device
    /// </summary>
    /// <returns></returns>
    public static ImageCodecInfo[] GetInstalledDecoders()
    {
      ImagingFactory factory = new ImagingFactoryClass();
      return GetInstalledCodecs(factory.GetInstalledDecoders);
    }

    private static ImageCodecInfo[] GetInstalledCodecs(GetCodecDelegate getMethod)
    {
      ImagingFactory factory = new ImagingFactoryClass();

      uint count;
      IntPtr pCodec;
      //int hResult = factory.GetInstalledEncoders(out count, out pCodec);
      int hResult = getMethod(out count, out pCodec);

      if (hResult != 0) throw new System.ComponentModel.Win32Exception(hResult);

      ImageCodecInfo info = new ImageCodecInfo();
      List<ImageCodecInfo> list = new List<ImageCodecInfo>();

      for (int i = 0; i < count; i++)
      {
        IntPtr p = new IntPtr(pCodec.ToInt32() + (i * Marshal.SizeOf(info)));
        info = (ImageCodecInfo)Marshal.PtrToStructure(p, typeof(ImageCodecInfo));
        list.Add(info);
      }
      return list.ToArray();
    }
  }
}
