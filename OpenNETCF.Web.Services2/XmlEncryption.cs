
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace OpenNETCF.Web.Services2
{
	public class EncryptedType
	{
		public EncryptedType() {}

		[XmlElement(Namespace=Ns.xenc)]
		public EncryptionMethod EncryptionMethod;
		[XmlElement(Namespace=Ns.ds)]
		public KeyInfo KeyInfo;
		[XmlElement(Namespace=Ns.xenc)]
		public CipherData CipherData; //mandatory
		[XmlElement(Namespace=Ns.xenc)]
		public EncryptionProperties EncryptionProperties;

		//optional
		[XmlAttribute(Namespace=Ns.wsu)] 
		public string Id;
		[XmlAttribute()] 
		public string Type; //content, element, arbitrary
		[XmlAttribute()] 
		public string MimeType;
		[XmlAttribute()] 
		public string Encoding;
	}

	public class EncryptionMethod
	{
		public EncryptionMethod() {}

		//optional
		[XmlElement(Namespace=Ns.xenc)]
		public int KeySize;
		[XmlElement(Namespace=Ns.xenc)]
		public string OAEparams;

		//required
		[XmlAttribute()]
		public AlgorithmEnum Algorithm;

		//any element
	}

	public enum AlgorithmEnum
	{
		//Block Encryption
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#tripledes-cbc")]
		TRIPLEDES, //required
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#aes128-cbc")]
		AES128, //required
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#aes256-cbc")]
		AES256, //required
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#aes192-cbc")]
		AES192, //optional
		//Stream Encryption 
		//NONE
		//Key Transport 
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#rsa-1_5")]
		RSA15, //required
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p")]
		RSAOAEP,
		//Key Agreement 
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#dh")]
		DIFFHELL, //optional
		//Symmetric Key Wrap 
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#kw-tripledes")]
		KWTRIPLEDES, //required
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#kw-aes128")]
		KWAES128, //required
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#kw-aes256")]
		KWAES256, //required
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#kw-aes192")]
		KWAES192, //optional
		//Message Digest 
		[XmlEnum("http://www.w3.org/2000/09/xmldsig#sha1")]
		SHA1, //required
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#sha256")]
		SHA256, //recommended
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#sha512")]
		SHA512, //optional
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#ripemd160")]
		RIPEMD160, //optional
		//Message Authentication 
		[XmlEnum("http://www.w3.org/2000/09/xmldsig#")]
		XMLDSIG, //recommended
		//Canonicalization 
		[XmlEnum("http://www.w3.org/TR/2001/REC-xml-c14n-20010315")]
		XMLC14N, //optional
		[XmlEnum("http://www.w3.org/TR/2001/REC-xml-c14n-20010315#WithComments")]
		XMLC14NWith, //optional
		[XmlEnum("http://www.w3.org/2001/10/xml-exc-c14n#")]
		XMLEXCC14N, //optional
		[XmlEnum("http://www.w3.org/2001/10/xml-exc-c14n#WithComments")]
		XMLEXCC14NWith, //optional
		//Encoding 
		[XmlEnum("http://www.w3.org/2000/09/xmldsig#base64")]
		BASE64, //required
	}

	
	/*
	<xenc:CipherData>
	  <xenc:CipherValue>mSqLUbMpUrKgQAzJ9zeg2nWyln1GyMjTBSG+12i7lWnW+vPfu/cJCi897LJH9/R+qHV5EB99CHyZacP5ut/f9O1tPythN1IPSoDI2swf81N3h/dO3lJZ84VdCW4tp6sNS4fiAk7ZM4aQRKmryfStxwPnSPfJG6HjTHs1vxntBlM=</xenc:CipherValue> 
	  </xenc:CipherData>
	*/

	//[XmlRoot(Namespace=Ns.xenc)]
	public class CipherData
	{
		public CipherData() {}

		//choice
		[XmlElement(Namespace=Ns.xenc)] 
		public string CipherValue;
		[XmlElement(Namespace=Ns.xenc)] 
		public CipherReference CipherReference;
	}

	public class CipherReference
	{
		public CipherReference() {}

		//optional
		[XmlElement(Namespace=Ns.xenc)]
		public Transforms Transforms;

		//required
		[XmlAttribute()]
		public string URI;
	}

	public class Transforms
	{
		public Transforms() {}

		[XmlElement(Namespace=Ns.ds)]
		public Transform [] Transform;
	}

	//[XmlRoot(Namespace=Ns.enc)]
	public class EncryptedData : EncryptedType
	{
		public EncryptedData() {}
	}

	//[XmlRoot(Namespace=Ns.enc)]
	public class EncryptedKey : EncryptedType
	{
		public EncryptedKey() {}

		//optional
		[XmlElement(Namespace=Ns.xenc)] 
		public ReferenceList ReferenceList;
		[XmlElement(Namespace=Ns.xenc)] 
		public string CarriedKeyName;

		//optional
		[XmlAttribute()]
		public string Recipient;
	}

	/*
	<AgreementMethod Algorithm="example:Agreement/Algorithm">
       <KA-Nonce>Zm9v</KA-Nonce>
       <ds:DigestMethod
       Algorithm="http://www.w3.org/2001/04/xmlenc#sha1"/>
      <OriginatorKeyInfo>
         <ds:KeyValue>....</ds:KeyValue>
       </OriginatorKeyInfo>
       <RecipientKeyInfo>
         <ds:KeyValue>....</ds:KeyValue>
       </RecipientKeyInfo> 
     </AgreementMethod>
	*/
	public class AgreementMethod
	{
		public AgreementMethod() {}
		
		[XmlElement(ElementName="KA-Nonce", Namespace="Ns.xenc")]
		public string KA_Nonce;
		[XmlElement(Namespace=Ns.ds)]
		public KeyInfo OriginatorKeyInfo;
		[XmlElement(Namespace=Ns.ds)]
		public KeyInfo RecipientKeyInfo;
		
		[XmlAttribute()]
		public string Algorithm;

		//any element
	}

	public enum PlainTextType
	{
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#Element")]
		Element,
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#Content")]
		Content,
		[XmlEnum("*/*")]
		MediaType,
		[XmlEnum("http://www.w3.org/2001/04/xmlenc#EncryptedKey")]
		EncryptedKey,
	}
	
	/*
	<xenc:ReferenceList xmlns:xenc="http://www.w3.org/2001/04/xmlenc#">
	  <xenc:DataReference URI="#EncryptedContent-c163b16f-44c7-4eea-ac65-a6ce744e2651" /> 
	</xenc:ReferenceList>
	*/

	//[XmlRoot(Namespace=Ns.xenc)]
	public class ReferenceList
	{
		public ReferenceList() {}

		public ReferenceList(ReferenceTypeEnum rte, string uri)
		{
			if(rte == ReferenceTypeEnum.DataReference)
			{
				DataReference = new ReferenceType();
				DataReference.URI = uri;
			}
			else //KeyRef
			{
				KeyReference = new ReferenceType();
				KeyReference.URI = uri;
			}
		}

		//optional
		[XmlElement(Namespace=Ns.xenc)] 
		public ReferenceType DataReference;
		[XmlElement(Namespace=Ns.xenc)] 
		public ReferenceType KeyReference;
	}

	public enum ReferenceTypeEnum
	{
		DataReference,
		KeyReference,
	}

	public class ReferenceType
	{
		public ReferenceType() {}

		[XmlAttribute()]
		public string URI;
		[XmlAttribute()]
		public string ValueType; // = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3";

		//any element
	}

	public class EncryptionProperties
	{
		public EncryptionProperties(){}

		[XmlElement(Namespace=Ns.xenc)]
		public EncryptionProperty [] EncryptionProperty;

		//optional
		[XmlAttribute(Namespace=Ns.wsu)] 
		public string Id;
	}

	public class EncryptionProperty
	{
		public EncryptionProperty(){}

		//optional
		[XmlAttribute(Namespace=Ns.wsu)] 
		public string Id;
		[XmlAttribute()] 
		public string Target;

		//any element
		//any attribute
	}

}
