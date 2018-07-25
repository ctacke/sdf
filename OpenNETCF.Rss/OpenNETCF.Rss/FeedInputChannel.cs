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
		
		#endregion

		#region constructors
		
		/// <summary>
		/// Initializes a new instance of the FeedInputChannel class with the specified destination.
		/// </summary>
		/// <param name="destination">A RSS feed destination.</param>
		public FeedInputChannel(Uri destination)
		{
            if (destination == null) throw new ArgumentNullException();

			this.queue = new Queue();
			this.waitHandle = new ManualResetEvent(false);
			Destination = destination;
            Closed = false;
		} 

		#endregion

        #region IRssInputChannel Members

		public virtual IAsyncResult BeginReceive(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// The destination URL of the feed input
        /// </summary>
		public Uri Destination { get; private set; }

        /// <summary>
        /// Returns <b>true</b> if the connection is closed, otherwise <b>false</b>.
        /// </summary>
		public virtual bool Closed { get; private set; }

		#endregion

    }
}
