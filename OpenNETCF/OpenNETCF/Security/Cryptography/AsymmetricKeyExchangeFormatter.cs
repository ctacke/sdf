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