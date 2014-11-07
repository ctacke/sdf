// casey chesnut (casey@brains-N-brawn.com)
// (C) 2003, 2004 brains-N-brawn LLC
using System;

namespace OpenNETCF.Security.Cryptography
{
	public sealed class SHA512Managed : SHA512
	{
		public SHA512Managed()
		{

		}

		private byte [] hash = null;
		public override byte[] Hash 
		{ 
			get{return hash;}
		}

		public override int HashSize 
		{ 
			get{return 512;}
		}

		public override byte [] ComputeHash(byte [] buffer)
		{
			hash = NetSHA.SHA512.MessageSHA512(buffer);
			return hash;
		}
	}
}