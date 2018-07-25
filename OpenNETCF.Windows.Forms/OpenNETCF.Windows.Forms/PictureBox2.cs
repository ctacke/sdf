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
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using OpenNETCF.Win32;

namespace OpenNETCF.Windows.Forms
{
    public partial class PictureBox2 : PictureBox, IWin32Window
    {
        //public new event EventHandler Click;
        //public new event KeyEventHandler KeyDown;
        //public new event KeyPressEventHandler KeyPress;
        //public new event KeyEventHandler KeyUp;

        Color _transparentColor;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PictureBox2()
        {
            InitializeComponent();
            _transparentColor = Color.FromArgb(255, 0, 255);
        }


        /// <summary>
        /// Sets or gets transparent color for an Image. The default color is 255, 0, 255.
        /// </summary>
        public Color TransparentColor
        {
            get
            {
                return _transparentColor;
            }
            set
            {
                _transparentColor = value;
            }
        }

        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
            SolidBrush sb = new SolidBrush(Parent.BackColor);
            e.Graphics.FillRectangle(sb, this.ClientRectangle);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (Image != null)
            {
                Rectangle destination = new Rectangle(0, 0, this.Width, this.Height); //stretch Mode
                Rectangle source = new Rectangle(0, 0, Image.Width, Image.Height);
                if (SizeMode == PictureBoxSizeMode.CenterImage)
                {
                    destination = new Rectangle(this.Width / 2 - this.Image.Width / 2,
                        this.Height / 2 - this.Image.Height / 2,
                        this.Image.Width, this.Image.Height);
                }
                else if (SizeMode == PictureBoxSizeMode.Normal)
                {
                    destination = new Rectangle(0, 0, this.Image.Width, this.Image.Height);
                }
                ImageAttributes attr = new ImageAttributes();
                attr.SetColorKey(_transparentColor, _transparentColor);
                e.Graphics.DrawImage(this.Image, destination,
                    source.X, source.Y, source.Width, source.Height, 
                    GraphicsUnit.Pixel, attr);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            base.Invalidate();
        }
    }
}
