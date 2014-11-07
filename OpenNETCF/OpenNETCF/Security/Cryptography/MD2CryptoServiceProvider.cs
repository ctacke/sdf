// casey chesnut (casey@brains-N-brawn.com)
// (C) 2003, 2004 brains-N-brawn LLC
using System;
using OpenNETCF.Security.Cryptography.Internal;

namespace OpenNETCF.Security.Cryptography
{
	public class MD2CryptoServiceProvider : MD2
	{
		public MD2CryptoServiceProvider()
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
			hash = OpenNETCF.Security.Cryptography.Internal.Hash.ComputeHash(CalgHash.MD2, buffer);
			if(hash.Length  != 16)
				throw new Exception("Md2 hash value is not 128 bits");
			return hash;
		}
	}
}
