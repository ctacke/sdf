
using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// This structure contains information about the type, size, and layout of a file that containing a device-independent bitmap (DIB).
	/// </summary>
	/// <remarks>Wraps the native <b>BITMAPFILEHEADER</b> structure</remarks>
	public class BitmapFileHeader 
	{ 
		private byte[] data;

		public BitmapFileHeader()
		{
			data = new byte[14];
		}
		/// <summary>
		/// Specifies the file type. It must be BM.
		/// </summary>
		public short  Type
		{
			get { return BitConverter.ToInt16(data, 0); }
			set { BitConverter.GetBytes(value).CopyTo(data, 0); }
		}
		/// <summary>
		/// Specifies the size, in bytes, of the bitmap file.
		/// </summary>
		public int    Size
		{
			get { return BitConverter.ToInt32(data, 2); }
			set { BitConverter.GetBytes(value).CopyTo(data, 2); }
		}

		//public ushort  bfReserved1;
		//public ushort  bfReserved2; 
		/// <summary>
		/// Specifies the offset, in bytes, from the <b>BitmapFileHeader</b> structure to the bitmap bits.
		/// </summary>
		public int    OffBits
		{
			get { return BitConverter.ToInt32(data, 10); }
			set { BitConverter.GetBytes(value).CopyTo(data, 10); }
		}

        /// <summary>
        /// Internal data
        /// </summary>
		public byte[] Data { get { return data; } }
	}
}
