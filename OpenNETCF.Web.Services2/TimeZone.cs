
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenNETCF.Web.Services2
{
	public class TimeZone
	{
		public TimeZone() {}

		[DllImport("coredll.dll", EntryPoint="GetTimeZoneInformation", SetLastError=true)]
		private static extern UInt32 GetTimeZoneInformationCe(byte [] lpTimeZoneInformation);
		[DllImport("kernel32.dll", EntryPoint="GetTimeZoneInformation", SetLastError=true)]
		private static extern UInt32 GetTimeZoneInformationXp(byte [] lpTimeZoneInformation);
		[CLSCompliant(false)]
		public static UInt32 GetTimeZoneInformation(byte [] lpTimeZoneInformation)
		{
			if(System.Environment.OSVersion.Platform == PlatformID.WinCE)
				return GetTimeZoneInformationCe(lpTimeZoneInformation);
			else
				return GetTimeZoneInformationXp(lpTimeZoneInformation);
		}


		[DllImport("coredll.dll", EntryPoint="SetTimeZoneInformation", SetLastError=true)]
		private static extern bool SetTimeZoneInformationCe(byte [] lpTimeZoneInformation);
		[DllImport("kernel32.dll", EntryPoint="SetTimeZoneInformation", SetLastError=true)]
		private static extern bool SetTimeZoneInformationXp(byte [] lpTimeZoneInformation);
		private static bool SetTimeZoneInformation(byte [] lpTimeZoneInformation)
		{
			if(System.Environment.OSVersion.Platform == PlatformID.WinCE)
				return SetTimeZoneInformationCe(lpTimeZoneInformation);
			else
				return SetTimeZoneInformationXp(lpTimeZoneInformation);
		}

		public struct TIME_ZONE_INFORMATION
		{
			public int Bias;
			public string StandardName;
			public SYSTEMTIME StandardDate;
			public int StandardBias;
			public string DaylightName;
			public SYSTEMTIME DaylightDate;
			public int DaylightBias;
		}

		public struct SYSTEMTIME
		{
			public short wYear;
			public short wMonth;
			public short wDayOfWeek;
			public short wDay;
			public short wHour;
			public short wMinute;
			public short wSecond;
			public short wMilliseconds;
		}

		private static SYSTEMTIME ManuallyUnmarshalSystemTime(byte [] managedBa, int offset)
		{
			SYSTEMTIME st = new SYSTEMTIME();
			st.wYear = BitConverter.ToInt16(managedBa, offset);
			st.wMonth = BitConverter.ToInt16(managedBa, offset + 2);
			st.wDayOfWeek = BitConverter.ToInt16(managedBa, offset + 4);
			st.wDay = BitConverter.ToInt16(managedBa, offset + 6);
			st.wHour = BitConverter.ToInt16(managedBa, offset + 8);
			st.wMinute = BitConverter.ToInt16(managedBa, offset + 10);
			st.wSecond = BitConverter.ToInt16(managedBa, offset + 12);
			st.wMilliseconds = BitConverter.ToInt16(managedBa, offset + 14);
			return st;
		}

		private static byte[] ManuallyMarshalSystemTime(SYSTEMTIME st)
		{
			byte[] bBuffer = new byte[16];
			byte [] ba = BitConverter.GetBytes(st.wYear);
			Buffer.BlockCopy(ba, 0, bBuffer, 0, 2 );
			ba = BitConverter.GetBytes(st.wMonth);
			Buffer.BlockCopy(ba, 0, bBuffer, 2, 2 );
			ba = BitConverter.GetBytes(st.wDayOfWeek);
			Buffer.BlockCopy(ba, 0, bBuffer, 4, 2 );
			ba = BitConverter.GetBytes(st.wDay);
			Buffer.BlockCopy(ba, 0, bBuffer, 6, 2 );
			ba = BitConverter.GetBytes(st.wHour);
			Buffer.BlockCopy(ba, 0, bBuffer, 8, 2 );
			ba = BitConverter.GetBytes(st.wMinute);
			Buffer.BlockCopy(ba, 0, bBuffer, 10, 2 );
			ba = BitConverter.GetBytes(st.wSecond);
			Buffer.BlockCopy(ba, 0, bBuffer, 12, 2 );
			ba = BitConverter.GetBytes(st.wMilliseconds);
			Buffer.BlockCopy(ba, 0, bBuffer, 14, 2 );
			return bBuffer;
		}

		public static TIME_ZONE_INFORMATION GetTimeZoneInfo()
		{
			TIME_ZONE_INFORMATION tzi;
			byte[] bBuffer = new byte[172];
			UInt32 zoneId = GetTimeZoneInformation(bBuffer);
			//TODO check return - returns 2

			GCHandle hTemp = GCHandle.Alloc(bBuffer, GCHandleType.Pinned);
			IntPtr pInt = hTemp.AddrOfPinnedObject();
			
			tzi.Bias = BitConverter.ToInt32(bBuffer, 0); //0-3
			tzi.StandardName = Encoding.Unicode.GetString(bBuffer, 4, 64); //4-67
			tzi.StandardName = tzi.StandardName.Replace("\0","");
			tzi.StandardDate = ManuallyUnmarshalSystemTime(bBuffer, 68); //68-83
			tzi.StandardBias = BitConverter.ToInt32(bBuffer, 84); //84-87
			tzi.DaylightName = Encoding.Unicode.GetString(bBuffer, 88, 64); //88-151
			tzi.DaylightName = tzi.DaylightName.Replace("\0","");
			tzi.DaylightDate = ManuallyUnmarshalSystemTime(bBuffer, 152); //152-167
			tzi.DaylightBias = BitConverter.ToInt32(bBuffer, 168); //168-171
			
			hTemp.Free();
			return tzi;
		}

		public static bool SetTimeZoneInfo(TIME_ZONE_INFORMATION tzi)
		{
			byte[] bBuffer = new byte[172];

			byte [] ba = BitConverter.GetBytes(tzi.Bias);
			Buffer.BlockCopy( ba, 0, bBuffer, 0, 4 );
			
			ba = Encoding.Unicode.GetBytes(tzi.StandardName);
			Buffer.BlockCopy(ba, 0, bBuffer, 4, Math.Min( ba.Length, 64 ) );
			
			ba = ManuallyMarshalSystemTime(tzi.StandardDate);
			Buffer.BlockCopy( ba, 0, bBuffer, 68, 16 );
			
			ba = BitConverter.GetBytes(tzi.StandardBias);
			Buffer.BlockCopy( ba, 0, bBuffer, 84, 4 );
			
			tzi.DaylightName=tzi.DaylightName.TrimEnd(new char[]{(char)0});
			ba = Encoding.Unicode.GetBytes(tzi.DaylightName);
			Buffer.BlockCopy(ba, 0, bBuffer, 88, Math.Min( ba.Length, 64 ) );
			
			ba = ManuallyMarshalSystemTime(tzi.DaylightDate);
			Buffer.BlockCopy( ba, 0, bBuffer, 152, 16 );
			
			ba = BitConverter.GetBytes(tzi.DaylightBias);
			Buffer.BlockCopy( ba, 0, bBuffer, 168, 4 );

			bool retVal = SetTimeZoneInformation(bBuffer);
			return retVal;
		}
	}
}
