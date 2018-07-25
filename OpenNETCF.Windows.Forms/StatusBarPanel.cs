
using System;
using System.ComponentModel;
using System.Drawing;

namespace OpenNETCF.Windows.Forms
{
	#region StatusBarPanelBorderStyle enum
	/// <summary>
	/// Specifies the appearance of the border for a <see cref="T:OpenNETCF.Windows.Forms.StatusBarPanel"/> on a <see cref="T:OpenNETCF.Windows.Forms.StatusBarEx"/> control.
	/// </summary>
	public enum StatusBarPanelBorderStyle 
	{
		/// <summary>
		/// Show the StatusBarPanel without any border.
		/// </summary>
		None,
		/// <summary>
		/// Show the StatusBarPanel in 3D with a raised border.
		/// </summary>
		Raised,
		/// <summary>
		/// Show the StatusBarPanel in 3D with a sunken border
		/// </summary>
		Sunken
	}
	#endregion

	#region StatusBarPanel class
	/// <summary>
	/// Represents a panel in a <see cref="T:OpenNETCF.Windows.Forms.StatusBarEx"/> control.
	/// </summary>

	#region design support
#if DESIGN
	    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
	    [DefaultProperty("Text")]
	    [DesignTimeVisible(false)]
		[ToolboxItemAttribute(false)]
#endif
	#endregion

	public class StatusBarPanel : Component
	{
		#region StatusBarPanel private data members
		private bool disposed;
		private int minWidth;	// in pixels
		private int width;		// in pixels
		internal StatusBarEx parent;
		private StatusBarPanelBorderStyle borderStyle;
		private string text;
		#endregion

		#region StatusBarPanel constructor
		/// <summary>
		/// Initializes a new instance of the StatusBarPanel class.
		/// </summary>
		public StatusBarPanel()
		{
			parent = null;
			disposed = false;
			minWidth = 10;
			width = 100;
			text = this.GetType().Name;
			borderStyle = StatusBarPanelBorderStyle.Sunken;
		}
		#endregion

		#region StatusBarPanel properties

		#region MinWidth property
#if DESIGN
		[DefaultValue(10), Category("Behavior")]
#endif
		/// <summary>
		/// Gets or sets the minimum allowed width of the status bar panel within the <see cref="T:OpenNETCF.Windows.Forms.StatusBarEx"/> control.
		/// </summary>
		/// <value>The minimum width (in pixels) of the StatusBarPanel.</value>
		public int MinWidth 
		{
			set 
			{
				if (value < 0) 
				{
					throw new ArgumentException("minWidth can not be less then zero");
				}
				minWidth = value;
			}
			get
			{
				return minWidth;
			}
		}
		#endregion

		#region Text property
		/// <summary>
		/// Gets or sets the text of the status bar panel.
		/// </summary>
		/// <value>The text displayed in the panel.</value>
		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
				parent.Invalidate();
			}
		}
		#endregion

		#region Width property
#if DESIGN
		[DefaultValue(100), Category("Appearance")]
#endif
		/// <summary>
		/// Gets or sets the width of the status bar panel within the StatusBarEx control.
		/// </summary>
		/// <value>The width (in pixels) of the StatusBarPanel.</value>
		public int Width
		{
			get
			{
				return width;
			}
			set
			{
				if (value < this.MinWidth) 
				{
					throw new ArgumentException("width can not be less then minWidth");
				}
				width = value;
				parent.Invalidate();
			}
		}
		#endregion

		#region BorderStyle property
#if DESIGN
		[DefaultValue(StatusBarPanelBorderStyle.Sunken), Category("Appearance")]
#endif
		/// <summary>
		/// Gets or sets the border style of the status bar panel.
		/// </summary>
		/// <value>One of the StatusBarPanelBorderStyle values. The default is Sunken.</value>
		public StatusBarPanelBorderStyle BorderStyle
		{
			get
			{
				return borderStyle;
			}
			set
			{
				if (System.Enum.IsDefined(typeof(StatusBarPanelBorderStyle), value) )
				{
					borderStyle = value;
					parent.Invalidate();
				} 
				else 
				{
					// TODO: Create a InvalidEnumArgumentException to be compliant to the .NET Framework.
					throw new ArgumentException("Illegal StatusBarPanelStyle enum value");
				}
			}
		}
		#endregion

		#region Parent property
#if DESIGN
		[Browsable(false)]
#endif
		/// <summary>
		/// Gets the <see cref="T:OpenNETCF.Windows.Forms.StatusBarEx"/> control that hosts the status bar panel.
		/// </summary>
		/// <value>The StatusBarEx object that contains the panel.</value>
		public object Parent
		{
			get
			{
				return parent;
			}
		}
		#endregion
		#endregion

		#region StatusBarPanel public members

		#region ToString method
		/// <summary>
		/// This member overrides Object.ToString.
		/// </summary>
		/// <returns>A String that represents the current Object.</returns>
		public override string ToString()
		{
			return "StatusBarPanel: {" + this.Text + "}";
		}
		#endregion

		#endregion

		#region StatusBarPanel protected members
		/// <summary>
		/// Releases the unmanaged resources used by the StatusBarPanel and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if(! this.disposed)
			{
				if(disposing)
				{
					this.parent = null;
				}
			}

			disposed = true;

			base.Dispose (disposing);
		}
		#endregion

	}
	#endregion
}
