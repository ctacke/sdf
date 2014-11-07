
using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using OpenNETCF.Drawing.Imaging;

namespace OpenNETCF.Drawing
{
	/// <summary>
	/// Summary description for GDIPlus.
	/// </summary>
	internal class GDIPlus
	{
		private GDIPlus(){}

		#region Font stuff
		[DllImport("coredll.dll", SetLastError=true, EntryPoint="CreateFontIndirect")]
		private static extern IntPtr CreateFontIndirectCE(IntPtr pLogFont);

		/// <summary>
		/// This function creates a logical font that has the characteristics specified in the specified structure. 
		/// An application can subsequently select the font as the current font for any device context (DC). 			
		/// </summary>
		/// <param name="pLogFont">Long pointer to a <see cref="LOGFONT"/> that defines the characteristics of the logical font.</param>
		/// <returns>A handle to a logical font.</returns>
		public static IntPtr CreateFontIndirect(IntPtr pLogFont)
		{
			IntPtr font = CreateFontIndirectCE(pLogFont);
			if (font == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Impossible to create a logical font.");
			return font;
		}

		/// <summary>
		/// This structure defines the attributes of a font.
		/// </summary>
		public class LOGFONT
		{
			public int      lfHeight;
			public int      lfWidth;
			public int      lfEscapement;
			public int      lfOrientation;
			public int      lfWeight;
			public byte      lfItalic;
			public byte      lfUnderline;
			public byte      lfStrikeOut;
			public byte      lfCharSet;
			public byte      lfOutPrecision;
			public byte      lfClipPrecision;
			public byte      lfQuality;
			public byte      lfPitchAndFamily = 0;
			//public char[]    lfFaceName;
		} 

		public const int TRANSPARENT        = 1;
		public const int OPAQUE             = 2;

		public const int DT_TOP                     = 0x00000000;
		public const int DT_LEFT                    = 0x00000000;
		public const int DT_CENTER                  = 0x00000001;
		public const int DT_RIGHT                   = 0x00000002;
		public const int DT_VCENTER                 = 0x00000004;
		public const int DT_BOTTOM                  = 0x00000008;
		public const int DT_WORDBREAK               = 0x00000010;
		public const int DT_SINGLELINE              = 0x00000020;
		public const int DT_EXPANDTABS              = 0x00000040;
		public const int DT_TABSTOP                 = 0x00000080;
		public const int DT_NOCLIP                  = 0x00000100;
		public const int DT_EXTERNALLEADING         = 0x00000200;
		public const int DT_CALCRECT                = 0x00000400;
		public const int DT_NOPREFIX                = 0x00000800;
		public const int DT_INTERNAL                = 0x00001000;

		public const int DT_EDITCONTROL             = 0x00002000;
		public const int DT_PATH_ELLIPSIS           = 0x00004000;
		public const int DT_END_ELLIPSIS            = 0x00008000;
		public const int DT_MODIFYSTRING            = 0x00010000;
		public const int DT_RTLREADING              = 0x00020000;
		public const int DT_WORD_ELLIPSIS           = 0x00040000;
		public const int DT_NOFULLWIDTHCHARBREAK    = 0x00080000;

		public const int ANSI_CHARSET				 = 0x0;
		public const int DEFAULT_CHARSET         = 1;

		public const uint FF_DONTCARE         =(0<<4);  /* Don't care or don't know. */
		public const uint FF_ROMAN            =(1<<4);  /* Variable stroke width, serifed. */
		/* Times Roman, Century Schoolbook, etc. */
		public const uint FF_SWISS            =(2<<4);  /* Variable stroke width, sans-serifed. */
		/* Helvetica, Swiss, etc. */
		public const uint FF_MODERN           =(3<<4);  /* Constant stroke width, serifed or sans-serifed. */
		/* Pica, Elite, Courier, etc. */
		public const uint FF_SCRIPT           =(4<<4);  /* Cursive, etc. */
		public const uint FF_DECORATIVE       =(5<<4);  /* Old English, etc. */

		/* Font Weights */
		public const int FW_DONTCARE         =0;
		public const int FW_THIN             =100;
		public const int FW_EXTRALIGHT       =200;
		public const int FW_LIGHT            =300;
		public const int FW_NORMAL           =400;
		public const int FW_MEDIUM           =500;
		public const int FW_SEMIBOLD         =600;
		public const int FW_BOLD             =700;
		public const int FW_EXTRABOLD        =800;

		public const uint DEFAULT_QUALITY        = 0;
		public const uint DRAFT_QUALITY          = 1;
		public const uint PROOF_QUALITY          = 2;
		public const uint NONANTIALIASED_QUALITY = 3;
		public const uint ANTIALIASED_QUALITY    = 4;
		public const uint CLEARTYPE_QUALITY  =     5;


		#endregion


		[DllImport("message.dll", SetLastError=true)]
		public static extern IntPtr LoadImageDec(string file);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr LocalAlloc(int flags, int size);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern void LocalFree(IntPtr p);

		public const int BI_RGB	=		0;
		public const int BI_BITFIELDS  = 3;
		public const int BI_ALPHABITFIELDS   = 6;

		public const int SRCCOPY     = 0x00CC0020;

		public struct TRIVERTEX
		{
			public int x;
			public int y;
			public int Red;
			public int Green;
			public int Blue;
			public int Alpha;

			private TRIVERTEX(int x, int y, int r, int b, int g, int a)
			{
				this.x = x;
				this.y = y;
				this.Red = r;
				this.Blue = b;
				this.Green = g;
				this.Alpha = a;
			}
		}

		public struct GRADIENT_RECT
		{
			public int UpperLeft;
			public int LowerRight;

			private GRADIENT_RECT(int UpperLeft, int LowerRight)
			{
				this.UpperLeft = UpperLeft;
				this.LowerRight = LowerRight;
			}
		}

		public struct POINT
		{
			public int x;
			public int y;
			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}

		public enum PenStyle
		{
			PS_SOLID = 0,
			PS_DASH = 1,
			PS_NULL = 5
		}

		public enum ROP
		{
			R2_BLACK          =  1,   /*  0       */
			R2_NOTMERGEPEN      =2,   /* DPon     */
			R2_MASKNOTPEN       =3,   /* DPna     */
			R2_NOTCOPYPEN       =4,   /* PN       */
			R2_MASKPENNOT       =5,   /* PDna     */
			R2_NOT              =6,   /* Dn       */
			R2_XORPEN           =7,   /* DPx      */
			R2_NOTMASKPEN       =8,   /* DPan     */
			R2_MASKPEN          =9,   /* DPa      */
			R2_NOTXORPEN        =10,  /* DPxn     */
			R2_NOP              =11,  /* D        */
			R2_MERGENOTPEN      =12,  /* DPno     */
			R2_COPYPEN          =13,  /* P        */
			R2_MERGEPENNOT      =14,  /* PDno     */
			R2_MERGEPEN         =15,  /* DPo      */
			R2_WHITE            =16,  /*  1       */
			R2_LAST             =16,		
		}

		public const int LOGPIXELSX =   88;    /* Logical pixels/inch in X                 */
		public const int LOGPIXELSY  =  90;    /* Logical pixels/inch in Y                 */

		public struct RECT 
		{ 
			public int left; 
			public int top; 
			public int right; 
			public int bottom; 
		}  

		public struct SIZE
		{
			public int width;
			public int height;
		}

		public struct BITMAP 
		{
			public int bmType; 
			public int bmWidth; 
			public int bmHeight; 
			public int bmWidthBytes; 
			public ushort bmPlanes; 
			public ushort bmBitsPixel; 
			public int bmBits; 
		} 


		public struct BITMAPINFOHEADER
		{
			public uint  biSize; 
			public int   biWidth; 
			public int   biHeight; 
			public ushort   biPlanes; 
			public ushort   biBitCount; 
			public uint  biCompression; 
			public uint  biSizeImage; 
			public int   biXPelsPerMeter; 
			public int   biYPelsPerMeter; 
			public uint  biClrUsed; 
			public uint  biClrImportant; 
		}
		
		struct BITMAPFILEHEADER 
		{ 
			public ushort  bfType; 
			public uint    bfSize; 
			public ushort  bfReserved1; 
			public ushort  bfReserved2; 
			public uint    bfOffBits; 

			private BITMAPFILEHEADER(ushort type, uint size, uint offset)
			{
				this.bfReserved1 = 0;
				this.bfReserved2 = 1;

				this.bfType = type;
				this.bfSize = size;
				this.bfOffBits = offset;
			}
		}

		[DllImport("coredll.dll", EntryPoint="GetCapture", SetLastError=true)]
		public static extern IntPtr GetCapture();

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr SetCapture(IntPtr hWnd);

		[DllImport("coredll.dll", EntryPoint="GetDC", SetLastError=true)]
		public static extern IntPtr GetDC(IntPtr hWnd);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr CreateCompatibleBitmap(
			IntPtr hdc, 
			int nWidth, 
			int nHeight ); 

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int Polyline(
			IntPtr hdc, 
			int[] lppt, 
			int cPoints ); 

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc ); 
		

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern void ReleaseDC( IntPtr hDC );

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr CreatePen(int fnPenStyle, int nWidth, int crColor);


		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int GetWindowRect( 
			IntPtr hWnd, 
			ref RECT lpRect ); 


		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int GetWindowRect( 
			IntPtr hWnd, 
			IntPtr lpRect ); 


		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr CreateSolidBrush(int color);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr CreateSolidBrush(int[] color);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr GetFocus();
		
		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int SelectClipRgn(IntPtr hDC, IntPtr hRgn);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int SetTextColor(IntPtr hDC, int cColor);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int SetBkColor(IntPtr hDC, int cColor);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int SetBkMode(IntPtr hDC, int nMode);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr SetROP2(IntPtr hDC, ROP rop);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int DrawText(IntPtr hDC, string Text, int nLen, IntPtr pRect, uint uFormat);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int DrawText(IntPtr hDC, string Text, int nLen, ref RECT rect , uint uFormat);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int ExtTextOut(
			IntPtr hdc, 
			int X, 
			int Y, 
			uint fuOptions, 
			ref RECT lprc, 
				string lpString, 
				int cbCount, 
				int[] lpDx ); 


		//		[DllImport("coredll")]
		//		public static extern bool Polyline( IntPtr hdc, int[] Points, int cPoints );

		[DllImport("coredll", SetLastError=true)]
		public static extern bool Polygon(
			IntPtr hdc, 
			ref POINT[] lpPoints, 
			int nCount ); 


		[DllImport("coredll", SetLastError=true)]
		public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

		[DllImport("coredll", SetLastError=true)]
		public static extern IntPtr DeleteObject(IntPtr hObject);

		[DllImport("coredll", SetLastError=true)]
		public static extern int DeleteDC(
			IntPtr hdc ); 

		[DllImport("coredll", SetLastError=true)]
		public static extern int FillRect(
			IntPtr hDC, 
			ref Rectangle lprc, 
			IntPtr hbr); 


		[DllImport("coredll", SetLastError=true)]
		public static extern int FillRgn( 
			IntPtr hdc, 
			IntPtr hrgn, 
			IntPtr hbr );
		

		[DllImport("coredll", SetLastError=true)]
		public static extern  IntPtr CreateRectRgn( 
			int nLeftRect, 
			int nTopRect, 
			int nRightRect, 
			int nBottomRect );

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern bool Rectangle(IntPtr hdc, 
			int nLeftRect, 
			int nTopRect, 
			int nRightRect, 
			int nBottomRect
			);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int LineTo(
			IntPtr hdc,
			int nXEnd,
			int nYEnd);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int MoveToEx(
			IntPtr hdc,
			int X,
			int Y,
			ref POINT lpPoint);

		[DllImport("coredll.dll", EntryPoint="GradientFill", SetLastError=true)]
		public static extern bool GradientFill(IntPtr hdc, /*TRIVERTEX[]*/uint[] pVertex,
			int dwNumVertex, /*GRADIENT_RECT*/int[] pMesh, int dwNumMesh,
			int dwMode);

		[DllImport("aygshell.dll", EntryPoint="#75", SetLastError=true)]
		public static extern IntPtr SHLoadImageFile(
			string szFileName ); 

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int GetObject(
			IntPtr hgdiobj, 
			int cbBuffer, 
			ref BITMAP lpvObject ); 

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int BitBlt(
			IntPtr hdcDest, 
			int nXDest, 
			int nYDest, 
			int nWidth, 
			int nHeight, 
			IntPtr hdcSrc, 
			int nXSrc, 
			int nYSrc, 
			uint dwRop);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int StretchBlt(
			IntPtr hdcDest, 
			int nXOriginDest, 
			int nYOriginDest, 
			int nWidthDest, 
			int nHeightDest, 
			IntPtr hdcSrc, 
			int nXOriginSrc, 
			int nYOriginSrc, 
			int nWidthSrc, 
			int nHeightSrc, 
			uint dwRop ); 

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int GetTextExtentExPoint(
			IntPtr hdc, 
			string lpszStr, 
			int cchString, 
			int nMaxExtent, 
			out int lpnFit, 
			int[] alpDx, 
			ref SIZE lpSize ); 

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern uint SetPixel(
			IntPtr hdc, 
			int X, 
			int Y, 
			uint crColor ); 


		public struct TEXTMETRIC 
		{
			public int tmHeight; 
			public int tmAscent; 
			public int tmDescent; 
			public int tmInternalLeading; 
			public int tmExternalLeading; 
			public int tmAveCharWidth; 
			public int tmMaxCharWidth; 
			public int tmWeight; 
			public int tmOverhang; 
			public int tmDigitizedAspectX; 
			public int tmDigitizedAspectY; 
			public char tmFirstChar; 
			public char tmLastChar; 
			public char tmDefaultChar; 
			public char tmBreakChar; 
			public byte tmItalic; 
			public byte tmUnderlined; 
			public byte tmStruckOut; 
			public byte tmPitchAndFamily; 
			public byte tmCharSet; 

			private TEXTMETRIC(int tmHeight, int tmAscent, int tmDescent, int tmInternalLeading, int tmExternalLeading, int tmAveCharWidth, int tmMaxCharWidth, int tmWeight, int tmOverhang, int tmDigitizedAspectX, int tmDigitizedAspectY, char tmFirstChar, char tmLastChar, char tmDefaultChar, char tmBreakChar, byte tmItalic, byte tmUnderlined, byte tmStruckOut, byte tmPitchAndFamily, byte tmCharSet)
			{
				this.tmHeight           = tmHeight;
				this.tmAscent           = tmAscent;
				this.tmDescent          = tmDescent;
				this.tmInternalLeading  = tmInternalLeading;
				this.tmExternalLeading  = tmExternalLeading;
				this.tmAveCharWidth     = tmAveCharWidth;
				this.tmMaxCharWidth     = tmMaxCharWidth;
				this.tmWeight           = tmWeight;
				this.tmOverhang         = tmOverhang;
				this.tmDigitizedAspectX = tmDigitizedAspectX;
				this.tmDigitizedAspectY = tmDigitizedAspectY;
				this.tmFirstChar        = tmFirstChar;
				this.tmLastChar         = tmLastChar;
				this.tmDefaultChar      = tmDefaultChar;
				this.tmBreakChar        = tmBreakChar;
				this.tmItalic           = tmItalic;
				this.tmUnderlined       = tmUnderlined;
				this.tmStruckOut        = tmStruckOut;
				this.tmPitchAndFamily   = tmPitchAndFamily;
				this.tmCharSet          = tmCharSet;        
			}
		}

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int GetPixel(
			IntPtr hdc, 
			int nXPos, 
			int nYPos ); 

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int GetTextMetrics(
			IntPtr hdc, 
			ref TEXTMETRIC lptm
			); 

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr CreateDIBSection(IntPtr hdc, BITMAPINFOHEADER hdr, uint colors, ref IntPtr pBits, IntPtr hFile, uint offset);
		[DllImport("coredll.dll", SetLastError=true)]
		public static extern IntPtr CreateDIBSection(IntPtr hdc, IntPtr hdr, uint colors, ref IntPtr pBits, IntPtr hFile, uint offset);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int SetBitmapBits(IntPtr hBitmap, int flag, IntPtr hData);


		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int  TransparentImage(
			IntPtr IntPtr, 
			int DstX, 
			int DstY, 
			int DstCx, 
			int DstCy,
			IntPtr hSrc, 
			int SrcX, 
			int SrcY, 
			int SrcCx, 
			int SrcCy, 
			int TransparentColor );


		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int DrawFrameControl( 
			IntPtr hdc, 
			ref Rectangle rect, 
			int uType, 
			int uState ); 

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int RoundRect(
			IntPtr hdc, 
			int nLeftRect, 
			int nTopRect, 
			int nRightRect, 
			int nBottomRect, 
			int nWidth, 
			int nHeight ); 

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int Ellipse(
			IntPtr hdc, 
			int nLeftRect, 
			int nTopRect, 
			int nRightRect, 
			int nBottomRect);


		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int GetDeviceCaps(
			IntPtr hdc, 
			int nIndex );

		/*public static uint RGB(Color color)
		{
			// Format the value of color - 0x00bbggrr
			return ((uint) (((uint) (color.R) | ((uint) (color.G) << 8)) | (((uint)
				(color.B)) << 16)));
		}*/

		internal const int GPTR = 0x40;

		private static int BytesPerLine(int nWidth, int nBitsPerPixel)
		{
			return ( (nWidth * nBitsPerPixel + 31) & (~31) ) / 8;
		}

		public static IntPtr BitmapLockBits(BitmapEx bitmap, int flags, int format, BitmapData bitmapData)
		{	
			IntPtr hDC = GetDC(IntPtr.Zero);

			BITMAPINFOHEADER bi = new BITMAPINFOHEADER();
			bi.biSize = (uint)Marshal.SizeOf(bi);
			//For now they are all converted to 24 bits
			bi.biBitCount = 24; // Creating RGB bitmap. The following three members don't matter
			//bi.biBitCount = (ushort)bitmap.bitsPixel;
			bi.biClrUsed = 0;
			bi.biClrImportant = 0;
			if (bi.biBitCount == 16  || bi.biBitCount == 32)
				bi.biCompression = BI_BITFIELDS; 
			else
				bi.biCompression = BI_RGB;
			//bi.biCompression = 0;
			bi.biHeight = bitmap.Height;
			bi.biWidth = bitmap.Width;
			bi.biPlanes = 1;
			int cb = (int)(bi.biHeight * bi.biWidth * bi.biBitCount / 8); //8 is bits per byte
			//bi.biSizeImage = (uint)cb;
			bi.biSizeImage = (uint)(BytesPerLine(bi.biBitCount, bitmap.Width) * bitmap.Height);
			bi.biXPelsPerMeter = 0xb12; // 72 ppi, 96 would work well too
			bi.biYPelsPerMeter = 0xb12; // 72 ppi

			IntPtr pBits = IntPtr.Zero;
			//Allocate memory for bitmap bits
			IntPtr pBI = LocalAlloc(GPTR, (int)bi.biSize);
			// Not sure if this needed - simply trying to keep marshaller happy
			Marshal.StructureToPtr(bi, pBI, false);
			//This will return IntPtr to actual DIB bits in pBits
			IntPtr hBmp = CreateDIBSection(hDC, pBI, 0, ref pBits, IntPtr.Zero, 0);
			//Marshall back - now we have BITMAPINFOHEADER correctly filled in
			Marshal.PtrToStructure(pBI, bi);
			BITMAPINFOHEADER biNew = (BITMAPINFOHEADER)Marshal.PtrToStructure(pBI, typeof( BITMAPINFOHEADER ));
			
			IntPtr hMemDC = CreateCompatibleDC(hDC);
			IntPtr hTargetDC = CreateCompatibleDC(hDC);
			//Usual stuff
			IntPtr hOldBitmap1 = SelectObject(hMemDC, bitmap.hBitmap);
			IntPtr hOldBitmap2 = SelectObject(hTargetDC, hBmp);
			//Grab bitmap
			int nRet = BitBlt(hTargetDC, 0, 0, bitmap.Width, bitmap.Height, hMemDC, 0, 0, SRCCOPY);

			bitmapData.Height = bitmap.Height;
			bitmapData.Width = bitmap.Width;
			bitmapData.Scan0 = pBits;
			bitmapData.Stride = BytesPerLine(biNew.biBitCount, bitmap.Width);

			//Restore
			SelectObject(hMemDC, hOldBitmap1);
			SelectObject(hTargetDC, hOldBitmap2);
			//Clean up
			DeleteDC(hMemDC);
			DeleteDC(hTargetDC);

			return hBmp;
		}

	
		private int CountBits(int dw)
		{
			int iBits = 0;
    
			while (dw == 0) 
			{
				iBits += (dw & 1);
				dw >>= 1;
			}
    
			return iBits;
		}


		public static void Snapshot(IntPtr hReal, string FileName, Rectangle rect)
		{
			RECT rc = new RECT();
			rc.bottom = 0;
			rc.left = 0;
			rc.right = 0;
			rc.top = 0;	
			IntPtr pRC = LocalAlloc(GPTR, Marshal.SizeOf(rc));
			GetWindowRect(hReal, pRC);
			rc = (RECT)Marshal.PtrToStructure(pRC, typeof(RECT));
			int nWidth = rc.right-rc.left-1;
			int nHeight = rc.bottom-rc.top;

			nWidth = rect.Width;
			nHeight = rect.Height;

			//User32.UpdateWindow(hReal);

			IntPtr hDCInk = GetDC(hReal);
			IntPtr hMemDC = CreateCompatibleDC(hDCInk);

			BITMAPINFOHEADER bi = new BITMAPINFOHEADER();
			bi.biSize = (uint)Marshal.SizeOf(bi);
			bi.biBitCount = 24; // Creating RGB bitmap. The following three members don't matter
			bi.biClrUsed = 0;
			bi.biClrImportant = 0;
			bi.biCompression = 0;
			bi.biHeight = nHeight;
			bi.biWidth = nWidth;
			bi.biPlanes = 1;
			int cb = (int)(bi.biHeight * bi.biWidth * bi.biBitCount / 8); //8 is bits per byte
			bi.biSizeImage = (uint)cb;
			bi.biXPelsPerMeter = 0xb12; // 72 ppi, 96 would work well too
			bi.biYPelsPerMeter = 0xb12; // 72 ppi

			IntPtr pBits = IntPtr.Zero;
			//Allocate memory for bitmap bits
			IntPtr pBI = LocalAlloc(GPTR, (int)bi.biSize);
			// Not sure if this needed - simply trying to keep marshaller happy
			Marshal.StructureToPtr(bi, pBI, false);
			//This will return IntPtr to actual DIB bits in pBits
			IntPtr hBmp = CreateDIBSection(hDCInk, pBI, 0, ref pBits, IntPtr.Zero, 0);
			//Marshall back - now we have BITMAPINFOHEADER correctly filled in
			Marshal.PtrToStructure(pBI, bi);
			BITMAPINFOHEADER biNew = (BITMAPINFOHEADER)Marshal.PtrToStructure(pBI, typeof( BITMAPINFOHEADER ));

			//Usual stuff
			IntPtr hOldBitmap = SelectObject(hMemDC, hBmp);
			//Grab bitmap
			int nRet = BitBlt(hMemDC, 0, 0, nWidth, nHeight, hDCInk, 0, 0, SRCCOPY);
			// Allocate memory for a copy of bitmap bits
			byte[] RealBits = new byte[cb];
			// And grab bits from DIBSestion data
			Marshal.Copy(pBits, RealBits, 0, cb);

			// This simply creates valid bitmap file header, so it can be saved to disk
			BITMAPFILEHEADER bfh = new BITMAPFILEHEADER();
			bfh.bfSize = (uint)cb + 0x36; // Size of header + size of BITMAPINFOHEADER size of bitmap bits
			bfh.bfType = 0x4d42; //BM
			bfh.bfOffBits = 0x36; // 
			int HdrSize = 14;
			byte[] header = new byte[HdrSize];
			BitConverter.GetBytes(bfh.bfType).CopyTo(header, 0);
			BitConverter.GetBytes(bfh.bfSize).CopyTo(header, 2);
			BitConverter.GetBytes(bfh.bfOffBits).CopyTo(header, 10);

			//Allocate enough memory for complete bitmap file
			byte[] data = new byte[cb+0x36];
			//BITMAPFILEHEADER
			header.CopyTo(data, 0);

			//BITMAPINFOHEADER
			header = new byte[Marshal.SizeOf(bi)];
			IntPtr pHeader = LocalAlloc(GPTR, Marshal.SizeOf(bi));
			Marshal.StructureToPtr(biNew, pHeader, false);
			Marshal.Copy(pHeader, header, 0, Marshal.SizeOf(bi));
			LocalFree(pHeader);
			header.CopyTo(data, HdrSize);
			
			//Bitmap bits
			RealBits.CopyTo(data, 0x36);

			FileStream fs = new FileStream(FileName, FileMode.Create);
			fs.Write(data, 0, data.Length);
			fs.Flush();
			fs.Close();

			data = null;
			
			DeleteObject(SelectObject(hMemDC, hOldBitmap));
			DeleteDC(hMemDC);
			ReleaseDC(hDCInk);
		}

	}
}
