using System;
using System.Security.Cryptography;

namespace OpenNETCF.Web.Services2
{
	public class TripleDesKeyWrap : SymmetricAlgorithm
	{
		private byte [] kwIv = new byte [] {0x4a, 0xdd, 0xa2, 0x2c, 0x79, 0xe8, 0x21, 0x05}; //WTF?
		private TripleDESCryptoServiceProvider _tdcsp;

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void GenerateKey()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void GenerateIV()
        {
            throw new Exception("The method or operation is not implemented.");
        }

		public TripleDesKeyWrap(TripleDESCryptoServiceProvider tdcsp)
		{
			_tdcsp = tdcsp;
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

        
		public byte [] EncryptValue(byte [] key)
		{
			//http://www.w3.org/TR/2002/REC-xmlenc-core-20021210/Overview.html
			byte[] bs1 = _tdcsp.Key;
			SHA1CryptoServiceProvider sha1Csp = new SHA1CryptoServiceProvider();
			byte[] bs2 = sha1Csp.ComputeHash(key);
			RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
			byte[] bs3 = new byte[8];
			rNGCryptoServiceProvider.GetBytes(bs3);
			byte[] bs4 = new byte[(int)key.Length + 8];
			_tdcsp.IV = bs3;
			Buffer.BlockCopy(key, 0, bs4, 0, (int)key.Length);
			Buffer.BlockCopy(bs2, 0, bs4, (int)key.Length, 8);				
			TripleDesNoPadding tdnp1 = new TripleDesNoPadding(_tdcsp);
			byte [] bs5 = tdnp1.Encrypt(bs4);
			byte[] bs6 = new byte[(int)bs3.Length + (int)bs5.Length];
			Buffer.BlockCopy(bs3, 0, bs6, 0, (int)bs3.Length);
			Buffer.BlockCopy(bs5, 0, bs6, (int)bs3.Length, (int)bs5.Length);
			Array.Reverse(bs6, 0, bs6.Length);
			_tdcsp.Key = bs1;
			_tdcsp.IV = kwIv;
			TripleDesNoPadding tdnp2 = new TripleDesNoPadding(_tdcsp);
			byte [] cipher = tdnp2.Encrypt(bs6);
			return cipher;
		}

		public byte [] DecryptValue(byte [] keyWrap)
		{
			//1. Check if the length of the cipher text is reasonable given the key type. 
			//It must be 40 bytes for a 168 bit key and either 32, 40, or 48 bytes for a 128, 192, or 256 bit key. 
			//If the length is not supported or inconsistent with the algorithm for which the key is intended, return error. 
			if(keyWrap.Length < 40)
			{
				throw new Exception("kw-tripledes must be 40 bytes for TripleDES key");
			}

			//2. Decrypt the cipher text with TRIPLEDES in CBC mode using the KEK and an initialization vector (IV) of 0x4adda22c79e82105. Call the output TEMP3. 
			_tdcsp.IV = kwIv;
			TripleDesNoPadding tdnp = new TripleDesNoPadding(_tdcsp);
			byte [] temp3 = tdnp.Decrypt(keyWrap);

			//3. Reverse the order of the octets in TEMP3 and call the result TEMP2. 
			byte [] temp2 = (byte []) temp3.Clone();
			Array.Reverse(temp2, 0, temp2.Length);
			//4. Decompose TEMP2 into IV, the first 8 octets, and TEMP1, the remaining octets. 
			byte [] temp1iv = new byte[_tdcsp.IV.Length];
			Array.Copy(temp2, 0, temp1iv, 0, temp1iv.Length);
			byte [] temp1 = new byte [temp2.Length - temp1iv.Length];
			Array.Copy(temp2, temp1iv.Length, temp1, 0, temp1.Length);

			//5. Decrypt TEMP1 using TRIPLEDES in CBC mode using the KEK and the IV found in the previous step. Call the result WKCKS.
			_tdcsp.IV = temp1iv;
			byte [] WKCKS = tdnp.Decrypt(temp1);

			//6. Decompose WKCKS. CKS is the last 8 octets and WK, the wrapped key, are those octets before the CKS. 
			byte [] CKS = new byte[8];
			byte [] WK = new byte[WKCKS.Length - 8];
			Array.Copy(WKCKS, WKCKS.Length - 8, CKS, 0, 8);
			Array.Copy(WKCKS, 0, WK, 0, WKCKS.Length - 8);

			//7. Calculate a CMS key checksum (section 5.6.1) over the WK and compare with the CKS extracted in the above step. If they are not equal, return error. 
			SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
			byte [] CMS = sha1.ComputeHash(WK);

			//check hash
			for(int i=0; i<CKS.Length; i++)
			{
				if(CKS[i] != CMS[i])
					throw new Exception("KeyWrap CheckSum failed");
			}
								
			//8. WK is the wrapped key, now extracted for use in data decryption. 
			return WK;
		}
	}

	public class TripleDesNoPadding
	{
		private TripleDESCryptoServiceProvider _tdcsp;
		public TripleDesNoPadding(TripleDESCryptoServiceProvider tdcsp)
		{
			_tdcsp = tdcsp;
		}

		public byte [] Encrypt(byte [] plain)
		{
			//int iters = plain.Length / 8;
			int rem = plain.Length % 8;
			if(rem != 0)
				throw new Exception("must be in 8 byte blocks");

			//just encrypt and throw away the last 8 bytes

            // note: this is changed from SDF 1.x to compile under CF 2.0 with its
            // TripleDESCryptoServiceProvider.  This may not work!
            byte[] cipher = new byte[plain.Length];
            ICryptoTransform i = _tdcsp.CreateEncryptor();
            i.TransformBlock(plain, 0, plain.Length, cipher, 0);            
			byte [] cipherNoPad = new byte[cipher.Length - _tdcsp.IV.Length];
			Array.Copy(cipher, 0, cipherNoPad, 0, cipherNoPad.Length);

            return cipherNoPad;
		}

		public byte [] Decrypt(byte [] cipher)
		{
			//int iters = cipher.Length / 8;
			int blockSize = 8;
			int rem = cipher.Length % blockSize;
			if(rem != 0)
				throw new Exception("must be in 8 byte blocks");

			byte [] lastBlock = new byte[blockSize];
			Array.Copy(cipher, cipher.Length - blockSize, lastBlock, 0, blockSize);

			TripleDESCryptoServiceProvider tempTdcsp = new TripleDESCryptoServiceProvider();
			tempTdcsp.Key = (byte[]) _tdcsp.Key.Clone();
			tempTdcsp.IV = lastBlock;
			//TODO make this support other lengths 0x01 - 0x07070707070707
			byte [] pkcs5 = new byte [] {0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08};

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, tempTdcsp.CreateDecryptor(tempTdcsp.Key, tempTdcsp.IV), CryptoStreamMode.Write);
            cs.Write(pkcs5, 0, pkcs5.Length);
            cs.Close();
            byte[] padPlus = ms.ToArray();

            byte [] paddedCipher = new byte [cipher.Length + blockSize];
			Array.Copy(cipher, 0, paddedCipher, 0, cipher.Length);
			Array.Copy(padPlus, 0, paddedCipher, cipher.Length, blockSize);

            cs = new CryptoStream(new System.IO.MemoryStream(paddedCipher), _tdcsp.CreateDecryptor(_tdcsp.Key, _tdcsp.IV), CryptoStreamMode.Read);
            byte [] plain = new byte[cs.Length];
            cs.Read(plain, 0, (int)cs.Length);
            cs.Close();

            return plain;
		}
	}
}
