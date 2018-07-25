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