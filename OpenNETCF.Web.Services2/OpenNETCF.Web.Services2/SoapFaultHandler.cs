
using System;
using System.Collections;
using System.Xml;

namespace OpenNETCF.Web.Services2
{
	public class SoapFaultHandler
	{
		public SoapFaultHandler() {}

		/*
		<env:Body>
			<env:Fault>
				<env:Code>
				<env:Value>env:Sender</env:Value>
				<env:Subcode>
					<env:Value>rpc:BadArguments</env:Value>
				</env:Subcode>
				</env:Code>
				<env:Reason>
				<env:Text xml:lang="en-US">Processing error</env:Text>
				<env:Text xml:lang="cs">Chyba zpracování</env:Text>
				</env:Reason>
				<env:Detail>
				<e:myFaultDetails 
					xmlns:e="http://travelcompany.example.org/faults">
					<e:message>Name does not match card number</e:message>
					<e:errorcode>999</e:errorcode>
				</e:myFaultDetails>
				</env:Detail>
			</env:Fault>
		</env:Body> 
		*/
		public static void CheckXml(XmlDocument plainDoc)
		{
			XmlElement envelope = plainDoc.DocumentElement;

			XmlElement headerOrBody = (XmlElement) envelope.ChildNodes[0];
			XmlElement header;
			XmlElement body;
			if(headerOrBody.LocalName == Elem.Body)
			{
				header = plainDoc.CreateElement(headerOrBody.Prefix, Elem.Header, headerOrBody.NamespaceURI);
				envelope.InsertBefore(header, headerOrBody);
			}
			header = (XmlElement) envelope.ChildNodes[0];
			body = (XmlElement) envelope.ChildNodes[1];
			
		}
	}
}
