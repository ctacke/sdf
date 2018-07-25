
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.ComponentModel;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Captures a signature from the user.
	/// Can be saved to a control specific byte array or a Bitmap.
	/// </summary>
	public partial class Signature : Control
	{
		/// <summary>
		/// Gets or sets the width of the signature draw pen
		/// </summary>
		public float PenWidth
		{
			get
			{
				return linePen.Width;
			}
			set
			{
				Color c = linePen.Color;
				System.Drawing.Drawing2D.DashStyle d = linePen.DashStyle;
				linePen.Dispose();
				linePen = new Pen(c);
				linePen.Width = penWidth = value;
				linePen.DashStyle = d;
			}
		}
	}

	/// <summary>
	/// Internal class to facilitate pen width control
	/// </summary>
	internal struct Point2
	{
		int _x;
		int _y;
		float _width;

		public Point2(int x, int y)
		{
			_x = x;
			_y = y;
			_width = 1;
		}

		public Point2(int x, int y, float w)
		{
			_x = x;
			_y = y;
			_width = w;
		}

		public int X
		{
			get { return _x; }
		}

		public int Y
		{
			get { return _y; }
		}

		public float Width
		{
			get { return _width; }
		}
	}
}