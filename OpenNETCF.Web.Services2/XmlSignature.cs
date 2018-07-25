
using System;
using System.Xml.Serialization;

namespace OpenNETCF.Web.Services2
{
	public class Signature
	{
		public Signature() {}
	}

	/*
	<ds:KeyInfo Id="..." xmlns:ds="http://www.w3.org/2000/09/xmldsig#">
		<ds:KeyName>CN=Hiroshi Maruyama, C=JP</ds:KeyName>
	</ds:KeyInfo> 
	*/

	//[XmlRoot(Namespace=Ns.ds)]
	public class KeyInfo
	{
		public KeyInfo() {}

		[XmlAttribute(Namespace=Ns.wsu)] 
		public string Id;

		[XmlElement(Namespace=Ns.ds)]
		public string KeyValue;
		[XmlElement(Namespace=Ns.ds)]
		public string KeyName;
		[XmlElement(Namespace=Ns.ds)]
		public string RetrievalMethod;

		//xmlEnc extensions
		[XmlElement(Namespace=Ns.xenc)]
		public EncryptedKey EncryptedKey;
		[XmlElement(Namespace=Ns.xenc)]
		public string AgreementMethod;

		[XmlElement(Namespace=Ns.wsse)]
		public SecurityTokenReference SecurityTokenReference;
	}

	/*
	<element name="RetrievalMethod" type="ds:RetrievalMethodType"/> 
   <complexType name="RetrievalMethodType">
	 <sequence>
	   <element name="Transforms" type="ds:TransformsType" minOccurs="0"/> 
	 </sequence>  
	 <attribute name="URI" type="anyURI"/>
	 <attribute name="Type" type="anyURI" use="optional"/>
   </complexType>
   */
	public class RetrievalMethod
	{
		public RetrievalMethod(){}
		
		//TODO Transforms here or in xenc: ?
		[XmlElement(Namespace=Ns.ds)]
		public Transform [] Transforms;

		[XmlAttribute()]
		public string URI;
		[XmlAttribute()]
		public string Type;
	}

	//ds:
	public class Transform
	{
		public Transform() {}

		//TODO
	}

	/*
	public class KeyName
	{
		public KeyName() {}

		[XmlText()]
		public string Text;
	}
	*/
}
