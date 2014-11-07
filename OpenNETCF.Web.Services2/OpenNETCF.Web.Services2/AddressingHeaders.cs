
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Web.Services.Protocols; 

/*
<wsa:From>
  <wsa:Address>http://schemas.xmlsoap.org/ws/2003/03/addressing/role/anonymous</wsa:Address>
</wsa:From>
<wsa:MessageID>uuid:b0bdb2f9-e8c3-412d-8450-5b4415301e86</wsa:MessageID>

<wsa:ReplyTo>
  <wsa:Address>http://business456.com/client1</wsa:Address>
</wsa:ReplyTo>
<wsa:To>http://fabrikam123.com/Purchasing</wsa:To>
<wsa:Action>http://fabrikam123.com/SubmitPO</wsa:Action>

<wsa:EndpointReference>
  <wsa:Address>xs:anyURI</wsa:Address>
  <wsa:ReferenceProperties> ... </wsa:ReferenceProperties> ?
  <wsa:PortType>xs:QName</wsa:PortType> ?
  <wsa:ServiceName PortName="xs:NCName"?>xs:QName</wsa:ServiceName> ?
  <wsp:Policy/> *
</wsa:EndpointReference>

<wsa:EndpointReference xmlns:wsa="..." xmlns:fabrikam="...">
  <wsa:Address>http://www.fabrikam123.com/acct</wsa:Address>
  <wsa:ReferenceProperties>
     <fabrikam:CustomerKey>123456789</fabrikam:CustomerKey>
  </wsa:ReferenceProperties>
</wsa:EndpointReference>

<wsa:To>http://www.fabrikam123.com/acct</wsa:To>
<fabrikam:CustomerKey>123456789</fabrikam:CustomerKey>

<wsa:MessageID> xs:anyURI </wsa:MessageID>
<wsa:RelatesTo RelationshipType="..."?>xs:anyURI</wsa:RelatesTo>
<wsa:To>xs:anyURI</wsa:To>
<wsa:Action>xs:anyURI</wsa:Action>
<wsa:From>endpoint-reference</wsa:From>
<wsa:ReplyTo>endpoint-reference</wsa:ReplyTo>
<wsa:FaultTo>endpoint-reference</wsa:FaultTo>
<wsa:Recipient>endpoint-reference</wsa:Recipient>

<wsa:MessageID>uuid:aaaabbbb-cccc-dddd-eeee-ffffffffffff</wsa:MessageID>
<wsa:RelatesTo>uuid:11112222-3333-4444-5555-666666666666</wsa:RelatesTo>
<wsa:ReplyTo>
  <wsa:Address>http://business456.com/client1</wsa:Address>
</wsa:ReplyTo>
<wsa:FaultTo>
  <wsa:Address>http://business456.com/deadletters</wsa:Address>
</wsa:FaultTo>
<wsa:To S:mustUnderstand="1">mailto:joe@fabrikam123.com</wsa:To>
<wsa:Action>http://fabrikam123.com/mail#Delete</wsa:Action>
*/

namespace OpenNETCF.Web.Services2
{
	[XmlRoot(Namespace=Ns.wsa, ElementName="From")]
	public class FromHeader : SoapHeader
	{
		public FromHeader() {}

		public FromHeader(string addressText)
		{
			this.Address = new Address();
			if(addressText==null || addressText==String.Empty)
			{
				addressText = Misc.wsaAddress;
			}
			this.Address.text = addressText;
		}

		[XmlElement(Namespace=Ns.wsa)]
		public Address Address;
	}

	[XmlRoot(Namespace=Ns.wsa, ElementName="ReplyTo")]
	public class ReplyToHeader : SoapHeader
	{
		public ReplyToHeader() {}

		public ReplyToHeader(string addressText) 
		{
			this.Address = new Address();
			if(addressText==null || addressText==String.Empty)
			{
				addressText = Misc.wsaAddress;
			}
			this.Address.text = addressText;
		}

		[XmlElement(Namespace=Ns.wsa)]
		public Address Address;
	}

	[XmlRoot(Namespace=Ns.wsa, ElementName="FaultTo")]
	public class FaultToHeader : SoapHeader
	{
		public FaultToHeader() {}

		[XmlElement(Namespace=Ns.wsa)]
		public Address Address;
	}

	public class Address
	{
		public Address() {}

		[XmlText()]
		public string text;
	}

	[XmlRoot(Namespace=Ns.wsa, ElementName="MessageID")]
	public class MessageIdHeader : SoapHeader
	{
		public MessageIdHeader() {}
		
		public MessageIdHeader(string id)
		{
			if(id==null || id==String.Empty)
				id = "uuid:" + Guid.NewGuid().ToString("D"); 
				//id = "uuid:" + System.Guid.NewGuid().ToString("D"); 
			this.text = id;
		}

		[XmlText()]
		public string text;
	}

	[XmlRoot(Namespace=Ns.wsa, ElementName="To")]
	public class ToHeader : SoapHeader
	{
		public ToHeader() {}

		public ToHeader(string to)
		{
			if(to!=null && to!=String.Empty)
			{
				this.text = to;
			}
		}
		
		[XmlText()]
		public string text;
	}

	[XmlRoot(Namespace=Ns.wsa, ElementName="Action")]
	public class ActionHeader : SoapHeader
	{
		public ActionHeader() {}

		public ActionHeader(string text)
		{
			if(text!=null && text!=String.Empty)
			{
				this.text = text;
			}
		}
		
		[XmlText()]
		public string text;
	}

	[XmlRoot(Namespace=Ns.wsa, ElementName="Recipient")]
	public class RecipientHeader : SoapHeader
	{
		public RecipientHeader() {}
		
		[XmlText()]
		public string text;
	}

	[XmlRoot(Namespace=Ns.wsa, ElementName="RelatesTo")]
	public class RelatesToHeader : SoapHeader
	{
		public RelatesToHeader() {}

		public RelatesToHeader(string id) 
		{
			this.text = id;
		}

		[XmlAttribute()]
		public string RelationshipType;

		[XmlText()]
		public string text;
	}

	[XmlRoot(Namespace=Ns.wsa, ElementName="EndpointReference")]
	public class EndpointReferenceHeader : SoapHeader
	{
		public EndpointReferenceHeader() {}

		[XmlElement(Namespace=Ns.wsa)]
		public Address Address;
		[XmlElement(Namespace=Ns.wsa)]
		public ReferenceProperties ReferenceProperties;
		[XmlElement(Namespace=Ns.wsa)]
		public string PortType;
		[XmlElement(Namespace=Ns.wsa)]
		public ServiceName ServiceName;
		[XmlElement(Namespace=Ns.wsa)]
		public string Policy;

		//any
		[XmlAnyElement]
		public XmlElement [] anyElements;
		[XmlAnyAttribute]
		public XmlAttribute [] anyAttributes;
	}

	public class ReferenceProperties
	{
		public ReferenceProperties() {}

		[XmlText()]
		public string text;

		//any
		[XmlAnyElement]
		public XmlElement [] anyElements;
	}

	public class ServiceName
	{
		public ServiceName() {}

		[XmlAttribute()]
		public string PortName;
	}

	/*
	//from samples, this would be an extensibility point through any elements
	[XmlRoot(ElementName="CustomerKey")]
	public class CustomerKeyHeader : SoapHeader
	{
		public CustomerKeyHeader() {}
		
		[XmlText()]
		public string text;
	}
	*/
}
