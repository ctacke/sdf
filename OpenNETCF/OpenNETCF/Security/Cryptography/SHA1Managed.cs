// casey chesnut (casey@brains-N-brawn.com)
// (C) 2003, 2004 brains-N-brawn LLC
using System;

namespace OpenNETCF.Security.Cryptography
{
	public sealed class SHA1Managed : SHA1
	{
		public SHA1Managed()
		{

		}

		private byte [] hash = null;
		public override byte[] Hash 
		{ 
			get{return hash;}
		}

		public override int HashSize 
		{ 
			get{return 1;}
		}

		public override byte [] ComputeHash(byte [] buffer)
		{
			hash = NetSHA.SHA1.MessageSHA1(buffer);
			return hash;
		}
	}
}