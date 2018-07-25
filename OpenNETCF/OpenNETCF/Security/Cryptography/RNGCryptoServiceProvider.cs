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