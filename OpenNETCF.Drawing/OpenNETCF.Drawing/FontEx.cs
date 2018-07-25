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
using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections;

namespace OpenNETCF.Drawing
{
	/// <summary>
	/// Defines a particular format for text, including font face, size, and style attributes. 
	/// </summary>
	public class FontEx : IDisposable	
	{
		#region private fields
 
		private float size;
		private string fontName;
		private FontStyle fontStyle;
		internal IntPtr hFont;
		private bool clearType = false;
		private int angle = 0;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new FontEx object that uses the specified attributes.  
		/// </summary>
		/// <param name="fontName">A string representation of the System.Drawing.FontFamily object.</param>
		/// <param name="size">The size of the new FontEx object.</param>
		/// <param name="fontStyle">The style of the new FontEx object.</param>
		public FontEx(string fontName, float size, FontStyle fontStyle)
		{
			this.fontName = fontName;
			this.size = size;
			this.fontStyle = fontStyle;

			hFont = CreateFont(fontName, size, fontStyle, angle*10);
		}

		#endregion

		#region public properties

		/// <summary>
		/// Gets the face name of this FontEx object.
		/// </summary>
		public string Name
		{
			get
			{
				return this.fontName;
			}
			set
			{
				if (fontName != value)
				{
					fontName = value; 
					if ( hFont != IntPtr.Zero )
						GDIPlus.DeleteObject(hFont);

					hFont = CreateFont(fontName, size, fontStyle, angle*10);
				}
			}
		}

		/// <summary>
		/// Gets the em-size of this FontEx object measured in the unit of this FontEx object.
		/// </summary>
		public float Size
		{
			get
			{
				return size;
			}
			set
			{
				if (size != value)
				{
					size = value; 
					if ( hFont != IntPtr.Zero )
						GDIPlus.DeleteObject(hFont);

					hFont = CreateFont(fontName, size, fontStyle, angle*10);
				}
			}
		}

		/// <summary>
		/// Gets style information for this FontEx object.
		/// </summary>
		public FontStyle Style
		{
			get
			{
				return fontStyle;
			}
			set
			{
				if (fontStyle != value)
				{
					fontStyle = value; 
					if ( hFont != IntPtr.Zero )
						GDIPlus.DeleteObject(hFont);

					hFont = CreateFont(fontName, size, fontStyle, angle*10);
				}
			}
		}

		/// <summary>
		/// Gets sets the angle for the FontEx.
		/// </summary>
		public int Angle
		{
			get 
			{ 
				return angle; 
			} 
			set 
			{ 
				if (angle != value)
				{
					angle = value; 
					if ( hFont != IntPtr.Zero )
						GDIPlus.DeleteObject(hFont);
//					if (angle == 90)
//					{
//						IntPtr tempFont = CreateFont(fontName, size, fontStyle, 0);
//						IntPtr hdc = GDIPlus.GetDC(IntPtr.Zero);
//						IntPtr hOldFont = GDIPlus.SelectObject(hdc, tempFont);
//						GDIPlus.TEXTMETRIC metric = new OpenNETCF.Drawing.GDIPlus.TEXTMETRIC();
//						GDIPlus.GetTextMetrics(hdc, ref metric);
//						GDIPlus.SelectObject(hdc, hOldFont);
//						GDIPlus.DeleteObject(tempFont);
//						hFont = CreateFont(fontName, size, fontStyle, angle*10, metric.tmAveCharWidth);
//					}
//					else
					 hFont = CreateFont(fontName, size, fontStyle, angle*10);

				}
			} 
		}


		/// <summary>
		/// Gets or sets a clear type for FontEx object.
		/// </summary>
		public bool ClearType
		{
			get 
			{ 
				return clearType; 
			} 
			set 
			{ 

				if (clearType != value)
				{
					clearType = value; 
					if ( hFont != IntPtr.Zero )
						GDIPlus.DeleteObject(hFont);

					hFont = CreateFont(fontName, size, fontStyle, angle*10);

				}
			} 
		}

		#endregion

		#region private helper methods

		internal IntPtr CreateFont(string Family, float Size, FontStyle Style, int Escapement)
		{

			GDIPlus.LOGFONT lf = new GDIPlus.LOGFONT();
			lf.lfCharSet = GDIPlus.DEFAULT_CHARSET;
			lf.lfClipPrecision = 0;
			lf.lfEscapement = Escapement;
			IntPtr hdc = GDIPlus.GetDC(IntPtr.Zero);
			int cyDevice_Res = GDIPlus.GetDeviceCaps(hdc, GDIPlus.LOGPIXELSY);
			GDIPlus.ReleaseDC(IntPtr.Zero, hdc);
			// Calculate font height.
			float flHeight = ((float)Size * (float)cyDevice_Res) / 72.0F;
			int iHeight = (int)(flHeight + 0.5);
			// Set height negative to request 'Em-Height' (versus
			// 'character-cell height' for positive size)
			iHeight = iHeight * (-1);

			lf.lfHeight = iHeight;
			lf.lfWidth = 0;
			
			lf.lfItalic = (Style & FontStyle.Italic) == FontStyle.Italic? (byte)1: (byte)0;
			lf.lfOrientation = Escapement;
			lf.lfOutPrecision = 0;
			//lf.lfPitchAndFamily = (byte)GDIPlus.FF_DONTCARE;
			if (clearType)
				lf.lfQuality = (byte)GDIPlus.CLEARTYPE_QUALITY;
			else
				lf.lfQuality = (byte)GDIPlus.DEFAULT_QUALITY;

			lf.lfStrikeOut = (Style & FontStyle.Strikeout) == FontStyle.Strikeout? (byte)1: (byte)0;
			lf.lfUnderline = (Style & FontStyle.Underline) == FontStyle.Underline? (byte)1: (byte)0;
			lf.lfWeight = (Style & FontStyle.Bold) == FontStyle.Bold? GDIPlus.FW_BOLD: GDIPlus.FW_NORMAL;
			
			IntPtr pLF = GDIPlus.LocalAlloc(0x40, 92);
			Marshal.StructureToPtr(lf, pLF, false);
			if ( Family.Length > 32 ) Family = Family.Substring(0, 32);
			Marshal.Copy(Family.ToCharArray(), 0, (IntPtr) ((int)pLF + 28), Family.Length);
			hFont = GDIPlus.CreateFontIndirect(pLF);
			Marshal.PtrToStructure(pLF, lf);
			GDIPlus.LocalFree(pLF);
			
			return hFont;
		}

		internal IntPtr CreateFont(string Family, float Size, FontStyle Style, int Escapement, int width)
		{

			GDIPlus.LOGFONT lf = new GDIPlus.LOGFONT();
			lf.lfCharSet = GDIPlus.DEFAULT_CHARSET;
			lf.lfClipPrecision = 0;
			lf.lfEscapement = Escapement;
			IntPtr hdc = GDIPlus.GetDC(IntPtr.Zero);
			int cyDevice_Res = GDIPlus.GetDeviceCaps(hdc, GDIPlus.LOGPIXELSY);
			GDIPlus.ReleaseDC(IntPtr.Zero, hdc);
			// Calculate font height.
			float flHeight = ((float)Size * (float)cyDevice_Res) / 72.0F;
			int iHeight = (int)(flHeight + 0.5);
			// Set height negative to request 'Em-Height' (versus
			// 'character-cell height' for positive size)
			iHeight = iHeight * (-1);

			lf.lfHeight = iHeight;
			lf.lfWidth = width;
			
			lf.lfItalic = (Style & FontStyle.Italic) == FontStyle.Italic? (byte)1: (byte)0;
			lf.lfOrientation = Escapement;
			lf.lfOutPrecision = 0;
			//lf.lfPitchAndFamily = (byte)GDIPlus.FF_DONTCARE;
			if (clearType)
				lf.lfQuality = (byte)GDIPlus.CLEARTYPE_QUALITY;
			else
				lf.lfQuality = (byte)GDIPlus.DEFAULT_QUALITY;

			lf.lfStrikeOut = (Style & FontStyle.Strikeout) == FontStyle.Strikeout? (byte)1: (byte)0;
			lf.lfUnderline = (Style & FontStyle.Underline) == FontStyle.Underline? (byte)1: (byte)0;
			lf.lfWeight = (Style & FontStyle.Bold) == FontStyle.Bold? GDIPlus.FW_BOLD: GDIPlus.FW_NORMAL;
			
			IntPtr pLF = GDIPlus.LocalAlloc(0x40, 92);
			Marshal.StructureToPtr(lf, pLF, false);
			if ( Family.Length > 32 ) Family = Family.Substring(0, 32);
			Marshal.Copy(Family.ToCharArray(), 0, (IntPtr) ((int)pLF + 28), Family.Length);
			hFont = GDIPlus.CreateFontIndirect(pLF);
			Marshal.PtrToStructure(pLF, lf);
			GDIPlus.LocalFree(pLF);
			
			return hFont;
		}

		internal IntPtr CreateFont(Font fromFont)
		{
			return CreateFont(fromFont.Name, fromFont.Size, fromFont.Style, 0);
		}

		#endregion
		
		/// <summary>
		/// Returns a handle to this FontEx object.
		/// </summary>
		/// <returns>A Windows handle to this FontEx object.</returns>
		public IntPtr ToHfont()
		{
			return hFont;
		}
	

		~FontEx()
		{
			Dispose();
		}


		#region IDisposable Members

		public void Dispose()
		{
//			foreach( FontFamilyItem item in FontFamilies)
//			{
//				foreach(FontVariation var in item.SizeList)
//				{
//					foreach( IntPtr hFont in var.Fonts )
//					{
						if ( hFont != IntPtr.Zero )
							GDIPlus.DeleteObject(hFont);
//					}
//				}
//			}

			GC.SuppressFinalize(this);

		}

		#endregion
	}
	
}
