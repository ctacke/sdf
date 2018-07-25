
using System;
using System.IO;

namespace OpenNETCF.Web.Services2.Dime {
    /// <summary>
    /// Writes a DIME message as a series of DIME records to a stream.
    /// </summary>
    public class DimeWriter {
        const int ChunkSizeDefault = 1024;

        Stream m_stream; 
        DimeRecord m_currentRecord;  
        bool m_closed;
        bool m_firstRecord;
        int m_defaultChunkSize;

        /// <summary>
        /// Creates a DimeWriter to write a DIME message to the given stream.
        /// </summary>
        /// <param name="stream">stream must be an open writeable stream</param>
        public DimeWriter(Stream stream){
            if (null == stream)
                throw new ArgumentNullException("stream");
            if (!stream.CanWrite)
                throw new Exception("Cannot write to stream.");
            m_stream = stream;
            m_firstRecord = true;
            m_defaultChunkSize = ChunkSizeDefault;
        }


        /// <summary>
        /// Returns the next record to write in the DIME message. The record payload will be written
        /// in chunked mode since no contentLength is specified. When the DimeWriter is closed then the
        /// message end flag is automatically set on the last record.
        /// </summary>
        /// <param name="id">The unique identifier of the record in the form of a URI.</param>
        /// <param name="type">The type of payload data in the record. The format of this type is 
        /// specified by the typeFormat parameter. For example, if typeFormat is TypeFormatEnum.MediaType
        /// then a valid type is "plain/text; charset=utf-8"</param>
        /// <param name="typeFormat">The format of the type parameter.</param>
        /// <returns>a write-only DimeRecord.</returns>
        private DimeRecord CreateRecord(Uri id, string type, TypeFormatEnum typeFormat) {
            return CreateRecord(id, type, typeFormat, -1);
        }

        /// <summary>
        /// Returns the next record to write in the DIME message. If the length of the payload data is not
        /// known upfront then use contentLength==-1 to specify chunked records. Otherwise the amount of data
        /// written to the record must match the content length specified. When the DimeWriter is closed then the
        /// message end flag is automatically set on the last record. If writing in non-chunked mode then
        /// an empty terminating record with TNF=TypeFormatEnum.None will be automatically sent when the
        /// DimeWriter is closed.
        /// </summary>
        /// <param name="id">The unique identifier of the record in the form of a URI.</param>
        /// <param name="type">The type of payload data in the record. The format of this type is 
        /// specified by the typeFormat parameter. For example, if typeFormat is TypeFormatEnum.MediaType
        /// then a valid type is "plain/text; charset=utf-8"</param>
        /// <param name="typeFormat">The format of the type parameter.</param>
        /// <param name="contentLength">The count of bytes to be written to the DIME record, or -1 to specify
        /// writing a chunked payload record.
        /// </param>
        /// <returns>A write-only DimeRecord.</returns>
        private DimeRecord CreateRecord(Uri id, string type, TypeFormatEnum typeFormat, int contentLength) {
            if (type == null) throw new ArgumentNullException("type");
            if (m_closed) throw new InvalidOperationException("The writer is closed.");

            // if a record currently exists, send it.
            if (m_currentRecord != null)
                m_currentRecord.Close(false);

            m_currentRecord = new DimeRecord(m_stream, id, type, typeFormat, m_firstRecord, contentLength, m_defaultChunkSize);
            m_firstRecord = false;
			if(m_lastRecord == true)
				m_currentRecord.EndOfMessage = true;

            return m_currentRecord;
        }

		private bool m_lastRecord = false;
		public DimeRecord NewRecord(string id, string type, TypeFormatEnum typeFormat, int contentLength) 
		{
			Uri daUri = new Uri(id);
			return CreateRecord(daUri, type, typeFormat, contentLength);
		}

		public DimeRecord LastRecord(string id, string type, TypeFormatEnum typeFormat, int contentLength) 
		{
			m_lastRecord = true;
			Uri daUri = new Uri(id);
			return CreateRecord(daUri, type, typeFormat, contentLength);
		}

        /// <summary>
        /// Specifies the default chunk size in bytes when writing chunked payload DIME records. The default
        /// value for the DefaultChunkSize is 1024 bytes.
        /// </summary>
        public int DefaultChunkSize {
            get { return m_defaultChunkSize; }

            set { 
                if (value <= 0)
                    throw new ArgumentException("Default chunk size must be greater than zero.");
                m_defaultChunkSize = value;
            }
        }

        /// <summary>
        /// Closes the DIME message. If writing chunked payloads then any buffered data is written to
        /// the underlying stream and the chunked record and message end flags are set. If writing
        /// non-chunked then a terminating empty record with TNF=TypeFormatEnum.None is written to 
        /// end the DIME message..
        /// </summary>
        public void Close() {
            if (m_closed) return;

            // if a record currently exists, send it.
            if (m_currentRecord != null) {
                m_currentRecord.Close(true);
                m_currentRecord = null;
            }
            m_closed = true;
        }
    }
}