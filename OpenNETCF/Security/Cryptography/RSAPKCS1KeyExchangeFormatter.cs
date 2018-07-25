using System;
using System.Security.Cryptography;

namespace OpenNETCF.Security.Cryptography 
{	
    /// <summary>
    /// Creates the PKCS#1 key exchange data using RSA.
    /// </summary>
	public class RSAPKCS1KeyExchangeFormatter: AsymmetricKeyExchangeFormatter
	{
		private RSA rsa;
		private RandomNumberGenerator random;
	
        /// <summary>
        /// Initializes a new instance of the RSAPKCS1KeyExchangeFormatter class.
        /// </summary>
		public RSAPKCS1KeyExchangeFormatter ()
		{
		}
	
        /// <summary>
        /// Initializes a new instance of the RSAPKCS1KeyExchangeFormatter class.
        /// </summary>
        /// <param name="key">The instance of the RSA algorithm that holds the public key</param>
		public RSAPKCS1KeyExchangeFormatter (AsymmetricAlgorithm key)
		{
			SetKey (key);
		}
	
        /// <summary>
        /// Gets or sets the random number generator algorithm to use in the creation of the key exchange.
        /// </summary>
		public RandomNumberGenerator Rng 
		{
			get { return random; }
			set { random = value; }
		}

	    /// <summary>
        /// Gets the parameters for the PKCS #1 key exchange
	    /// </summary>
        /// <value>An XML string containing the parameters of the PKCS #1 key exchange operation</value>
		public override string Parameters 
		{
			get { return "<enc:KeyEncryptionMethod enc:Algorithm=\"http://www.microsoft.com/xml/security/algorithm/PKCS1-v1.5-KeyEx\" xmlns:enc=\"http://www.microsoft.com/xml/security/encryption/v1.0\" />"; }
		}

	    /// <summary>
        /// Creates the encrypted key exchange data
	    /// </summary>
        /// <param name="rgbData">The secret information to be passed in the key exchange</param>
        /// <returns>The encrypted key exchange data to be sent to the intended recipient</returns>
		public override byte[] CreateKeyExchange (byte[] rgbData)
		{
			if (rsa == null)
				throw new CryptographicException ();
			if (random == null)
				random = new RNGCryptoServiceProvider();
			return rsa.EncryptValue(rgbData);
		}
	
        /// <summary>
        /// Creates the encrypted key exchange data
        /// </summary>
        /// <param name="rgbData">The secret information to be passed in the key exchange</param>
        /// <param name="symAlgType">This parameter is not used in the current version</param>
        /// <returns>The encrypted key exchange data to be sent to the intended recipient</returns>
		public override byte[] CreateKeyExchange (byte[] rgbData, Type symAlgType)
		{
			return CreateKeyExchange (rgbData);
		}
	
        /// <summary>
        /// Sets the public key to use for encrypting the key exchange data
        /// </summary>
        /// <param name="key">The instance of the RSA algorithm that holds the public key</param>
		public override void SetKey (AsymmetricAlgorithm key)
		{
			if (key != null) {
				if (key is RSA) {
					rsa = (RSA)key;
				}
				else
					throw new InvalidCastException ();
			}
		}
	}
}
