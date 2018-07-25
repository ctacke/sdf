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

using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OpenNETCF.Windows.Forms
{
  /// <summary>
  /// Provides data for the DrawItem event.
  /// </summary>
  public class DrawItemEventArgs : System.EventArgs
  {
    //private members
    private Color backColor;
    private Color foreColor;
    private Font font;
    private int index;
    private Graphics graphics;
    private Rectangle rect;
    private DrawItemState state;

    /// <summary>
    /// Initializes a new instance
    /// </summary>
    public DrawItemEventArgs(System.Drawing.Graphics graphics, System.Drawing.Font font, System.Drawing.Rectangle rect, System.Int32 index, DrawItemState state, System.Drawing.Color foreColor, System.Drawing.Color backColor)
    {
      this.graphics = graphics;
      this.font = font;
      this.rect = rect;
      this.index = index;
      this.state = state;
      this.foreColor = foreColor;
      this.backColor = backColor;

    }

    /// <summary>
    /// Initializes a new instance
    /// </summary>
    public DrawItemEventArgs(System.Drawing.Graphics graphics, System.Drawing.Font font, System.Drawing.Rectangle rect, System.Int32 index, DrawItemState state)
    {
      this.graphics = graphics;
      this.font = font;
      this.rect = rect;
      this.index = index;
      this.state = state;
      this.foreColor = SystemColors.ControlText;
      this.backColor = SystemColors.Window;
    }

    /// <summary>
    /// Gets the rectangle that represents the bounds of the item that is being drawn.
    /// </summary>
    public Rectangle Bounds
    {
      get
      {
        return rect;
      }
    }

    /// <summary>
    /// Draws the background within the bounds specified in the DrawItemEventArgs constructor and with the appropriate color.
    /// </summary>
    public virtual void DrawBackground()
    {
      Brush brush;
      if (state == DrawItemState.Selected)
      {
        brush = new SolidBrush(SystemColors.Highlight);
      }
      else
      {
        brush = new SolidBrush(backColor);
      }
      //graphics.FillRectangle(brush, rect);
      Rectangle rc = new Rectangle(rect.X + 1, rect.Y, rect.Width, rect.Height - 1);
      rc.Y += 1;
      graphics.FillRectangle(brush, rc);
      brush.Dispose();
    }

    /// <summary>
    /// Draws the background within the bounds specified in the DrawItemEventArgs constructor and with the appropriate color.
    /// </summary>
    public virtual void DrawBackground(Color color)
    {
      Brush brush;
      brush = new SolidBrush(color);
      Rectangle rc = new Rectangle(rect.X + 1, rect.Y, rect.Width, rect.Height - 1);
      rc.Y += 1;
      //rc.Inflate(0, -1);
      graphics.FillRectangle(brush, rc);
      brush.Dispose();
    }

    /// <summary>
    /// Draws a focus rectangle.
    /// </summary>
    public virtual void DrawFocusRectangle()
    {
      Rectangle focusRect = rect;
      focusRect.Width--;
      focusRect.Inflate(-1, 0);
      graphics.DrawRectangle(new Pen(SystemColors.Highlight), focusRect);
    }

    /// <summary>
    /// Gets the state of the item being drawn.
    /// </summary>
    public DrawItemState State
    {
      get { return state; }
    }

    /// <summary>
    /// Gets the graphics surface to draw the item on.
    /// </summary>
    public Graphics Graphics
    {
      get { return graphics; }
    }

    /// <summary>
    /// Gets the index value of the item that is being drawn.
    /// </summary>
    public int Index
    {
      get { return index; }
    }

    /// <summary>
    /// Gets the font assigned to the item being drawn.
    /// </summary>
    public System.Drawing.Font Font
    {
      get { return font; }
    }

    /// <summary>
    /// Gets the background color of the item that is being drawn.
    /// </summary>
    public Color BackColor
    {
      get { return backColor; }
    }

    /// <summary>
    /// Gets the foreground color of the of the item being drawn.
    /// </summary>
    public Color ForeColor
    {
      get { return foreColor; }
    }

  }
}
