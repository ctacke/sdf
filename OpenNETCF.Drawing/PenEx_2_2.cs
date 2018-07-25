using System;
using System.Drawing;
using OpenNETCF.Drawing.Drawing2D;

namespace OpenNETCF.Drawing
{
	public partial class PenEx
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="PenEx"/> class with the <see cref="Color"/>.  
		/// </summary>
		/// <param name="color">The <see cref="Color"/> of the <see cref="PenEx"/>.</param>
		/// <param name="width">The <see cref="Width"/> of the <see cref="PenEx"/>.</param>
		/// <param name="style">The <see cref="DashStyle"/> of the <see cref="PenEx"/>.</param>
		public PenEx(Color color, DashStyle style, int width)
		{
			this.color = color;
			this.penStyle = style;
			this.width = width;
			hPen = GDIPlus.CreatePen((int)style, width, ColorTranslator.ToWin32(color)/*GDIPlus.RGB(color)*/);
		}

		#endregion


	}

	
}
