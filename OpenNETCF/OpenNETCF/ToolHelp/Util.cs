using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenNETCF.ToolHelp
{
	internal class Util
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
            if(Environment.OSVersion.Platform == PlatformID.WinCE)
			    return Encoding.Unicode.GetString(aData, Offset, Length);
            else
                return Encoding.ASCII.GetString(aData, Offset, Length);
		}
		
		// utility:  set a unicode string in the byte array
		internal static void SetString(byte[] aData, int Offset, string Value)
		{
			byte[] arr = Encoding.ASCII.GetBytes(Value);
			Buffer.BlockCopy(arr, 0, aData, Offset, arr.Length);
		}
		#endregion

	}
}
