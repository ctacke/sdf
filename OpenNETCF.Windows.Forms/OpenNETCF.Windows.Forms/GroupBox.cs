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
using System.Drawing;
using System.Windows.Forms;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Represents a container control used to group other controls.
	/// </summary>
    public class GroupBox : System.Windows.Forms.Panel, IWin32Window
	{
		#region Fields ==============================================================================

		private static readonly Font DefaultFontValue = new Font("Tahoma", 9.0F, FontStyle.Bold);

		// Public Property Correspondents
		private BorderStyle borderStyleValue = BorderStyle.FixedSingle;
		private Font fontValue = DefaultFontValue;

		#endregion ==================================================================================

		#region Properties ==========================================================================

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
				return borderStyleValue;
			}
			set
			{
				if (Enum.IsDefined(typeof(System.Windows.Forms.BorderStyle), value))
				{
					if (borderStyleValue != value)
					{
						// OnBorderStyleChanging(...)
						borderStyleValue = value;
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
		/// Gets or sets the font of the text displayed by the control.
		/// </summary>
		/// <value>The <see cref="T:System.Drawing.Font"/> of the text displayed by the control. The default is "Tahoma, 9pt, Bold".</value>
		public override Font Font
		{
			get
			{
				return fontValue;
			}
			set
			{
				if (fontValue != value)
				{
					// OnFontChanging(...)
					fontValue = value;
					this.Invalidate();
					// OnFontChanged(...)
				}
			}
		}
// need to disable warning VSD101 here, but can't figure out how
//#pragma warning disable 0101
        /// <summary>
		/// Gets or sets the foreground color of the control.
		/// </summary>
		/// <value>The <see cref="T:System.Drawing.Color"/> used as the foreground color for the control.</value>
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}
//#pragma warning restore 1691

		/// <summary>
		/// Gets or sets the text associated with the control.
		/// </summary>
		/// <value>The <see cref="T:System.String"/> containing the text associated with the control.</value>
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		#endregion ==================================================================================

		#region Methods =============================================================================

		/// <summary>
		/// Initializes a new instance of the GroupBox class.
		/// </summary>
		public GroupBox() {	}

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
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			int scaleFactor = (int)(e.Graphics.DpiX / 96);

			bool textAvailable = false;
			Font fontValue = this.Font;
			int lineOffset = 0;
			int offset = (8 * scaleFactor);
			int space = (4 * scaleFactor);
			int topOffset = 0;
			Rectangle textRect = Rectangle.Empty;
			SizeF textSize = SizeF.Empty;
			string textValue = this.Text;

			if ((textValue != null) && (fontValue != null))
			{
				if (textValue.Length > 0)
				{
					textAvailable = true;
					textSize = e.Graphics.MeasureString(textValue, fontValue);
					topOffset = (int)(textSize.Height / 2);
					textRect = new Rectangle((offset + space), 0, (this.ClientRectangle.Width - ((offset + space) * 2)), this.ClientRectangle.Height);
					lineOffset = Math.Min((textRect.X + textRect.Width), (offset + Convert.ToInt32(textSize.Width) + (space * 2)));
				}
			}

			#region Background =======================================================================

			using (SolidBrush backGroundBrush = new SolidBrush(this.BackColor))
			{
				e.Graphics.FillRectangle(backGroundBrush, 0, 0, this.Width, this.Height);
			}

			#endregion ===============================================================================

			#region Text =============================================================================

			if (textAvailable)
			{
				Color textColor;

				if (this.Enabled)
				{
					textColor = this.ForeColor;
				}
				else
				{
					textColor = SystemColors.GrayText;
				}

				using (SolidBrush textBrush = new SolidBrush(textColor))
				{
					e.Graphics.DrawString(textValue, fontValue, textBrush, textRect);
				}
			}

			#endregion ===============================================================================

			#region Border ===========================================================================

			switch (this.BorderStyle)
			{
				case BorderStyle.FixedSingle:
				{
					Color borderColor;

					if (this.Enabled)
					{
						borderColor = Color.Black;
					}
					else
					{
						borderColor = SystemColors.GrayText;
					}

					using (Pen borderPen = new Pen(borderColor, (1 * scaleFactor)))
					{
						int penOffset = (int)(borderPen.Width / 2);
						Size ctrlSize = this.ClientRectangle.Size;

						// Top border.
						if (textAvailable)
						{
							// Since the "right end cap" on thick lines seems to be odd, draw the line using a filled rectangle.
//							e.Graphics.DrawLine(borderPen, 0, (topOffset + penOffset), offset, (topOffset + penOffset));
							using (SolidBrush lineBrush = new SolidBrush(borderColor))
							{
								e.Graphics.FillRectangle(lineBrush, 0, topOffset, offset, (1 * scaleFactor));
							}
							e.Graphics.DrawLine(borderPen, lineOffset, (topOffset + penOffset), (ctrlSize.Width - ((penOffset == 0) ? 1 : penOffset)), (topOffset + penOffset));
						}
						else
						{
							e.Graphics.DrawLine(borderPen, 0, penOffset, (ctrlSize.Width - ((penOffset == 0) ? 1 : penOffset)), penOffset);
						}

						// Right border.
						e.Graphics.DrawLine(borderPen, (ctrlSize.Width - ((penOffset == 0) ? 1 : penOffset)), topOffset, (ctrlSize.Width - ((penOffset == 0) ? 1 : penOffset)), (ctrlSize.Height - 1));

						// Bottom border.
						e.Graphics.DrawLine(borderPen, 0, (ctrlSize.Height - ((penOffset == 0) ? 1 : penOffset)), (ctrlSize.Width - 1), (ctrlSize.Height - ((penOffset == 0) ? 1 : penOffset)));

						// Left border.
						e.Graphics.DrawLine(borderPen, penOffset, topOffset, penOffset, (ctrlSize.Height - 1));
					}

					break;
				}
				case BorderStyle.Fixed3D:
				{
					using (Pen border3DDarkPen = new Pen(SystemColors.ControlDark))
					{
						using (Pen border3DLightPen = new Pen(SystemColors.ControlLightLight))
						{
							Size ctrlSize = this.ClientRectangle.Size;

							// Top border.
							if (textAvailable)
							{
								// Dark line.
								e.Graphics.DrawLine(border3DDarkPen, 0, topOffset, offset, topOffset);
								e.Graphics.DrawLine(border3DDarkPen, lineOffset, topOffset, (ctrlSize.Width - 2), topOffset);
								// Light line.
								e.Graphics.DrawLine(border3DLightPen, 1, (topOffset + 1), offset, (topOffset + 1));
								e.Graphics.DrawLine(border3DLightPen, lineOffset, (topOffset + 1), (ctrlSize.Width - 3), (topOffset + 1));
							}
							else
							{
								// Dark line.
								e.Graphics.DrawLine(border3DDarkPen, 0, 0, (ctrlSize.Width - 2), 0);
								// Light line.
								e.Graphics.DrawLine(border3DLightPen, 1, 1, (ctrlSize.Width - 3), 1);
							}

							// Right border.
							e.Graphics.DrawLine(border3DDarkPen, (ctrlSize.Width - 2), topOffset, (ctrlSize.Width - 2), (ctrlSize.Height - 2));
							e.Graphics.DrawLine(border3DLightPen, (ctrlSize.Width - 1), topOffset, (ctrlSize.Width - 1), (ctrlSize.Height - 1));

							// Bottom border.
							e.Graphics.DrawLine(border3DDarkPen, 0, (ctrlSize.Height - 2), (ctrlSize.Width - 2), (ctrlSize.Height - 2));
							e.Graphics.DrawLine(border3DLightPen, 0, (ctrlSize.Height - 1), (ctrlSize.Width - 1), (ctrlSize.Height - 1));

							// Left border.
							e.Graphics.DrawLine(border3DDarkPen, 0, topOffset, 0, (ctrlSize.Height - 2));
							e.Graphics.DrawLine(border3DLightPen, 1, (topOffset + 1), 1, (ctrlSize.Height - 3));
						}
					}

					break;
				}
			}

			#endregion ===============================================================================

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
			this.Invalidate();
			base.OnResize(e);
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

		#endregion ==================================================================================
	}
}