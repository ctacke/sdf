
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.ComponentModel;

namespace OpenNETCF.Windows.Forms
{
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