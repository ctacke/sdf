
using System;
using System.IO;

namespace OpenNETCF.Web.Services2.Dime {

    /// <summary>
    /// Reads DIME messages from a stream as a series of DIME records.
    /// </summary>
    public class DimeReader {
        Stream m_stream; 
        DimeRecord m_currentRecord;
        bool m_closed;

        /// <summary>
        /// Creates a DimeMessageReader object 
        /// </summary>
        /// <param name="stream">Must be an open readable stream.</param>
        public DimeReader(Stream stream) {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new Exception("Cannot read from stream.");

            m_stream = stream;
        }

        /// <summary>
        /// Returns the next record in the DIME message in read-only mode. If the DimeReader is closed then
        /// an InvalidOperationException will occur. When the end of
        /// the message is reached ReadRecord will return null. ReadRecord is a blocking i/o call in that the 
        /// DIME record header is read before the method returns.
        /// </summary>
        /// <returns>
        /// A read-only DimeRecord, or null if it's the end of the DIME message. If the record has TNF=None
        /// and it is the last record in the message null is also returned.
        /// </returns>
        public DimeRecord ReadRecord() {
            if (m_closed)
                throw new InvalidOperationException("DimeReader is closed.");

            if (m_currentRecord != null) {
                if (m_currentRecord.EndOfMessage)
                    return null;
                m_currentRecord.Close();
            }
            
            m_currentRecord = new DimeRecord(m_stream);
            if (m_currentRecord.TypeFormat == TypeFormatEnum.None && m_currentRecord.EndOfMessage) {
                m_currentRecord.Close();
                return null;
            }
            return m_currentRecord;
        }

        /// <summary>
        /// Close discards the remaining records in the message before closing.
        /// Close is a blocking call if all records haven't been read since it will
        /// </summary>
        public void Close() {
            if (m_closed) return;
            if (m_currentRecord != null) {
                if (m_currentRecord.BodyStream.CanRead)
                    throw new InvalidOperationException("The previous record's stream is still open");

                //if not end of message, discard remaining records
                while (!m_currentRecord.EndOfMessage) {
                    if (ReadRecord() != null)
                        m_currentRecord.BodyStream.Close();
                }
                m_currentRecord.Close();
            }
            m_closed = true;
        }
    }
}