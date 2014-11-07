
using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// Contains information about the dimensions and color format of a device-independent bitmap (DIB).
	/// </summary>
	/// <remarks>Wrapper for Win32 <b>BITMAPINFOHEADER</b> structure</remarks>
	public class BitmapInfoHeader
	{
		private byte[] data;
		public BitmapInfoHeader()
		{
			data = new byte[40];
		}

		/// <summary>
		/// Specifies the number of bytes required by the structure.
		/// </summary>
		public int Size
		{
			get { return BitConverter.ToInt32(data, 0); }
			set { BitConverter.GetBytes(value).CopyTo(data, 0); }
		}
		/// <summary>
		/// Specifies the width of the bitmap, in pixels.
		/// </summary>
		public int Width
		{
			get { return BitConverter.ToInt32(data, 4); }
			set { BitConverter.GetBytes(value).CopyTo(data, 4); }
		}
		/// <summary>
		/// Specifies the height of the bitmap, in pixels.
		/// If biHeight is positive, the bitmap is a bottom-up DIB and its origin is the lower-left corner.
		/// If biHeight is negative, the bitmap is a top-down DIB and its origin is the upper-left corner.
		/// If biHeight is negative, indicating a top-down DIB, biCompression must be either BI_RGB or BI_BITFIELDS.
		/// Top-down DIBs cannot be compressed.
		/// </summary>
		public int  Height 
		{
			get { return BitConverter.ToInt32(data, 8); }
			set { BitConverter.GetBytes(value).CopyTo(data, 8); }
		}
		/// <summary>
		/// Specifies the number of planes for the target device.
		/// This value must be set to 1.
		/// </summary>
		public short  Planes
		{
			get { return BitConverter.ToInt16(data, 12); }
			set { BitConverter.GetBytes(value).CopyTo(data, 12); }
		}
		/// <summary>
		/// Specifies the number of bits per pixel.
		/// The biBitCount member determines the number of bits that define each pixel and the maximum number of colors in the bitmap.
		/// </summary>
		public short  BitCount
		{
			get { return BitConverter.ToInt16(data, 14); }
			set { BitConverter.GetBytes(value).CopyTo(data, 14); }
		}
		/// <summary>
		/// Specifies the type of compression for a compressed bottom-up bitmap (top-down DIBs cannot be compressed).
		/// </summary>
		public int Compression
		{
			get { return BitConverter.ToInt32(data, 16); }
			set { BitConverter.GetBytes(value).CopyTo(data, 16); }
		}
		/// <summary>
		/// Specifies the size, in bytes, of the image. This may be set to zero for BI_RGB bitmaps.
		/// </summary>
		public int SizeImage
		{
			get { return BitConverter.ToInt32(data, 20); }
			set { BitConverter.GetBytes(value).CopyTo(data, 20); }
		}
		/// <summary>
		/// Specifies the horizontal resolution, in pixels per meter, of the target device for the bitmap.
		/// An application can use this value to select a bitmap from a resource group that best matches the characteristics of the current device.
		/// </summary>
		public int  XPelsPerMeter
		{
			get { return BitConverter.ToInt32(data, 24); }
			set { BitConverter.GetBytes(value).CopyTo(data, 24); }
		}
		/// <summary>
		/// Specifies the vertical resolution, in pixels per meter, of the target device for the bitmap
		/// </summary>
		public int  YPelsPerMeter
		{
			get { return BitConverter.ToInt32(data, 28); }
			set { BitConverter.GetBytes(value).CopyTo(data, 28); }
		}
		/// <summary>
		/// Specifies the number of color indexes in the color table that are actually used by the bitmap.
		/// If this value is zero, the bitmap uses the maximum number of colors corresponding to the value of the biBitCount member for the compression mode specified by biCompression.
		/// </summary>
		public int ClrUsed
		{
			get { return BitConverter.ToInt32(data, 32); }
			set { BitConverter.GetBytes(value).CopyTo(data, 32); }
		}
		/// <summary>
		/// Specifies the number of color indexes required for displaying the bitmap.
		/// If this value is zero, all colors are required.
		/// </summary>
		public int ClrImportant
		{
			get { return BitConverter.ToInt32(data, 36); }
			set { BitConverter.GetBytes(value).CopyTo(data, 36); }
		}
		public byte[] Data { get { return data; } }
	}
}
