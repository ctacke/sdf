
using System;
using System.IO;
using System.Diagnostics;

namespace OpenNETCF.Web.Services2.Dime {
    /// <summary>
    /// DimeStream is used to read or write the record payload data through a Stream
    /// interface. The DimeStream delegates all of its work back to DimeRecord.
    /// </summary>
    internal class DimeStream : Stream {          
        DimeRecord m_dimeRecord;

        /// <summary>
        /// Creates a DimeStream object
        /// </summary>
        /// <param name="dimeRecord">A valid header for the DIME record.</param>
        internal DimeStream (DimeRecord dimeRecord) {
            m_dimeRecord = dimeRecord;            
        }

        public override bool CanSeek { get { return false;} }

        public override long Length { get { throw new NotSupportedException("Length"); } }

        public override long Position {
            get { throw new NotSupportedException("Position"); }
            set { throw new NotSupportedException("Position"); }
        }

        public override bool CanRead {
            get { return m_dimeRecord.CanRead; }
        }        

        public override bool CanWrite {
            get { return m_dimeRecord.CanWrite; }
        }      

        /// <summary>
        /// Reads data from the stream.  Returns 0 when the end of the record is reached.  
        /// For chunked records, read will continue to read data, discarding the chunked 
        /// record headers until the end of the chunked record set is reached. If Read() 
        /// is called when the stream is closed, an exception is thrown.
        /// </summary>
        /// <param name="buffer">The buffer the data is copied into.</param>
        /// <param name="offset">The offset in the buffer to begin copying to.</param>
        /// <param name="count">The number of bytes to copy.</param>
        /// <returns>The number of bytes copied.</returns>
        public override int Read(byte[] buffer, int offset, int count) {
            return m_dimeRecord.ReadBody(buffer, offset, count);
        }
            
        /// <summary>
        /// Writes to the stream.  If ContentLength is exceeded, an exception is thrown. 
        /// For chunked records, each write will generate a new chunked record if enough
        /// bytes are available to exceed the chunk size. If Write() 
        /// is called when the stream is closed, an exception is thrown.
        /// </summary>
        /// <param name="buffer">The data to be written.</param>
        /// <param name="offset">The offset to write the data from.</param>
        /// <param name="count">The number of bytes to write.</param>
        public override void Write(byte[] buffer, int offset, int count) {
            m_dimeRecord.WriteBody(buffer, offset, count);
        }

        /// <summary>
        /// Closes the stream.  
        /// Read-only mode: If the number of bytes read is less than ContentLength, the remaining
        /// bytes are read from the stream and discarded.  
        /// Write-only mode: If number of bytes written doesn’t equal the ContentLength, an 
        /// exception is thrown.
        /// </summary>
        public override void Close() {
             m_dimeRecord.Close(false);
        }        
        
        public override void Flush() { 
        }        

        public override long Seek(long offset, SeekOrigin origin) { throw new NotSupportedException("Seek"); }        
        
        public override void SetLength(long value) { throw new NotSupportedException("SetLength"); }        
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, Object state) { throw new NotSupportedException("BeginWrite"); }  
        public override void EndWrite(IAsyncResult asyncResult) { throw new NotSupportedException("EndWrite"); }         
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, Object state) { throw new NotSupportedException("BeginRead"); }  
        public override int EndRead(IAsyncResult asyncResult) { throw new NotSupportedException("EndRead"); } 
    }
}