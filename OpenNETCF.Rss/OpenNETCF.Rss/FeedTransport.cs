
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Reflection;

using OpenNETCF.Rss.Configuration;


namespace OpenNETCF.Rss
{
	/// <summary>
	/// Represents the protocol used to transport the RSS feeds. 
	/// </summary>
	public class FeedTransport 
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the FeedTransport.
		/// </summary>
		public FeedTransport()
		{
			InputChannels = new HybridDictionary();
		}
		
		#endregion

		#region public methods

		/// <summary>
		/// Gets the input channel for the transport. 
		/// </summary>
		/// <param name="destination">The Uri to receive the RSS feeds.</param>
		/// <returns>The IFeedInputChannel associated with the transport. </returns>
		public static IFeedInputChannel StaticGetInputChannel(Uri destination)
		{
			IFeedTransport transport = FeedTransport.GetTransport(destination);
			if (transport != null)
			{
				return transport.GetInputChannel(destination);
			}
			return null;
		}

		/// <summary>
		/// Loads feed serializer
		/// </summary>
		/// <param name="typeName">Type name of serializer</param>
		/// <returns>Feed serializer object.</returns>
		public static IFeedSerializer LoadSerializer(string typeName)
		{
			Type type = Type.GetType(typeName, false);
			if (type != null)
			{
				return FeedTransport.LoadSerializer(type);
			}
			return null;
		}

		/// <summary>
		/// Loads feed serializer
		/// </summary>
		/// <param name="type">Type of serializer</param>
		/// <returns>Feed serializer object.</returns>
		public static IFeedSerializer LoadSerializer(Type type)
		{
			ConstructorInfo info = null;

			if (Utility.ImplementsInterface(type, typeof(IFeedSerializer)))
			{
				info = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);
				if (info == null)
				{
					throw new ArgumentException(type.Name);
				}
				return (info.Invoke(null) as IFeedSerializer);
			}

			return null;
		}

		/// <summary>
		/// Get the transport associated with the specified destination. 
		/// </summary>
		/// <param name="destination">Feed destination.</param>
		/// <returns>Transport object.</returns>
		public static IFeedTransport GetTransport(Uri destination)
		{
			IFeedTransport transport = null;

			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			transport = FeedConfiguration.TransportConfiguration.GetTransport(destination.Scheme);

			if (transport == null)
			{
				switch (destination.Scheme)
				{
					case "http":
						transport = new FeedHttpTransport();
						break;
					case "https":
						transport = new FeedHttpTransport();
						break;
				}
			}

			return transport;

		}

		#endregion

		#region properties
		
		/// <summary>
		/// Gets channels colletion.
		/// </summary>
		public HybridDictionary InputChannels { get; private set; }

		#endregion

	}
}
