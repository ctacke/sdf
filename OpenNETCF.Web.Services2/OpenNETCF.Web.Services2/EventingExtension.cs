
using System;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Xml;

namespace OpenNETCF.Web.Services2
{
	public class EventingExtension : SoapExtension 
	{
		Stream oldStream;
		Stream newStream;

		public override Stream ChainStream( Stream stream )
		{
			oldStream = stream;
			newStream = new MemoryStream();
			return newStream;
		}

		public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute) 
		{
			return null;
		}

		public override object GetInitializer(Type WebServiceType) 
		{
			return null;
		}

		public override void Initialize(object initializer) 
		{

		}

		public override void ProcessMessage(SoapMessage message) 
		{
			switch (message.Stage) 
			{
				case SoapMessageStage.BeforeSerialize:
					BeforeSerialize(message);
					break;
				case SoapMessageStage.AfterSerialize:
					AfterSerialize(message);
					break;
				case SoapMessageStage.BeforeDeserialize:
					BeforeDeserialize(message);
					break;
				case SoapMessageStage.AfterDeserialize:
					AfterDeserialize(message);
					break;
				default:
					throw new Exception("invalid SoapExtension.ProcessMessage stage");
			}
		}

		public void BeforeSerialize(SoapMessage message) //ObjectOut
		{
			if(onClientOrServer == OnClientOrServer.Unknown)
				onClientOrServer = OnClientOrServer.Client;
			if(System.Environment.OSVersion.Platform != PlatformID.WinCE)
			{	
				//NOTE this only works if you handle StreamOut/In too
				//NOTE this only works on .NETfx
				//TimestampHeader timestampHeader = new TimestampHeader(300);
				//message.Headers.Add(timestampHeader);
				if(onClientOrServer == OnClientOrServer.Server)
				{
					ActionHeader actionHeader = new ActionHeader(message.Action + "Response");
					//else check what the inAction is
					RelatesToHeader relatesToHeader = null;
					if(inMessageId != null)
						relatesToHeader = new RelatesToHeader(inMessageId);
					ToHeader toHeader = null;
					if(inReplyTo != null)
						toHeader = new ToHeader(inReplyTo);
					//TODO ReferenceProperties
					message.Headers.Add(actionHeader);
					if(relatesToHeader != null)
						message.Headers.Add(relatesToHeader);
					if(toHeader != null)
						message.Headers.Add(toHeader);
				}
				else
				{
					//if(EndPoint == EndPointType.Addressing)
					ActionHeader actionHeader = new ActionHeader(message.Action);
					FromHeader fromHeader = new FromHeader(null);
					MessageIdHeader messageIdHeader = new MessageIdHeader(null);
					ToHeader toHeader = new ToHeader(message.Url);
					//TODO Subscription would need a ReplyTo header for asynch web services
					//ReplyToHeader replyToHeader = new ReplyToHeader("http://tempuri.org/RespondToClientCall/");
					ReplyToHeader replyToHeader = new ReplyToHeader(message.Url); //just respond normally?
					message.Headers.Add(replyToHeader);
					message.Headers.Add(actionHeader);
					message.Headers.Add(fromHeader);
					message.Headers.Add(messageIdHeader);
					message.Headers.Add(toHeader);
				}
				//else //routing
				//pathHeader pHeader = new pathHeader(message.Action, message.Url, null);
				//message.Headers.Add(pHeader);
			}
			return;
		}

		public void AfterSerialize(SoapMessage message) //StreamOut
		{
			if(onClientOrServer == OnClientOrServer.Unknown)
				onClientOrServer = OnClientOrServer.Client;

			//HACK because .NETcf does not add Headers in BeforeSerialize above
			if(System.Environment.OSVersion.Platform == PlatformID.WinCE)
			{				
				newStream.Position = 0;
				//Hack MemoryStream below because of XmlDocument bug closing stream
				byte [] tempBa = new byte[newStream.Length];
				int bytesRead = newStream.Read(tempBa, 0, (int) newStream.Length);
				MemoryStream ms = new MemoryStream(tempBa);
				//BUG NOTE this closes the underlying stream on .NETcf
				XmlDocument xd = new XmlDocument();
				xd.Load(ms); //MemoryStream will be closed now

				HeadersHandler.AddHeaders(xd, message.Action, message.Url);

				newStream.Position = 0;
				XmlTextWriter xtw = new XmlTextWriter(newStream, System.Text.Encoding.UTF8);
				xtw.Namespaces = true;
				xd.WriteTo(xtw);
				xtw.Flush();
				//xtw.Close();
			} //END WinCE HACK

			//these 2 lines have be called at a minimum
			newStream.Position = 0;
			Copy(newStream, oldStream); //for next guy
			return;
		}

		public void BeforeDeserialize(SoapMessage message) //StreamIn
		{
			if(onClientOrServer == OnClientOrServer.Unknown)
				onClientOrServer = OnClientOrServer.Server;
			Copy(oldStream, newStream);
			newStream.Position = 0;
			
			//if(Environment.OSVersion.Platform == PlatformID.WinCE)
			//{	
				//Hack MemoryStream below because of XmlDocument bug closing stream
				byte [] tempBa = new byte[newStream.Length];
				int bytesRead = newStream.Read(tempBa, 0, (int) newStream.Length);
				MemoryStream ms = new MemoryStream(tempBa);
				//BUG NOTE this closes the underlying stream on .NETcf
				XmlDocument xd = new XmlDocument();
				xd.Load(ms); //MemoryStream will be closed now
			
				HeadersHandler.CheckHeaders(xd);

				//newStream is too big now
				newStream.SetLength(0); //doing a new MemoryStream here was bad
				XmlTextWriter xtw = new XmlTextWriter(newStream, System.Text.Encoding.UTF8);
				xtw.Namespaces = true;
				xd.WriteTo(xtw);
				xtw.Flush();
				//xtw.Close();
				newStream.Position = 0;
				return;
			//}
		}

		OnClientOrServer onClientOrServer = OnClientOrServer.Unknown;
		string inAction = null;
		string inMessageId = null;
		string inReplyTo = null;
		XmlElement subscriptionEnd = null;

		public void AfterDeserialize(SoapMessage message) //ObjectIn
		{
			if(onClientOrServer == OnClientOrServer.Unknown)
				onClientOrServer = OnClientOrServer.Server;
			if(onClientOrServer == OnClientOrServer.Server)
			{
				foreach(SoapHeader sh in message.Headers)
				{
					//TODO check for SubscriptionEnd header for WS-Addressing
					if(sh is SoapUnknownHeader)
					{
						SoapUnknownHeader suh = (SoapUnknownHeader) sh;
						if(suh.Element.LocalName == "Action")
							inAction = suh.Element.InnerText;
						if(suh.Element.LocalName == "MessageID")
							inMessageId = suh.Element.InnerText;
						if(suh.Element.LocalName == "ReplyTo")
							inReplyTo = suh.Element.FirstChild.InnerText;
						if(suh.Element.LocalName == "SubscriptionEnd")
							subscriptionEnd = suh.Element;
					}
					if(sh is ActionHeader)
					{
						ActionHeader ah = (ActionHeader) sh;
						inAction = ah.text;
					}
					if(sh is MessageIdHeader)
					{
						MessageIdHeader mih = (MessageIdHeader) sh;
						inMessageId = mih.text;
					}
					if(sh is ReplyToHeader)
					{
						ReplyToHeader rth = (ReplyToHeader) sh;
						inReplyTo = rth.Address.text;
					}
				}
			}
			else //client
			{

			}
			return;
		}

		void Copy(Stream from, Stream to) 
		{
			TextReader reader = new StreamReader(from);
			TextWriter writer = new StreamWriter(to);
			//string strRead = reader.ReadToEnd();
			//writer.WriteLine(strRead);
			writer.WriteLine(reader.ReadToEnd());
			writer.Flush();
		}
	}

	public enum OnClientOrServer
	{
		Unknown,
		Client,
		Server,
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class EventingExtensionAttribute : SoapExtensionAttribute 
	{
		public override Type ExtensionType 
		{
			get { return typeof(EventingExtension); }
		}

		private int priority;
		public override int Priority 
		{
			get { return priority; }
			set { priority = value; }
		}
	}
}