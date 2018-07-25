using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Represents a label control that displays as a hyperlink.
	/// </summary>
    public class LinkLabel2 : System.Windows.Forms.Control, OpenNETCF.Windows.Forms.IButtonControl, IWin32Window
	{
		#region Fields ==============================================================================

		private const int focusBorderWidth = 1;
		private const int padding = 1;

		private Color currentLinkColor;

		// Public Property Correspondents
		private Color activeLinkColorValue = Color.Red;
		private bool autoSizeValue = false;
		private DialogResult dialogResultValue = DialogResult.None;
		private Color disabledLinkColorValue = SystemColors.GrayText;
		private LinkBehavior linkBehaviorValue = LinkBehavior.AlwaysUnderline;
		private Color linkColorValue = Color.Blue;
		private object linkDataValue = null;
		private bool linkVisitedValue = false;
		private Color visitedLinkColorValue = Color.Purple;

		#endregion ==================================================================================

		#region Properties ==========================================================================

		/// <summary>
		/// Gets or sets the color of the text in an active state.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Color"/> that represents the color of the text in an active state. The default is Color.Red.</value>
		public Color ActiveLinkColor
		{
			get
			{
				return activeLinkColorValue;
			}
			set
			{
				if (activeLinkColorValue != value)
				{
					// OnActiveLinkColorChanging(...)
					activeLinkColorValue = value;
					// OnActiveLinkColorChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the control is automatically resized to display its contents.
		/// </summary>
		/// <value>A <see cref="T:System.Boolean"/> that is set to <b>true</b> if the control is automatically resized; otherwise, <b>false</b>. The default is <b>false</b>.</value>
		public bool AutoSize
		{
			get
			{
				return autoSizeValue;
			}
			set
			{
				if (autoSizeValue != value)
				{
					// OnAutoSizeChanging(...)
					autoSizeValue = value;
					AdjustControlSize();
					// OnAutoSizeChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that is returned to the parent form when the link is activated.
		/// </summary>
		/// <value>One of the <see cref="T:System.Windows.Forms.DialogResult"/> values. The default value is DialogResult.None.</value>
		public virtual DialogResult DialogResult
		{
			get
			{
				return dialogResultValue;
			}
			set
			{
				if (Enum.IsDefined(typeof(System.Windows.Forms.DialogResult), value))
				{
					if (dialogResultValue != value)
					{
						// OnDialogResultChanging(...)
						dialogResultValue = value;
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
		/// Gets or sets the color of the text in a disabled state.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Color"/> that represents the color of the text in a disabled state. The default is SystemColors.GrayText.</value>
		public Color DisabledLinkColor
		{
			get
			{
				return disabledLinkColorValue;
			}
			set
			{
				if (disabledLinkColorValue != value)
				{
					// OnDisabledLinkColorChanging(...)
					disabledLinkColorValue = value;
					if (!this.Enabled)
					{
						this.currentLinkColor = disabledLinkColorValue;
						this.Invalidate();
					}
					// OnDisabledLinkColorChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets the font of the text displayed by the control.
		/// </summary>
		/// <value>The <see cref="T:System.Drawing.Font"/> of the text displayed by the control.</value>
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				AdjustControlSize();
			}
		}

		/// <summary>
		/// Gets or sets a value that represents the behavior of the link.
		/// </summary>
		/// <value>A <see cref="T:OpenNETCF.Windows.Forms.LinkBehavior"/> value that represents the behavior of the link. The default is LinkBehavior.AlwaysUnderline.</value>
		public LinkBehavior LinkBehavior
		{
			get
			{
				return linkBehaviorValue;
			}
			set
			{
				if (Enum.IsDefined(typeof(OpenNETCF.Windows.Forms.LinkBehavior), value))
				{
					if (linkBehaviorValue != value)
					{
						// OnLinkBehaviorChanging(...)
						linkBehaviorValue = value;
						this.Invalidate();
						// OnLinkBehaviorChanged(...)
					}
				}
				else
				{
					throw new OpenNETCF.ComponentModel.InvalidEnumArgumentException("The LinkBehavior property must be one of the OpenNETCF.Windows.Forms.LinkBehavior values.");
				}
			}
		}

		/// <summary>
		/// Gets or sets the color of the text in a normal state.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Color"/> that represents the color of the text in a normal state. The default is Color.Blue.</value>
		public Color LinkColor
		{
			get
			{
				return linkColorValue;
			}
			set
			{
				if (linkColorValue != value)
				{
					// OnLinkColorChanging(...)
					linkColorValue = value;
					if ((this.Enabled) && (!this.LinkVisited))
					{
						this.currentLinkColor = linkColorValue;
						this.Invalidate();
					}
					// OnLinkColorChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets the data associated with the link.
		/// </summary>
		/// <value>A <see cref="T:System.Object"/> that represents the data associated with the link. The default is a null reference (Nothing in Visual Basic).</value>
		public object LinkData
		{
			get
			{
				return linkDataValue;
			}
			set
			{
				if (linkDataValue != value)
				{
					// OnLinkDataChanging(...)
					linkDataValue = value;
					// OnLinkDataChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the link should be displayed as though it were visited.
		/// </summary>
		/// <value>A <see cref="T:System.Boolean"/> that is set to <b>true</b> if the link should be displayed as though it were visited; otherwise, <b>false</b>. The default is <b>false</b>.</value>
		/// <remarks>
		/// This control does not automatically denote that a link is a visited link. 
		/// To display the link as a visited link, you can set the value of this property to <b>true</b> 
		/// in an event handler for the LinkClicked event.
		/// </remarks>
		public bool LinkVisited
		{
			get
			{
				return linkVisitedValue;
			}
			set
			{
				if (linkVisitedValue != value)
				{
					// OnLinkVisitedChanging(...)
					linkVisitedValue = value;
					if (linkVisitedValue)
					{
						this.currentLinkColor = this.VisitedLinkColor;
					}
					else
					{
						if (this.Enabled)
						{
							this.currentLinkColor = this.LinkColor;
						}
						else
						{
							this.currentLinkColor = this.DisabledLinkColor;
						}
					}
					this.Invalidate();
					// OnLinkVisitedChanged(...)
				}
			}
		}

		/// <summary>
		/// Gets or sets the color used to indicate that the link has been previously visited.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Color"/> that represents the color used to indicate that the link has been previously visited. The default is Color.Purple.</value>
		public Color VisitedLinkColor
		{
			get
			{
				return visitedLinkColorValue;
			}
			set
			{
				if (visitedLinkColorValue != value)
				{
					// OnVisitedLinkColorChanging(...)
					visitedLinkColorValue = value;
					if (this.LinkVisited)
					{
						this.currentLinkColor = visitedLinkColorValue;
						this.Invalidate();
					}
					// OnVisitedLinkColorChanged(...)
				}
			}
		}

		#endregion ==================================================================================

		#region Methods =============================================================================

		/// <summary>
		/// Initializes a new instance of the LinkLabel2 class.
		/// </summary>
		public LinkLabel2()
		{
			this.currentLinkColor = this.LinkColor;
			this.Size = new Size(72, 20);
		}

		/// <summary>
		/// Adjusts the size of the control based on the AutoSize property.
		/// </summary>
		private void AdjustControlSize()
		{
			if (this.AutoSize)
			{
				using (Graphics g = this.CreateGraphics())
				{
					int scaleFactor = (int)(g.DpiX / 96);
					SizeF size = g.MeasureString(this.Text, this.Font);
					this.Bounds = new Rectangle(this.Left, this.Top, (Convert.ToInt32(size.Width) + (((focusBorderWidth + padding) * 2) * scaleFactor)), (Convert.ToInt32(size.Height) + (((focusBorderWidth + padding) * 2) * scaleFactor)));
				}
			}
		}

		/// <summary>
		/// Notifies the link whether it is the default link so that it can adjust its appearance accordingly.
		/// </summary>
		/// <param name="value"><b>true</b> if the link is to have the appearance of the default link; otherwise, <b>false</b>.</param>
		/// <remarks>
		/// A LinkLabel2 control appears the same regardless of whether it is the default, so 
		/// calling this method will not change the appearance or behavior of the LinkLabel2.
		/// </remarks>
		public virtual void NotifyDefault(bool value) {	}

		/// <summary>
		/// Raises the Click event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnClick(System.EventArgs e)
		{
			// Raise the LinkClicked event.
			LinkLabel2LinkClickedEventArgs args = new LinkLabel2LinkClickedEventArgs(this.LinkData);
			OnLinkClicked(args);
			// If applicable, indicate to the Form, through the Forms DialogResult property, that the 
			// control was selected.
			if ((this.DialogResult != DialogResult.None) && (this.TopLevelControl is System.Windows.Forms.Form))
			{
				((System.Windows.Forms.Form)this.TopLevelControl).DialogResult = this.DialogResult;
			}
			base.OnClick(e);
		}

		/// <summary>
		/// Raises the EnabledChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnEnabledChanged(System.EventArgs e)
		{
			if (this.Enabled)
			{
				if (!this.LinkVisited)
				{
					this.currentLinkColor = this.LinkColor;
				}
				else
				{
					this.currentLinkColor = this.VisitedLinkColor;
				}
			}
			else
			{
				this.currentLinkColor = this.DisabledLinkColor;
			}
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
					case (int)Keys.Enter:
					case (int)Keys.Space:
					{
						this.PerformClick();
						break;
					}
				}
			}
		}

		/// <summary>
		/// Raises the LinkClicked event.
		/// </summary>
		/// <param name="e">A LinkLabel2LinkClickedEventArgs that contains the event data.</param>
		protected virtual void OnLinkClicked(LinkLabel2LinkClickedEventArgs e)
		{
			if (LinkClicked != null)
			{
				LinkClicked(this, e);
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
			// that the control is focused and then invalidate.
			if (e.Button == MouseButtons.Left)
			{
				this.currentLinkColor = this.ActiveLinkColor;
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
		/// Raises the MouseUp event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			// If the user has selected the control, by indication of the left mouse button, then invalidate.
			if (e.Button == MouseButtons.Left)
			{
				if (!this.LinkVisited)
				{
					this.currentLinkColor = this.LinkColor;
				}
				else
				{
					this.currentLinkColor = this.VisitedLinkColor;
				}
				this.Invalidate();
			}
			base.OnMouseUp(e);
		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			int scaleFactor = (int)(e.Graphics.DpiX / 96);

			// Paint the background.
			using (SolidBrush backColorBrush = new SolidBrush(this.BackColor))
			{
				e.Graphics.FillRectangle(backColorBrush, 0, 0, this.Width, this.Height);
			}

			// Create the font used to paint the text.
			bool fontCreated = false;
			Font font = this.Font;
			switch (this.LinkBehavior)
			{
				case LinkBehavior.AlwaysUnderline:
				{
					font = new Font(font.Name, font.Size, (font.Style | FontStyle.Underline));
					fontCreated = true;
					break;
				}
				case LinkBehavior.NeverUnderline:
				{
					font = new Font(font.Name, font.Size, (font.Style & (~FontStyle.Underline)));
					fontCreated = true;
					break;
				}
			}

			// Paint the text.
			using (SolidBrush foreColorBrush = new SolidBrush(this.currentLinkColor))
			{
				// Deflate the client rectangle to allow for a focus border and padding.
				Rectangle textRect = this.ClientRectangle;
				textRect.Inflate(-((focusBorderWidth + padding) * scaleFactor), -((focusBorderWidth + padding) * scaleFactor));
				// Paint the text.
				e.Graphics.DrawString(this.Text, font, foreColorBrush, textRect);
			}

			// Dispose of the font if it was created in this method.
			if (fontCreated)
			{
				font.Dispose();
			}

			// Paint the focus border, if applicable.
			if (this.Focused)
			{
				using (Pen focusPen = new Pen(SystemColors.Highlight, (focusBorderWidth * scaleFactor)))
				{
					int penOffset = (int)(focusPen.Width / 2);
					e.Graphics.DrawRectangle(focusPen, penOffset, penOffset, (this.ClientRectangle.Width - (penOffset * 2) - 1), (this.ClientRectangle.Height - (penOffset * 2) - 1));
				}
			}

			base.OnPaint(e);
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
			AdjustControlSize();
			this.Invalidate();
			base.OnResize(e);
		}

		/// <summary>
		/// Raises the TextChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnTextChanged(System.EventArgs e)
		{
			AdjustControlSize();
			this.Invalidate();
			base.OnTextChanged(e);
		}

		/// <summary>
		/// Generates a Click event for the link.
		/// </summary>
		public virtual void PerformClick()
		{
			if ((this.Enabled) && (this.Visible))
			{
				this.OnClick(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Resets the LinkBehavior property to its default value.
		/// </summary>
		private void ResetLinkBehavior()
		{
			this.LinkBehavior = LinkBehavior.AlwaysUnderline;
		}

		/// <summary>
		/// Indicates whether the LinkBehavior property should be persisted.
		/// </summary>
		/// <returns><b>true</b> if the property value has changed from its default; otherwise, <b>false</b>.</returns>
		private bool ShouldSerializeLinkBehavior()
		{
			return (this.LinkBehavior != LinkBehavior.AlwaysUnderline);
		}

		#endregion ==================================================================================

		#region Events ==============================================================================

		/// <summary>
		/// Occurs when the link is clicked.
		/// </summary>
		public event LinkLabel2LinkClickedEventHandler LinkClicked;

		#endregion ==================================================================================
	}

	#region Support ================================================================================

	/// <summary>
	/// Specifies the behavior of a link in a <see cref="T:OpenNETCF.Windows.Forms.LinkLabel2"/>.
	/// </summary>
	public enum LinkBehavior
	{
		/// <summary>
		/// The link always displays with underlined text.
		/// </summary>
		AlwaysUnderline = 1,

		/// <summary>
		/// The link text is never underlined. The link can still be distinguished from other text by use of the LinkColor property of the <see cref="T:OpenNETCF.Windows.Forms.LinkLabel2"/> control.
		/// </summary>
		NeverUnderline = 3
	}

	/// <summary>
	/// Provides data for the LinkClicked event of the <see cref="T:OpenNETCF.Windows.Forms.LinkLabel2"/> control.
	/// </summary>
	public class LinkLabel2LinkClickedEventArgs : System.EventArgs
	{
		private object linkDataValue;

		/// <summary>
		/// Gets the data associated with the link.
		/// </summary>
		public object LinkData
		{
			get
			{
				return linkDataValue;
			}
		}

		/// <summary>
		/// Initializes a new instance of the LinkLabel2LinkClickedEventArgs class, given the link data.
		/// </summary>
		/// <param name="data">The LinkData of the <see cref="T:OpenNETCF.Windows.Forms.LinkLabel2"/> instance.</param>
		public LinkLabel2LinkClickedEventArgs(object data)
		{
			this.linkDataValue = data;
		}
	}

	/// <summary>
	/// Represents the method that will handle the LinkClicked event of a <see cref="T:OpenNETCF.Windows.Forms.LinkLabel2"/>.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">A <see cref="T:OpenNETCF.Windows.Forms.LinkLabel2LinkClickedEventArgs"/> that contains the event data.</param>
	public delegate void LinkLabel2LinkClickedEventHandler(object sender, LinkLabel2LinkClickedEventArgs e);

	#endregion =====================================================================================

	#region Design-Time ============================================================================

	// TODO: The designer needs to be placed into a full framework 2.0 project, built, and then the 
	// resulting assembly needs to be installed in the GAC. The designer can then be applied to the 
	// LinkLabel2 control through the associated xmta file, and referenced by the VS 2005 visual 
	// designer.

	//public class LinkLabel2Designer : Microsoft.CompactFramework.Design.DeviceControlDesigner
	//{
	//   public override SelectionRules SelectionRules
	//   {
	//      get
	//      {
	//         SelectionRules selectionRules = base.SelectionRules;
	//         PropertyDescriptor propDescriptor = TypeDescriptor.GetProperties(this.Component)["AutoSize"];
	//         if (propDescriptor != null)
	//         {
	//            bool autoSize = (bool)propDescriptor.GetValue(this.Component);
	//            if (autoSize)
	//            {
	//               selectionRules = (SelectionRules.Visible | SelectionRules.Moveable);
	//            }
	//            else
	//            {
	//               selectionRules = (SelectionRules.Visible | SelectionRules.AllSizeable | SelectionRules.Moveable);
	//            }
	//         }
	//         return selectionRules;
	//      }
	//   }
	//
	//   public LinkLabel2Designer() { }
	//}

	#endregion =====================================================================================
}