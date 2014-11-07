using System;

namespace OpenNETCF.Security.Cryptography
{
	public class HMACSHA512 : HMAC
	{
		public HMACSHA512() : base(new SHA512Managed())
		{
		}

		public HMACSHA512(byte [] sessKey) : base(new SHA512Managed())
		{
			base.Key = sessKey;
		}

		public override int HashSize
		{
			get
			{
				return 512;
			}
		}
	}
}
