
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Security.Cryptography.X509Certificates;

/*
<wsse:BinarySecurityToken
             xmlns:wsse="http://schemas.xmlsoap.org/ws/2002/04/secext" 
             Id="myToken"
             ValueType="wsse:X509v3"
             EncodingType="wsse:Base64Binary">
             MIIEZzCCA9CgAwIBAgIQEmtJZc0...
	       </wsse:BinarySecurityToken>

<wsse:BinarySecurityToken 
             xmlns:wsse="http://schemas.xmlsoap.org/ws/2002/04/secext" 
             Id="myToken"
             ValueType="x:MyType" xmlns:x="http://fabrikam123.com/x"
             EncodingType="wsse:Base64Binary">
             MIIEZzCCA9CgAwIBAgIQEmtJZc0...
          </wsse:BinarySecurityToken>

<BinarySecurityToken 
			d4p1:Id=\"SecurityToken-be7ae32c-bcb9-c7a2-1135-30fcd5310c8d\" 
			ValueType=\"wsse:X509v3\" 
			EncodingType=\"wsse:Base64Binary\" 
			xmlns:d4p1=\"http://schemas.xmlsoap.org/ws/2002/07/utility\">
			MIIF0DCCBLigAwIBAgIKN+E7vwA...
		</BinarySecurityToken>

1993 columns - GetRawCertData
MIIF0DCCBLigAwIBAgIKN+E7vwAAAABgQzANBgkqhkiG9w0BAQUFADCByDEjMCEGCSqGSIb3DQEJARYUdGVzdGNhQG1pY3Jvc29
mdC5jb20xCzAJBgNVBAYTAlVTMQswCQYDVQQIEwJXQTEQMA4GA1UEBxMHUmVkbW9uZDEvMC0GA1UEChMmTWljcm9zb2Z0LCBJbn
Rlcm9wYWJpbGl0eSBUZXN0aW5nIE9ubHkxLzAtBgNVBAsTJk1pY3Jvc29mdCwgSW50ZXJvcGFiaWxpdHkgVGVzdGluZyBPbmx5M
RMwEQYDVQQDEwpTRUNURVNUQ0ExMB4XDTAyMDkxNzIyMzI0NVoXDTAyMTEwMzAwNTU1OVowgasxCzAJBgNVBAYTAlVTMQswCQYD
VQQIEwJDQTEQMA4GA1UEBxMHT2FrbGFuZDEZMBcGA1UEChMQQ3VycmVudFVzZXIgQ2VydDEaMBgGA1UECxMRV1NESyBXZWIgU2V
ydmljZXMxGDAWBgNVBAMTD09ha0xlYWYgU3lzdGVtczEsMCoGCSqGSIb3DQEJARYdUm9nZXJfSmVubmluZ3NAY29tcHVzZXJ2ZS
5jb20wgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBAM9sFDLuBov/661sTcehoW9FQN1H/ZVaiEdYhV8gRiIiTnaKICsMltQqt
m8Fq9vRC+o87N2+WXcNW25+fUTpam3NrBCee2DaT8mvu8V7aAiYBaGYcW73u3gUIKeZIk85pzL9xZkGI2telifQe2HCfBOH86U2
smvX2vLgFGqub4orAgMBAAGjggJZMIICVTAOBgNVHQ8BAf8EBAMCBPAwRAYJKoZIhvcNAQkPBDcwNTAOBggqhkiG9w0DAgICAIA
wDgYIKoZIhvcNAwQCAgCAMAcGBSsOAwIHMAoGCCqGSIb3DQMHMB0GA1UdDgQWBBRtnh9GiV8PLAV0oKAlE1uLswOxiDATBgNVHS
UEDDAKBggrBgEFBQcDAjAfBgNVHSMEGDAWgBT9IPDHsSVmnS2sGwYhF3iLnMKUNjCCAQkGA1UdHwSCAQAwgf0wgfqggfeggfSGL
WZpbGU6Ly9cXFNFQ1RFU1RDQTRcQ2VydEVucm9sbFxTRUNURVNUQ0ExLmNybIY6aHR0cDovL3NlY3Rlc3QucnRlLm1pY3Jvc29m
dC5jb20vY2VydGVucm9sbC9zZWN0ZXN0Y2ExLmNybIYraHR0cDovL3NlY3Rlc3RjYTQvQ2VydEVucm9sbC9TRUNURVNUQ0ExLmN
ybIYraHR0cDovL3NlY3Rlc3RjYTQvQ2VydEVucm9sbC9TRUNURVNUQ0ExLmNybIYtZmlsZTovL1xcU0VDVEVTVENBNFxDZXJ0RW
5yb2xsXFNFQ1RFU1RDQTEuY3JsMIGaBggrBgEFBQcBAQSBjTCBijBCBggrBgEFBQcwAoY2aHR0cDovL3NlY3Rlc3RjYTQvQ2Vyd
EVucm9sbC9TRUNURVNUQ0E0X1NFQ1RFU1RDQTEuY3J0MEQGCCsGAQUFBzAChjhmaWxlOi8vXFxTRUNURVNUQ0E0XENlcnRFbnJv
bGxcU0VDVEVTVENBNF9TRUNURVNUQ0ExLmNydDANBgkqhkiG9w0BAQUFAAOCAQEAI0EVCHVORu2iXkOsWk1gvAPeESowUeefqmy
fEPZVW5EoLwlythet57/PGiTw9XpavL6Pd3v2NME+w2POhOV/mI7bqGuefZ2/p6xnnwzWiig4us4Cey7+hqOyZuolmVBzl8RShx
4AHyA5LIiqxBJtR046XL4tNLkNcUVDghiGKuv+DaNCQQyZ5md5wdMZ/v1m54nEY70ymoJ165q4ML/veWkT9/dyV6mPssw1YT95i
f87JPToXZ5R+uhBnpSXewUo6AXKkYIqqkrh0FaGBrJLCyMgqxWGElrFsodxSLXbB31fpxF+PgSvcRcnbKX2xWjRtjz2ZVQ1bVNL
N8pNC/s3ew==

ss.securityHeader = new bNb.SecurityHeader();
ss.securityHeader.BinarySecurityToken = new BinarySecurityToken();
ss.securityHeader.BinarySecurityToken.SetX509Cert(cert, EncodingType.Base64Binary);
*/

namespace OpenNETCF.Web.Services2
{
	//[XmlRoot(Namespace=Ns.wsse)]
	public class BinarySecurityToken
	{
		public BinarySecurityToken() {}

		public BinarySecurityToken(X509Certificate cert, EncodingType encType)
		{
			System.Guid g = Guid.NewGuid();
			this.Id = "SecurityToken-" + g.ToString("D");
			//this.ValueType = "wsse:X509v3";
			this.ValueType = Misc.tokenProfX509 + "#X509v3";
			//which to pass (dont pass private keys!)
			//byte [] pubKeyBa = cert.GetPublicKey(); //140 bytes
			byte [] pubKeyBa = cert.GetRawCertData(); //1492 bytes
			if(encType == OpenNETCF.Web.Services2.EncodingType.Base64Binary)
			{
				//this.EncodingType = "wsse:Base64Binary";
				this.EncodingType = Misc.encodingType + "#Base64Binary";
				this.text = Convert.ToBase64String(pubKeyBa, 0, pubKeyBa.Length);
			}
			if(encType == OpenNETCF.Web.Services2.EncodingType.HexBinary)
			{
				//this.EncodingType = "wsse:HexBinary";
				this.EncodingType = Misc.encodingType + "#HexBinary";
                this.text = OpenNETCF.Security.Cryptography.Internal.Format.GetHexBin(pubKeyBa);
			}
		}

		public XmlElement WriteXml(XmlDocument plainDoc, XmlElement security)
		{
			XmlElement binSecTok = plainDoc.CreateElement(Pre.wsse, Elem.BinarySecurityToken, Ns.wsseLatest);
			XmlAttribute valueType = plainDoc.CreateAttribute(Attrib.ValueType);
			valueType.Value = this.ValueType;
			binSecTok.Attributes.Append(valueType);
			XmlAttribute encType = plainDoc.CreateAttribute(Attrib.EncodingType);
			encType.Value = this.EncodingType;
			binSecTok.Attributes.Append(encType);
			XmlAttribute bid = plainDoc.CreateAttribute(Pre.wsu, Attrib.Id, Ns.wsuLatest);
			bid.Value = this.Id;
			binSecTok.Attributes.Append(bid);
			binSecTok.InnerText = this.text;
			security.AppendChild(binSecTok);
			return binSecTok;
		}

		[XmlAttribute(Namespace=Ns.wsu)] 
		public string Id;
		[XmlAttribute()] 
		public string ValueType;
		[XmlAttribute()] 
		public string EncodingType;
		
		[XmlText()] 
		public string text;

		//any
		//[XmlAnyAttribute]
		//public XmlAttribute [] anyAttributes;
	}

	public enum ValueType
	{
		X509v3, //X.509 v3 certificate 
		Kerberosv5TGT, //Kerberos v5 ticket as defined in Section 5.3.1 of Kerberos. This ValueType is used when the ticket is a ticket granting ticket (TGT) 
		Kerberosv5ST, //Kerberos v5 ticket as defined in Section 5.3.1 of Kerberos. This ValueType is used when the ticket is a service ticket (ST)
		PKCS7,
		PKIPath,
	}
}
