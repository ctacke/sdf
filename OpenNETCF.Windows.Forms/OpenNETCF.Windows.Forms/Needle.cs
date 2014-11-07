using System;
using System.Drawing;

namespace OpenNETCF.Windows.Forms
{
	internal class Needle
	{
		private double		angle			= 0;
		private double		backangle		= 0;
		private int			targetDiam;
		private int			needleLength;
		private int			needleWidth		= 5;
		private Color		color;
		private int			offset;
		private int			woffset;
		private Point[]		points			= new Point[4];

		public Needle(int TargetDiameter, float StartAngle, int Length)
		{
			targetDiam = TargetDiameter;
			Angle = StartAngle;
			color = Color.Black;
			
			needleLength = Length;

			offset = (targetDiam / 2) - needleLength;
			woffset = (targetDiam / 2) - needleWidth;
		}

		public int Length
		{
			get
			{
				return needleLength;
			}
			set
			{
				needleLength = value;
				offset = (targetDiam / 2) - needleLength;
			}
		}

		public int Width
		{
			get
			{
				return needleWidth;
			}
			set 
			{
				// "needleWidth" is misleading, it's actually offset from center
				// to make it user intuitive, we'll just half it here
				needleWidth = (value / 2);
				woffset = (targetDiam / 2) - needleWidth;
			}
		}

		public Color NeedleColor
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

		public void Draw(Graphics TargetGraphics)
		{
			SolidBrush brush = new SolidBrush(color);
			double theta = angle;
			
			int tailLength = (int)(needleLength * 0.1);

			points[0].X = (int)((needleLength) * Math.Sin(theta));
			points[0].Y = (int)((needleLength) * Math.Cos(theta));
			points[0].X += (needleLength + offset);
			points[0].Y = needleLength - points[0].Y + offset;

			for(int a = 1 ; a <= points.GetUpperBound(0) ; a++)
			{
				theta += (Math.PI / 2);

				points[a].X = (int)((needleWidth) * Math.Sin(theta));
				points[a].Y = (int)((needleWidth) * Math.Cos(theta));
				points[a].X += (needleWidth + woffset);
				points[a].Y = needleWidth - points[a].Y + woffset;
			}
			
			TargetGraphics.FillPolygon(brush, points);
			brush.Dispose();
		}

		public double Angle
		{
			set
			{
				// store angle in radians so we don't compute it with every draw
				angle = value * (Math.PI / 180F);
				backangle = (value > 180) ? value - 180 : value + 180;
				backangle *= (Math.PI / 180F);
				
			}
		}
	}
}
