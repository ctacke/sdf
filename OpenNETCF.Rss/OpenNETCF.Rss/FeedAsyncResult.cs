

using System;
using System.Text;
using System.IO;

using OpenNETCF.Rss.Data;

namespace OpenNETCF.Rss
{
	public class FeedAsyncResult : IAsyncResult
	{
		#region IAsyncResult Members

		private FeedInputChannel channel;

		public FeedAsyncResult(FeedInputChannel channel)
		{
			this.channel = channel;
		}

		public FeedInputChannel Channel
		{
			get
			{
				return channel;
			}
		}

		public object AsyncState
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public System.Threading.WaitHandle AsyncWaitHandle
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public bool CompletedSynchronously
		{
			get { return false; }
		}

		public bool IsCompleted
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion
}
}
