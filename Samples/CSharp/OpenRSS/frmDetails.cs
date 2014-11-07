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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using OpenNETCF.Windows.Forms;
using OpenNETCF.Rss;
using OpenNETCF.Rss.Data;


namespace OpenRSS
{
	/// <summary>
	/// Summary description for frmDetails.
	/// </summary>
	public class frmDetails : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.WebBrowser htmlDetails;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.ToolBar toolBar;
		private System.Windows.Forms.ToolBarButton butDown;
		private System.Windows.Forms.ToolBarButton butUp;
		private Feed feed;
		private int currentIndex;
	
		public frmDetails()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}

		public void ShowFeed(Feed feed, int index)
		{
			this.feed = feed;
			
			if ((index < 0) || (index >= this.feed.Items.Count)) 

				return;

			currentIndex = index;
			
			FeedItem feedItem = this.feed.Items[index];
			
			feedItem.DirtyFlag = true;
			
			if (feedItem == null)
				return;
			
			string html = "<html><head><title>" + " " +  "</title>";
			html += "<p><h3>" + feedItem.Title + "</h3></p>";
			html += "<base href='" + feedItem.Parent.Link + "' /><body>";
			html += feedItem.Description;
			html += "</body></html>";
            htmlDetails.Text = html;
			
		}

		public void ShowFeed(int index)
		{
			if (this.feed == null)
				return;

			if (index < 0) 
			{
				index = 0;
				return;
			}

			if (index >= this.feed.Items.Count)
			{
				index = this.feed.Items.Count - 1;
				return;
			}

			FeedItem feedItem = this.feed.Items[index];

			if (feedItem == null)
				return;
			
			string html = "<html><head><title>" + " " +  "</title>";

			html += "<p><h3>" + feedItem.Title + "</h3></p>";
			

			html += "<base href='" + feedItem.Parent.Link + "' /><body>";

			html += feedItem.Description;
			html += "</body></html>";
            htmlDetails.Text = html;			
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmDetails));
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.htmlDetails = new System.Windows.Forms.WebBrowser();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.imageList = new System.Windows.Forms.ImageList();
			this.toolBar = new System.Windows.Forms.ToolBar();
			this.butDown = new System.Windows.Forms.ToolBarButton();
			this.butUp = new System.Windows.Forms.ToolBarButton();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.Add(this.menuItem1);
			// 
			// menuItem1
			// 
			this.menuItem1.MenuItems.Add(this.menuItem2);
			this.menuItem1.Text = "View";
			// 
			// htmlDetails
			// 			
			this.htmlDetails.Size = new System.Drawing.Size(240, 269);			
			// 
			// menuItem2
			// 
			this.menuItem2.Text = "Options";
			// 
			// imageList
			// 
			this.imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource"))));
			this.imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource1"))));
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			// 
			// toolBar
			// 
			this.toolBar.Buttons.Add(this.butDown);
			this.toolBar.Buttons.Add(this.butUp);
			this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			// 
			// frmDetails
			// 
			this.Controls.Add(this.htmlDetails);
			this.Controls.Add(this.toolBar);
			this.MaximizeBox = false;
			this.Menu = this.mainMenu1;
			this.MinimizeBox = false;
			this.Text = "Feed Details";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmDetails_Closing);
			this.Load += new System.EventHandler(this.frmDetails_Load);

		}
		#endregion

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			
		}

		private void frmDetails_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
		}

		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch(toolBar.Buttons.IndexOf(e.Button))
			{
				case 0:
					currentIndex--;
					ShowFeed(currentIndex);
					break; 
				case 1:
					currentIndex++;
					ShowFeed(currentIndex);
					break; 
			}

		}

		private void frmDetails_Load(object sender, System.EventArgs e)
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmDetails));
			this.imageList.Images.Clear();
			Bitmap icon1 = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".images.PageDown.bmp")); 
			this.imageList.Images.Add(icon1);

			Bitmap icon2 = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".images.PageUp.bmp")); 
			this.imageList.Images.Add(icon2);

			this.toolBar.ImageList = this.imageList;

			this.butDown.ImageIndex = 1;
			this.butUp.ImageIndex = 0;
		}
	}
}
