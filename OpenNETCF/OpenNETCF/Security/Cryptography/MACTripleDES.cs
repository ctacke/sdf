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
using OpenNETCF.Security.Cryptography.Internal;

namespace OpenNETCF.Security.Cryptography
{
	public class MACTripleDES : KeyedHashAlgorithm
	{
		public MACTripleDES()
		{
			IntPtr prov = Context.AcquireContext();
			IntPtr ipKey = OpenNETCF.Security.Cryptography.Internal.Key.GenKey(prov, Calg.TRIP_DES, GenKeyParam.EXPORTABLE);
            key = OpenNETCF.Security.Cryptography.Internal.Key.ExportSessionKey(prov, ipKey, 24, true);
			//reversed above
            OpenNETCF.Security.Cryptography.Internal.Key.DestroyKey(ipKey);
            OpenNETCF.Security.Cryptography.Internal.Context.ReleaseContext(prov);
		}

		//MACTripleDES uses a key of length 8, 16 or 24 bytes
		public MACTripleDES(byte [] desKey)
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
            IntPtr prov = OpenNETCF.Security.Cryptography.Internal.Context.AcquireContext();
			byte [] baKey = (byte []) key.Clone();
			//reversed below
            IntPtr ipKey = OpenNETCF.Security.Cryptography.Internal.Key.ImportSessionKey(prov, Calg.TRIP_DES, baKey, true);
            hash = OpenNETCF.Security.Cryptography.Internal.Hash.ComputeKeyedHash(prov, CalgHash.MAC, buffer, ipKey);
            OpenNETCF.Security.Cryptography.Internal.Key.DestroyKey(ipKey);
            OpenNETCF.Security.Cryptography.Internal.Context.ReleaseContext(prov);
			return hash;
		}
	}
}
