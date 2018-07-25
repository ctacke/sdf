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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace OpenNETCF.Windows.Forms
{
  /// <summary>
  /// Represents an enhanced CheckBox with similar functionality that's available in the .NET Framework.
  /// </summary>
  public partial class CheckBox2 : Control, IWin32Window
  {
    #region private members
    private ContentAlignment checkAlign = ContentAlignment.TopLeft;
    private Bitmap m_bmpOffscreen;
    private bool bChecked;
    private Brush textBrush;
    private Rectangle hotClickArea;
    private Graphics gxOff;
    private Color backColor;
    private Color foreColor;
    private System.Windows.Forms.CheckState checkState;
    private Pen forePen;
    private Color textColor;
    private bool autoCheck;
    private bool raiseEvent;
    private System.Windows.Forms.BorderStyle borderStyle;
    private string text;
    private int m_scale = 1;
    private Size m_checkRectangleSize = new Size(14, 14);

    #endregion

    /// <summary>
    /// Occurs when the value of the CheckBox.CheckState property changes.  
    /// </summary>
    public event System.EventHandler CheckStateChanged;

    public new event System.EventHandler GotFocus;
    public new event System.EventHandler LostFocus;

    /// <summary>
    /// Initializes a new instance of the CheckBox2 class
    /// </summary>
    public CheckBox2()
    {
      InitializeComponent();
      using (Graphics g = this.CreateGraphics())
        m_scale = (int)StaticMethods.Scale(g);
      text = Name;
      bChecked = false;
      foreColor = SystemColors.ControlText;
      backColor = Color.Empty;
      textColor = SystemColors.ControlText;
      textBrush = new SolidBrush(textColor);
      forePen = new Pen(this.ForeColor);
      borderStyle = BorderStyle.None;
      checkState = CheckState.Unchecked;
      autoCheck = true;
      raiseEvent = false;
      hotClickArea = new Rectangle(2, 2, CheckRectangleWidth, CheckRectangleHeight);
      this.Font = new Font("Tahoma", 9F, FontStyle.Regular);
      this.Width = 100 * m_scale;
      this.Height = 20 * m_scale;
    }



    private int Offset
    {
      get
      {
        return m_scale * 2;
      }
    }
    private int CheckRectangleWidth
    {
      get
      {
        return m_checkRectangleSize.Width * m_scale;
      }
    }

    private int CheckRectangleHeight
    {
      get
      {
        return m_checkRectangleSize.Height * m_scale;
      }
    }


    /// <summary>
    /// Raises the System.Windows.Forms.CheckBox.CheckStateChanged event.  
    /// </summary>
    /// <param name="e">A System.EventArgs that contains the event data.</param>
    protected virtual void OnCheckStateChanged(System.EventArgs e)
    {
      //Raise event
      if (CheckStateChanged != null)
        CheckStateChanged(this, e);
    }

    private void DrawCheck(Graphics mainGfx, Pen pen, Point pt)
    {

      if (m_scale == 1)
      {
        mainGfx.DrawLine(pen, pt.X, pt.Y, pt.X, pt.Y + 2);
        mainGfx.DrawLine(pen, pt.X + 1, pt.Y + 1, pt.X + 1, pt.Y + 3);
        mainGfx.DrawLine(pen, pt.X + 2, pt.Y + 2, pt.X + 2, pt.Y + 4);
        mainGfx.DrawLine(pen, pt.X + 3, pt.Y + 3, pt.X + 3, pt.Y + 5);
        mainGfx.DrawLine(pen, pt.X + 4, pt.Y + 2, pt.X + 4, pt.Y + 4);
        mainGfx.DrawLine(pen, pt.X + 5, pt.Y + 1, pt.X + 5, pt.Y + 3);
        mainGfx.DrawLine(pen, pt.X + 6, pt.Y, pt.X + 6, pt.Y + 2);
        mainGfx.DrawLine(pen, pt.X + 7, pt.Y - 1, pt.X + 7, pt.Y + 1);
        mainGfx.DrawLine(pen, pt.X + 8, pt.Y - 2, pt.X + 8, pt.Y);
      }
      else
      {
        mainGfx.DrawLine(pen, pt.X, pt.Y, pt.X, pt.Y + 5);
        mainGfx.DrawLine(pen, pt.X + 1, pt.Y, pt.X + 1, pt.Y + 6);
        mainGfx.DrawLine(pen, pt.X + 2, pt.Y + 1, pt.X + 2, pt.Y + 7);
        mainGfx.DrawLine(pen, pt.X + 3, pt.Y + 2, pt.X + 3, pt.Y + 8);
        mainGfx.DrawLine(pen, pt.X + 4, pt.Y + 3, pt.X + 4, pt.Y + 9);
        mainGfx.DrawLine(pen, pt.X + 5, pt.Y + 4, pt.X + 5, pt.Y + 10);
        mainGfx.DrawLine(pen, pt.X + 6, pt.Y + 5, pt.X + 6, pt.Y + 11);
        mainGfx.DrawLine(pen, pt.X + 7, pt.Y + 5, pt.X + 7, pt.Y + 11);
        mainGfx.DrawLine(pen, pt.X + 8, pt.Y + 4, pt.X + 8, pt.Y + 10);
        mainGfx.DrawLine(pen, pt.X + 9, pt.Y + 3, pt.X + 9, pt.Y + 9);
        mainGfx.DrawLine(pen, pt.X + 10, pt.Y + 2, pt.X + 10, pt.Y + 8);
        mainGfx.DrawLine(pen, pt.X + 11, pt.Y + 1, pt.X + 11, pt.Y + 7);
        mainGfx.DrawLine(pen, pt.X + 12, pt.Y, pt.X + 12, pt.Y + 6);
        mainGfx.DrawLine(pen, pt.X + 13, pt.Y - 1, pt.X + 13, pt.Y + 5);
        mainGfx.DrawLine(pen, pt.X + 14, pt.Y - 2, pt.X + 14, pt.Y + 4);
        mainGfx.DrawLine(pen, pt.X + 15, pt.Y - 3, pt.X + 15, pt.Y + 3);
        mainGfx.DrawLine(pen, pt.X + 16, pt.Y - 4, pt.X + 16, pt.Y + 2);
        mainGfx.DrawLine(pen, pt.X + 17, pt.Y - 4, pt.X + 17, pt.Y + 1);
      }
    }

    /// <summary>
    /// Gets or sets the alignment of the checkBox.
    /// </summary>
    /// <exception cref="System.NotSupportedException">CheckBox2 does not support ContentAlignment.TopCenter.</exception>
    public ContentAlignment CheckAlign
    {
      set
      {
        if (value == ContentAlignment.TopCenter)
          throw new NotSupportedException("ContentAlignment.TopCenter is not supported.");
        if (value == ContentAlignment.TopLeft)
          hotClickArea = new Rectangle(Offset, Offset, CheckRectangleWidth, CheckRectangleHeight);
        else
          hotClickArea = new Rectangle(Width - CheckRectangleWidth, Offset, CheckRectangleWidth, CheckRectangleHeight);

        checkAlign = value;
        this.Invalidate();
      }
      get { return checkAlign; }
    }

    /// <summary>
    /// Gets or sets the state of the check box.  
    /// </summary>
    public System.Windows.Forms.CheckState CheckState
    {
      get
      {
        return checkState;
      }
      set
      {
        if (checkState != value)
        {
          checkState = value;
          if (checkState == CheckState.Checked)
            bChecked = true;
          else if (checkState == CheckState.Unchecked)
            bChecked = false;
          else
            bChecked = false;

          this.Invalidate();
          OnCheckStateChanged(null);
        }
      }
    }

    /// <summary>
    /// Gets or sets the border style color of the control. 
    /// </summary>
    public System.Windows.Forms.BorderStyle BorderStyle
    {
      get
      {
        return borderStyle;
      }
      set
      {
        if (borderStyle != value)
        {
          borderStyle = value;
          this.Invalidate();
        }
      }
    }

    /// <summary>
    /// Gets or sets the text associated with this control.
    /// </summary>
    public new string Text
    {
      get { return text; }
      set
      {
        text = value;
        this.Invalidate();
      }
    }

    /// <summary>
    /// Gets or set a value indicating whether the check box is in the checked state.  
    /// </summary>
    public bool Checked
    {
      get
      {
        return bChecked;
      }
      set
      {
        if (bChecked != value)
        {
          bChecked = value;
          if (bChecked)
            checkState = CheckState.Checked;
          else
            checkState = CheckState.Unchecked;

          this.Invalidate();
        }
      }
    }

    /// <summary>
    ///  Gets or set a value indicating whether the Checked or CheckState values and the check box's appearance are automatically changed when the check box is clicked.  
    /// </summary>
    public bool AutoCheck
    {
      get
      {
        return autoCheck;
      }
      set
      {
        if (autoCheck != value)
        {
          autoCheck = value;
        }
      }
    }

    #region Base Overrides

    /// <summary>
    /// Scales a controls location, size, padding and margin.
    /// </summary>
    /// <param name="factor"></param>
    /// <param name="specified"></param>
    protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
    {
      //height scaling is handled internally
      int prevHeight = base.Height;
      base.ScaleControl(factor, specified);
      base.Height = prevHeight;
    }

    /// <summary>
    /// Raises the PaintEvent.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPaint(PaintEventArgs e)
    {
      Rectangle textRect = this.ClientRectangle;

      textRect.Inflate(-2, -2);

      //Paint background.
      SolidBrush backColorBrush = new SolidBrush(this.BackColor);
      gxOff.FillRectangle(backColorBrush, 0, 0, this.Width, this.Height);
      backColorBrush.Dispose();

      Rectangle checkRect = Rectangle.Empty;
      Point pt;
      Brush _textBrush = textBrush;
      Pen _pen = forePen;
      if (!Enabled)
      {
        _textBrush = new SolidBrush(SystemColors.InactiveBorder);
        _pen = new Pen(SystemColors.InactiveBorder);
      }

      // draw the checkbox
      switch (checkAlign)
      {
        case ContentAlignment.TopLeft:

          checkRect = new Rectangle(Offset, Offset, CheckRectangleWidth, CheckRectangleHeight);
          textRect.X = checkRect.Right + Offset * 2;
          break;
        case ContentAlignment.TopRight:
          checkRect = new Rectangle(((Width - 1) - CheckRectangleWidth) - Offset, Offset, CheckRectangleWidth, CheckRectangleHeight);
          textRect.X = checkRect.Left - (((int)(e.Graphics.MeasureString(Text, Font).Width) + Offset)) - Offset;
          break;
      }

      //Draw checkbox image
      if (checkState == CheckState.Checked)
      {
        pt = new Point(checkRect.Left + (3 * m_scale), checkRect.Top + (6 * m_scale));
        DrawCheck(gxOff, _pen, pt);
      }
      else if (checkState == CheckState.Indeterminate)
      {
        gxOff.FillRectangle(new SolidBrush(Color.LightGray), checkRect);
      }

      //Draw the check rectangle
      float prevWidth = _pen.Width;
      _pen.Width = m_scale;
      gxOff.DrawRectangle(_pen, checkRect);
      _pen.Width = prevWidth;

      //Draw string
      gxOff.DrawString(this.Text, this.Font, _textBrush, textRect);

      if (borderStyle == BorderStyle.FixedSingle)
      {
        Rectangle rc = this.ClientRectangle;
        rc.Height--;
        rc.Width--;
        //Draw border
        gxOff.DrawRectangle(new Pen(Color.Black), rc);
      }

      //Only dispose if disabled
      if (!Enabled)
      {
        _pen.Dispose();
        _textBrush.Dispose();
      }

      //Blit on the control's Graphics
      e.Graphics.DrawImage(m_bmpOffscreen, 0, 0);

      base.OnPaint(e);
    }

    /// <summary>
    /// Gets or sets the color of the text of the control. 
    /// </summary>
    public override Color ForeColor
    {
      get
      {
        return textColor;
      }
      set
      {
        if (textColor != value)
        {
          textColor = value;
          textBrush = new SolidBrush(textColor);
          this.Invalidate();
        }
      }
    }

    /// <summary>
    /// Gets or sets the color of the checkbox itself in the control. 
    /// </summary>
    public Color CheckBoxColor
    {
      get
      {
        return foreColor;
      }
      set
      {
        if (foreColor != value)
        {
          foreColor = value;
          forePen = new Pen(foreColor);
          this.Invalidate();
        }
      }
    }

    /// <summary>
    /// Gets or sets the background color for the control.  
    /// </summary>
    public override Color BackColor
    {
      get
      {
        if ((backColor == Color.Empty) && (this.Parent != null))
        {
          return this.Parent.BackColor;
        }
        return backColor;
      }
      set
      {
        backColor = value;
        this.Invalidate();
      }
    }

    /// <summary>
    /// Raises the GotFocus event
    /// </summary>
    /// <param name="e"></param>
    protected override void OnGotFocus(EventArgs e)
    {
      if (this.GotFocus != null)
        this.GotFocus(this, null);
    }

    /// <summary>
    /// Raises the LostFocus event
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLostFocus(EventArgs e)
    {
      if (this.LostFocus != null)
        this.LostFocus(this, null);
    }

    /// <summary>
    /// Raises the ParentChanged event
    /// </summary>
    /// <param name="e"></param>
    protected override void OnParentChanged(EventArgs e)
    {
      base.OnParentChanged(e);

      if (this.Parent != null)
        this.backColor = Parent.BackColor;
    }

    /// <summary>
    /// Raises the Resize event
    /// </summary>
    /// <param name="e"></param>
    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      if ((this.Width > 0) && (this.Height > 0))
      {
        m_bmpOffscreen = new Bitmap(this.Width, this.Height);
        gxOff = Graphics.FromImage(m_bmpOffscreen);
        m_scale = (int)StaticMethods.Scale(gxOff);
      }
      if (this.Height < (this.CheckRectangleHeight + (Offset * 2)))
        base.Height = this.Offset * 2 + this.CheckRectangleHeight;
    }

    /// <summary>
    /// Raises the MouseUp event
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseUp(MouseEventArgs e)
    {
      if (raiseEvent)
        OnCheckStateChanged(null);

      raiseEvent = false;
      base.OnMouseUp(e);
    }

    /// <summary>
    /// Raises the MouseDown event
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseDown(MouseEventArgs e)
    {
      this.Focus();

      raiseEvent = false;
      if ((new Rectangle(0, 0, this.Width, this.Height).Contains(e.X, e.Y)) && (autoCheck == true))
      {
        if (bChecked)
          checkState = CheckState.Unchecked;
        else
          checkState = CheckState.Checked;

        bChecked = !bChecked;
        raiseEvent = true;
        this.Invalidate();
      }

      base.OnMouseDown(e);
    }

    /// <summary>
    /// Raises the TextChanged event
    /// </summary>
    /// <param name="e"></param>
    protected override void OnTextChanged(EventArgs e)
    {
      this.Invalidate();
      base.OnTextChanged(e);
    }

    /// <summary>
    /// Raises the EnabledChanged event
    /// </summary>
    /// <param name="e"></param>
    protected override void OnEnabledChanged(EventArgs e)
    {
      base.OnEnabledChanged(e);
      this.Invalidate();
    }


    #endregion
  }
}