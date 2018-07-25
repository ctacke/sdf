using System;
using System.Collections;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Security.Cryptography;
using OpenNETCF.Security.Cryptography;

namespace OpenNETCF.Web.Services2
{
	public class XmlSigHandler
	{
		public XmlSigHandler() {}

		public static int NumKeyBytes = 16;
		public static string StrKeyLabel = Misc.labelSecurity; //per Ales Pour, for 24 bytes

		public static XmlSigObject SigObj;
		//public static string ClearPassword;

		/*
		-generate (in order)
		1) gen refs in SignedInfo 
		2) construct SignedInfo and create sig element
		1) obtain raw data
		add wsu:Id to everything
		apply transforms / canonical
		calculate digest
		create ref elem
		2) create signed info
		apply canonical
		create actual signature value
		sig element
		w/SignedInfo, SigVlaue, KeyInfo, ...
		*/
		public static XmlDocument SignXml(XmlDocument plainDoc)
		{
			if(SigObj == null)
				return plainDoc; //nothing to sign

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
			XmlNodeList headers = header.ChildNodes;
			XmlElement security = null;
			foreach(XmlNode xn in headers)
			{
				if(xn.LocalName == Elem.Security)
					security = (XmlElement) xn;
			}
			if(security == null)
			{
				security = plainDoc.CreateElement(Pre.wsse, Elem.Security, Ns.wsseLatest);
				XmlAttribute mustUnderstand = plainDoc.CreateAttribute(Pre.soap, Attrib.mustUnderstand, Ns.soap);
				mustUnderstand.Value = "1";
				security.Attributes.Append(mustUnderstand);
				header.AppendChild(security);
			}

			XmlElement tokenElem = null;
			string secTokId = null;
			string sigAlgVal = null;
			//add BinarySecurityToken or UsernameToken under Security
			if(SigObj.BinSecTok != null)
			{
				XmlElement binSecTok = SigObj.BinSecTok.WriteXml(plainDoc, security);
				
				secTokId = SigObj.BinSecTok.Id;
				sigAlgVal = Alg.rsaSha1;
				if(SigObj.AsymmAlg is DSACryptoServiceProvider)
					sigAlgVal = Alg.dsaSha1;
				tokenElem = binSecTok;
			}
			/*
			<wsse:UsernameToken xmlns:wsu="http://schemas.xmlsoap.org/ws/2002/07/utility" wsu:Id="SecurityToken-344570f1-e3b7-42fc-9b78-f0dcd1f90bd8">
				<wsse:Username>Admin</wsse:Username>
				<wsse:Password Type="wsse:PasswordDigest">W5xVfXpb+NoV9KaPIQXUIslGGak=</wsse:Password>
				<wsse:Nonce>+7L+k37JW8qQCK1SPopXeQ==</wsse:Nonce>
				<wsu:Created>2003-10-23T04:40:04Z</wsu:Created>
			</wsse:UsernameToken>
			*/
			if(SigObj.UserTok != null)
			{
				XmlElement userTokElem = SigObj.UserTok.WriteXml(plainDoc, security);
				tokenElem = userTokElem;
				/*
				//xmlns:wsu="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"
				XmlElement userTokElem = plainDoc.CreateElement(Pre.wsse, Elem.UsernameToken, Ns.wsseLatest);
				security.AppendChild(userTokElem);
				XmlAttribute uid = plainDoc.CreateAttribute(Pre.wsu, Attrib.Id, Ns.wsuLatest);
				uid.Value = SigObj.UserTok.Id;
				userTokElem.Attributes.Append(uid);
				XmlElement userElem = plainDoc.CreateElement(Pre.wsse, Elem.Username, Ns.wsseLatest);
				userElem.InnerText = SigObj.UserTok.Username.Text;
				userTokElem.AppendChild(userElem);
				if(SigObj.UserTok.Password != null)
				{
					XmlElement passElem = plainDoc.CreateElement(Pre.wsse, Elem.Password, Ns.wsseLatest);
					XmlAttribute type = plainDoc.CreateAttribute(Attrib.Type);
					type.Value = SigObj.UserTok.Password.Type;
					passElem.Attributes.Append(type);
					passElem.InnerText = SigObj.UserTok.Password.Text;
					userTokElem.AppendChild(passElem);
				}
				XmlElement nonceElem = plainDoc.CreateElement(Pre.wsse, Elem.Nonce, Ns.wsseLatest);
				nonceElem.InnerText = SigObj.UserTok.Nonce.Text;
				userTokElem.AppendChild(nonceElem);
				XmlElement creElem = plainDoc.CreateElement(Pre.wsu, Elem.Created, Ns.wsuLatest);
				creElem.InnerText = SigObj.UserTok.Created;
				userTokElem.AppendChild(creElem);
				userTokElem.Attributes.Append(uid);
				*/
				secTokId = SigObj.UserTok.Id;
				sigAlgVal = Alg.hmacSha1;
			}
			if(SigObj.securityContextToken != null)
			{
				XmlNode sctNode = LameXpath.SelectSingleNode(header, Elem.SecurityContextToken);
				if(sctNode == null)
				{
					//i need to import this node 1st
					sctNode = plainDoc.ImportNode(SigObj.securityContextToken, true);
					string dupeId = sctNode.Attributes[Attrib.Id, Ns.wsuLatest].Value;
					XmlElement dupeElem = LameXpath.SelectSingleNode(dupeId, security);
					if(dupeElem == null)
						security.AppendChild(sctNode);
					else
						sctNode = LameXpath.SelectSingleNode(dupeId, security);
				}
				//<wsse:SecurityContextToken wsu:Id=\"SecurityToken-feb27552-6eb5-4a27-a831-e1bdfca326e2\">
				secTokId = sctNode.Attributes[Attrib.Id, Ns.wsuLatest].Value;
				sigAlgVal = Alg.hmacSha1;
				tokenElem = (XmlElement) sctNode;

				if(SigObj.derKeyTok != null)
				{
					XmlNode idElem = LameXpath.SelectSingleNode(sctNode, Elem.Identifier);
					if(idElem != null)
						SigObj.derKeyTok.secTokRef.Reference.URI = idElem.InnerText;

					XmlElement derKeyTokElem = SigObj.derKeyTok.WriteXml(plainDoc, security, (XmlElement) sctNode);
					secTokId = SigObj.derKeyTok.id;
				}
			}
			

			//add Signature element, SignedInfo, CanonicalizationMethod and SignatureMethod
			XmlElement sigElem = plainDoc.CreateElement(Pre.ds, Elem.Signature, Ns.ds);
			security.AppendChild(sigElem); //just append
			//add SignedInfo
			XmlElement sigInfoElem = plainDoc.CreateElement(Pre.ds, Elem.SignedInfo, Ns.ds);
			sigElem.AppendChild(sigInfoElem);
			XmlElement canMethElem = plainDoc.CreateElement(Pre.ds, Elem.CanonicalizationMethod, Ns.ds);
			XmlAttribute canAlg = plainDoc.CreateAttribute(Attrib.Algorithm);
			canAlg.Value = Alg.xmlExcC14n;
			canMethElem.Attributes.Append(canAlg);
			sigInfoElem.AppendChild(canMethElem);
			XmlElement sigMethElem = plainDoc.CreateElement(Pre.ds, Elem.SignatureMethod, Ns.ds);
			XmlAttribute sigAlg = plainDoc.CreateAttribute(Attrib.Algorithm);
			sigAlg.Value = sigAlgVal;
			sigMethElem.Attributes.Append(sigAlg);
			sigInfoElem.AppendChild(sigMethElem);
			
			//get each Refs element, add Id if missing
			//canonical, Digest, add ReferenceElement
			bool comments = false;
			bool exclusive = true;
            System.Security.Cryptography.SHA1CryptoServiceProvider shaCsp = new System.Security.Cryptography.SHA1CryptoServiceProvider();
			foreach(object oRef in SigObj.Refs)
			{
				XmlElement refdElem = LameXpath.SelectSingleNode(plainDoc, oRef.ToString());
                if(refdElem == null)
					continue; //cant sign it because it doesnt exist
				//get or add Id
				XmlAttribute xaId = null;
				foreach(XmlAttribute xa in refdElem.Attributes)
				{
					if(xa.LocalName == Attrib.Id)
					{
						xaId = xa;
						break;
					}
				}
				if(xaId == null)
				{
					xaId = plainDoc.CreateAttribute(Pre.wsu, Attrib.Id, Ns.wsuLatest);
					string preId = "Id-";
					if(oRef.ToString() == Elem.Timestamp)
						preId = "Timestamp-";
					xaId.Value = preId + Guid.NewGuid().ToString("D");
					refdElem.Attributes.Append(xaId);
				}
				XmlDocument xdRefd = new XmlDocument();
				xdRefd.LoadXml(refdElem.OuterXml);
				XmlCanonicalizer xc = new XmlCanonicalizer(comments, exclusive);
				MemoryStream msRefd = (MemoryStream) xc.Canonicalize(xdRefd);
				byte [] baRefd = new byte[msRefd.Length];
				msRefd.Read(baRefd, 0, baRefd.Length);
				string debugName = oRef.ToString();
				byte [] baDigest = shaCsp.ComputeHash(baRefd);
                XmlElement refElem = plainDoc.CreateElement(Pre.ds, Elem.Reference, Ns.ds);
				XmlAttribute refUri = plainDoc.CreateAttribute(Attrib.URI);
				refUri.Value = "#" + xaId.Value;
				refElem.Attributes.Append(refUri);
				sigInfoElem.AppendChild(refElem);
				XmlElement transsElem = plainDoc.CreateElement(Pre.ds, Elem.Transforms, Ns.ds);
				refElem.AppendChild(transsElem);
				XmlElement transElem = plainDoc.CreateElement(Pre.ds, Elem.Transform, Ns.ds);
				XmlAttribute transAlg = plainDoc.CreateAttribute(Attrib.Algorithm);
				transAlg.Value = Alg.xmlExcC14n;
				transElem.Attributes.Append(transAlg);
				transsElem.AppendChild(transElem);
                XmlElement digMethElem = plainDoc.CreateElement(Pre.ds, Elem.DigestMethod, Ns.ds);
				XmlAttribute digMethAlg = plainDoc.CreateAttribute("Algorithm");
				digMethAlg.Value = Alg.sha1;
				digMethElem.Attributes.Append(digMethAlg);
				refElem.AppendChild(digMethElem);
				XmlElement digValElem = plainDoc.CreateElement(Pre.ds, Elem.DigestValue, Ns.ds);
                digValElem.InnerText = OpenNETCF.Security.Cryptography.Internal.Format.GetB64(baDigest);
				refElem.AppendChild(digValElem);
			}

			//canonical SignedInfo, get key, get signature
			XmlDocument xdSigInfo = new XmlDocument();
			xdSigInfo.LoadXml(sigInfoElem.OuterXml);
			XmlCanonicalizer xcSi = new XmlCanonicalizer(comments, exclusive);
			MemoryStream msSigInfo = (MemoryStream) xcSi.Canonicalize(xdSigInfo);
			byte [] baSigInfo = new byte[msSigInfo.Length];
			msSigInfo.Read(baSigInfo, 0, baSigInfo.Length);
			byte [] baSig = null;
			if(SigObj.BinSecTok != null)
			{
				byte [] baUnsigHash = shaCsp.ComputeHash(baSigInfo);
                if (SigObj.AsymmAlg is RSACryptoServiceProvider)
                    baSig = (SigObj.AsymmAlg as RSACryptoServiceProvider).SignHash(baUnsigHash, "SHA");
                else if (SigObj.AsymmAlg is DSACryptoServiceProvider)
                    baSig = (SigObj.AsymmAlg as DSACryptoServiceProvider).SignHash(baUnsigHash, "SHA");
                else
                    throw new NotSupportedException("Only RSA and DSA are supported");
			}
			if(SigObj.UserTok != null)
			{
				byte [] derKey = P_SHA1.DeriveKey(SigObj.ClearPassword, StrKeyLabel, SigObj.UserTok.Nonce.Text, SigObj.UserTok.Created, NumKeyBytes);
				HMACSHA1 hs = new HMACSHA1(derKey);
				baSig = hs.ComputeHash(baSigInfo);
			}
			if(SigObj.securityContextToken != null)
			{
				//XmlElement createdElem = LameXpath.SelectSingleNode(SigObj.securityContextToken, "Created");
				//string strCreated = createdElem.InnerText; //2004-03-05T01:59:49Z
				//string label = "WS-Security";
				////byte [] baKey = P_SHA1.DeriveKey(password, label, nonce, created, 24);
				//byte [] baKey = P_SHA1.DeriveKey(String.Empty, label, String.Empty, strCreated, 24);
				//HMACSHA1 hs = new HMACSHA1(baKey);
				//baSig = hs.ComputeHash(baSigInfo);
				byte [] hashKey;
				if(SigObj.derKeyTok != null)
					hashKey = SigObj.derKeyTok.derKey;
				else
					hashKey = SigObj.securityContextKey;
				HMACSHA1 hs = new HMACSHA1(hashKey);
				baSig = hs.ComputeHash(baSigInfo);
			}

			//add SignatureValue
			XmlElement sigValElem = plainDoc.CreateElement(Pre.ds, Elem.SignatureValue, Ns.ds);
            sigValElem.InnerText = OpenNETCF.Security.Cryptography.Internal.Format.GetB64(baSig);
			sigElem.AppendChild(sigValElem);
			//add KeyInfo
			XmlElement keyInfoElem = plainDoc.CreateElement(Pre.ds, Elem.KeyInfo, Ns.ds);				
			XmlElement secTokRefElem = plainDoc.CreateElement(Pre.wsse, Elem.SecurityTokenReference, Ns.wsseLatest);
			XmlElement sigRefElem = plainDoc.CreateElement(Pre.wsse, Elem.Reference, Ns.wsseLatest);
			XmlAttribute uri = plainDoc.CreateAttribute(Attrib.URI);
			uri.Value = "#" + secTokId;
			sigRefElem.Attributes.Append(uri);
			XmlAttribute valueType = plainDoc.CreateAttribute(Attrib.ValueType);
			valueType.Value = "#" + secTokId;
			sigRefElem.Attributes.Append(valueType);
			
			if(SigObj.UserTok != null)
			{
				XmlAttribute valType = plainDoc.CreateAttribute(Attrib.ValueType);
				valType.Value = Misc.tokenProfUsername + "#UsernameToken";
				sigRefElem.Attributes.Append(valType);
			}
			if(SigObj.BinSecTok != null)
			{
				XmlAttribute valType = plainDoc.CreateAttribute(Attrib.ValueType);
				valType.Value = Misc.tokenProfX509 + "#X509v3";
				sigRefElem.Attributes.Append(valType);
			}
			
			secTokRefElem.AppendChild(sigRefElem);
			keyInfoElem.AppendChild(secTokRefElem);
			sigElem.AppendChild(keyInfoElem);

			//SigObj = null;
			return plainDoc;
		}

		public static void VerifySig(XmlDocument sigDoc)
		{
			try
			{
				XmlElement envelope = sigDoc.DocumentElement;

				XmlElement securityElem = LameXpath.SelectSingleNode(sigDoc, Elem.Security);
				if(securityElem != null)
				{
					XmlAttribute mustUndAtt = securityElem.Attributes[Attrib.mustUnderstand,Ns.soap];
					if(mustUndAtt != null)
						mustUndAtt.Value = "0";
				}

				XmlElement sigElem = LameXpath.SelectSingleNode(sigDoc, Elem.Signature);
				if(sigElem == null)
					return;

				XmlElement sigValElem = LameXpath.SelectSingleNode(sigElem, Elem.SignatureValue);
                byte[] baSigVal = OpenNETCF.Security.Cryptography.Internal.Format.GetB64(sigValElem.InnerText);

				bool comments = false;
				bool exclusive = true;
                System.Security.Cryptography.SHA1CryptoServiceProvider shaCsp = new System.Security.Cryptography.SHA1CryptoServiceProvider();

				XmlElement sigMethElem = LameXpath.SelectSingleNode(sigElem, Elem.SignatureMethod);
				string segMeth = sigMethElem.Attributes["Algorithm"].Value;

				XmlElement signedInfoElem = LameXpath.SelectSingleNode(sigElem, Elem.SignedInfo);
				XmlDocument xdSignedInfo = new XmlDocument();
				xdSignedInfo.LoadXml(signedInfoElem.OuterXml);
				XmlCanonicalizer xc = new XmlCanonicalizer(comments, exclusive);
				MemoryStream ms = (MemoryStream) xc.Canonicalize(xdSignedInfo);
				byte [] baMs = new byte[ms.Length];
				ms.Read(baMs, 0, baMs.Length);

				ArrayList keyInfoRefElem = LameXpath.SelectChildNodes(sigElem, Elem.SecurityTokenReference, Elem.Reference);
				XmlElement keyInfoRef = (XmlElement) keyInfoRefElem[0];
				string secTokUri = keyInfoRef.Attributes["URI"].Value;
				secTokUri = secTokUri.TrimStart(new char[]{'#'});
				XmlElement secTokElem = LameXpath.SelectSingleNode(secTokUri, sigDoc);
			
				if(secTokElem.LocalName == Elem.UsernameToken)
				{
					XmlElement nonce = LameXpath.SelectSingleNode(secTokElem, Elem.Nonce);
					XmlElement created = LameXpath.SelectSingleNode(secTokElem, Elem.Created);
					//DerivedKeyGenerator seems to be off by 1?
					//byte [] baKey = P_SHA1.DeriveKey(ClearPassword, StrKeyLabel, nonce.InnerText, created.InnerText, NumKeyBytes);
					byte [] baKey = P_SHA1.DeriveKey(SigObj.ClearPassword, StrKeyLabel, nonce.InnerText, created.InnerText, NumKeyBytes);
					HMACSHA1 hmacSha = new HMACSHA1(baKey);
					byte [] baSig = hmacSha.ComputeHash(baMs);
                    OpenNETCF.Security.Cryptography.Internal.Format.SameBytes(baSigVal, baSig);
				}
				else if(secTokElem.LocalName == Elem.BinarySecurityToken)
				{
                    byte[] baCert = OpenNETCF.Security.Cryptography.Internal.Format.GetB64(secTokElem.InnerText);
					X509Certificate cert = new X509Certificate(baCert); //pub key to verify sig.
					byte [] exponent;
					byte [] modulus;
					DecodeCertKey.GetPublicRsaParams(cert, out exponent, out modulus);
                    System.Security.Cryptography.RSAParameters rsaParam = new System.Security.Cryptography.RSAParameters();
					rsaParam.Exponent = exponent;
					rsaParam.Modulus = modulus;
                    System.Security.Cryptography.RSACryptoServiceProvider rsaCsp = new System.Security.Cryptography.RSACryptoServiceProvider();
					rsaCsp.ImportParameters(rsaParam);
               
					byte [] baUnsigHash = shaCsp.ComputeHash(baMs);
					bool valid = rsaCsp.VerifyHash(baUnsigHash, "SHA", baSigVal);
					if(valid == false)
						throw new Exception("signature is not valid");
				}
				else if(secTokElem.LocalName == Elem.SecurityContextToken)
				{
					//TODO how to validate signature?
				}
				else
				{
					throw new Exception("only support Username, BinarySecurity, and SecurityContext Token signature");
				}

				//verify reference hashes
				string refdName = String.Empty;
				ArrayList refNodes = LameXpath.SelectChildNodes(sigDoc, Elem.SignedInfo, Elem.Reference);
				foreach(object oXn in refNodes)
				{
					XmlNode xn = (XmlNode) oXn;
					string uriId = xn.Attributes[Attrib.URI].Value;
					uriId = uriId.TrimStart(new char[]{'#'});
					XmlElement digValElem = LameXpath.SelectSingleNode(xn, Elem.DigestValue);
                    byte[] baDigest = OpenNETCF.Security.Cryptography.Internal.Format.GetB64(digValElem.InnerText);

					XmlElement refdElem = LameXpath.SelectSingleNode(uriId, sigDoc);
					XmlDocument xdRefdElem = new XmlDocument();
					refdName = refdElem.LocalName; //for debug visibility
					xdRefdElem.LoadXml(refdElem.OuterXml);
					//not reusable
					xc = new XmlCanonicalizer(comments, exclusive);
					//MemoryStream ms = (MemoryStream) xc.Canonicalize(refdElem);
					ms = (MemoryStream) xc.Canonicalize(xdRefdElem);
					baMs = new byte[ms.Length];
					ms.Read(baMs, 0, baMs.Length);
					byte [] baHash = shaCsp.ComputeHash(baMs);
					try
					{
                        OpenNETCF.Security.Cryptography.Internal.Format.SameBytes(baDigest, baHash);
					}
					catch(Exception ex)
					{
						throw new Exception(refdName + ":" + ex.Message, ex);
					}
				}
			}
			finally
			{
				//ClearPassword = null;
				SigObj = null;
			}
		}
	}
}
