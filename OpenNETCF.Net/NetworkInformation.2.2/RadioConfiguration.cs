using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Describes configuration parameters for an 802.11 radio
    /// </summary>
    public class RadioConfiguration
    {
        internal byte[] m_data;
        internal int m_offset;

        private const int LengthOffset = 0;
        private const int BeaconPeriodOffset = 4;
        private const int ATIMWindowOffset = 8;
        private const int DSConfigOffset = 12;
        private const int FHConfigOffset = 16;

        internal RadioConfiguration(byte[] data)
        {
            m_data = data;
            m_offset = 0;
        }

        internal RadioConfiguration(byte[] data, int offset)
        {
            m_data = data;
            m_offset = offset;
        }

        internal uint Length
        {
            get
            {
                return BitConverter.ToUInt32(m_data, m_offset + LengthOffset);
            }
        }

        /// <summary>
        /// Specifies the interval between beacon message transmissions. This value is specified in K탎ec (1024 탎ec). 
        /// </summary>
        [CLSCompliant(false)]
        public uint BeaconPeriod
        {
            get
            {
                return BitConverter.ToUInt32(m_data, m_offset + BeaconPeriodOffset);
            }
        }

        /// <summary>
        /// Specifies the announcement traffic information message (ATIM) window in K탎ec (1024 탎ec). The ATIM window is a short time period immediately after the transmission of each beacon in an IBSS configuration. During the ATIM window, any station can indicate the need to transfer data to another station during the following data-transmission window. 
        /// </summary>
        [CLSCompliant(false)]
        public uint ATIMWindow
        {
            get
            {
                return BitConverter.ToUInt32(m_data, m_offset + ATIMWindowOffset);
            }
        }

        /// <summary>
        /// Specifies the frequency of the selected channel in kHz. 
        /// </summary>
        public int Frequency
        {
            get
            {
                return (int)BitConverter.ToUInt32(m_data, m_offset + DSConfigOffset);
            }
        }

        /// <summary>
        /// Specifies the hop pattern used to determine the hop sequence. As defined by the 802.11 standard, the layer management entity (LME) of the physical layer uses a hop pattern to determine the hop sequence. 
        /// </summary>
        [CLSCompliant(false)]
        public uint FrequencyHopPattern
        {
            get { return FHConfig.HopPattern; }
        }

        /// <summary>
        /// Specifies a set of patterns. The LME of the physical layer uses these patterns to determine the hop sequence
        /// </summary>
        [CLSCompliant(false)]
        public uint FrequencyHopSet
        {
            get { return FHConfig.HopSet; }
        }

        /// <summary>
        /// Specifies the maximum period of time during which the transmitter should remain fixed on a channel. This interval is described in K탎ec (1024 탎ec).
        /// </summary>
        [CLSCompliant(false)]
        public uint FrequencyHopDwellTime
        {
            get { return FHConfig.DwellTime; }
        }

        internal NDIS_802_11_CONFIGURATION_FH FHConfig
        {
            get
            {
                return new NDIS_802_11_CONFIGURATION_FH(m_data, m_offset + FHConfigOffset);
            }
        }
    }
}
