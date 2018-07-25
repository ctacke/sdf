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
using System.Text;
using System.Xml;

using OpenNETCF.Rss.Data;

namespace OpenNETCF.Rss
{
    public class FeedHttpTransport : FeedTransport, IFeedTransport
    {
		public static readonly string UriScheme = "http";

		private Hashtable connections;
		private IFeedSerializer serializer;
		internal AsyncCallback ReceiveCompleteCallback;
		private int outboundLimit;
		private int receiveTimeout;
		private int defaulPort;
		
		public FeedHttpTransport()
		{
			this.connections = new Hashtable();
			serializer = new FeedSerializer();
		}

		public FeedHttpTransport(XmlNodeList configData)
		{
			this.connections = new Hashtable();
			this.defaulPort = 80;
			this.outboundLimit = 16;
			this.receiveTimeout = 10000;
	
			if (configData != null)
			{
				foreach (XmlNode node in configData)
				{
					XmlElement element = node as XmlElement;
					if (element != null)
					{
						switch (element.LocalName)
						{
							case "connectionLimit":
								{
									int outLimit = Convert.ToInt32(element.Attributes["inbound"].Value);
									//int inLimit = Convert.ToInt32(element.Attributes["outbound"].Value);
									this.outboundLimit = outLimit;
									//this.inboundLimit = inLimit;
									continue;
								}
							case "defaultPort":
								{
									this.defaulPort = Convert.ToInt32(element.Attributes["value"].Value);
									continue;
								}
							case "serializer":
								{
									string serializerType = element.Attributes["type"].Value;
									this.serializer = FeedTransport.LoadSerializer(serializerType);
									continue;
								}
							case "hosts":
								{
									//this.ParseHosts(element);
									continue;
								}
							case "receiveTimeout":
								{
									this.receiveTimeout = Convert.ToInt32(element.Attributes["value"].Value);
									continue;
								}
							//case "sendTimeout":
							//    {
							//        this.sendTimeout = Convert.ToInt32(element.Attributes["value"].Value);
							//        continue;
							//    }

						}
					}
				}
			}
			if (this.serializer == null)
			{
				this.serializer = new FeedSerializer();
			}
		}

        #region IFeedTransport Members

        public IFeedInputChannel GetInputChannel(Uri address)
        {
			FeedHttpInputChannel channel = base.InputChannels[address.ToString()] as FeedHttpInputChannel;
			if (channel == null)
			{
				channel = new FeedHttpInputChannel(address, this);
				base.InputChannels.Add(address.ToString(), channel);
			}
			return channel;
        }

        #endregion

		private FeedHttpConnection GetConnection(Uri destination)
		{
			FeedHttpConnection connection = null;
			lock (this.connections.SyncRoot)
			{

				connection = this.connections[destination] as FeedHttpConnection;

				if (connection == null)
				{
					connection = new FeedHttpConnection(destination, serializer);
					connection.ReceiveTimeout = receiveTimeout;
					this.connections[destination] = connection;
				}

			}

			return connection;

				//if ((connection == null) || !connection.Connected || !connection.SocketConnected)
				//{
				//    IPEndPoint point = this.GetLocalIPEndPoint(uri);
				//    if ((point != null) && IPAddress.IsLoopback(point.Address))
				//    {
				//        connection = new SoapTcpConnection(destination, this.options, this.formatter);
				//    }
				//    else
				//    {
				//        this.PurgeConnections();
				//        Interlocked.Increment(ref this.outboundCount);
				//        connection = new SoapTcpConnection(destination, this.options, this.formatter);

				//    }
				//    this.connections[destination] = connection;
				//    //connection.BeginReceive(new AsyncCallback(this.ReceiveCallback), connection);

				//}
				//else
				//{
				//    connection.UpdateIdleTimeout();

				//}
		}


		internal IAsyncResult BeginReceive(Uri destination, AsyncCallback callback, object state)
		{
			IAsyncResult result = null;
			this.ReceiveCompleteCallback = callback;
			FeedHttpConnection connection = this.GetConnection(destination);
			result = connection.BeginReceive(new AsyncCallback(OnReceiveComplete));
			return result;

		}

		private void OnReceiveComplete(IAsyncResult ar)
		{
			ReceiveAsyncResult result = ar as ReceiveAsyncResult;
			FeedHttpConnection connection = result.Connection;

			Feed feed = null;
			feed = connection.EndReceive(ar);
			FeedInputChannel channel = (FeedInputChannel)base.InputChannels[connection.Destination.ToString()];
			if (channel == null)
			{
				throw new ApplicationException("Failed to get InputChannel");
			}
			channel.Enqueue(feed);

            FeedAsyncResult feedResult = new FeedAsyncResult(channel);
			this.ReceiveCompleteCallback(feedResult);

		}

		public Feed Receive(Uri destination)
		{
			FeedHttpConnection connection = null;

			if (this.connections[destination] == null)
			{
				connection = new FeedHttpConnection(destination, serializer);
				this.connections.Add(destination, connection);

			}
			else
			{
				connection = this.connections[destination] as FeedHttpConnection;
			}

			return connection.Receive();

		}

		
}
}
