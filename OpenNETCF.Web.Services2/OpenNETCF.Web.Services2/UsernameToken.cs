
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

using System.Security.Cryptography;

/*
<wsse:UsernameToken xmlns:wsu="http://schemas.xmlsoap.org/ws/2002/07/utility" wsu:Id="SecurityToken-ab1d05d8-9c30-4b95-8a5a-c0fb783e88ec">
  <wsse:Username>5671mVwIyooyAwNspl60UZHOsU+P26wp</wsse:Username>
  <wsse:Password Type="wsse:PasswordDigest">ZUGAntcM9QJQOPB6kggItfAxPlw=</wsse:Password>
  <wsse:Nonce>25mxnkz0tyQeKcaxoXI1+Q==</wsse:Nonce>
  <wsu:Created>2003-09-15T18:00:24Z</wsu:Created>
</wsse:UsernameToken>

ss.securityHeader = new bNb.SecurityHeader();
ss.securityHeader.UsernameToken = new UsernameToken();
ss.securityHeader.UsernameToken.SetUserPass(username, password, PasswordOption.SendHashed, EncodingType.Base64Binary);
*/

namespace OpenNETCF.Web.Services2
{
	//[XmlRoot(Namespace=Ns.wsse)]
	public class UsernameToken
	{
		public UsernameToken() {}

		public UsernameToken(string username, string password, 
			PasswordOption passType, EncodingType encType)
		{
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                
			this.Username = new Username();
			this.Username.Text = username;

			System.Guid g = Guid.NewGuid();
			this.Id = "SecurityToken-" + g.ToString("D");

			if(passType == PasswordOption.SendNone)
			{
				this.Password = null;
				DateTime dtCreated = DateTime.UtcNow;
				this.Created = XmlConvert.ToString(dtCreated, "yyyy-MM-ddTHH:mm:ssZ");

				this.Nonce = new Nonce();

                byte[] baNonce = new byte[20]; // OpenNETCF.Security.Cryptography.Internal.Rand.GetRandomBytes(20);
                rng.GetBytes(baNonce);

				this.Nonce.Text = Convert.ToBase64String(baNonce, 0, baNonce.Length);
				return;
			}

			this.Password = new Password();

			if(passType == PasswordOption.SendPlainText)
			{
				//this.Password.Type = "wsse:PasswordText";
				this.Password.Type = Misc.tokenProfUsername + "#PasswordText";
				this.Password.Text = password;
			}

			if(passType == PasswordOption.SendHashed)
			{
				//this.Password.Type = "wsse:PasswordDigest";
				this.Password.Type = Misc.tokenProfUsername + "#PasswordDigest";

				DateTime dtCreated = DateTime.UtcNow;
				this.Created = XmlConvert.ToString(dtCreated, "yyyy-MM-ddTHH:mm:ssZ");
				byte [] baCreated = Encoding.UTF8.GetBytes(this.Created);

				this.Nonce = new Nonce();
				//this random number gen is not as strong
				//Random r = new Random(Environment.TickCount);
				//byte [] baNonce = new byte[20];
				//r.NextBytes(baNonce);
                byte[] baNonce = new byte[20];// OpenNETCF.Security.Cryptography.Internal.Rand.GetRandomBytes(20);
                rng.GetBytes(baNonce);
				this.Nonce.Text = Convert.ToBase64String(baNonce, 0, baNonce.Length);

				byte [] baPassword = Encoding.UTF8.GetBytes(password);

				int baDigestLength = baNonce.Length + baCreated.Length + baPassword.Length; 
				byte [] baDigest = new byte[baDigestLength]; 
				Array.Copy(baNonce, 0, baDigest, 0, baNonce.Length); 
				Array.Copy(baCreated, 0, baDigest, baNonce.Length, baCreated.Length); 
				Array.Copy(baPassword, 0, baDigest, baNonce.Length + baCreated.Length, baPassword.Length);
 				
				//byte [] hash = Hash.ComputeHash(CalgHash.SHA1, baDigest);
				SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
				byte [] hash = sha1.ComputeHash(baDigest);

				if(encType == EncodingType.Base64Binary)
				{
					//default is Base64Binary so dont have to set
					//this.Password.Type = "wsse:Base64Binary";
					this.Password.Text = Convert.ToBase64String(hash, 0, hash.Length);
				}
				if(encType == EncodingType.HexBinary)
				{
					//this.Password.Type = "wsse:HexBinary";
					this.Password.Type = Misc.tokenProfUsername + "#HexBinary";
                    this.Password.Text = OpenNETCF.Security.Cryptography.Internal.Format.GetHexBin(hash);
				}
			}
		}

		public XmlElement WriteXml(XmlDocument plainDoc, XmlNode security)
		{
			XmlElement userTokElem = plainDoc.CreateElement(Pre.wsse, Elem.UsernameToken, Ns.wsseLatest);
			security.AppendChild(userTokElem);
			XmlAttribute uid = plainDoc.CreateAttribute(Pre.wsu, Attrib.Id, Ns.wsuLatest);
			uid.Value = this.Id;
			userTokElem.Attributes.Append(uid);
			XmlElement userElem = plainDoc.CreateElement(Pre.wsse, Elem.Username, Ns.wsseLatest);
			userElem.InnerText = this.Username.Text;
			userTokElem.AppendChild(userElem);
			if(this.Password != null)
			{
				XmlElement passElem = plainDoc.CreateElement(Pre.wsse, Elem.Password, Ns.wsseLatest);
				XmlAttribute type = plainDoc.CreateAttribute(Attrib.Type);
				type.Value = this.Password.Type;
				passElem.Attributes.Append(type);
				passElem.InnerText = this.Password.Text;
				userTokElem.AppendChild(passElem);
			}
			XmlElement nonceElem = plainDoc.CreateElement(Pre.wsse, Elem.Nonce, Ns.wsseLatest);
			nonceElem.InnerText = this.Nonce.Text;
			userTokElem.AppendChild(nonceElem);
			XmlElement creElem = plainDoc.CreateElement(Pre.wsu, Elem.Created, Ns.wsuLatest);
			creElem.InnerText = this.Created;
			userTokElem.AppendChild(creElem);
			userTokElem.Attributes.Append(uid);
			//secTokId = SigObj.UserTok.Id;
			//sigAlgVal = "http://www.w3.org/2000/09/xmldsig#hmac-sha1";
			return userTokElem;
		}

		[XmlAttribute(Namespace=Ns.wsu)] 
		public string Id;

		//required
		[XmlElement(Namespace=Ns.wsse)] 
		public Username Username; 

		//optional
		[XmlElement(Namespace=Ns.wsse)]
		public Password Password;
		[XmlElement(Namespace=Ns.wsse)] 
		public Nonce Nonce;
		[XmlElement(Namespace=Ns.wsu)] 
		public string Created;

		//any
		//[XmlAnyElement]
		//public XmlElement [] anyElements;
		//[XmlAnyAttribute]
		//public XmlAttribute [] anyAttributes;
	}

	public class Nonce
	{
		public Nonce() {}

		[XmlText()]
		public string Text;
		[XmlAttribute(Namespace=Ns.wsse)]
		public string EncodingType;
	}

	public class Username
	{
		public Username() {}

		[XmlText()]
		public string Text;

		//any
		//[XmlAnyAttribute]
		//public XmlAttribute [] anyAttributes;
	}

	public class Password
	{
		public Password() {}

		[XmlText()]
		public string Text;
		[XmlAttribute()] 
		public string Type; 

		//any
		//[XmlAnyAttribute]
		//public XmlAttribute [] anyAttributes;
	}

	public enum PasswordOption
	{
		SendHashed,
		SendNone,
		SendPlainText,
	}


	}
