
using System;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Windows.Forms;

#region designer suport
#if DESIGN
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
#endif

#if DESIGN && STANDALONE

[assembly: System.CF.Design.RuntimeAssembly
    ("StatusBarEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
]

#endif
#endregion

namespace OpenNETCF.Windows.Forms
{
	#region designer suport
#if DESIGN
		[Designer(typeof(System.Windows.Forms.Design.ControlDesigner))]
#endif
	#endregion

	#region StatusBarEx class

	#region designer support
#if DESIGN
		[CLSCompliant(false)]
		[DefaultProperty("Text")]
#endif
	#endregion

	/// <summary>
	/// Represents a Windows status bar control.
	/// </summary>
	public class StatusBarEx : Control
	{
		#region StatusBarPanelCollection class
		/// <summary>
		/// Represents the collection of panels in a StatusBarEx control.
		/// </summary>
		public class StatusBarPanelCollection : IList, ICollection, IEnumerable
		{
			#region StatusBarPanelCollection private data members
			private ArrayList panelList;
			private StatusBarEx parent;
			#endregion

			#region StatusBarPanelCollection Constructor
			/// <summary>
			/// Constructor for the StatusBarPanelCollection class
			/// </summary>
			/// <param name="parent">The StatusBarEx owning the collection of StatusBarPanels</param>
			public StatusBarPanelCollection(StatusBarEx parent)
			{
				panelList = new ArrayList();
				this.parent = parent;
			}
			#endregion

			#region StatusBarPanelCollection public methods
			/// <summary>
			/// Gets or sets the StatusBarPanel at the specified index.
			/// </summary>
			public virtual OpenNETCF.Windows.Forms.StatusBarPanel this[int index]
			{
				// This indexer is specifically present to get designer support using a collection editor.
				// The IList indexer is therefor sort of redundant but we leave it in for .NET compatibility.
				get
				{
					if (panelList.Count < index) 
					{
						throw new ArgumentOutOfRangeException("Index out of range");
					}
					return (OpenNETCF.Windows.Forms.StatusBarPanel)panelList[index];
				}
				set
				{
					panelList[index] = (OpenNETCF.Windows.Forms.StatusBarPanel)value;
				}
			}
			#endregion

			#region ICollection Members

			#region IsSynchronized property
			/// <summary>
			/// Gets a value indicating whether access to the ICollection is synchronized (thread-safe).
			/// This property is not implemented and returns always false right now.
			/// </summary>
			public bool IsSynchronized
			{
				get
				{
					return false;
				}
			}
			#endregion

			#region Count Property
			/// <summary>
			/// Gets the number of items in the collection.
			/// </summary>
			public int Count
			{
				get
				{
					return panelList.Count;
				}
			}
			#endregion

			#region CopyTo method
			/// <summary>
			/// When implemented by a class, copies the elements of the ICollection to an Array, starting at a particular Array index.
			/// </summary>
			/// <param name="array">The one-dimensional Array that is the destination of the elements copied from ICollection. The Array must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in array at which copying begins.</param>
			/// <remarks>StatusBarPanelCollection.CopyTo is not implemented and throws an exception!</remarks>
			public void CopyTo(Array array, int index)
			{
				throw new NotSupportedException("StatusBarPanelCollection.CopyTo method not supported");
			}
			#endregion

			#region SyncRoot method
			/// <summary>
			/// When implemented by a class, gets an object that can be used to synchronize access to the ICollection.
			/// StatusBarPanelCollection.SyncRoot is not implemented.
			/// </summary>
			public object SyncRoot
			{
				get
				{
					throw new NotSupportedException("StatusBarPanelCollection.SyncRoot property not supported");
				}
			}
			#endregion

			#endregion

			#region IEnumerable Members

			#region GetEnumerator method
			/// <summary>
			/// Returns an enumerator to use to iterate through the item collection.
			/// </summary>
			/// <returns>An IEnumerator object that represents the item collection.</returns>
			public IEnumerator GetEnumerator()
			{
				return panelList.GetEnumerator();
			}
			#endregion

			#endregion
		
			#region IList Members

			#region IsReadOnly property
			/// <summary>
			/// Gets a value indicating whether this collection is read-only.
			/// </summary>
			/// <remarks>
			/// This property is always false for this collection.
			/// </remarks>
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			#endregion

			#region StatusBarPanelCollection IList:Indexer
			/// <summary>
			/// Gets or sets the StatusBarPanel at the specified index.
			/// </summary>
			object IList.this[int index]
			{
				get
				{
					if (panelList.Count < index) 
					{
						throw new ArgumentOutOfRangeException("Index out of range");
					}
					return (StatusBarPanel)panelList[index];
				}
				set
				{
					if (value is StatusBarPanel) 
					{
						panelList[index] = (StatusBarPanel)value;
						parent.Invalidate();
					}
					else
					{
						throw new ArgumentException("object is not a valid StatusBarPanel");
					}
				}
			}
			#endregion

			#region RemoveAt method
			/// <summary>
			/// Removes the StatusBarPanel located at the specified index within the collection.
			/// </summary>
			/// <param name="index">The zero-based index of the item to remove.</param>
			/// <remarks>
			/// When you remove a panel from the collection, the indexes change for subsequent panels in the collection.
			/// All information about the removed panel is deleted. You can use this method to remove a specific panel 
			/// from the list by specifying the index of the panel to remove from the collection. To specify the panel 
			/// to remove instead of the index to the panel, use the Remove method. To remove all panels from the 
			/// StatusBarEx control, use the Clear method.
			/// </remarks>
			public void RemoveAt(int index)
			{
				if (index >= 0 && index < panelList.Count)
				{
					StatusBarPanel StatusBarPanel = (StatusBarPanel)panelList[index];
					panelList.RemoveAt(index);
					parent.Invalidate();
				} 
				else
				{
					throw new ArgumentOutOfRangeException("index");
				}
			}

			#endregion

			#region Insert methods
			/// <summary>
			/// Inserts the specified object as long as it is a valid StatusBarPanel into the collection at 
			/// the specified index.
			/// </summary>
			/// <param name="index">The zero-based index location where the panel is inserted.</param>
			/// <param name="value">A StatusBarPanel representing the panel to insert.</param>
			public void Insert(int index, object value)
			{
				if (! (value is StatusBarPanel)) 
				{
					throw new ArgumentException("object is not a valid StatusBarPanel");
				} 
				Insert(index, (StatusBarPanel)value);
				parent.Invalidate();
			}

			/// <summary>
			/// Inserts the specified StatusBarPanel into the collection at the specified index.
			/// </summary>
			/// <param name="index">The zero-based index location where the panel is inserted.</param>
			/// <param name="value">A StatusBarPanel representing the panel to insert.</param>
			public virtual void Insert(int index, StatusBarPanel value)
			{
				if (value == null) 
				{
					throw new ArgumentNullException();
				}
				if (index < 0 || index > panelList.Count) 
				{
					throw new ArgumentOutOfRangeException();
				}
				value.parent = this.parent;
				panelList.Insert(index, value);
				parent.Invalidate();
			}
			#endregion

			#region Remove methods
			/// <summary>
			/// Removes the specified object from the collection if it is a StatusBarPanel.
			/// </summary>
			/// <param name="value">An object representing the panel to remove from the collection.</param>
			public void Remove(object value)
			{
				if (! (value is StatusBarPanel)) 
				{
					throw new ArgumentException("object is not a valid StatusBarPanel");
				} 
				Remove((StatusBarPanel)value);
				parent.Invalidate();
			}

			/// <summary>
			/// Removes the specified StatusBarPanel from the collection.
			/// </summary>
			/// <param name="value">The StatusBarPanel representing the panel to remove from the collection.</param>
			public virtual void Remove (StatusBarPanel value)
			{
				if (value == null) 
				{
					throw new ArgumentNullException();
				}
				panelList.Remove(value);
				parent.Invalidate();
			}

			#endregion

			#region Contains methods
			/// <summary>
			/// Determines whether the specified object is a StatusBarPanel and if it is located within the collection.
			/// </summary>
			/// <param name="value">The object to locate in the collection.</param>
			/// <returns>true if the object is located within the collection; otherwise, false.</returns>
			public bool Contains(object value)
			{
				if (! (value is StatusBarPanel)) 
				{
					throw new ArgumentException("object is not a valid StatusBarPanel");
				} 
				return Contains((StatusBarPanel)value);
			}


			/// <summary>
			/// Determines whether the specified panel is located within the collection.
			/// </summary>
			/// <param name="panel">The StatusBarPanel to locate in the collection.</param>
			/// <returns>true if the panel is located within the collection; otherwise, false.</returns>
			public bool Contains(StatusBarPanel panel)
			{
				return panelList.Contains(panel);
			}

			#endregion

			#region Clear method
			/// <summary>
			/// Removes all items from the collection.
			/// </summary>
			/// <remarks>
			/// To remove a single panel from the StatusBarEx, use the Remove or RemoveAt method.
			/// </remarks>
			public void Clear()
			{
				panelList.Clear();
				parent.Invalidate();
			}

			#endregion

			#region IndexOf methods
			/// <summary>
			/// Returns the index within the collection of the specified object if it is a StatusBarPanel.
			/// </summary>
			/// <param name="value">The object to locate in the collection.</param>
			/// <returns>The zero-based index where the panel is located within the collection; otherwise, negative one (-1).</returns>
			public int IndexOf(object value)
			{
				if (! (value is StatusBarPanel)) 
				{
					throw new ArgumentException("object is not a valid StatusBarPanel");
				} 
				return IndexOf((StatusBarPanel)value);
			}

			/// <summary>
			/// Returns the index within the collection of the specified panel.
			/// </summary>
			/// <param name="value">The StatusBarPanel object to locate in the collection.</param>
			/// <returns>The zero-based index where the panel is located within the collection; otherwise, negative one (-1).</returns>
			public int IndexOf(StatusBarPanel value)
			{
				return panelList.IndexOf(value);
			}
			#endregion

			#region Add methods
			/// <summary>
			/// Adds an object to the collection if it is a StatusBarPanel.
			/// </summary>
			/// <param name="value">A StatusBarPanel that represents the panel to add to the collection.</param>
			/// <returns>The zero-based index of the item in the collection.</returns>
			public int Add(object value)
			{
				if (! (value is StatusBarPanel)) 
				{
					throw new ArgumentException("object is not a valid StatusBarPanel");
				}
				return Add(value as StatusBarPanel);
			}

			/// <summary>
			/// Adds a StatusBarPanel to the collection.
			/// </summary>
			/// <param name="value">A StatusBarPanel that represents the panel to add to the collection.</param>
			/// <returns>The zero-based index of the item in the collection.</returns>
			public virtual int Add(StatusBarPanel value)
			{
				if (value == null) 
				{
					throw new ArgumentNullException("StatusBarPanel", "no valid object passed");
				}
				StatusBarPanel statusBarPanel = value;
				statusBarPanel.parent = parent;
				parent.Invalidate();
				return panelList.Add(value);
			}

			/// <summary>
			/// Adds a StatusBarPanel with the specified text to the collection.
			/// </summary>
			/// <param name="text">The text for the StatusBarPanel that is being added.</param>
			/// <returns>A StatusBarPanel that represents the panel that was added to the collection.</returns>
			public virtual StatusBarPanel Add(string text)
			{
				StatusBarPanel statusBarPanel = new StatusBarPanel();
				statusBarPanel.Text = text;
				parent.Invalidate();
				panelList.Add(statusBarPanel);
				return statusBarPanel;
			}

			#endregion

			#region IsFixedSize property
			/// <summary>
			/// Gets a value indicating whether this collection is fixed-sized.
			/// </summary>
			/// <remarks>
			/// This property is always false for this collection.
			/// </remarks>
			public bool IsFixedSize
			{
				get
				{
					return false;
				}
			}
			#endregion

			#endregion
		}
		#endregion

		#region StatusBarEx private data members
		private bool showPanels;
		private StatusBarPanelCollection panels;
		#endregion

		#region StatusBarEx constructor
		/// <summary>
		/// Constructor of the StatusBarEx control.
		/// </summary>
		public StatusBarEx()
		{
			Text = GetType().Name;
			showPanels = false;   // don't show different StatusBarPanels initially
			panels = new StatusBarPanelCollection(this);
			panels.Clear();
		}
		#endregion

		#region StatusBarEx properties

		#region Panels property
#if DESIGN
		[Category("Appearance"), Description("The panels in the StatusBarEx.")]
		//[EditorAttribute(typeof(System.ComponentModel.Design.CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
#endif
		/// <summary>
		/// Gets the collection of <see cref="T:OpenNETCF.Windows.Forms.StatusBarEx"/> panels contained within the control.
		/// </summary>
		/// <value>A StatusBarEx.StatusBarPanelCollection containing the StatusBarPanel objects of the StatusBar control.</value>
		public StatusBarPanelCollection Panels
		{
			get
			{
				return panels;
			}
		}
		#endregion

		#region ShowPanels property
#if DESIGN
		[DefaultValue(false), Category("Behavior")]
		[Description("Determines if a StatusBarEx displays panels, of if it displays a single line of text.")]
#endif
		/// <summary>
		/// Gets or sets a value indicating whether any panels that have been added to the control are displayed.
		/// </summary>
		/// <value>true if panels are displayed; otherwise, false. The default is false.</value>
		public bool ShowPanels
		{
			get
			{
				return showPanels;
			}
			set
			{
				showPanels = value;
				Invalidate();
			}
		}
		#endregion

		#region BackColor property
		/// <summary>
		/// Gets or sets the background color for the StatusBarEx control.
		/// </summary>
		/// <value>A System.Drawing.Color that represents the background color of the control.</value>
#if DESIGN
		[CLSCompliant(false)]
#endif
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				Invalidate();
			}
		}
		#endregion

		#region Text property
		/// <summary>
		/// Gets or sets the text associated with this StatusBarEx control.
		/// </summary>
		/// <value>The text associated with the StatusBarEx control.</value>
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				Invalidate();
			}
		}
		#endregion

		#region Location property
		/// <summary>
		/// Gets or sets the coordinates of the upper-left corner of the StatusBarEx control relative to the 
		/// upper-left corner of its container.
		/// </summary>
		/// <value>The Point that represents the upper-left corner of the control relative to the upper-left corner of its container.</value>
#if DESIGN
		[CLSCompliant(false)]
#endif
		public new Point Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				base.Location = value;
				Invalidate();
			}
		}
		#endregion

		#region Size property
		/// <summary>
		/// Gets or sets the height and width of the StatusBarEx control.
		/// </summary>
		/// <value>The Size object that represents the height and width of the control in pixels.</value>
#if DESIGN
		[CLSCompliant(false)]
#endif
		public new Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
				Invalidate();
			}
		}
		#endregion

		#endregion

		#region StatusBarEx public members

		#region ToString method
		/// <summary>
		/// This member overrides Object.ToString.
		/// </summary>
		/// <returns>A String that represents the current Object.</returns>
		public override string ToString()
		{
			StringBuilder strBuilder = new StringBuilder();
			strBuilder.Append(base.ToString());
			strBuilder.Append(", Panels.Count: ");
			strBuilder.Append(Panels.Count.ToString());
			for (int i = 0; i < Panels.Count; i++) 
			{
				strBuilder.Append(", Panels[");
				strBuilder.Append(i.ToString());
				strBuilder.Append("]: ");
				strBuilder.Append(Panels[i].ToString());
			}
			return strBuilder.ToString();
		}
		#endregion

		#endregion

		#region StatusBarEx protected members

		#region StatusBarEx.OnResize method
		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnResize(EventArgs e)
		{
			// TODO: Resizing the form during run time & design time should "dock" the StatusBarEx
			//   NB: During design time we can truly dock, because we are running desktop .NET then
			base.OnResize (e);
		}
		#endregion

		#region StatusBarEx.OnPaint method
		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
#if DESIGN
		[CLSCompliant(false)]
#endif
		protected override void OnPaint(PaintEventArgs e)
		{
#if ! DESIGN
			// If the StatusBarEx's Visible Property == false we should not draw it during runtime.
			// However, during design time it is useful to draw, so we see where the StatusBarEx is located.
			if (! this.Visible)
			{
				return;
			}
#endif
			// Initialize the StatusBarEx to fit exactly in the container's client area.
			// Note that we are always "docking" to the bottom of the client area of the container.
			// TODO: Find a proper location for this modification of the bar.
			this.Width = this.Parent.ClientSize.Width;
			this.Top = this.Parent.ClientRectangle.Height - this.Height;
			this.Left = this.Parent.ClientRectangle.Left;

			// create a brushes to display texts and backgrounds
			SolidBrush textBrush = new SolidBrush(this.ForeColor);
			SolidBrush backGroundBrush = new SolidBrush(this.BackColor);

			// and some pens to show information
			Pen blackPen = new Pen(Color.Black);
			Pen whitePen = new Pen(Color.White);

			Graphics gfx = e.Graphics;
			Rectangle rect = this.ClientRectangle;
			gfx.FillRectangle(backGroundBrush, rect);

			if (this.Panels.Count == 0 || this.ShowPanels == false) 
			{
				gfx.DrawString(this.Text, this.Font, textBrush, (float)4, (float)2);
			} 
			else if (this.Panels.Count > 0)
			{
				int x = 2; // starting location of first panel
				foreach (StatusBarPanel sbp in this.Panels) 
				{
					Rectangle StatusBarPanelRect = new Rectangle(x, rect.Y, sbp.Width, rect.Height);
					gfx.FillRectangle(backGroundBrush, StatusBarPanelRect);
					
					if (sbp.BorderStyle == StatusBarPanelBorderStyle.Raised) 
					{
						gfx.DrawLine(blackPen, StatusBarPanelRect.Left, StatusBarPanelRect.Bottom, StatusBarPanelRect.Right, StatusBarPanelRect.Bottom);
						gfx.DrawLine(blackPen, StatusBarPanelRect.Right, StatusBarPanelRect.Bottom-1, StatusBarPanelRect.Right, StatusBarPanelRect.Top);
						gfx.DrawLine(whitePen, StatusBarPanelRect.Left, StatusBarPanelRect.Top, StatusBarPanelRect.Right-1, StatusBarPanelRect.Top);
						gfx.DrawLine(whitePen, StatusBarPanelRect.Left, StatusBarPanelRect.Bottom, StatusBarPanelRect.Left, StatusBarPanelRect.Top);
					}

					if (sbp.BorderStyle == StatusBarPanelBorderStyle.Sunken) 
					{ 
						gfx.DrawLine(whitePen, StatusBarPanelRect.Left, StatusBarPanelRect.Bottom, StatusBarPanelRect.Right, StatusBarPanelRect.Bottom);
						gfx.DrawLine(whitePen, StatusBarPanelRect.Right, StatusBarPanelRect.Bottom-1, StatusBarPanelRect.Right, StatusBarPanelRect.Top);
						gfx.DrawLine(blackPen, StatusBarPanelRect.Left, StatusBarPanelRect.Top, StatusBarPanelRect.Right-1, StatusBarPanelRect.Top);
						gfx.DrawLine(blackPen, StatusBarPanelRect.Left, StatusBarPanelRect.Bottom, StatusBarPanelRect.Left, StatusBarPanelRect.Top);
					}

					gfx.DrawString(sbp.Text, this.Font, textBrush, (float)(StatusBarPanelRect.X+2), (float)2);
					x += (sbp.Width+2);
				}
			}

			textBrush.Dispose();
			backGroundBrush.Dispose();
			whitePen.Dispose();
			blackPen.Dispose();
		}
		#endregion

		#endregion

	}
	#endregion
}
