
using System;

namespace OpenNETCF.Web.Services2
{
	/// <summary>
	/// Summary description for Ns.
	/// </summary>
	public class Ns
	{
		//static - modifiable
		public static string wsaLatest = wsa;
		public static string wsuLatest = wsu;
		public static string wsseLatest = wsse;

		//xml
		public const string xmlns = "http://www.w3.org/2000/xmlns/";
		//soap
		public const string soap = "http://schemas.xmlsoap.org/soap/envelope/";
		public const string xsi = "http://www.w3.org/2001/XMLSchema-instance";
		public const string xsd = "http://www.w3.org/2001/XMLSchema";

		//security
		public const string S = "http://www.w3.org/2001/12/soap-envelope";
		public const string S11 = "http://schemas.xmlsoap.org/soap/envelope/";
		public const string S12 = "http://www.w3.org/2003/05/soap-envelope";
		public const string ds = "http://www.w3.org/2000/09/xmldsig#";
		public const string xenc = "http://www.w3.org/2001/04/xmlenc#";
		public const string m = "http://schemas.xmlsoap.org/rp";
		//security addendum
		public const string wsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
		public const string wsu0207 = "http://schemas.xmlsoap.org/ws/2002/07/utility";
		public const string wsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
		public const string wsse0212 = "http://schemas.xmlsoap.org/ws/2002/12/secext";
		public const string wsse0207 = "http://schemas.xmlsoap.org/ws/2002/07/secext";
		public const string wsse0204 = "http://schemas.xmlsoap.org/ws/2002/04/secext";

		//trust
		public const string wst = "http://schemas.xmlsoap.org/ws/2004/04/trust";
		//secure conversation
		public const string wssc = "http://schemas.xmlsoap.org/ws/2004/04/sc";

		//addressing
		public const string wsa = "http://schemas.xmlsoap.org/ws/2004/03/addressing";
		public const string wsa0303 = "http://schemas.xmlsoap.org/ws/2003/03/addressing";
		public const string wsp = "http://schemas.xmlsoap.org/ws/2002/12/policy";
		public const string xs = "http://www.w3.org/2001/XMLSchema";
		//routing
		public const string wsrp = "http://schemas.xmlsoap.org/rp";
		public const string r = "http://schemas.xmlsoap.org/2001/10/referral/";
		//eventing
		public const string wse = "http://schemas.xmlsoap.org/ws/2004/01/eventing";
		//other gxa
		public const string wscoor = "http://schemas.xmlsoap.org/ws/2002/08/wsccor";
		public const string wsil = "http://schemas.xmlsoap.org/ws/2001/10/inspection";
		public const string wsrm = "http://schemas.xmlsoap.org/ws/2003/03/rm";
		public const string wsba = "http://schemas.xmlsoap.org/ws/2002/08/wsba";
		public const string wstx = "http://schemas.xmlsoap.org/ws/2002/08/wstx";
		//pending
		public const string xop = "http://www.w3.org/2003/12/xop/include";
		public const string xdt = "http://www.w3.org/2003/05/xpath-datatypes";
	}

	public class Misc
	{
		public const string labelSecurity = "WS-Security";
		public const string labelSecureConversation = "WS-SecureConversation";

		public const string wsaAddress = "http://schemas.xmlsoap.org/ws/2004/03/addressing/role/anonymous";
		public const string wsaAddress0303 = "http://schemas.xmlsoap.org/ws/2003/03/addressing/role/anonymous";
		public const string pathActorNext = "http://schemas.xmlsoap.org/soap/actor/next";

		public const string tokenProfUsername = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0"; //# passwordDigest
		public const string tokenProfX509 = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0"; //# valueType
		public const string encodingType = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0"; //#

		public const string secScSct = "http://schemas.xmlsoap.org/ws/2004/04/security/sc/sct";
		public const string secScDkPsha1 = "http://schemas.xmlsoap.org/ws/2004/04/security/sc/dk/p_sha1";
		public const string secTrustIssue = "http://schemas.xmlsoap.org/ws/2004/04/security/trust/Issue";

		public const string plainTextTypeContent = "http://www.w3.org/2001/04/xmlenc#Content";
		public const string plainTextTypeElement = "http://www.w3.org/2001/04/xmlenc#Element";
	}

	public class Alg //AlgorithmEnum?
	{
		public const string sha1 = "http://www.w3.org/2000/09/xmldsig#sha1";
		public const string dsaSha1 = "http://www.w3.org/2000/09/xmldsig#dsa-sha1";
		public const string rsaSha1 = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
		public const string hmacSha1 = "http://www.w3.org/2000/09/xmldsig#hmac-sha1";

		public const string tripledesCbc = "http://www.w3.org/2001/04/xmlenc#tripledes-cbc";
		public const string aes128cbc = "http://www.w3.org/2001/04/xmlenc#aes128-cbc";
		public const string kwTripledes = "http://www.w3.org/2001/04/xmlenc#kw-tripledes";
		public const string kwAes128 = "http://www.w3.org/2001/04/xmlenc#kw-aes128";

		public const string rsa15 = "http://www.w3.org/2001/04/xmlenc#rsa-1_5";
		public const string xmlExcC14n = "http://www.w3.org/2001/10/xml-exc-c14n#";
	}

	public class Pre
	{
		public const string ds = "ds";
		public const string m = "m";
		public const string r = "r";
		public const string S = "S";
		public const string soap = "soap";
		public const string wsa = "wsa";
		public const string wscoor = "wscoor";
		public const string wsba = "wsba";
		public const string wse = "wse";
		public const string wsil = "wsil";
		public const string wsp = "wsp";
		public const string wsrm = "wsrm";
		public const string wsrp = "wsrp";
		public const string wssc = "wssc";
		public const string wsse = "wsse";
		public const string wst = "wst";
		public const string wstx = "wstx";
		public const string wsu = "wsu";
		public const string xenc = "xenc";
		public const string xmlns = "xmlns";
		public const string xs = "xs";
		public const string xsd = "xsd";
		public const string xsi = "xsi";
	}

	public class Elem
	{
		public const string Action = "Action";
		public const string Address = "Address";
		public const string AppliesTo = "AppliesTo";
		public const string Base = "Base";
		public const string BinarySecurityToken = "BinarySecurityToken";
		public const string Body = "Body";
		public const string CanonicalizationMethod = "CanonicalizationMethod";
		public const string CarriedKeyName = "CarriedKeyName";
		public const string CipherData = "CipherData";
		public const string CipherValue = "CipherValue";
		public const string ComputedKey = "ComputedKey";
		public const string Created = "Created";
		public const string DataReference = "DataReference";
		public const string DerivedKeyToken = "DerivedKeyToken";
		public const string DigestMethod = "DigestMethod";
		public const string DigestValue = "DigestValue";
		public const string EncryptedData = "EncryptedData";
		public const string EncryptedKey = "EncryptedKey";
		public const string EncryptionMethod = "EncryptionMethod";
		public const string EndpointReference = "EndpointReference";
		public const string Entropy = "Entropy";
		public const string Expires = "Expires";
		public const string From = "From";
		public const string Generation = "Generation";
		public const string Header = "Header";
		public const string Identifier = "Identifier";
		public const string KeyIdentifier = "KeyIdentifier";
		public const string KeyInfo = "KeyInfo";
		public const string KeyName = "KeyName";
		public const string Label = "Label";
		public const string Length = "Length";
		public const string LifeTime = "LifeTime";
		public const string MessageID = "MessageID";
		public const string Nonce = "Nonce";
		public const string Password = "Password";
		public const string Reference = "Reference";
		public const string ReferenceList = "ReferenceList";
		public const string ReplyTo = "ReplyTo";
		public const string RequestedProofToken = "RequestedProofToken";
		public const string RequestedSecurityToken = "RequestedSecurityToken";
		public const string RequestSecurityToken = "RequestSecurityToken";
		public const string RequestSecurityTokenResponse = "RequestSecurityTokenResponse";
		public const string RequestType = "RequestType";
		public const string Security = "Security";
		public const string SecurityContextToken = "SecurityContextToken";
		public const string SecurityTokenReference = "SecurityTokenReference";
		public const string Signature = "Signature";
		public const string SignatureMethod = "SignatureMethod";
		public const string SignatureValue = "SignatureValue";
		public const string SignedInfo = "SignedInfo";
		public const string Timestamp = "Timestamp";
		public const string To = "To";
		public const string TokenType = "TokenType";
		public const string Transform = "Transform";
		public const string Transforms = "Transforms";
		public const string Username = "Username";
		public const string UsernameToken = "UsernameToken";

		[CLSCompliant(false)]
		public const string action = "action";
		public const string id = "id";
		public const string path = "path";
		[CLSCompliant(false)]
		public const string to = "to";
	}

	public class Attrib
	{
		public const string Algorithm = "Algorithm";
		public const string EncodingType = "EncodingType";
		public const string Id = "Id";
		public const string Type = "Type";
		public const string URI = "URI";
		public const string ValueType = "ValueType";

		public const string actor = "actor";
		public const string mustUnderstand = "mustUnderstand";
	}
}
