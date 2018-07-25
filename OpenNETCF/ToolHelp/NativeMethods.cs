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
