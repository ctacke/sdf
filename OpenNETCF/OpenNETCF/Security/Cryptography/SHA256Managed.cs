// casey chesnut (casey@brains-N-brawn.com)
// (C) 2003, 2004 brains-N-brawn LLC
using System;

namespace OpenNETCF.Security.Cryptography
{
	public sealed class SHA256Managed : SHA256
	{
		public SHA256Managed()
		{

		}

		private byte [] hash = null;
		public override byte[] Hash 
		{ 
			get{return hash;}
		}

		public override int HashSize 
		{ 
			get{return 256;}
		}

		public override byte [] ComputeHash(byte [] buffer)
		{
			hash = NetSHA.SHA256.MessageSHA256(buffer);
			return hash;
		}
	}
}