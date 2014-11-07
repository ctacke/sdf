using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    internal class NDIS_802_11_CONFIGURATION_FH
    {
        internal byte[] data;
        internal int offset;

        protected const int LengthOffset = 0;
        protected const int HopPatternOffset = 4;
        protected const int HopSetOffset = 8;
        protected const int DwellTimeOffset = 12;

        public NDIS_802_11_CONFIGURATION_FH(byte[] d, int o)
        {
            data = d;
            offset = o;
        }

        public uint Length
        {
            get
            {
                return BitConverter.ToUInt32(data, offset + LengthOffset);
            }
        }

        public uint HopPattern
        {
            get
            {
                return BitConverter.ToUInt32(data, offset + HopPatternOffset);
            }
        }

        public uint HopSet
        {
            get
            {
                return BitConverter.ToUInt32(data, offset + HopSetOffset);
            }
        }

        /// <summary>
        /// Amount of time dwelling in each frequency (in kusec).
        /// </summary>
        public uint DwellTime
        {
            get
            {
                return BitConverter.ToUInt32(data, offset + DwellTimeOffset);
            }
        }
    }
}
