
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace OpenNETCF.Net.Ftp
{
	/// <summary>
	/// Summary description for FtpWebResponse.
	/// </summary>
	public class FtpWebResponse : WebResponse, IDisposable
	{
		private FtpDataStream dataStream;
		internal FtpWebResponse( FtpDataStream stream )
		{
			dataStream = stream;
		}

		public override void Close()
		{
			throw new NotSupportedException();
		}

		public override Stream GetResponseStream()
		{
			return dataStream;
		}

		public string BannerMessage
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long ContentLength
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public string ExitMessage
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override WebHeaderCollection Headers
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public DateTime LastModified
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override Uri ResponseUri
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public FtpStatusCode Status // TODO: Implement FtpStatusCode
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public string StatusDescription
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public string WelcomeMessage
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion
	}
}
