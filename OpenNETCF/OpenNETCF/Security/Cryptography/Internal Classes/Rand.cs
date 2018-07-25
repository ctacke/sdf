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
/*
namespace OpenNETCF.Security.Cryptography.Internal
{
	internal class Rand
	{
		/// <summary>
		/// if crypto is not available
		/// </summary>
		/// <remarks>not on PPC 2002 device</remarks>
		public static byte [] CeGenRandom(int length)
		{
			byte [] baTemp = new byte[length];
            bool retVal = NativeMethods.CeGenRandom(baTemp.Length, baTemp);
			ErrCode ec = Error.HandleRetVal(retVal);
			return baTemp;
		}

		/// <summary>
		/// not seeded
		/// </summary>
		public static byte [] GetRandomBytes(int length)
		{
			byte[] randomBuf = new byte[length];
			return GetRandomBytes(randomBuf);
		}

		/// <summary>
		/// seeded, dont have to specify a provider
		/// </summary>
		public static byte [] GetRandomBytes(byte[] seed)
		{
			IntPtr prov = Context.AcquireContext(ProvType.RSA_FULL);
            bool retVal = NativeMethods.CryptGenRandom(prov, seed.Length, seed);
			ErrCode ec = Error.HandleRetVal(retVal);
			Context.ReleaseContext(prov);
			return seed;
		}

		public static byte [] GetNonZeroBytes(byte[] seed)
		{
			byte [] buffer = GetRandomBytes(seed);
			for(int i=0; i<buffer.Length; i++)
			{
				if(buffer[i] == 0)
					buffer[i] = 1;
			}
			return buffer;
		}
	}
}
*/