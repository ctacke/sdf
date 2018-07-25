#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion




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
	public class FtpControlStream : FtpStream, IDisposable
	{
		private int statusCode;
		private MemoryStream bufferMemoryStream;

		internal FtpControlStream( Socket inSocket )
			: base( inSocket )
		{
		}

		internal int StatusCode
		{
			get
			{
				return statusCode;
			}
		}

		private void SetStatus( byte[] bytes, int bytesRead )
		{
			if( bytesRead == 0 )
				return;
			if( bytesRead < 5 )
				return;
			string tmp = Encoding.ASCII.GetString( bytes, 0, bytesRead );
			if( tmp[tmp.Length-1] != '\n' || tmp[tmp.Length-2] != '\r' )
				return;
			else
			{
				try
				{
					tmp = tmp.Substring( 0, tmp.Length - 2 );
					int start = tmp.LastIndexOf( "\r\n" );
					if( start != -1 )
						tmp = tmp.Substring( start+2 );
					statusCode = Int32.Parse( tmp.Substring( 0, 3 ) );
				}
				catch( FormatException )
				{
					statusCode = -1;
					;//DO NOTHING
				}
				catch( OverflowException )
				{
					statusCode = -1;
					;//DO NOTHING
				}
				return;
			}
		}

        /// <summary>
        /// Writes data to the stream
        /// </summary>
        /// <param name="buffer">Data to write</param>
        /// <param name="offset">Offset at which to begin the write</param>
        /// <param name="count">Number of bytes from the buffer to write</param>
		public override void Write(byte[] buffer, int offset, int count)
		{
			base.Write( buffer, offset, count );
		}

        /// <summary>
        /// Sets the current position in the stream
        /// </summary>
		public override long Position
		{
			get
			{
				if( bufferMemoryStream != null )
					return bufferMemoryStream.Position;
				else
					throw new NotSupportedException();
			}
			set
			{
				if( bufferMemoryStream != null )
					bufferMemoryStream.Position = value;
				else
					throw new NotSupportedException();
			}
		}

        /// <summary>
        /// Returns the readability status of the stream.  Always <b>true</b> for this class
        /// </summary>
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

        /// <summary>
        /// Returns the writability status of the stream.  Always <b>true</b> for this class
        /// </summary>
        public override bool CanWrite
		{
			get
			{
				return true;
			}
		}


		private int ReadFromSocket( byte[] buffer, int offset, int length )
		{
			int bytesRead = base.Read( buffer, offset, length );
			SetStatus( buffer, bytesRead );
			return bytesRead;
		}

		internal int ReadButDontClear( byte[] buffer, int offset, int length )
		{
			if( bufferMemoryStream == null )
				bufferMemoryStream = new MemoryStream();
			int bytesRead = ReadFromSocket( buffer, offset, length );
			bufferMemoryStream.Write( buffer, offset, bytesRead );
			return bytesRead;
		}

        /// <summary>
        /// Reads data from the stream
        /// </summary>
        /// <param name="buffer">Target buffer into which data will be read</param>
        /// <param name="offset">Offset in the buffer at which the data will be placed</param>
        /// <param name="length">Number of bytes to read</param>
        /// <returns>Number of bytes actually read</returns>
		public override int Read( byte[] buffer, int offset, int length )
		{
			int bytesRead = 0;
			if( bufferMemoryStream != null )
			{
				// We need to read from the "cache" first
				bytesRead = bufferMemoryStream.Read( buffer, offset, length );
				if( bufferMemoryStream.Position >= bufferMemoryStream.Length )
					bufferMemoryStream = null;
				if( bytesRead >= length )
					return bytesRead; // We got all we needed from the buffer, else we need to fill up from socket too
			}
			bytesRead += ReadFromSocket( buffer, offset, length );
			return bytesRead;
		}
	}
}













