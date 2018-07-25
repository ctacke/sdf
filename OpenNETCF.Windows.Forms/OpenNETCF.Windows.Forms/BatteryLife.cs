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
	/// Represents a control that indicates the percentage of remaining battery life.
	/// </summary>
	/// <remarks>This control will not work as expected on the emulator since, in the current release, battery life is not emulated.</remarks>
	/// <example>
	/// <code>
	/// [C#]
	/// public class Form1 : System.Windows.Forms.Form
	/// {
	///	private OpenNETCF.Windows.Forms.BatteryLife batteryLife1;
	///	private System.Windows.Forms.Button button1;
	///
	///	public Form1()
	///	{
	///		batteryLife1 = new OpenNETCF.Windows.Forms.BatteryLife();
	///		button1 = new System.Windows.Forms.Button();
	///		button1.Click += new System.EventHandler(button1_Click);
	///	}
	///
	///	private void button1_Click(object sender, EventArgs e)
	///	{
	///		// Call the UpdateBatteryLife method when the percentage bar should be refreshed with the latest battery life status.
	///		batteryLife1.UpdateBatteryLife();
	///	}
	/// }
	/// </code>
	/// </example>
    public class BatteryLife : System.Windows.Forms.Control, IWin32Window
	{
		#region Fields ==============================================================================

		private bool disposed = false;
		private byte batteryLifePercentCache = 0;

		// Double Buffer
		private Bitmap bufferBitmap = null;
		private Graphics bufferGraphics = null;

		// Public Property Correspondents
		private Color borderColorValue = Color.Black;
		private Color percentageBarColorValue = SystemColors.Highlight;
		private PowerStatus powerStatusValue = new PowerStatus();

		#endregion ==================================================================================

		#region Properties ==========================================================================

		/// <summary>
		/// Gets or sets the color of the border for the control.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Color" /> value that represents the border color of the control. The default is Color.Black.</value>
		public Color BorderColor
		{
			get
			{
				return borderColorValue;
			}
			set
			{
				if (borderColorValue != value)
				{
					borderColorValue = value;
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets or sets the color used to display the percentage of remaining battery life.
		/// </summary>
		/// <value>A <see cref="T:System.Drawing.Color" /> value that represents the color used to display the percentage of remaining battery life. The default is SystemColors.Highlight.</value>
		public Color PercentageBarColor
		{
			get
			{
				return percentageBarColorValue;
			}
			set
			{
				if (percentageBarColorValue != value)
				{
					percentageBarColorValue = value;
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets the system power status information.
		/// </summary>
		/// <value>A <see cref="T:OpenNETCF.Windows.Forms.PowerStatus" /> that represents the system power status information.</value>
		public PowerStatus PowerStatus
		{
			get
			{
				return powerStatusValue;
			}
		}

		#endregion ==================================================================================

		#region Methods =============================================================================

		/// <summary>
		/// Initializes a new instance of the BatteryLife class.
		/// </summary>
		public BatteryLife()
		{
			this.BackColor = SystemColors.Control;
			this.ForeColor = SystemColors.HighlightText;
			this.Size = new Size(152, 20);
			this.TabStop = false;
			this.UpdateBatteryLife();
		}

		/// <summary>
		/// Allows an instance of the BatteryLife class to attempt to free resources and perform other cleanup operations.
		/// </summary>
		~BatteryLife()
		{
			Dispose(false);
		}

		/// <summary>
		/// Releases all resources used by the BatteryLife instance.
		/// </summary>
		public new void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases the unmanaged resources used by the BatteryLife instance and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing"><b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				try
				{
					if (disposing)
					{
						// Dispose managed resources.
						this.bufferBitmap.Dispose();
						this.bufferGraphics.Dispose();
					}
					// Dispose unmanaged resources.
					this.disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			int scaleFactor = (int)(e.Graphics.DpiX / 96);
			int borderWidth = (1 * scaleFactor);
			int padding = (1 * scaleFactor);

			#region Background =======================================================================

			using (SolidBrush backColorBrush = new SolidBrush(this.BackColor))
			{
				this.bufferGraphics.FillRectangle(backColorBrush, 0, 0, this.Width, this.Height);
			}

			#endregion ===============================================================================

			#region Content ==========================================================================

			byte batteryPercent;
			string batteryPercentText;
			Rectangle contextRect;

			if (Environment.OSVersion.Platform == PlatformID.WinCE)
			{
				batteryPercent = (this.batteryLifePercentCache > 100) ? (byte)100 : this.batteryLifePercentCache;
			}
			else
			{
				batteryPercent = 75;
			}

			batteryPercentText = batteryPercent.ToString() + " %";

			// Deflate the client rectangle to allow for a border and padding.
			contextRect = this.ClientRectangle;
			contextRect.Inflate(-(borderWidth + padding), -(borderWidth + padding));

			// Draw the percentage bar representing the battery percentage remaining.
			using (SolidBrush percentageBarBrush = new SolidBrush(this.PercentageBarColor))
			{
				this.bufferGraphics.FillRectangle(percentageBarBrush, contextRect.X, contextRect.Y, Convert.ToInt32(contextRect.Width * (Convert.ToSingle(batteryPercent) / 100)), contextRect.Height);
			}

			// Draw the text representing the battery percentage remaining.
			using (SolidBrush foreColorBrush = new SolidBrush(this.ForeColor))
			{
				using (StringFormat format = new StringFormat())
				{
					format.Alignment = StringAlignment.Center;
					format.FormatFlags = StringFormatFlags.NoWrap;
					format.LineAlignment = StringAlignment.Center;
					this.bufferGraphics.DrawString(batteryPercentText, this.Font, foreColorBrush, contextRect, format);
				}
			}

			#endregion ===============================================================================

			#region Border ===========================================================================

			using (Pen borderColorPen = new Pen(this.BorderColor, borderWidth))
			{
				int penOffset = (borderWidth / 2);
				this.bufferGraphics.DrawRectangle(borderColorPen, penOffset, penOffset, (this.ClientRectangle.Width - (penOffset * 2) - 1), (this.ClientRectangle.Height - (penOffset * 2) - 1));
			}

			#endregion ===============================================================================

			e.Graphics.DrawImage(this.bufferBitmap, 0, 0);

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
			if (this.bufferGraphics != null)
			{
				this.bufferGraphics.Dispose();
			}

			if (this.bufferBitmap != null)
			{
				this.bufferBitmap.Dispose();
			}

			this.bufferBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
			this.bufferGraphics = Graphics.FromImage(this.bufferBitmap);

			this.Invalidate();

			base.OnResize(e);
		}

		/// <summary>
		/// Updates the display that represents the percentage of remaining battery life.
		/// </summary>
		/// <remarks>In addition to updating the display this method also refreshes the value of the PowerStatus property.</remarks>
		public void UpdateBatteryLife()
		{
			if (Environment.OSVersion.Platform == PlatformID.WinCE)
			{
				this.batteryLifePercentCache = this.PowerStatus.BatteryLifePercent;
			}
			this.Invalidate();
		}

		#endregion ==================================================================================
	}
}