using System;

namespace OpenNETCF.Security.Cryptography
{
    public abstract class HashAlgorithm
    {
        protected HashAlgorithm() { }

        public abstract int HashSize { get; }
        public abstract byte[] Hash { get; }

        public abstract byte[] ComputeHash(byte[] buffer);
    }

    public abstract class SHA256 : HashAlgorithm
    {
        public SHA256()
        {

        }
    }
    
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