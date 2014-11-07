
using System;
using System.IO;
using System.Text;

namespace OpenNETCF.Web.Services2.Dime 
{
    /// <summary>
    /// Represents the header and payload of a DIME record. DimeRecord is not directly creatable, they are
    /// created by DimeReader and DimeWriter when reading and writing DIME messages. 
    /// </summary>
    public class DimeRecord 
	{
     	const int BaseHeaderSize = 12; //8; mod
        const int MaxIdTypeLength = 0x1FFF;
        const int PaddingMultiple = 4;        
        const byte FlagMask = 0xE0;

        IOModeEnum m_ioMode;
        Stream m_stream;
        DimeStream m_dimeStream;
        MemoryStream m_bodyStreamBuffer; // only used for chunked

        bool m_chunked;
        bool m_firstChunk;
        bool m_headerWritten;
        bool m_beginOfMessage;
        bool m_endOfMessage;
        bool m_closed;
        int m_chunkSize;

        Uri m_id;        
        string m_type;
        TypeFormatEnum m_typeFormat;
        int m_contentLength;        

        int m_bytesReadWritten;

        /// <summary>
        ///  Creates a read-only DimeRecord object.  
        /// </summary>
        /// <param name="stream">Must be an open and readable stream.</param>
        internal DimeRecord (Stream stream) {	
            if (stream == null) throw new ArgumentNullException("stream");

            if (!stream.CanRead) throw new ArgumentException("Stream CanRead property must be true.", "stream");

            m_ioMode = IOModeEnum.ReadOnly;
            m_stream = stream;
            
            // Note, a blocking I/O call in the constructor. Consider delaying this to do it on-demand so
            // we can provide async semantics.
            ReadHeader();
        }

        /// <summary>
        /// Creates a write-only DimeRecord object.
        /// </summary>
        /// <param name="stream">stream must be open and writeable.</param>
        /// <param name="id">id must be null or a valid URI.</param>
        /// <param name="type">type must be a valid URI or a media type.</param>
        /// <param name="typeFormat"></param>
        /// <param name="contentLength">contentLength is the length of the records content.  If contentLength
        /// equals -1, chunking is used.</param>
        /// <param name="beginOfMessage">beginOfMessage marks this record as the first record in a message.</param>
        /// <param name="chunkSize"></param>
        internal DimeRecord (Stream stream, Uri id, String type, TypeFormatEnum typeFormat, bool beginOfMessage, int contentLength, int chunkSize) {
            if (stream == null) throw new ArgumentNullException("stream");
            if (type == null) throw new ArgumentNullException("type");
            if (!stream.CanWrite) throw new ArgumentException("Stream CanWrite property must be true.", "stream");
            if (contentLength < -1) throw new ArgumentException("contentLength must be -1 for chunked or a non-negative number", "contentLength");

            SetType(type, typeFormat);
            m_id = id;            
            m_contentLength = contentLength;
            m_chunked = contentLength == -1;
            m_firstChunk = m_chunked;
            m_beginOfMessage = beginOfMessage;
            m_stream = stream;                        
            m_ioMode = IOModeEnum.WriteOnly;            
            m_chunkSize = chunkSize;
        }

        /// <summary>
        /// A stream for reading or writing the payload data of the DIME record. This stream
        /// will read from or write to the stream passed to the DimeReader or DimeWriter. However
        /// it is a different Stream instance because it has a different position, open/closed state,
        /// etc. When writing chunked messages the stream may be partially buffered, otherwise
        /// the writes will go directly to the underlying stream.
        /// </summary>
        public Stream BodyStream { 
            get { 
                if (m_dimeStream == null) m_dimeStream = new DimeStream(this);
                return m_dimeStream; 
            } 
        }

        internal bool CanRead {
            get { 
                return ((m_ioMode == IOModeEnum.ReadOnly ? true : false) && !m_closed); 
            }
        }        

        internal bool CanWrite {
            get { 
                return ((m_ioMode == IOModeEnum.WriteOnly ? true : false) && !m_closed); 
            } 
        }      

        /// <summary>
        /// When reading a record this property indicates that this record is chunked.
        /// </summary>
        public bool Chunked {
            get { 
                if (CanWrite)
                    throw new InvalidOperationException("This property is only supported when reading a record.");
                return m_chunked; 
            }
        }

        /// <summary>
        /// Specifies the minimum size of a chunk when writing chunked payload DIME records.
        /// Essentially this is the amount of buffering that will occur before the chunked 
        /// payload is written to the underlying stream, the actual chunk size may be
        /// greater. The default value for ChunkSize is specified by DimeWriter.DefaultChunkSize.
        /// </summary>
        public int ChunkSize {
            get { return m_chunkSize; }
            
            set { 
                if (value <= 0) throw new ArgumentException("Chunk size must be greater than 0.");
                m_chunkSize = value;
            }
        }

        /// <summary>
        /// When writing specifies the payload length in bytes of the DIME record or -1 if the record
        /// is chunked. When reading indicates the length of the payload as specified in the
        /// DIME record header. 
        /// </summary>       
        public int ContentLength {
            get { return m_chunked ? -1 : m_contentLength; }
        }        


        /// <summary>
        /// When reading a record this property indicates that this record is the last one 
        /// in the message. When writing a record the message end flag is automatically set
        /// when the DimeWriter is closed.
        /// </summary>
        public bool EndOfMessage {
            get { 
                if (CanWrite)
                    throw new InvalidOperationException("This property is only supported when reading a record.");
                return m_endOfMessage; 
            }
			set
			{
				m_endOfMessage = true;
			}
        }

        /// <summary>
        /// Specifies the type of the data in the payload. The TypeFormat property should be used
        /// to interpret the Type property. For example if TypeFormat is TypeFormatEnum.MediaType
        /// then a valid Type would be "plain/text; charset=utf-8" or "image/jpeg".
        /// </summary>
        public String Type {
            get { return m_type; }
        } 

        /// <summary>
        /// Specifies what format the Type property is so that the format of the payload can be 
        /// understood.
        /// </summary>
        public TypeFormatEnum TypeFormat {
            get { return m_typeFormat; }
        }

        /// <summary>
        /// The id of the DIME record. Should be unique for all DIME records
        /// </summary>
        public Uri Id { get { return m_id; } }

        /// <summary>
        /// Closes the record. If writing a chunked record then any buffered
        /// data will be written out. DimeReader and DimeWriter will automatically
        /// close a record when starting to read or write the next one, or if
        /// DimeReader.Close() or DimeWriter.Close() is called.
        /// </summary>
        public void Close() {
            Close(false);
        }

        internal void Close(bool endOfMessage) {
            if (m_closed) return;

            m_closed = true;

            //if reading, dispose of remaining bytes
            if (m_ioMode == IOModeEnum.ReadOnly) {
                if (m_bytesReadWritten == m_contentLength)
                    return;
                byte[] buffer = new byte[m_contentLength];
				//BEGIN MOD
				int numRead = m_stream.Read(buffer, 0, buffer.Length);
				int remainder = numRead % 4;
				if(remainder != 0)
				{
					byte[] padding = new byte[remainder];
					numRead = m_stream.Read(buffer, 0, padding.Length);
				}
                //while (m_stream.Read(buffer, 0, buffer.Length) > 0) 
				//{
                //    ;
                //}
				//END MOD
            }
            else if (m_ioMode == IOModeEnum.WriteOnly) {
                if (m_chunked)
                    WriteChunkedPayload(true, endOfMessage);
                else if (endOfMessage)
                    WriteMessageEndRecord();
            }
        } 

		/// <summary>
		/// Writes the header to the stream.
		/// </summary>
        /// <param name="endOfRecord"></param>
        /// <param name="endOfMessage"></param>
        /// <param name="contentLength"></param>
        internal void WriteHeader(bool endOfRecord, bool endOfMessage, long contentLength) 
		{
			byte[] local9;

			byte[] buffer = new byte[BaseHeaderSize];
			byte[] typeBuffer = null;
			byte[] idBuffer = null;
			int byteCount = 0;

			FlagsEnum flags = 0;
			if (this.m_chunked && !(endOfRecord)) 
				flags = FlagsEnum.ChunkedRecord;            

			if (this.m_beginOfMessage) 
				flags |= FlagsEnum.BeginOfMessage;
			if (this.m_endOfMessage) //&& endOfRecord
				flags |= FlagsEnum.EndOfMessage;
			//else not begin or end
            
			TypeFormatEnum typeFormat;
			Uri id;
			if (!(this.m_chunked) || this.m_firstChunk) 
			{
				typeFormat = this.m_typeFormat;
				id = this.m_id;
			}
			else // middle-chunk
			{
				typeFormat = TypeFormatEnum.Unchanged;
				id = null;
				//this.m_type = null;
			}
            
			buffer[0] = (byte)((byte) 8 | (byte) flags);
			//buffer[0] = (byte) flags;
			buffer[1] = (byte) typeFormat;
			buffer[2] = 0;
			//buffer[2] = (byte)((byte)typeFormat << 5);     
			buffer[3] = 0;
			int length = 0;

			if (id != null && (length = id.AbsoluteUri.Length) > 0) 
			{
				byteCount = Encoding.ASCII.GetByteCount(id.AbsoluteUri);
				if (byteCount > MaxIdTypeLength)
					throw new Exception("The length of the encoded id exceeds 8191 bytes");
				idBuffer = new byte[PaddedCount(byteCount)];
				Encoding.ASCII.GetBytes(m_id.AbsoluteUri, 0, length, idBuffer, 0);
				(local9 = buffer)[4] = (byte)((byte) local9[4] | (byte) byteCount >> 8);
				buffer[5] = (byte) byteCount;
				//buffer[0] |= (byte)(byteCount >> 8);
				//buffer[1] = (byte)byteCount;
			}

			if (m_type != null && m_type.Length > 0) 
			{
				byteCount = Encoding.ASCII.GetByteCount(m_type);
				if (byteCount > MaxIdTypeLength)
					throw new Exception("The length of the encoded type exceeds 8191 bytes");
				typeBuffer = new byte[PaddedCount(byteCount)];
				Encoding.ASCII.GetBytes(m_type, 0, m_type.Length, typeBuffer, 0);
				(local9 = buffer)[6] = (byte)((byte) local9[6] | (byte) byteCount >> 8);
				buffer[7] = (byte) byteCount;
				//buffer[2] |= (byte)(byteCount >> 8);
				//buffer[3] = (byte)byteCount;
			}

			if (contentLength > (long) 0) 
			{
				buffer[8] = (byte) (contentLength >> 24);
				buffer[9] = (byte) (contentLength >> 16);
				buffer[10] = (byte) (contentLength >> 8);
				buffer[11] = (byte) contentLength;
			}

			//write base header to stream
			m_stream.Write(buffer, 0, BaseHeaderSize);
			if (idBuffer != null && idBuffer.Length > 0)
				m_stream.Write(idBuffer, 0, idBuffer.Length);
			if (typeBuffer != null && typeBuffer.Length > 0)
				m_stream.Write(typeBuffer, 0, typeBuffer.Length);
			m_firstChunk = false;
		}      

        /// <summary>
        /// Reads the header's properties from a stream
        /// </summary>
		internal void ReadHeader() 
		{ 
			//1st 3 (4-byte) chunks are
			//5-Version 1-MB 1-ME 1-ChunkFlag 4-type_t 4-reserved 16-optionsLength
			//16-idLength 16-typeLength
			//32-data length
			
			byte[] buffer = new byte[BaseHeaderSize];
			ForceRead(m_stream, buffer, BaseHeaderSize);
			//check version
			byte version = (byte) (buffer[0] & 248);
			if (version != 8) //WTF
			{
				object [] local13 = new Object[2];
				local13[0] = version >> 3;
				local13[1] = 1;
				throw new Exception("dime_WrongVersion");
			}
			//get flags
			FlagsEnum flags = (FlagsEnum) (buffer[0] & 7); //FlagMask
			m_beginOfMessage = (flags & FlagsEnum.BeginOfMessage) != 0;
			m_endOfMessage = (flags & FlagsEnum.EndOfMessage) != 0;
			m_chunked = (flags & FlagsEnum.ChunkedRecord) != 0;
			//get type flag
			//m_typeFormat = (TypeFormatEnum) ((buffer[2] & FlagMask) >> 5); 
			TypeFormatEnum lTypeFormat = (TypeFormatEnum) (buffer[1] & 240);
			if (lTypeFormat != 0)
				this.m_typeFormat = lTypeFormat;
			//check reserved
			int reserved = buffer[1] & 15;
			if (reserved != 0)
				throw new Exception("dime_RESRVDMustBeZero");
			//get options length
			int optionsLength = (buffer[2] << 8) + buffer[3];
			//get id length
			int idLength = (buffer[4] << 8) + buffer[5];
			//get type length
			int typeLength = (buffer[6] << 8) + buffer[7];
			//get content length;
			//this.m_contentLength = buffer[8] << 24;
			//this.m_contentLength = this.m_contentLength + buffer[9] << 16;
			//this.m_contentLength = this.m_contentLength + buffer[10] << 8;
			//this.m_contentLength = this.m_contentLength + buffer[11];
			//this.m_contentLength = (int) BitConverter.ToInt32(buffer, 8);
			int n = 0;
			for (int i=8;i<11;i++)
				n = (n | buffer[i]) << 8;
			n = n | buffer[11];
			this.m_contentLength = n;
			//content length
			this.m_chunkSize = this.m_contentLength;

			//get options
			if (optionsLength > 0) 
			{
				//the number of bytes received must be a multiple of 4.
				buffer = new byte[optionsLength + (((optionsLength % PaddingMultiple) > 0)?(PaddingMultiple - (optionsLength % PaddingMultiple)):0)];
				ForceRead(m_stream, buffer, buffer.Length);
				string value = Encoding.ASCII.GetString(buffer, 0, optionsLength);
				//TODO do something with this
			}

			//get id
			if (idLength > 0) 
			{
				//the number of bytes received must be a multiple of 4.
				buffer = new byte[idLength + (((idLength % PaddingMultiple) > 0)?(PaddingMultiple - (idLength % PaddingMultiple)):0)];
				ForceRead(m_stream, buffer, buffer.Length);
				string value = Encoding.ASCII.GetString(buffer, 0, idLength);
				m_id = new Uri(value);
			}

			//get type
			if (typeLength > 0) 
			{
				//the number of bytes received must be a multiple of 4.
				buffer = new byte[typeLength + (((typeLength % PaddingMultiple) > 0)?(PaddingMultiple - (typeLength % PaddingMultiple)):0)];
				ForceRead(m_stream, buffer, buffer.Length);
				string value = Encoding.ASCII.GetString(buffer, 0, typeLength);
				m_type = value;
			}
			
			//TODO CheckValid(type, id, typeFormat);
		} 

        private void ForceRead(Stream stream, byte[] buffer, int length) {
            int totalRead = 0;
            while (totalRead < length) {
                int read = stream.Read(buffer, totalRead, length - totalRead);
                if (read == 0) {
                    throw new Exception("Unable to read required bytes from stream.");
                }
                totalRead += read;
            }
        }

        internal int ReadBody(byte[] buffer, int offset, int count) {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || count < 0)
                throw new ArgumentOutOfRangeException();
            if (m_closed)
                throw new Exception("Stream is closed");
            if (m_ioMode != IOModeEnum.ReadOnly)
                throw new InvalidOperationException("Read not allowed");

            int bytesRead = 0;
            int bytesLeft = m_contentLength - m_bytesReadWritten;
            if (m_chunked && bytesLeft == 0 ) {
                //read next chunked record header, discarding empty records.
                do {
                    ReadHeader();
                } while(m_contentLength == 0 && m_chunked);

                bytesLeft = m_contentLength;
                m_bytesReadWritten = 0;
            }
            if (bytesLeft > 0) {
                bytesRead = m_stream.Read(buffer, offset, (bytesLeft < count) ? bytesLeft : count);
                m_bytesReadWritten += bytesRead;
                if (m_bytesReadWritten == m_contentLength)
                    ReadPadding(m_stream, m_bytesReadWritten);                
            }
            return bytesRead;
        }


        internal void SetType(string type, TypeFormatEnum typeFormat) {            
            switch (typeFormat) {
                case TypeFormatEnum.Unchanged:
                    throw new ArgumentException("The typeFormat parameter must not be TypeFormatEnum.Unchanged", "typeFormat");
                    //break;
                case TypeFormatEnum.MediaType:
                    if (type == null || type.Length == 0)
                        throw new ArgumentException("Media type name must be greater than length zero.", "type");                                        
                    break;
                case TypeFormatEnum.AbsoluteUri:
                    try { Uri u = new Uri(type); }
                    catch (Exception e) {
                        //throw new ArgumentExceptionString.Format("Invalid uri format for uri: '{0}'", type), "type", e);
						throw new ArgumentException("Invalid uri format for uri: " + type + "exc: " + e.ToString(), "type");
                    }                    
                    break;
                case TypeFormatEnum.None:
                    if (type != null || type.Length != 0)
                        throw new ArgumentException("Type name for TypeFormatEnum.None must be null or zero-length.", "type");                    
                    break;
                case TypeFormatEnum.Unknown:                    
                    break;
            }
            m_typeFormat = typeFormat;
            m_type = type;
        }

        private int PaddedCount(int byteCount) {
            return byteCount + (((byteCount % PaddingMultiple) > 0) ? (PaddingMultiple - (byteCount % PaddingMultiple)) : 0);
        }

        internal void WriteMessageEndRecord() {            
            /*
			//MOD
			m_stream.WriteByte((byte)FlagsEnum.EndOfMessage);
            m_stream.WriteByte((byte)0); // id length
            m_stream.WriteByte((byte)0x80); // TNF=None is 1000 0000
            // The rest of the header is just zero
            for (int i = 0; i < BaseHeaderSize - 3; i++) {
                m_stream.WriteByte(0);
            } 
			*/           
        }

        internal void WriteBody(byte[] buffer, int offset, int count) {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (count < 0 || offset < 0)
                throw new ArgumentOutOfRangeException("offset must be >= 0.  count must be >= 0");
            if (m_closed)
                throw new InvalidOperationException("Stream is closed");
            if (m_ioMode != IOModeEnum.WriteOnly)
                throw new InvalidOperationException("Write not allowed");

            if (m_chunked) {
                if (count > ChunkSize) {
                    // TODO: could make this more efficient by combining the buffered writes with
                    // TODO: the current one into a single chunk.
                    if (m_bodyStreamBuffer != null && m_bodyStreamBuffer.Length > 0)
                        WriteChunkedPayload(false, false);            
                    WriteChunkedPayload(false, false, buffer, offset, count);
                }
                else {
                    if (m_bodyStreamBuffer == null)
                        m_bodyStreamBuffer = new MemoryStream(count < 512 ? 512 : count);               
                    m_bodyStreamBuffer.Write(buffer, offset, count);
                }
                m_bytesReadWritten += count;
            }
            else {
                if (m_bytesReadWritten + count > m_contentLength)
                    throw new Exception("The number of bytes written exceed the specified content length");

                if (!m_headerWritten) {
                    WriteHeader(false, false, m_contentLength);
                    m_headerWritten = true;
                }
                m_stream.Write(buffer, offset, count);                
                m_bytesReadWritten += count;
                if (m_bytesReadWritten == m_contentLength)
                    WritePadding(m_stream, m_bytesReadWritten);
            }                
        }

        private void WritePadding(Stream stream, int bytesWritten) {
            int padCount = PaddedCount(bytesWritten) - bytesWritten;
            for (int i = 0; i < padCount; i++) {
                stream.WriteByte(0);
            }
        }

        private void ReadPadding(Stream stream, int bytesRead) {
            int padCount = PaddedCount(bytesRead) - bytesRead;
            for (int i = 0; i < padCount; i++) {
                stream.ReadByte();
            }
        }    

        internal void WriteChunkedPayload(bool endOfRecord, bool endOfMessage) {                        
            if (m_bodyStreamBuffer != null) {                
                byte[] bytes = m_bodyStreamBuffer.GetBuffer();
                WriteChunkedPayload(endOfRecord, endOfMessage, bytes, 0, (int) m_bodyStreamBuffer.Length);
                m_bodyStreamBuffer.SetLength(0);            
            }
            else
                WriteChunkedPayload(endOfRecord, endOfMessage, null, 0, 0);
        }

        internal void WriteChunkedPayload(bool endOfRecord, bool endOfMessage, byte[] bytes, int offset, int count) {
            WriteHeader(endOfRecord, endOfMessage, count);
            if (bytes != null) {
                m_stream.Write(bytes, offset, count);       
                WritePadding(m_stream, count);
            }     
        }
    }
}