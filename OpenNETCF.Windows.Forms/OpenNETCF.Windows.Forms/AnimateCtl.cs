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
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.CodeDom;
using System.ComponentModel;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Animates an image.
	/// </summary>
	public partial class AnimateCtl : System.Windows.Forms.UserControl, IWin32Window
	{
		
		#region Private Members ==================================================
		/// <summary>
		/// The bitmap to animate.  Can be a GIF or BMP
		/// </summary>
		private Image _bitmap;
		/// <summary>
		/// The number of frames in the image
		/// </summary>
		private int _frameCount;
		/// <summary>
		/// The width of a frame
		/// </summary>
		private int _frameWidth;
		/// <summary>
		/// The Height of the frame
		/// </summary>
		private  int _frameHeight;
		/// <summary>
		/// Value to see if the control is animating
		/// </summary>
		private bool animating = false;
		/// <summary>
		/// Number of frames available
		/// </summary>
		private int _currentFrame = 0;
		/// <summary>
		/// Number of times to loop the animation
		/// </summary>
		private int _loopCount = 0;
		/// <summary>
		/// Number of times the animation as looped
		/// </summary>
		private int _loopCounter = 0;
		/// <summary>
		/// Delay interval for the animation
		/// </summary>
		private int _delayInterval = 0;
		/// <summary>
		/// The timer for the animation
		/// </summary>
		private System.Windows.Forms.Timer fTimer = new System.Windows.Forms.Timer();

		/// <summary>
		/// The direction of the animiation
		/// </summary>
		private DrawDirection drawDirection = DrawDirection.Horizontal;
        /// <summary>
        /// Whether the animiation should autostart
        /// </summary>
        private bool autoStartAnimation = false;
		#endregion

		#region Constructor/Destructor ==================================================
		/// <summary>
		/// Default contructor
		/// </summary>
		public AnimateCtl()
		{
            //this._frameHeight = 30;
            //this._frameWidth = 30;
            //this._delayInterval = 25;

            //Hook up to the Timer's Tick event
            fTimer.Tick += new System.EventHandler(this.timer1_Tick);
		}
		#endregion

		#region Public Members ==================================================
		/// <summary>
        /// The direction of the sequence of images in the image file.
		/// </summary>
        [Obsolete("This property has been deprecated. Use AnimateCtl.DrawDirection instead.",false)]
		public DrawDirection AnimDrawDirection

		{
			set
			{
                this.DrawDirection = value;
			}
			get
			{
				return this.DrawDirection;
			}
		}

        /// <summary>
        /// The direction of the sequence of images in the image file.
        /// </summary>
        public DrawDirection DrawDirection
        {
            set
            {
                this.drawDirection = value;
                this.ResizeControl();
            }
            get
            {
                return this.drawDirection;

            }
        }

		/// <summary>
        /// The image to animate.
		/// </summary>
		public Image Image
		{
			get
			{
				return this._bitmap;
			}
			set
			{
				this._bitmap = value;
				this.Invalidate();
			}
		}

        /// <summary>
        /// Gets or sets the Height of the control
        /// </summary>
        public new int Height
		{
			get
			{
				return base.Height;
			}
			set
			{
				this._frameHeight =value;
				base.Height = value;
				this.ResizeControl();
			}
		}

        /// <summary>
        /// Gets or sets the width of the control
        /// </summary>
		public new int Width
		{
			get
			{
				return base.Width;
			}
			set
			{
                this._frameWidth  =value;
				base.Width = value;
				this.ResizeControl();
			}
		}

		/// <summary>
		/// Gets or sets  the width of the frame to animate
		/// </summary>
		public int FrameWidth
		{
			get{return this._frameWidth;}
			set
			{
				this._frameWidth = value;
				this.Width = value;
			}
		}

        /// <summary>
        /// Gets or sets  the height of the frame to animate
        /// </summary>
        public int FrameHeight
        {
            get { return this._frameHeight; }
            set
            {
                this._frameHeight = value;
                this.Height = value;
            }
        }

		/// <summary>
		/// Gets or sets the delay interval for the control
		/// </summary>
		public int DelayInterval
		{
			set
			{
				this._delayInterval = value;
				this.fTimer.Interval = this._delayInterval;
			}
			get{return this._delayInterval;}
		}

		/// <summary>
		/// Gets or sets  the amount of times to loop the animation. -1 to loop infinitly.
		/// </summary>
		public int LoopCount
		{
			set{this._loopCount = value;}
			get{return this._loopCount;}
		}

        
        /// <summary>
        /// Gets or sets whether the animation should auto start.
        /// </summary>
        public bool AutoStartAnimation
        {
            get
            {
                return this.autoStartAnimation;
            }
            set
            {
                this.autoStartAnimation = value;
            }
        }

		/// <summary>
		/// Override the painBackground to avoid flickering
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if(!this.animating)
				base.OnPaintBackground (e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			//Determin what to paint
			if(this._bitmap != null)
			{
				//			if(this.animating)
				//			{
				if (this._loopCount == 0) //loop continuosly
				{
                    if(!StaticMethods.IsDesignMode(this))
                        if(this.autoStartAnimation)
					        this.StartAnimation();

					this.DrawFrame(e.Graphics);
				}
				else
				{
					if (this._loopCount == this._loopCounter) //stop the animation
					{
						fTimer.Enabled = false;	
						return;
					}
					else
						this.DrawFrame(e.Graphics);
				}
				//			}
			}
			base.OnPaint(e);
			//GC.Collect();
		}

		protected override void OnParentChanged(EventArgs e)
		{
			if(this.Parent!=null)
			{
				this.BackColor = this.Parent.BackColor;
			}
			base.OnParentChanged (e);
			this.Invalidate();
		}

		/// <summary>
		/// Begin animating the image
		/// </summary>
		public void StartAnimation()
		{
			if(!this.animating)
			{
				this.animating = true;				

				//Reset loop counter
				this._loopCounter = 0;

				//Calculate the frameCount
				this.ResizeControl();
			
				//Resize the control
				this.Size = new Size(this._frameWidth, this._frameHeight);
				//Assign delay interval to the timer
				fTimer.Interval = this._delayInterval;

				//Start the timer
                if (!StaticMethods.IsDesignMode(this))
				    fTimer.Enabled = true;

				this.Visible = true;
				this.BringToFront();
			}
			
		}

		/// <summary>
		/// Stops the current animation
		/// </summary>
		public void StopAnimation()
		{				
			if(this.animating)
			{
				fTimer.Enabled = false;	
				this.animating = false;
			}
		}



		#endregion

		#region Private Routines ==================================================

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			this.Invalidate();
		}

		/// <summary>
		/// Draw the frame
		/// </summary>
		private void DrawFrame(Graphics g)
		{
			if (this._currentFrame < this._frameCount-1)
			{
				this._currentFrame++;
			}
			else
			{
				//increment the loopCounter
				this._loopCounter++;
				this._currentFrame = 0;
			}
			this.Draw(this._currentFrame,g);
		}

		/// <summary>
		/// Draw the image
		/// </summary>
		/// <param name="iframe"></param>
        /// <param name="g"></param>
        private void Draw(int iframe, Graphics g)
		{
            if (StaticMethods.IsDesignMode(this))
			    iframe = 0;

			Rectangle rect = Rectangle.Empty;
			//Calculate the left location of the drawing frame
            if(this.drawDirection == DrawDirection.Horizontal)
                rect = new Rectangle(iframe * this._frameWidth, 0, this._frameWidth, this._frameHeight);	
            else
                rect = new Rectangle(0, iframe * this._frameHeight, this._frameWidth, this._frameHeight);					

			//Draw image
			g.DrawImage(this._bitmap, 0, 0, rect, GraphicsUnit.Pixel);
			//ImageAttributes ia = new ImageAttributes();
			//ia.SetColorKey(BackgroundImageColor(this._bitmap),BackgroundImageColor(this._bitmap));
			//g.Clear(this.Parent.BackColor);
//			g.DrawImage(this._bitmap,
//				new Rectangle(0,0,this._frameWidth,this._frameHeight),
//				Location,0,this._frameWidth,this._frameHeight,GraphicsUnit.Pixel,ia);
		}

		/// <summary>
		/// Gets the background color to make transparent
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		private Color BackgroundImageColor(Image image)
		{
			Bitmap bmp = new Bitmap(image);
			Color ret = bmp.GetPixel(0, 0);
			return ret;
		}

		/// <summary>
		/// Resize the animation control
		/// </summary>
		private void ResizeControl()
		{
			if(this._bitmap!=null)
			{
				if(this.drawDirection == DrawDirection.Horizontal)
				{
					this._frameCount = this._bitmap.Width / this._frameWidth;
				}
				else
				{
                    this._frameCount = this._bitmap.Height / this._frameHeight;
				}
                this.Size = new Size(this._frameWidth, this._frameHeight);
				this.Invalidate();
			}
		}

        

		#endregion

		
	}
}
