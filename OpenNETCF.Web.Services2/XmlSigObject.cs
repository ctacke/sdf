using System;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Xml;
using System.Security.Cryptography;

namespace OpenNETCF.Web.Services2
{
	public class XmlSigObject
	{
		public XmlSigObject()
		{
			//defaults
			//For digital signing, the WSE signs the entire contents of the <Body> element, 
			//the Created and Expires elements of the WS-Timestamp SOAP header, 
			//and the Action, To, Id, and From elements of the WS-Routing SOAP header.
			Refs = new ArrayList();

			Refs.Add(Elem.Action);

			Refs.Add(Elem.MessageID);

			//Refs.Add(Elem.From);
			Refs.Add(Elem.ReplyTo);
			
			Refs.Add(Elem.To);

			//Refs.Add(Elem.Created);
			//Refs.Add(Elem.Expires);
			Refs.Add(Elem.Timestamp);

			Refs.Add(Elem.Body);
		}

		public XmlElement securityContextToken;
		public byte [] securityContextKey;
		public BinarySecurityToken BinSecTok;
		public UsernameToken UserTok;
		public string ClearPassword;

		public ArrayList Refs;

		public AsymmetricAlgorithm AsymmAlg; //for DSA or RSA signing

		public DerivedKeyToken derKeyTok;
	}
}
