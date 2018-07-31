using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace OpenNETCF.Drawing
{
	/// <summary>
	/// Encapsulates a GDI+ drawing surface.
	/// </summary>
    public partial class GraphicsEx: IDisposable
    {
		#region private fields
		
		internal IntPtr hDC;
		internal IntPtr hwnd;
		internal bool bCopy = false;
		private GDIPlus.RECT rect = new GDIPlus.RECT();
		internal bool dispose = false;
        private Graphics m_managedGraphics = null;
		#endregion

		#region constructors
        
		private GraphicsEx(IntPtr nativeGraphics)
		{
			this.hDC = nativeGraphics;
			this.dispose = false;
		}

		private GraphicsEx(IntPtr nativeGraphics, bool dispose)
		{
			this.hDC = nativeGraphics;
			this.dispose = dispose;
		}
		
		#endregion

		#region public methods

        /// <summary>
		/// Creates a new <see cref="GraphicsEx"/> object from the specified native graphics handle.  
		/// </summary>
		/// <param name="nativeGraphics">native graphics handle.</param>
		public static GraphicsEx FromHdc(IntPtr nativeGraphics)
		{
			GraphicsEx gx = null;

			if (nativeGraphics != IntPtr.Zero)
				gx = new GraphicsEx(nativeGraphics);
			else
				return null;

			return gx;
		}

		/// <summary>
		/// Creates a new <see cref="GraphicsEx"/> object from the specified handle to a window.  
		/// </summary>
		/// <param name="hwnd">Handle to a window.</param>
		/// <returns>This method returns a new <see cref="GraphicsEx"/> object for the specified window handle.  </returns>
		public static GraphicsEx FromHwnd(IntPtr hwnd)
		{
			//GraphicsEx gx = null;

			IntPtr hdc = GDIPlus.GetDC(hwnd);

			GraphicsEx gx =  FromHdc(hdc);
			gx.hwnd = hwnd;

			return gx;
		}
		
		/// <summary>
		/// Creates a new <see cref="GraphicsEx"/> object from the specified <see cref="Control"/> object.  
		/// </summary>
		/// <param name="ctl"><see cref="Control"/> object</param>
		/// <returns>This method returns a new OpenNETCF.Drawing.GraphicsEx object for the specified specified Control object.</returns>
		public static GraphicsEx FromControl(Control ctl)
		{
			IntPtr hwnd = GetControlHandle(ctl);
			GraphicsEx gx = FromHwnd(hwnd);
			GDIPlus.GetWindowRect(hwnd, ref gx.rect);
			return gx;
		}

		/// <summary>
		/// Creates a new copy of <see cref="GraphicsEx"/> object from the specified Control object.  
		/// </summary>
		/// <param name="ctl"><see cref="Control"/> object.</param>
		/// <returns>This method returns a new <see cref="GraphicsEx"/> object for the specified specified Control object.</returns>
		public static GraphicsEx CompatibleGraphics(Control ctl)
		{
			IntPtr hwnd = GetControlHandle(ctl);
			IntPtr hdc = GDIPlus.GetDC(hwnd);
			IntPtr hDCCopy = GDIPlus.CreateCompatibleDC(hdc);
			GraphicsEx gx = FromHdc(hDCCopy);
			GDIPlus.ReleaseDC(hwnd, hdc);
			gx.bCopy = true;
			return gx;
		}

		/// <summary>
		/// Creates a new copy of <see cref="GraphicsEx"/> object from the existing <see cref="GraphicsEx"/> objec.
		/// </summary>
		/// <returns></returns>
		public GraphicsEx CompatibleGraphics()
		{
			IntPtr hDCCopy = GDIPlus.CreateCompatibleDC(this.hDC);
			IntPtr hBmpMem = GDIPlus.CreateCompatibleBitmap(this.hDC, this.rect.right - this.rect.left, this.rect.bottom - this.rect.top);
			GDIPlus.SelectObject(hDCCopy, hBmpMem);
			GraphicsEx gx = FromHdc(hDCCopy);
			gx.bCopy = true;
			return gx;
		}

		/// <summary>
		/// Copies the graphics.
		/// </summary>
		/// <param name="ctl"></param>
		/// <param name="rc"></param>
		public void CopyGraphics(Control ctl, Rectangle rc)
		{
			IntPtr hwnd = GetControlHandle(ctl);
			IntPtr hdcSrc = GDIPlus.GetDC(hwnd);
			//GDIPlus.RECT srcRect = new GraphicsProj.GDIPlus.RECT();
			//GDIPlus.GetWindowRect(hwnd, ref srcRect);
			GDIPlus.BitBlt(hDC, rc.Left, rc.Top, rc.Width, rc.Height, hdcSrc, 0, 0, GDIPlus.SRCCOPY);
			GDIPlus.ReleaseDC(hwnd, hdcSrc);
		}

		/// <summary>
		/// Copies the graphics.
		/// </summary>
		/// <param name="gx"></param>
		/// <param name="rc"></param>
		public void CopyGraphics(GraphicsEx gx, Rectangle rc)
		{
			
			int ret = GDIPlus.BitBlt(hDC, rc.Left, rc.Top, rc.Width, rc.Height, gx.hDC, 0, 0, GDIPlus.SRCCOPY);

		}

		/// <summary>
		/// Copies graphics from <see cref="Control"/> with transparent color.
		/// </summary>
		/// <param name="ctl">Control to copy graphics from</param>
		/// <param name="rc">Rectangle to copy.</param>
		/// <param name="transpColor">Transaprent color.</param>
		public void CopyGraphics(Control ctl, Rectangle rc, Color transpColor)
		{
			IntPtr hwnd = GetControlHandle(ctl);
			IntPtr hdcSrc = GDIPlus.GetDC(hwnd);
			GDIPlus.RECT srcRect = new GDIPlus.RECT();
			GDIPlus.GetWindowRect(hwnd, ref srcRect);
			GDIPlus.TransparentImage(hDC, rc.Left, rc.Top, rc.Width, rc.Height, hdcSrc, 0, 0, srcRect.right - srcRect.left, srcRect.bottom - srcRect.top, ColorTranslator.ToWin32(transpColor)/*(int)GDIPlus.RGB(transpColor)*/);
			GDIPlus.ReleaseDC(hwnd, hdcSrc);
		}

		/// <summary>
		/// Draws a rectangle specified by a <see cref="Rectangle"/> structure. 
		/// </summary>
		/// <param name="pen">A <see cref="PenEx"/> object that determines the color, width, and style of the rectangle. </param>
		/// <param name="rc">A <see cref="Rectangle"/> structure that represents the rectangle to draw. </param>
		public void DrawRectangle(PenEx pen, Rectangle rc)
		{
			IntPtr hOldPen = IntPtr.Zero;
			hOldPen = GDIPlus.SelectObject(hDC, pen.hPen);
			int[] pt = new int[10];

			pt[0] = rc.Left;
			pt[1] = rc.Top;
			pt[2] = rc.Right-1;
			pt[3] = rc.Top;
			pt[4] = rc.Right-1;
			pt[5] = rc.Bottom-1;
			
			pt[6]  = rc.Left;
			pt[7]  = rc.Bottom-1;
			pt[8] = rc.Left;
			pt[9] = rc.Top;

			GDIPlus.Polyline(hDC,  pt, 5);
			GDIPlus.DeleteObject(GDIPlus.SelectObject(hDC, hOldPen));

		}


		/// <summary>
		/// Draws a rectangle.
		/// </summary>
		/// <param name="pen">A <see cref="PenEx"/> object that determines the color, width, and style of the rectangle. </param>
		/// <param name="x">x: x-coordinate of the upper-left corner of the rectangle to draw. </param>
		/// <param name="y"> y: y-coordinate of the upper-left corner of the rectangle to draw.  </param>
		/// <param name="cx">width: width of the rectangle to draw. </param>
		/// <param name="cy">height: Height of the rectangle to draw. </param>
		public void DrawRectangle(PenEx pen, int x , int y , int cx , int cy )
		{
			DrawRectangle(pen, new Rectangle(x, y, cx, cy));
		}

		/// <summary>
		/// Measures the specified string when drawn with the specified <see cref="FontEx"/> object.  
		/// </summary>
		/// <param name="text">String to measure.</param>
		/// <param name="font"><see cref="FontEx"/> object that defines the text format of the string.</param>
		/// <param name="width">Width to fit the string.</param>
		/// <returns></returns>
		public SizeF MeasureString(string text, FontEx font, int width)
		{
			//GDIPlus.SIZE sz = new GDIPlus.SIZE();
			IntPtr hdcTemp = GDIPlus.CreateCompatibleDC(hDC);
			IntPtr oldFont = GDIPlus.SelectObject(hdcTemp, font.hFont);

			GDIPlus.RECT rc = new GDIPlus.RECT();
			rc.right = width;
			rc.bottom = 320;

			int height = GDIPlus.DrawText(hdcTemp, text, text.Length, ref rc, 
				GDIPlus.DT_LEFT | GDIPlus.DT_TOP | GDIPlus.DT_WORDBREAK | GDIPlus.DT_CALCRECT); 

			GDIPlus.SelectObject(hdcTemp, oldFont); 
			GDIPlus.DeleteDC(hdcTemp);
			return new SizeF(width, height);
		}

		/// <summary>
		/// Measures the specified string when drawn with the specified <see cref="OpenNETCF.Drawing.FontEx"/> object.  
		/// </summary>
		/// <param name="text"><see cref="String"/> to measure.</param>
		/// <param name="font"><see cref="OpenNETCF.Drawing.FontEx"/> object that defines the text format of the string.</param>
		/// <returns></returns>
		public SizeF MeasureString(string text, FontEx font)
		{
			GDIPlus.SIZE sz = new GDIPlus.SIZE();
			int fit = 0;
			IntPtr hOldFont = GDIPlus.SelectObject(hDC, font.hFont);
			int width = 100;
			GDIPlus.GetTextExtentExPoint(hDC, text, text.Length, width, out fit, null, ref sz);
			GDIPlus.SelectObject(hDC, hOldFont);
			return new SizeF(sz.width, sz.height);
		}

		/// <summary>
		/// Fills the interior of a rectangle specified by a <see cref="System.Drawing.Rectangle"/> structure.  
		/// </summary>
		/// <param name="color">The <see cref="Color"/> to fill.</param>
		/// <param name="rc"><see cref="Rectangle"/> structure that represents the rectangle to fill. </param>
		public void FillRectangle(Color color, Rectangle rc)
		{
			IntPtr hRgn = GDIPlus.CreateRectRgn (rc.Left, rc.Top, rc.Right, rc.Bottom);

			// Create a solid brush.
			IntPtr hBrush = GDIPlus.CreateSolidBrush (ColorTranslator.ToWin32(color)/*GDIPlus.RGB(color)*/); 

			GDIPlus.FillRgn (hDC, hRgn, hBrush);

			// Delete the rectangular region. 
			GDIPlus.DeleteObject (hRgn);

			// Delete the brush object and free all resources associated with it.
			GDIPlus.DeleteObject (hBrush);
			

		}
		

		/// <summary>
		/// Draws the specified text string at the specified location with the specified <see cref="Color"/> and <see cref="FontEx"/> objects.
		/// </summary>
		/// <param name="text">String to draw</param>
		/// <param name="font"><see cref="FontEx"/> object that defines the text format of the string</param>
		/// <param name="textColor">The <see cref="Color"/> of text draw.</param>
		/// <param name="rc">Rectangle structure that specifies the location of the drawn text</param>
		public void DrawString(string text, FontEx font, Color textColor, Rectangle rc)
		{
			if (font.Angle == 0)
				DrawText(hDC, font.hFont, text, text.Length, ref rc, GDIPlus.DT_LEFT|GDIPlus.DT_TOP|GDIPlus.DT_WORDBREAK, textColor, Color.Empty);  
			else if (font.Angle == 90)
				DrawText(hDC, font.hFont, text, text.Length, ref rc, GDIPlus.DT_BOTTOM|GDIPlus.DT_LEFT, textColor, Color.Empty);  
			else if (font.Angle == 270)
				DrawText(hDC, font.hFont, text, text.Length, ref rc, GDIPlus.DT_RIGHT|GDIPlus.DT_CENTER, textColor, Color.Empty);  
			else
				DrawText(hDC, font.hFont, text, text.Length, ref rc, GDIPlus.DT_LEFT|GDIPlus.DT_TOP, textColor, Color.Empty);  


		}

//		public void DrawFrameControl(Rectangle rc, FrameType type, int state)
//		{
//			GDIPlus.DrawFrameControl(hDC, ref rc, (int)type, state);
//
//		}
		
		/// <summary>
		/// Draws a rectangle with rounded corners.
		/// </summary>
		/// <param name="pen">A <see cref="PenEx"/> object that determines the color, width, and style of the rectangle</param>
		/// <param name="rc">A <see cref="System.Drawing.Rectangle"/> structure that represents the rectangle to draw.</param>
		/// <param name="size">A <see cref="Size"/> structre that defines the corner radius.</param>
		public void DrawRoundRectangle(PenEx pen,  Rectangle rc, Size size)
		{
			IntPtr hOldPen = IntPtr.Zero;
			hOldPen = GDIPlus.SelectObject(hDC, pen.hPen);
			GDIPlus.RoundRect(hDC, rc.Left, rc.Top, rc.Right, rc.Bottom, size.Height, size.Width);
			GDIPlus.DeleteObject(GDIPlus.SelectObject(hDC, hOldPen));

		}

		/// <summary>
		/// Draws an ellipse specified by a bounding <see cref="System.Drawing.Rectangle"/> structure.
		/// </summary>
		/// <param name="pen">A <see cref="PenEx"/> object that determines the color, width, and style of the ellipse.</param>
		/// <param name="rc">A <see cref="System.Drawing.Rectangle"/> structure that represents the rectangle to draw.</param>
		public void DrawEllipse(PenEx pen, Rectangle rc)
		{
			IntPtr hOldPen = IntPtr.Zero;
			hOldPen = GDIPlus.SelectObject(hDC, pen.hPen);
			GDIPlus.Ellipse(hDC, rc.Left, rc.Top, rc.Right, rc.Bottom);
			GDIPlus.DeleteObject(GDIPlus.SelectObject(hDC, hOldPen));
		}

		/// <summary>
		/// Draws an ellipse defined by a bounding rectangle specified by a pair of coordinates, a height, and a width.
		/// </summary>
		/// <param name="pen">A <see cref="PenEx"/> object that determines the color, width, and style of the ellipse.</param>
		/// <param name="x">x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
		/// <param name="y">y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
		/// <param name="width">Width of the bounding rectangle that defines the ellipse.</param>
		/// <param name="height">Height of the bounding rectangle that defines the ellipse.</param>
		public void DrawEllipse(PenEx pen, int x, int y, int width, int height)
		{
			IntPtr hOldPen = IntPtr.Zero;
			hOldPen = GDIPlus.SelectObject(hDC, pen.hPen);
			GDIPlus.Ellipse(hDC, x, y, x + width, y + height);
			GDIPlus.DeleteObject(GDIPlus.SelectObject(hDC, hOldPen));
		}

		/// <summary>
		/// Draws the specified <see cref="BitmapEx"/> object at the specified location and with the original size.  
		/// </summary>
		/// <param name="image"><see cref="BitmapEx"/> object to draw.</param>
		/// <param name="x">x-coordinate of the upper-left corner of the drawn image.</param>
		/// <param name="y">y-coordinate of the upper-top corner of the drawn image.</param>
		public void DrawImage(BitmapEx image, int x, int y)
		{
			IntPtr hdcMem = GDIPlus.CreateCompatibleDC(hDC);
			IntPtr hOldSel = GDIPlus.SelectObject(hdcMem, image.hBitmap);
			GDIPlus.BitBlt(hDC, x, y, image.Width, image.Height, hdcMem, 0, 0, GDIPlus.SRCCOPY);
			GDIPlus.SelectObject(hdcMem, hOldSel);
			GDIPlus.DeleteDC(hdcMem);
		}


		/// <summary>
		/// Draws the specified portion of the specified System.Drawing.Image object at the specified location and with the specified size.  
		/// </summary>
		/// <param name="image"><see cref="OpenNETCF.Drawing.BitmapEx"/> object to draw.</param>
		/// <param name="destRect"><see cref="Rectangle"/> structure that specifies the location and size of the drawn image.
		/// The image is scaled to fit the rectangle.</param>
		/// <param name="srcRect"><see cref="Rectangle"/> structure that specifies the portion of the image object to draw.</param>
		public void DrawImage(BitmapEx image, Rectangle destRect, Rectangle srcRect)
		{
			IntPtr hdcMem = GDIPlus.CreateCompatibleDC(hDC);
			IntPtr hOldSel = GDIPlus.SelectObject(hdcMem, image.hBitmap);
			//GDIPlus.BitBlt(hDC, 0, 0, image.Width, image.Height, hdcMem, 0, 0, GDIPlus.SRCCOPY);
			GDIPlus.StretchBlt(hDC, destRect.Left, destRect.Top, destRect.Width, destRect.Height, hdcMem, srcRect.Left, srcRect.Top, srcRect.Width, srcRect.Height, GDIPlus.SRCCOPY); 
			GDIPlus.SelectObject(hdcMem, hOldSel);
			GDIPlus.DeleteDC(hdcMem);
		}

		/// <summary>
		/// Draws a line connecting the two points specified by coordinate pairs.  
		/// </summary>
		/// <param name="pen">PenEx object that determines the color, width, and style of the line.</param>
		/// <param name="xStart">x-coordinate of the first point.</param>
		/// <param name="yStart">y-coordinate of the first point.</param>
		/// <param name="xEnd">x-coordinate of the second point.</param>
		/// <param name="yEnd">x-coordinate of the seconf point.</param>
		public void DrawLine(PenEx pen, int xStart,  int yStart, int xEnd, int yEnd)
		{
			IntPtr hOldPen = IntPtr.Zero;
			hOldPen = GDIPlus.SelectObject(hDC, pen.hPen);
			//Set start position
			GDIPlus.POINT pt = new GDIPlus.POINT();
			GDIPlus.MoveToEx(hDC, xStart, yStart, ref pt);
			//Drawe line
			GDIPlus.LineTo(hDC, xEnd, yEnd);
			//Restore the initial position
			GDIPlus.MoveToEx(hDC, pt.x, pt.y, ref pt);
			//Clean up
			GDIPlus.DeleteObject(GDIPlus.SelectObject(hDC, hOldPen));
				
		}

		/// <summary>
		/// Gets the handle to the device context associated with this <see cref="GraphicsEx"/> object.
		/// </summary>
		/// <returns>Handle to the device context associated with this <see cref="GraphicsEx"/> object</returns>
		public IntPtr GetHdc()
		{
			return hDC;
		}
		#endregion


		#region internal and helper methods
		internal static IntPtr GetControlHandle(Control ctl)
		{
			
				IntPtr hOldWnd = GDIPlus.GetCapture();
				ctl.Capture = true;
				IntPtr hWnd = GDIPlus.GetCapture();
				ctl.Capture = false;
				GDIPlus.SetCapture(hOldWnd);
				return hWnd;

		}

		internal int DrawText(IntPtr hDC, IntPtr hFont, string sText, int nLen, ref Rectangle rect, uint uFormat, Color foreColor, Color backColor)
		{
			
			
			IntPtr hOldFont = GDIPlus.SelectObject(hDC, hFont);
			
			GDIPlus.SetTextColor(hDC, ColorTranslator.ToWin32(foreColor)/*(int)GDIPlus.RGB(foreColor)*/);
			GDIPlus.SetBkColor(hDC, ColorTranslator.ToWin32(backColor)/*(int)GDIPlus.RGB(backColor)*/);
			if ( backColor == Color.Empty )
				GDIPlus.SetBkMode(hDC, GDIPlus.TRANSPARENT);
			else
				GDIPlus.SetBkMode(hDC, GDIPlus.OPAQUE);

			GDIPlus.RECT rc = new GDIPlus.RECT();
			rc.left = rect.Left;
			rc.top = rect.Top;
			rc.right = rect.Right;
			rc.bottom = rect.Bottom;

			//int nRet = GDIPlus.DrawText(hDC, sText, nLen, ref rc, uFormat);

			int nRet = GDIPlus.ExtTextOut(hDC, rc.left, rc.top, uFormat, ref rc, sText, sText.Length, null);
			GDIPlus.SelectObject(hDC, hOldFont);

			return nRet;
		}
		#endregion

		#region destructor /dispose

		~GraphicsEx()
		{
			this.Dispose();
		}

		public void Dispose()
		{
            if (m_managedGraphics != null)
                m_managedGraphics.ReleaseHdc(hDC);
			else if (bCopy || dispose)
				GDIPlus.DeleteDC(hDC);
			else
				GDIPlus.ReleaseDC(hwnd, hDC);

			GC.SuppressFinalize(this);

		}
		
		#endregion

	}

	public enum FrameType
	{
		Caption    =         1,
		Scroll     =         3,
		Button     =         4
	}


	namespace Drawing2D
	{
		//replaces PenStyle

		/// <summary>
		/// Specifies the style of dashed lines drawn with a <see cref="PenEx"/> object.
		/// </summary>
		public enum DashStyle
		{
			/// <summary>
			/// Specifies a line consisting of dashes.
			/// </summary>
			Dash = 1,
			/// <summary>
			/// Specifies a solid line.
			/// </summary>
			Solid = 0,
		}
	}
}
