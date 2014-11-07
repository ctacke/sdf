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
using System.Windows.Forms;
using System.Data;
using System.Threading;

using OpenNETCF.Windows.Forms;
using OpenNETCF.Rss;
using OpenNETCF.Rss.Data;

namespace OpenRSS
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.TreeView treeFeeds;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.ListView listFeeds;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.ColumnHeader colTitle;
		private System.Windows.Forms.ColumnHeader colDate;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem5;


		private Feed currentFeed;
		internal string opmlPath;
		private int loadProgress;

		private frmDetails formDetails;

		private EventHandler startProgressHandler;
		private EventHandler endProgressHandler;
		private EventHandler updateProgressHandler;
		private EventHandler addNodeHandler;
		private EventHandler updateNodeHandler;


		private System.Windows.Forms.MenuItem menuItem7;
		
		private Process process;
		

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
	

			addNodeHandler = new EventHandler(AddTreeNode);
			updateNodeHandler = new EventHandler(UpdateTreeNode);
			startProgressHandler = new EventHandler(StartProgress);
			endProgressHandler = new EventHandler(EndProgress);
			updateProgressHandler = new EventHandler(UpdateProgress);
			
			//Set fonts
			listFeeds.Font = new Font("Tahoma", 8F, FontStyle.Bold);
			treeFeeds.Font = new Font("Tahoma", 8F, FontStyle.Bold);
			
			
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMain));
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.treeFeeds = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.listFeeds = new System.Windows.Forms.ListView();
			this.colTitle = new System.Windows.Forms.ColumnHeader();
			this.colDate = new System.Windows.Forms.ColumnHeader();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.Add(this.menuItem1);
			this.mainMenu1.MenuItems.Add(this.menuItem4);
			// 
			// menuItem1
			// 
			this.menuItem1.MenuItems.Add(this.menuItem2);
			this.menuItem1.MenuItems.Add(this.menuItem3);
			this.menuItem1.Text = "File";
			// 
			// menuItem2
			// 
			this.menuItem2.Text = "Open";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Text = "Exit";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.MenuItems.Add(this.menuItem7);
			this.menuItem4.MenuItems.Add(this.menuItem6);
			this.menuItem4.MenuItems.Add(this.menuItem5);
			this.menuItem4.Text = "Tools";
			// 
			// menuItem7
			// 
			this.menuItem7.Text = "Add New Feed";
			this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Text = "Update All Feeds";
			this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Text = "Options";
			// 
			// treeFeeds
			// 
			this.treeFeeds.ImageIndex = 2;
			this.treeFeeds.ImageList = this.imageList;
			this.treeFeeds.SelectedImageIndex = 4;
			this.treeFeeds.Size = new System.Drawing.Size(240, 120);
			this.treeFeeds.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeFeeds_AfterSelect);
			// 
			// imageList
			// 
			this.imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource"))));
			this.imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource1"))));
			this.imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource2"))));
			this.imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource3"))));
			this.imageList.Images.Add(((System.Drawing.Image)(resources.GetObject("resource4"))));
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "*.opml|(*.opml)|*.xml|(*.xml)";
			// 
			// listFeeds
			// 
			this.listFeeds.Columns.Add(this.colTitle);
			this.listFeeds.Columns.Add(this.colDate);
			this.listFeeds.FullRowSelect = true;
			this.listFeeds.Location = new System.Drawing.Point(0, 120);
			this.listFeeds.Size = new System.Drawing.Size(240, 149);
			this.listFeeds.SmallImageList = this.imageList;
			this.listFeeds.View = System.Windows.Forms.View.Details;
			this.listFeeds.SelectedIndexChanged += new System.EventHandler(this.listFeeds_SelectedIndexChanged);
			// 
			// colTitle
			// 
			this.colTitle.Text = "Title";
			this.colTitle.Width = 161;
			// 
			// colDate
			// 
			this.colDate.Text = "Date";
			this.colDate.Width = 74;
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(0, 248);
			this.progressBar.Size = new System.Drawing.Size(240, 20);
			// 
			// frmMain
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.listFeeds);
			this.Controls.Add(this.treeFeeds);
			this.Controls.Add(this.progressBar);
			this.Menu = this.mainMenu1;
			this.Text = "OpenRSS";
			this.Load += new System.EventHandler(this.frmMain_Load);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		static void Main() 
		{
			Application.Run(new frmMain());
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			FeedEngine.Stop();
			Application.Exit();
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			openFileDialog.ShowDialog();
			if (openFileDialog.FileName != "")
			{
				Application.DoEvents();
				//StartLoadOpmlThread();
			}
		}

		private void LoadOpml()
		{	
			//Load opml
			int saveHeight = listFeeds.Height;
			//listFeeds.Height = saveHeight - 20;

			Cursor.Current = Cursors.WaitCursor;

			Opml opml = FeedEngine.LoadOpml(opmlPath);
			
			int count = opml.Items.Length;

			this.Invoke(startProgressHandler);
			// Populate TreeView
			for(int i=0;i<count;i++)
			{
				//TreeNode node = new TreeNode(opmlItem.Title);
				//node.Tag = opmlItem.Title;
				OpmlItem opmlItem = (OpmlItem)opml.Items[i];
				currentFeed = FeedEngine.Storage.GetFeed(opmlItem.Title);
				if (currentFeed == null)
				{
					currentFeed = FeedEngine.Receive(new Uri(opmlItem.XmlUrl));
					FeedEngine.Storage.Add(currentFeed);
				}
				loadProgress = (int)((double)i/count * 100);
				this.Invoke(updateProgressHandler);
				this.Invoke(addNodeHandler);
			}
			
			FeedEngine.SubscribeOpml(opml);

			this.Invoke(endProgressHandler);
			//listFeeds.Height = saveHeight;
			Cursor.Current = Cursors.Default;
			//FeedEngine.Start();
			
		}

		#region invoke callers

		public void StartProgress()
		{
			this.Invoke(startProgressHandler);
		}

		public void EndProgress()
		{
			this.Invoke(endProgressHandler);
		}

		public void UpdateProgress()
		{
			this.Invoke(updateProgressHandler);
		}

		public void AddTreeNode()
		{
			this.Invoke(addNodeHandler);
		}

		public void UpdateTreeNode()
		{
			this.Invoke(updateNodeHandler);
		}


		#endregion

		#region invoke handlers

		private void StartProgress(object sender, System.EventArgs e)
		{
			progressBar.Value = 0;
			listFeeds.Height = listFeeds.Height - 20;
		}

		private void EndProgress(object sender, System.EventArgs e)
		{
			listFeeds.Height = listFeeds.Height + 20;
		}

		private void UpdateProgress(object sender, System.EventArgs e)
		{
			progressBar.Value = process.LoadProgress;
		}

		private void AddTreeNode(object sender, System.EventArgs e)
		{
			TreeNode node = new TreeNode(process.CurrentFeed.Title);
			node.Tag = process.CurrentFeed.Title;
			node.Text+= " (" + process.CurrentFeed.Items.Count.ToString() + ")";
			treeFeeds.Nodes.Add(node);
		}

		private void UpdateTreeNode(object sender, System.EventArgs e)
		{
			foreach(TreeNode node in treeFeeds.Nodes)
			{
				// Change the updated feed's icon and count
				if (node.Tag.ToString() == process.CurrentFeed.Title)
				{
					node.ImageIndex = 3;
					node.Text = process.CurrentFeed.Title + " (" + process.CurrentFeed.Items.Count.ToString() + "/" + process.UpdateCount.ToString() + ")";
					break;
				}
			}
			
		}

		#endregion

		private void FeedEngine_FeedReceived(Feed feed)
		{
			currentFeed = feed;

			this.Invoke(new EventHandler(ShowFeed));

			
		}

		private void ShowFeed(object sender, System.EventArgs e)
		{
			// Find the node in the TreeView
			foreach(TreeNode node in treeFeeds.Nodes)
			{
				if (node.Text.ToString() == currentFeed.Title)
				{
					foreach(FeedItem feedItem in currentFeed.Items)
					{
						TreeNode feedNode = new TreeNode(feedItem.Title);
						feedNode.Tag = feedItem;
						node.Nodes.Add(feedNode);
					}
					break;
				}
			}
		}

		public void LoadList(Feed feed)
		{
			if (feed == null)
				return;

			Cursor.Current = Cursors.WaitCursor;

			listFeeds.Items.Clear();

			foreach(FeedItem feedItem in feed.Items)
			{
				ListViewItem listItem = new ListViewItem(feedItem.Title);
				listItem.SubItems.Add(feedItem.PubDate.Date.ToString());
				if (feedItem.DirtyFlag)
				{
					listItem.ImageIndex = 0;
				}
				else
				{
					listItem.ImageIndex = 1;
				}
				listFeeds.Items.Add(listItem);
			}
				
			Cursor.Current = Cursors.Default;
		}


		private void LoadTreeNode(TreeNode node, Feed feed)
		{
		
			foreach(FeedItem feedItem in feed.Items)
			{
				TreeNode feedNode = new TreeNode(feedItem.Title);
				feedNode.Tag = feedItem;
				node.Nodes.Add(feedNode);
			}

		}

		private void treeFeeds_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			currentFeed = FeedEngine.Storage.GetFeed(e.Node.Tag.ToString());
			LoadList(currentFeed);
		}

		

		private void listFeeds_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (listFeeds.SelectedIndices.Count > 0)
			{
				int selectedIndex = listFeeds.SelectedIndices[0];
				listFeeds.Items[selectedIndex].ImageIndex = 0;
				formDetails.ShowFeed(currentFeed, selectedIndex);
				formDetails.Show();
			}
		}


		private void frmMain_Load(object sender, System.EventArgs e)
		{
			opmlPath = Application2.StartupPath + @"\news.opml";
			formDetails = new frmDetails();
			formDetails.Hide();
			process = new Process(this);
			process.LoadOpml();
			this.BringToFront();
			this.Focus();
		}

		private void menuItem6_Click(object sender, System.EventArgs e)
		{
			process.UpdateAllFeeds();
		}

		private void menuItem7_Click(object sender, System.EventArgs e)
		{
			AddFeedDialog addFeedDial = new AddFeedDialog();
			addFeedDial.ShowDialog();
			process.AddFeed(addFeedDial.Url);
			process.UpdateOpml(process.CurrentFeed, addFeedDial.Url);
		}
	}
}
