//==========================================================================================
//
//		OpenNETCF.Windows.Forms.MACTripleDES
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
using OpenNETCF.Web.Services2.Security.Cryptography;
using OpenNETCF.Security.Cryptography.Internal;

namespace OpenNETCF.Security.Cryptography
{
	public class MACTripleDES2 : KeyedHashAlgorithm
	{
		public MACTripleDES2()
		{
            IntPtr prov = OpenNETCF.Security.Cryptography.Internal.Context.AcquireContext();
            IntPtr ipKey = OpenNETCF.Security.Cryptography.Internal.Key.GenKey(prov, Calg.TRIP_DES, GenKeyParam.EXPORTABLE);
            key = OpenNETCF.Security.Cryptography.Internal.Key.ExportSessionKey(prov, ipKey, 24, true);
			//reversed above
            OpenNETCF.Security.Cryptography.Internal.Key.DestroyKey(ipKey);
            OpenNETCF.Security.Cryptography.Internal.Context.ReleaseContext(prov);
		}

		//MACTripleDES uses a key of length 8, 16 or 24 bytes
		public MACTripleDES2(byte [] desKey)
		{
			if(desKey.Length != 24)
				throw new Exception("only supports 24 byte key lengths");
			key = (byte []) desKey.Clone();
		}

		private byte [] key = null;
		public override byte[] Key 
		{ 
			get{return key;}
			set
			{key = value;}
		}

		private byte [] hash = null;
		public override byte[] Hash 
		{ 
			get{return hash;}
		}

		public override int HashSize 
		{ 
			get{return 64;}
		}

		public override byte [] ComputeHash(byte [] buffer)
		{
			//TODO this isnt working on SP
			//IntPtr prov = OpenNETCF.Security.Cryptography.NativeMethods.Context.AcquireContext();
			//byte [] baKey = (byte []) key.Clone();
			//reversed below
			//IntPtr ipKey = OpenNETCF.Security.Cryptography.NativeMethods.Key.ImportSessionKey(prov, Calg.TRIP_DES, baKey, true);
			//hash = OpenNETCF.Security.Cryptography.NativeMethods.Hash.ComputeKeyedHash(prov, CalgHash.MAC, buffer, ipKey);
			//OpenNETCF.Security.Cryptography.NativeMethods.Key.DestroyKey(ipKey);
			//OpenNETCF.Security.Cryptography.NativeMethods.Context.ReleaseContext(prov);
			//return hash;

			//http://www.itl.nist.gov/fipspubs/fip81.htm - Appendix F
			int blocks = buffer.Length / 8;
			int rem = buffer.Length % 8;
			if(rem != 0)
				blocks = blocks + 1;
			byte [] padBuffer = new byte[blocks * 8];
			Array.Copy(buffer, 0, padBuffer, 0, buffer.Length);
			//this leaves 0's at the end

			TripleDESCryptoServiceProvider tdcsp = new TripleDESCryptoServiceProvider();
			tdcsp.Key = this.key;
			tdcsp.IV = new byte[8];
			TripleDesNoPadding tdnp = new TripleDesNoPadding(tdcsp);
			byte [] cipher = tdnp.Encrypt(padBuffer);

			//#if NET_1_0 - from Mono
			// add an empty (zeros) block for MAC padding
			//byte[] emptyBlock = new byte [blockSize];
			//result = enc.TransformFinalBlock (emptyBlock, 0, blockSize);

			hash = new byte[8];
			Array.Copy(cipher, cipher.Length - 8, hash, 0, 8);
			return hash;
		}
	}
}
