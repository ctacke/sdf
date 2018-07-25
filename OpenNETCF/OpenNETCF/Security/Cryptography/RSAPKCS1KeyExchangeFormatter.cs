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
