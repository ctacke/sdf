
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Security.Cryptography.X509Certificates;

using OpenNETCF.Security.Cryptography;

namespace OpenNETCF.Web.Services2
{
	
	public class DerivedKeyToken
	{
		public DerivedKeyToken(byte [] origKey)
		{
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            
			length = XmlSigHandler.NumKeyBytes;

			secTokRef = new SecurityTokenReference();
			secTokRef.Reference = new Reference();
			secTokRef.Reference.ValueType = "http://schemas.xmlsoap.org/ws/2004/04/security/sc/sct";

            byte[] baNonce = new byte[20];// OpenNETCF.Security.Cryptography.Internal.Rand.GetRandomBytes(20);
            rng.GetBytes(baNonce);
			nonce = Convert.ToBase64String(baNonce, 0, baNonce.Length);

			derKey = P_SHA1.DeriveKey(origKey, this.label, baNonce, length);

			System.Guid g = Guid.NewGuid();
			this.id = "SecurityToken-" + g.ToString("D");
		}

		public string id;
		public string algorithm = Misc.secScDkPsha1;

		public SecurityTokenReference secTokRef;
		public int generation = 0;
		public int length = 24; //16 XmlSigHandler.NumKeyBytes above
		public string label = Misc.labelSecureConversation;
		public string nonce;

		public byte [] derKey;

		//<wssc:DerivedKeyToken wsu:Id="SecurityToken-32d4ce5f-8619-4d7f-b0e2-f900b7aef082" wssc:Algorithm="http://schemas.xmlsoap.org/ws/2004/04/security/sc/dk/p_sha1" xmlns:wssc="http://schemas.xmlsoap.org/ws/2004/04/sc">
		//  <wsse:SecurityTokenReference>
		//    <wsse:Reference URI="uuid:c04ae4a1-2e3e-4ab9-8066-7e4a8ab7699c" ValueType="http://schemas.xmlsoap.org/ws/2004/04/security/sc/sct" />
		//  </wsse:SecurityTokenReference>
		//  <wssc:Generation>0</wssc:Generation>
		//  <wssc:Length>16</wssc:Length>
		//  <wssc:Label>WS-SecureConversation</wssc:Label>
		//  <wsse:Nonce>7ReuLXwwNiLRNHc9PtkINw==</wsse:Nonce>
		//</wssc:DerivedKeyToken>
		public XmlElement WriteXml(XmlDocument plainDoc, XmlElement parent, XmlElement after)
		{
			XmlElement derKeyTokElem = plainDoc.CreateElement(Pre.wssc, Elem.DerivedKeyToken, Ns.wssc);
			XmlAttribute idAttrib = plainDoc.CreateAttribute(Pre.wsu, Attrib.Id, Ns.wsuLatest);
			idAttrib.Value = this.id;
			derKeyTokElem.Attributes.Append(idAttrib);
			XmlAttribute algAttrib = plainDoc.CreateAttribute(Pre.wssc, Attrib.Algorithm, Ns.wssc);
			algAttrib.Value = this.algorithm;
			derKeyTokElem.Attributes.Append(algAttrib);
			//parent.AppendChild(derKeyTokElem);
			parent.InsertAfter(derKeyTokElem, after);

			XmlElement securityTokenReferenceElem = plainDoc.CreateElement(Pre.wsse, Elem.SecurityTokenReference, Ns.wsseLatest);
			derKeyTokElem.AppendChild(securityTokenReferenceElem);
			XmlElement referenceElem = plainDoc.CreateElement(Pre.wsse, Elem.Reference, Ns.wsseLatest);
			securityTokenReferenceElem.AppendChild(referenceElem);
			XmlAttribute uriAttrib = plainDoc.CreateAttribute(Attrib.URI);
			uriAttrib.Value = secTokRef.Reference.URI;
			referenceElem.Attributes.Append(uriAttrib);
			XmlAttribute valueTypeAttrib = plainDoc.CreateAttribute(Attrib.ValueType);
			valueTypeAttrib.Value = secTokRef.Reference.ValueType;

			XmlElement genElem = plainDoc.CreateElement(Pre.wssc, Elem.Generation, Ns.wssc);
			genElem.InnerText = this.generation.ToString();
			derKeyTokElem.AppendChild(genElem);

			XmlElement lenElem = plainDoc.CreateElement(Pre.wssc, Elem.Length, Ns.wssc);
			lenElem.InnerText = this.length.ToString();
			derKeyTokElem.AppendChild(lenElem);

			XmlElement labElem = plainDoc.CreateElement(Pre.wssc, Elem.Label, Ns.wssc);
			labElem.InnerText = this.label;
			derKeyTokElem.AppendChild(labElem);

			XmlElement nonElem = plainDoc.CreateElement(Pre.wsse, Elem.Nonce, Ns.wsseLatest);
			nonElem.InnerText = this.nonce;
			derKeyTokElem.AppendChild(nonElem);

			referenceElem.Attributes.Append(valueTypeAttrib);
			return derKeyTokElem;
		}
	}

	//http://schemas.xmlsoap.org/ws/2004/04/trust
	public class TokenType
	{
		public TokenType(){}

		[XmlText()]
		public string InnerText = "http://www.contoso.com/BinaryToken#BinaryToken";
	}

	//http://schemas.xmlsoap.org/ws/2004/04/trust
	public class RequestType
	{
		public RequestType(){}

		[XmlText()]
		public string InnerText = Misc.secTrustIssue;
	}
	
	//http://schemas.xmlsoap.org/ws/2004/04/trust
	public class Base
	{
		public Base()
		{
			SecurityTokenReference = new SecurityTokenReferenceType();
		}

		[XmlElement(Namespace=Ns.wsse)]
		public SecurityTokenReferenceType SecurityTokenReference;
	}

	public class SecurityTokenReferenceType
	{
		public SecurityTokenReferenceType()
		{
			Reference = new ReferenceType();
			//KeyIdentifier = new KeyIdentifierType();
		}
		
		[XmlElement()]
		public ReferenceType Reference;
		//[XmlElement()]
		//public KeyIdentifierType KeyIdentifier;
	}

	/*
	public class KeyIdentifierType
	{
		public KeyIdentifierType(){}
		
		[XmlAttribute()]
		public string URI = "#SecurityToken-";
		[XmlAttribute()]
		public string ValueType = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3";
		[XmlText()]
		public string InnerText = null;
	}
	*/

	/*
	//defined in XmlEncryption
	public class ReferenceType
	{
		public ReferenceType(){}
		
		[XmlAttribute()]
		public string URI = "#SecurityToken-";
		[XmlAttribute()]
		public string ValueType = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3";
	}
	*/

	public class Entropy
	{

	}

	//http://schemas.xmlsoap.org/ws/2002/12/policy
	public class AppliesTo
	{
		public AppliesTo()
		{
			EndpointReference = new EndpointReferenceType();
		}

		[XmlElement(Namespace=Ns.wsa)]
		public EndpointReferenceType EndpointReference;
	}

	public class EndpointReferenceType
	{
		public EndpointReferenceType()
		{
			Address = new AddressType();
		}
		
		[XmlElement()]
		public AddressType Address;
	}

	public class AddressType
	{
		public AddressType(){}
		
		[XmlText()]
		public string InnerText = "http://localhost/CustomBinaryServiceCode/CustomBinaryServiceCode.asmx";
	}

	//http://schemas.xmlsoap.org/ws/2004/04/trust
	public class LifeTime
	{
		public LifeTime()
		{
			Expires = new ExpiresType();
		}

		[XmlElement(Namespace=Ns.wsse)]
		public ExpiresType Expires;
	}

	public class ExpiresType
	{
		public ExpiresType()
		{
		}
		
		[XmlText()]
		public string InnerText = "2004-06-12T23:02:05Z";
	}
}
