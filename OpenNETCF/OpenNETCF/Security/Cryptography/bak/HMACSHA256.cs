using System;

namespace OpenNETCF.Security.Cryptography
{
	public class HMACSHA256 : HMAC
	{
		public HMACSHA256() : base(new SHA256Managed())
		{
		}

		public HMACSHA256(byte [] sessKey) : base(new SHA256Managed())
		{
			base.Key = sessKey;
		}

		public override int HashSize
		{
			get
			{
				return 256;
			}
		}
	}
}
