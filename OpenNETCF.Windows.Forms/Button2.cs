using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenNETCF.Drawing;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Represents a button control.
	/// </summary>
    public class Button2 : OpenNETCF.Windows.Forms.ButtonBase2, OpenNETCF.Windows.Forms.IButtonControl, IWin32Window
	{
		#region Fields ==============================================================================

		private static readonly Font DefaultFontValue = new Font("Tahoma", 9.0F, FontStyle.Bold);

		private readonly int sequentialPaintingToken = Int32.MinValue;
		private bool active = false;

		// Public Property Correspondents
		private Color activeBackColor = Color.Black;
		private Image activeBackgroundImage = null;
		private Color activeBorderColor = Color.Black;
		private Color activeForeColor = Color.White;
//		private bool autoSize = false;
		private Color borderColor = Color.Black;
		private BorderStyle borderStyle = BorderStyle.FixedSingle;
		private DialogResult dialogResult = DialogResult.None;
		private Color disabledBackColor = SystemColors.Control;
		private Image disabledBackgroundImage = null;
		private Color disabledBorderColor = SystemColors.GrayText;
		private Color disabledForeColor = SystemColors.GrayText;
		private bool transparentImage = true;

		#endregion ==================================================================================

		#region Properties ==========================================================================

		/// <summary>
		/// Gets or sets the background color for the control in an active state.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Color"/> that represents the active background color of the control. The default is SystemColors.ControlText.</value>
		public virtual Color ActiveBackColor
		{
			get
			{
				return activeBackColor;
			}
			set
			{
				if (activeBackColor != value)
				{
					// OnActiveBackColorChanging(...)
					activeBackColor = value;
					if (this.active)
					{
						this.Invalidate();
					}
					// OnActiveBackColorChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets the background image displayed for the control in an active state.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Image"/> that represents the active image to display in the background of the control.</value>
		public virtual Image ActiveBackgroundImage
		{
			get
			{
				return activeBackgroundImage;
			}
			set
			{
				if (activeBackgroundImage != value)
				{
					// OnActiveBackgroundImageChanging(...)
					activeBackgroundImage = value;
					if (this.active)
					{
						this.Invalidate();
					}
					// OnActiveBackgroundImageChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets the color of the border for the control in an active state.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Color"/> that represents the border color of the control. The default is Color.Black.</value>
		/// <remarks>
		/// This property is only valid when the BorderStyle property is set to FixedSingle.
		/// </remarks>
		public virtual Color ActiveBorderColor
		{
			get
			{
				return activeBorderColor;
			}
			set
			{
				if (activeBorderColor != value)
				{
					// OnActiveBorderColorChanging(...)
					activeBorderColor = value;
					if ((this.active) && (this.BorderStyle == BorderStyle.FixedSingle))
					{
						this.Invalidate();
					}
					// OnActiveBorderColorChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets the foreground color for the control in an active state.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Color"/> that represents the active foreground color of the control. The default is SystemColors.Control.</value>
		public virtual Color ActiveForeColor
		{
			get
			{
				return activeForeColor;
			}
			set
			{
				if (activeForeColor != value)
				{
					// OnActiveForeColorChanging(...)
					activeForeColor = value;
					if (this.active)
					{
						this.Invalidate();
					}
					// OnActiveForeColorChanged(...)
				}
			}
		}

		///// <summary>
		///// Gets or sets a value indicating whether the control is automatically resized to display its contents.
		///// </summary>
		///// <value>A <see cref="T:System.Boolean"/> that is set to <b>true</b> if the control is automatically resized; otherwise, <b>false</b>.</value>
		//public virtual bool AutoSize
		//{
		//   get
		//   {
		//      return autoSize;
		//   }
		//   set
		//   {
		//      if (autoSize != value)
		//      {
		//         // OnAutoSizeChanging(...)
		//         autoSize = value;
		//         // TODO: Provide an implementation, and all other necessary infrastructure, to support this property.
		//         // OnAutoSizeChanged(...)
		//      }
		//   }
		//}

		/// <summary>
		/// Gets or sets the color of the border for the control in an enabled state.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Color"/> that represents the border color of the control. The default is Color.Black.</value>
		/// <remarks>
		/// This property is only valid when the BorderStyle property is set to FixedSingle.
		/// </remarks>
		public virtual Color BorderColor
		{
			get
			{
				return borderColor;
			}
			set
			{
				if (borderColor != value)
				{
					// OnBorderColorChanging(...)
					borderColor = value;
					if ((this.Enabled) && (this.BorderStyle == BorderStyle.FixedSingle))
					{
						this.Invalidate();
					}
					// OnBorderColorChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets the style of the border for the control.
		/// </summary>
		/// <value>One of the <see cref="T:System.Windows.Forms.BorderStyle"/> values. The default is FixedSingle.</value>
		/// <remarks>
		/// It is recommended that applications targeting the broad Windows platform use the Fixed3D value, and applications targeting either the Pocket PC or Smartphone platform use the default, FixedSingle.
		/// </remarks>
		public BorderStyle BorderStyle
		{
			get
			{
				return borderStyle;
			}
			set
			{
				if (Enum.IsDefined(typeof(System.Windows.Forms.BorderStyle), value))
				{
					if (borderStyle != value)
					{
						// OnBorderStyleChanging(...)
						borderStyle = value;
						this.Invalidate();
						// OnBorderStyleChanged(...)
					}
				}
				else
				{
					throw new OpenNETCF.ComponentModel.InvalidEnumArgumentException("The BorderStyle property must be one of the System.Windows.Forms.BorderStyle values.");
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that is returned to the parent form when the button is activated.
		/// </summary>
		/// <value>One of the <see cref="T:System.Windows.Forms.DialogResult"/> values. The default value is None.</value>
		public virtual DialogResult DialogResult
		{
			get
			{
				return dialogResult;
			}
			set
			{
				if (Enum.IsDefined(typeof(System.Windows.Forms.DialogResult), value))
				{
					if (dialogResult != value)
					{
						// OnDialogResultChanging(...)
						dialogResult = value;
						// OnDialogResultChanged(...)
					}
				}
				else
				{
					throw new OpenNETCF.ComponentModel.InvalidEnumArgumentException("The DialogResult property must be one of the System.Windows.Forms.DialogResult values.");
				}
			}
		}

		/// <summary>
		/// Gets or sets the background color for the control in a disabled state.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Color"/> that represents the disabled background color of the control. The default is SystemColors.Control.</value>
		public virtual Color DisabledBackColor
		{
			get
			{
				return disabledBackColor;
			}
			set
			{
				if (disabledBackColor != value)
				{
					// OnDisabledBackColorChanging(...)
					disabledBackColor = value;
					if (!this.Enabled)
					{
						this.Invalidate();
					}
					// OnDisabledBackColorChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets the background image displayed for the control in a disabled state.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Image"/> that represents the disabled image to display in the background of the control.</value>
		public virtual Image DisabledBackgroundImage
		{
			get
			{
				return disabledBackgroundImage;
			}
			set
			{
				if (disabledBackgroundImage != value)
				{
					// OnDisabledBackgroundImageChanging(...)
					disabledBackgroundImage = value;
					if (!this.Enabled)
					{
						this.Invalidate();
					}
					// OnDisabledBackgroundImageChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets the color of the border for the control in a disabled state.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Color"/> that represents the disabled border color of the control.</value>
		/// <remarks>
		/// This property is only valid when the BorderStyle property is set to FixedSingle.
		/// </remarks>
		public virtual Color DisabledBorderColor
		{
			get
			{
				return disabledBorderColor;
			}
			set
			{
				if (disabledBorderColor != value)
				{
					// OnDisabledBorderColorChanging(...)
					disabledBorderColor = value;
					if ((!this.Enabled) && (this.BorderStyle == BorderStyle.FixedSingle))
					{
						this.Invalidate();
					}
					// OnDisabledBorderColorChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets the foreground color for the control in a disabled state.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Color"/> that represents the disabled foreground color of the control.</value>
		public virtual Color DisabledForeColor
		{
			get
			{
				return disabledForeColor;
			}
			set
			{
				if (disabledForeColor != value)
				{
					// OnDisabledForeColorChanging(...)
					disabledForeColor = value;
					if (!this.Enabled)
					{
						this.Invalidate();
					}
					// OnDisabledForeColorChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the foreground image for the control contains transparency.
		/// </summary>
		/// <value>A <see cref="T:System.Boolean"/> that is set to <b>true</b> if the foreground image of the control contains transparency; otherwise, <b>false</b>. The default is <b>true</b>.</value>
		/// <remarks>
		/// The color of the top-left pixel in the foreground image is used as the transparency key.
		/// </remarks>
		public bool TransparentImage
		{
			get
			{
				return transparentImage;
			}
			set
			{
				if (transparentImage != value)
				{
					// OnTransparentImageChanging(...)
					transparentImage = value;
					if (this.Image != null)
					{
						this.Invalidate();
					}
					// OnTransparentImageChanged(...)
				}
			}
		}

        protected bool Active
        {
            get
            {
                return this.active;
            }
        }
		#endregion ==================================================================================

		#region Methods =============================================================================

		/// <summary>
		/// Initializes a new instance of the Button2 class.
		/// </summary>
		public Button2()
		{
			// Register in the sequential painting process so that, primarily, if this class is the most 
			// derived class in the hierarchy that needs to update the presentation of the control, the 
			// Paint event may be delayed until all classes in the hierarchy have had a chance to update 
			// the presentation.
			this.sequentialPaintingToken = base.RegisterSequentialPainting();

			// Request that the presentation be buffered before being drawn to the control.
			base.DoubleBuffered = true;

			this.BackColor = SystemColors.Control;
			this.Font = DefaultFontValue;
			this.Size = new Size(72, 20);
		}

		/// <summary>
		/// Determines the location at which the content should be displayed, on the control, based on the indicated alignment value.
		/// </summary>
		/// <param name="alignment">The <see cref="T:OpenNETCF.Drawing.ContentAlignment2"/> that represents how the content should be positioned on the control.</param>
		/// <param name="contentSize">The <see cref="T:System.Drawing.SizeF"/> containing the width and height of the content to display on the control.</param>
		/// <param name="clipRect">The <see cref="T:System.Drawing.Rectangle"/> that represents the allotted area in which the content may be displayed.</param>
		/// <returns>A <see cref="T:System.Drawing.Point"/> containing the x and y coordinates at which the content should be displayed.</returns>
		private Point GetLocationFromContentAlignment(OpenNETCF.Drawing.ContentAlignment2 alignment, SizeF contentSize, Rectangle clipRect)
		{
			byte x = 0;
			Point point = Point.Empty;

			// Vertical Alignment
			if ((((int)alignment) & 0xF00) > 0)			// Bottom
			{
				point.Y = (int)(clipRect.Height - contentSize.Height);
				x = (byte)((int)alignment >> 8);
			}
			else if ((((int)alignment) & 0x0F0) > 0)	// Middle
			{
				point.Y = (int)(((float)clipRect.Height / 2) - (contentSize.Height / 2));
				x = (byte)((int)alignment >> 4);
			}
			else if ((((int)alignment) & 0x00F) > 0)	// Top
			{
				point.Y = 0;
				x = (byte)((int)alignment);
			}

			// Horizontal Alignment
			if ((x & 0x2) > 0)		// Center
			{
				point.X = (int)(((float)clipRect.Width / 2) - (contentSize.Width / 2));
			}
			else if ((x & 0x1) > 0)	// Left
			{
				point.X = 0;
			}
			else if ((x & 0x4) > 0)	// Right
			{
				point.X = (int)(clipRect.Width - contentSize.Width);
			}

			// Ensure that the x coordinate is greater than or equal to zero.
			if (point.X < 0)
			{
				point.X = 0;
			}

			// Ensure that the y coordinate is greater than or equal to zero.
			if (point.Y < 0)
			{
				point.Y = 0;
			}

			// Offset the x and y coordinates based on the clip rectangle.
			point.X += clipRect.X;
			point.Y += clipRect.Y;

			return point;
		}

		/// <summary>
		/// Notifies the button whether it is the default button so that it can adjust its appearance accordingly.
		/// </summary>
		/// <param name="value"><b>true</b> if the button is to have the appearance of the default button; otherwise, <b>false</b>.</param>
		public virtual void NotifyDefault(bool value)
		{
			if (this.IsDefault != value)
			{
				this.IsDefault = value;
			}
		}

		/// <summary>
		/// Raises the Click event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnClick(System.EventArgs e)
		{
			// If the user has selected the control, then ensure that the presentation of the control 
			// is updated accordingly.
			if (this.active)
			{
				this.active = false;
				this.Invalidate();
			}
			// If applicable, indicate to the Form, through the Forms DialogResult property, that the 
			// control was selected.
			if ((this.DialogResult != DialogResult.None) && (this.TopLevelControl is System.Windows.Forms.Form))
			{
				((System.Windows.Forms.Form)this.TopLevelControl).DialogResult = this.DialogResult;
			}
			base.OnClick(e);
		}

		/// <summary>
		/// Raises the DoubleClick event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnDoubleClick(System.EventArgs e)
		{
			// If the user has selected the control, then ensure that the presentation of the control 
			// is updated accordingly.
			if (this.active)
			{
				this.active = false;
				this.Invalidate();
			}
			base.OnDoubleClick(e);
		}

		/// <summary>
		/// Raises the EnabledChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnEnabledChanged(System.EventArgs e)
		{
			this.Invalidate();
			base.OnEnabledChanged(e);
		}

		/// <summary>
		/// Raises the GotFocus event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnGotFocus(System.EventArgs e)
		{
			this.Invalidate();
			base.OnGotFocus(e);
		}

		/// <summary>
		/// Raises the KeyPress event.
		/// </summary>
		/// <param name="e">A KeyPressEventArgs that contains the event data.</param>
		protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			// If the end-developer hasn't explicitly handled the key, then simulate a click if the key 
			// represents either the Enter or Space key.
			if (!e.Handled)
			{
				switch (Convert.ToInt32(e.KeyChar))
				{
					case (int)Keys.Enter: case (int)Keys.Space:
					{
						this.PerformClick();
						break;
					}
				}
			}
		}

		/// <summary>
		/// Raises the LostFocus event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnLostFocus(System.EventArgs e)
		{
			this.Invalidate();
			base.OnLostFocus(e);
		}

		/// <summary>
		/// Raises the MouseDown event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			// If the user has selected the control, by indication of the left mouse button, then ensure 
			// that the control is focused and update the presentation.
			if (e.Button == MouseButtons.Left)
			{
				this.active = true;
				if (!this.Focused)
				{
					this.Focus();
				}
				else
				{
					this.Invalidate();
				}
			}
			base.OnMouseDown(e);
		}

		/// <summary>
		/// Raises the MouseMove event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			// If the user has selected the control, by indication of the left mouse button, and then 
			// moves the mouse position, ensure that the presentation of the control is updated accordingly.
			if (e.Button == MouseButtons.Left)
			{
				Rectangle controlRect = new Rectangle(0, 0, this.Width, this.Height);
				if ((this.active) && (!controlRect.Contains(e.X, e.Y)))
				{
					this.active = false;
					this.Invalidate();
				}
				else if ((!this.active) && (controlRect.Contains(e.X, e.Y)))
				{
					this.active = true;
					this.Invalidate();
				}
			}
			base.OnMouseMove(e);
		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		/// <remarks>
		/// <b>Notes to Inheritors:</b> See the example section, in the <see cref="T:OpenNETCF.Windows.Forms.ButtonBase2"/> control documentation, for the recommended pattern to ensure that double buffering and structured paint sequencing are both accommodated.
		/// </remarks>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			// Call the base classes OnPaint method to ensure that all appropriate base painting has been 
			// done prior to performing the presentation contribution of this class.
			base.OnPaint(e);

			// Get a reference to the proper Graphics object used to update the presentation of the control.
			Graphics presentation = base.GetPresentationMedium(e.Graphics);

			#region Presentation Logic ===============================================================

			int scaleFactor = (int)(presentation.DpiX / 96);
			Size focusBorderSize = new Size((1 * scaleFactor), (1 * scaleFactor));

			#region Background =======================================================================

			Image backgroundImage = this.BackgroundImage;

			// Determine the image to be used as the background for the control, if applicable.
			if ((this.active) && (this.ActiveBackgroundImage != null))
			{
				backgroundImage = this.ActiveBackgroundImage;
			}
			else if ((!this.Enabled) && (this.DisabledBackgroundImage != null))
			{
				backgroundImage = this.DisabledBackgroundImage;
			}

			// If there is no background image for the control, paint using the appropriate background 
			// color; otherwise, paint the background image.
			if (backgroundImage == null)
			{
				SolidBrush backColorBrush = new SolidBrush(this.BackColor);
				if (this.active)
				{
					backColorBrush.Color = this.ActiveBackColor;
				}
				else if (!this.Enabled)
				{
					backColorBrush.Color = this.DisabledBackColor;
				}
				presentation.FillRectangle(backColorBrush, 0, 0, this.Width, this.Height);
				backColorBrush.Dispose();
			}
			else
			{
				Rectangle destinationRect = new Rectangle(0, 0, this.Width, this.Height);
				Rectangle sourceRect = new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height);
				presentation.DrawImage(backgroundImage, destinationRect, sourceRect, GraphicsUnit.Pixel);
			}

			#endregion ===============================================================================

			#region Content ==========================================================================

			int minimumContentOffset = 1;
			Rectangle minimumContentRect = Rectangle.Empty;
			Size maximumBorderSize = Size.Empty;

			// Determine the maximum size of the border based on the specified border style.
			switch (this.BorderStyle)
			{
				case BorderStyle.None:
				{
					maximumBorderSize = new Size(0, 0);
					break;
				}
				case BorderStyle.FixedSingle:
				{
					maximumBorderSize = new Size(1, 1);
					break;
				}
				case BorderStyle.Fixed3D:
				{
					maximumBorderSize = new Size(2, 2);
					break;
				}
			}

			// Multiply to the scale factor.
			maximumBorderSize.Width *= scaleFactor;
			maximumBorderSize.Height *= scaleFactor;

			// Add the focus border size to the maximum border size value.
			maximumBorderSize.Width += focusBorderSize.Width;
			maximumBorderSize.Height += focusBorderSize.Height;

			// Determine the worst case scenario for the content rectangle.
			minimumContentRect = new Rectangle((maximumBorderSize.Width + minimumContentOffset), (maximumBorderSize.Height + minimumContentOffset), (this.ClientRectangle.Width - ((maximumBorderSize.Width + minimumContentOffset) * 2)), (this.ClientRectangle.Height - ((maximumBorderSize.Height + minimumContentOffset) * 2)));

			#region Image ============================================================================

			Image foregroundImage = this.Image;

			// Paint the image to be used as part of the content for the control, if applicable.
			if (foregroundImage != null)
			{
				ImageAttributes attributes;
				Point contentPoint;
				Rectangle destinationRect;
				SizeF contentSize;

				// Create an ImageAttributes object to set the transparency key, if applicable.
				attributes = new ImageAttributes();

				// Determine the location at which to draw the content image.
				contentSize = new SizeF((foregroundImage.Size.Width * scaleFactor), (foregroundImage.Size.Height * scaleFactor));
				contentPoint = GetLocationFromContentAlignment(this.ImageAlign, contentSize, minimumContentRect);

				// Determine the rectangle in which the content image may be painted.
				destinationRect = new Rectangle(contentPoint.X, contentPoint.Y, (foregroundImage.Width * scaleFactor), (foregroundImage.Height * scaleFactor));

				// Set the transparency color to be used when painting the content image, if applicable.
				if (this.TransparentImage)
				{
					Bitmap foregroundBitmap;
					bool bitmapCreated;
					Color foregroundColor;
					// Get a reference to the content image as a Bitmap object.
					bitmapCreated = false;
					foregroundBitmap = foregroundImage as Bitmap;
					if (foregroundBitmap == null)
					{
						foregroundBitmap = new Bitmap(foregroundImage);
						bitmapCreated = true;
					}
					// Get the color of the top-left pixel in the content image.
					foregroundColor = foregroundBitmap.GetPixel(0, 0);
					// Set the transparency key on the ImageAttributes object.
					attributes.SetColorKey(foregroundColor, foregroundColor);
					// If a Bitmap object was created, release the resources.
					if (bitmapCreated)
					{
						foregroundBitmap.Dispose();
					}
				}

				// Paint the content image for the control.
				presentation.DrawImage(foregroundImage, destinationRect, 0, 0, foregroundImage.Width, foregroundImage.Height, GraphicsUnit.Pixel, attributes);
			}

			#endregion ===============================================================================

			#region Text =============================================================================

			// TODO: If the control is disabled and the BorderStyle property is set to Fixed3D, the content 
			// text should be painted in the resting state using the dark border color and then the content 
			// text should again be painted at the same location as it would be in the active state using 
			// the lightest border color.

			string foregroundText = this.Text;

			// Paint the text to be used as part of the content for the control, if applicable.
			if ((foregroundText != null) && (this.Font != null))
			{
				bool activeAction;
				Point contentPoint;
				Rectangle destinationRect;
				SizeF contentSize;
				SolidBrush foreColorBrush;

				// Determine the location at which to draw the content text.
				contentSize = presentation.MeasureString(foregroundText, this.Font);
				contentPoint = GetLocationFromContentAlignment(this.TextAlign, contentSize, minimumContentRect);

				// If the control is active and rendered with a three-dimensional appearance, offset the 
				// location of the content text.
				activeAction = false;
				if ((this.BorderStyle == BorderStyle.Fixed3D) && (this.active))
				{
					activeAction = true;
					contentPoint.X += (1 * scaleFactor);
					contentPoint.Y += (1 * scaleFactor);
				}

				// Determine the rectangle in which the content text may be painted.
				destinationRect = new Rectangle(contentPoint.X, contentPoint.Y, (minimumContentRect.X + minimumContentRect.Width - contentPoint.X + ((activeAction) ? (1 * scaleFactor) : 0)), (minimumContentRect.Y + minimumContentRect.Height - contentPoint.Y + ((activeAction) ? (1 * scaleFactor) : 0)));

				// Determine the color to be used as the foreground for the control.
				foreColorBrush = new SolidBrush(this.ForeColor);
				if (this.active)
				{
					foreColorBrush.Color = this.ActiveForeColor;
				}
				else if (!this.Enabled)
				{
					foreColorBrush.Color = this.DisabledForeColor;
				}

				// Paint the content text for the control.
				presentation.DrawString(foregroundText, this.Font, foreColorBrush, destinationRect);

				// Release any resources.
				foreColorBrush.Dispose();
			}

			#endregion ===============================================================================

			#endregion ===============================================================================

			#region Border ===========================================================================

			bool paintFocusBorder = false;
			Pen borderPen = new Pen(this.BorderColor, (1 * scaleFactor));
			Size borderOffsetSize = new Size(0, 0);
			int rectWidth = (int)borderPen.Width;
			int penOffset = (rectWidth / 2);

			// Determine the color to be used as the border for the control.
			if (this.active)
			{
				borderPen.Color = this.ActiveBorderColor;
			}
			else if (!this.Enabled)
			{
				borderPen.Color = this.DisabledBorderColor;
			}

			// If the control is focused or specified as the default, indicate that the focus border 
			// should be painted.
			if ((this.Focused) || (this.IsDefault))
			{
				paintFocusBorder = true;
			}

			// Modify the border offset size if the focus border is to be painted.
			if (paintFocusBorder)
			{
				borderOffsetSize = focusBorderSize;
			}

			// Paint the border for the control.
			switch (this.BorderStyle)
			{
				case BorderStyle.FixedSingle:
				{
					// Paint the border for the control with a single-line appearance.
					presentation.DrawRectangle(borderPen, (borderOffsetSize.Width + penOffset), (borderOffsetSize.Height + penOffset), (this.ClientRectangle.Width - (borderOffsetSize.Width * 2) - (penOffset * 2) - 1), (this.ClientRectangle.Height - (borderOffsetSize.Height * 2) - (penOffset * 2) - 1));
					break;
				}
				case BorderStyle.Fixed3D:
				{
					// TODO: Ensure that the fixed 3D border can scale on higher DPI platforms.

					// TODO: If the control is focused and the BorderStyle property is set to Fixed3D, a dotted 
					// rectangle should be painted around the outside of the content rectangle.

					// TODO: Calculate the light and dark border colors based on the painted background color.

					Color darkBorderColor = SystemColors.ControlDark;
					Color darkestBorderColor = Color.Black;
					Color lightBorderColor = SystemColors.Control;
					Color lightestBorderColor = SystemColors.ControlLightLight;
					Pen borderPenFixed3D = new Pen(darkBorderColor);

					// Paint the border for the control with a three-dimensional appearance. The border 
					// will look different depending on whether the control is in an active or inactive 
					// state.
					if (!this.active)
					{
						// Paint the dark lines.
						presentation.DrawLine(borderPenFixed3D, (this.ClientRectangle.Width - borderOffsetSize.Width - 2), (borderOffsetSize.Height + 1), (this.ClientRectangle.Width - borderOffsetSize.Width - 2), (this.ClientRectangle.Height - borderOffsetSize.Height - 2));
						presentation.DrawLine(borderPenFixed3D, (borderOffsetSize.Width + 1), (this.ClientRectangle.Height - borderOffsetSize.Height - 2), (this.ClientRectangle.Width - borderOffsetSize.Width - 2), (this.ClientRectangle.Height - borderOffsetSize.Height - 2));
						// Paint the darkest lines.
						borderPenFixed3D.Color = darkestBorderColor;
						presentation.DrawLine(borderPenFixed3D, (this.ClientRectangle.Width - borderOffsetSize.Width - 1), borderOffsetSize.Height, (this.ClientRectangle.Width - borderOffsetSize.Width - 1), (this.ClientRectangle.Height - borderOffsetSize.Height - 1));
						presentation.DrawLine(borderPenFixed3D, borderOffsetSize.Width, (this.ClientRectangle.Height - borderOffsetSize.Height - 1), (this.ClientRectangle.Width - borderOffsetSize.Width - 1), (this.ClientRectangle.Height - borderOffsetSize.Height - 1));
						// Paint the light lines.
						borderPenFixed3D.Color = lightBorderColor;
						presentation.DrawLine(borderPenFixed3D, (borderOffsetSize.Width + 1), (borderOffsetSize.Height + 1), (this.ClientRectangle.Width - borderOffsetSize.Width - 3), (borderOffsetSize.Height + 1));
						presentation.DrawLine(borderPenFixed3D, (borderOffsetSize.Width + 1), (borderOffsetSize.Height + 1), (borderOffsetSize.Width + 1), (this.ClientRectangle.Height - borderOffsetSize.Height - 3));
						// Paint the lightest lines.
						borderPenFixed3D.Color = lightestBorderColor;
						presentation.DrawLine(borderPenFixed3D, borderOffsetSize.Width, borderOffsetSize.Height, (this.ClientRectangle.Width - borderOffsetSize.Width - 2), borderOffsetSize.Height);
						presentation.DrawLine(borderPenFixed3D, borderOffsetSize.Width, borderOffsetSize.Height, borderOffsetSize.Width, (this.ClientRectangle.Height - borderOffsetSize.Height - 2));
					}
					else
					{
						presentation.DrawRectangle(borderPenFixed3D, borderOffsetSize.Width, borderOffsetSize.Height, (this.ClientRectangle.Width - (borderOffsetSize.Width * 2) - 1), (this.ClientRectangle.Height - (borderOffsetSize.Height * 2) - 1));
					}

					// Release any resources.
					borderPenFixed3D.Dispose();

					break;
				}
			}

			// If the control is focused or specified as the default, paint a focus border.
			if (paintFocusBorder)
			{
				presentation.DrawRectangle(borderPen, penOffset, penOffset, (this.ClientRectangle.Width - (penOffset * 2) - 1), (this.ClientRectangle.Height - (penOffset * 2) - 1));
			}

			// Release any resources.
			borderPen.Dispose();

			#endregion ===============================================================================

			#endregion ===============================================================================

			// Indicate that this class is done updating the presentation. If the sequential paint token 
			// for this class was the last one registered, then the Paint event will be triggered, and, 
			// if the presentation was double buffered, the contents of the buffer will be drawn to the 
			// control.
			base.NotifyPaintingComplete(this.sequentialPaintingToken, e);
		}

		/// <summary>
		/// Raises the TextChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnTextChanged(System.EventArgs e)
		{
			this.Invalidate();
			base.OnTextChanged(e);
		}

		/// <summary>
		/// Generates a Click event for a button.
		/// </summary>
		public virtual void PerformClick()
		{
			if ((this.Enabled) && (this.Visible))
			{
				this.OnClick(EventArgs.Empty);
			}
		}

		#endregion ==================================================================================
	}
}