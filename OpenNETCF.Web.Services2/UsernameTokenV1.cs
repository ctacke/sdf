using System; 
using System.Xml;
using System.Xml.Serialization; 
using System.Web.Services.Protocols; 
using System.Text;
using System.Security.Cryptography;

namespace OpenNETCF.Web.Services2
{
	[XmlRoot(Namespace=Ns.wsse0207, ElementName="Security")] 
	public class SecurityHeaderV1 : SoapHeader 
	{ 
		public SecurityHeaderV1() 
		{
			this.MustUnderstand = true;
		} 
  
		//optional
		[CLSCompliant(false)]
		[XmlAttribute(Namespace=Ns.S)]
		public string actor;

		//typed
		[XmlElement(Namespace=Ns.wsse0207)] 
		public UsernameTokenV1 UsernameToken;
		[XmlElement(Namespace=Ns.wsse0207)] 
		public BinarySecurityToken BinarySecurityToken;
		
		[XmlElement(Namespace=Ns.wsse0207)] 
		public ReferenceList ReferenceList;
		[XmlElement(Namespace=Ns.wsse0207)] 
		public EncryptedKey EncryptedKey;
	}

	public class UsernameTokenV1
	{
		public UsernameTokenV1() {}

		public UsernameTokenV1(string username, string password, 
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
			}

			this.Password = new PasswordV1();

			if(passType == PasswordOption.SendPlainText)
			{
				this.Password.Type = "wsse:PasswordText";
				this.Password.Text = password;
			}

			if(passType == PasswordOption.SendHashed)
			{
				this.Password.Type = "wsse:PasswordDigest";

				DateTime dtCreated = DateTime.UtcNow;
				this.Created = XmlConvert.ToString(dtCreated, "yyyy-MM-ddTHH:mm:ssZ");
				byte [] baCreated = Encoding.UTF8.GetBytes(this.Created);

				this.Nonce = new Nonce();
				//this random number gen is not as strong
				//Random r = new Random(Environment.TickCount);
				//byte [] baNonce = new byte[20];
				//r.NextBytes(baNonce);
                byte[] baNonce = new byte[20];//OpenNETCF.Security.Cryptography.Internal.Rand.GetRandomBytes(20);
                rng.GetBytes(baNonce);

				this.Nonce.Text = Convert.ToBase64String(baNonce, 0, baNonce.Length);

				byte [] baPassword = Encoding.UTF8.GetBytes(password);

				int baDigestLength = baNonce.Length + baCreated.Length + baPassword.Length; 
				byte [] baDigest = new byte[baDigestLength]; 
				Array.Copy(baNonce, 0, baDigest, 0, baNonce.Length); 
				Array.Copy(baCreated, 0, baDigest, baNonce.Length, baCreated.Length); 
				Array.Copy(baPassword, 0, baDigest, baNonce.Length + baCreated.Length, baPassword.Length); 
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
					this.Password.Type = "wsse:HexBinary";
                    this.Password.Text = OpenNETCF.Security.Cryptography.Internal.Format.GetHexBin(hash);
				}
			}
		}

		[XmlAttribute(Namespace=Ns.wsu0207)] 
		public string Id;

		//required
		[XmlElement(Namespace=Ns.wsse0207)] 
		public Username Username; 

		//optional
		[XmlElement(Namespace=Ns.wsse0207)]
		public PasswordV1 Password;
		[XmlElement(Namespace=Ns.wsse0207)] 
		public Nonce Nonce;
		[XmlElement(Namespace=Ns.wsu0207)] 
		public string Created;
	}

	public class PasswordV1
	{
		public PasswordV1() {}

		[XmlText()]
		public string Text;
		//[XmlAttribute(Namespace=Ns.wsse0207)] 
		[XmlAttribute()] 
		public string Type; 

		//any
		//[XmlAnyAttribute]
		//public XmlAttribute [] anyAttributes;
	}
}