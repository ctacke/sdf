using System;
using System.Xml;
using System.IO;
using System.Text;

namespace OpenNETCF.Security.Cryptography.Internal
{
	internal class DiffHell
	{
		public DiffHell(byte [] rawKey)
		{
			this.rawKey = rawKey; //400 //144

			pks = new PUBLICKEYSTRUC();
			pks.bType = rawKey[0]; //7 //6
			pks.bVersion = rawKey[1]; //2
			pks.reserved = BitConverter.ToUInt16(rawKey, 2); //0
			pks.aiKeyAlg = BitConverter.ToUInt32(rawKey, 4); //43522

			kb = (KeyBlob) pks.bType; //private //public
			c = (Calg) pks.aiKeyAlg; //DH_EPHEM or DH_SF
			
			if(kb != KeyBlob.PUBLICKEYBLOB && kb != KeyBlob.PRIVATEKEYBLOB)
				throw new Exception("unsupported blob type");

			dhpk = new DHPUBKEY();
			//PRIV 0x32484400. This hex value is the ASCII encoding of "DH2."
			//PUB 0x31484400. This hex value is the ASCII encoding of "DH1."
			dhpk.magic = BitConverter.ToUInt32(rawKey, 8); //843596800 //826819584
			//Number of bits in the prime modulus, P.  
			dhpk.bitlen = BitConverter.ToUInt32(rawKey, 12); //1024
			uint byteLen = dhpk.bitlen / 8; //128

			this.SetSizeAndPosition(dhpk.bitlen);

			bool revBytes = true;
			if(kb == KeyBlob.PRIVATEKEYBLOB)
			{
				P = Format.GetBytes(this.rawKey, ypPos, ypLen, revBytes); 
				G = Format.GetBytes(this.rawKey, gPos, gLen, revBytes); 
				X = Format.GetBytes(this.rawKey, xPos, xLen, revBytes);
			}
			if(kb == KeyBlob.PUBLICKEYBLOB)
			{
				Y = Format.GetBytes(this.rawKey, ypPos, ypLen, revBytes);
			}
		}

		public byte [] rawKey;
		public PUBLICKEYSTRUC pks;
		public DHPUBKEY dhpk;
		public KeyBlob kb;
		public Calg c;

		//public
		public byte [] Y; //// Where y = (G^X) mod P
		//private
		public byte [] P; //prime
		public byte [] G; //generator
		public byte [] X; //secret

		private uint pksLen;
		private uint dhpkLen;
		private uint ypLen;
		private uint gLen;
		private uint xLen;

		//private uint pksPos;
		private uint dhpkPos;
		private uint ypPos;
		private uint gPos;
		private uint xPos;

		private uint privByteLen;
		private uint pubByteLen;

		private void SetSizeAndPosition(uint bitLen)
		{
			//size = 8 8 
			pksLen = 8;
			dhpkLen = 8;
			ypLen = bitLen / 8; //128 (both y and p)
			gLen = bitLen / 8; //128
			xLen = bitLen / 8; //128

			privByteLen = pksLen + dhpkLen + ypLen + gLen + xLen; //400
			pubByteLen = pksLen + dhpkLen + ypLen; //144

			//1024 =  8 16 
			//pksPos = 0;
			dhpkPos = pksLen; //8
			ypPos = dhpkPos + dhpkLen; //16
			gPos = ypPos + ypLen; //144
			xPos = gPos + gLen; //272
		}
	}
}
