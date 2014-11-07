// casey chesnut (casey@brains-N-brawn.com)
// (C) 2003, 2004 brains-N-brawn LLC
using System;

using Org.Mentalis.Security.Cryptography;

namespace OpenNETCF.Security.Cryptography
{
	public sealed class RIPEMD160Managed : RIPEMD160
	{
		public RIPEMD160Managed()
		{

		}

		private byte [] hash = null;
		public override byte[] Hash 
		{ 
			get{return hash;}
		}

		public override int HashSize 
		{ 
			get{return 160;}
		}

		public override byte [] ComputeHash(byte [] buffer)
		{
			Org.Mentalis.Security.Cryptography.RIPEMD160Managed r160 = new Org.Mentalis.Security.Cryptography.RIPEMD160Managed();
			hash = r160.ComputeHash(buffer);
			return hash;
		}
	}
}