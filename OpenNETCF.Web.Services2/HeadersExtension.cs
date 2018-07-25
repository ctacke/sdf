
using System;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Xml;

namespace OpenNETCF.Web.Services2
{
	public class HeadersExtension : SoapExtension 
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
			if(System.Environment.OSVersion.Platform != PlatformID.WinCE)
			{	
				//NOTE this only works if you handle StreamOut/In too
				//NOTE this only works on .NETfx
				TimestampHeader timestampHeader = new TimestampHeader(300);
				message.Headers.Add(timestampHeader);

				//if(EndPoint == EndPointType.Addressing)
				ActionHeader actionHeader = new ActionHeader(message.Action);
				//FromHeader fromHeader = new FromHeader(null);
				ReplyToHeader replyToHeader = new ReplyToHeader(null);
				MessageIdHeader messageIdHeader = new MessageIdHeader(null);
				ToHeader toHeader = new ToHeader(message.Url);
				message.Headers.Add(actionHeader);
				//message.Headers.Add(fromHeader);
				message.Headers.Add(replyToHeader);
				message.Headers.Add(messageIdHeader);
				message.Headers.Add(toHeader);
				//else //routing
				//pathHeader pHeader = new pathHeader(message.Action, message.Url, null);
				//message.Headers.Add(pHeader);
			}
			return;
		}

		public void AfterSerialize(SoapMessage message) //StreamOut
		{
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
			Copy(oldStream, newStream);
			newStream.Position = 0;

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
		}

		public void AfterDeserialize(SoapMessage message) //ObjectIn
		{
			SoapMessage mess = message;
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

	[AttributeUsage(AttributeTargets.Method)]
	public class HeadersExtensionAttribute : SoapExtensionAttribute 
	{
		public override Type ExtensionType 
		{
			get { return typeof(HeadersExtension); }
		}

		private int priority;
		public override int Priority 
		{
			get { return priority; }
			set { priority = value; }
		}
	}
}