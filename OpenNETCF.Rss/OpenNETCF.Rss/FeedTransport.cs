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
