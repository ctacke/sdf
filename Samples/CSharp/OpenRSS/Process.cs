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
using System.Collections;
using System.Windows.Forms;
using System.Threading;

using OpenNETCF.Windows.Forms;
using OpenNETCF.Rss;
using OpenNETCF.Rss.Data;

namespace OpenRSS
{
	/// <summary>
	/// Summary description for Process.
	/// </summary>
	public class Process
	{
		private Feed currentFeed;
		private string opmlPath;
		private Thread workerThread;
		private frmMain mainForm;
		private int loadProgress;
		private Opml opml;
		private int updateCount;

		public Process(frmMain mainForm)
		{
			this.mainForm = mainForm;
			this.opmlPath = mainForm.opmlPath;
			//Subscribe to the event
			FeedEngine.FeedReceived += new FeedReceivedHandler(FeedEngine_FeedReceived);
		}

		public void LoadOpml()
		{
			ThreadStart threadStart = new ThreadStart(LoadOpmlThread);
			workerThread = new Thread(threadStart);
			workerThread.Start();
		}
	
		public void UpdateOpml(Feed feed, string url)
		{
			//FeedEngine.UpdateOpml(opmlPath, feed, url);
		}

		public void UpdateAllFeeds()
		{
			ThreadStart threadStart = new ThreadStart(UpdateAllFeedsThread);
			workerThread = new Thread(threadStart);
			workerThread.Start();

		}

		private void UpdateAllFeedsThread()
		{
			Cursor.Current = Cursors.WaitCursor;

			mainForm.StartProgress();

			for(int i=0;i<opml.Items.Length;i++)
			{
				Feed feed = FeedEngine.Receive(new Uri(opml.Items[i].XmlUrl));
				if (feed != null)
				{
					updateCount = FeedEngine.Storage.Update(feed);
					this.currentFeed = feed;
					mainForm.UpdateTreeNode();
				}
				this.loadProgress = (int)((double)i/opml.Items.Length * 100);

				mainForm.UpdateProgress();
			}
			
			mainForm.EndProgress();
			
			Cursor.Current = Cursors.Default;
		}

		private void LoadOpmlThread()
		{	
		
			Cursor.Current = Cursors.WaitCursor;

			opml = FeedEngine.LoadOpml(opmlPath);
			
			int count = opml.Items.Length;

			mainForm.StartProgress();

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
					updateCount = currentFeed.Items.Count;
					mainForm.UpdateTreeNode();
					FeedEngine.Storage.Add(currentFeed);
				}
				this.loadProgress = (int)((double)i/count * 100);
				mainForm.UpdateProgress();
				mainForm.AddTreeNode();
			}
			
			FeedEngine.SubscribeOpml(opml);

			mainForm.EndProgress();

			Cursor.Current = Cursors.Default;

			FeedEngine.Start();
		}

		public void AddFeed(string url)
		{
			Feed feed = FeedEngine.Receive(new Uri(url));
			if (feed != null)
			{
				this.currentFeed = feed;
				// Add to storage
				FeedEngine.Storage.Add(feed);
				//Update the treeview
				mainForm.AddTreeNode();
			}
		}

		public int UpdateCount
		{
			get
			{
				return updateCount;
			}
		}
	
		public int LoadProgress
		{
			get
			{
				return loadProgress;
			}
		}

		public Feed CurrentFeed
		{
			get
			{
				return currentFeed;
			}
			set
			{
				currentFeed = value;
			}
		}

		private void FeedEngine_FeedReceived(Feed feed)
		{
			currentFeed = feed;
			updateCount = FeedEngine.Storage.Update(currentFeed);
			if (updateCount > 0)
			{
				mainForm.UpdateTreeNode();
			}

		}
	}
}
