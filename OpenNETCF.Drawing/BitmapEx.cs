using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using OpenNETCF.Drawing.Imaging;
using System.Drawing.Imaging;

namespace OpenNETCF.Drawing
{
	/// <summary>
	/// Encapsulates a GDI+ bitmap, which consists of the pixel data for a graphics image and its attributes. A BitmapEx object is an object used to work with images defined by pixel data. 
	/// </summary>
	public class BitmapEx : IDisposable	
	{
		#region private fields

		internal IntPtr hBitmap = IntPtr.Zero;
		private IntPtr saveHBitmap = IntPtr.Zero;

		private int width;
		private int height;
		internal int widthBytes;
		internal int bitsPixel;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the BitmapEx class with the specified size.  
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public BitmapEx(int width, int height)
		{
			this.width = width;
			this.height = height;

		}

		/// <summary>
		/// Initializes a new instance of the System.Drawing.Bitmap class from the specified file.  
		/// </summary>
		/// <param name="filename"></param>
		public BitmapEx(string filename)
		{
			if (File.Exists(filename))
			{
				hBitmap = GDIPlus.SHLoadImageFile(filename);
				//hBitmap = GDIPlus.LoadImageDec(filename);
				
				if (hBitmap == IntPtr.Zero)
					throw new FileNotFoundException("Invalid image file.");

				GDIPlus.BITMAP bm = new GDIPlus.BITMAP();

				GDIPlus.GetObject(hBitmap, Marshal.SizeOf(bm), ref bm);
				width = bm.bmWidth;            // Get width of bitmap
				height = bm.bmHeight;           // Get height of bitmap
				widthBytes = bm.bmWidthBytes;
				bitsPixel = bm.bmBitsPixel;

			}
			else
				throw new FileNotFoundException("Image file doesn't exist.");
		}
		
		#endregion

		#region public methods

		/// <summary>
		/// Creates a GDI bitmap object from this BitmapEx object.  
		/// </summary>
		/// <returns>A handle to the GDI bitmap object that this method creates.</returns>
		public System.IntPtr GetHbitmap()
		{
			return hBitmap;
		}

		/// <summary>
		/// BitmapEx object into system memory.  
		/// </summary>
		/// <param name="rect">A System.Drawing.Rectangle structure specifying the portion of the BitmapEx to lock.  </param>
		/// <param name="flags">Access level (read and write) for the BitmapEx object. ></param>
		/// <param name="format">PixelFormat enumeration specifying the data format of this BitmapEx object.</param>
		/// <returns>BitmapData object containing information about this lock operation.  </returns>
		public OpenNETCF.Drawing.Imaging.BitmapData LockBits(Rectangle rect, int flags, PixelFormat format) 
		{
			//TODO: Don't care about PixelFormat. For now they are all converted to 24 bits
            OpenNETCF.Drawing.Imaging.BitmapData data;

            data = new OpenNETCF.Drawing.Imaging.BitmapData();
			saveHBitmap = hBitmap;
			hBitmap = GDIPlus.BitmapLockBits(this, 0, 0, data);

			GC.KeepAlive(this);
			
			return data;
		}

		/// <summary>
		/// Unlocks this BitmapEx from system memory.  
		/// </summary>
		/// <param name="data"></param>
        public void UnlockBits(OpenNETCF.Drawing.Imaging.BitmapData data)
		{
			GDIPlus.DeleteObject(saveHBitmap);
			//hBitmap = saveHBitmap;
		}

		#endregion

		#region public properties

		/// <summary>
		/// Gets the width of this BitmapEx object.
		/// </summary>
		public int Width
		{
			get 
			{ 
				return width; 
			} 
		}

		/// <summary>
		/// Gets the height of this BitmapEx object.
		/// </summary>
		public int Height
		{
			get 
			{ 
				return height; 
			} 
		}

		#endregion

		#region helper methods
		
		private int BytesPerLine(int nWidth, int nBitsPerPixel)
		{
			return ( (nWidth * nBitsPerPixel + 31) & (~31) ) / 8;
		}
		
		#endregion
		~BitmapEx()
		{
			Dispose();
		}

		#region IDisposable Members

		public void Dispose()
		{
			if ( hBitmap != IntPtr.Zero )
				GDIPlus.DeleteObject(hBitmap);

			GC.SuppressFinalize(this);
		}

		#endregion
	}

	namespace Imaging
	{
		/// <summary>
		/// Specifies the attributes of a bitmap image. The BitmapData class is used by the LockBits and UnlockBits methods of the BitmapEx class.
		/// </summary>
		public class BitmapData
		{
			#region private fields

			private int width;
			private int height;
			private int stride;
			private IntPtr scan0;
			private PixelFormat pixelFormat;

			#endregion

			public BitmapData()
			{

			}

			#region public properties

			/// <summary>
			/// Gets or sets the format of the pixel information.
			/// </summary>
			public PixelFormat PixelFormat
			{
				get 
				{ 
					return pixelFormat; 
				} 
				set 
				{ 
					pixelFormat = value; 
				} 
			}
		
			/// <summary>
			/// Gets or sets the address of the first pixel data in the bitmap.
			/// </summary>
			public IntPtr Scan0
			{
				get 
				{ 
					return scan0; 
				} 
				set 
				{ 
					scan0 = value; 
				} 
			}

			/// <summary>
			/// Gets or sets the stride width (also called scan width) of the bitmap object.  
			/// </summary>
			public int Stride
			{
				get 
				{ 
					return stride; 
				} 
				set 
				{ 
					stride = value; 
				} 
			}

			/// <summary>
			/// Gets or sets the pixel width of the BitmapEx object. This can also be thought of as the number of pixels in one scan line.  
			/// </summary>
			public int Width
			{
				get 
				{ 
					return width; 
				} 
				set 
				{ 
					width = value; 
				} 
			}

			/// <summary>
			/// Gets or sets the pixel height of the BitmapEx object. 
			/// </summary>
			public int Height
			{
				get 
				{ 
					return height; 
				} 
				set 
				{ 
					height = value; 
				} 
			}

			#endregion
		}
/*
		public enum PixelFormat
		{
			Format16bppRgb555 = 0x21005,
			Format16bppRgb565 = 0x21006,
			Format24bppRgb = 0x21808
		}
*/ 
	}
}
