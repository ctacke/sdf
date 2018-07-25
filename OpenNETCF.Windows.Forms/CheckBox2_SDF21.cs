using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace OpenNETCF.Windows.Forms
{
    partial class CheckBox2
    {
        private bool m_drawFocusedRectangle = false;

        /// <summary>
        /// New in v2.1. Gets or Sets the value indicating if the focused rectangle should be drawn.
        /// </summary>
        /// <version>2.1</version>
        public bool DrawFocusedRectangle
        {
            get { return m_drawFocusedRectangle; }
            set
            {
                if (value != m_drawFocusedRectangle)
                {
                    m_drawFocusedRectangle = value;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Raises the OnKeyPress event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if ((e.KeyChar == (char)Keys.Space) || (e.KeyChar == (char)Keys.Enter))
                this.Checked = !this.Checked;
        }

        /// <summary>
        /// Raises the OnKeyUp event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!this.Focused)
                return;

            if (this.Parent != null)
            {
                if (e.KeyValue == (int)Keys.Up || e.KeyValue == (int)Keys.Left)
                {
                    this.Parent.SelectNextControl(this, false, true, true, true);
                }
                else if (e.KeyValue == (int)Keys.Down || e.KeyValue == (int)Keys.Right)
                {
                    this.Parent.SelectNextControl(this, true, true, true, true);
                }
            }
        }
        /// <summary>
        /// New for v2.1.  Draws a focus rectangle similar to WM5 functionality.
        /// </summary>
        /// <param name="checkRect"></param>
        /// <param name="textRect"></param>
        /// <version>2.1</version>
        private void DrawFocusRectangle(Rectangle checkRect, Rectangle textRect)
        {
            SizeF textSize = gxOff.MeasureString(this.text, this.Font);
            Rectangle tRect = new Rectangle(checkRect.X + m_scale, checkRect.Y + m_scale, checkRect.Width - (2 * m_scale), checkRect.Height - (2 * m_scale));
            Rectangle tTextRect;
            if (checkAlign == ContentAlignment.TopLeft)
                tTextRect = new Rectangle(checkRect.Right + Offset,
                    checkRect.Top + (checkRect.Height - (int)textSize.Height),
                    (int)textSize.Width + Offset + m_scale,
                    (int)textSize.Height + Offset);
            else
                tTextRect = new Rectangle(checkRect.Left - Offset - (int)textSize.Width - Offset - 2,
                    checkRect.Top + (checkRect.Height - (int)textSize.Height),
                    (int)textSize.Width + Offset,
                    (int)textSize.Height + Offset);

            using (Pen p = new Pen(SystemColors.Highlight, m_scale))
            {
                gxOff.DrawRectangle(p, tRect);
                gxOff.DrawRectangle(p, tTextRect);
            }
        }
    }
}
