
using System;
using System.Threading;
using System.Collections;
using System.Text;

using OpenNETCF.Rss.Data;

namespace OpenNETCF.Rss
{
    /// <summary>
	/// Defines a connection that allows RSS feeds to be received from a given destination. 
    /// </summary>
	public class FeedInputChannel : IFeedInputChannel
    {
		#region fields
		
		private Queue queue;
		private ManualResetEvent waitHandle;
		private Uri destination;
		private bool closed; 
		
		#endregion

		#region constructors
		
		/// <summary>
		/// Initializes a new instance of the FeedInputChannel class with the specified destination.
		/// </summary>
		/// <param name="destination">A RSS feed destination.</param>
		public FeedInputChannel(Uri destination)
		{
			this.queue = new Queue();
			this.waitHandle = new ManualResetEvent(false);
			this.destination = destination;
			this.closed = false;
		} 

		#endregion

        #region IRssInputChannel Members

		public virtual IAsyncResult BeginReceive(AsyncCallback callback, object state)
        {
			IAsyncResult result = null;
			//if (this.Closed)
			//{
			//    throw new InvalidOperationException("closed");
			//}
			//lock (this.queue.SyncRoot)
			//{
			//    result = new WaitHandleAsyncResult(this.waitHandle, callback, state);
			//}
			return result;
        }

		public virtual Feed EndReceive(IAsyncResult result)
        {
			Feed feedReturn;
			if (result == null)
			{
				throw new ArgumentNullException("result");
			}
			lock (this.queue.SyncRoot)
			{
				Feed feed = this.queue.Dequeue() as Feed;
				if (this.queue.Count == 0)
				{
					this.waitHandle.Reset();
				}
				feedReturn = feed;
			}
			return feedReturn;
        }

		public virtual void Enqueue(Feed feed)
		{
			//if (this.Closed)
			//{
			//    throw new InvalidOperationException(Microsoft.Web.Services2.SR.GetString("WSE801"));
			//}
			//if (message == null)
			//{
			//    throw new ArgumentNullException("message");
			//}
			lock (this.queue.SyncRoot)
			{
				this.queue.Enqueue(feed);
				if (this.queue.Count == 1)
				{
					this.waitHandle.Set();
				}
			}
		}

        public virtual Feed Receive()
        {
			Feed feed;
			this.waitHandle.WaitOne();
			lock (this.queue.SyncRoot)
			{
				Feed feedResult = this.queue.Dequeue() as Feed;
				if (this.queue.Count == 0)
				{
					this.waitHandle.Reset();
				}
				feed = feedResult;
			}

			return feed;
        }

        #endregion

        #region IRssChannel Members

        public void Close()
        {
            
        }

        #endregion

		#region properties

		public Uri Destination
		{
			get
			{
				return destination;
			}
		}

		public virtual bool Closed
		{
			get
			{
				return this.closed;
			}
		} 

		#endregion

    }
}
