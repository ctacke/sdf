using System;
using System.Security;
using System.Security.Cryptography;

namespace OpenNETCF.Security.Cryptography 
{
    /// <summary>
    /// Represents the base class from which all asymmetric key exchange formatters derive.
    /// </summary>
	public abstract class AsymmetricKeyExchangeFormatter 
	{
        /// <summary>
        /// Initializes a new instance of AsymmetricKeyExchangeFormatter
        /// </summary>
		public AsymmetricKeyExchangeFormatter() {}
		
        /// <summary>
        /// When overridden in a derived class, gets the parameters for the asymmetric key exchange.
        /// </summary>
		public abstract string Parameters {get;}
		
        /// <summary>
        /// When overridden in a derived class, creates the encrypted key exchange data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
		public abstract byte[] CreateKeyExchange(byte[] data);

        /// <summary>
        /// When overridden in a derived class, creates the encrypted key exchange data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="symAlgType"></param>
        /// <returns></returns>
		public abstract byte[] CreateKeyExchange(byte[] data, Type symAlgType);

        /// <summary>
        /// When overridden in a derived class, sets the public key to use for encrypting the secret information.
        /// </summary>
        /// <param name="key"></param>
		public abstract void SetKey(AsymmetricAlgorithm key);
		
	}
} 