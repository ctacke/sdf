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
using System.Threading;
using System.IO;

using OpenNETCF.Rss;

namespace MSNSearchMobile
{
    public partial class Main : Form
    {
        private event FeedReceivedHandler FeedReceived;

        private string searchTerm = "";
        private string searchTermFileName = "searchTerms.xml";

        public Main()
        {
            InitializeComponent();
            this.pictureBox1.Size = this.pictureBox1.Image.Size;
            this.pictureBox1.Top = this.cmbSearchTerm.Top - this.pictureBox1.Height;
            this.animateCtl1.Left = Screen.PrimaryScreen.Bounds.Width / 2 - this.animateCtl1.Bounds.Width / 2;
            this.animateCtl1.DrawDirection = OpenNETCF.Windows.Forms.DrawDirection.Vertical;
            string strAppDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            this.searchTermFileName = Path.Combine(strAppDir, searchTermFileName);
            this.LoadSearchTerms();
            this.FeedReceived+=new FeedReceivedHandler(Main_FeedReceived);
        }

        private void LoadSearchTerms()
        {
            if (File.Exists(this.searchTermFileName))
                this.searchTerms1.ReadXml(this.searchTermFileName);
        }

        private void SaveSearchTerm()
        {
            if (this.cmbSearchTerm.Text.Length > 0)
            {
                if (this.searchTerms1.SearchTerm.Select("Value = '" + this.cmbSearchTerm.Text + "'").Length == 0)
                {
                    this.searchTerms1.SearchTerm.AddSearchTermRow(this.cmbSearchTerm.Text);
                    this.searchTerms1.WriteXml(this.searchTermFileName);
                }
            }
        }

        void  Main_FeedReceived(OpenNETCF.Rss.Data.Feed feed)
        {
            this.Invoke(new FeedReceivedHandler(FeedReceivedInvoke), feed);
        }

        void FeedReceivedInvoke(OpenNETCF.Rss.Data.Feed feed)
        {
            this.EnableAnimation(false);

            if (feed != null)
            {
                using (SearchResults sr = new SearchResults(feed))
                {
                    sr.ShowDialog();
                }
            }
            else
                this.Display = "Unable to retreive results.";
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {

            //Save the params to get from another thread
            this.searchTerm = this.cmbSearchTerm.Text;

            //Save the search term
            this.SaveSearchTerm();
            this.cmbSearchTerm.Text = this.searchTerm;

            //Enable the animation
            this.EnableAnimation(true);

            //Start the thread
            Thread t = new Thread(new ThreadStart(FeedThread));
            t.Start();

            
        }

        private void SaveSearchString(string value)
        {
            
        }

        private void FeedThread()
        {
            OpenNETCF.Rss.Data.Feed feed = null;
            if (this.searchTerm.Length > 0)
            {
                string uri = string.Format("http://search.msn.com/results.aspx?q={0}&format=rss&FORM=RSRE", this.searchTerm);
                feed = FeedEngine.Receive(new Uri(uri));
            }

            if (this.FeedReceived != null)
                this.FeedReceived(feed);
        }

        private string Display
        {
            set
            {
                this.lblResponse.Visible = true;
                this.lblResponse.Text = value;
            }
        }
        private void EnableAnimation(bool enable)
        {
            if (enable)
            {
                this.lblResponse.Text = string.Format("Searching '{0}'", this.cmbSearchTerm.Text);
                this.animateCtl1.StartAnimation();
            }
            else
            {
                this.lblResponse.Text = "Searching Complete";
                this.animateCtl1.StopAnimation();
            }

            this.lblResponse.Visible = this.animateCtl1.Visible = enable;
        }

        

    }
}