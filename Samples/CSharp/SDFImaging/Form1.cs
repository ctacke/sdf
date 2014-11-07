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
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

using System.Reflection;
using System.IO;

using SDFImaging.Properties;
using OpenNETCF.Drawing;
using OpenNETCF.Drawing.Imaging;

namespace OpenNETCF.Samples.Imaging
{

    public partial class Form1 : Form
    {

        private Bitmap lastImage = null;
        private string lastImagePath = "";
        private ImagingFactoryClass imageFactory = new ImagingFactoryClass();

        public Form1()
        {
            InitializeComponent();
        }

        private void lnkSelectImage_Click(object sender, EventArgs e)
        {
            using (ImageSelector dlg = new ImageSelector())
            {
                if (dlg.ShowDialog() == DialogResult.Yes)
                {
                    this.LoadImage(dlg.FileSelected);
                    this.trackBar1.Value = 5;

                }
            }
        }

        private void btnRotateLeft_Click(object sender, EventArgs e)
        {
            Bitmap b = ImageUtils.Rotate((Bitmap)this.imageViewer1.Image, 90);
            this.imageViewer1.Image = b;
        }

        private void btnRotateRight_Click(object sender, EventArgs e)
        {
            Bitmap b = ImageUtils.Rotate((Bitmap)this.imageViewer1.Image, -90);
            this.imageViewer1.Image = b;
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            this.LoadImage(this.lastImagePath);
            this.trackBar1.Value = 5;
        }

        private void LoadImage(string path)
        {
            string imageFilePath = this.lastImagePath = path;
            if (this.lastImage != null)
            {
                this.lastImage.Dispose();
                this.lastImage = null;
            }
            if (this.lastImagePath != null)
            {
                ImageChangedArgs args = new ImageChangedArgs(imageFilePath);
                this.lastImage = (Bitmap)args.Image.Clone();
                this.LoadImage(args);
                this.EnableControls();
                args.Dispose();
                args = null;
            }
        }

        private void EnableControls()
        {
            this.btnReload.Enabled =
                this.btnRotateLeft.Enabled = this.btnRotateRight.Enabled = (this.lastImage != null);
        }

        private void LoadImage(ImageChangedArgs imageArgs)
        {
            try
            {

                //Set the thumbnail
                IBitmapImage ibi = ImageUtils.CreateThumbnail(imageArgs.Stream, this.pbThumbnail.Size);
                Bitmap bmpThumb = ImageUtils.IBitmapImageToBitmap(ibi);
                this.pbThumbnail.Image = bmpThumb;

                //Set the image
                ibi = ImageUtils.CreateThumbnail(imageArgs.Stream, imageArgs.Image.Size);
                bmpThumb = ImageUtils.IBitmapImageToBitmap(ibi);
                this.imageViewer1.Image = bmpThumb;
            }
            catch
            {
                this.pbThumbnail.Image = null;
            }
        }

        #region Temp code
        static public IBitmapImage CreateThumbnail1(Stream stream, Size size)
        {
            //IImage image, imageThumb;
            //factory.CreateImageFromStream(new StreamOnFile(stream), out image);
            //image.GetThumbnail((uint)size.Width, (uint)size.Height, out imageThumb);
            //IBitmapImage imageBitmap;
            //ImageInfo ii;
            //image.GetImageInfo(out ii);
            //factory.CreateBitmapFromImage(image, (uint)size.Width, (uint)size.Height, ii.PixelFormat, InterpolationHint.InterpolationHintDefault, out imageBitmap);
            //return imageBitmap;
            return null;
        }

        static public IBitmapImage CreateGrayScaleImage(Stream stream)
        {
            ImagingFactory factory = new ImagingFactoryClass();
            IImage image;
            factory.CreateImageFromStream(new StreamOnFile(stream), out image);
            ImageInfo ii;

            image.GetImageInfo(out ii);

            image.SetImageFlags((ImageFlags)((int)ii.Flags & (int)ImageFlags.ImageFlagsColorSpaceGRAY));
            IBitmapImage imageBitmap = null;

            try
            {
                factory.CreateBitmapFromImage(image, (uint)ii.Width, (uint)ii.Height, ii.PixelFormat, InterpolationHint.InterpolationHintDefault, out imageBitmap);
            }
            catch { }
            return imageBitmap;
        }
        #endregion

        private class ImageChangedArgs : EventArgs, IDisposable
        {
            private Bitmap image;
            /// <summary>
            /// used to create the thumbnail
            /// </summary>
            private Stream stream;
            public ImageChangedArgs(string imageFilePath)
            {

                try
                {
                    this.stream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
                    this.image = new Bitmap(this.stream);
                }
                catch
                {
                    this.image = null;
                    if (this.stream != null)
                    {
                        this.stream.Close();
                        this.stream = null;
                    }
                }
            }

            public Bitmap Image
            {
                get
                {
                    return this.image;
                }
            }

            public Stream Stream
            {
                get
                {
                    return this.stream;
                }
            }

            #region IDisposable Members

            public void Dispose()
            {
                if (this.image != null)
                    this.image.Dispose();
                this.image = null;
                if (this.stream != null)
                    this.stream.Close();
                this.stream = null;
            }

            #endregion
        }

        private void trackBar1_ValueChanged_1(object sender, EventArgs e)
        {
            //Values for the track bar are from 1-10
            //5 = 100%
            //every increment up/down add 10% to the size

            try
            {
                FileStream fs = new FileStream(this.lastImagePath, FileMode.Open);
                Size size = this.lastImage.Size;
                float factor = 1;
                switch (this.trackBar1.Value)
                {
                    case 1:
                        factor = -5;
                        break;
                    case 2:
                        factor = -4;
                        break;
                    case 3:
                        factor = -3;
                        break;
                    case 4:
                        factor = -2;
                        break;
                    case 5:
                        factor = 1;
                        break;
                    case 6:
                        factor = 2;
                        break;
                    case 7:
                        factor = 3;
                        break;
                    case 8:
                        factor = 4;
                        break;
                    case 9:
                        factor = 5;
                        break;
                    case 10:
                        factor = 6;
                        break;
                }
                size.Width += (int)((float)size.Width * (factor / 10f));
                size.Height += (int)((float)size.Height * (factor / 10f));

                IBitmapImage ibi = ImageUtils.CreateThumbnail(fs, size);
                fs.Close();
                Bitmap bmpThumb = ImageUtils.IBitmapImageToBitmap(ibi);

                this.imageViewer1.Image = bmpThumb;
            }
            catch
            {
            }
        }

        private void lnkTakePicture_Click(object sender, EventArgs e)
        {
            Microsoft.WindowsMobile.Forms.CameraCaptureDialog ccd = new Microsoft.WindowsMobile.Forms.CameraCaptureDialog();
            ccd.Mode = Microsoft.WindowsMobile.Forms.CameraCaptureMode.Still;
            ccd.Owner = this;
            ccd.Resolution = new Size(640, 480);
            ccd.StillQuality = Microsoft.WindowsMobile.Forms.CameraCaptureStillQuality.Normal;
            ccd.ShowDialog();
            this.trackBar1.Value = 5;
            this.LoadImage(ccd.FileName);
            ccd.Dispose();
        }
    }
}