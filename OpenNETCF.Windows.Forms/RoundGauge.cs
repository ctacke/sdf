using System;
using System.Windows.Forms;
using System.Drawing;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Used when a gauge value exceeds its high warn level.
	/// </summary>
	public delegate void HighWarnHandler(object sender, bool EnteredZone);
	/// <summary>
	/// Used when a gauge value falls below its low warn level.
	/// </summary>
	public delegate void LowWarnHandler(object sender, bool EnteredZone);
	/// <summary>
	/// Used when a gauge value exceeds its max value.
	/// </summary>
	public delegate void MaxValueHandler(object sender);
	/// <summary>
	/// Used when a gauge value falls below its low value.
	/// </summary>
	public delegate void MinValueHandler(object sender);

	/// <summary>
	/// A representation of a dial gauge with indicator lamps
	/// </summary>
    public class RoundGauge : UserControl, IWin32Window
	{
		/// <summary>
		/// Fires when the RoundGauge Value exceeds the HighWarnValue
		/// <seealso cref="Value"/>
		/// <seealso cref="HighWarnValue"/>
		/// </summary>
		public event HighWarnHandler	HighWarn;
		/// <summary>
		/// Fires when the RoundGauge Value falls below the LowWarnValue
		/// <seealso cref="Value"/>
		/// <seealso cref="LowWarnValue"/>
		/// </summary>
		public event LowWarnHandler		LowWarn;
		/// <summary>
		/// Fires when the RoundGauge Value exceeds the MaxValue
		/// <seealso cref="Value"/>
		/// <seealso cref="MaxValue"/>
		/// </summary>
		public event MaxValueHandler	GaugeMax;
		/// <summary>
		/// Fires when the RoundGauge Value falls below the MinValue
		/// <seealso cref="Value"/>
		/// <seealso cref="MinValue"/>
		/// </summary>
		public event MinValueHandler	GaugeMin;
	
		private Color		bezelColor			= Color.SlateGray;
		private Color		lineColor			= Color.Black;
		private Color		highwarnColor		= Color.Red;
		private Color		lowwarnColor		= Color.Red;
		private Bitmap		bufferBitmap;
		private Font		valueFont;
		private RectangleF	valueRect;
		private RectangleF	valueBorder;
		private int			arcBottom			= 0;
		private int			valueDigits			= -1;
		private int			valueFontSize		= 8;
		private int			bezelWidth			= 5;
		private int			valueArc;
		private float		startAngle;
		private float		valueAngle;
		private int			minValue			= 0;
		private int			highwarnValue		= 80;
		private int			lowwarnValue		= 0;
		private int			maxValue			= 100;
		private int			lineBezelSpacing	= 10;
		private int			arcWidth			= 3;
		private int			tickInterval		= 10;
		private int			tickLength			= 10;
		private bool		updating;
		private Rectangle	face;
		private bool		showTickLabels		= true;
		private int			tickLabelInterval	= 1;
		private float		currentValue		= 0F;
		private int			tickLabelPadding	= 10;
		private double		unitsPerDegree		= 0F;
		private int			diameter			= 100;
		private Needle		needle;
		private short		needleWidth			= 5;
		private Color		needleColor			= Color.Black;
		private Lamps		lamps				= new Lamps();
		private int			lampdiameter;
		private bool		inLowRange	= false;
		private bool		inHighRange	= false;

		/// <summary>
		/// Create a RoundGauge object
		/// </summary>
		public RoundGauge()
		{			
			// set defaults
			updating = true;

			BackColor = Color.Cornsilk;
			ValueArc = 300;
			currentValue = 0;
			
			valueFont = new Font(FontFamily.GenericMonospace, valueFontSize, FontStyle.Regular);
			face = new Rectangle(bezelWidth, bezelWidth, diameter - (2 * bezelWidth), diameter - (2 * bezelWidth));
			bufferBitmap = new Bitmap(diameter, diameter);	
			needle = new Needle(this.Width, valueAngle, (Width / 2 - bezelWidth - lineBezelSpacing));
			needle.Width = needleWidth;
			needle.NeedleColor = needleColor;

			UpdateNeedleLength();
			unitsPerDegree = (double)ValueArc /  (double)(maxValue - minValue);

            RecalculateFace();

            Lamps.LampsChanged += new ChangeHandler(lamps_LampsChanged);
		}

		#region ----- Hidden Properties -----
		#if DESIGN
		[Browsable(false),
		EditorBrowsable(EditorBrowsableState.Never),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Width { get{return base.Width;} set{base.Width = value;} }

		[Browsable(false),
		EditorBrowsable(EditorBrowsableState.Never),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Height { get{return base.Height;} set{base.Height = value;} }

		[Browsable(false),
		EditorBrowsable(EditorBrowsableState.Never),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Text { get{return base.Text;} set{base.Text = value;} }
		#endif
		#endregion

		#region Lamps Property
		/// <summary>
		/// The RoundGauge's collection of Lamps
		/// <seealso cref="Lamps"/>
		/// <seealso cref="Lamp"/>
		/// </summary>
		#if DESIGN
		[
		EditorAttribute(typeof(System.ComponentModel.Design.CollectionEditor), typeof(System.Drawing.Design.UITypeEditor)),
		Category("Appearance"),
		Description("A collection of indicator lamps."),
		DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)
		]
		#endif
		public Lamps Lamps
		{
			get
			{
				return lamps;
			}
			set
			{
				lamps = value;
			}
		}
		#endregion

		#region Needle Properties
		/// <summary>
		/// Specifies the width of the gauge needle base.
		/// </summary>
		public short NeedleWidth
		{
			get
			{
				return needleWidth;
			}
			set
			{
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("NeedleWidth must be greater than zero");

				needleWidth = value;
				needle.Width = needleWidth;

				if(!updating)
					this.Refresh();
			}
		}

        /// <summary>
        /// The color of the Guage's needle
        /// </summary>
		public Color NeedleColor
		{
			get
			{
				return needleColor;
			}
			set
			{
				needleColor = value;
				needle.NeedleColor = needleColor;

				if(!updating)
					this.Refresh();
			}
		}
		#endregion

		#region Drawing/Painting Methods
		
		/// <summary>
		/// Refreshes the control
		/// </summary>
		public override void Refresh()
		{
			Graphics g = Graphics.FromImage(bufferBitmap);
			if(!(this.Parent == null))
			{
				SolidBrush backColorBrush = new SolidBrush(this.Parent.BackColor);
				g.FillRectangle(backColorBrush, 0, 0, this.Width, this.Height);
				backColorBrush.Dispose();
			}

			DetermineValueRect(out valueRect, out valueBorder);
			
			this.Invalidate();
			
			updating = false;
			
			g.Dispose();

			base.Refresh();
		}

		/// <summary>
		/// Suspends control painting
		/// </summary>
		public void BeginUpdate()
		{
			updating = true;
		}

		/// <summary>
		/// Resumes control painting
		/// </summary>
		public void EndUpdate()
		{
			updating = false;
			this.Refresh();
		}

		private void DrawBezel(Graphics g)
		{
			SolidBrush bezbrush = new SolidBrush(bezelColor);
			SolidBrush brush = new SolidBrush(this.BackColor);
			
			g.FillEllipse(bezbrush, 0, 0, diameter, diameter);
			g.FillEllipse(brush, face);

			brush.Dispose();
		}

		private void UpdateNeedleLength()
		{
			needle.Length = (this.Width / 2) - lineBezelSpacing - tickLength;
		}

		private Point[] CreateArc(float StartAngle, float SweepAngle, int PointsInArc, int Radius, int xOffset, int yOffset, int LineWidth)
		{
			if(PointsInArc < 0)
				PointsInArc = 0;

			if(PointsInArc > 360)
				PointsInArc = 360;

			Point[] points = new Point[PointsInArc * 2];
			int xo;
			int yo;
			int xi;
			int yi;
			float degs;
			double rads;
			
			for(int p = 0 ; p < PointsInArc ; p++)
			{
				degs = StartAngle + ((SweepAngle / PointsInArc) * p);
				
				rads = (degs * (Math.PI / 180));

				xo = (int)(Radius * Math.Sin(rads));
				yo = (int)(Radius * Math.Cos(rads));
				xi = (int)((Radius - LineWidth) * Math.Sin(rads));
				yi = (int)((Radius - LineWidth) * Math.Cos(rads));

				xo += (Radius + xOffset);
				yo = Radius - yo + yOffset;
				xi += (Radius + xOffset);
				yi = Radius - yi + yOffset;

				points[p] = new Point(xo, yo);
				points[(PointsInArc * 2) - (p + 1)] = new Point(xi, yi);
			}

			return points;
		}

		private void DetermineValueRect(out RectangleF rect, out RectangleF border)
		{
			int w = 0;
			float x = 0;
			float y = 0;
			SizeF size;
			float offset;
			Graphics g = Graphics.FromImage(bufferBitmap);

			// we pad with 1/2 digit space on left and right
			w = (valueDigits < 0) ? currentValue.ToString().Length + 1 : valueDigits + 1;
			
			offset = g.MeasureString("0", valueFont).Width;
			size = g.MeasureString(new string('0', w), valueFont);

			// center horizontally
			x = (this.Width - size.Width) / 2;

			// vertically at arc bottom
			y = arcBottom - size.Height;

			border = new RectangleF(x, y, size.Width, size.Height);
			rect = new RectangleF(x + (offset * 0.5F), y, size.Width, size.Height);

			g.Dispose();
		}

		private void DrawValue(Graphics g)
		{
			// only do something if we are showing digits
			if(valueDigits == 0) return;

			string val = currentValue.ToString();
			
			if(valueDigits > 0)
			{
				val = val.PadLeft(valueDigits, '0');
			}			

			g.DrawRectangle(new Pen(bezelColor), Rectangle.Ceiling(valueBorder));
			g.DrawString(val, valueFont, new SolidBrush(lineColor), valueRect);
		}
		
		private void DrawLines(Graphics g)
		{
			Point[] points;
			float arcstart = 0F;
			float arcsweep = 0F;
			int xo = 0;
			int yo = 0;
			int xi = 0;
			int yi = 0;
			int xl = 0;
			int yl = 0;
			int tickvalue = 0;
			int tickcount = 0;
			bool drawlabel = true;
			SizeF labelSize;
			double rads;
			int r = (face.Width / 2) - lineBezelSpacing;
			int offset = bezelWidth + lineBezelSpacing;
			Pen pen;
			Brush brush;

			float arcRange = (float)maxValue - (float)minValue;
			
			arcstart = startAngle;
			arcBottom = 0;

			// draw redline arc
			if(lowwarnValue > minValue)
			{
				pen = new Pen(lowwarnColor);
				brush = new SolidBrush(lowwarnColor);

				arcsweep = Convert.ToInt32((float)valueArc * (((float)lowwarnValue - (float)minValue) / arcRange));

				points = CreateArc(arcstart, (int)arcsweep, (int)arcsweep, r, offset, offset, arcWidth);				

				// get lowest point of arc (for value alignment later)
				arcBottom = (points[0].Y > arcBottom) ? points[0].Y : arcBottom;
				
				g.FillPolygon(brush, points);
				g.DrawPolygon(pen, points);

				arcstart = startAngle + (int)arcsweep;
			}

			// draw main arc
			pen = new Pen(lineColor);
			brush = new SolidBrush(lineColor);

			arcsweep = Convert.ToInt32((float)valueArc * ((float)(highwarnValue - lowwarnValue) / arcRange));

			points = CreateArc(arcstart, (int)arcsweep, (int)arcsweep, r, offset, offset, arcWidth);

			if(points.Length == 0)
				return;

			// get lowest point of arc (for value alignment later)
			arcBottom = (points[0].Y > arcBottom) ? points[0].Y : arcBottom;

			g.FillPolygon(brush, points);
			g.DrawPolygon(pen, points);

			arcstart += (int)arcsweep;

			// draw redline arc
			if(highwarnValue < maxValue)
			{
				pen = new Pen(highwarnColor);
				brush = new SolidBrush(highwarnColor);

				arcsweep = Convert.ToInt32((float)valueArc * ((float)(maxValue - highwarnValue) / arcRange));

				points = CreateArc(arcstart, (int)arcsweep, (int)arcsweep, r, offset, offset, arcWidth);

				g.FillPolygon(brush, points);
				g.DrawPolygon(pen, points);
			}

			// draw tics and labels
			pen = new Pen(lineColor);
			brush = new SolidBrush(lineColor);

			float tickIntervalDegs = ((float)valueArc / ((float)maxValue - (float)minValue)) * (float)tickInterval;
			
			if(tickIntervalDegs < 1)
			{
				throw new Exception("Cannot have a TickInterval < 1 degree");
			}

			tickvalue = minValue;
			for(int t = (int)startAngle ; t <= (startAngle + valueArc) ; t += (int)tickIntervalDegs)
			{
				labelSize = g.MeasureString(tickvalue.ToString(), this.Font);
				rads = (t * (Math.PI / 180));

				drawlabel = (showTickLabels && ((tickcount % tickLabelInterval) == 0));

				xo = (int)(r * Math.Sin(rads));
				yo = (int)(r * Math.Cos(rads));
				xo += (r + offset);
				yo = r - yo + offset;
				
				if(drawlabel)
				{
					xi = (int)((r - (tickLength * 1.5)) * Math.Sin(rads));
					yi = (int)((r - (tickLength * 1.5)) * Math.Cos(rads));
					xi += (r + offset);
					yi = r - yi + offset;
				}
				else
				{
					xi = (int)((r - tickLength) * Math.Sin(rads));
					yi = (int)((r - tickLength) * Math.Cos(rads));
					xi += (r + offset);
					yi = r - yi + offset;
				}

				g.DrawLine(pen, xo, yo, xi, yi);
				
				if(drawlabel)
				{
					xl = (int)((r - ((tickLength * 2) + tickLabelPadding)) * Math.Sin(rads));
					yl = (int)((r - ((tickLength * 2) + tickLabelPadding)) * Math.Cos(rads));
					xl += (r + offset - (int)(labelSize.Width / 2));
					yl = r - yl + offset - (int)(labelSize.Height / 2);

				    g.DrawString(tickvalue.ToString(), this.Font, brush, xl, yl);
				}

				tickcount++;
				tickvalue += tickInterval;
			}

			// clean up
			pen.Dispose();
			brush.Dispose();	
		}

		/// <summary>
		/// OnPaintBackground
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

        protected override void OnParentChanged(EventArgs e)
        {
            Refresh();
            base.OnParentChanged(e);
        }

		/// <summary>
		/// OnPaint
		/// </summary>
		/// <param name="e">PaintEventArgs</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// double buffer the output
			Bitmap b = new Bitmap(bufferBitmap);
			Graphics g = Graphics.FromImage(b);
			
			DrawBezel(g);
			DrawLines(g);

			DrawValue(g);
			needle.Angle = valueAngle;
			needle.Draw(g);
			
			foreach(Lamp l in lamps)
			{
				if(l.Visible)
				{
					l.Draw(g);
				}
			}

			// draw the gauge
			e.Graphics.DrawImage(b, 0, 0);

			// clean up
			b.Dispose();
			g.Dispose();
		}
		#endregion

		#region Diameter
		/// <summary>
		/// Dimaeter of the RoundGauge face
		/// </summary>
		public int Diameter
		{
			get
			{
				return diameter;
			}
			set
			{
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Diameter must be greater than zero");
                
                diameter = value;
				Width = diameter;
				Height = diameter;
				if(!updating)
					this.Refresh();
			}
		}
		#endregion
		#region Value
		/// <summary>
		/// Value that the RoundGauge needle points to
		/// </summary>
		public float Value
		{
			get
			{
				return currentValue;
			}
			set
			{
                if(value < MinValue)
                    currentValue = minValue;
                else if(value > MaxValue)
                    currentValue = maxValue;
                else
				    currentValue = value;

				valueAngle = startAngle + (int)((currentValue - minValue) * unitsPerDegree);

				if(valueDigits == -1)
				{
					DetermineValueRect(out valueRect, out valueBorder);
				}

				#region event handling
				if((GaugeMax != null) && (currentValue >= MaxValue))
				{
					GaugeMax(this);
				}

				if((GaugeMin != null) && (currentValue <= MinValue))
				{
					GaugeMin(this);
				}

				if((LowWarn != null) && (LowWarnValue > MinValue))
				{
					if(currentValue < LowWarnValue)
					{
						if(! inLowRange)
						{
							// just passed into low
							LowWarn(this, true);
							inLowRange = true;
						}
					}
					else
					{
						if(inLowRange)
						{
							// just passed out of low
							LowWarn(this, false);
							inLowRange = false;
						}
					}
				}

				if((HighWarn != null) && (HighWarnValue > MinValue))
				{
					if(currentValue > HighWarnValue)
					{
						if(! inHighRange)
						{
							// just passed into High
							HighWarn(this, true);
							inHighRange = true;
						}
					}
					else
					{
						if(inHighRange)
						{
							// just passed out of High
							HighWarn(this, false);
							inHighRange = false;
						}
					}
				}
				#endregion Event Handling

				base.Invalidate();
			}
		}
		#endregion
		#region ValueArc
		/// <summary>
		/// The arc over which value ticks are drawn (in degrees)
		/// </summary>
		public int ValueArc
		{
			get 
			{
				return valueArc;
			}
			set 
			{
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("ValueArc must be greater than zero");
                if (value >= 360)
                    value = 360;

				startAngle = 180 + ((360 - value) / 2);
				valueAngle = startAngle;
				valueArc = value;
				unitsPerDegree = (double)ValueArc /  (double)(maxValue - minValue);

				if(!updating)
					this.Refresh();
			}
		}
		#endregion
		#region TickLabelInterval
		/// <summary>
		/// Specifies the number of ticks between TickLabels.
		/// </summary>
		public int TickLabelInterval
		{
			get
			{
				return tickLabelInterval;
			}
			set
			{
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("TickLabelInterval must be greater than zero");
                
                tickLabelInterval = value;

				if(!updating)
					this.Refresh();
			}

		}
		#endregion
		#region ShowTickLabels
		/// <summary>
		/// Specifies whether or not TickLabels will be displayed.
		/// </summary>
		#if DESIGN
		[
		System.ComponentModel.Category("Ticks"),
		System.ComponentModel.Description("Specifies whether or not TickLabels will be displayed.")
		]
		#endif
		public bool ShowTickLabels
		{
			get 
			{
				return showTickLabels;
			}
			set 
			{
				showTickLabels = value;

				if(!updating)
					this.Refresh();
			}

		}
		#endregion
		#region LineBezelSpacing
		/// <summary>
		/// Specifies the gap between the ValueArc and the gauge bezel.
		/// </summary>
		#if DESIGN
		[
		System.ComponentModel.Category("Appearance"),
		System.ComponentModel.Description("Specifies the gap between the ValueArc and the gauge bezel.")
		]
		#endif
		public int LineBezelSpacing
		{
			get 
			{
				return lineBezelSpacing;
			}
			set 
			{
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("LineBezelSpacing must be greater than zero");
                
                lineBezelSpacing = value;

				if(!updating)
					this.Refresh();
			}
		}
		#endregion
		#region ArcWidth
		/// <summary>
		/// Specifies the thickness of the ValueArc line.
		/// </summary>
		#if DESIGN
		[
		System.ComponentModel.Category("Appearance"),
		System.ComponentModel.Description("Specifies the thickness of the ValueArc line.")
		]
		#endif
		public int ArcWidth
		{
			get 
			{
				return arcWidth;
			}
			set 
			{
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("ArcWidth must be greater than zero");
                
                arcWidth = value;

				if(!updating)
					this.Refresh();
			}
		}
		#endregion
		#region TickInterval
		/// <summary>
		/// Specifies the interval, based on Value, between individual Tick lines.
		/// </summary>
		#if DESIGN
		[
		System.ComponentModel.Category("Ticks"),
		System.ComponentModel.Description("Specifies the interval, based on Value, between individual Tick lines.")
		]
		#endif
		public int TickInterval
		{
			get 
			{
				return tickInterval;
			}
			set 
			{
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("TickInterval must be greater than zero");
                
                tickInterval = value;

				if(!updating)
					this.Refresh();
			}
		}
		#endregion
		#region TickLabelPadding
		/// <summary>
		/// Specifies the gap between the Tick lines and the TickLabels.
		/// </summary>
		#if DESIGN
		[
		System.ComponentModel.Category("Ticks"),
		System.ComponentModel.Description("Specifies the gap between the Tick lines and the TickLabels.")
		]
		#endif
		public int TickLabelPadding
		{
			get 
			{
				return tickLabelPadding;
			}
			set 
			{
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("TickLabelPadding must be greater than zero");
                
                tickLabelPadding = value;

				if(!updating)
					this.Refresh();
			}
		}
		#endregion
		#region TickLength
		/// <summary>
		/// Specifies the length of standard Tick lines.  Tick lines with a label are twice this value.
		/// </summary>
		#if DESIGN
		[
		System.ComponentModel.Category("Ticks"),
		System.ComponentModel.Description("Specifies the length of standard Tick lines.  Tick lines with a label are twice this value.")
		]
		#endif
		public int TickLength
		{
			get 
			{
				return tickLength;
			}
			set 
			{
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("TickLength must be greater than zero");
                
                tickLength = value;

				if(!updating)
					this.Refresh();
			}
		}
		#endregion
		#region MinValue
		/// <summary>
		/// Specifies the minimum value displayed on the gauge.
		/// </summary>
		public int MinValue
		{
			get 
			{
				return minValue;
			}
			set 
			{
                if (value > MaxValue)
                    throw new ArgumentOutOfRangeException("MinValue must be less than than Max Value");
                
                minValue = value;

				if(lowwarnValue < minValue)
				{
					lowwarnValue = minValue;
				}
				
				unitsPerDegree = (double)ValueArc /  (double)(maxValue - minValue);

				if(!updating)
					this.Refresh();
			}
		}
		#endregion
		#region HighWarnValue
		/// <summary>
		/// Specifies the lower limit of the HighWarn region.
		/// </summary>
		public int HighWarnValue
		{
			get 
			{
				return highwarnValue;
			}
			set 
			{
				if(value > maxValue)
					highwarnValue = maxValue;
                else if (value < lowwarnValue)
                    throw new ArgumentOutOfRangeException("HighWarnValue must be greater than the LowWarnValue");
                else
				    highwarnValue = value;

				if(!updating)
					this.Refresh();
			}
		}
		
		#endregion
		#region LowWarnValue
		/// <summary>
		/// Specifies the upper limit of the LowWarn region.
		/// </summary>
		public int LowWarnValue
		{
			get
			{
				return lowwarnValue;
			}
			set 
			{
				if(lowwarnValue < minValue)
				{
					lowwarnValue = minValue;
				}

				lowwarnValue = value;

				if(!updating)
					this.Refresh();
			}

		}

		#endregion
		#region MaxValue
		/// <summary>
		/// Specifies the maximum value displayed on the gauge.
		/// </summary>
		public int MaxValue
		{
			get 
			{
				return maxValue;
			}
			set 
			{
                if (value <= minValue)
                    throw new ArgumentOutOfRangeException("MaxValue must be greater than MinValue");

				maxValue = value;
				
				if(highwarnValue > maxValue)
				{
					highwarnValue = maxValue;
				}
				unitsPerDegree = (double)ValueArc /  (double)(maxValue - minValue);

				if(!updating)
					this.Refresh();
			}
		}
		#endregion
		#region ValueDigits
		/// <summary>
		/// Specifies the number of digits to display in the value label.  0 will hide the value label.  -1 sets it to autosize.
		/// </summary>
		public int ValueDigits
		{
			get
			{
				return valueDigits;
			}
			set
			{
                if (value <= -2)
                    throw new ArgumentOutOfRangeException("ValueDigits must be -1 or greater");
                
                valueDigits = value;
				DetermineValueRect(out valueRect, out valueBorder);

				if(!updating)
					this.Refresh();
			}
		}
		#endregion
		#region BezelWidth
		/// <summary>
		/// Specifies the thickness of the gauge bezel.
		/// </summary>
		public int BezelWidth
		{
			get 
			{
				return bezelWidth;
			}
			set 
			{
                if (value < 0)
                    bezelWidth = 0;
                else
                    bezelWidth = value;

				face = new Rectangle(bezelWidth, bezelWidth, this.Width - (2 * bezelWidth), this.Height - (2 * bezelWidth));

				if(!updating)
					this.Refresh();
			}
		}
		#endregion
		#region BezelColor
		/// <summary>
		/// Specifies the Color of the gauge bezel.
		/// </summary>
		public Color BezelColor
		{
			get 
			{
				return bezelColor;
			}
			set 
			{
				bezelColor = value;

				if(!updating)
					this.Refresh();
			}
		}
		#endregion
		#region LineColor
		/// <summary>
		/// Specifies the Color of Ticks and the ValueArc between the LowWarn and HighWarn regions.
		/// </summary>
		public Color LineColor
		{
			get 
			{
				return lineColor;
			}
			set 
			{
				lineColor = value;

				if(!updating)
					this.Refresh();
			}
		}
		#endregion
		#region LowWarnColor
		/// <summary>
		/// Specifies the Color of the ValueArc in the LowWarn region.
		/// </summary>
		public Color LowWarnColor
		{
			get 
			{
				return lowwarnColor;
			}
			set 
			{
				lowwarnColor = value;

				if(!updating)
					this.Refresh();
			}
		}
		#endregion
		#region HighWarnColor
		/// <summary>
		/// Specifies the Color of the ValueArc in the HighWarn region.
		/// </summary>
		public Color HighWarnColor
		{
			get 
			{
				return highwarnColor;
			}
			set 
			{
				highwarnColor = value;

				if(!updating)
					this.Refresh();
			}
		}
		#endregion

        private void RecalculateFace()
        {
            diameter = Width = Height;

            face = new Rectangle(bezelWidth, bezelWidth, diameter - (2 * bezelWidth), diameter - (2 * bezelWidth));
            bufferBitmap = new Bitmap(diameter, diameter);
            DetermineValueRect(out valueRect, out valueBorder);
            needle = new Needle(this.Width, valueAngle, (Width / 2 - bezelWidth - lineBezelSpacing));
            needle.Width = needleWidth;

            // update lamp data
            int lampx = 0, lampy = 0;
            lampdiameter = (int)((this.Width / 2) - ((this.Width / 2) / 1.4142135623730950488016887242097));

            foreach (Lamp l in lamps)
            {
                switch (l.Position)
                {
                    case LampPosition.UpperLeft:
                        lampx = lampdiameter + 1;
                        lampy = lampdiameter + 1;
                        break;
                    case LampPosition.UpperRight:
                        lampx = this.Width - 1;
                        lampy = lampdiameter + 1;
                        break;
                    case LampPosition.LowerLeft:
                        lampx = lampdiameter + 1;
                        lampy = this.Height - 1;
                        break;
                    case LampPosition.LowerRight:
                        lampx = this.Width - 1;
                        lampy = this.Height - 1;
                        break;
                }
                l.UpperRight = new Point(lampx, lampy);
                l.Diameter = lampdiameter;
            }
        }

		/// <summary>
		/// Fired when the RoundGauge is resized
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
            RecalculateFace();

			base.OnResize (e);

			if(!updating)
				this.Refresh();
		}

		/// <summary>
		/// Dispose of the current object instance
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
        }

		private void lamps_LampsChanged()
		{
			Refresh();
		}

	}
}
