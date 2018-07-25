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
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Drawing.Imaging;

namespace UPnPFinder
{
    public partial class PictureViewer : Form
    {
        private IImage _image = null;
        MemoryStream ms = null;
        private Size _imageSize;
        private Size _viewerSize = new Size(0, 0);
        StreamOnFile _stream = null;

        // Displays an image from a specified url with a given domension
        public PictureViewer(string url, int cx, int cy)
        {
            _imageSize = new Size(cx, cy);
            InitializeComponent();
            pbImageDisplay.SizeMode = PictureBoxSizeMode.StretchImage;
            LoadImage(url);


        }


        // Loads image from a specified url
        private void LoadImage(string url)
        {
            HttpWebRequest rq = HttpWebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse rsp = null;
            
            try
            {
                // Request data from the url
                rsp = rq.GetResponse() as HttpWebResponse;
                Stream st = rsp.GetResponseStream();
                
                // Read all the image data to memory
                // If we run out of memory, so be it.
                ms = new MemoryStream();
                byte[] buffer = new byte[1024];
                int cb = 0;
                while ((cb = st.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, cb);
                st.Close();
                rsp.Close();
                
                // Use ImageFactory class to load image
                _stream = new StreamOnFile(ms);
                if (_stream != null)
                {
                    IImagingFactory factory = new ImagingFactoryClass();
                    factory.CreateImageFromStream(_stream, out _image);
                }
            }
            catch(WebException wex)
            {
                if ( wex.Response != null )
                    wex.Response.Close();
                throw;
            }
            catch
            {
                MessageBox.Show("Unable to load image");
            }
            finally
            {
                if ( rsp != null )
                    rsp.Close();
            }
        }

        private void PictureViewer_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;
        }

        //Takes care of resizing the image viewer control 
        // to accomodate the image
        private void PictureViewer_Resize(object sender, EventArgs e)
        {
            try
            {
                if (ms == null)
                    return;
                if (_viewerSize.Equals(this.Size))
                    return;
                _viewerSize = this.Size;

                if (_image == null)
                    return;
                ImageInfo ii;
                _image.GetImageInfo(out ii);
                _imageSize = new Size((int)ii.Width, (int)ii.Height);

                int cxPic = Width;
                int cyPic = (int)(_imageSize.Height * cxPic / _imageSize.Width);
                pbImageDisplay.Top = (Height - cyPic) / 2;
                pbImageDisplay.Left = 0;
                if (cyPic > Height)
                {
                    cyPic = Height;
                    cxPic = (int)(_imageSize.Width * cyPic / _imageSize.Height);
                    pbImageDisplay.Top = 0;
                    pbImageDisplay.Left = (Width - cxPic) / 2;
                }
                pbImageDisplay.Width = cxPic;
                pbImageDisplay.Height = cyPic;

                if (pbImageDisplay.Image != null)
                    pbImageDisplay.Image.Dispose();

                // Create a scaled-down image size of the screen
                int cx = pbImageDisplay.Width, cy = pbImageDisplay.Height;
                Bitmap bm = new Bitmap(cx, cy);
                Graphics g = Graphics.FromImage(bm);
                IntPtr hDC = g.GetHdc();
                RECT rcSrc = new RECT(0, 0, (int)ii.Width, (int)ii.Height);
                RECT rcDst = new RECT(0, 0, cx, cy);
                _image.Draw(hDC, rcDst, null);
                g.ReleaseHdc(hDC);
                g.Dispose();
                pbImageDisplay.Image = bm;
            }
            catch(Exception ex)
            {
                Bitmap bm = new Bitmap(pbImageDisplay.Width, pbImageDisplay.Height);
                Graphics g = Graphics.FromImage(bm);
                g.Clear(Color.White);
                g.DrawString("Unable to display picture", Font, new SolidBrush(Color.Black), 0, 0);
                g.Dispose();
                pbImageDisplay.Image = bm;
            }

        }

        // Cleanup
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (pbImageDisplay.Image != null)
                pbImageDisplay.Image.Dispose();
            pbImageDisplay.Image = null;
        }
    }
}