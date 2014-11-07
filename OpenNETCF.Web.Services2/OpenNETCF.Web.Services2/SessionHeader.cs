
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Web.Services.Protocols;
 
namespace OpenNETCF.Web.Services2
{
	[XmlRoot(Namespace="http://keithba.com/2003/05/Session")]
	public class SessionHeader : SoapHeader
	{
		public SessionHeader(){}

		[XmlText()]
		public string Id;
		[XmlElement()]
		public Initiate Initiate;
		[XmlElement()]
		public Terminate Terminate;

		//any
		[XmlAnyElement()]
		public XmlElement [] anyElements;
		[XmlAnyAttribute()]
		public XmlAttribute [] anyAttributes;
	}
 
	public class Initiate
	{
		public Initiate(){}
 
		[XmlText()]
		public string Expires;
 
		//any
		[XmlAnyElement()]
		public XmlElement [] anyElements;
		[XmlAnyAttribute()]
		public XmlAttribute [] anyAttributes;
	}
 
	public class Terminate
	{
		public Terminate() {}
 
		//any
		[XmlAnyElement()]
		public XmlElement [] anyElements;
		[XmlAnyAttribute()]
		public XmlAttribute [] anyAttributes;
	}
}