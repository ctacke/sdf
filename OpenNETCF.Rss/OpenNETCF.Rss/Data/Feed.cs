
using System;
using System.Collections;
using System.Text;

namespace OpenNETCF.Rss.Data
{
	/// <summary>
	/// Identifies the Feed of a RSS feed.
	/// </summary>
	public class Feed
	{
		#region fields
		
		private string title;
		private string link;
		private string description;
		//private string language;
		private DateTime pubDate;
		private DateTime lastBuildDate;
		private string copyright; 
		//private FeedItem[] items;

		private FeedItemCollection items;


		#endregion

		#region contructor
		
		/// <summary>
		/// Initializes a new instance of the Feed class.
		/// </summary>
		public Feed()
		{
			title = String.Empty;
			link = String.Empty;
			description = String.Empty;
			copyright = String.Empty;
			pubDate = DateTime.MaxValue;
			lastBuildDate = DateTime.MaxValue;
			//items = null;
			items = new FeedItemCollection();
		} 

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets feed items.
		/// </summary>
		public FeedItemCollection Items
		{
			get
			{
				return items;
			}
		}

//		/// <summary>
//		/// Gets or sets feed items.
//		/// </summary>
//		public FeedItem[] Items
//		{
//			get
//			{
//				return items;
//			}
//			set
//			{
//				items = value;
//			}
//		}

		/// <summary>
		/// Gets or sets feed title.
		/// </summary>
		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
			}
		}

		/// <summary>
		/// Gets or sets feed link.
		/// </summary>
		public string Link
		{
			get
			{
				return link;
			}
			set
			{
				link = value;
			}
		}


		/// <summary>
		/// Gets or sets feed description.
		/// </summary>
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}

		/// <summary>
		/// Gets or sets feed copyright.
		/// </summary>
		public string Copyright
		{
			get
			{
				return copyright;
			}
			set
			{
				copyright = value;
			}
		}


		/// <summary>
		/// Gets or sets feed pubDate.
		/// </summary>
		public DateTime PubDate
		{
			get
			{
				return pubDate;
			}
			set
			{
				pubDate = value;
			}
		}

		/// <summary>
		/// Gets or sets feed LastBuildDate.
		/// </summary>
		public DateTime LastBuildDate
		{
			get
			{
				return lastBuildDate;
			}
			set
			{
				lastBuildDate = value;
			}
		} 

		#endregion

	}
}
