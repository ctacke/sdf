#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



using System;
using OpenNETCF.Security.Cryptography.Internal;

namespace OpenNETCF.Security.Cryptography
{
    /// <summary>
    /// Computes a Hash-based Message Authentication Code (HMAC) using the SHA1 hash function.
    /// </summary>
    /// <devnotes>
    /// Based on comments and commented-out code in the original implemenation of this class
    /// I'm not certain it even functions.  I'm no crypto expert, so I don't know how to test it
    /// - ctacke
    /// </devnotes>
	public class HMACSHA1 : KeyedHashAlgorithm
	{
        /// <summary>
        /// Initializes an implementation of HMACSHA1
        /// </summary>
        public override void Initialize()
        {
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] key = new byte[64];
            rng.GetBytes(key);
            //key = OpenNETCF.Security.Cryptography.Internal.Rand.GetRandomBytes(64);
        }

        /// <summary>
        /// Initializes a new instance of the HMACSHA1 class
        /// </summary>
		public HMACSHA1()
		{
            Initialize();
		}

        private byte[] key = null;
        private byte[] rgbInner;
		private byte [] rgbOuter;

        private byte[] KeySetup (byte[] key, byte padding) 
		{
			byte[] buf = new byte [64];
			for (int i = 0; i < key.Length; ++i)
				buf [i] = (byte) ((byte) key [i] ^ padding);
			for (int i = key.Length; i < 64; ++i)
				buf [i] = padding;
			return buf;
		}

        /// <summary>
        /// Initializes a new instance of the HMACSHA1 class
        /// </summary>
        /// <param name="sessKey"></param>
		public HMACSHA1(byte [] sessKey)
		{
			key = (byte []) sessKey.Clone();
		}

        /// <summary>
        /// Gets or sets the key to be used in the hash algorithm
        /// </summary>
		public override byte[] Key 
		{ 
			get{return key;}
			set{key = value;}
		}

		private byte [] hash = null;
        /// <summary>
        /// Gets the value of the computed hash code
        /// </summary>
		public override byte[] Hash 
		{ 
			get{return hash;}
		}

        /// <summary>
        /// Gets the size of the computed hash code in bits
        /// </summary>
		public override int HashSize 
		{ 
			get{return 160;}
		}

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            //TODO CF 2.0 requires this, though I'm not sure if doing nothing will cause 
            // a problem, since the class worked in SDF 1.0
        }

        protected override byte[] HashFinal()
        {
            //TODO CF 2.0 requires this, though I'm not sure if doing nothing will cause 
            // a problem, since the class worked in SDF 1.0
            return null;
        }

        /// <summary>
        /// Computes the hash value for the input data
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
		public new byte [] ComputeHash(byte [] buffer)
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
		}
	}
}
