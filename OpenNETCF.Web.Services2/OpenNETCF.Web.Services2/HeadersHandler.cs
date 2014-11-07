
using System;
using System.Collections;
using System.Xml;

using OpenNETCF.Security.Cryptography;

namespace OpenNETCF.Web.Services2
{
	public class HeadersHandler
	{
		public HeadersHandler() {}

		public static string soapActorUrl = null;
		[CLSCompliant(false)]
		public static string SoapActorUrl
		{
			get
			{return soapActorUrl;}
			set
			{soapActorUrl = value;}
		}
		//public static string Action = null;

		public static EndPointType EndPoint = EndPointType.Addressing;
		public enum EndPointType
		{
			Routing,
			Addressing,
		}

		[CLSCompliant(false)]
		public static int secondsToTimeout = 300; //5 minutes
		public int SecondsToTimeout
		{
			get{return secondsToTimeout;}
			set{secondsToTimeout = value;}
		}

		public static XmlDocument AddHeaders(XmlDocument xmlDoc, string action, string url)
		{
			return AddHeaders(xmlDoc, action, url, Misc.wsaAddress);
		}

		public static XmlDocument AddHeaders(XmlDocument xmlDoc, string action, string url, string strReplyTo)
		{
			XmlElement envelope = xmlDoc.DocumentElement;
			//add namespaces
			if(EndPoint == EndPointType.Addressing)
			{
				XmlAttribute wsa = xmlDoc.CreateAttribute(Pre.xmlns, Pre.wsa, Ns.xmlns);
				wsa.Value = Ns.wsaLatest;
				envelope.Attributes.Append(wsa);
			}
			else //routing
			{
				XmlAttribute wsrp = xmlDoc.CreateAttribute(Pre.xmlns, Pre.wsrp, Ns.xmlns);
				wsrp.Value = Ns.wsrp;
				envelope.Attributes.Append(wsrp);
			}
			XmlAttribute wsu = xmlDoc.CreateAttribute(Pre.xmlns, Pre.wsu, Ns.xmlns);
			wsu.Value = Ns.wsuLatest;
			envelope.Attributes.Append(wsu);
			XmlAttribute wsse = xmlDoc.CreateAttribute(Pre.xmlns, Pre.wsse, Ns.xmlns);
			wsse.Value = Ns.wsseLatest;
			envelope.Attributes.Append(wsse);

			XmlElement headerOrBody = (XmlElement) envelope.ChildNodes[0];
			XmlElement header;
			XmlElement body;
			if(headerOrBody.LocalName == Elem.Body)
			{
				header = xmlDoc.CreateElement(headerOrBody.Prefix, Elem.Header, headerOrBody.NamespaceURI);
				envelope.InsertBefore(header, headerOrBody);
			}
			header = (XmlElement) envelope.ChildNodes[0];
			body = (XmlElement) envelope.ChildNodes[1];
			XmlNodeList headers = header.ChildNodes;
			bool hasTimestamp = false;
			bool hasReplyTo = false;
			bool hasAction = false;
			//bool hasFrom = false;
			bool hasMessageId = false;
			bool hasTo = false;
			bool hasPath = false;
			foreach(XmlNode xn in headers)
			{
				XmlElement headerElement = (XmlElement) xn;
				switch(headerElement.LocalName)
				{
					case Elem.Timestamp:
						hasTimestamp = true;
						break;
					case Elem.ReplyTo:
						hasReplyTo = true;
						break;
					case Elem.Action:
						hasAction = true;
						break;
					//case Elem.From:
					//	hasFrom = true;
					//	break;
					case Elem.MessageID:
						hasMessageId = true;
						break;
					case Elem.To:
						hasTo = true;
						break;
					case Elem.path:
						hasPath = true;
						break;
				}
			}
			if(EndPoint == EndPointType.Addressing)
			{
				if(hasAction == false)
				{
					//<wsa:Action>http://stockservice.contoso.com/wse/samples/2003/06/StockQuoteRequest</wsa:Action>
					XmlElement actionElem = xmlDoc.CreateElement(Pre.wsa, Elem.Action, Ns.wsaLatest);
					actionElem.InnerText = action;
					header.AppendChild(actionElem);
				}
				if(hasMessageId == false)
				{
					//<wsa:MessageID>uuid:e5b8263b-43f1-4799-aa11-0d53a1ea7ace</wsa:MessageID>
					XmlElement messageId = xmlDoc.CreateElement(Pre.wsa, Elem.MessageID, Ns.wsaLatest);
					messageId.InnerText = "uuid:" + Guid.NewGuid().ToString("D"); 
					header.AppendChild(messageId);
				}
				//<ReplyTo xmlns="http://schemas.xmlsoap.org/ws/2003/03/addressing"><Address>http://tempuri.org/RespondToClientCall/</Address></ReplyTo>
				if(hasReplyTo == false)
				{
					XmlElement replyTo = xmlDoc.CreateElement(Pre.wsa, Elem.ReplyTo, Ns.wsaLatest);
					XmlElement address = xmlDoc.CreateElement(Pre.wsa, Elem.Address, Ns.wsaLatest);
					address.InnerText = strReplyTo;
					replyTo.AppendChild(address);
					header.AppendChild(replyTo);
				}
				if(hasTo == false)
				{
					//<wsa:To>http://localhost/RouterService/StockService.asmx</wsa:To>
					XmlElement to = xmlDoc.CreateElement(Pre.wsa, Elem.To, Ns.wsaLatest);
					if(SoapActorUrl == null || SoapActorUrl == String.Empty)
						to.InnerText = url; 
					else
						to.InnerText = SoapActorUrl;
					header.AppendChild(to);
				}
			}
			else //routing
			{
				if(hasPath == false)
				{
					XmlElement path = xmlDoc.CreateElement(Pre.wsrp, Elem.path, Ns.wsrp);
					XmlAttribute actor = xmlDoc.CreateAttribute(Pre.soap, Attrib.actor, Ns.soap);
					actor.Value = Misc.pathActorNext;
					path.Attributes.Append(actor);
					XmlAttribute mustUnderstand = xmlDoc.CreateAttribute(Pre.soap, Attrib.mustUnderstand, Ns.soap);
					mustUnderstand.Value = "1";
					path.Attributes.Append(mustUnderstand);
					XmlElement actionElem = xmlDoc.CreateElement(Pre.wsrp, Elem.action, Ns.wsrp);
					actionElem.InnerText = action;
					path.AppendChild(actionElem);
					XmlElement to = xmlDoc.CreateElement(Pre.wsrp, Elem.to, Ns.wsrp);
					to.InnerText = url;
					path.AppendChild(to);
					XmlElement id = xmlDoc.CreateElement(Pre.wsrp, Elem.id, Ns.wsrp);
					id.InnerText = "uuid:" + Guid.NewGuid().ToString("D");
					path.AppendChild(id);
					header.AppendChild(path);
				}
			}
			if(hasTimestamp == false)
			{
				XmlElement security = null;
				foreach(XmlNode xn in headers)
				{
					if(xn.LocalName == Elem.Security)
						security = (XmlElement) xn;
				}
				if(security == null)
				{
					security = xmlDoc.CreateElement(Pre.wsse, Elem.Security, Ns.wsseLatest);
					XmlAttribute mustUnderstand = xmlDoc.CreateAttribute(Pre.soap, Attrib.mustUnderstand, Ns.soap);
					mustUnderstand.Value = "1";
					security.Attributes.Append(mustUnderstand);
					header.AppendChild(security);
				}
				XmlElement timestamp = xmlDoc.CreateElement(Pre.wsu, Elem.Timestamp, Ns.wsuLatest);
				XmlElement created = xmlDoc.CreateElement(Pre.wsu, Elem.Created, Ns.wsuLatest);
				created.InnerText = TimestampHeader.ConvertDateTime(DateTime.UtcNow);
				XmlElement expires = xmlDoc.CreateElement(Pre.wsu, Elem.Expires, Ns.wsuLatest);
				expires.InnerText = TimestampHeader.ConvertDateTime(DateTime.UtcNow.AddSeconds(secondsToTimeout));
				timestamp.AppendChild(created);
				timestamp.AppendChild(expires);
				security.AppendChild(timestamp);
			}
			return xmlDoc;
		}

		public static XmlDocument CheckHeaders(XmlDocument xmlDoc)
		{
			XmlElement envelope = xmlDoc.DocumentElement;
			XmlElement headerOrBody = (XmlElement) envelope.ChildNodes[0];
			XmlElement header = null;
			XmlElement body = null;
			if(headerOrBody.LocalName == Elem.Header)
			{
				header = (XmlElement) envelope.ChildNodes[0];
				body = (XmlElement) envelope.ChildNodes[1];
			}
			else //no header
			{
				body = (XmlElement) envelope.ChildNodes[0];
			}

			if(header != null)
			{
				XmlElement tsElem = LameXpath.SelectSingleNode(header, Elem.Timestamp);
				if(tsElem != null)
				{
					XmlElement creElem = LameXpath.SelectSingleNode(tsElem, Elem.Created);
					XmlElement expElem = LameXpath.SelectSingleNode(tsElem, Elem.Expires);
					string cre = creElem.InnerText;
					string exp = expElem.InnerText;
					
					if(exp.CompareTo(cre) == -1) //less than
						throw new Exception("Timestamp expired before creation");
					bool retVal = TimestampHeader.IsExpired(exp);
					if(retVal == true)
						throw new Exception("response TimeStamp is expired");
				}
			}

			return xmlDoc;
		}
	}
}
