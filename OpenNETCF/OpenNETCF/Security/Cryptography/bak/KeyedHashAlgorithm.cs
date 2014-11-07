using System;

namespace OpenNETCF.Security.Cryptography
{
    /// <summary>
    /// Represents the abstract class from which all implementations of keyed hash algorithms must derive
    /// </summary>
    public abstract class KeyedHashAlgorithm : System.Security.Cryptography.HashAlgorithm, IDisposable
	{
        /// <summary>
        /// Initializes a new instance of KeyedHashAlgorithm
        /// </summary>
		protected KeyedHashAlgorithm() {}

        /// <summary>
        /// Gets or sets the key to be used in the hash algorithm
        /// </summary>
		public abstract byte[] Key { get; set; }

        void IDisposable.Dispose()
        {
            base.Dispose(true);
        }
    }
}
