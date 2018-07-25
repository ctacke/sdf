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
using System.Text;

namespace OpenNETCF.ToolHelp
{
	internal class NativeMethods
	{
		#region Helper conversion functions
		// utility:  get a ulong from the byte array
		internal static ulong GetULng(byte[] aData, int Offset)
		{
			return BitConverter.ToUInt64(aData, Offset);
		}

		// utility:  get a uint from the byte array
		internal static uint GetUInt(byte[] aData, int Offset)
		{
			return BitConverter.ToUInt32(aData, Offset);
		}
		
		// utility:  set a uint int the byte array
		internal static void SetUInt(byte[] aData, int Offset, int Value)
		{
			byte[] buint = BitConverter.GetBytes(Value);
			Buffer.BlockCopy(buint, 0, aData, Offset, buint.Length);
		}

		// utility:  get a ushort from the byte array
		internal static ushort GetUShort(byte[] aData, int Offset)
		{
			return BitConverter.ToUInt16(aData, Offset);
		}
		
		// utility:  set a ushort int the byte array
		internal static void SetUShort(byte[] aData, int Offset, int Value)
		{
			byte[] bushort = BitConverter.GetBytes((short)Value);
			Buffer.BlockCopy(bushort, 0, aData, Offset, bushort.Length);
		}
		
		// utility:  get a unicode string from the byte array
		internal static string GetString(byte[] aData, int Offset, int Length)
		{
			String sReturn =  Encoding.Unicode.GetString(aData, Offset, Length);
			return sReturn;
		}
		
		// utility:  set a unicode string in the byte array
		internal static void SetString(byte[] aData, int Offset, string Value)
		{
			byte[] arr = Encoding.ASCII.GetBytes(Value);
			Buffer.BlockCopy(arr, 0, aData, Offset, arr.Length);
		}
		#endregion

		#region PInvoke declarations
		internal const int PROCESS_TERMINATE = 1;
		internal const int INVALID_HANDLE_VALUE = -1;
		internal const int TH32CS_SNAPHEAPLIST = 0x00000001;
		internal const int TH32CS_SNAPPROCESS = 0x00000002;
		internal const int TH32CS_SNAPTHREAD = 0x00000004;
		internal const int TH32CS_SNAPMODULE = 0x00000008;
        internal const int TH32CS_SNAPNOHEAPS = 0x40000000;

		[DllImport("toolhelp.dll", SetLastError=true)]
		internal static extern IntPtr CreateToolhelp32Snapshot(uint flags, uint processid);
		[DllImport("toolhelp.dll", SetLastError=true)]
		internal static extern int CloseToolhelp32Snapshot(IntPtr handle);
		[DllImport("toolhelp.dll", SetLastError=true)]
		internal static extern int Process32First(IntPtr handle, byte[] pe);
		[DllImport("toolhelp.dll", SetLastError=true)]
		internal static extern int Process32Next(IntPtr handle, byte[] pe);
		[DllImport("toolhelp.dll", SetLastError=true)]
		internal static extern int Thread32First(IntPtr handle, byte[] te);
		[DllImport("toolhelp.dll", SetLastError=true)]
		internal static extern int Thread32Next(IntPtr handle, byte[] te);
        [DllImport("toolhelp.dll", SetLastError = true)]
        internal static extern int Module32First(IntPtr handle, byte[] me);
        [DllImport("toolhelp.dll", SetLastError = true)]
        internal static extern int Module32Next(IntPtr handle, byte[] me);
        #endregion
	}
}
