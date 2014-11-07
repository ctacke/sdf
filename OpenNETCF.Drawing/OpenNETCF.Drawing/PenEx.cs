using System;
using System.Drawing;
using OpenNETCF.Drawing.Drawing2D;

namespace OpenNETCF.Drawing
{
	/// <summary>
	/// Defines an object used to draw lines and curves.
	/// </summary>
	public partial class PenEx : IDisposable
	{
		private Color color;
		//private PenStyle penStyle;
		private DashStyle penStyle;
		private int width;
		internal IntPtr hPen;

		#region constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="PenEx"/> class with the <see cref="Color"/>.  
		/// </summary>
		/// <param name="color">The <see cref="Color"/> of the <see cref="PenEx"/>.</param>
		public PenEx(Color color)
            : this(color, DashStyle.Solid)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PenEx"/> class with the Width.  
		/// </summary>
		/// <param name="color">The <see cref="Color"/> of the <see cref="PenEx"/>.</param>
		/// <param name="width">The <see cref="Width"/> of the <see cref="PenEx"/>.</param>
		public PenEx(Color color, int width)
		{
            if (width <= 0) throw new ArgumentOutOfRangeException("width");

			this.color = color;
			this.penStyle = DashStyle.Solid;
			this.width = width;
			hPen = GDIPlus.CreatePen((int)penStyle, width, ColorTranslator.ToWin32(color) /*GDIPlus.RGB(color)*/);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PenEx"/> class with the <see cref="DashStyle"/>.
		/// </summary>
		/// <param name="color">The <see cref="Color"/> of the <see cref="PenEx"/>.</param>
		/// <param name="style">The <see cref="DashStyle"/> of the <see cref="PenEx"/>.</param>
		public PenEx(Color color, DashStyle style)
		{
			this.color = color;
			this.penStyle = style;
			this.width = 1;
			hPen = GDIPlus.CreatePen((int)style, width, ColorTranslator.ToWin32(color)/*GDIPlus.RGB(color)*/);
		}

		#endregion

		#region public properties

		/// <summary>
		/// Gets or sets the color of this object.  
		/// </summary>
		public Color Color
		{
			get 
			{ 
				return color; 
			} 
			set 
			{ 
				color = value; 
			} 
		}

		/// <summary>
		/// Gets or sets the style used for dashed lines drawn with this <see cref="PenEx"/> object.
		/// </summary>
		public Drawing2D.DashStyle DashStyle
		{
			get
			{
				return penStyle;
			}
			set
			{
				penStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the Width of this object.
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
		
		#endregion

		~PenEx()
		{
			this.Dispose();
		}

		#region IDisposable Members

		public void Dispose()
		{
			GDIPlus.DeleteObject(hPen);
		}

		#endregion
	}

	
}
