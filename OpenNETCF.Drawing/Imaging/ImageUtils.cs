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
        static public Bitmap RotateFlip(Bitmap bitmap, RotateFlipType type)
        {
            bool flipX = false, flipY = false;
            float angle = 0;
            switch (type)
            {
                case RotateFlipType.RotateNoneFlipX:
                    flipX = true;
                    break;
                case RotateFlipType.Rotate90FlipX:
                    angle = 90;
                    flipX = true;
                    break;
                case RotateFlipType.Rotate270FlipX:
                    angle = 270;
                    flipX = true;
                    break;
                case RotateFlipType.RotateNoneFlipXY:
                    angle = 0;
                    flipX = true;
                    flipY = true;
                    break;
                case RotateFlipType.Rotate90FlipXY:
                    angle = 90;
                    flipX = true;
                    flipY = true;
                    break;
                case RotateFlipType.Rotate180FlipXY:
                    angle = 180;
                    flipX = true;
                    flipY = true;
                    break;
                case RotateFlipType.Rotate270FlipXY:
                    angle = 270;
                    flipX = true;
                    flipY = true;
                    break;
                case RotateFlipType.RotateNoneFlipY:
                    angle = 0;
                    flipY = true;
                    break;
            }

            IBitmapImage imageBitmap = BitmapToIImageBitmap(bitmap);
            IBasicBitmapOps ops = (IBasicBitmapOps)imageBitmap;
            if (angle != 0)
            {
                ops.Rotate(angle, InterpolationHint.InterpolationHintDefault, out imageBitmap);
                Marshal.FinalReleaseComObject(ops);
                ops = (IBasicBitmapOps)imageBitmap;
            }
            if (flipX || flipY)
            {
                ops.Flip(flipX, flipY, out imageBitmap);
                Marshal.FinalReleaseComObject(ops);
                ops = (IBasicBitmapOps)imageBitmap;
            }
            Bitmap bmRet = IBitmapImageToBitmap(imageBitmap);
            Marshal.FinalReleaseComObject(imageBitmap);
            return bmRet;
        }

        /// <summary>
        /// Rotates image by specified amount of degrees
        /// </summary>
        /// <param name="bitmap">Image</param>
        /// <param name="angle">Amount of degrees to rotate image by. Must be 90, 180, or 270</param>
        /// <returns>Rotated image</returns>
        static public Bitmap Rotate(Bitmap bitmap, float angle)
        {
            if ( angle < 0 )
                angle += 360;
            if (angle != 0 && angle != 90 && angle != 180 && angle != 270)
                throw new ArgumentException("Only -90, 0, 90, 180 or 270 degrees rotation is supported", "angle");
            IBitmapImage imageBitmap = BitmapToIImageBitmap(bitmap);
            IBasicBitmapOps ops = (IBasicBitmapOps)imageBitmap;
            ops.Rotate(angle, InterpolationHint.InterpolationHintDefault, out imageBitmap);
            Bitmap bmRet = IBitmapImageToBitmap(imageBitmap);
            Marshal.FinalReleaseComObject(imageBitmap);
            return bmRet;
        }

        /// <summary>
        /// Flips image around X and/or Y axes
        /// </summary>
        /// <param name="bitmap">Image</param>
        /// <param name="flipX">Whether to flip around X axis</param>
        /// <param name="flipY">Whether to flip around Y axis</param>
        /// <returns>Flipped image</returns>
        static public Bitmap Flip(Bitmap bitmap, bool flipX, bool flipY)
        {
            IBitmapImage imageBitmap = BitmapToIImageBitmap(bitmap);
            IBasicBitmapOps ops = (IBasicBitmapOps)imageBitmap;
            ops.Flip(flipX, flipY, out imageBitmap);
            Bitmap bmRet = IBitmapImageToBitmap(imageBitmap);
            Marshal.FinalReleaseComObject(imageBitmap);
            return bmRet;
        }

        /// <summary>
        /// Converts Imaging API IBitmapImage object to .NET Bitmap object
        /// </summary>
        /// <param name="imageBitmap">Source IImageBitmap object</param>
        /// <returns>Bitmap object</returns>
        [CLSCompliant(false)]
        static public Bitmap IBitmapImageToBitmap(IBitmapImage imageBitmap)
        {
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
#if !NDOC
            ImagingFactory factory = new ImagingFactoryClass();
            IImage image, imageThumb;
            factory.CreateImageFromStream(new StreamOnFile(stream), out image);
            image.GetThumbnail((uint)size.Width, (uint)size.Height, out imageThumb);
#endif
            IBitmapImage imageBitmap = null;
#if !NDOC
            ImageInfo ii;
            image.GetImageInfo(out ii);
            factory.CreateBitmapFromImage(image, (uint)size.Width, (uint)size.Height, ii.PixelFormat, InterpolationHint.InterpolationHintDefault, out imageBitmap);
#endif
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
#if !NDOC
            ImagingFactory factory = new ImagingFactoryClass();
            System.Drawing.Imaging.BitmapData bd = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
#endif
            IBitmapImage imageBitmap = null;
#if !NDOC
            factory.CreateNewBitmap((uint)bd.Width, (uint)bd.Height, bd.PixelFormat, out imageBitmap);
            BitmapDataInternal bdi = new BitmapDataInternal();
            RECT rc = new RECT(0, 0, bd.Width, bd.Height);
            imageBitmap.LockBits(rc, (int)ImageLockMode.WriteOnly, bd.PixelFormat, ref bdi);
            int cb = bdi.Stride * bdi.Height;
            CopyMemory(bdi.Scan0, bd.Scan0, cb);
            bitmap.UnlockBits(bd);
            imageBitmap.UnlockBits(ref bdi);
#endif
            return imageBitmap;
        }

        static internal void CopyMemory(IntPtr pDst, IntPtr pSrc, int cbSize)
        {
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
            StreamOnFile st = new StreamOnFile(imagePath);
            IImageDecoder decoder = null;
#if !NDOC
            ImagingFactory factory = new ImagingFactoryClass();
            factory.CreateImageDecoder(st, DecoderInitFlag.DecoderInitFlagNone, out decoder);
#endif
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
            IntPtr pPalette;
            if ( bitmap.GetPalette(out pPalette) != 0 )
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
            return bitmap.SetPalette(palette.ConvertToMemory()) == 0;
        }

    }
}
