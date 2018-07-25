
using System;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Xml;

using OpenNETCF.Security.Cryptography;

namespace OpenNETCF.Web.Services2
{
	/// <summary>
	/// Summary description for SecConvHandler.
	/// </summary>
	public class SecConvHandler
	{
		public SecConvHandler(){}

		public static SecConvObject SecConv;

		public static void TokenIssuerOut(XmlDocument plainDoc)
		{
			if(SecConv == null)
				return; //nothing to secureConversate :)

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
			
			//TODO header for actual request?

			//do request token stuff to Body
			//<wst:RequestSecurityToken xmlns:wst="http://schemas.xmlsoap.org/ws/2004/04/trust">
			XmlElement reqSecTokElem = LameXpath.SelectSingleNode(body, Elem.RequestSecurityToken);
			if(reqSecTokElem != null)
			{
				//<wst:TokenType>http://schemas.xmlsoap.org/ws/2004/04/security/sc/sct</wst:TokenType>
				XmlElement tokenTypeElem = plainDoc.CreateElement(Pre.wst, Elem.TokenType, Ns.wst);
				tokenTypeElem.InnerText = SecConv.tokenType.InnerText;
				reqSecTokElem.AppendChild(tokenTypeElem);

				//<wst:RequestType>http://schemas.xmlsoap.org/ws/2004/04/security/trust/Issue</wst:RequestType>
				XmlElement requestTypeElem = plainDoc.CreateElement(Pre.wst, Elem.RequestType, Ns.wst);
				requestTypeElem.InnerText = SecConv.requestType.InnerText;
				reqSecTokElem.AppendChild(requestTypeElem);

				//<wst:Base>
				//	<wsse:SecurityTokenReference>
				//		<wsse:Reference URI="#SecurityToken-b2aa8429-e44f-42a3-b181-f068f5c23cba" ValueType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#UsernameToken" />
				//		<wsse:KeyIdentifier ValueType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509SubjectKeyIdentifier">bBwPfItvKp3b6TNDq+14qs58VJQ=</wsse:KeyIdentifier>
				//	</wsse:SecurityTokenReference>
				//</wst:Base>
				XmlElement baseElem = plainDoc.CreateElement(Pre.wst, Elem.Base, Ns.wst);
				reqSecTokElem.AppendChild(baseElem);
				XmlElement secTokRefElem = plainDoc.CreateElement(Pre.wsse, Elem.SecurityTokenReference, Ns.wsseLatest);
				baseElem.AppendChild(secTokRefElem);
				//if(SecConv.base_.SecurityTokenReference.Reference != null)
				//{
					XmlElement refElem = plainDoc.CreateElement(Pre.wsse, Elem.Reference, Ns.wsseLatest);
					secTokRefElem.AppendChild(refElem);
					XmlAttribute uriAttrib = plainDoc.CreateAttribute(Attrib.URI);
					uriAttrib.Value = SecConv.base_.SecurityTokenReference.Reference.URI;
					refElem.Attributes.Append(uriAttrib);
					XmlAttribute valTypeAttrib = plainDoc.CreateAttribute(Attrib.ValueType);
					valTypeAttrib.Value = SecConv.base_.SecurityTokenReference.Reference.ValueType;
					refElem.Attributes.Append(valTypeAttrib);
				//}
				//else
				//{
				//	XmlElement keyIdElem = plainDoc.CreateElement(Pre.wsse, Elem.KeyIdentifier, Ns.wsseLatest);
				//	secTokRefElem.AppendChild(keyIdElem);
				//	XmlAttribute uriAttrib = plainDoc.CreateAttribute(Attrib.URI);
				//	uriAttrib.Value = SecConv.base_.SecurityTokenReference.KeyIdentifier.URI;
				//	keyIdElem.Attributes.Append(uriAttrib);
				//	XmlAttribute valTypeAttrib = plainDoc.CreateAttribute(Attrib.ValueType);
				//	valTypeAttrib.Value = SecConv.base_.SecurityTokenReference.KeyIdentifier.ValueType;
				//	keyIdElem.Attributes.Append(valTypeAttrib);
				//	keyIdElem.InnerText = SecConv.base_.SecurityTokenReference.KeyIdentifier.InnerText;
				//}

				//<wst:Entropy>
				//	<xenc:EncryptedKey xmlns:xenc="http://www.w3.org/2001/04/xmlenc#">
				//		<xenc:EncryptionMethod Algorithm="http://www.w3.org/2001/04/xmlenc#rsa-1_5" />
				//		<KeyInfo xmlns="http://www.w3.org/2000/09/xmldsig#">
				//			<wsse:SecurityTokenReference>
				//				<wsse:KeyIdentifier ValueType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509SubjectKeyIdentifier">bBwPfItvKp3b6TNDq+14qs58VJQ=</wsse:KeyIdentifier>
				//			</wsse:SecurityTokenReference>
				//		</KeyInfo>
				//		<xenc:CipherData>
				//			<xenc:CipherValue>qhCkEPVYmSVkO30i8oeD+Yj3zYqfQGvSRWt+VS3UZFTfbfZR1GGH2kL+lxySu5mwcJ6/a8NxQPfIvepNdola5NDnJU8mJJ3G8b7iQ6E7Byd6rxa4/RnYs37X3apK4vj0CQe1DyUFf4u999jllmi5nMVQ/G/ZzauKgerf5Rb1hwM=</xenc:CipherValue>
				//		</xenc:CipherData>
				//		<xenc:CarriedKeyName>SecurityToken-bbe94668-fe29-422a-bac6-9d4d41914313</xenc:CarriedKeyName>
				//	</xenc:EncryptedKey>
				//</wst:Entropy>
				if(SecConv.entropy != null)
				{
					//TODO entropy
					XmlElement entropyElem = plainDoc.CreateElement(Pre.wst, Elem.Entropy, Ns.wst);
					reqSecTokElem.AppendChild(entropyElem);

					//encrypted key
					XmlElement encKeyElem = plainDoc.CreateElement(Pre.xenc, Elem.EncryptedKey, Ns.xenc);
					entropyElem.AppendChild(encKeyElem);

					//encryption method
					XmlElement encMethElem = plainDoc.CreateElement(Pre.xenc, Elem.EncryptionMethod, Ns.xenc);
					XmlAttribute alg = plainDoc.CreateAttribute(Attrib.Algorithm);
					alg.Value = Alg.rsa15;
					encMethElem.Attributes.Append(alg);
					encKeyElem.AppendChild(encMethElem);
				
					//key info
					XmlElement keyInfoElem = plainDoc.CreateElement(Pre.ds, Elem.KeyInfo, Ns.ds);				
					XmlElement secTokRefElem2 = plainDoc.CreateElement(Pre.wsse, Elem.SecurityTokenReference, Ns.wsseLatest);
					XmlElement keyIdElem = plainDoc.CreateElement(Pre.wsse, Elem.KeyIdentifier, Ns.wsseLatest);
					XmlAttribute valueType = plainDoc.CreateAttribute(Attrib.ValueType);
					//valueType.Value = "wsse:X509v3";
					valueType.Value = Misc.tokenProfX509 + "#X509SubjectKeyIdentifier";
					keyIdElem.Attributes.Append(valueType);
					keyIdElem.InnerText = SecConv.KeyId;
					secTokRefElem2.AppendChild(keyIdElem);
					keyInfoElem.AppendChild(secTokRefElem2);
					encKeyElem.AppendChild(keyInfoElem);
				
					//encrypt key
					byte [] baSessKey = SecConv.SymmAlg.Key.Key;
					byte [] baEncSessKey = SecConv.RSACSP.Encrypt(baSessKey, false);
					XmlElement ciphDataElem = plainDoc.CreateElement(Pre.xenc, Elem.CipherData, Ns.xenc);
					XmlElement ciphValElem = plainDoc.CreateElement(Pre.xenc, Elem.CipherValue, Ns.xenc);
					ciphValElem.InnerText = OpenNETCF.Security.Cryptography.Internal.Format.GetB64(baEncSessKey);
					ciphDataElem.AppendChild(ciphValElem);
					encKeyElem.AppendChild(ciphDataElem);

					//carried key
					XmlElement carriedKeyNameElem = plainDoc.CreateElement(Pre.xenc, Elem.CarriedKeyName, Ns.xenc);
					carriedKeyNameElem.InnerText = SecConv.CarriedKeyName;
					encKeyElem.AppendChild(carriedKeyNameElem);
				}

				//<wsp:AppliesTo xmlns:wsp="http://schemas.xmlsoap.org/ws/2002/12/policy">
				//	<wsa:EndpointReference>
				//		<wsa:Address>http://localhost/SecureConvCodeService/SecureConvService.asmx</wsa:Address>
				//	</wsa:EndpointReference>
				//</wsp:AppliesTo>
				XmlElement appliesToElem = plainDoc.CreateElement(Pre.wsp, Elem.AppliesTo, Ns.wsp);
				reqSecTokElem.AppendChild(appliesToElem);
				XmlElement endRefElem = plainDoc.CreateElement(Pre.wsa, Elem.EndpointReference, Ns.wsaLatest);
				appliesToElem.AppendChild(endRefElem);
				XmlElement addressElem = plainDoc.CreateElement(Pre.wsa, Elem.Address, Ns.wsaLatest);
				addressElem.InnerText = SecConv.appliesTo.EndpointReference.Address.InnerText;
				endRefElem.AppendChild(addressElem);

				//<wst:LifeTime>
				//	<wsu:Expires>2004-06-14T17:55:18Z</wsu:Expires>
				//</wst:LifeTime>
				XmlElement lifeTimeElem = plainDoc.CreateElement(Pre.wst, Elem.LifeTime, Ns.wst);
				reqSecTokElem.AppendChild(lifeTimeElem);
				XmlElement expiresElem = plainDoc.CreateElement(Pre.wsu, Elem.Expires, Ns.wsuLatest);
				expiresElem.InnerText = SecConv.lifeTime.Expires.InnerText;
				lifeTimeElem.AppendChild(expiresElem);
			}
		}

		public static void TokenIssuerIn(XmlDocument convDoc)
		{
			
		}
	}
}
