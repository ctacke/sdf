
using System;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Xml;
using System.Collections;

namespace OpenNETCF.Web.Services2
{
	public class WsExtension : SoapExtension 
	{
		//for custom SoapInputFilter and SoapOutputFilter
		public static ArrayList InputFilters = new ArrayList();
		public static ArrayList OutputFilters = new ArrayList();
		
		public static Hashtable reqHt = new Hashtable();
		public static Hashtable RequestSoapContext
		{
			get{return reqHt;}
			set{reqHt = value;}
		}
		public static Hashtable resHt = new Hashtable();
		public static Hashtable ResponseSoapContext
		{
			get{return resHt;}
			set{resHt = value;}
		}

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
			//NOTE this only works if you handle StreamOut/In too
			//NOTE this only works on .NETfx
			return;
		}

		public void AfterSerialize(SoapMessage message) //StreamOut
		{
			newStream.Position = 0;
			//Hack MemoryStream below because of XmlDocument bug closing stream
			byte [] tempBa = new byte[newStream.Length];
			int bytesRead = newStream.Read(tempBa, 0, (int) newStream.Length);
			MemoryStream ms = new MemoryStream(tempBa);
			//BUG NOTE this closes the underlying stream on .NETcf
			XmlDocument xd = new XmlDocument();
			xd.Load(ms); //MemoryStream will be closed now
			
			//OUTGOING
			
			//timestamp and routing
			SecConvHandler.TokenIssuerOut(xd); //have to do body before signing
			HeadersHandler.AddHeaders(xd, message.Action, message.Url); 
			XmlSigHandler.SignXml(xd); //1st ... sign what you see
			XmlEncHandler.EncryptXml(xd);
			//SecConvHandler.ConversationOut(xd);
			//Trace

			//custom filters
			//should these happen before or after - BOTH!
			//should handle priorities of filters
			foreach(object oSof in OutputFilters)
			{
				SoapOutputFilter sof = (SoapOutputFilter) oSof;
				SoapEnvelope se = new SoapEnvelope();
				se.LoadXml(xd.OuterXml);
				se.Context = RequestSoapContext;
				sof.ProcessMessage(se);
				xd.LoadXml(se.OuterXml); //not performant
			}
			RequestSoapContext.Clear();
			OutputFilters.Clear();
			
			newStream.Position = 0;
			XmlTextWriter xtw = new XmlTextWriter(newStream, System.Text.Encoding.UTF8);
			xtw.Namespaces = true;
			xd.WriteTo(xtw);
			xtw.Flush();
			//xtw.Close();

			//these 2 lines have be called at a minimum
			newStream.Position = 0;
			Copy(newStream, oldStream); //for next guy
			return;
		}

		/*
		ChainStream
		BeforeSerial
		AfterSerial
		ChainStream
		BeforeDeserial
		AfterDeserial
		*/

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
			
			//INCOMING

			int xmlSigCnt = 0;
			XmlElement xmlSigElem = LameXpath.SelectSingleNode(xd, Elem.Signature, null, ref xmlSigCnt);
			int xmlEncCnt = 0;
			XmlElement xmlEncElem = LameXpath.SelectSingleNode(xd, Elem.EncryptedKey, null, ref xmlEncCnt);
			int xmlEncCnt2 = 0;
			XmlElement xmlEncElem2 = LameXpath.SelectSingleNode(xd, Elem.ReferenceList, null, ref xmlEncCnt2);
			//TODO make sure not 0
			xmlEncCnt = Math.Min(xmlEncCnt, xmlEncCnt2);

			if(xmlSigElem != null && (xmlEncElem != null || xmlEncElem2 != null))
			{
				if(xmlSigCnt <= xmlEncCnt)
				{
					//Signature then Encryption means encrypt 1st (Release)
					XmlSigHandler.VerifySig(xd);
					xd = XmlEncHandler.DecryptXml(xd);
				}
				else
				{
					//Encryption then Signature means sign 1st (TechPreview) sign what you see
					xd = XmlEncHandler.DecryptXml(xd);
					XmlSigHandler.VerifySig(xd);
				}
				HeadersHandler.CheckHeaders(xd); 
			}
			else //default to 2.0 Release
			{
				//Trace
				XmlSigHandler.VerifySig(xd); //used to be after for TP
				xd = XmlEncHandler.DecryptXml(xd);
				HeadersHandler.CheckHeaders(xd); 
				//timestamp and routing
			}

			//custom filters
			foreach(object oSif in InputFilters)
			{
				SoapInputFilter sif = (SoapInputFilter) oSif;
				SoapEnvelope se = new SoapEnvelope();
				se.LoadXml(xd.OuterXml);
				se.Context = ResponseSoapContext;
				sif.ProcessMessage(se);
				xd.LoadXml(se.OuterXml); //not performant
			}
			ResponseSoapContext.Clear();
			InputFilters.Clear();

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
	public class WsExtensionAttribute : SoapExtensionAttribute 
	{
		public override Type ExtensionType 
		{
			get { return typeof(WsExtension); }
		}

		private int priority;
		public override int Priority 
		{
			get { return priority; }
			set { priority = value; }
		}
	}
}