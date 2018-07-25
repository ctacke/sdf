using System;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Handler for FTP responses
	/// </summary>
	public delegate void FTPResponseHandler(FTPResponse Response);
	/// <summary>
	/// Handler for FTP commands
	/// </summary>
	public delegate void FTPCommandHandler(string CommandSent);
	/// <summary>
	/// Handler for FTP connections
	/// </summary>
	public delegate void FTPConnectedHandler();

	internal enum FTPMode
	{
		Passive = 0,
		Active	= 1
	}

	/// <summary>
	/// FTP Transfer type
	/// </summary>
	public enum FTPTransferType
	{
		/// <summary>
		/// Binary Transfer
		/// </summary>
		Binary	= 0,
		/// <summary>
		/// ASCII Transfer
		/// </summary>
		ASCII	= 1
	}

	/// <summary>
	/// Detected FTP Server type
	/// </summary>
	public enum FTPServerType
	{
		/// <summary>
		/// Unix-compliant server
		/// </summary>
		Unix	= 0,
		/// <summary>
		/// Windows/IIS-compliant server
		/// </summary>
		Windows	= 1,
		/// <summary>
		/// Unknown server type
		/// </summary>
		Unknown	= 2
	}
	
	/// <summary>
	/// Information returned in a response from an FTP command
	/// <seealso cref="FTP.ReadResponse()"/>
	/// </summary>
	public struct FTPResponse
	{
		/// <summary>
		/// Response ID value
		/// </summary>
		public int ID;
		/// <summary>
		/// Response text
		/// </summary>
		public string Text;
	}

	/// <summary>
	/// FTP File Type
	/// </summary>
	public enum FTPFileType
	{
		/// <summary>
		/// A file
		/// </summary>
		File		= 0,
		/// <summary>
		/// A directory
		/// </summary>
		Directory	= 1
	}
}
