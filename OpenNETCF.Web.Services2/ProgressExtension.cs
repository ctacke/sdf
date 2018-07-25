//slightly modified from MSDN article, incomplete

using System;
using System.IO;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace OpenNETCF.Web.Services2
{
	public class ProgressExtension : SoapExtension
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
			
			return;
		}

		public void AfterSerialize(SoapMessage message) //StreamOut
		{
			if(onClientOrServer == OnClientOrServer.Unknown)
				onClientOrServer = OnClientOrServer.Client;

			/*
			//HACK because .NETcf does not add Headers in BeforeSerialize above
			if(Environment.OSVersion.Platform == PlatformID.WinCE)
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
			*/

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
			
			/*
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
			*/
			return;
			//}
		}

		OnClientOrServer onClientOrServer = OnClientOrServer.Unknown;
		//string inAction = null;
		//string inMessageId = null;
		//string inReplyTo = null;
		//XmlElement subscriptionEnd = null;

		public void AfterDeserialize(SoapMessage message) //ObjectIn
		{
			if(onClientOrServer == OnClientOrServer.Unknown)
				onClientOrServer = OnClientOrServer.Server;
			if(onClientOrServer == OnClientOrServer.Server)
			{
				//nothing
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

		/*
		internal class ProgressClient //: localhost.Service1
		{
			public ProgressBar Progress;
			public int TransferSize;
			public UpdateDelegate ProgressDelegate;
		}

		public delegate void UpdateDelegate();

		// Holds the original stream
		private Stream m_oldStream;
		// The new stream
		private Stream m_newStream;
		// The buffer for reading from the old stream
		// and writing to the new stream
		private byte[] m_bufferIn;
		// The progress bar we will be incrementing
		private ProgressBar m_Progress;
		// The size of each read
		private int m_readSize;
		// The delegate we will invoke for updating the
		// progress bar.
		//private Form1.UpdateDelegate m_progressDelegate;
		private UpdateDelegate m_progressDelegate;
		// Used to keep track of which stream we are trying
		// to chain into
		private bool m_isAfterSerialization;
		public override void ProcessMessage(SoapMessage message)
		{
			switch(message.Stage)
			{
				case SoapMessageStage.AfterSerialize:
					// To let us know that the next ChainStream call
					// will let us hook in where we want.
					m_isAfterSerialization = true;
					break;
				case SoapMessageStage.BeforeDeserialize:
					// This is where we stream through the data
					SoapClientMessage clientMessage 
						= (SoapClientMessage)message;
					if (clientMessage.Client is ProgressClient)
					{
						ProgressClient proxy 
							= (ProgressClient)clientMessage.Client;
						m_Progress = proxy.Progress;
						// Read 1/100th of the request at a time.
						// This will give the progress bar 100 
						// notifications.
						m_readSize = proxy.TransferSize / 100;
						m_progressDelegate = proxy.ProgressDelegate;
					}
					while (true)
					{
						try
						{
							int bytesRead 
								= m_oldStream.Read(m_bufferIn, 
								0, 
								m_readSize);
							if (bytesRead == 0) 
							{
								// end of message...rewind the
								// memory stream so it is ready
								// to be read during deserial.
								m_newStream.Seek(0, 
									System.IO.SeekOrigin.Begin);
								return;
							}
							m_newStream.Write(m_bufferIn, 
								0, 
								bytesRead);
							// Update the progress bar
							m_Progress.Invoke(m_progressDelegate);
						}
						catch
						{
							// rewind the memory stream
							m_newStream.Seek(0, 
								System.IO.SeekOrigin.Begin);
							return;
						}
					}
			}
		}

		public override Stream ChainStream(Stream stream)
		{
			if (m_isAfterSerialization)
			{
				m_oldStream = stream;
				m_newStream = new MemoryStream();
				m_bufferIn = new Byte[8192];
				return m_newStream;
			}
			return stream;
		}
		// We don't have an initializer to be shared across streams
		public override object GetInitializer(Type serviceType)
		{
			return null;
		}

		public override object GetInitializer(
			LogicalMethodInfo methodInfo, 
			SoapExtensionAttribute attribute)
		{
			return null;
		}

		public override void Initialize(object initializer) 
		{m_isAfterSerialization = false;}
		*/
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class ProgressExtensionAttribute : SoapExtensionAttribute 
	{
		public override Type ExtensionType 
		{
			get { return typeof(ProgressExtension); }
		}

		private int priority;
		public override int Priority 
		{
			get { return priority; }
			set { priority = value; }
		}
	}
}
