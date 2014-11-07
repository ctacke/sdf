
using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace OpenNETCF.Net.Ftp
{
	/// <summary>
	/// Summary description for FtpStream.
	/// </summary>
	public abstract class FtpStream  : Stream, IDisposable
	{
		protected Socket socket;

		// We wait for ONE second trying to poll from socket before giving up and setting read stream in "CanRead = false" state
		protected int socketPollTime = 1000000;

		internal FtpStream( Socket inSocket )
		{
			socket = inSocket;
		}

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">Source buffer for data to write</param>
        /// <param name="offset">Offset in the buffer at which source data begins</param>
        /// <param name="length">Number of bytes to write</param>
		public override void Write( byte [] buffer, int offset, int length)
		{
			socket.Send( buffer, offset, length, SocketFlags.None );
		}

        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read
        /// </summary>
        /// <param name="buffer">Buffer into which data will be read</param>
        /// <param name="offset">Offset in the buffer to begin placing data</param>
        /// <param name="length">Number of bytes to read</param>
        /// <returns>Actual number of bytes read</returns>
		public override int Read( byte[] buffer, int offset, int length )
		{
			if( socket.Available <= 0 )
			{
				if( !socket.Poll( socketPollTime, SelectMode.SelectRead ) )
				{
					return 0;
				}
			}
			int bytesRead = socket.Receive( buffer, offset, length, SocketFlags.None );
			return bytesRead;
		}

        /// <summary>
        /// Closes the current stream and releases any resources (such as sockets and file handles) associated with the current stream
        /// </summary>
		public override void Close()
		{
			socket.Shutdown( SocketShutdown.Both );
			socket.Close();
		}

        /// <summary>
        /// FTPStream are not seekable.  Returns <b>false</b>
        /// </summary>
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

        /// <summary>
        /// Unsupported
        /// </summary>
		public override long Length
		{
			get
			{
				throw new NotSupportedException("This stream cannot be seeked");
			}
		}

        /// <summary>
        /// Unsupported
        /// </summary>
		public override long Position
		{
			get 
			{
				throw new NotSupportedException("This stream cannot be seeked");
			}

			set 
			{
				throw new NotSupportedException("This stream cannot be seeked");
			}
		}

        /// <summary>
        /// Unsupported
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
		public override long Seek( long offset, SeekOrigin origin ) 
		{
			throw new NotSupportedException("This stream cannot be seeked");
		}

        /// <summary>
        /// Has no effect on the FTP stream
        /// </summary>
		public override void Flush() 
		{
		}

        /// <summary>
        /// Unsupported
        /// </summary>
        /// <param name="value"></param>
		public override void SetLength( long value ) 
		{
			throw new NotSupportedException("This stream cannot be seeked");
		}

		#region IDisposable Members

        /// <summary>
        /// Releases socket resources used by the stream
        /// </summary>
		public void Dispose()
		{
			socket.Close();
		}

		#endregion
	}
}
