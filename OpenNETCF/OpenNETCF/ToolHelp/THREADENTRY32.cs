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

namespace OpenNETCF.ToolHelp
{
	internal class THREADENTRY32
	{
		// constants for structure definition
		private const int SizeOffset = 0;
		private const int UsageOffset = 4;
		private const int ThreadIDOffset=8;
		private const int OwnerProcessIDOffset = 12;
		private const int BasePriorityOffset = 16;
		private const int DeltaPriorityOffset = 24;
		private const int FlagsOffset = 32;
		private const int AccessKeyOffset = 36;
		private const int CurrentProcessIDOffset = 40;
		private const int Size = 44;

		// data members
		public uint dwSize; 
		public uint cntUsage; 
		public uint th32ThreadID; 
		public uint th32OwnerProcessID; 
		public int tpBasePri; 
		public int tpDeltaPri; 
		public uint dwFlags; 
		public uint th32AccessKey;
		public uint th32CurrentProcessID;

		//Default constructor
		public THREADENTRY32()
		{
		}

		// create a PROCESSENTRY instance based on a byte array		
		public THREADENTRY32(byte[] aData)
		{
            dwSize = Util.GetUInt(aData, SizeOffset);
            cntUsage = Util.GetUInt(aData, UsageOffset);
            th32ThreadID = Util.GetUInt(aData, ThreadIDOffset);
			th32OwnerProcessID = BitConverter.ToUInt32(aData, OwnerProcessIDOffset);
			tpBasePri = BitConverter.ToInt32(aData, BasePriorityOffset);
			tpDeltaPri = BitConverter.ToInt32(aData, DeltaPriorityOffset);
            dwFlags = Util.GetUInt(aData, FlagsOffset);
            th32AccessKey = Util.GetUInt(aData, AccessKeyOffset);
            th32CurrentProcessID = Util.GetUInt(aData, CurrentProcessIDOffset);
		}

		// create an initialized data array
		public byte[] ToByteArray()
		{
			byte[] aData;
			aData = new byte[Size];
			//set the Size member
            Util.SetUInt(aData, SizeOffset, Size);
			return aData;
		}
	}
}
