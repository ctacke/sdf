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
using System.Runtime.InteropServices;

namespace OpenNETCF.Security.Cryptography.Internal
{
	//http://smartdevices.microsoftdev.com/Learn/Articles/500.aspx#netcfadvinterop_topic3
	internal class Mem
	{
		/// <summary>
		/// The CryptMemAlloc function allocates memory for a buffer. 
		/// It is used by all Crypt32.lib functions that return allocated buffers.
		/// </summary>
		/// <param name="cbSize">Number of bytes to be allocated. </param>
		/// <returns>Returns a pointer to the buffer allocated. 
		/// If the function fails, NULL is returned. </returns>
		//LPVOID WINAPI CryptMemAlloc(ULONG cbSize);
		[DllImport("crypt32.dll", EntryPoint="CryptMemAlloc", SetLastError=true)]
		public static extern IntPtr CryptMemAlloc(int cbSize);
		
		/// <summary>
		/// The CryptMemFree function frees memory allocated by 
		/// CryptMemAlloc or CryptMemRealloc.
		/// </summary>
		/// <param name="pv">Pointer to the buffer to be freed. </param>
		//void WINAPI CryptMemFree(LPVOID pv);
		[DllImport("crypt32.dll", EntryPoint="CryptMemFree", SetLastError=true)]
		public static extern void CryptMemFree(IntPtr pv);
		
		/// <summary>
		/// The CryptMemRealloc function frees the memory currently allocated for a buffer 
		/// and allocates memory for a new buffer.
		/// </summary>
		/// <param name="pv">Pointer to a currently allocated buffer. </param>
		/// <param name="cbSize">Number of bytes to be allocated. </param>
		/// <returns>Returns a pointer to the buffer allocated. 
		/// If the function fails, NULL is returned. </returns>
		//LPVOID WINAPI CryptMemRealloc(LPVOID pv, ULONG cbSize);
		[DllImport("crypt32.dll", EntryPoint="CryptMemRealloc", SetLastError=true)]
		public static extern IntPtr CryptMemRealloc(IntPtr pv, int cbSize);

	}
}
