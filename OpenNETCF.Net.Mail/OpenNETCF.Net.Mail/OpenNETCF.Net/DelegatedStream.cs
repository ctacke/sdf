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
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Net.Mail;
using System.IO;
using System.Net.Sockets;

namespace OpenNETCF.Net
{
    internal class DelegatedStream : Stream
    {
        // Fields
        private NetworkStream netStream;
        private Stream stream;

        // Methods
        protected DelegatedStream()
        {
        }

        protected DelegatedStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            this.stream = stream;
            this.netStream = stream as NetworkStream;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
            //if (!this.CanRead)
            //{
            //    throw new NotSupportedException(SR.GetString("ReadNotSupported"));
            //}
            //if (this.netStream != null)
            //{
            //    return this.netStream.UnsafeBeginRead(buffer, offset, count, callback, state);
            //}
            //return this.stream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
            //if (!this.CanWrite)
            //{
            //    throw new NotSupportedException(SR.GetString("WriteNotSupported"));
            //}
            //if (this.netStream != null)
            //{
            //    return this.netStream.UnsafeBeginWrite(buffer, offset, count, callback, state);
            //}
            //return this.stream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void Close()
        {
            this.stream.Close();
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            if (!this.CanRead)
            {
                throw new NotSupportedException(SR.GetString("ReadNotSupported"));
            }
            return this.stream.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            if (!this.CanWrite)
            {
                throw new NotSupportedException(SR.GetString("WriteNotSupported"));
            }
            this.stream.EndWrite(asyncResult);
        }

        public override void Flush()
        {
            this.stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!this.CanRead)
            {
                throw new NotSupportedException(SR.GetString("ReadNotSupported"));
            }
            return this.stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (!this.CanSeek)
            {
                throw new NotSupportedException(SR.GetString("SeekNotSupported"));
            }
            return this.stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            if (!this.CanSeek)
            {
                throw new NotSupportedException(SR.GetString("SeekNotSupported"));
            }
            this.stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!this.CanWrite)
            {
                throw new NotSupportedException(SR.GetString("WriteNotSupported"));
            }
            this.stream.Write(buffer, offset, count);
        }

        // Properties
        public Stream BaseStream
        {
            get
            {
                return this.stream;
            }
        }

        public override bool CanRead
        {
            get
            {
                return this.stream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this.stream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this.stream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                if (!this.CanSeek)
                {
                    throw new NotSupportedException(SR.GetString("SeekNotSupported"));
                }
                return this.stream.Length;
            }
        }

        public override long Position
        {
            get
            {
                if (!this.CanSeek)
                {
                    throw new NotSupportedException(SR.GetString("SeekNotSupported"));
                }
                return this.stream.Position;
            }
            set
            {
                if (!this.CanSeek)
                {
                    throw new NotSupportedException(SR.GetString("SeekNotSupported"));
                }
                this.stream.Position = value;
            }
        }
    }
}