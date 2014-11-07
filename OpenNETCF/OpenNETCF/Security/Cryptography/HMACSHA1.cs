//==========================================================================================
//
//		OpenNETCF.Windows.Forms.HMACSHA1
//		Copyright (c) 2003, OpenNETCF.org
//
//		This library is free software; you can redistribute it and/or modify it under 
//		the terms of the OpenNETCF.org Shared Source License.
//
//		This library is distributed in the hope that it will be useful, but 
//		WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
//		FITNESS FOR A PARTICULAR PURPOSE. See the OpenNETCF.org Shared Source License 
//		for more details.
//
//		You should have received a copy of the OpenNETCF.org Shared Source License 
//		along with this library; if not, email licensing@opennetcf.org to request a copy.
//
//		If you wish to contact the OpenNETCF Advisory Board to discuss licensing, please 
//		email licensing@opennetcf.org.
//
//		For general enquiries, email enquiries@opennetcf.org or visit our website at:
//		http://www.opennetcf.org
//
//		!!! A HUGE thank-you goes out to Casey Chesnut for supplying this class library !!!
//      !!! You can contact Casey at http://www.brains-n-brawn.com                      !!!
//
//==========================================================================================
using System;
using OpenNETCF.Security.Cryptography.Internal;

namespace OpenNETCF.Security.Cryptography
{
	public class HMACSHA1 : KeyedHashAlgorithm
	{
		public HMACSHA1()
		{
            key = OpenNETCF.Security.Cryptography.Internal.Rand.GetRandomBytes(64);
			/*
			//1st cut was wrong
			IntPtr prov = OpenNETCF.Security.Cryptography.Context.AcquireContext();
			//3DES is WSE default?
			IntPtr ipKey = OpenNETCF.Security.Cryptography.Key.GenKey(prov, Calg.TRIP_DES, GenKeyParam.EXPORTABLE);
			key = OpenNETCF.Security.Cryptography.Key.ExportSessionKey(prov, ipKey, 24, true);
			//reversed above
			OpenNETCF.Security.Cryptography.Key.DestroyKey(ipKey);
			OpenNETCF.Security.Cryptography.Context.ReleaseContext(prov);
			*/
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

		public HMACSHA1(byte [] sessKey)
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
			get{return 160;}
		}

		//http://groups.google.com/groups?q=calg_hmac&hl=en&lr=&ie=UTF-8&oe=UTF-8&selm=8bWw7.2452%241q2.225894%40news2-win.server.ntlworld.com&rnum=1
		public override byte [] ComputeHash(byte [] buffer)
		{
			byte [] tempBa = (byte []) buffer.Clone();
			rgbInner = KeySetup(key, 0x36);
			rgbOuter = KeySetup(key, 0x5C);

            IntPtr prov = OpenNETCF.Security.Cryptography.Internal.Context.AcquireContext();

            IntPtr hash1 = OpenNETCF.Security.Cryptography.Internal.Hash.CreateHash(prov, CalgHash.SHA1);
            OpenNETCF.Security.Cryptography.Internal.Hash.HashData(hash1, rgbInner);
            OpenNETCF.Security.Cryptography.Internal.Hash.HashData(hash1, tempBa);
            hash = OpenNETCF.Security.Cryptography.Internal.Hash.GetHashParam(hash1);
            OpenNETCF.Security.Cryptography.Internal.Hash.DestroyHash(hash1);

            IntPtr hash2 = OpenNETCF.Security.Cryptography.Internal.Hash.CreateHash(prov, CalgHash.SHA1);
            OpenNETCF.Security.Cryptography.Internal.Hash.HashData(hash2, rgbOuter);
            OpenNETCF.Security.Cryptography.Internal.Hash.HashData(hash2, hash);
            hash = OpenNETCF.Security.Cryptography.Internal.Hash.GetHashParam(hash2);
            OpenNETCF.Security.Cryptography.Internal.Hash.DestroyHash(hash2);

            OpenNETCF.Security.Cryptography.Internal.Context.ReleaseContext(prov);
			return hash;

			/*
			//1st cut was wrong
			IntPtr prov = OpenNETCF.Security.Cryptography.Context.AcquireContext();
			byte [] baKey = (byte []) key.Clone();
			//reversed below
			IntPtr ipKey = IntPtr.Zero;
			if(baKey.Length == 8)
				ipKey = OpenNETCF.Security.Cryptography.Key.ImportSessionKey(prov, Calg.DES, baKey, true);
			if(baKey.Length == 16)
				ipKey = OpenNETCF.Security.Cryptography.Key.ImportSessionKey(prov, Calg.RC2, baKey, true);
			if(baKey.Length == 24)
				ipKey = OpenNETCF.Security.Cryptography.Key.ImportSessionKey(prov, Calg.TRIP_DES, baKey, true);
			
			IntPtr hmacHash = OpenNETCF.Security.Cryptography.Hash.CreateHash(prov, CalgHash.HMAC, ipKey);
			byte [] baHmacInfo = new byte[20]; //create new HMAC_Info byte[]
			byte [] algId = BitConverter.GetBytes((uint)CalgHash.SHA1); 
			Buffer.BlockCopy(algId, 0, baHmacInfo, 0, 4); //set HashAlgid
			OpenNETCF.Security.Cryptography.Hash.SetHashParam(hmacHash, HashParam.HMAC_INFO, baHmacInfo);
			OpenNETCF.Security.Cryptography.Hash.HashData(hmacHash, buffer);
			hash = OpenNETCF.Security.Cryptography.Hash.GetHashParam(hmacHash);
			OpenNETCF.Security.Cryptography.Hash.DestroyHash(hmacHash);
			
			OpenNETCF.Security.Cryptography.Key.DestroyKey(ipKey);
			OpenNETCF.Security.Cryptography.Context.ReleaseContext(prov);
			return hash;
			*/
		}
	}
}
