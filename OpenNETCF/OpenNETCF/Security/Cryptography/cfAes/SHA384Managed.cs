// casey chesnut (casey@brains-N-brawn.com)
// (C) 2003, 2004 brains-N-brawn LLC
using System;

namespace OpenNETCF.Security.Cryptography
{
	public sealed class SHA384Managed : SHA384
	{
		public SHA384Managed()
		{

		}

		private byte [] hash = null;
		public override byte[] Hash 
		{ 
			get{return hash;}
		}

		public override int HashSize 
		{ 
			get{return 384;}
		}

		public override byte [] ComputeHash(byte [] buffer)
		{
			hash = NetSHA.SHA384.MessageSHA384(buffer);
			return hash;
		}
	}
}