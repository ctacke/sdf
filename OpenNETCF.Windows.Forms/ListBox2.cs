using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;


namespace OpenNETCF.Windows.Forms
{
	/// <summary>
    /// Represents a Owner-drawn list control, which displays a collection of items.
	/// </summary>
	public class ListBox2 : OwnerDrawnList
	{

		#region private members
		const int DRAW_OFFSET  = 4;
		private ImageList imageList = null;
		private bool wrapText = false;
		ImageAttributes imageAttr = new ImageAttributes();

		private ItemCollection itemList;

		//private ArrayList arrList;

		private Color colorEvenItem;
		private Brush evenItemBrush;
		private Color lineColor;
		private Pen penLine;

		private bool showLines;

		#endregion

		/// <summary>
		/// Gets the items of the ListBox2.
		/// </summary>
		//Editor(typeof(ItemCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))
		public ItemCollection Items
		{
			get
			{
				return itemList;
			}
		}

		/// <summary>
		/// Gets a items collection.
		/// </summary>
		protected override IList BaseItems
		{
			get
			{
				if (itemList == null)
					itemList = new ItemCollection(this);
				return itemList;
			}
		}

		#region public properties
		/// <summary>
		/// Gets or sets the background color of the even item.  
		/// </summary>
		public Color EvenItemColor
		{
			get { return colorEvenItem; }
			set 
			{ 
				if (colorEvenItem != value)
				{
					colorEvenItem = value; 
					if (evenItemBrush != null)
						evenItemBrush.Dispose();

					evenItemBrush = new SolidBrush(value);

					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets or sets the color of the lines. 
		/// </summary>
		public Color LineColor
		{
			get { return lineColor; }
			set 
			{ 
				if (lineColor != value)
				{
					lineColor = value; 
					if (penLine != null)
						penLine.Dispose();

					penLine = new Pen(value);

					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets or sets text wrapping in the list items
		/// </summary>
		public bool ShowLines
		{
			get
			{
				return showLines;
			}
			set
			{
				showLines = value;
			}
		}

		/// <summary>
		/// Gets or sets text wrapping in the list items
		/// </summary>
		public bool WrapText
		{
			get
			{
				return wrapText;
			}
			set
			{
				wrapText = value;
				
				//this.ItemHeight = GetItemHeight();
				
			}
		}

		/// <summary>
		/// Gets or sets the System.Windows.Forms.ImageList to use when displaying item's icons in the control.  
		/// </summary>
		public ImageList ImageList
		{
			get
			{
				return imageList;
			}
			set
			{
				imageList = value;

				for(int i = 0; i < this.Items.Count; i++) 
				{
					this.Items[i].imageList = this.imageList;
				}

				//reset the ItemHeight
				if (imageList != null)
					this.ItemHeight = Math.Max(imageList.ImageSize.Height + 2, this.ItemHeight);
				else
					this.ItemHeight = Math.Max(GetItemHeight(), this.ItemHeight);

			}
		}


		#endregion

        #region constructor

        /// <summary>
		/// Default constructor
		/// </summary>
		public ListBox2()
		{
			this.ShowScrollbar = true;
			this.ForeColor = SystemColors.ControlText;
			this.EvenItemColor = SystemColors.Control;
			this.LineColor = SystemColors.ControlText;
			this.BackColor = SystemColors.Window;
			showLines = true;
			
			this.Width = 100;
			this.Height = 100;
			//Set the item's height
			using(Graphics g = this.CreateGraphics())
			{
				this.ItemHeight = Math.Max((int)(g.MeasureString("A", this.Font).Height), this.ItemHeight);

			}
			
			if (itemList == null)
				itemList = new ItemCollection(this);

        }

        #endregion


		#region base overrides

		/// <summary>
		/// Gets or sets the item height
		/// </summary>
#if DESIGN
		[Category("Appearance"),
		Description("Specifies the height of an individual item.")]
#endif
		public override int ItemHeight
		{
			get
			{
				return base.ItemHeight;
			}
			set
			{
				base.ItemHeight = value;
			}
		}


		protected override void OnDrawItem(object sender, DrawItemEventArgs e)
		{
            if (this.DrawMode == DrawMode.Normal)
            {
                Brush textBrush; //Brush for the text
                ListItem item;

                Rectangle rc = e.Bounds;
                rc.X += DRAW_OFFSET;


                //Get the ListItem
                if ((e.Index > -1) && (e.Index < itemList.Count))
                    item = (ListItem)itemList[e.Index];
                else
                    return;


                //Check if the item is selected
                if (e.State == DrawItemState.Selected)
                {
                    //Highlighted
                    e.DrawBackground();
                    textBrush = new SolidBrush(SystemColors.HighlightText);
                }
                else
                {

                    //Change the background for every even item
                    if ((e.Index % 2) == 0)
                    {
                        e.DrawBackground(colorEvenItem);
                    }
                    else
                    {
                        e.DrawBackground(this.BackColor);
                    }


                    if (this.BackgroundImage != null) // don't do back ground if there is back image
                    {
                        Rectangle rcImage = Rectangle.Empty;
                        if (showLines)
                            rcImage = new Rectangle(0, rc.Y + 1, this.Width, rc.Height - 1);
                        else
                            rcImage = new Rectangle(0, rc.Y, this.Width, rc.Height);

                        e.Graphics.DrawImage(BackgroundImage, 0, rc.Y + 1, rcImage, GraphicsUnit.Pixel);
                    }

                    textBrush = new SolidBrush(item.ForeColor);
                }


                //			//Check if the item has a image
                if ((item.ImageIndex > -1) && (imageList != null))
                {
                    Image img = imageList.Images[item.ImageIndex];
                    if (img != null)
                    {
                        imageAttr = new ImageAttributes();
                        //Set the transparency key
                        imageAttr.SetColorKey(BackgroundImageColor(img), BackgroundImageColor(img));
                        //Image's rectangle
                        Rectangle imgRect = new Rectangle(2, rc.Y + 2, img.Width, img.Height);
                        //Draw the image
                        e.Graphics.DrawImage(img, imgRect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imageAttr);
                        //Shift the text to the right
                        rc.X += img.Width + 4;
                    }
                }

                //Draw item's text
                if (wrapText)
                    e.Graphics.DrawString(item.Text, item.Font, textBrush, rc);
                else
                {
                    //center the text vertically
                    SizeF size = e.Graphics.MeasureString(item.Text, item.Font);
                    Rectangle rcText = new Rectangle(rc.X, rc.Y + (rc.Height - (int)size.Height) / 2, rc.Width, (int)size.Height);
                    e.Graphics.DrawString(item.Text, item.Font, textBrush, rcText);
                }

                if (textBrush != null)
                    textBrush.Dispose();

                //Draw the line
                if (showLines)
                    e.Graphics.DrawLine(penLine, 0, e.Bounds.Bottom, e.Bounds.Width, e.Bounds.Bottom);

            }
            else
            {
                //Call the base's OnDrawEvent	
                base.OnDrawItem(sender, e);
            }
		}
		#endregion

		#region helper functions

        internal ListItem AddItem(ListItem item)
        {
            itemList.Add(item);
            this.Invalidate();
            return item;
        }

		//helper function
		private Color BackgroundImageColor(Image image)
		{
			Bitmap bmp = new Bitmap(image);
			Color ret = bmp.GetPixel(0, 0);
			return ret;
		}

		private int GetItemHeight()
		{
			using(Graphics g = this.CreateGraphics())
			{
				if (wrapText)
					return 2 * Math.Max((int)(g.MeasureString("A", this.Font).Height), this.ItemHeight);
				else
					return Math.Max((int)(g.MeasureString("A", this.Font).Height), this.ItemHeight);

			}
		}

		#endregion

		protected override object BuildItemForRow(object row)
		{
			//jsm - Bug 150 - We're seeing that the DataSource is getting out of sync with the itemList by one; we need to prevent this
			if (this.DataSource is IList && ((IList)this.DataSource).Count < itemList.Count + 1)
				return null;

			string itemText = "";
			if ((DisplayMember != null) && (DisplayMember != ""))
				itemText = m_properties[DisplayMember].GetValue(row).ToString();
			else
				itemText = row.ToString();

			return itemList.Add(itemText);
		}

		#region ItemCollection
		/// <summary>
		/// Represents the collection of items in a ListBox2. 
		/// </summary>
		public class ItemCollection : CollectionBase
		{
			
			ListBox2 parent = null;

            public ItemCollection()
            {


            }

			internal ItemCollection(ListBox2 parent):base()
			{
				this.parent = parent;

			}

			/// <summary>
			/// Adds an item to the list of items for a ListBox2. 
			/// </summary>
			/// <param name="value">ListItem to add</param>
			/// <returns>Newly created ListItem</returns>
			public ListItem Add(ListItem value)
			{
				// Use base class to process actual collection operation
				//				if (value.Font == null)
				value.Font = parent.Font;
				//				if (value.ForeColor == Color.Empty)
				value.ForeColor = parent.ForeColor;
				//
				value.Parent = parent;
				//parent.arrList.Add(value);
				int i = List.Add(value as ListItem);
				if (parent != null)
					parent.Refresh();

				//return i;
				return value;
			}

			/// <summary>
			/// Adds an item to the list of items for a ListBox2 
			/// </summary>
			/// <param name="value">string for text property</param>
			/// <returns>Newly created ListItem</returns>
			public ListItem Add(string value)
			{
				// Use base class to process actual collection operation
				ListItem item = new ListItem(value);
				item.Parent = parent;
				item.Font = parent.Font;
				item.ForeColor = parent.ForeColor;
				if (!List.Contains(item))
					List.Add(item);
				
				//MessageBox.Show("Items.Add");
				//parent.arrList.Add(value);
				parent.Refresh();

				return item;
			}



			//			public void AddRange(ListItem[] values)
			//			{
			//				// Use existing method to add each array entry
			//				foreach(ListItem page in values)
			//					Add(page);
			//
			//				parent.RefreshItems();
			//			}

			/// <summary>
			/// Removes the specified object from the collection.
			/// </summary>
			/// <param name="value">ListItem to remove</param>
			public void Remove(ListItem value)
			{
				List.Remove(value);
				parent.Refresh();
			}

			/// <summary>
			/// Inserts an item into the list box at the specified index.  
			/// </summary>
			/// <param name="index">The zero-based index location where the item is inserted.</param>
			/// <param name="value">An object representing the item to insert.</param>
			public void Insert(int index, ListItem value)
			{
				// Use base class to process actual collection operation
				List.Insert(index, value as object);
				parent.Refresh();
			}

			/// <summary>
			/// Determines whether the specified item is located within the collection.  
			/// </summary>
			/// <param name="value">An object representing the item to locate in the collection.</param>
			/// <returns>true if the item is located within the collection; otherwise, false .</returns>
			public bool Contains(ListItem value)
			{
				// Use base class to process actual collection operation
				//return base.List.Contains(value as object);

				return List.Contains(value);
				//return parent.arrList.Contains(value as object);
			}

			/// <summary>
			/// Gets or sets the item.
			/// </summary>
			public ListItem this[int index]
			{
				// Use base class to process actual collection operation
				get 
				{ 
					ListItem item = (ListItem)List[index];
					//ListItem item = parent.arrList[index] as ListItem;
					return item; 
				}
				set
				{
					List[index] = (ListItem)value;

				}
			}

			/// <summary>
			/// Removes all elements from the System.Collections.ArrayList.  
			/// </summary>
			public new void Clear()
			{
				base.Clear();
				parent.Refresh();

			}

			/// <summary>
			/// Gets the item.
			/// </summary>
			public ListItem this[string text]
			{
				get 
				{
					// Search for a Page with a matching title
					foreach(ListItem page in List)
						if (page.Text == text)
							return page;

					return null;
				}
			}

			//			public new int Count
			//			{
			//				get
			//				{
			//					return parent.arrList.Count;
			//				}
			//
			//			}

			/// <summary>
			/// Returns the index within the collection of the specified item
			/// </summary>
			/// <param name="value">An object representing the item to locate in the collection.</param>
			/// <returns>The zero-based index where the item is located within the collection; otherwise, negative one (-1). </returns>
			public int IndexOf(ListItem value)
			{
				// Find the 0 based index of the requested entry
				return List.IndexOf(value);
			}
		}
		#endregion
	}
	
}
