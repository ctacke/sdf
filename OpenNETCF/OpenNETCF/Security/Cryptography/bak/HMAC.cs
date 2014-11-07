using System;
using System.Security.Cryptography;
using OpenNETCF.Security.Cryptography.Internal;

namespace OpenNETCF.Security.Cryptography
{
	public abstract class HMAC : KeyedHashAlgorithm
	{
		public HMAC() : this(new SHA1CryptoServiceProvider())
		{
		}

        private System.Security.Cryptography.HashAlgorithm mHashAlg;
        public HMAC(System.Security.Cryptography.HashAlgorithm ha)
		{
			mHashAlg = ha;
			key = Rand.GetRandomBytes(64);
		}

		private byte [] rgbInner;
		private byte [] rgbOuter;
		//MONO
		private byte[] KeySetup (byte[] key, byte padding) 
		{
			byte[] buf = new byte [64];
			for (int i = 0; i < key.Length; ++i)
				buf [i] = (byte) ((byte) key [i] ^ padding);
			for (int i = key.Length; i < 64; ++i)
				buf [i] = padding;
			return buf;
		}

		public HMAC(byte [] sessKey)
		{
			//if(sessKey.Length != 16)
			//	throw new Exception("only supports 16 byte RC2 key lengths");
			key = (byte []) sessKey.Clone();
		}

		private byte [] key = null;
		public override byte[] Key 
		{ 
			get{return key;}
			set{key = value;}
		}

		private byte [] hash = null;
		public override byte[] Hash 
		{ 
			get{return hash;}
		}

		public override int HashSize 
		{ 
			get{return mHashAlg.HashSize;}
		}

		public override byte [] ComputeHash(byte [] buffer)
		{
			byte [] tempBa = (byte []) buffer.Clone();
			rgbInner = KeySetup(key, 0x36);
			rgbOuter = KeySetup(key, 0x5C);

			//SHA1CryptoServiceProvider scap = new SHA1CryptoServiceProvider();
			byte [] in1 = PSHA1.ConcatBa(rgbInner, tempBa);
			hash = mHashAlg.ComputeHash(in1);

			byte [] in2 = PSHA1.ConcatBa(rgbOuter, hash);
			hash = mHashAlg.ComputeHash(in2);

			return hash;
		}
	}
}
