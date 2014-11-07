
using System;
using System.Collections;
using System.Text;

using OpenNETCF.Rss.Data;

namespace OpenNETCF.Rss
{
	/// <summary>
	/// Provides a base class for receiving RSS feeds.
	/// </summary>
	public abstract class FeedReceiver
	{
		/// <summary>
		/// A callback that receives RSS feed.
		/// </summary>
		/// <param name="feed"></param>
		public virtual void Receive(Feed feed)
		{
		
		}
	
	}
}
