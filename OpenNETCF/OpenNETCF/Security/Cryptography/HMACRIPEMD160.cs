using System;

namespace OpenNETCF.Security.Cryptography
{
	public class HMACRIPEMD160 : HMAC
	{
		public HMACRIPEMD160() : base(new RIPEMD160Managed())
		{
		}

		public HMACRIPEMD160(byte [] sessKey) : base(new RIPEMD160Managed())
		{
			base.Key = sessKey;
		}

		public override int HashSize
		{
			get
			{
				return 160;
			}
		}
	}
}
