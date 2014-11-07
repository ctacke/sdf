//==========================================================================================
//
//		OpenNETCF.Windows.Forms.PasswordDeriveBytes
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
//		author: casey chesnut
//		http://www.brains-N-brawn.com
//
//==========================================================================================

using System;

using OpenNETCF.Security.Cryptography;

namespace OpenNETCF.Web.Services2.Security.Cryptography
{
	public class PSHA1
	{
		//TODO
		//public PSHA1(byte[] secret, byte[] labelSeed);
		//public PSHA1(byte[] secret, string label, byte[] seed);
		//private byte GetByte();
		//public byte[] GetKeyBytes(int position, int size);

		public PSHA1(){}

		//http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnglobspec/html/ws-trust.asp
		//http://www.dotnet247.com/247reference/msgs/33/165330.aspx
		//http://www.faqs.org/rfcs/rfc2246.html
		//HMAC_SHA(secret, data)
		//P_SHA1 (password, label + nonce + timestamp)

		/*
		P_hash(secret, seed) = HMAC_hash(secret, A(1) + seed) +
		HMAC_hash(secret, A(2) + seed) +
		HMAC_hash(secret, A(3) + seed) + ...

		Where + indicates concatenation.

		A() is defined as:
		A(0) = seed
		A(i) = HMAC_hash(secret, A(i-1))
		*/

		public static byte [] DeriveKey (byte [] reqEntropy, byte [] resEntropy, int size)
		{
			byte [] baNonce = new byte[0]; //not passing a nonce
			byte [] baTimestamp = new byte[0]; //not passing a timestamp
			return DeriveKey(reqEntropy, resEntropy, baNonce, baTimestamp, size);
		}
		
		public static byte [] DeriveKey (byte [] secret, string label, byte [] seed, int size)
		{
			byte [] baLabel = OpenNETCF.Security.Cryptography.NativeMethods.Format.GetBytes(label);
			byte [] baTimestamp = new byte[0]; //not passing a timestamp
			return DeriveKey(secret, baLabel, seed, baTimestamp, size);
		}

		public static byte [] DeriveKey(string password, string label, string nonce, string timestamp, int size)
		{	
			byte [] baPassword = OpenNETCF.Security.Cryptography.NativeMethods.Format.GetBytes(password);
			byte [] baLabel = OpenNETCF.Security.Cryptography.NativeMethods.Format.GetBytes(label);
			byte [] baNonce = new byte[0];
			if(nonce != null && nonce != String.Empty)
				baNonce = OpenNETCF.Security.Cryptography.NativeMethods.Format.GetB64(nonce);
			byte [] baTimestamp = OpenNETCF.Security.Cryptography.NativeMethods.Format.GetBytes(timestamp);

			return DeriveKey(baPassword, baLabel, baNonce, baTimestamp, size);
		}

		//or port from openssl.org (tls1_P_hash)
		public static byte [] DeriveKey(byte [] baPassword, byte [] baLabel, byte [] baNonce, byte [] baTimestamp, int size)
		{
			byte [] secret = baPassword;
			byte [] data = ConcatBa(baLabel, baNonce);
			data = ConcatBa(data, baTimestamp);

			HMACSHA1 hs = new HMACSHA1(secret);
			byte [] seed = (byte[]) data.Clone(); //A0
			byte [] A1 = hs.ComputeHash(seed); 
			byte [] A2 = hs.ComputeHash(A1); 
			byte [] A3 = hs.ComputeHash(A2); 
			byte [] A4 = hs.ComputeHash(A3); 

			byte [] hash1 = hs.ComputeHash(ConcatBa(A1, seed));
			byte [] hash2 = hs.ComputeHash(ConcatBa(A2, seed));
			byte [] hash3 = hs.ComputeHash(ConcatBa(A3, seed));
			byte [] hash4 = hs.ComputeHash(ConcatBa(A4, seed));

			byte [] baOut = new byte[size];
			byte [] baTemp = ConcatBa(hash1, hash2);
			baTemp = ConcatBa(baTemp, hash3);
			baTemp = ConcatBa(baTemp, hash4);

			Array.Copy(baTemp, 0, baOut, 0, size);
			return baOut;
		}

		public static byte [] ConcatBa(byte [] one, byte [] two)
		{
			byte [] ba = new byte[one.Length + two.Length];
			Array.Copy(one, 0, ba, 0, one.Length);
			Array.Copy(two, 0, ba, one.Length, two.Length);
			return ba;
		}
	}
}
