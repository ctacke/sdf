
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.ComponentModel;

namespace OpenNETCF.Windows.Forms
{
  /// <summary>
  /// Captures a signature from the user.
  /// Can be saved to a control specific byte array or a Bitmap.
  /// </summary>
  public partial class Signature : Control
  {
    private Color borderColor = Color.Black;
    private Graphics graphics = null;
    private Graphics offscreenGraphics = null;
    private int currentX;
    private int currentY;
    private bool hasCapture = false;
    private BorderStyle borderStyle = BorderStyle.FixedSingle;
    private Pen linePen = new Pen(Color.Black);
    private Bitmap offscreenBm = null;
    private Image m_backgroundImage;
    private float penWidth = 1;

    ArrayList currentLine = new ArrayList();
    ArrayList totalLines = new ArrayList();

    /// <summary>
    /// Constructor, creates the graphics object
    /// </summary>
    public Signature()
    {
      graphics = this.CreateGraphics();
    }

    /// <summary>
    /// Gets or sets the color for the border.
    /// </summary>
    public Color BorderColor
    {
      get
      {
        return borderColor;
      }
      set
      {
        borderColor = value;
        Invalidate();
      }
    }

    /// <summary>
    /// Gets or sets the background image to use when painting
    /// </summary>
    public Image BackgroundImage
    {
      get
      {
        return m_backgroundImage;
      }
      set
      {
        m_backgroundImage = value;
        Invalidate();
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="System.Windows.Forms.BorderStyle"/> of the control.
    /// </summary>
    public BorderStyle BorderStyle
    {
      get
      {
        return borderStyle;
      }
      set
      {
        borderStyle = value;
        Invalidate();
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="ForeColor"/> of the control.
    /// </summary>
    public override Color ForeColor
    {
      get
      {
        return base.ForeColor;
      }
      set
      {
        base.ForeColor = value;
        linePen.Dispose();
        linePen = new Pen(value, penWidth);
      }
    }

    /// <summary>
    /// Converts the signature to a bitmap.
    /// </summary>
    /// <returns></returns>
    public Bitmap ToBitmap()
    {
      return new Bitmap(offscreenBm);
    }

    /// <summary>
    /// Returns the signature data consisting of points (x,y) coordinates
    /// </summary>
    /// <returns>Signature data</returns>
    public byte[] GetSignatureEx()
    {
      MemoryStream ms = new MemoryStream();
      BinaryWriter writer = new BinaryWriter(ms);

      // first write out the width and the height of the control
      writer.Write(System.Convert.ToInt16(this.Width));
      writer.Write(System.Convert.ToInt16(this.Height));

      // now write out each line
      foreach (ArrayList line in totalLines)
      {
        writer.Write(System.Convert.ToInt16(line.Count));
        foreach (Point2 p in line)
        {
          if (this.Width < 256)
            writer.Write(System.Convert.ToByte(p.X));
          else
            writer.Write(System.Convert.ToInt16(p.X));
          if (this.Height < 256)
            writer.Write(System.Convert.ToByte(p.Y));
          else
            writer.Write(System.Convert.ToInt16(p.Y));
          writer.Write(System.Convert.ToInt16(p.Width));
        }
      }

      writer.Close();
      ms.Close();
      return ms.ToArray();
    }

    /// <summary>
    /// Loads a signature from the given bytes consisting of points (x,y) coordinates
    /// </summary>
    /// <param name="b">Signature data previously serialized with e.g. GetSignatureEx</param>
    public void LoadSignatureEx(byte[] b)
    {
      bool done = false;
      int pointCount = 0;
      Point2 previousPoint, currentPoint;

      MemoryStream ms = new MemoryStream(b);
      BinaryReader reader = new BinaryReader(ms);

      int width = reader.ReadInt16();
      int height = reader.ReadInt16();

      if (width != this.Width || height != this.Height)
        throw new Exception("Dimensions not the same");

      totalLines.Clear();
      while (!done)
      {
        currentLine.Clear();
        // TODO: Fix this logic to check for EOF instead of depending upon getting an exception (exceptions are expensive in resources)
        try
        {
          pointCount = reader.ReadInt16();

          // Getting previous point
          int xTmp, yTmp;
          float wTmp;
          if (this.Width < 256)
            xTmp = reader.ReadByte();
          else
            xTmp = reader.ReadInt16();
          if (this.Height < 256)
            yTmp = reader.ReadByte();
          else
            yTmp = reader.ReadInt16();
          wTmp = reader.ReadInt16();
          previousPoint = new Point2(xTmp, yTmp, wTmp);
          currentLine.Add(previousPoint);

          for (int idx = 1; idx < pointCount; idx++)
          {
            // Getting current point
            if (this.Width < 256)
              xTmp = reader.ReadByte();
            else
              xTmp = reader.ReadInt16();
            if (this.Height < 256)
              yTmp = reader.ReadByte();
            else
              yTmp = reader.ReadInt16();
            wTmp = reader.ReadInt16();
            currentPoint = new Point2(xTmp, yTmp, wTmp);
            currentLine.Add(currentPoint);
            previousPoint = currentPoint;
          }
        }
        catch
        {
          if (currentLine.Count > 0)
            totalLines.Add(currentLine.Clone());
          break;
        }

        totalLines.Add(currentLine.Clone());
      }

      Invalidate();
    }

    /// <summary>
    /// Clears the signature area.
    /// </summary>
    public void Clear()
    {
      currentLine.Clear();
      totalLines.Clear();
      graphics.Clear(BackColor);
      offscreenGraphics.Clear(BackColor);
      Invalidate();
    }

    /// <summary>
    /// Releases the unmanaged resources used by the System.Windows.Forms.Control
    /// and its child controls and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">
    /// true to release both managed and unmanaged resources; false to release only
    /// unmanaged resources.
    /// </param>
    protected override void Dispose(bool disposing)
    {
      graphics.Dispose();
      if (offscreenBm != null)
      {
        offscreenBm.Dispose();
      }

      base.Dispose(disposing);
    }

    /// <summary>
    /// Draw the border of the signature box if it is required.
    /// </summary>
    private void DrawBorder(Graphics g)
    {
      if (BorderStyle == BorderStyle.None)
        return;

      using (Pen p = new Pen(BorderColor))
      {
        switch (BorderStyle)
        {
          case BorderStyle.FixedSingle:
            g.DrawRectangle(p, 0, 0, this.Width - 1, this.Height - 1);
            break;

          case BorderStyle.Fixed3D:
            Color darkBorderColor = SystemColors.ControlDark;
            Color darkestBorderColor = Color.Black;
            Color lightBorderColor = SystemColors.Control;
            Color lightestBorderColor = SystemColors.ControlLightLight;
            Pen borderPenFixed3D = new Pen(darkBorderColor);
            Size borderOffsetSize = new Size(0, 0);
            // Paint the dark lines.
            g.DrawLine(borderPenFixed3D, (this.ClientRectangle.Width - borderOffsetSize.Width - 2), (borderOffsetSize.Height + 1), (this.ClientRectangle.Width - borderOffsetSize.Width - 2), (this.ClientRectangle.Height - borderOffsetSize.Height - 2));
            g.DrawLine(borderPenFixed3D, (borderOffsetSize.Width + 1), (this.ClientRectangle.Height - borderOffsetSize.Height - 2), (this.ClientRectangle.Width - borderOffsetSize.Width - 2), (this.ClientRectangle.Height - borderOffsetSize.Height - 2));
            // Paint the darkest lines.
            borderPenFixed3D.Color = darkestBorderColor;
            g.DrawLine(borderPenFixed3D, (this.ClientRectangle.Width - borderOffsetSize.Width - 1), borderOffsetSize.Height, (this.ClientRectangle.Width - borderOffsetSize.Width - 1), (this.ClientRectangle.Height - borderOffsetSize.Height - 1));
            g.DrawLine(borderPenFixed3D, borderOffsetSize.Width, (this.ClientRectangle.Height - borderOffsetSize.Height - 1), (this.ClientRectangle.Width - borderOffsetSize.Width - 1), (this.ClientRectangle.Height - borderOffsetSize.Height - 1));
            // Paint the light lines.
            borderPenFixed3D.Color = lightBorderColor;
            g.DrawLine(borderPenFixed3D, (borderOffsetSize.Width + 1), (borderOffsetSize.Height + 1), (this.ClientRectangle.Width - borderOffsetSize.Width - 3), (borderOffsetSize.Height + 1));
            g.DrawLine(borderPenFixed3D, (borderOffsetSize.Width + 1), (borderOffsetSize.Height + 1), (borderOffsetSize.Width + 1), (this.ClientRectangle.Height - borderOffsetSize.Height - 3));
            // Paint the lightest lines.
            borderPenFixed3D.Color = lightestBorderColor;
            g.DrawLine(borderPenFixed3D, borderOffsetSize.Width, borderOffsetSize.Height, (this.ClientRectangle.Width - borderOffsetSize.Width - 2), borderOffsetSize.Height);
            g.DrawLine(borderPenFixed3D, borderOffsetSize.Width, borderOffsetSize.Height, borderOffsetSize.Width, (this.ClientRectangle.Height - borderOffsetSize.Height - 2));
            borderPenFixed3D.Dispose();
            break;
        }
      }
    }

    /// <summary>
    /// Raises the MouseDown event.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
    {
      hasCapture = true;
      currentX = e.X;
      currentY = e.Y;
      currentLine.Add(new Point2(e.X, e.Y, PenWidth));
    }

    /// <summary>
    /// Raises the MouseMove event.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
    {
      // make sure we have the mouse capture (for windows only)
      if (hasCapture)
      {
        // TODO: This should probably be changed to NOT ADD up points if signature is "out of bounds" since they will not be shown anyway when rendereing the signature
        int x = e.X;
        int y = e.Y;
        if (x > Width)
          x = Width;
        if (x < 0)
          x = 0;
        if (y > Height)
          y = Height;
        if (y < 0)
          y = 0;
        graphics.DrawLine(linePen, currentX, currentY, x, y);
        offscreenGraphics.DrawLine(linePen, currentX, currentY, x, y);
        currentX = x;
        currentY = y;
        currentLine.Add(new Point2(x, y, PenWidth));
      }
    }

    /// <summary>
    /// Raises the MouseUp event.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
    {
      hasCapture = false;
      if (currentLine.Count > 0)
      {
        totalLines.Add(currentLine.Clone());
        currentLine.Clear();
      }
    }

    /// <summary>
    /// Raises the PaintBackground event.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPaintBackground(PaintEventArgs e)
    {
    }

    #region Designer OnPaint Support
#if DESIGN
		protected override void OnPaint(PaintEventArgs e)
		{
			
		}
#else
    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
      if (this.DesignMode)
      {
        if (m_backgroundImage != null)
        {
          // Assuming Image will fill the whole of the background, therefor not doing the "clear" thing
          e.Graphics.DrawImage(m_backgroundImage, new Rectangle(0, 0, this.Width, this.Height),
              new Rectangle(0, 0, m_backgroundImage.Width, m_backgroundImage.Height), GraphicsUnit.Pixel);
        }
        else
        {
			//fill background
			// jsm - Bug 276 - Minor VS memory leak
			using (SolidBrush backgroundBrush = new SolidBrush(this.BackColor))
			{
				e.Graphics.FillRectangle(backgroundBrush, 0, 0, this.Width, this.Height);
			}
        }

        if (BorderStyle != BorderStyle.None)
        {
          //draw a border
          DrawBorder(e.Graphics);
        }

        base.OnPaint(e);
      }
      else
      {
        Point2 previousPoint, currentPoint;
        if (m_backgroundImage != null)
        {
          e.Graphics.DrawImage(m_backgroundImage, new Rectangle(0, 0, this.Width, this.Height),
               new Rectangle(0, 0, m_backgroundImage.Width, m_backgroundImage.Height), GraphicsUnit.Pixel);
        }
        else
        {
          graphics.Clear(this.BackColor);
        }
        foreach (ArrayList line in totalLines)
        {
          if (line.Count == 0)
            continue;

          previousPoint = (Point2)line[0];
          for (int x = 1; x < line.Count; x++)
          {
            currentPoint = (Point2)line[x];
            //jsm - Enhancement 138 - switch Pen out if we've switched pen widths
            if (linePen.Width != currentPoint.Width)
              PenWidth = currentPoint.Width;
            graphics.DrawLine(linePen, previousPoint.X, previousPoint.Y, currentPoint.X, currentPoint.Y);
            previousPoint = currentPoint;
          }
        }
        this.DrawBorder(e.Graphics);
      }
    }
#endif
    #endregion

    /// <summary>
    /// Raises the Resize event.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnResize(EventArgs e)
    {
		// create a new offscreen bitmap after disposing first
		if (offscreenBm != null)
		{
			// jsm - Bug 276 - User was getting OOM on scrolling
			offscreenBm.Dispose();
		}
      offscreenBm = new Bitmap(this.Width, this.Height);
      offscreenGraphics = Graphics.FromImage(offscreenBm);
      offscreenGraphics.Clear(this.BackColor);
      this.Invalidate();
    }
    /// <summary>
    /// Gets or sets the width of the signature draw pen
    /// </summary>
    public float PenWidth
    {
      get
      {
        return linePen.Width;
      }
      set
      {
        Color c = linePen.Color;
        System.Drawing.Drawing2D.DashStyle d = linePen.DashStyle;
        linePen.Dispose();
        linePen = new Pen(c);
        linePen.Width = penWidth = value;
        linePen.DashStyle = d;
      }
    }

    #region Design Mode
    /// <summary>
    /// Gets a value indicating whether a control is being used on a design surface.
    /// </summary>
    protected internal bool DesignMode
    {
        get
        {
            if (this.Site != null && this.Site.DesignMode)
                return true;

            return StaticMethods.IsDesignMode(this.Parent);
        }
    }
    #endregion



  }
}