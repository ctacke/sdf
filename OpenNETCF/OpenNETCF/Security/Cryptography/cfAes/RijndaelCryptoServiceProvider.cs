// casey chesnut (casey@brains-N-brawn.com)
// (C) 2003, 2004 brains-N-brawn LLC
using System;

using aes;
using sc;

namespace OpenNETCF.Security.Cryptography
{
	public class RijndaelCryptoServiceProvider : Rijndael
	{
		public RijndaelCryptoServiceProvider()
		{
			iv = OpenNETCF.Security.Cryptography.NativeMethods.Rand.GetRandomBytes(16); //i think this is always 16 bytes
			key = OpenNETCF.Security.Cryptography.NativeMethods.Rand.GetRandomBytes(16); //Aes128 by default
		}

		private byte [] iv;
		public override byte[] IV 
		{ 
			get{return iv;}
			set{iv = value;} 
		}

		private byte [] key;
		public override byte[] Key 
		{ 
			get{return (byte[])key.Clone();} 
			set{key = (byte[])value.Clone();} 
		}

		private CipherMode mode = CipherMode.CBC;
		public CipherMode Mode
		{
			get{return mode;}
			set{mode = value;} 
		}

		//non-standard
		public override byte[] DecryptValue(byte[] cBuff)
		{
			IBlockCipher ibc = AesFactory.GetAes(false); //native
			StreamCtx _aes = null;
			if(mode == CipherMode.CBC)
				_aes = StreamCipher.MakeStreamCtx(ibc, key, iv, StreamCipher.Mode.CBC);
			else
				_aes = StreamCipher.MakeStreamCtx(ibc, key, iv, StreamCipher.Mode.ECB);
			byte[] pBuff = StreamCipher.Encode(_aes, cBuff, StreamCipher.DECRYPT);
			return pBuff;
		}

		//non-standard
		public override byte[] EncryptValue(byte[] pBuff)
		{
			IBlockCipher ibc = AesFactory.GetAes(false); //native
			StreamCtx _aes = null;
			if(mode == CipherMode.CBC)
				_aes = StreamCipher.MakeStreamCtx(ibc, key, iv, StreamCipher.Mode.CBC);
			else
				_aes = StreamCipher.MakeStreamCtx(ibc, key, iv, StreamCipher.Mode.ECB);
			byte[] cBuff = StreamCipher.Encode(_aes, pBuff, StreamCipher.ENCRYPT);
			return cBuff;
		}
	}
}
