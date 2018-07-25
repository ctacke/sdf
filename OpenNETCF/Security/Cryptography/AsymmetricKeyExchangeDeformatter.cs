using System;
using System.Security;
using System.Security.Cryptography;

namespace OpenNETCF.Security.Cryptography 
{    
    /// <summary>
    /// Represents the base class from which all asymmetric key exchange deformatters derive.
    /// </summary>
	public abstract class AsymmetricKeyExchangeDeformatter 
	{
        /// <summary>
        /// Initializes a new instance of AsymmetricKeyExchangeDeformatter.
        /// </summary>
		public AsymmetricKeyExchangeDeformatter() {}
        /// <summary>
        /// When overridden in a derived class, gets or sets the parameters for the asymmetric key exchange.
        /// </summary>
		public abstract string Parameters {get; set;}
        /// <summary>
        /// When overridden in a derived class, extracts secret information from the encrypted key exchange data.
        /// </summary>
        /// <param name="rgb"></param>
        /// <returns></returns>
		public abstract byte[] DecryptKeyExchange(byte[] rgb);
        /// <summary>
        /// When overridden in a derived class, sets the private key to use for decrypting the secret information.
        /// </summary>
        /// <param name="key"></param>
		public abstract void SetKey(AsymmetricAlgorithm key);	
	}	
}