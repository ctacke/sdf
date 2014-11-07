
using System;

namespace OpenNETCF.Net.Ftp
{
	/// <summary>
	/// Summary description for FtpStatusCode.
	/// </summary>
	public enum FtpStatusCode // TODO: Check up the actual number code to associate the different enum values with
	{
		ActionNotTaken,
		ArgumentSyntaxError,
		BadCommandSequence,
		CantOpenData,
		ClosingControl,
		ClosingData,
		CommandExtraneous,
		CommandNotImplemented,
		CommandOk,
		CommandSyntaxError,
		DataAlreadyOpen,
		DirectoryStatus,
		EnteringPassive,
		FileActionOk,
		FileCommandPending,
		FileStatus,
		LoggedInProceed,
		NeedLoginAccount,
		NotLoggedIn,
		OpeningData,
		PathnameCreated,
		RestartMarker,
		SendPasswordCommand,
		SendUserCommand
	}
}






















