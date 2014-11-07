

using System;
using System.Collections;
using System.Text;

using OpenNETCF.Rss.Configuration;
using OpenNETCF.Rss.Data;



namespace OpenNETCF.Rss
{
	#region delegates

	public delegate void FeedReceivedHandler(Feed feed);
	internal delegate void ReceiveHandler(Uri destination);

	#endregion

	/// <summary>
	/// Defines an engine that subscribes to RSS feeds. 
	/// </summary>
	public class FeedEngine : IDisposable
	{
		#region events

		/// <summary>
		///  Represents the method that will handle the FeedReceivedHandler event.
		/// </summary>
		public static event FeedReceivedHandler FeedReceived;

		#endregion // events


		#region fields

		private static Hashtable transports;
		private static Hashtable channels;
		private static Hashtable receivers;
		private static FeedWorker feedWorker;
		private bool disposed = false;
		private static int refreshInterval = 60000;

		#endregion

		#region constructors

		static FeedEngine()
		{
			FeedEngine.transports = new Hashtable();
			FeedEngine.channels = new Hashtable();
			FeedEngine.receivers = new Hashtable();

			FeedEngine.refreshInterval = FeedConfiguration.TransportConfiguration.RefreshInterval;
			FeedEngine.Storage = FeedConfiguration.StorageConfiguration.GetProvider();
			FeedEngine.feedWorker = new FeedWorker(new ReceiveHandler(OnReceive), refreshInterval);
		} 
	
		#endregion


		#region public methods

		/// <summary>
		/// Loads the OPML file.
		/// </summary>
		/// <param name="opmlPath">The full path to the OPML file.</param>
		/// <returns>The Opml object.</returns>
		public static Opml LoadOpml(string opmlPath)
		{
            if (opmlPath == null) throw new ArgumentNullException();
            if (opmlPath == string.Empty) throw new ArgumentException();

			OpmlParser parser = new OpmlParser();
			Opml opml = parser.Process(opmlPath);
			return opml;
		}

		/// <summary>
		/// Subscribes to the RSS feeds supplied in the OPML object.
		/// </summary>
		/// <param name="opml">The Opml object.</param>
		/// <returns>The Opml object.</returns>
		public static Opml SubscribeOpml(Opml opml)
		{
            if (opml == null) throw new ArgumentNullException();
            
            foreach (OpmlItem item in opml.Items)
			{
				FeedEngine.Subscribe(new Uri(item.XmlUrl));
			}

			return opml;
		}



		/// <summary>
		/// Subscribes to the RSS feeds supplied in the OPML file.
		/// </summary>
		/// <param name="opmlPath">The full path to the OPML file.</param>
		/// <returns>The Opml object.</returns>
		public static Opml SubscribeOpml(string opmlPath)
		{
            if (opmlPath == null) throw new ArgumentNullException();
            if (opmlPath == string.Empty) throw new ArgumentException();
            
            OpmlParser parser = new OpmlParser();

			Opml opml = parser.Process(opmlPath);

			foreach (OpmlItem item in opml.Items)
			{
				FeedEngine.Subscribe(new Uri(item.XmlUrl));
			}

			return opml;
		}

		/// <summary>
		/// Subscribes to a RSS feed.
		/// </summary>
		/// <param name="destination">The Uri destination object.</param>
		public static void Subscribe(Uri destination)
		{
            if (destination == null) throw new ArgumentNullException();
            
            IFeedInputChannel channel = null;

			if (channels[destination] == null)
			{
				channel = FeedTransport.StaticGetInputChannel(destination);
				if (channel != null)
				{
					channels.Add(destination, channel);
					feedWorker.AddFeed(destination);
				}
			}
			else
			{
				channel = channels[destination] as IFeedInputChannel;
			}
		}

		/// <summary>
		/// Subscribes to a RSS feed
		/// </summary>
		/// <param name="destination">The Uri destination object.</param>
		/// <param name="receiver">The FeedReceiver that will receive the RSS feed.</param>
		public static void Subscribe(Uri destination, FeedReceiver receiver)
		{
            if (destination == null) throw new ArgumentNullException("destination");
            if (receiver == null) throw new ArgumentNullException("receiver");

            IFeedInputChannel channel = null;

			if (channels[destination] == null)
			{
				channel = FeedTransport.StaticGetInputChannel(destination);
				if (channel != null)
				{
					channels.Add(destination, channel);
					receivers.Add(destination, receiver);
					feedWorker.AddFeed(destination);
				}
			}
			else
			{
				channel = channels[destination] as IFeedInputChannel;

			}

			channel.BeginReceive(new AsyncCallback(FeedEngine.OnReceiveComplete), destination);

		}

		/// <summary>
		/// Starts the process of retreiving the RSS feeds.
		/// </summary>
		public static void Start()
		{
			if (!feedWorker.Started)
			{
				feedWorker.Start();
			}
		}

		/// <summary>
		/// Stops the process of retreiving the RSS feeds.
		/// </summary>
		public static void Stop()
		{
			if (feedWorker.Started)
			{
				feedWorker.Stop();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="destination"></param>
		/// <returns></returns>
		public static Feed Receive(Uri destination)
		{
            if (destination == null) throw new ArgumentNullException();

			IFeedInputChannel channel = FeedTransport.StaticGetInputChannel(destination);
			return channel.Receive();
		} 

		#endregion

		/// <summary>
		/// Gets the current feed storage.
		/// </summary>
		public static IFeedStorage Storage { get; private set; }


		#region callbacks

		private static void OnReceive(Uri destination)
		{
			FeedReceiver receiver = receivers[destination] as FeedReceiver;
			FeedEngine.Subscribe(destination, receiver);
		}

		private static void OnReceiveComplete(IAsyncResult ar)
		{
			FeedAsyncResult result = ar as FeedAsyncResult;
			FeedInputChannel channel = result.Channel;

			Feed feed = channel.EndReceive(result);

			if (FeedEngine.FeedReceived != null)
			{
				FeedEngine.FeedReceived(feed);
			}

			if (receivers.Count > 0)
			{
				FeedReceiver receiver = receivers[channel.Destination] as FeedReceiver;
				if (receiver != null)
				{
					receiver.Receive(feed);
				}
			}

			// Update the storage
            if (Storage != null)
			{
                Storage.Add(feed);
			}

		} 
		#endregion


		#region IDisposable Members

		~FeedEngine()
		{
			Dispose(false);

		}

		public void Dispose()
		{
			FeedEngine.Stop();
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!this.disposed)
			{
				// If disposing equals true, dispose all managed 
				// and unmanaged resources.
				if (disposing)
				{
					// Dispose managed resources.
					this.Dispose();
				}

			}
			disposed = true;
		}


		#endregion
}
}
