using System;
using System.Drawing;
using System.Windows.Forms;
using OpenNETCF.Drawing;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Implements the basic functionality common to button controls.
	/// </summary>
    public abstract class ButtonBase2 : System.Windows.Forms.Control, IWin32Window
	{
		#region Fields ==============================================================================

		private const int sequentialPaintingToken = 0;

		private Bitmap bufferBitmap = null;
		private bool internalChange = false;
		private int registeredSequentialPaintingToken = sequentialPaintingToken;

		// Public Property Correspondents
		private Image backgroundImage = null;
		private Image image = null;
		private OpenNETCF.Drawing.ContentAlignment2 imageAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleCenter;
		private int imageIndex = -1;
		private ImageList imageList = null;
		private bool isDisposed = false;
		private OpenNETCF.Drawing.ContentAlignment2 textAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleCenter;

		// Protected Property Correspondents
		private Graphics bufferGraphics = null;
		private bool doubleBuffered = false;
		private bool isDefault = false;

		#endregion ==================================================================================

		#region Properties ==========================================================================

		#region Public ==============================================================================

		/// <summary>
		/// Gets or sets the background image displayed in the control.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Image"/> that represents the image to display in the background of the control.</value>
		public virtual Image BackgroundImage
		{
			get
			{
				return backgroundImage;
			}
			set
			{
				if (backgroundImage != value)
				{
					// OnBackgroundImageChanging(...)
					backgroundImage = value;
					this.Invalidate();
					// OnBackgroundImageChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets the image that is displayed on the control.
		/// </summary>
		/// <value>The <see cref="T:System.Drawing.Image"/> displayed on the control. The default value is a null reference (Nothing in Visual Basic).</value>
		public Image Image
		{
			get
			{
				int index = this.ImageIndex;
				if (index > -1)
				{
					return this.ImageList.Images[index];
				}
				return image;
			}
			set
			{
				if (image != value)
				{
					// OnImageChanging(...)
					image = value;
					if (!this.internalChange)
					{
						this.internalChange = true;
						this.ImageIndex = -1;
						this.ImageList = null;
						this.internalChange = false;
						this.Invalidate();
					}
					// OnImageChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets the alignment of the image on the control.
		/// </summary>
		/// <value>One of the <see cref="T:OpenNETCF.Drawing.ContentAlignment2"/> values. The default value is MiddleCenter.</value>
		public OpenNETCF.Drawing.ContentAlignment2 ImageAlign
		{
			get
			{
				return imageAlign;
			}
			set
			{
				if (Enum.IsDefined(typeof(OpenNETCF.Drawing.ContentAlignment2), value))
				{
					if (imageAlign != value)
					{
						// OnImageAlignChanging(...)
						imageAlign = value;
						this.Invalidate();
						// OnImageAlignChanged(...)
					}
				}
				else
				{
					throw new OpenNETCF.ComponentModel.InvalidEnumArgumentException("The ImageAlign property must be one of the OpenNETCF.Drawing.ContentAlignment2 values.");
				}
			}
		}

		/// <summary>
		/// Gets or sets the image list index value of the image displayed on the control.
		/// </summary>
		/// <value>A zero-based index, which represents the image position in a <see cref="T:System.Windows.Forms.ImageList"/>. The default is -1.</value>
		public int ImageIndex
		{
			get
			{
				if (this.ImageList != null)
				{
					int lastIndex = this.ImageList.Images.Count - 1;
					if (imageIndex > lastIndex)
					{
						return lastIndex;
					}
					return imageIndex;
				}
				return -1;
			}
			set
			{
				if (value >= -1)
				{
					if (imageIndex != value)
					{
						// OnImageIndexChanging(...)
						imageIndex = value;
						if (!this.internalChange)
						{
							this.internalChange = true;
							this.Image = null;
							this.internalChange = false;
							this.Invalidate();
						}
						// OnImageIndexChanged(...)
					}
				}
				else
				{
					throw new ArgumentException("The ImageIndex property must be greater than, or equal to, -1.");
				}
			}
		}

		/// <summary>
		/// Gets or sets the image list that contains the image displayed on the control.
		/// </summary>
		/// <value>A <see cref="T:System.Windows.Forms.ImageList"/>. The default value is a null reference (Nothing in Visual Basic).</value>
		public ImageList ImageList
		{
			get
			{
				return imageList;
			}
			set
			{
				if (imageList != value)
				{
					// OnImageListChanging(...)
					imageList = value;
					if (!this.internalChange)
					{
						this.internalChange = true;
						this.Image = null;
						this.internalChange = false;
						this.Invalidate();
					}
					// OnImageListChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether the control has been disposed.
		/// </summary>
		/// <value><b>true</b> if the control has been disposed; otherwise, <b>false</b>.</value>
		public bool IsDisposed
		{
			get
			{
				return isDisposed;
			}
		}

		/// <summary>
		/// Gets or sets the alignment of the text on the control.
		/// </summary>
		/// <value>One of the <see cref="T:OpenNETCF.Drawing.ContentAlignment2"/> values. The default value is MiddleCenter.</value>
		public virtual OpenNETCF.Drawing.ContentAlignment2 TextAlign
		{
			get
			{
				return textAlign;
			}
			set
			{
				if (Enum.IsDefined(typeof(OpenNETCF.Drawing.ContentAlignment2), value))
				{
					if (textAlign != value)
					{
						// OnTextAlignChanging(...)
						textAlign = value;
						this.Invalidate();
						// OnTextAlignChanged(...)
					}
				}
				else
				{
					throw new OpenNETCF.ComponentModel.InvalidEnumArgumentException("The TextAlign property must be one of the OpenNETCF.Drawing.ContentAlignment2 values.");
				}
			}
		}

		#endregion ==================================================================================

		#region Protected ===========================================================================

		/// <summary>
		/// Gets the object used to represent the double buffer for the presentation of the control.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Graphics"/> object used to double buffer the presentation of the control.</value>
		/// <remarks>
		/// The DoubleBuffered property must be set to <b>true</b> for this property to return a valid object. If the DoubleBuffered property is set to <b>false</b>, the default, then this property will return a null reference (Nothing in Visual Basic).
		/// </remarks>
		protected Graphics DoubleBuffer
		{
			get
			{
				return this.bufferGraphics;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the presentation of the control is double buffered.
		/// </summary>
		/// <value>A <see cref="T:System.Boolean"/> that is set to <b>true</b> if the presentation of the control is double buffered; otherwise, <b>false</b>. The default is <b>false</b>.</value>
		/// <remarks>
		/// See the example section of the OnPaint method for the recommended pattern to ensure that double buffering is accommodated.
		/// </remarks>
		protected bool DoubleBuffered
		{
			get
			{
				return doubleBuffered;
			}
			set
			{
				if (doubleBuffered != value)
				{
					doubleBuffered = value;
					if (doubleBuffered)
					{
						UpdateDoubleBuffer();
					}
					else
					{
						DisposeDoubleBuffer();
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the button control is the default button.
		/// </summary>
		/// <value><b>true</b> if the button control is the default button; otherwise, <b>false</b>.</value>
		protected bool IsDefault
		{
			get
			{
				return isDefault;
			}
			set
			{
				if (isDefault != value)
				{
					isDefault = value;
					this.Invalidate();
				}
			}
		}

		#endregion ==================================================================================

		#endregion ==================================================================================

		#region Methods =============================================================================

		/// <summary>
		/// Initializes the base information for a class derived from ButtonBase2.
		/// </summary>
		protected ButtonBase2() {	}

		/// <summary>
		/// Releases the unmanaged resources used by the ButtonBase2 and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing"><b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (!this.isDisposed)
			{
				try
				{
					if (disposing)
					{
						// Dispose managed resources.
						DisposeDoubleBuffer();
					}
					// Dispose unmanaged resources.
					this.isDisposed = true;
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		/// <summary>
		/// Clean up the objects used to represent the double buffer for the presentation of the control.
		/// </summary>
		private void DisposeDoubleBuffer()
		{
			if (this.bufferGraphics != null)
			{
				this.bufferGraphics.Dispose();
				this.bufferGraphics = null;
			}
			if (this.bufferBitmap != null)
			{
				this.bufferBitmap.Dispose();
				this.bufferBitmap = null;
			}
		}

		/// <summary>
		/// Draws the contents of the buffer to the control if the presentation was double buffered.
		/// </summary>
		/// <param name="controlGraphics">A <see cref="T:System.Drawing.Graphics"/> object representing the drawing surface of the control in which to output the contents of the buffer.</param>
		protected void DrawDoubleBuffer(Graphics controlGraphics)
		{
			if (this.DoubleBuffered)
			{
				controlGraphics.DrawImage(this.bufferBitmap, 0, 0);
			}
			else
			{
				throw (new InvalidOperationException("A call to this method is only valid when the DoubleBuffered property is set to true."));
			}
		}

		/// <summary>
		/// Gets a reference to the proper drawing medium used to update the presentation of the control.
		/// </summary>
		/// <param name="controlGraphics">A <see cref="T:System.Drawing.Graphics"/> object representing the drawing surface of the control.</param>
		/// <returns>A <see cref="T:System.Drawing.Graphics"/> object that should be used to update the presentation of the control.</returns>
		/// <remarks>
		/// The <see cref="T:System.Drawing.Graphics"/> object returned from this method will either be a reference to the buffer, if the DoubleBuffered property is set to <b>true</b>, or the reference to the drawing surface of the control that was provided as the argument.
		/// </remarks>
		protected Graphics GetPresentationMedium(Graphics controlGraphics)
		{
			if ((this.DoubleBuffer != null) && (this.DoubleBuffered))
			{
				return this.DoubleBuffer;
			}
			else
			{
				return controlGraphics;
			}
		}

		/// <summary>
		/// Determines if the specified sequential painting token was the last one registered.
		/// </summary>
		/// <param name="sequentialPaintingToken">A <see cref="T:System.Int32"/> that specifies the sequential painting token to compare with the last registered token.</param>
		/// <returns>A <see cref="T:System.Boolean"/> that is set to <b>true</b> if the specified sequential painting token was the last one registered; otherwise, <b>false</b>.</returns>
		/// <remarks>
		/// A control can register for sequential painting by calling the RegisterSequentialPainting method.
		/// </remarks>
		protected bool IsSequentialPaintingComplete(int sequentialPaintingToken)
		{
			return (this.registeredSequentialPaintingToken == sequentialPaintingToken);
		}

		/// <summary>
		/// Indicates that a certain class (generation) in the control hierarchy has completed updating the presentation.
		/// </summary>
		/// <param name="sequentialPaintingToken">A <see cref="T:System.Int32"/> that specifies the sequential painting token of the class (generation) that has completed updating the presentation.</param>
		/// <param name="args">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that references the original paint argument passed to the OnPaint method.</param>
		/// <remarks>
		/// If the sequential painting token argument represents the last token registered, this method will raise the Paint event, using the double buffer <see cref="T:System.Drawing.Graphics"/> object if the DoubleBuffered property is set to <b>true</b>, and will also draw the contents of the buffer to the control, if applicable.
		/// </remarks>
		protected void NotifyPaintingComplete(int sequentialPaintingToken, PaintEventArgs args)
		{
			if (IsSequentialPaintingComplete(sequentialPaintingToken))
			{
				// Raise the Paint event so that the end-developer will have a chance to update the presentation.
				RaisePaintEvent(args);
				// If the presentation was double buffered, draw the contents of the buffer to the control.
				if (this.DoubleBuffered)
				{
					DrawDoubleBuffer(args.Graphics);
				}
			}
		}

		/// <summary>
		/// Raises the KeyDown event.
		/// </summary>
		/// <param name="e">A KeyEventArgs that contains the event data.</param>
		protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
		{
			base.OnKeyDown(e);
			// If the end-developer hasn't explicitly handled the key, then simulate a Tab, or Shift+Tab, 
			// if the key represents a navigational key (Up, Down, Left, or Right).
			if (!e.Handled)
			{
				switch (e.KeyCode)
				{
					// Simulate a Shift+Tab key combination.
					case Keys.Up:
					case Keys.Left:
					{
						if (this.Parent != null)
						{
							this.Parent.SelectNextControl(this, false, true, true, true);
						}
						break;
					}
					// Simulate a Tab key.
					case Keys.Down:
					case Keys.Right:
					{
						if (this.Parent != null)
						{
							this.Parent.SelectNextControl(this, true, true, true, true);
						}
						break;
					}
				}
			}
		}

		#region Documentation =======================================================================
		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		/// <remarks>
		/// <b>Notes to Inheritors:</b> See the example section for the recommended pattern to ensure that double buffering and structured paint sequencing are both accommodated.
		/// </remarks>
		/// <example>
		/// <code>
		/// [C#]
		/// namespace MyCompanyName.TechnologyName
		/// {
		///   public class Button : OpenNETCF.Windows.Forms.ButtonBase2
		///   {
		///     private readonly int sequentialPaintingToken = Int32.MinValue;
		///
		///     public Button()
		///     {
		///       // Note: The RegisterSequentialPainting method should only be called if the OnPaint method, 
		///       // in this class, is to be overridden.
		///       // Register in the sequential painting process so that, primarily, if this class is the most 
		///       // derived class in the hierarchy that needs to update the presentation of the control, the 
		///       // Paint event may be delayed until all classes in the hierarchy have had a chance to update 
		///       // the presentation.
		///       this.sequentialPaintingToken = base.RegisterSequentialPainting();
		///
		///       // Note: Double buffering does not need to be enabled for sequential painting to take place.
		///       // Request that the presentation be buffered before being drawn to the control.
		///       base.DoubleBuffered = true;
		///     }
		///
		///     protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		///     {
		///       // Call the base classes OnPaint method to ensure that all appropriate base painting has been 
		///       // done prior to performing the presentation contribution of this class.
		///       base.OnPaint(e);
		///
		///       // Get a reference to the proper Graphics object used to update the presentation of the control.
		///       Graphics presentation = base.GetPresentationMedium(e.Graphics);
		///
		///       // ...
		///       // presentation.FillRectangle(Brush, X, Y, Width, Height);
		///       // ...
		///
		///       // Indicate that this class is done updating the presentation. If the sequential paint token 
		///       // for this class was the last one registered, then the Paint event will be triggered, and, 
		///       // if the presentation was double buffered, the contents of the buffer will be drawn to the 
		///       // control.
		///       base.NotifyPaintingComplete(this.sequentialPaintingToken, e);
		///     }
		///   }
		/// }
		/// </code>
		/// </example>
		#endregion ==================================================================================
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			// Note: Do not call the base classes OnPaint method in this method.

			// Get a reference to the proper Graphics object used to update the presentation of the control.
			Graphics presentation = GetPresentationMedium(e.Graphics);

			#region Presentation Logic ===============================================================

			// Paint a background for the control using the specified BackColor.
			SolidBrush backColorBrush = new SolidBrush(this.BackColor);
			presentation.FillRectangle(backColorBrush, 0, 0, this.Width, this.Height);
			backColorBrush.Dispose();

			#endregion ===============================================================================

			// Indicate that this class is done updating the presentation. If the sequential paint token 
			// for this class was the last one registered, then the Paint event will be triggered, and, 
			// if the presentation was double buffered, the contents of the buffer will be drawn to the 
			// control.
			NotifyPaintingComplete(sequentialPaintingToken, e);
		}

		/// <summary>
		/// Paints the background of the control.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains information about the control to paint.</param>
		/// <remarks>
		/// <b>Notes to Inheritors:</b> With the the hope of preventing noticeable flicker, this method has be overridden to do nothing. Therefore, all painting should be done in the OnPaint method.
		/// </remarks>
		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
		{
			// Do nothing with the hope of preventing noticeable flicker.
		}

		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnResize(System.EventArgs e)
		{
			if (this.DoubleBuffered)
			{
				UpdateDoubleBuffer();
			}
			this.Invalidate();
			base.OnResize(e);
		}

		/// <summary>
		/// Explicitly raises the Paint event with double buffer awareness.
		/// </summary>
		/// <param name="args">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that references the original paint argument passed to the OnPaint method.</param>
		/// <remarks>
		/// If the DoubleBuffered property is set to <b>true</b>, the double buffer <see cref="T:System.Drawing.Graphics"/> object will be passed through the event.
		/// </remarks>
		protected void RaisePaintEvent(PaintEventArgs args)
		{
			PaintEventArgs pea;
			// If double buffering is enabled, create a new PaintEventArgs object that references the 
			// double buffer Graphics object; otherwise, use the PaintEventArgs object that was passed 
			// into this method.
			if (this.DoubleBuffered)
			{
				pea = new PaintEventArgs(this.DoubleBuffer, args.ClipRectangle);
			}
			else
			{
				pea = args;
			}
			// Since this classes base class is System.Windows.Forms.Control, no painting is actually 
			// performed in the base classes OnPaint method. Instead, the base classes OnPaint method 
			// simply raises the Paint event, if necessary.
			base.OnPaint(pea);
		}

		/// <summary>
		/// Registers the caller in the sequential painting process by generating a token that the caller can use to identify its sequencing order.
		/// </summary>
		/// <returns>A <see cref="T:System.Int32"/> that represents a sequential painting token.</returns>
		/// <remarks>
		/// The token generated by this method should be stored by the caller and passed back to this class when calling methods such as NotifyPaintingComplete or IsSequentialPaintingComplete.
		/// </remarks>
		protected int RegisterSequentialPainting()
		{
			return (this.registeredSequentialPaintingToken += 1);
		}

		/// <summary>
		/// Resets the Image property to its default value.
		/// </summary>
		private void ResetImage()
		{
			this.Image = null;
		}

		/// <summary>
		/// Resets the ImageAlign property to its default value.
		/// </summary>
		private void ResetImageAlign()
		{
			this.ImageAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleCenter;
		}

		/// <summary>
		/// Resets the TextAlign property to its default value.
		/// </summary>
		private void ResetTextAlign()
		{
			this.TextAlign = OpenNETCF.Drawing.ContentAlignment2.MiddleCenter;
		}

		/// <summary>
		/// Indicates whether the Image property should be persisted.
		/// </summary>
		/// <returns><b>true</b> if the property value has changed from its default; otherwise, <b>false</b>.</returns>
		private bool ShouldSerializeImage()
		{
			return (this.image != null);
		}

		/// <summary>
		/// Indicates whether the ImageAlign property should be persisted.
		/// </summary>
		/// <returns><b>true</b> if the property value has changed from its default; otherwise, <b>false</b>.</returns>
		private bool ShouldSerializeImageAlign()
		{
			return (this.ImageAlign != OpenNETCF.Drawing.ContentAlignment2.MiddleCenter);
		}

		/// <summary>
		/// Indicates whether the TextAlign property should be persisted.
		/// </summary>
		/// <returns><b>true</b> if the property value has changed from its default; otherwise, <b>false</b>.</returns>
		private bool ShouldSerializeTextAlign()
		{
			return (this.TextAlign != OpenNETCF.Drawing.ContentAlignment2.MiddleCenter);
		}

		/// <summary>
		/// Creates, or recreates, the objects used to represent the double buffer for the presentation of the control.
		/// </summary>
		private void UpdateDoubleBuffer()
		{
			DisposeDoubleBuffer();
			if ((this.ClientRectangle.Width > 0) && (this.ClientRectangle.Height > 0))
			{
				this.bufferBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
				this.bufferGraphics = Graphics.FromImage(this.bufferBitmap);
			}
		}

		#endregion ==================================================================================
	}
}