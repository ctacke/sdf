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
    /// Image viewer control to display various image formats.
    /// </summary>
    public partial class ImageViewer : UserControl, IWin32Window
    {
        #region fields

        private string imageFile = "";
        private System.Windows.Forms.HScrollBar hScrollBar;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private Image imageBmp;
        private float zoomFactor;
        private float prev_zoomFactor;
        private int iOffsetY;
        private int iOffsetX;
        private Graphics gxOff;
        private Bitmap m_bmpOffscreen;
        private Brush cornerBrush;
        private Rectangle cornerRect;
        private bool center = false;
        private bool bSkip = false;
        //private bool bScrollBoth;
        private Font fontText;

        #endregion // fields

        /// <summary>
        /// Default constructor
        /// </summary>
        public ImageViewer()
        {
            //this.bScrollBoth = false;
            this.zoomFactor = 1;
            this.prev_zoomFactor = 1;
            this.iOffsetY = 1;
            this.iOffsetX = 1;
            this.cornerBrush = new SolidBrush(SystemColors.Control);
            this.cornerRect = Rectangle.Empty;

            InitializeComponent();	
        }

        #region properties

        /// <summary>
        /// Gets or sets the image that the ImageViewer displays.
        /// </summary>
        public Image Image
        {
            get
            {
                return imageBmp;
            }
            set
            {
                if (imageBmp != null)
                {
                    // Clean up previouse instance
                    imageBmp.Dispose();
                }

                imageBmp = value;
                if (imageBmp != null)
                {
                    zoomFactor = 1;
                    SizeScrollBars();
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the value to specify if the image should be centered.
        /// </summary>
        public bool Center
        {
            get
            {
                return center;
            }
            set
            {
                center = value;
            }
        }

        #endregion // properties

        #region public methods

        /// <summary>
        /// Zooms in the image by the default 0.2 factor.
        /// </summary>
        public void ZoomOut()
        {
            zoomFactor -= 0.2F;
            SizeScrollBars();
            this.Invalidate();
        }

        /// <summary>
        /// Zooms the image.
        /// </summary>
        /// <param name="zoomFactor">Zoom factor.</param>
        public void Zoom(int zoomFactor)
        {
            this.zoomFactor = zoomFactor;
            SizeScrollBars();
            this.Invalidate();
        }

        /// <summary>
        /// Zooms out the image by the default 0.2 factor.
        /// </summary>
        public void ZoomIn()
        {
            zoomFactor += 0.2F;
            SizeScrollBars();
            this.Invalidate();
        }

        #endregion // public methods

        #region helper methods

        private void LoadImage(string fileName)
        {
            imageFile = fileName;

            if (imageBmp != null)
                imageBmp.Dispose();

            if (System.IO.File.Exists(imageFile))
            {
                imageBmp = new Bitmap(imageFile);
            }
            else
            {
                string text = "Image is not available";
                DrawText(text);
            }

            zoomFactor = 1;
            SizeScrollBars();
            this.Invalidate();
        }

        private void DrawText(string text)
        {
            fontText = new Font("Nina", 14F, FontStyle.Bold);
            imageBmp = new Bitmap(this.Width, this.Height);
            using (Graphics gx = Graphics.FromImage(imageBmp))
            {
                gx.Clear(Color.White);
                SizeF size = gx.MeasureString(text, fontText);
                Rectangle rcText = new Rectangle((int)(this.Width - size.Width) / 2, (int)(this.Height - size.Height) / 2, (int)size.Width, (int)size.Height);
                gx.DrawString(text, fontText, new SolidBrush(Color.Gray), rcText);
            }
        }

        private void SizeScrollBars()
        {
            hScrollBar.Minimum = 0;
            vScrollBar.Minimum = 0;

            //bScrollBoth = false;

            if (imageBmp == null)
            {
                hScrollBar.Visible = false;
                vScrollBar.Visible = false;
                return;
            }

            if ((this.Width >= imageBmp.Width * zoomFactor) && (this.Height >= imageBmp.Height * zoomFactor))
            {
                hScrollBar.Visible = false;
                vScrollBar.Visible = false;
                iOffsetX = 0;
                iOffsetY = 0;
            }
            else if (this.Width > imageBmp.Width * zoomFactor)
            {
                hScrollBar.Visible = false;
                vScrollBar.Bounds = new Rectangle(this.Right - vScrollBar.Width, 0, vScrollBar.Width, this.Height);
                vScrollBar.Visible = true;
            }
            else if (this.Height > imageBmp.Height * zoomFactor)
            {
                vScrollBar.Visible = false;
                hScrollBar.Bounds = new Rectangle(0, this.Height - hScrollBar.Height, this.Width, hScrollBar.Height);
                hScrollBar.Visible = true;
            }
            else
            {
                hScrollBar.Visible = true;
                vScrollBar.Visible = true;

                hScrollBar.Bounds = new Rectangle(0, this.Height - hScrollBar.Height, this.Width - vScrollBar.Width, hScrollBar.Height);
                vScrollBar.Bounds = new Rectangle(this.Right - vScrollBar.Width, 0, vScrollBar.Width, this.Height - hScrollBar.Height);

                cornerRect = new Rectangle(hScrollBar.Right, vScrollBar.Bottom, vScrollBar.Width, hScrollBar.Height);
                //bScrollBoth = true;

            }

            if (imageBmp != null)
            {
                hScrollBar.Maximum = (int)(imageBmp.Width * zoomFactor) + (vScrollBar.Width * 3) - this.Width;
                vScrollBar.Maximum = (int)(imageBmp.Height * zoomFactor) + (hScrollBar.Height * 3) - this.Height;
            }
            else
            {
                hScrollBar.Maximum = 10;
                vScrollBar.Maximum = 10;
            }

            vScrollBar.SmallChange = vScrollBar.Maximum / 20;
            vScrollBar.LargeChange = vScrollBar.Maximum / 10;

            hScrollBar.SmallChange = hScrollBar.Maximum / 20;
            hScrollBar.LargeChange = hScrollBar.Maximum / 10;

            vScrollBar.SmallChange = vScrollBar.Maximum / 20;
            vScrollBar.LargeChange = vScrollBar.Maximum / 10;

            hScrollBar.SmallChange = hScrollBar.Maximum / 20;
            hScrollBar.LargeChange = hScrollBar.Maximum / 10;

            if (vScrollBar.SmallChange == 0)
                vScrollBar.Visible = false;


            if (hScrollBar.SmallChange == 0)
                hScrollBar.Visible = false;

            bSkip = true;
            hScrollBar.Value = iOffsetX;
            vScrollBar.Value = iOffsetY;
            bSkip = false;
        }

        #endregion // helper methods

        #region overrides

        protected override void OnPaint(PaintEventArgs e)
        {
            if (gxOff == null)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Black),new Rectangle(0,0,this.Width-1,this.Height-1));
                return;
            }

            gxOff.Clear(Color.White);

            if (imageBmp == null)
            {
                e.Graphics.DrawImage(m_bmpOffscreen, 0, 0);
                return;
            }

            Rectangle destRect = Rectangle.Empty;

            if (zoomFactor != prev_zoomFactor)
            {
                float coefX = (float)(imageBmp.Width * prev_zoomFactor) / iOffsetX;
                float coefY = (float)(imageBmp.Height * prev_zoomFactor) / iOffsetY;

                iOffsetX = (int)((imageBmp.Width * zoomFactor) / coefX);

                iOffsetY = (int)((imageBmp.Height * zoomFactor) / coefY);

                prev_zoomFactor = zoomFactor;
            }

            if (!center)
                destRect = new Rectangle(-iOffsetX, -iOffsetY, (int)(imageBmp.Width * zoomFactor), (int)(imageBmp.Height * zoomFactor));
            else
            {
                int x_cent = (this.Width - (int)(imageBmp.Width * zoomFactor)) / 2;
                int y_cent = (this.Height - (int)(imageBmp.Height * zoomFactor)) / 2;

                destRect = new Rectangle(x_cent, y_cent, (int)(imageBmp.Width * zoomFactor), (int)(imageBmp.Height * zoomFactor));

            }

            Rectangle srcRect = new Rectangle(0, 0, imageBmp.Width, imageBmp.Height);
            gxOff.DrawImage(imageBmp, destRect, srcRect, GraphicsUnit.Pixel);

            if (vScrollBar.Visible && hScrollBar.Visible)
                gxOff.FillRectangle(cornerBrush, cornerRect);

            e.Graphics.DrawImage(m_bmpOffscreen, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //do nothing;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if ((this.Width > 0) && (this.Height > 0))
            {
                if (m_bmpOffscreen != null)
                {
                    m_bmpOffscreen.Dispose();
                    gxOff.Dispose();
                }

                m_bmpOffscreen = new Bitmap(this.Width, this.Height);
                gxOff = Graphics.FromImage(m_bmpOffscreen);

                SizeScrollBars();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.Focus();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            int key = (int)e.KeyCode;
            int val = e.KeyValue;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    vScrollBar.Value -= vScrollBar.SmallChange;
                    break;
                case Keys.Down:
                    vScrollBar.Value += vScrollBar.SmallChange;
                    break;
                case Keys.Left:
                    hScrollBar.Value -= hScrollBar.SmallChange;
                    break;
                case Keys.Right:
                    hScrollBar.Value += hScrollBar.SmallChange;
                    break;
                case Keys.Enter:
                    // Zoom in
                    prev_zoomFactor = zoomFactor;
                    zoomFactor += 0.2F;
                    this.Invalidate();
                    SizeScrollBars();
                    break;

            }
            base.OnKeyDown(e);
        }

        #endregion // overrides

        #region scrollbar events

        private void hScrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (!bSkip)
            {
                iOffsetX = hScrollBar.Value;
                this.Invalidate();
            }
        }

        private void vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (!bSkip)
            {
                iOffsetY = vScrollBar.Value;
                this.Invalidate();
            }
        }

        #endregion // scrollbar events
    }
}
