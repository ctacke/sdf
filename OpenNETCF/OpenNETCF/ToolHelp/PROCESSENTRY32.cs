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

namespace OpenNETCF.ToolHelp
{
	#region PROCESSENTRY32 implementation

	internal class PROCESSENTRY32
	{
		// constants for structure definition
		private const int SizeOffset = 0;
		private const int UsageOffset = 4;
		private const int ProcessIDOffset=8;
		private const int DefaultHeapIDOffset = 12;
		private const int ModuleIDOffset = 16;
		private const int ThreadsOffset = 20;
		private const int ParentProcessIDOffset = 24;
		private const int PriClassBaseOffset = 28;
		private const int dwFlagsOffset = 32;
		private const int ExeFileOffset = 36;
		private const int MemoryBaseOffset = 556;
		private const int AccessKeyOffset = 560;
		private const uint SizeCE = 564;
        private const uint SizeWin32 = 300;
        private const int MAX_PATH = 260;

		// data members
		public uint dwSize; 
		public uint cntUsage; 
		public uint th32ProcessID; 
		public uint th32DefaultHeapID; 
		public uint th32ModuleID; 
		public uint cntThreads; 
		public uint th32ParentProcessID; 
		public long pcPriClassBase; 
		public uint dwFlags; 
		public string szExeFile;
		public uint th32MemoryBase;
		public uint th32AccessKey;

		//Default constructor
		public PROCESSENTRY32()
		{
            dwSize = (Environment.OSVersion.Platform == PlatformID.WinCE) ? SizeCE : SizeWin32;
		}

		// create a PROCESSENTRY instance based on a byte array		
		public PROCESSENTRY32(byte[] aData)
		{
            dwSize = Util.GetUInt(aData, SizeOffset);
            cntUsage = Util.GetUInt(aData, UsageOffset);
            th32ProcessID = Util.GetUInt(aData, ProcessIDOffset);
            th32DefaultHeapID = Util.GetUInt(aData, DefaultHeapIDOffset);
            th32ModuleID = Util.GetUInt(aData, ModuleIDOffset);
            cntThreads = Util.GetUInt(aData, ThreadsOffset);
            th32ParentProcessID = Util.GetUInt(aData, ParentProcessIDOffset);
            pcPriClassBase = (long)Util.GetUInt(aData, PriClassBaseOffset);
            dwFlags = Util.GetUInt(aData, dwFlagsOffset);
            szExeFile = Util.GetString(aData, ExeFileOffset, MAX_PATH).TrimEnd('\0');
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                th32MemoryBase = Util.GetUInt(aData, MemoryBaseOffset);
                th32AccessKey = Util.GetUInt(aData, AccessKeyOffset);
            }
		}

		// create an initialized data array
		public byte[] ToByteArray()
		{
			byte[] aData;
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                aData = new byte[SizeCE];
            }
            else
            {
                aData = new byte[SizeWin32];
            }

			//set the Size member
            Util.SetUInt(aData, SizeOffset, (int)((Environment.OSVersion.Platform == PlatformID.WinCE) ? SizeCE : SizeWin32));
			return aData;
		}

		public string ExeFile
		{
			get { return szExeFile; }
		}

		/// <summary>
		/// Identifier of the process. The contents of this member can be used by Win32 API elements.
		/// </summary>
		public ulong ProcessID
		{
			get
			{
				return th32ProcessID;
			}
		}

		public uint BaseAddress
		{
			get
			{
				return th32MemoryBase;
			}
		}

		public ulong ThreadCount
		{
			get
			{
				return cntThreads;
			}
		}
	}
	#endregion
}
