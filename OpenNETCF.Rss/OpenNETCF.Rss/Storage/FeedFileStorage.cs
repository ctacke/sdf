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




using System;
using System.IO;
using System.Xml;
using OpenNETCF.Rss.Data;

namespace OpenNETCF.Rss
{
	/// <summary>
	/// Defines an Feed File storage.
	/// </summary>
	public class FeedFileStorage : IFeedStorage
	{
		#region fields

		private string path;
		private int feedSizeLimit;
		private FeedSerializer serializer;

		#endregion // fields


		#region constructors

		/// <summary>
		/// Initializes a new instance of the FeedInputChannel class
		/// </summary>
		public FeedFileStorage()
		{
			this.path = "";
			this.feedSizeLimit = 200; //200 KB
			this.serializer = new FeedSerializer();
		}

		/// <summary>
		/// Initializes a new instance of the FeedFileStorage class with a specified feed location path.
		/// </summary>
		/// <param name="path">Feed location path.</param>
		public FeedFileStorage(string path)
		{
			this.path = path;
			this.serializer = new FeedSerializer();
		}

		#endregion // constructors


		#region IFeedStorage Members

		/// <summary>
		/// Inits the storage provider.
		/// </summary>
		/// <param name="section">Represents a single node in the XML document</param>
		public void Init(XmlNode section)
		{
			string name = "";

			foreach (XmlNode node in section.ChildNodes)
			{
				XmlElement currentElement = node as XmlElement;

				name = currentElement.LocalName;
				if (name == "path")
				{
					this.path = currentElement.GetAttribute("value");
					continue;
				}

				if (name == "feedSizeLimit")
				{
					this.feedSizeLimit = Convert.ToInt32(currentElement.GetAttribute("value"));
					continue;
				}

			}

		}

		/// <summary>
		/// Adds a Feed into the storage.
		/// </summary>
		/// <param name="feed"></param>
		public void Add(Feed feed)
		{
			string feedPath = FeedPath(feed.Title);

			if (!File.Exists(feedPath))
			{
				using(FileStream fs = new FileStream(feedPath, FileMode.CreateNew, FileAccess.Write))
				{
					serializer.Serialize(feed, fs);
				}
			}
			else
			{	
				//throw new InvalidOperationException("Feed already exists.");
				this.Update(feed);
			}
		}

		/// <summary>
		/// Removes all elements from the storage.
		/// </summary>
		public void Flush()
		{
			string[] files = Directory.GetFiles(path, "*.xml");
			foreach(string file in files)
			{
				File.Delete(file);
			}
		}

		/// <summary>
		/// Gets the Feed with the specified key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns>The Feed object.</returns>
		public Feed GetFeed(string key)
		{
			Feed feed = null;
			string feedPath = this.FeedPath(key);
		
			if (File.Exists(feedPath))
			{
				using(FileStream fs = new FileStream(feedPath, FileMode.Open, FileAccess.Read))
				{
					feed = serializer.Deserialize(fs);
				}
			}

			return feed;
		}

		/// <summary>
		/// Gets the Feed with the specified destination Uri.
		/// </summary>
        /// <param name="destination"></param>
		/// <returns>The Feed object.</returns>
		public Feed GetFeed(Uri destination)
		{
			Feed feed = null;
			string[] files = Directory.GetFiles(path, "*.xml");

			foreach(string file in files)
			{
				using(FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
				{
					feed = serializer.Deserialize(fs);
					if (feed.Link == destination.ToString())
					{
						break;
					}
				}
			}

			return feed;
		}

		/// <summary>
		/// Removes the Feed with the specified key.
		/// </summary>
		/// <param name="key">The unique key.</param>
		public void Remove(string key)
		{
			string feedPath = this.FeedPath(key);
			if (File.Exists(feedPath))
			{
				File.Delete(feedPath);
			}
		}

		/// <summary>
		/// Updates the Feed with the specified key.
		/// </summary>
		/// <param name="feed">The Feed to update.</param>
		public int Update(Feed feed)
		{
			string feedPath = this.FeedPath(feed.Title);

			bool updatedFlag = false;

			int updateCount = 0;

			if (File.Exists(feedPath))
			{
				FileStream fs = new FileStream(feedPath, FileMode.Open, FileAccess.ReadWrite);
				int fileSize = (int)fs.Length;
				Feed origFeed = serializer.Deserialize(fs);

				if (fileSize > this.FeedSizeLimit)
				{
					this.TruncateFeed(feed, fileSize);
				}

				fs.Close();

				foreach(FeedItem item in feed.Items)
				{
					if (!origFeed.Items.Contains(item.Id))
					{
						// Add new item
						item.DirtyFlag = true;
						origFeed.Items.Insert(0, item);
						updatedFlag = true;
						updateCount++;
					}
					else // item exists
					{
						// check the pubDate
						if (item.PubDate.CompareTo(origFeed.PubDate) > 0)
						{
							// Update
							item.DirtyFlag = true;
							origFeed.Items[item.Id] = item;
							updatedFlag = true;
							updateCount++;
						}
					}
				}

				if (updatedFlag)
				{
					fs = new FileStream(feedPath, FileMode.Create, FileAccess.Write);
					serializer.Serialize(origFeed, fs);
					fs.Close();
				}	
			}
			else
			{	
				this.Add(feed);
			}

			return updateCount;
		}

		/// <summary>
		/// Gets the number of Feeds in the storage.
		/// </summary>
		public int Size
		{
			get
			{
				//DirectoryInfo dirInfo = new DirectoryInfo(path + "\\*.xml");
				string[] files = Directory.GetFiles(path, "*.xml");
				return files.Length;
			}
		}

		#endregion

		private int FeedSizeLimit
		{
			get
			{
				return feedSizeLimit * 1024;
			}
		}
			
		private void TruncateFeed(Feed feed, int currentSize)
		{
			int size = 0;
			// start from the end
			for(int i=feed.Items.Count - 1;i>0;i--)
			{
				size+=feed.Items[i].Description.Length + 200;
				if ((currentSize - size) < this.FeedSizeLimit)
				{
					feed.Items.Remove(feed.Items[i]);
					break;
				}
				else
				{
					feed.Items.Remove(feed.Items[i]);
				}
			}
			

		}

		private int GetFeedSize(Feed feed)
		{
			MemoryStream stream = new MemoryStream();
			serializer.Serialize(feed, stream);
			int size = stream.ToArray().Length;
			stream.Close();
			return size;
		}

		private string FeedPath(string feedTitle)
		{
			return Path.Combine(path, feedTitle.Trim() + ".xml");
		}
	}
}
