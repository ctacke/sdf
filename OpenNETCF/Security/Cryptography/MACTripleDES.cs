using System;
using OpenNETCF.Security.Cryptography.Internal;

namespace OpenNETCF.Security.Cryptography
{
    /// <summary>
    /// Computes a Message Authentication Code (MAC) using TripleDES for the input data CryptoStream
    /// </summary>
	public class MACTripleDES : KeyedHashAlgorithm, IDisposable
	{
        /// <summary>
        /// Initializes an implementation of MACTripleDES
        /// </summary>
        public override void Initialize()
        {
            IntPtr prov = OpenNETCF.Security.Cryptography.Internal.Context.AcquireContext();
            IntPtr ipKey = OpenNETCF.Security.Cryptography.Internal.Key.GenKey(prov, Calg.TRIP_DES, GenKeyParam.EXPORTABLE);
            key = OpenNETCF.Security.Cryptography.Internal.Key.ExportSessionKey(prov, ipKey, 24, true);
            //reversed above
            OpenNETCF.Security.Cryptography.Internal.Key.DestroyKey(ipKey);
            OpenNETCF.Security.Cryptography.Internal.Context.ReleaseContext(prov);
        }

        /// <summary>
        /// Initializes a new instance of the MACTripleDES class
        /// </summary>
		public MACTripleDES()
		{
            Initialize();
		}

		//MACTripleDES uses a key of length 8, 16 or 24 bytes
        /// <summary>
        /// Initializes a new instance of the MACTripleDES class
        /// </summary>
        /// <param name="desKey"></param>
		public MACTripleDES(byte [] desKey)
		{
			if(desKey.Length != 24)
				throw new Exception("only supports 24 byte key lengths");
			key = (byte []) desKey.Clone();
		}

		private byte [] key = null;
        /// <summary>
        /// Gets or sets the key to be used in the hash algorithm
        /// </summary>
		public override byte[] Key 
		{ 
			get { return key; }
			set	{ key = value; }
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
			get{return 64;}
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
        /// Overloaded. Computes the hash value for the input data
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
		public new byte [] ComputeHash(byte [] buffer)
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

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            base.Dispose(true);
        }

        #endregion
    }
}
