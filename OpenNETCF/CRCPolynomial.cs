using System;

namespace OpenNETCF
{
    [CLSCompliant(false)]
    public enum CRCPolynomial : ulong
    {
        CRC_8 = 0xE0,
        CRC_10 = 0x233,
        CRC_12 = 0x80F,
        CRC_16 = 0xA001,
        CRC_16ARC = 0x8005,
        CRC_24 = 0x1864CFB,
        CRC_CCITT16 = 0x1021,
        CRC_32 = 0x04C11DB7,
        CRC_CCITT32 = 0xEDB88320,
        CRC_64 = 0x1B
    }
}
