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
using System.Collections;
using System.Drawing.Imaging;
using System.ComponentModel;

using OpenNETCF.Windows.Forms;


namespace OpenNETCF.Windows.Forms
{
    /// <summary>
    /// Represents a Owner-drawn list control, which displays a collection of items which can be filtered using key press.
    /// </summary>
    public class SmartList : OwnerDrawnList, IWin32Window
    {
        #region events
        /// <summary>
		/// Occurs when the filtering is started.
		/// </summary>
		public event EventHandler FilteringStarted;

		/// <summary>
		/// Occurs when the filtering is completed.
		/// </summary>
		public event EventHandler FilteringComplete;

        #endregion


        #region private members

        const int DRAW_OFFSET  = 4;
		private ImageList imageList = null;
		private bool wrapText = false;
		ImageAttributes imageAttr = new ImageAttributes();

		private ItemCollection itemList;

		private Color colorEvenItem;
		private Brush evenItemBrush;
		private Color lineColor;
		private Pen penLine;

		private bool showLines;
		private bool autoNumbering;
		private int selected = 0;

		private Hashtable numToLetter;
		private ArrayList searchBuffer = new ArrayList();
		private ArrayList arrSearch = new ArrayList();
		private ArrayList virtItems = new ArrayList();

		private ItemCollection savedItems = new ItemCollection();
		private ItemCollection tempItems;
		private bool bTempCleaned;
		private int itemsMatched = 0;

		private bool fullKeyboard = false;

		#endregion

		/// <summary>
		/// Gets the items of the SmartList .
		/// </summary>

		//Editor(typeof(ItemCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))
		

		#region public properties

        public ItemCollection Items
        {
            get
            {
                return itemList;
            }
            set
            {
                itemList = value;
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

        /// <summary>
        /// Sets or gets the value indicating usage of QWERTY keyboard
        /// </summary>
		public bool FullKeyboard
		{
			get
			{
				return fullKeyboard;
			}
			set
			{
				fullKeyboard = value;
			}
		}

		/// <summary>
		/// Sets or gets AutoNumbering.
		/// </summary>
		public bool AutoNumbering
		{
			get
			{
				return autoNumbering;
			}
			set
			{
				autoNumbering = value;
				if (autoNumbering)
				{
					itemList.Sorted = false;
					selected = 0;
				}
				else
					itemList.Sorted = true;

			}
		}

		/// <summary>
		/// Gets or sets key mappings for filtering.
		/// </summary>
		public Hashtable KeyMappings
		{
			get
			{
				return numToLetter;
			}
		}

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
				
				this.ItemHeight = GetItemHeight();
				
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

		/// <summary>
		/// Gets a number of items found as a result of filtering.
		/// </summary>
		public int ItemsMatchedCount
		{
			get
			{
				return itemsMatched;
			}

		}

		#endregion

		/// <summary>
		/// Default constructor
		/// </summary>
		public SmartList()
		{
			this.ShowScrollbar = true;
			this.ForeColor = SystemColors.ControlText;
			this.EvenItemColor = SystemColors.Control;
			this.LineColor = SystemColors.ControlText;
			this.BackColor = SystemColors.Window;
			showLines = true;
			tempItems = new ItemCollection(this);
			//Set the item's height
			using(Graphics g = this.CreateGraphics())
			{
				this.ItemHeight = Math.Max((int)(g.MeasureString("A", this.Font).Height), this.ItemHeight);

			}
			
			if (itemList == null)
				itemList = new ItemCollection(this);


            this.Width = 100;
            this.Height = 100;
			autoNumbering = false;
			
			InitNumberToLetterMappings();
			
		}

		
		internal SmartListItem AddItem(SmartListItem item)
		{
			itemList.Add(item);
			this.Invalidate();
			return item;
		}


		//		/// <summary>
		//		/// Gets or sets a number of charachters highlighted in the item.
		//		/// </summary>
		//		public int CharHighlight
		//		{
		//			get
		//			{
		//				return selected;
		//			}
		//			set
		//			{
		//				selected = value;
		//			}
		//		}

#if DESIGN
		internal bool ShouldSerializeFont() 
		{
			return Font != null;
		}
#endif


		#region base overrides

		/// <summary>
		/// Gets or sets the item height
		/// </summary>
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
                SmartListItem item;

                Rectangle rc = e.Bounds;
                int autoNum = (int)rc.Y / rc.Height + 1;

                //int autoNum = e.Index + 1;

                rc.X += DRAW_OFFSET;

                //Get the SmartListItem
                if ((e.Index > -1) && (e.Index < itemList.Count))
                    item = (SmartListItem)itemList[e.Index];
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


                    if (this.BackgroundImage != null) // don't do background if there is back image
                    {
                        Rectangle rcImage = Rectangle.Empty;
                        if (showLines)
                            rcImage = new Rectangle(0, rc.Y + 1, this.Width, rc.Height - 1);
                        else
                            rcImage = new Rectangle(0, rc.Y, this.Width, rc.Height);

                        e.Graphics.DrawImage(BackgroundImage, 1, rc.Y + 1, rcImage, GraphicsUnit.Pixel);
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

                    }

                    textBrush = new SolidBrush(item.ForeColor);
                }

                int numberShift = 0;
                SizeF numSize = SizeF.Empty;

                if (autoNumbering)
                {
                    numSize = e.Graphics.MeasureString("22", item.Font);
                    numberShift = (int)numSize.Width + 4;

                }

                // Check if the item has a image
                if ((item.ImageIndex > -1) && (imageList != null))
                {
                    Image img = imageList.Images[item.ImageIndex];
                    if (img != null)
                    {
                        imageAttr = new ImageAttributes();
                        //Set the transparency key
                        imageAttr.SetColorKey(BackgroundImageColor(img), BackgroundImageColor(img));
                        //Image's rectangle
                        Rectangle imgRect = Rectangle.Empty;

                        imgRect = new Rectangle(2 + numberShift, rc.Y + 2, img.Width, img.Height);
                        //Draw the image
                        e.Graphics.DrawImage(img, imgRect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imageAttr);
                        //Shift the text to the right
                        rc.X += img.Width + 2;
                    }
                }
                //			

                if (autoNumbering)
                {
                    e.Graphics.DrawString(autoNum.ToString(), item.Font, textBrush, 6, rc.Top + (rc.Height - (int)numSize.Height) / 2);
                    rc.X += numberShift;
                }
                //Draw item's text
                if (wrapText)
                    e.Graphics.DrawString(item.Text, item.Font, textBrush, rc);
                else
                {
                    //center the text vertically
                    SizeF size = e.Graphics.MeasureString(item.Text, item.Font);
                    Rectangle rcText = Rectangle.Empty;


                    rcText = new Rectangle(rc.X, rc.Y + (rc.Height - (int)size.Height) / 2, rc.Width - rc.X - 3, (int)size.Height);

                    string itemText = ShortString(item, e, rcText.Width);


                    if (selected > 0)
                        DrawTextSelected(e.Graphics, item, itemText, textBrush, 0, selected, rcText, e.State);
                    else
                        e.Graphics.DrawString(itemText, item.Font, textBrush, rcText);

                }

                //Draw the line
                if (showLines)
                    e.Graphics.DrawLine(penLine, 0, e.Bounds.Bottom, e.Bounds.Width, e.Bounds.Bottom);

                textBrush.Dispose();
            }
            else
            {
                //Call the base's OnDrawEvent	
                base.OnDrawItem(sender, e);
            }
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress (e);
			char ch = Convert.ToChar(13); // Enter\Action
			char chBack; // = Convert.ToChar(27); //Back key


			// Do nothing if the action key is pressed
			if (e.KeyChar == ch)
				return;
			
			// handle AutoNumbering
			if (autoNumbering)
			{
				//try to find the item
				int pressedKey = Convert.ToInt32(e.KeyChar.ToString());
				this.SelectedIndex = pressedKey - 1;
				return;
			}
			
			string strLet = String.Empty;

			//letters from the pressed key
			if (!fullKeyboard)
			{
				chBack = Convert.ToChar(27);
				strLet = (string)numToLetter[e.KeyChar];
			}
			else
			{
				chBack = (char)Keys.Back;
				strLet = e.KeyChar.ToString();
			}
			


			if (savedItems.Count == 0)
			{
				savedItems = this.Items.CloneItems();
			}

			string strSearch = "";

			// Initialize helper arrays
			ArrayList tempArrSearch = (ArrayList)arrSearch.Clone();
			ItemCollection filterItems = this.Items.CloneItems();

			bTempCleaned = false;

			#region Back key

			if (e.KeyChar == chBack) //back key
			{
				if (searchBuffer.Count == 0)
					return;
				else
					e.Handled = true;

				arrSearch.Clear();

				this.BeginUpdate();
				
				OnFilteringStarted(null);

				if (searchBuffer.Count > 1)
				{
					ArrayList tmpSearch = (ArrayList)searchBuffer[searchBuffer.Count - 2];

					for(int n=0;n<tmpSearch.Count;n++)
					{
						strSearch = (string)tmpSearch[n];
						if (strSearch.Length >= 1)
						{
							if (FilterItems(strSearch, savedItems))
								arrSearch.Add(strSearch);
						}
					}
					searchBuffer.RemoveAt(searchBuffer.Count - 1);
				}
				else
				{
					searchBuffer.Clear();
					PopulateItems(savedItems, 0);
					tempItems.Clear();

				}

				OnFilteringComplete(null);


				if (tempItems.Count > 0)
					PopulateItems(tempItems, strSearch.Length);

				itemsMatched = tempItems.Count;

				this.EndUpdate();
				this.EnsureVisible(0);
				
				return;
			}

			#endregion

			if (strLet == null)
				return;

			this.BeginUpdate();

			arrSearch.Clear();

			OnFilteringStarted(null);

			tempItems = ItemSearch(strLet, tempArrSearch);

			OnFilteringComplete(null);

			if (arrSearch.Count > 0)
				searchBuffer.Add(arrSearch.Clone());
			else
			{
				//itemsMatched = 0;
				//				string nomatch = "No matches:";
				//				this.Items.Clear();
				//				this.selected = 0;
				//				this.Items.Add(new SmartListItem(nomatch));
				//				this.Invalidate();
				//				return;
				//tempItems.Clear();
				//tempItems.Add("No matches:");

			}


			if (tempItems.Count > 0)
				PopulateItems(tempItems, searchBuffer.Count);

			itemsMatched = tempItems.Count;

			
			this.EndUpdate();
			this.EnsureVisible(0);
			
		}

		#endregion

		#region public virtal methods
		/// <summary>
		/// Raises the FilteringStarted event.  
		/// </summary>
		/// <param name="e"> An System.EventArgs that contains the event data.</param>
		protected virtual void OnFilteringStarted(EventArgs e)
		{
			if (FilteringStarted != null)
			{
				FilteringStarted(this, e);
			}
		}

		/// <summary>
		/// Raises the FilteringStarted event.
		/// </summary>
		/// <param name="e">An System.EventArgs that contains the event data.</param>
		protected virtual void OnFilteringComplete(EventArgs e)
		{
			if (FilteringComplete != null)
			{
				FilteringComplete(this, e);
			}
		}

		#endregion

		#region helper functions

		// Search for the items
		private ItemCollection ItemSearch(string strLetters, ArrayList letterList)
		{
			string strSearch = "";

			ItemCollection newItems = new ItemCollection();

			for(int i=0;i<strLetters.Length;i++)
			{
				if (letterList.Count > 0)
				{
					for(int n=0;n<letterList.Count;n++)
					{
						strSearch = (string)letterList[n];
						strSearch += strLetters[i]; //   .Substring(i, 1); 
						if (FilterItems(strSearch, this.Items, newItems))
						{
							arrSearch.Add(strSearch);	
						}
					}
				}
				else if (searchBuffer.Count == 0)
				{
					//searchBuffer.Clear();
					//strSearch = strLetters.Substring(i, 1);
					strSearch = strLetters[i].ToString();
					if (FilterItems(strSearch,  this.Items, newItems))
					{
						arrSearch.Add(strSearch);
					}	
				}
			}

			return newItems;
		}


		private bool FilterItems(string value, ItemCollection items, ItemCollection filteredItems)
		{
			bool result = false;

			//ItemCollection filteredItems = new ItemCollection();
			
			for(int i=0;i<items.Count;i++)
			{
				string item = (string)items[i].Text;
				string val = item.Substring(0, value.Length).ToLower();
				
				if (String.Compare(val, value, true) == 0)
				{
					filteredItems.Add(items[i]);

					result = true;
				}

				//result = true;
			}

			return result;
		}


		private bool FilterItems(string value, ItemCollection items)
		{
			bool res = false;
			
			for(int i=0;i<items.Count;i++)
			{
				string item = (string)items[i].Text;
				string val = item.Substring(0, value.Length).ToLower();
				
				if (String.Compare(val, value, true) == 0)
				{
					if (!bTempCleaned)
					{
						tempItems.Clear();
						bTempCleaned = true;
					}
					//SmartListItem SmartListItem = new SmartListItem(item);
					tempItems.Add(items[i]);
					res = true;
				}
			}

			return res;
		}



		private bool FilterItems(string value, ArrayList items)
		{
			bool res = false;
			
			for(int i=0;i<items.Count;i++)
			{
				string item = (string)items[i];
				string val = item.Substring(0, value.Length).ToLower();
				
				if (String.Compare(val, value, true) == 0)
				{
					if (!bTempCleaned)
					{
						tempItems.Clear();
						bTempCleaned = true;
					}
					SmartListItem SmartListItem = new SmartListItem(item);
					tempItems.Add(SmartListItem);
					res = true;
				}
			}

			return res;
		}

		private int FindListItem(string value)
		{
			int result = 0;

			Array searchArray = this.Items.ToArray();
			int index = Array.BinarySearch(searchArray, 0, this.Items.Count, value, new Comparer());
			if (index > 0)
			{
				result = index;
			}
			else
			{
				result = ~index;
			}

			return result;
		}

		
		private void PopulateItems(ItemCollection list , int selected)
		{
			Cursor.Current = Cursors.WaitCursor;

			this.BeginUpdate();
			//lstCountry.Items.Clear();
			this.selected = selected;

			this.Items = list;

			this.EnsureVisible(0);
			this.SelectedIndex = 0;

			this.EndUpdate();
			this.Invalidate();
	
			Cursor.Current = Cursors.Default;
		}

		// Populate phone keys translation
		private void InitNumberToLetterMappings()
		{
			numToLetter = new Hashtable();			
			numToLetter.Add('2', "abc");
			numToLetter.Add('3', "def");
			numToLetter.Add('4', "ghi");
			numToLetter.Add('5', "jkl");
			numToLetter.Add('6', "mno");
			numToLetter.Add('7', "prs");
			numToLetter.Add('8', "tuv");
			numToLetter.Add('9', "wxyz");
		}

		// This methos will shorten a long string to the desired width and add "..."
		private string ShortString(SmartListItem item, DrawItemEventArgs e, int desWidth)
		{
			SizeF size = e.Graphics.MeasureString(item.Text, e.Font);
			SizeF p_size = e.Graphics.MeasureString("...", e.Font);

			string tempString = item.Text;

			if (size.Width > desWidth)
			{
				//get the new width
				int nWidth = desWidth - (int)p_size.Width;
				//get the desired string
				char[] textChars = item.Text.ToCharArray();
				tempString = "";
				tempString += textChars[0].ToString();
				int nPos = 0;

				for(int i=1;i<textChars.Length;i++)
				{
					SizeF tempSize = e.Graphics.MeasureString(tempString, e.Font);
					if (tempSize.Width >= nWidth)
					{
						nPos = tempString.Length;
						break;
					}
					else
					{
						tempString += textChars[i].ToString();

					}
				}
				tempString += "...";
			}

			return tempString;
		}


		private void DrawTextSelected(Graphics g, SmartListItem item, string text, Brush textBrush, int selStart, int selLen, Rectangle rc, DrawItemState state)
		{
			SizeF textSize = g.MeasureString(text, item.Font);
		
			SizeF sizeStart = SizeF.Empty;
			Brush backBrush = null;
			Brush selBrush = null;

			string selStr = text.Substring(0, selLen);

			string rightStr = text.Substring(selLen, text.Length - selLen);

			//if (selStart > 0)
			sizeStart = g.MeasureString(selStr, item.Font);
		
			
			Rectangle rcSel = new Rectangle(rc.X, rc.Y + 1, (int)sizeStart.Width, rc.Height);

			Rectangle rcRight = new Rectangle((int)(rc.X+sizeStart.Width), rc.Y + 1, (int)(textSize.Width - sizeStart.Width), rc.Height);

			if (state == DrawItemState.Selected)
			{
				backBrush = new SolidBrush(SystemColors.HighlightText);
				selBrush = new SolidBrush(SystemColors.Highlight);
			}
			else
			{
				backBrush = new SolidBrush(SystemColors.Highlight);
				selBrush = new SolidBrush(SystemColors.HighlightText);
			}

			g.FillRectangle(backBrush, rcSel);
 
			g.DrawString(selStr, item.Font, selBrush, rcSel);

			g.DrawString(rightStr, item.Font, textBrush, rcRight);

			backBrush.Dispose();
			selBrush.Dispose();

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
			string itemText = "";
			if ((DisplayMember != null) && (DisplayMember != ""))
				itemText = m_properties[DisplayMember].GetValue(row).ToString();
			else
				itemText = row.ToString();

			return itemList.Add(itemText);
		}


		#region ItemCollection
		/// <summary>
		/// Represents the collection of items in a SmartList . 
		/// </summary>
		public class ItemCollection : CollectionBase
		{
			
			SmartList parent = null;
			private bool _IsSorted = true;

			public ItemCollection():base()
			{



			}

			internal ItemCollection(SmartList parent):base()
			{
				this.parent = parent;

			}

			public bool Sorted
			{
				get
				{
					return _IsSorted;
				}
				set
				{
					_IsSorted = value;
				}

			}

			//			public SmartListItem[] Clone()
			//			{
			//				//ItemCollection iColl = new ItemCollection();
			//				SmartListItem[] items = base.InnerList.ToArray();
			//				return items;
			//			}

			public object[] ToArray()
			{
				return base.InnerList.ToArray();
			}

			public ArrayList Clone()
			{
				ArrayList arrList = new ArrayList();
				int i =0;

				while(i<this.Count)
				{
					arrList.Add(this[i].Text);
					i++;
				}
				return arrList;
			}

			public ItemCollection CloneItems()
			{


				ItemCollection arrList = new ItemCollection();

				int i =0;

				while(i<this.Count)
				{
					arrList.Add((SmartListItem)this.List[i]);
					i++;
				}

				return arrList;
			}


			/// <summary>
			/// Adds an item to the list of items for a SmartList . 
			/// </summary>
			/// <param name="value">SmartListItem to add</param>
			/// <returns>Newly created SmartListItem</returns>
			public SmartListItem Add(SmartListItem value)
			{
				// Use base class to process actual collection operation
				//				if (value.Font == null)
				if (parent != null)
				{
					value.Font = parent.Font;
					//				if (value.ForeColor == Color.Empty)
					value.ForeColor = parent.ForeColor;
					//
					value.Parent = parent;
				}

				int Return  = 0;
				int NewIndex;

				if (_IsSorted)
				{
					int Index = IndexOf(value);
					NewIndex = Index>=0 ? Index : -Index-1;
					if (NewIndex >= Count) 
						List.Add(value);
					else 
						List.Insert(NewIndex, value);
				}
				else
					NewIndex = List.Add(value);
				   
				Return = NewIndex;

				//parent.arrList.Add(value);
				//				int i = List.Add(value as SmartListItem);
				if (parent != null)
					parent.Refresh();

				//return i;
				return value;
			}

			/// <summary>
			/// Adds an item to the list of items for a SmartList . 
			/// </summary>
			/// <param name="value">string for text property</param>
			/// <returns>Newly created SmartListItem</returns>
			public SmartListItem Add(string value)
			{
				// Use base class to process actual collection operation
				SmartListItem item = new SmartListItem(value);
				if (parent != null)
				{
					item.Font = parent.Font;
					//				if (value.ForeColor == Color.Empty)
					item.ForeColor = parent.ForeColor;
					//
					item.Parent = parent;
				}

				int NewIndex;

				if (_IsSorted)
				{
					int Index = IndexOf(item);
					NewIndex = Index>=0 ? Index : -Index-1;
					if (NewIndex>=Count) 
						List.Add(item);
					else 
						List.Insert(NewIndex, item);
				}
				else
					NewIndex = List.Add(item);
				   
				if (parent!=null)
					parent.Refresh();

				return item;
			}

			/// <summary>
			/// Removes all elements from the System.Collections.ArrayList.  
			/// </summary>
			public new void Clear()
			{
				base.Clear();
				if (parent!=null)
					parent.Refresh();

			}


			//			public void AddRange(SmartListItem[] values)
			//			{
			//				// Use existing method to add each array entry
			//				foreach(SmartListItem page in values)
			//					Add(page);
			//
			//				parent.RefreshItems();
			//			}

			/// <summary>
			/// Removes the specified object from the collection.
			/// </summary>
			/// <param name="value">SmartListItem to remove</param>
			public void Remove(SmartListItem value)
			{
				List.Remove(value);
				parent.Refresh();
			}

			/// <summary>
			/// Inserts an item into the list box at the specified index.  
			/// </summary>
			/// <param name="index">The zero-based index location where the item is inserted.</param>
			/// <param name="value">An object representing the item to insert.</param>
			public void Insert(int index, SmartListItem value)
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
			public bool Contains(SmartListItem value)
			{
				// Use base class to process actual collection operation
				//return base.List.Contains(value as object);

				return List.Contains(value);
				//return parent.arrList.Contains(value as object);
			}

			/// <summary>
			/// Gets or sets the item.
			/// </summary>
			public SmartListItem this[int index]
			{
				// Use base class to process actual collection operation
				get 
				{ 
					SmartListItem item = (SmartListItem)List[index];
					//SmartListItem item = parent.arrList[index] as SmartListItem;
					return item; 
				}
				set
				{
					List[index] = (SmartListItem)value;

				}
			}

			/// <summary>
			/// Gets the item.
			/// </summary>
			public SmartListItem this[string text]
			{
				get 
				{
					// Search for a Page with a matching title
					foreach(SmartListItem page in List)
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
			public int IndexOf(SmartListItem value)
			{
				// Find the 0 based index of the requested entry
				int Result = -1;
				if ( _IsSorted )
				{
					//Result = ArrayList.BinarySearch(value, _Comparer);
					Result = Array.BinarySearch(this.ToArray(), 0, List.Count, value, new ListComparer());
					while ( Result>0 && List[Result-1].Equals(value) ) Result--; // We want to point at the FIRST occurence
				}
				else Result = List.IndexOf(value);
				
				return Result;
				//return List.IndexOf(value);
			}


			protected override void OnInsertComplete(int index, object value) 
			{
				if (parent != null)
				{
					((SmartListItem)value).Parent = parent;
					parent.Invalidate();
				}
			}

			protected override void OnRemoveComplete(int index, object value) 
			{
				if (parent != null)
				{
					((SmartListItem)value).Parent = null;
					parent.Invalidate();
				}
			}
		}
		#endregion

		public class ListComparer : IComparer
		{
			#region IComparer Members

			public int Compare(object x, object y)
			{
				SmartListItem item1 = (SmartListItem)x;
				SmartListItem item2 = (SmartListItem)y;
				return String.Compare(item1.Text, item2.Text, true);
			}

			#endregion

		}
	}


	#region SmartListItem class

	/// <summary>
	/// Represents an item in a <see cref="SmartList"/> control.
	/// </summary>
#if DESIGN
	[
	DesignTimeVisible(false), TypeConverter("OpenNETCF.Windows.Forms.SmartListItemConverter"),
	ToolboxItem(false)
	]
#endif
	public class SmartListItem : System.ComponentModel.Component
	{
		private string text = "";
		private int imageIndex = -1;
		private Font font;
		private Color foreColor;
		internal ImageList imageList = null;
		private SmartList parent = null;

		/// <summary>
		/// Initializes a new instance of the SmartListItem class with default values.
		/// </summary>
		public SmartListItem()
		{
			font = new Font("Tahoma", 9F, FontStyle.Regular);
			foreColor = Color.Black;
		}

		/// <summary>
		/// Initializes a new instance of the SmartListItem class with specified item text.
		/// </summary>
		public SmartListItem(string text): this()
		{
			this.text = text;
		}

		/// <summary>
		/// Initializes a new instance of the SmartListItem class with specified item text and ImageIndex.
		/// </summary>
		public SmartListItem(string text, int imageIndex): this()
		{
			this.text = text;
			this.imageIndex = imageIndex;
		}

		internal SmartListItem(Font font)
		{
			this.font = font;
		}

		public override string ToString()
		{
			return text;
		}

		/// <summary>
		/// Gets or sets the text associated with this item.   
		/// </summary>
		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
				if (parent != null)
					parent.Invalidate();
			}
		}

#if DESIGN
		internal bool ShouldSerializeFont() 
		{
			if (parent == null) 
			{
				return Font != null;
			}
			else 
			{
				return Font != parent.Font;
			}            
		}
#endif

		/// <summary>
		/// Gets or sets the font associated with this item.   
		/// </summary>
		public Font Font
		{
			get
			{
				if (font == null && parent != null) 
				{
					return parent.Font;
				}
				return font;
			}
			set
			{
				font = value;
				if (parent != null)
					parent.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the foreground color of the item's text.
		/// </summary>
		public Color ForeColor
		{
			get
			{
				return foreColor;
			}
			set
			{
				foreColor = value;
				if (parent != null)
					parent.Invalidate();
			}
		}

		internal SmartList Parent
		{
			get
			{
				return parent;
			}
			set
			{
				parent = value;	
				//foreColor = parent.ForeColor;
				//font = parent.Font;
			}
		}

		/// <summary>
		/// Gets the <see cref="ImageList"/> that contains the image displayed with the item.
		/// </summary>
		public System.Windows.Forms.ImageList ImageList
		{
			get 
			{ 
				return this.imageList; 
			}
			//			set
			//			{
			//				imageList = value;
			//			}
		}

#if DESIGN
		[DefaultValue(-1)]
		[TypeConverter(typeof(System.Windows.Forms.ImageIndexConverter))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor", typeof(System.Drawing.Design.UITypeEditor))]
#endif
		/// <summary>
		/// Gets or sets the ImageIndex associated with this item.   
		/// </summary>
		public int ImageIndex
		{
			get
			{
				return imageIndex;
			}
			set
			{
				imageIndex = value;
				if (parent != null)
					parent.Invalidate();
			}
		}
	
	}
	#endregion



	public class Comparer : IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
			SmartListItem item1 = (SmartListItem)x;
			string item2 = (string)y;
			return String.Compare(item1.Text, (string)y, true);
		}

		#endregion

	}

	


	#region Type converters
#if DESIGN
	public class SmartListItemConverter: TypeConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

	public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
	{
		if (destType == typeof(InstanceDescriptor))
		{
			SmartListItem item = (SmartListItem)value;
	
//			System.Reflection.ConstructorInfo ci =
//				typeof(SmartListItem).GetConstructor(
//				System.Type.EmptyTypes);
//
//			return new InstanceDescriptor(ci, null, false);

				if (item.ShouldSerializeFont()) 
				{
					
					System.Reflection.ConstructorInfo ci = typeof(SmartListItem).GetConstructor(new Type[0]);
					return new InstanceDescriptor(ci, null, false);																
				}

			
		}

		return base.ConvertTo(context, culture, value, destType);
	}


//		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
//		{
//			if (destinationType == typeof(InstanceDescriptor) && value is SmartListItem)
//			{
//				SmartListItem lvi = (SmartListItem)value;
//
//				ConstructorInfo ci = typeof(SmartListItem).GetConstructor(new Type[] {});
//				if (ci != null)
//				{
//					return new InstanceDescriptor(ci, null, false);
//				}
//			}
//
//			
//			return base.ConvertTo(context, culture, value, destinationType);
//
////	        if(destinationType == typeof(InstanceDescriptor)) 
////            {
////                ConstructorInfo ci = typeof(SmartListItem).GetConstructor(new Type[]{typeof(string),
////                                                                                         typeof(int)});
////                SmartListItem item = (SmartListItem)value;
////                return new InstanceDescriptor(ci, new object[]{item.Text, item.ImageIndex}, true);
////            }
////            return base.ConvertTo(context, culture, value, destinationType);
//
//		}
	}

	
#if STANDALONE
	public class SmartItemCollectionEditor: System.ComponentModel.Design.CollectionEditor
		{

			public SmartItemCollectionEditor (System.Type type):base(type)
			{


			}

			protected override bool CanRemoveInstance(object value)
			{
				return true;
			}

	        protected override object CreateInstance(Type itemType)
		    {
			     //MessageBox.Show("CreateInstance");
	             return base.CreateInstance (itemType);
		    }

			protected override void DestroyInstance(object instance)
			{
				IDesignerHost host;

				if (instance as IComponent != null) 
				{
					host = (IDesignerHost) this.GetService(typeof(IDesignerHost));
					if (host != null) 
					{
						SmartListItem item = (SmartListItem)instance;
						host.Container.Remove(item);
					}
					//(IComponent) instance.Dispose();
				}
				
				base.DestroyInstance(instance);
			}


		}
	#endif

#endif

	#endregion



}
