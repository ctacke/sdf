using System;

namespace OpenNETCF.Security.Cryptography
{
	public class HMACSHA384 : HMAC
	{
		public HMACSHA384() : base(new SHA384Managed())
		{
		}

		public HMACSHA384(byte [] sessKey) : base(new SHA384Managed())
		{
			base.Key = sessKey;
		}

		public override int HashSize
		{
			get
			{
				return 384;
			}
		}
	}
}
