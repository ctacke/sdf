using System;
using OpenNETCF.Security.Cryptography.Internal;
using System.Security.Cryptography;

namespace OpenNETCF.Security.Cryptography
{
    /// <summary>
    /// Derives a key from a password
    /// </summary>
	public class PasswordDeriveBytes : DeriveBytes
	{
		private string password;
		private byte [] salt;
		private string hashName;
		int iterations;

        /// <summary>
        /// Initializes a new instance of the PasswordDeriveBytes class
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
		public PasswordDeriveBytes(string password, byte [] salt)
		{
			this.password = password;
			this.salt = salt;
			this.hashName = "SHA1"; //default hash
			this.iterations = 100; //default iterations
		}

        /// <summary>
        /// Initializes a new instance of the PasswordDeriveBytes class
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="hashName"></param>
        /// <param name="iterations"></param>
		public PasswordDeriveBytes(string password, byte[] salt, string hashName, int iterations)
		{
			this.password = password;
			this.salt = salt;
			this.hashName = hashName;
			this.iterations = iterations;
		}

		/// <summary>
        /// Gets or sets the name of the hash algorithm for the operation
		/// </summary>
		public string HashName
		{
			get{return hashName;}
			set{hashName = value;}
		}

        /// <summary>
        /// Gets or sets the number of iterations for the operation.
        /// </summary>
		public int IterationCount
		{
			get{return iterations;}
			set{iterations = value;}
		}

        /// <summary>
        /// Gets or sets the key salt value for the operation.
        /// </summary>
		public byte [] Salt
		{
			get{return salt;}
			set{salt = value;}
		}

		/// <summary>
		/// Derives a cryptographic key from the PasswordDeriveBytes object.
		/// </summary>
		/// <remarks>
		/// If the keySize parameter is set to 0, the default key size for the specified algorithm is used.
		/// </remarks>
		/// <param name="algName">The algorithm name for which to derive the key. </param>
		/// <param name="algHashName">The hash algorithm name to use to derive the key. </param>
		/// <param name="keySize">The size of the key to derive (in bits). </param>
		/// <param name="IV">The initialization vector (IV) to use to derive the key.</param>
		/// <returns>The derived key.</returns>
		public byte [] CryptDeriveKey(string algName, string algHashName, int keySize, byte [] IV)
		{
			//RC2 / SHA1 works
			//TODO DES / MD5 seems to be salted
			//TODO not using salt, IV, or keySize
			IntPtr prov = Context.AcquireContext();
			
			byte [] baPassword = Format.GetBytes(this.password);
			IntPtr hash = GetHashAlgorithm(prov, algHashName);
			Hash.HashData(hash, baPassword);

            IntPtr key = GetKeyAlgorithm(prov, hash, algName);
            // FIX: PasswordDeriveByte.CryptDeriveKey returned incorrect number of bytes for key (Bug #38)
			int keyLen = keySize / 8;
			byte [] baKey = Key.ExportSessionKey(prov, key, keyLen, true);

			Hash.DestroyHash(hash);
			Key.DestroyKey(key);

			Context.ReleaseContext(prov);
			return baKey;
		}

		/// <summary>
		/// Returns pseudo-random key bytes.
		/// </summary>
		/// <param name="cb">The number of pseudo-random key bytes to generate.</param>
		/// <returns>A byte array filled with pseudo-random key bytes.</returns>
		public override byte[] GetBytes(int cb)
		{
			byte[] data = ASCIIEncoder(password);
			if(salt != null)
			{
				byte[] temp = new byte[data.Length + salt.Length];
				Array.Copy(data, 0, temp, 0, data.Length);
				Array.Copy(salt, 0, temp, data.Length, salt.Length);
				data = temp;
			}

			var ha = GetHashAlgorithm(this.hashName);
			
            // FIX: PasswordDeriveBytes.GetBytes causes an ArgumentOutOfRange exception (Bug #25)
            if (iterations <= 0) iterations = 1;

            int len = 0;
            
            byte[] key = new byte[cb];
            while (len < cb)
            {
                for (int i = 0; i < iterations; i++)
                {
                    data = ha.ComputeHash(data);
                }
                int n = cb - len;
                n = (n < data.Length) ? n : data.Length;
                Array.Copy(data, 0, key, len, n);
                len += n;
            }

			return key;
		}

		/// <summary>
		/// Resets the state of the operation.
		/// </summary>
		public override void Reset()
		{
			this.salt = null;
		}

		private IntPtr GetKeyAlgorithm(IntPtr prov, IntPtr hash, string halg)
		{
			if(halg.ToLower().IndexOf("rc2") != -1)
				return Key.DeriveKey(prov, Calg.RC2, hash, GenKeyParam.EXPORTABLE | GenKeyParam.NO_SALT);
			else if(halg.ToLower().IndexOf("tripledes") != -1)
				return Key.DeriveKey(prov, Calg.TRIP_DES, hash, GenKeyParam.EXPORTABLE | GenKeyParam.NO_SALT);
			else if(halg.ToLower().IndexOf("des") != -1)
				return Key.DeriveKey(prov, Calg.DES, hash, GenKeyParam.EXPORTABLE | GenKeyParam.NO_SALT);
			else
				throw new Exception("unknown hash algorithm");
		}

		private IntPtr GetHashAlgorithm(IntPtr prov, string halg)
		{
			//keyed hashes
			if(halg.ToLower().IndexOf("hmacsha1") != -1)
				return Hash.CreateHash(prov, CalgHash.HMAC);
			if(halg.ToLower().IndexOf("mactripledes") != -1)
				return Hash.CreateHash(prov, CalgHash.MAC);
			//normal hashes
			if(halg.ToLower().IndexOf("md5") != -1)
				return Hash.CreateHash(prov, CalgHash.MD5);
			else if(halg.ToLower().IndexOf("sha") != -1)
				return Hash.CreateHash(prov, CalgHash.SHA1);
			else
				throw new Exception("unknown hash algorithm");
		}

        private System.Security.Cryptography.HashAlgorithm GetHashAlgorithm(string halg)
		{
			//keyed hashes
			if(halg.ToLower().IndexOf("hmacsha1") != -1)
				return new HMACSHA1();
			if(halg.ToLower().IndexOf("mactripledes") != -1)
				return new MACTripleDES();
				//normal hashes
			else if(halg.ToLower().IndexOf("md5") != -1)
				return new MD5CryptoServiceProvider();
			//else if(halg.ToLower().IndexOf("sha256") != -1)
			//	return new SHA256Managed();
			else if(halg.ToLower().IndexOf("sha1") != -1) //also in hmacsha1
				return new SHA1CryptoServiceProvider();
			else if(halg.ToLower().IndexOf("sha") != -1) //some form of sha last
				return new SHA1CryptoServiceProvider();
			else
				throw new Exception("unknown hash algorithm");
		}

		private static byte[] ASCIIEncoder(string s)
		{
			byte[] ascii = new byte[s.Length];
			for(int i = 0; i < s.Length; i++)
			{
				ascii[i] = (byte)s[i];
			}
			return ascii;
		}
	}
}
