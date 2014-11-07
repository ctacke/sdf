
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

/*
<wsse:Security soap:mustUnderstand="1" xmlns:wsse="http://schemas.xmlsoap.org/ws/2002/07/secext">
  <xenc:ReferenceList xmlns:xenc="http://www.w3.org/2001/04/xmlenc#">
    <xenc:DataReference URI="#EncryptedContent-c163b16f-44c7-4eea-ac65-a6ce744e2651" /> 
  </xenc:ReferenceList>
</wsse:Security>
  
<wsse:SecurityTokenReference xmlns:wsse="http://schemas.xmlsoap.org/ws/2002/04/secext"> 
   <wsse:Reference URI="http://www.fabrikam123.com/tokens/Zoe#X509token"/>
</wsse:SecurityTokenReference>

<wsse:SecurityTokenReference>
   <wsse:KeyIdentifier wsu:Id="..." 
                       ValueType="..."
                       EncodingType="...">
      ...
   </wsse:KeyIdentifier>
</wsse:SecurityTokenReference>
*/

namespace OpenNETCF.Web.Services2
{
	//[XmlRoot(Namespace=Ns.wsse)]
	public class SecurityTokenReference
	{
		public SecurityTokenReference() {}

		[XmlAttribute(Namespace=Ns.wsu)] 
		public string Id;

		[XmlElement(Namespace=Ns.wsse)] //
		public Reference Reference;
		[XmlElement(Namespace=Ns.wsse)] //
		public KeyIdentifier KeyIdentifier;
		[XmlElement(Namespace=Ns.xenc)] //
		public ReferenceList ReferenceList;

		//any
		//[XmlAnyAttribute]
		//public XmlAttribute [] anyAttributes;
		//[XmlAnyElement]
		//public XmlElement [] anyElements;
	}

	//[XmlRoot(Namespace=Ns.wsse)]
	public class Reference
	{
		public Reference() {}

		[XmlAttribute()] 
		public string URI;
		[XmlAttribute()] 
		public string ValueType;
		
		[XmlText()] 
		public string text;
	}

	//[XmlRoot(Namespace=Ns.wsse)]
	public class KeyIdentifier
	{
		public KeyIdentifier() {}

		[XmlAttribute(Namespace=Ns.wsu)] 
		public string Id;
		[XmlAttribute()] 
		public string ValueType;
		[XmlAttribute()] 
		public string EncodingType;
		
		[XmlText()] 
		public string text;

		//any
		//[XmlAnyAttribute]
		//public XmlAttribute [] anyAttributes;
	}
}
