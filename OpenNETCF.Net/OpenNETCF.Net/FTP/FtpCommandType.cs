using System;

namespace OpenNETCF.Net.Ftp
{
	/// <summary>
	/// Summary description for FtpMethod.
	/// </summary>
	public enum FtpCommandType
	{
		FtpControlCommand	   = 1,
		FtpDataReceiveCommand  = 2,
		FtpDataSendCommand	   = 3,
		FtpCommandNotSupported = 4,
	}
}
