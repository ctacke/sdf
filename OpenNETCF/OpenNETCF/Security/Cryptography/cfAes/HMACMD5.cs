using System;

namespace OpenNETCF.Security.Cryptography
{
	public class HMACMD5 : HMAC
	{
		public HMACMD5() : base(new MD5CryptoServiceProvider())
		{
		}

		public HMACMD5(byte [] sessKey) : base(new MD5CryptoServiceProvider())
		{
			base.Key = sessKey;
		}

		public override int HashSize
		{
			get
			{
				return 128;
			}
		}
	}
}
