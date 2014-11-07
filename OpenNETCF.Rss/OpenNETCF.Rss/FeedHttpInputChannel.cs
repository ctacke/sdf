

using System;
using System.Collections.Specialized;
using System.Text;
using OpenNETCF.Rss.Data;

namespace OpenNETCF.Rss
{
	/// <summary>
	/// Defines an HTTP connection that allowes feeds received from a given destination.
	/// </summary>
	public class FeedHttpInputChannel : FeedInputChannel
	{
		#region constructors
		
		/// <summary>
		/// Initializes a new instance of the FeedHttpInputChannel class with the specified destination and transport. 
		/// </summary>
		/// <param name="destination">An Uri that represents the target destination for the channel.</param>
		/// <param name="transport">A FeedHttpTransport that represents the transport for the channel.</param>
		public FeedHttpInputChannel(Uri destination, FeedHttpTransport transport)
			: base(destination)
		{
			Transport = transport;
		} 
		
		#endregion


		#region public methods

		/// <summary>
		/// Starts asyncronous retreival of the RSS Feed.
		/// </summary>
		/// <param name="callback">An System.AsyncCallback that represents the callback method.</param>
		/// <param name="state">An object that can be used to access state information for the asynchronous operation.</param>
		/// <returns>The System.IAsyncResult that identifies the asynchronous request. </returns>
		public override IAsyncResult BeginReceive(AsyncCallback callback, object state)
		{
			//return base.BeginReceive(callback, state);
			return Transport.BeginReceive(this.Destination, callback, state);
		}

		/// <summary>
		/// Ends a pending asynchronous receive operation. 
		/// </summary>
		/// <param name="result">The System.IAsyncResult that identifies the asynchronous receive operation to finish.</param>
		/// <returns>A Feed object that is received. </returns>
		public override Feed EndReceive(IAsyncResult result)
		{
			return base.EndReceive(result);
		}

		/// <summary>
		/// Receives a RSS feed on the channel. 
		/// </summary>
		/// <returns>A Feed object.</returns>
		public override Feed Receive()
		{
			return Transport.Receive(this.Destination);
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets colection of input channels.
		/// </summary>
		public HybridDictionary InputChannels { get; private set; }
        private FeedHttpTransport Transport { get; set; }
		#endregion
	}
}
