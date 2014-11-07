using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace OpenNETCF.Security.Cryptography.Internal
{
	internal class Error
	{
		private Error() {}

		//[DllImport("coredll.dll", EntryPoint="GetLastError", SetLastError=true)]
		//internal static extern uint GetLastError();
		/*
		public const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

		[DllImport("coredll.dll", EntryPoint="FormatMessage", SetLastError=true)]
		internal static extern uint FormatMessage(uint dwFlags, string lpSource, uint dwMessageId, 
			uint dwLanguageId, StringBuilder lpBuffer, uint nSize, string [] Arguments);
		*/
		public static ErrCode HandleRetVal(bool retVal)
		{
			ErrCode [] eca = new ErrCode[0];
			return HandleRetVal(retVal, eca);
		}

		public static ErrCode HandleRetVal(bool retVal, ErrCode expected)
		{
			ErrCode [] eca = new ErrCode[1];
			eca[0] = expected;
			return HandleRetVal(retVal, eca);
		}

		public static ErrCode HandleRetVal(bool retVal, ErrCode [] expected)
		{
			ErrCode ec = ErrCode.SUCCESS;
			if(retVal == false)
			{
				uint lastErr = (uint) Marshal.GetLastWin32Error();
				ec = (ErrCode) lastErr;
				bool isExpected = false;
				foreach(ErrCode expect in expected)
				{
					if(ec == expect)
						isExpected = true;
				}
				if(isExpected == false)
					throw new Exception("bNb.Sec: " + ec.ToString());						
			}
			return ec;
		}
	}
}
