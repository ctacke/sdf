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
using OpenNETCF.Drawing.Imaging;

namespace OpenNETCF.Drawing.Imaging.Samples
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamOnFile st = new StreamOnFile(openFileDialog1.FileName);
                IImageDecoder decoder = null;
                ImagingFactory factory = new ImagingFactoryClass();
                factory.CreateImageDecoder(st, DecoderInitFlag.DecoderInitFlagNone, out decoder);
                ImageProperty[] props = ImageUtils.GetAllProperties(decoder);
                foreach (ImageProperty prop in props)
                {
                    //For Specific tags see ImageTag enum
                    textBox1.Text = prop.Id.ToString() + ": " + GetValue(prop) + "\r\n" + textBox1.Text;
                }
                decoder.TerminateDecoder();
            }
        }

        private string GetValue(ImageProperty value)
        {
            if(value.GetValue().GetType() == typeof(byte[]))
            {
                //Modification may be needed here depending on your requirments since an ASCII string
                //is not always in the tag value.
                string s = Encoding.ASCII.GetString((byte[])value.GetValue(), 0, ((byte[])value.GetValue()).Length);
                if (s.IndexOf('\0') > 0)
                    return s.Substring(0, s.IndexOf('\0'));
                else
                    return s;
            }
            else
                return value.GetValue().ToString().Replace("\0", "");
        }
    }
}