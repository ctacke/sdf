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

namespace MSNSearchMobile
{
    public partial class Browser : Form
    {
        string link = "";
        public Browser(string link)
        {
            InitializeComponent();
            this.link = link;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Browser_Load(object sender, EventArgs e)
        {
            using (new OpenNETCF.Windows.Forms.Cursor2())
            {
                this.webBrowser1.Navigate(new Uri(this.link));
            }
        }
    }
}