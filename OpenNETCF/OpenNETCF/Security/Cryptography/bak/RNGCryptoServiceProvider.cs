using System;
using OpenNETCF.Security.Cryptography.Internal;
using System.Security.Cryptography;

/* - supported by netcf2
namespace OpenNETCF.Security.Cryptography
{
    /// <summary>
    /// Implements a cryptographic Random Number Generator (RNG) using the implementation provided by the cryptographic service provider (CSP).
    /// </summary>
	public class RNGCryptoServiceProvider : RandomNumberGenerator
	{
        /// <summary>
        /// Initializes a new instance of the RNGCryptoServiceProvider class.
        /// </summary>
		public RNGCryptoServiceProvider()
		{

		}

        /// <summary>
        /// Fills an array of bytes with a cryptographically strong random sequence of values
        /// </summary>
        /// <param name="seed"></param>
		public override void GetBytes(byte[] seed)
		{
            seed = OpenNETCF.Security.Cryptography.Internal.Rand.GetRandomBytes(seed);
		}

        /// <summary>
        /// Fills an array of bytes with a cryptographically strong random sequence of nonzero values.
        /// </summary>
        /// <param name="seed"></param>
		public override void GetNonZeroBytes(byte[] seed)
		{
            seed = OpenNETCF.Security.Cryptography.Internal.Rand.GetNonZeroBytes(seed);
		}
	}
}
*/