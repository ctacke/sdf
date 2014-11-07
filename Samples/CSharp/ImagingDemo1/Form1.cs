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
using Microsoft.WindowsMobile.Forms;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//
using OpenNETCF.Drawing.Imaging;

namespace ImagingDemo1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void mnuLoadDirect_Click(object sender, EventArgs e)
        {
            SelectPictureDialog dlg = new SelectPictureDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pbImage.Image = new Bitmap(dlg.FileName);
                }
                catch (OutOfMemoryException)
                {
                    MessageBox.Show("Out of memory");
                }
            }
        }

        private void mnuLoadImaging_Click(object sender, EventArgs e)
        {
            SelectPictureDialog dlg = new SelectPictureDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ImagingFactory factory = new ImagingFactoryClass();
                    IImage img;
                    factory.CreateImageFromFile(dlg.FileName, out img);
                    IBitmapImage imgB;
                    factory.CreateBitmapFromImage(img, 
                        (uint)pbImage.Width, 
                        (uint)pbImage.Height, 
                        System.Drawing.Imaging.PixelFormat.Format24bppRgb, 
                        InterpolationHint.InterpolationHintDefault, 
                        out imgB);
                    pbImage.Image = ImageUtils.IBitmapImageToBitmap(imgB);
                }
                catch (OutOfMemoryException)
                {
                    MessageBox.Show("Out of memory");
                }
            }
        }
    }
}