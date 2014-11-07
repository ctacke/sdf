using System;
using System.IO;

namespace OpenNETCF
{
    /// <summary>
    /// Class used for generating Cyclic Redundancy Check (CRC) Hashes
    /// </summary>
    [CLSCompliant(false)]
    public static class CRC
    {
        private static ulong m_polynomial = ulong.MaxValue;
        private static ulong[] m_table;

        /// <summary>
        /// Generates a CCITT32 CRC of the data
        /// </summary>
        /// <param name="data">Data to generate a CRC for</param>
        /// <returns>The CRC</returns>
        public static ulong GenerateChecksum(byte[] data)
        {
            return GenerateChecksum(data, 32, CRCPolynomial.CRC_CCITT32);
        }

        /// <summary>
        /// Generates a CRC using a common polynomial for the next closest bitlength above that specified.
        /// For example, a bit length of 13 through 16 will use the CCITT16 polynomial masking off bits necessary to maintain the specified bitlength.
        /// </summary>
        /// <param name="data">Data to generate a CRC for</param>
        /// <param name="offset">The offset into the source data at which to start the CRC</param>
        /// <param name="length">The number of bytes in the source data to analyze</param>
        /// <returns>The CRC</returns>
        /// <returns></returns>
        public static ulong GenerateChecksum(byte[] data, int offset, int length)
        {
            return GenerateChecksum(data, offset, length, CRCPolynomial.CRC_CCITT32, 0);
        }

        /// <summary>
        /// Generates a CRC using a common polynomial for the next closest bitlength above that specified.
        /// For example, a bit length of 13 through 16 will use the CCITT16 polynomial masking off bits necessary to maintain the specified bitlength.
        /// </summary>
        /// <param name="data">Data to generate a CRC for</param>
        /// <param name="crcBitLength">Length, in bits, of the polynomial to use</param>
        /// <returns>The CRC</returns>
        public static ulong GenerateChecksum(byte[] data, int crcBitLength)
        {
            if ((crcBitLength < 8) || (crcBitLength > 64))
                throw new ArgumentOutOfRangeException("Only Bit Length of 8 to 64 is supported");

            if (crcBitLength == 8)
            {
                return GenerateChecksum(data, 32, CRCPolynomial.CRC_8);
            }
            else if (crcBitLength <= 10)
            {
                return GenerateChecksum(data, 32, CRCPolynomial.CRC_10);
            }
            else if (crcBitLength <= 12)
            {
                return GenerateChecksum(data, 32, CRCPolynomial.CRC_12);
            }
            else if (crcBitLength <= 16)
            {
                return GenerateChecksum(data, 32, CRCPolynomial.CRC_CCITT16);
            }
            else if (crcBitLength <= 24)
            {
                return GenerateChecksum(data, 32, CRCPolynomial.CRC_24);
            }
            else if (crcBitLength <= 32)
            {
                return GenerateChecksum(data, 32, CRCPolynomial.CRC_CCITT32);
            }
            else
            {
                return GenerateChecksum(data, 32, CRCPolynomial.CRC_64);
            }
        }

        public static ulong GenerateChecksum(byte[] data, int crcBitLength, CRCPolynomial polynomial)
        {
            return GenerateChecksum(data, crcBitLength, (ulong)polynomial, 0);
        }

        public static ulong GenerateChecksum(byte[] data, int crcBitLength, ulong polynomial)
        {
            return GenerateChecksum(data, crcBitLength, polynomial, 0);
        }

        public static ulong GenerateChecksum(byte[] data, int crcBitLength, ulong polynomial, ulong seedCRC)
        {
            return GenerateChecksum(data, 0, data.Length, crcBitLength, polynomial, seedCRC);
        }

        /// <summary>
        /// Generates a CRC using a common polynomial for the next closest bitlength above that specified.
        /// For example, a bit length of 13 through 16 will use the CCITT16 polynomial masking off bits necessary to maintain the specified bitlength.
        /// </summary>
        /// <param name="data">Data to generate a CRC for</param>
        /// <param name="offset">The offset into the source data at which to start the CRC</param>
        /// <param name="length">The number of bytes in the source data to analyze</param>
        /// <param name="polynomial">Standard polynomial to use for the CRC</param>
        /// <param name="seedCRC">CRC seed value</param>
        /// <returns>The CRC</returns>
        /// <returns></returns>
        public static ulong GenerateChecksum(byte[] data, int offset, int length, CRCPolynomial polynomial, ulong seedCRC)
        {
            int bits = 0;
            switch (polynomial)
            {
                case CRCPolynomial.CRC_8:
                    bits = 8;
                    break;
                case CRCPolynomial.CRC_10:
                    bits = 10;
                    break;
                case CRCPolynomial.CRC_12:
                    bits = 12;
                    break;
                case CRCPolynomial.CRC_16:
                case CRCPolynomial.CRC_16ARC:
                case CRCPolynomial.CRC_CCITT16:
                    bits = 16;
                    break;
                case CRCPolynomial.CRC_24:
                    bits = 24;
                    break;
                case CRCPolynomial.CRC_CCITT32:
                case CRCPolynomial.CRC_32:
                    bits = 32;
                    break;
                case CRCPolynomial.CRC_64:
                    bits = 64;
                    break;
            }

            return GenerateChecksum(data, offset, length, bits, (ulong)polynomial, seedCRC);
        }
        
        /// <summary>
        /// Generates a CRC using a common polynomial for the next closest bitlength above that specified.
        /// For example, a bit length of 13 through 16 will use the CCITT16 polynomial masking off bits necessary to maintain the specified bitlength.
        /// </summary>
        /// <param name="data">Data to generate a CRC for</param>
        /// <param name="offset">The offset into the source data at which to start the CRC</param>
        /// <param name="length">The number of bytes in the source data to analyze</param>
        /// <param name="crcBitLength">Length, in bits, of the polynomial to use</param>
        /// <param name="polynomial">Custom polynomial to use for the CRC</param>
        /// <param name="seedCRC">CRC seed value</param>
        /// <returns>The CRC</returns>
        /// <returns></returns>
        public static ulong GenerateChecksum(byte[] data, int offset, int length, int crcBitLength, ulong polynomial, ulong seedCRC)
        {
            if ((crcBitLength < 8) || (crcBitLength > 64))
                throw new ArgumentOutOfRangeException("Only Bit Length of 8 to 64 is supported");

            ulong crc = seedCRC;
            ulong top = 0;
            int shiftval = crcBitLength - 8;
            ulong mask = 0;
            for (int i = 0; i < crcBitLength; i++)
            {
                mask |= 1UL << i;
            }

            GenerateCRCTable(crcBitLength, polynomial);
            for (int i = offset; i < length; i++)
            {
                top = crc >> shiftval;
                top ^= data[i];
                crc = ((crc << 8) ^ m_table[top]) & mask;
                // System.Diagnostics.Debug.Print(string.Format("  top: {0} crc: {1}", top, crc)); 
            }

            return crc;
        }

        /// <summary>
        /// Generates a CRC for the data in an open data stream
        /// </summary>
        /// <param name="stream">Stream to use as a source for the data</param>
        /// <param name="crcBitLength"></param>
        /// <param name="polynomial">Custom Polynomial or <c>CRCPolynomial</c> to use in the CRC</param>
        /// <returns>The data's CRC</returns>
        public static ulong GenerateChecksum(Stream stream, int crcBitLength, ulong polynomial)
        {
            if ((crcBitLength < 8) || (crcBitLength > 64))
                throw new ArgumentOutOfRangeException("Only Bit Length of 8 to 64 is supported");

            if(!stream.CanRead)
            {
                throw new ArgumentException("Unreadable stream");
            }

            if(stream.Length == 0)
            {
                throw new ArgumentException("Stream is empty");
            }

            ulong crc = 0;
            
            GenerateCRCTable(crcBitLength, polynomial);

            byte[] buffer = new byte[512];
            int read = 0;
            while (stream.Position < stream.Length)
            {
                read = stream.Read(buffer, 0, buffer.Length);
                crc = GenerateChecksum(buffer, 0, read, crcBitLength, polynomial, crc);
                // System.Diagnostics.Debug.WriteLine(string.Format("CRC 0x{0:x}", crc.ToString()));
            }
            return crc;
        }

        private static void GenerateCRCTable(int bits, ulong polynomial)
        {
            if (bits <= 0) throw new ArgumentOutOfRangeException("bits");

            if (m_polynomial == polynomial)
                return;

            ulong mask = 0;
            for (int i = 0; i < bits; i++)
            {
                mask |= (1ul << i);
            }

            // generate a table only if the polynomial has changed
            m_table = new ulong[256];

            for (ulong i = 0; i < 256; i++)
            {
                ulong register = i;

                for (int bit = 0; bit < 8; bit++)
                {
                    if ((register & 1ul) != 0)
                    {
                        register >>= 1;
                        register ^= polynomial;
                    }
                    else
                        register >>= 1;
                }
                register &= mask;
                m_table[i] = register;
            }
        }
    }
}
