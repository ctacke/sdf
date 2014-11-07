
using System; 
using System.Xml;
using System.Xml.Serialization; 
using System.Web.Services.Protocols; 

	/*
	<wsse:Security soap:mustUnderstand="1" xmlns:wsse="http://schemas.xmlsoap.org/ws/2002/07/secext">
	</wsse:Security>
	*/

namespace OpenNETCF.Web.Services2
{
	//add public member to the auto gen'd proxy 
	//public SecurityHeader securityHeader; 
	//add to WebMethod that will be called on auto gen'd proxy 
	//[System.Web.Services.Protocols.SoapHeaderAttribute("securityHeader")]
	[XmlRoot(Namespace=Ns.wsse, ElementName="Security")] 
	public class SecurityHeader : SoapHeader 
	{ 
		public SecurityHeader() 
		{
			this.MustUnderstand = true;
			//this.UsernameToken = new UsernameToken();
		} 
  
		//optional
		[CLSCompliant(false)]
		[XmlAttribute(Namespace=Ns.S)]
		public string actor;

		//typed
		[XmlElement(Namespace=Ns.wsse)] 
		public UsernameToken UsernameToken;
		[XmlElement(Namespace=Ns.wsse)] 
		public BinarySecurityToken BinarySecurityToken;
		
		[XmlElement(Namespace=Ns.xenc)] 
		public ReferenceList ReferenceList;
		[XmlElement(Namespace=Ns.xenc)] 
		public EncryptedKey EncryptedKey;

		[XmlElement(Namespace=Ns.wsu)] 
		public TimestampHeader Timestamp;

		//any
		//[XmlAnyElement]
		//public XmlElement [] anyElements;
		//[XmlAnyAttribute]
		//public XmlAttribute [] anyAttributes;
	}

	public enum EncodingType
	{
		Base64Binary,
		HexBinary,
	}

	public enum Errors
	{
		//Unsupported
		UnsupportedSecurityToken, //An unsupported token was provided
		UnsupportedAlgorithm, //An unsupported signature or encryption algorithm was used
		//Failure
		InvalidSecurity, //An error was discovered processing the <Security> header.  
		InvalidSecurityToken, //An invalid security token was provided 
		FailedAuthentication, //The security token could not be authenticated or authorized  
		FailedCheck, //The signature or decryption was invalid 
		SecurityTokenUnavailable, //Referenced security token could not be retrieved 
	}
}