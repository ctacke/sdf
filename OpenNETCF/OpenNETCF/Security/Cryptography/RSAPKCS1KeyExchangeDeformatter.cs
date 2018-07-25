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
