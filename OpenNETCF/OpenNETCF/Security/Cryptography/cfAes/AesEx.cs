// casey chesnut (casey@brains-N-brawn.com)
// (C) 2003, 2004 brains-N-brawn LLC

using System;

using OpenNETCF.Security.Cryptography;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
	public class AesKeyWrap : SymmetricAlgorithm
	{
		private byte [] kwIv = new byte [] {166, 166, 166, 166, 166, 166, 166, 166}; //WTF?
		private Rijndael _tdcsp;
		public AesKeyWrap(Rijndael tdcsp)
		{
			_tdcsp = tdcsp;
			//_tdcsp.IV = new byte[16];
		}

		public override byte[] IV 
		{ 
			get{return (byte[])_tdcsp.IV.Clone();}
			set{_tdcsp.IV = (byte[])value.Clone();} 
		}

		public override byte[] Key 
		{ 
			get{return (byte[])_tdcsp.Key.Clone();} 
			set{_tdcsp.Key = (byte[])value.Clone();} 
		}

		public static byte [] ConcatBa(byte [] one, byte [] two)
		{
			byte [] ba = new byte[one.Length + two.Length];
			Array.Copy(one, 0, ba, 0, one.Length);
			Array.Copy(two, 0, ba, one.Length, two.Length);
			return ba;
		}

		public override byte [] EncryptValue(byte [] key)
		{
			int N = key.Length / 8;
			int rem = key.Length % 8;
			if(rem != 0)
				throw new Exception("key must be in blocks of 8");

			AesNoPadding anp = new AesNoPadding(_tdcsp);
			if(N == 1) //If N is 1
			{
				//B=AES(K)enc(0xA6A6A6A6A6A6A6A6|P(1))
				byte [] keyIv_key = ConcatBa(kwIv, key);
				byte [] B = anp.Encrypt(keyIv_key); 
				return B;
			}
			//For i=1 to N, R(i)=P(i) 
			byte [] retBa = new byte[(N+1) * 8];
			Buffer.BlockCopy(key, 0, retBa, 8, key.Length);
			byte [] A = (byte []) kwIv.Clone(); //Set A to 0xA6A6A6A6A6A6A6A6 
			byte [] R = new byte[8];
			for(int j=0; j<=5; j++) //Forj=0 to 5, 
			{
				for(int i=1; i<=N; i++) //For i=1 to N,
				{
					long t = (long) i + (j * N); //t= i + j*N
					Buffer.BlockCopy(retBa, i*8, R, 0, 8);
					byte [] A_R = ConcatBa(A, R);
					byte [] B = anp.Encrypt(A_R); //B=AES(K)enc(A|R(i))
					for(int k=0; k<8; k++)
					{
						int xt = ((byte) ((t >> ((8 * (7 - k)) & 63)) & ((long) 255))); //WTF?
						A[k] = (byte)(xt ^ B[k]); //A=XOR(t,MSB(B))
					}
					Buffer.BlockCopy(B, 8, retBa, i*8, 8); //R(i)=LSB(B) 
				}
			}
			Buffer.BlockCopy(A, 0, retBa, 0, 8); //Set C(0)=A 
			return retBa; //For i=1 to N, C(i)=R(i) 
		}

		public override byte [] DecryptValue(byte [] keyWrap)
		{
			int N = keyWrap.Length / 8 - 1;
			int rem = keyWrap.Length % 8;
			if(rem != 0)
				throw new Exception("key must be in blocks of 8");

			byte [] key = new byte[N * 8];
			AesNoPadding anp = new AesNoPadding(_tdcsp);
			if(N == 1) //If N is 1: 
			{
				byte [] B = anp.Decrypt(keyWrap);
				for(int msb=0; msb<8; msb++) //If MSB(B) is 0xA6A6A6A6A6A6A6A6
				{
					if (B[msb] != kwIv[msb])
						throw new Exception("unwrapped key is bad size1");
				}
				Buffer.BlockCopy(B, 8, key, 0, 8); //P(1)=LSB(B) 
				return key;
			}
			Buffer.BlockCopy(keyWrap, 8, key, 0, key.Length); //For i=1 to N, R(i)=C(i) 
			byte [] A = new byte[8];
			byte [] R = new byte[8];
			Buffer.BlockCopy(keyWrap, 0, A, 0, 8); //A=C(0) 
			for(int j=5; j>=0; j--) //For j=5 to 0, 
			{
				for(int i=N; i>=1; i--) //For i=N to 1,
				{
					long t = (long) i + (j * N); //t= i + j*N
					for(int k=0; k<8; k++)
					{
						byte x = ((byte) ((t >> ((8 * (7 - k)) & 63)) & ((long) 255))); //WTF?
						A[k] = (byte) (A[k] ^ x);
					}
					//B=AES(K)dec(XOR(t,A)|R(i))
					Buffer.BlockCopy(key, (i-1)*8, R, 0, 8);
					byte [] A_R = ConcatBa(A, R);
					byte [] B = anp.Decrypt(A_R);
					Buffer.BlockCopy(B, 0, A, 0, 8); //A=MSB(B)
					Buffer.BlockCopy(B, 8, key, (i-1)*8, 8); //R(i)=LSB(B) 
				}
			}
			for(int a=0; a<8; a++)
			{
				if(A[a] != kwIv[a]) //If A is 0xA6A6A6A6A6A6A6A6
					throw new Exception("unwrapped key is bad size2");
			}
			return key; //For i=1 to N, P(i)=R(i) 
		}
	}

	public class AesNoPadding
	{
		private Rijndael _tdcsp;
		public AesNoPadding(Rijndael tdcsp)
		{
			_tdcsp = tdcsp;
		}

		public byte [] Encrypt(byte [] plain)
		{
			int div = plain.Length / _tdcsp.Key.Length;
			int rem = plain.Length % 16;
			if(rem != 0)
				throw new Exception("must be in 16 byte blocks");

			byte [] cipher = _tdcsp.EncryptValue(plain);
			//just encrypt and throw away the last block
			byte [] cipherNoPad = new byte[cipher.Length - _tdcsp.IV.Length];
			Array.Copy(cipher, 0, cipherNoPad, 0, cipherNoPad.Length);
			return cipherNoPad;
		}

		public byte [] Decrypt(byte [] cipher)
		{
			//int iters = cipher.Length / 8;
			int blockSize = _tdcsp.IV.Length;
			int rem = cipher.Length % 16;
			if(rem != 0)
				throw new Exception("must be in 16 byte blocks");

			byte [] lastBlock = new byte[blockSize];
			Array.Copy(cipher, cipher.Length - blockSize, lastBlock, 0, blockSize);

			//reuse passed in Rijndael
			byte [] origKey = (byte[])_tdcsp.Key.Clone();
			byte [] origIv = (byte[])_tdcsp.IV.Clone();
			//_tdcsp.Key = (byte[]) _tdcsp.Key.Clone();
			_tdcsp.IV = lastBlock;
			
			//tempTdcsp.Mode = CipherMode.ECB;
			//TODO make this support other lengths 0x01 - 0x07070707070707
			//byte [] pkcs5 = new byte [] {0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08};
			
			byte [] pkcs5 = new byte [] {0x10,0x10,0x10,0x10,0x10,0x10,0x10,0x10,0x10,0x10,0x10,0x10,0x10,0x10,0x10,0x10};
			byte [] padPlus = _tdcsp.EncryptValue(pkcs5);
			byte [] paddedCipher = new byte [cipher.Length + blockSize];
			Array.Copy(cipher, 0, paddedCipher, 0, cipher.Length);
			Array.Copy(padPlus, 0, paddedCipher, cipher.Length, blockSize);

			//no reset passed in Rijndael
			_tdcsp.Key = origKey;
			_tdcsp.IV = origIv;
			byte [] plain = _tdcsp.DecryptValue(paddedCipher);
			return plain;
		}
	}
}
