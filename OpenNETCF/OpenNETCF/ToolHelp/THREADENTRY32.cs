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
