
using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace OpenNETCF.Net.Ftp
{
	/// <summary>
	/// An FTP connection has got two streams, one for sending control commands an another one for "data" retrieval and "sending"
	/// This stream is the Control Stream of the FTP connection, meaning it will be used to transmit control commands and
	/// also for checking if transmissions of those control commands was ok.
	/// </summary>
	public class FtpDataStream : FtpStream, IDisposable
	{
		internal FtpDataStream( Socket inSocket )
			: base( inSocket )
		{
		}

		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		public override bool CanWrite
		{
			get
			{
				// Need to be able to write this stream to upload files
				// to the server.
				return true;
			}
		}

/*        
		#region IDisposable Members

		public void Dispose()
		{
			base.Dispose();
		}

		#endregion
*/
	}
}













