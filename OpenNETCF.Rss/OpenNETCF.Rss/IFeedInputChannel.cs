
using System;
using System.Collections;
using System.Text;

using OpenNETCF.Rss.Data;

namespace OpenNETCF.Rss
{
    /// <summary>
	/// Defines the members for a feed input channel. 
    /// </summary>
	public interface IFeedInputChannel : IFeedChannel
    {
        /// <summary>
		/// Starts an asynchronous receive operation. 
        /// </summary>
        /// <param name="callback">An System.AsyncCallback that represents the callback method.</param>
        /// <param name="state">An object that can be used to access state information of the asynchronous operation.</param>
		/// <returns>The System.IAsyncResult that identifies the asynchronous request. </returns>
		IAsyncResult BeginReceive(AsyncCallback callback, object state);

		/// <summary>
		/// Ends a pending asynchronous receive operation. 
		/// </summary>
		/// <param name="result">The System.IAsyncResult that identifies the asynchronous receive operation to finish.</param>
		/// <returns>A Feed object that is received. </returns>
        Feed EndReceive(IAsyncResult result);

		/// <summary>
		/// Receives a message on the channel. 
		/// </summary>
		/// <returns>A Feed object that is received.</returns>
        Feed Receive();
    }
}
