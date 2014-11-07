using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Security.Cryptography;
using OpenNETCF.Web.Services2.Security.Cryptography;

namespace OpenNETCF.Web.Services2
{
	public class XmlEncObject
	{
		public XmlEncObject()
		{
			//defaults
			TargetElement = "Body";
			Id = "EncryptedContent-" + Guid.NewGuid().ToString("D");
			Type = PlainTextType.Content;
		}

		public XmlElement securityContextToken;
		public byte [] securityContextKey;
		//header
		public SecurityTokenReference SecTokRef; //symmetric
		public BinarySecurityToken BinSecTok; //response enc
		public EncryptedKey EncKey; 

		public UsernameToken UserTok;
		public string ClearPassword;
		//public string ValueType;
		//public string UserTokenId;

		public string TargetElement;
		public string Id;
		public PlainTextType Type;

		public X509Certificate X509Cert;
		public string KeyId;
		public RSACryptoServiceProvider RSACSP; //only one that can encrypt
		public SymmetricKeyAlgorithm SymmAlg; //3DES or AES
		public string KeyName;

		public DerivedKeyToken derKeyTok;
		public SymmetricAlgorithm keyWrap;
	}
}
