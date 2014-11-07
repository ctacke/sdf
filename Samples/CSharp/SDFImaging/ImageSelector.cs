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

namespace OpenNETCF.Samples.Imaging
{
    public partial class ImageSelector : Form
    {
        private string file = "";
        public ImageSelector()
        {
            using (new Cursor2())
            {
                InitializeComponent();

            }
        }

        public string FileSelected
        {
            get
            {
                return this.file;
            }
        }

        private void ImageSelector_Closing(object sender, CancelEventArgs e)
        {

        }

        private void documentList1_DocumentActivated_1(object sender, Microsoft.WindowsCE.Forms.DocumentListEventArgs e)
        {
            file = e.Path;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    }
}