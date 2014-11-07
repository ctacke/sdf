
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Web.Services.Protocols; 

/*
(12)	<wse:SubscriptionEnd>
(13)       <wse:Id>uuid:22e8a584-0d18-4228-b2a8-3716fa2097fa</wse:Id>
(14)       <wse:Code>wse:SourceCanceling</wse:Code>
(15)       <wse:Reason xml:lang='en-US' >
(16)         Event source going off line.
(17)       </wse:Reason>
(18)     </wse:SubscriptionEnd>
*/

namespace OpenNETCF.Web.Services2
{
	[XmlRoot(Namespace=Ns.wsa, ElementName="SubscriptionEnd")]
	public class SubscriptionEndHeader : SoapHeader
	{
		public SubscriptionEndHeader() {}

		public SubscriptionEndHeader(string id)
		{
			/*
			this.Address = new Address();
			if(addressText==null || addressText==String.Empty)
				addressText = "http://schemas.xmlsoap.org/ws/2003/03/addressing/role/anonymous";
			this.Address.text = addressText;
			*/
		}

		[XmlElement(Namespace=Ns.wse)]
		public string Id;
		[XmlElement(Namespace=Ns.wse)]
		public string Code;
		[XmlElement(Namespace=Ns.wse)]
		public Reason Reason;
	}

	public class Reason
	{
		public Reason() {}

		//[XmlAttribute(Namespace=Ns.xmlns)]
		[XmlAttribute()] //TODO
		public string lang = "en-US";
		[XmlText()]
		public string text;
	}

	public enum Code
	{
		//wse:SourceCanceling
		Unsubscribed,
		Expired,
		NotifyToFailure,
		SourceCanceling,
	}
}
