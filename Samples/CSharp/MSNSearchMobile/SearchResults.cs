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

using OpenNETCF.Rss;
namespace MSNSearchMobile
{
    public partial class SearchResults : Form
    {
        private OpenNETCF.Rss.Data.Feed feed;
        public SearchResults(OpenNETCF.Rss.Data.Feed feed)
        {
            this.feed = feed;
            InitializeComponent();

            this.msnSearchResultsList1.BeginUpdate();
            this.msnSearchResultsList1.DataSource = feed.Items;
            this.msnSearchResultsList1.EndUpdate();
        }

        private void miBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void miOpenLink_Click(object sender, EventArgs e)
        {
            if (this.msnSearchResultsList1.SelectedIndex > -1)
            {
                string link = ((OpenNETCF.Rss.Data.FeedItemCollection)this.msnSearchResultsList1.DataSource)[this.msnSearchResultsList1.SelectedIndex].Link;
                if (link.Length > 0)
                {
                    using (Browser b = new Browser(link))
                    {
                        b.ShowDialog();
                    }
                }
            }
        }
    }

   
}