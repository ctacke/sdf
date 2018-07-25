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



//----------------------------------------------------------------------------
//  This file is part of the OpenNETCF Smart Device Framework Code Samples.
// 
//  Copyright (C) OpenNETCF Consulting, LLC.  All rights reserved.
// 
//  This source code is intended only as a supplement to Smart Device 
//  Framework and/or on-line documentation.  
// 
//  THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using OpenNETCF.Windows.Forms;

namespace MSNSearchMobile
{
    public partial class MSNSearchResultsList : OwnerDrawnList
    {
        private float scale = -1;
        private Font descriptionFont;
        public MSNSearchResultsList()
        {
            InitializeComponent();
            this.BackColor = SystemColors.Window;
            this.ForeColor = SystemColors.ActiveCaption;
            using (Graphics g = this.CreateGraphics())
            {
                this.ItemHeight = 60 * (int)this.Scale(g);
            }
            this.Font = new Font(this.Font.Name, 8, FontStyle.Bold);
            this.descriptionFont = new Font(this.Font.Name, 7, FontStyle.Regular);
            this.ShowScrollbar = true;
        }

        private float Scale(Graphics g)
        {
            if (this.scale == -1)
                this.scale = g.DpiX / 96;
            return this.scale;
        }

        protected override object BuildItemForRow(object row)
        {
            return BaseItems.Add(row);
        }

        protected override void OnDrawItem(object sender, DrawItemEventArgs e)
        {
             //Set the graphics to only the item bounds
            Rectangle rcClip = e.Bounds;
            //rcClip.Inflate(-1, -1);
            e.Graphics.Clip = new Region(rcClip);

            OpenNETCF.Rss.Data.FeedItem item = BaseItems[e.Index] as OpenNETCF.Rss.Data.FeedItem;

            SolidBrush backcolor = new SolidBrush(this.BackColor);
            SolidBrush forecolor = new SolidBrush(this.ForeColor);
            if (e.State == DrawItemState.Selected)
            {
                backcolor = new SolidBrush(SystemColors.Highlight);
                forecolor = new SolidBrush(SystemColors.HighlightText);
            }

            //Draw the background
            e.Graphics.FillRectangle(backcolor, rcClip);

            //Draw a border
            using (Pen p = new Pen(SystemColors.WindowFrame))
                e.Graphics.DrawRectangle(p, new Rectangle(e.Bounds.X,e.Bounds.Y,e.Bounds.Width,e.Bounds.Height-1));

            //Draw the string
            string stringToDraw = item.Title;
            stringToDraw = this.ShrinkString(stringToDraw, e.Bounds.Width, "...", e.Graphics, this.Font);
            e.Graphics.DrawString(stringToDraw, this.Font, forecolor,1, e.Bounds.Y);

            //Draw the summary
            SizeF size = e.Graphics.MeasureString(stringToDraw, this.Font);
            Rectangle rec = new Rectangle(1, e.Bounds.Y + (int)size.Height, e.Bounds.Width, e.Bounds.Height - ((int)size.Height*2));
            using (SolidBrush b = new SolidBrush(e.State == DrawItemState.Selected ? SystemColors.Info : Color.CornflowerBlue))
                e.Graphics.DrawString(item.Description, this.descriptionFont, b, rec);

            //Draw the link
            rec = new Rectangle(1, rec.Y + rec.Height, rec.Width, rec.Height - (int)size.Height);
            stringToDraw = this.ShrinkString(item.Link, e.Bounds.Width, "...", e.Graphics, this.descriptionFont);
            using (SolidBrush b = new SolidBrush(Color.Green))
                e.Graphics.DrawString(item.Link, this.descriptionFont, b, rec.X,rec.Y);

            //Dispose
            forecolor.Dispose();
            backcolor.Dispose();

            //Reset the graphics
            e.Graphics.ResetClip();
        }

        private string ShrinkString(string value, int maxWidth, string endString, Graphics graphic, Font font)
        {
            SizeF size = graphic.MeasureString(value, font);
            if (size.Width < maxWidth)
                return value;
            else
            {
                for (int x = value.Length - 1; x > 0; x--)
                {
                    if (value[x] != ' ')
                    {
                        size = graphic.MeasureString(value.Substring(0, x) + endString, font);
                        if (size.Width < maxWidth)
                        {
                            return value.Substring(0, x) + endString;
                        }
                    }
                }
            }

            return "..";
        }
    }
}
