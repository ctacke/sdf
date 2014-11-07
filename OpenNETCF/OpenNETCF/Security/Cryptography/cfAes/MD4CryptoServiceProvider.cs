// casey chesnut (casey@brains-N-brawn.com)
// (C) 2003, 2004 brains-N-brawn LLC
using System;
using OpenNETCF.Security.Cryptography.NativeMethods;

namespace OpenNETCF.Security.Cryptography
{
	public class MD4CryptoServiceProvider : MD4
	{
		public MD4CryptoServiceProvider()
		{

		}

		private byte [] hash = null;
		public override byte[] Hash 
		{ 
			get{return hash;}
		}

		public override int HashSize 
		{ 
			get{return 128;}
		}

		public override byte [] ComputeHash(byte [] buffer)
		{
			//TODO test for interop
			hash = OpenNETCF.Security.Cryptography.NativeMethods.Hash.ComputeHash(CalgHash.MD4, buffer);
			if(hash.Length  != 16)
				throw new Exception("Md4 hash value is not 128 bits");
			return hash;
		}
	}
}
