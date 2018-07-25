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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OpenNETCF.Windows.Forms
{
    /// <summary>
    /// Represents a progress bar control similar to the windows progress bar control.
    /// </summary>
    public partial class ProgressBar2 : UserControl, IWin32Window
    {
        /// <summary>
        /// The default size of the control
        /// </summary>
        private readonly Size defaultSize = new Size(190, 26);
        /// <summary>
        /// The orientation of the control
        /// </summary>
        private Orientation orientation = Orientation.Vertical;
        /// <summary>
        /// The Gradient mode that should be used
        /// </summary>
        private GradientStyle gradientStyle = GradientStyle.Normal;
        /// <summary>
        /// The draw style for the progress bar
        /// </summary>
        private DrawStyle drawStyle = DrawStyle.Gradient;
        /// <summary>
        /// Border style of the control
        /// </summary>
        private BorderStyle borderStyle = BorderStyle.FixedSingle;
        /// <summary>
        /// The maximum value of the progress bar
        /// </summary>
        private int maximum = 10;
        /// <summary>
        /// The minimum value of the progress bar
        /// </summary>
        private int minimum = 0;
        /// <summary>
        /// The current value of the progress bar
        /// </summary>
        private int currentValue = 0;
        /// <summary>
        /// Step to use when incrementing the value
        /// </summary>
        private int step;
        /// <summary>
        /// The color of the bar
        /// </summary>
        private Color barColor = System.Drawing.SystemColors.Highlight; //System.Drawing.Color.FromArgb(System.Drawing.SystemColors.Highlight.ToArgb());//System.Drawing.Color.FromArgb(-13538619);//System.Drawing.SystemColors.Highlight;
        /// <summary>
        /// The gradient color to use
        /// </summary>
        private Color barGradientColor = Color.White;
        /// <summary>
        /// The border color of the control
        /// </summary>
        private Color borderColor = Color.Black;
        /// <summary>
        /// Whether to show the text or not
        /// </summary>
        /// <remarks>Shows the text in the control.</remarks>
        private bool showValueText = true;
        /// <summary>
        /// Whether to show the percentage or not
        /// </summary>
        private bool showPercentValueText = false;
        /// <summary>
        /// The on pixel bitmap that is cached with the gradient so it doesn't have to be regenerated all the time.  The image will be streched depending on the value.
        /// </summary>
        private Bitmap onePixBar = null;
        /// <summary>
        /// The padding from the border of the control to the progress bar
        /// </summary>
        private Point borderPadding = new Point(2, 2);
        /// <summary>
        /// The offscreen bitmap to draw on
        /// </summary>
        private Bitmap offscreenBM = null;
        /// <summary>
        /// The offscreen graphics component
        /// </summary>
        private Graphics offscreenGX = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProgressBar2():base()
        {
            InitializeComponent();
        }

        #region Public Properties
        /// <summary>
        /// Gets or sets whether the current percentage of the progress bar should be shown.
        /// </summary>
        public bool ShowPercentValueText
        {
            get
            {
                return this.showPercentValueText;
            }
            set
            {
                this.showPercentValueText = value;
                if (this.showPercentValueText)
                    this.showValueText = false;
                this.Recalculate(false);
            }
        }
        /// <summary>
        /// Gets or sets whether the current value of the progress bar should be shown.  
        /// </summary>
        public bool ShowValueText
        {
            get
            {
                return this.showValueText;
            }
            set
            {
                this.showValueText = value;
                if (this.showValueText)
                    this.showPercentValueText = false;
                this.Recalculate(false);
            }
        }
        /// <summary>
        /// Gets or sets the gradient color of the progress bar
        /// </summary>
        /// <remarks>Only effective when DrawStyle is set to Gradient</remarks>
        public Color BarGradientColor
        {
            get
            {
                return this.barGradientColor;
            }
            set
            {
                this.barGradientColor = value;
                this.Recalculate(true);
            }
        }
        /// <summary>
        /// Gets or sets the color of the progress bar
        /// </summary>
        /// <value>The <see cref="System.Drawing.Color">Color</see> to set the bar of the ProgressBar2.</value>
        public Color BarColor
        {
            get
            {
                return this.barColor;
            }
            set
            {
                this.barColor = value;
                this.Recalculate(true);
            }
        }
        /// <summary>
        /// Gets or sets the style of the progress bar.
        /// </summary>
        public DrawStyle DrawStyle
        {
            get
            {
                return this.drawStyle;
            }
            set
            {
                this.drawStyle = value;
                this.Recalculate(true);
            }
        }
        /// <summary>
        /// Gets or sets the gradient draw mode. Two values are either Normal which is gradient from left to right
        /// and Middle which starts from the middle out
        /// </summary>
        public GradientStyle GradientStyle
        {
            get
            {
                return this.gradientStyle;
            }
            set
            {
                this.gradientStyle = value;
                this.Recalculate(true);
            }
        }
        /// <summary>
        /// Gets or sets the border style of the control
        /// </summary>
        public new BorderStyle BorderStyle
        {
            get
            {
                return this.borderStyle;
            }
            set
            {
                if (value == System.Windows.Forms.BorderStyle.Fixed3D)
                    throw new NotSupportedException("BorderStyle.Fixed3D is not supported.");
                this.borderStyle = value;
                this.Recalculate(false);
            }
        }
        /// <summary>
        /// Gets or sets the color of the control border
        /// </summary>
        public Color BorderColor
        {
            get
            {
                return this.borderColor;
            }
            set
            {
                this.borderColor = value;
                this.Recalculate(false);
            }
        }
        /// <summary>
        /// Gets or sets the space between the progress bar and the control border.
        /// </summary>
        public Point BorderPadding
        {
            get
            {
                return this.borderPadding;
            }
            set
            {
                this.borderPadding = value;
                this.Recalculate(false);
            }
        }
        /// <summary>
        /// Gets or sets the amount by which a call to the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.PerformStep">PerformStep</see> method increases the current position of the progress bar.
        /// </summary>
        /// <remarks>You can use the Step property to specify the amount that each completed task in an operation changes the value of the progress bar. For example, if you are copying a group of files, you might want to set the value of the Step property to 1 and the value of the Maximum property to the total number of files to copy. When each file is copied, you can call the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.PerformStep">PerformStep</see> method to increment the progress bar by the value of the Step property. If you want to have more flexible control of the value of the progress bar, you can use the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Increment">Increment</see> method or set the value of the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Value">Value</see> property directly.</remarks>
        /// <value>The amount by which to increment the progress bar with each call to the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.PerformStep">PerformStep</see> method. The default is 10.</value>
        public int Step
        {
            get
            {
                return this.step;
            }
            set
            {
                this.step = value;
            }
        }
        /// <summary>
        /// Gets or sets the current position of the progress bar.
        /// </summary>
        public int Value
        {
            get
            {
                return this.currentValue;
            }
            set
            {
                int oldValue = this.currentValue;
                try
                {
                    if (value > this.maximum)
                        this.currentValue = this.maximum;
                    else
                        this.currentValue = value;

                    if (value < this.minimum)
                        this.currentValue = this.minimum;

                    this.Recalculate(false);
                }
                catch
                {
                    this.currentValue = oldValue;
                }
            }
        }
        /// <summary>
        /// Gets or sets the minimum value of the range of the control.
        /// </summary>
        public int Minimum
        {
            get
            {
                return this.minimum;
            }
            set
            {
                if (value >= this.maximum)
                    throw new Exception("Minimum must be less than Maximum.");

                if (this.currentValue < value)
                    this.currentValue = value;

                this.minimum = value;
                this.Recalculate(false);
            }
        }
        /// <summary>
        /// Gets or sets the maximum value of the range of the control.
        /// </summary>
        public int Maximum
        {
            get
            {
                return this.maximum;
            }
            set
            {
                if (value <= this.minimum)
                    throw new Exception("Maximum must be greater than Minimum.");
                if (this.currentValue > value)
                    this.currentValue = value;
                this.maximum = value;
                this.Recalculate(false);
            }
        }

        /// <summary>
        /// Overridden. See <see cref="Control.BackColor">Control.BackColor</see>.
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                this.Recalculate(false);
            }
        }
        /// <summary>
        /// Overridden. See <see cref="Control.Text">Control.Text</see>.
        /// </summary>
        public override string Text
        {
            get
            {
                if (this.showValueText)
                    return this.currentValue.ToString();
                else
                    return (((float)this.currentValue / (float)this.maximum) * 100).ToString() + "%";
            }
            set
            {
            }
        }
        /// <summary>
        /// Overridden. See <see cref="Control.ForeColor">Control.ForeColor</see>.
        /// </summary>
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                this.Recalculate(false);
            }
        }
        /// <summary>
        /// Overridden. See <see cref="Control.Font">Control.Font</see>.
        /// </summary>
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                this.Recalculate(false);
            }
        }
        
        #endregion
        
        /// <summary>
        /// Advances the current position of the progress bar by the amount of the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Step">Step</see> property.
        /// </summary>
        /// <remarks>
        /// The PerformStep method increments the value of the progress bar by the amount specified by the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Step">Step</see> property. You can use the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Step">Step</see> property to specify the amount that each completed task in an operation changes the value of the progress bar. For example, if you are copying a group of files, you might want to set the value of the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Step">Step</see> property to 1 and the value of the Maximum property to the total number of files to copy. When each file is copied, you can call the PerformStep method to increment the progress bar by the value of the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Step">Step</see> property. If you want to have more flexible control of the value of the progress bar, you can use the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Increment">Increment</see> method or set the value of the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Value">Value</see> property directly.
        /// The <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Value">Value</see> property specifies the current position of the ProgressBar. If, after calling the PerformStep method, the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Value">Value</see> property is greater than the value of the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Maximum">Maximum</see> property, the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Value">Value</see> property remains at the value of the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Maximum">Maximum</see> property. If, after calling the PerformStep method with a negative value specified in the value parameter, the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Value">Value</see> property is less than the value of the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Minimum">Value</see> property, the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Value">Value</see> property remains at the value of the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Minimum">Minimum</see> property.
        /// </remarks>
        public void PerformStep()
        {
            this.Value += this.step;
        }

        /// <summary>
        /// Advances the current position of the progress bar by the specified amount.
        /// </summary>
        /// <remarks>The Increment method enables you to increment the value of the progress bar by a specific amount. This method of incrementing the progress bar is similar to using the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Step">Step</see> property with the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.PerformStep">PerformStep</see> method. The <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Value">Value</see> property specifies the current position of the ProgressBar. If, after calling the Increment method, the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Value">Value</see> property is greater than the value of the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Maximum">Maximum</see> property, the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Value">Value</see> property remains at the value of the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Maximum">Maximum</see> property. If, after calling the Increment method with a negative value specified in the value parameter, the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Value">Value</see> property is less than the value of the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Minimum">Minimum</see> property, the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Value">Value</see> property remains at the value of the <see cref="OpenNETCF.Windows.Forms.ProgressBar2.Minimum">Minimum</see> property.</remarks>
        /// <param name="value">The amount by which to increment the progress bar's current position.</param>
        public void Increment(int value)
        {
            this.Value += value;
        }

        #region Overriden Methods

        /// <summary>
        /// Raises the PaintBackground event. (Inherited from <see cref="System.Windows.Forms.Control">Control</see>.)
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data. </param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
           if(this.DesignMode)
              base.OnPaintBackground(e);
        }

        /// <summary>
        /// Raises the Paint event. (Inherited from <see cref="System.Windows.Forms.Control">Control</see>.)
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data. </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.offscreenBM != null)
                e.Graphics.DrawImage(this.offscreenBM, 0, 0);
            else
            {
                e.Graphics.Clear(this.BackColor);
            }
           

        }

        /// <summary>
        /// Raises the Resize event. (Inherited from <see cref="System.Windows.Forms.Control">Control</see>.)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            //Create the offscreen BM and GX
            this.offscreenBM = new Bitmap(this.Width, this.Height);
            this.offscreenGX = Graphics.FromImage(this.offscreenBM);

            this.Recalculate(true);
            base.OnResize(e);

        }

        /// <summary>
        /// Raises the ParentChanged event. (Inherited from <see cref="System.Windows.Forms.Control">Control</see>.)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentChanged(EventArgs e)
        {

            this.Recalculate(true);
            base.OnParentChanged(e);

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets whether the control is in design mode
        /// </summary>
        private bool DesignMode
        {
            get
            {
                if (this.Site != null)
                    if (this.Site.DesignMode)
                        return true;

                return false;
            }
        }

        private void Recalculate(bool forceRedrawOnePix)
        {
            //Draw the onePixBar
            if (forceRedrawOnePix)
                if (this.drawStyle == DrawStyle.Gradient)
                    this.onePixBar = this.CreateGradBackground(new Rectangle(0, 0, 1, this.Height), this.barGradientColor, this.barColor);
                else
                    this.onePixBar = this.CreateSolidBackGround(new Rectangle(0, 0, 1, this.Height), this.barColor);

            //Draw the control offscreen
            this.DrawOffScreen();

            //Invalidate the control
            this.Invalidate();
        }

        private void DrawOffScreen()
        {
            if (this.offscreenGX == null)
                return;

            //Draw the background
            this.offscreenGX.FillRectangle(new SolidBrush(this.BackColor), 0, 0, this.Width, this.Height);

            //draw the progress bar
            //We need to calculate how much of the progress bar to draw
            if (this.onePixBar != null)
            {

                float progressWidth = ((float)this.Width / (float)(this.maximum - this.minimum)) * (float)this.currentValue;
                Rectangle destRect = new Rectangle(borderPadding.X, borderPadding.Y, (int)progressWidth - (borderPadding.X * 2), this.Height - (borderPadding.Y * 2));
                Rectangle srcRect = new Rectangle(0, 0, this.onePixBar.Width, this.onePixBar.Height);
                if (this.currentValue > 0)
                {
                    if (this.DesignMode)
                        this.offscreenGX.FillRectangle(new TextureBrush(this.onePixBar), destRect);
                    else
                        this.offscreenGX.DrawImage(this.onePixBar, destRect, srcRect, GraphicsUnit.Pixel);
                }
            }

            //Draw the border
            if (this.borderStyle != BorderStyle.None)
            {
                Rectangle rect = new Rectangle(0, 0, this.Bounds.Width - 1, this.Bounds.Height - 1);
                this.offscreenGX.DrawRectangle(new Pen(this.borderColor), rect);
            }

            //Draw the progress text
            if (this.showValueText || this.showPercentValueText)
            {
                //find the middle of the control
                SizeF size = this.offscreenGX.MeasureString(this.Text, this.Font);
                int x = ((int)this.Width / 2) - ((int)size.Width / 2);
                int y = ((int)this.Height / 2) - ((int)size.Height / 2);

                //Draw the value
                this.offscreenGX.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), x, y);
            }
        }

        private Bitmap CreateSolidBackGround(Rectangle rc, Color color)
        {

            Bitmap bmpLine = new Bitmap(rc.Width, rc.Height);
            Graphics gx = Graphics.FromImage(bmpLine);
            gx.Clear(this.BarColor);
            gx.Dispose();
            return bmpLine;

        }

        private Bitmap CreateGradBackground(Rectangle rc, Color colStart, Color colEnd)
        {

            //Initialize gradient line + white space 
            Bitmap bmpLine;
            if (orientation == Orientation.Horizontal)
            {
                if (rc.Width <= 0)
                    rc.Width = 1;
                bmpLine = new Bitmap(rc.Width, 1);
            }
            else
            {
                if (rc.Height <= 0)
                    rc.Height = 1;
                bmpLine = new Bitmap(1, rc.Height);
            }
            Graphics gxLine = Graphics.FromImage(bmpLine);
            gxLine.Clear(Color.White);

            //Initialize Backgound bitmap 
            Bitmap bmpBack = new Bitmap(rc.Width, rc.Height);
            Graphics gxBack = Graphics.FromImage(bmpBack);
            gxBack.Clear(Color.White);

            //gradient line rectangle 
            Rectangle rcLine;
            if (orientation == Orientation.Horizontal)
                rcLine = new Rectangle(0, 0, rc.Width, 1);
            else
                rcLine = new Rectangle(0, 0, 1, rc.Height);

            //draw gradient 
            DrawGradient(gxLine, colStart, colEnd, rcLine);

            //Fill the whole backround with prepared lines 
            if (orientation == Orientation.Horizontal)
                for (int i = 0; i < rc.Height; i++)
                    gxBack.DrawImage(bmpLine, 0, i);
            else
                for (int i = 0; i < rc.Width; i++)
                    gxBack.DrawImage(bmpLine, i, 0);


            //			//Draw a black border around the image
            //			gxBack.DrawRectangle(new Pen(Color.DarkGreen),rc.X,rc.Y,rc.Width-1,rc.Height-1);

            gxLine.Dispose();
            gxBack.Dispose();

            return bmpBack;

        }

        private void DrawGradient(Graphics g, Color color1, Color color2, Rectangle rect)
        {

            Pen pen = null;
            //draw the lines
            if (orientation == Orientation.Horizontal)
            {
                if (this.gradientStyle == GradientStyle.Normal)
                {
                    //Draw Normally
                    for (int i = 0; i < rect.Width; i++)
                    {
                        Color currColor = currColor = InterpolateLinear(color1, color2, (float)i, (float)0, (float)rect.Width); ;
                        pen = new Pen(currColor);
                        g.DrawLine(pen, rect.X + i, rect.Top, rect.X + i, rect.Height);
                        pen.Dispose();
                    }
                }
                else
                {
                    //Draw from the middle out
                    for (int i = 0; i <= rect.Width / 2; i++)
                    {
                        Color currColor = currColor = InterpolateLinear(color1, color2, (float)i, (float)0, (float)rect.Width / 2);
                        pen = new Pen(currColor);
                        g.DrawLine(pen, rect.X + i, rect.Top, rect.X + i, rect.Bottom);
                        g.DrawLine(pen, rect.Width - i, rect.Top, rect.Width - i, rect.Height);
                        pen.Dispose();
                    }
                }
            }
            else
            {
                if (this.gradientStyle == GradientStyle.Normal)
                {
                    //Draw Normally
                    for (int i = 0; i < rect.Height; i++)
                    {
                        Color currColor = InterpolateLinear(color1, color2, (float)i, (float)0, (float)rect.Height);
                        pen = new Pen(currColor);
                        g.DrawLine(pen, rect.X, rect.Top + i, rect.X, rect.Height);
                        pen.Dispose();
                    }
                }
                else
                {
                    //Draw from the middle out
                    for (int i = 0; i <= rect.Height / 2; i++)
                    {
                        Color currColor = currColor = InterpolateLinear(color1, color2, (float)i, (float)0, (float)rect.Height / 2);
                        pen = new Pen(currColor);
                        g.DrawLine(pen, rect.X, rect.Top + i, rect.X, rect.Top + i + 1);
                        g.DrawLine(pen, rect.X, rect.Height - i, rect.X, rect.Height - i - 1);
                        pen.Dispose();
                    }
                }
            }


        }

        private Color InterpolateLinear(Color first, Color second, float position, float start, float end)
        {

            float R = ((second.R) * (position - start) + (first.R) * (end - position)) / (end - start);
            float G = ((second.G) * (position - start) + (first.G) * (end - position)) / (end - start);
            float B = ((second.B) * (position - start) + (first.B) * (end - position)) / (end - start);
            return Color.FromArgb((int)Math.Round((double)R), (int)Math.Round((double)G), (int)Math.Round((double)B));

        }

        #endregion
    }
}
