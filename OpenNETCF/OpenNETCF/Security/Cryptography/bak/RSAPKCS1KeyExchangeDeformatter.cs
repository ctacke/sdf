using System;
using System.Security.Cryptography;

namespace OpenNETCF.Security.Cryptography 
{ 
    /// <summary>
    /// Decrypts the PKCS #1 key exchange data
    /// </summary>
	public class RSAPKCS1KeyExchangeDeformatter : AsymmetricKeyExchangeDeformatter 
	{
		private RSA rsa;
		private string param;
		private RandomNumberGenerator random;

	    /// <summary>
        /// Initializes a new instance of the RSAPKCS1KeyExchangeDeformatter class.
	    /// </summary>
		public RSAPKCS1KeyExchangeDeformatter () 
		{
			rsa = null;
		}

	    /// <summary>
        /// Initializes a new instance of the RSAPKCS1KeyExchangeDeformatter class.
	    /// </summary>
	    /// <param name="key"></param>
		public RSAPKCS1KeyExchangeDeformatter (AsymmetricAlgorithm key) 
		{
			SetKey (key);
		}

	    /// <summary>
        /// Gets the parameters for the PKCS #1 key exchange.
	    /// </summary>
		public override string Parameters 
		{
			get { return param; }
			set { param = value; }
		}
	
        /// <summary>
        /// Gets or sets the random number generator algorithm to use in the creation of the key exchange.
        /// </summary>
		public RandomNumberGenerator RNG 
		{
			get { return random; }
			set { random = value; }
		}

	    /// <summary>
        /// Extracts secret information from the encrypted key exchange data.
	    /// </summary>
        /// <param name="rgbData">The key exchange data within which the secret information is hidden</param>
        /// <returns>The secret information derived from the key exchange data.</returns>
		public override byte[] DecryptKeyExchange (byte[] rgbData) 
		{
			if (rsa == null)
				throw new CryptographicException ();
			return rsa.DecryptValue(rgbData);
		}

	    /// <summary>
        /// Sets the private key to use for decrypting the secret information.
	    /// </summary>
        /// <param name="key">The instance of the RSA algorithm that holds the private key.</param>
		public override void SetKey (AsymmetricAlgorithm key) 
		{
			if (key is RSA) {
				rsa = (RSA)key;
			}
			else
				throw new CryptographicException ();
		}
	}
}
