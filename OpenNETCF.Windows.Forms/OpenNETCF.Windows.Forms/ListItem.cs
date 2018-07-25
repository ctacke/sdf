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
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;

namespace OpenNETCF.Windows.Forms
{
  public class ListItem : Component //: System.ComponentModel.Component
  {
    private string text = "";
    private int imageIndex = -1;
    private Font font;
    private Color foreColor;
    internal ImageList imageList = null;
    private ListBox2 parent = null;

    /// <summary>
    /// Initializes a new instance of the ListItem class with default values.
    /// </summary>
    public ListItem()
    {
      font = new Font("Tahoma", 9F, FontStyle.Regular);
      foreColor = Color.Black;
    }

    /// <summary>
    /// Initializes a new instance of the ListItem class with specified item text.
    /// </summary>
    public ListItem(string text)
      : this()
    {
      this.text = text;
    }

    /// <summary>
    /// Initializes a new instance of the ListItem class with specified item text and ImageIndex.
    /// </summary>
    public ListItem(string text, int imageIndex)
      : this()
    {
      this.text = text;
      this.imageIndex = imageIndex;
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

    /// <summary>
    /// Gets or sets the font associated with this item.   
    /// </summary>
    public Font Font
    {
      get
      {
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

    internal ListBox2 Parent
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
#if DESIGN
		[Browsable(false)]
#endif
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

    /// <summary>
    /// Gets or sets an object that contains data to associate with the item.
    /// </summary>
    public object Tag { get; set; }
  }
}
