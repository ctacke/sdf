using System;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Win32;

namespace OpenNETCF.WindowsCE
{
    /// <summary>
    /// Time-Zone
    /// </summary>
    internal class TZREG
    {
        // Declare array of bytes to represent the structure
        // contents in a form which can be marshalled.
        private byte[] flatStruct = new byte[4 + 4 + 4 + 16 + 16];

        #region Flat structure offset constants
        private const int biasOffset = 0;
        private const int standardBiasOffset = 4;
        private const int daylightBiasOffset = 4 + 4;
        private const int standardDateOffset = 4 + 4 + 4;
        private const int daylightDateOffset = 4 + 4 + 4 + 16 /* sizeof( SYSTEMTIME ) */;
        #endregion

        public TZREG()
        {
        }

        public TZREG(byte[] bytes)
            :
            this(bytes, 0)
        {
        }

        public TZREG(byte[] bytes, int offset)
        {
            // Dump the byte array into our array.
            Buffer.BlockCopy(bytes, offset, flatStruct, 0, flatStruct.Length);
        }

        public byte[] ToByteArray()
        {
            return flatStruct;
        }

        public static implicit operator byte[](TZREG tzr)
        {
            return tzr.ToByteArray();
        }

        public int Bias
        {
            get
            {
                return BitConverter.ToInt32(flatStruct, biasOffset);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, flatStruct, biasOffset, 4);
            }
        }
        public int StandardBias
        {
            get
            {
                return BitConverter.ToInt32(flatStruct, standardBiasOffset);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, flatStruct, standardBiasOffset, 4);
            }
        }
        public int DaylightBias
        {
            get
            {
                return BitConverter.ToInt32(flatStruct, daylightBiasOffset);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, flatStruct, daylightBiasOffset, 4);
            }
        }
        public SystemTime StandardDate
        {
            get
            {
                return new SystemTime(flatStruct, standardDateOffset);
            }
            set
            {
                byte[] bytes = value.ToByteArray();
                Buffer.BlockCopy(bytes, 0, flatStruct, standardDateOffset, 16);
            }
        }
        public SystemTime DaylightDate
        {
            get
            {
                return new SystemTime(flatStruct, daylightDateOffset);
            }
            set
            {
                byte[] bytes = value.ToByteArray();
                Buffer.BlockCopy(bytes, 0, flatStruct, daylightDateOffset, 16);
            }
        }
    }
}
