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
        /// Specifies the interval between beacon message transmissions. This value is specified in K�sec (1024 �sec). 
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
        /// Specifies the announcement traffic information message (ATIM) window in K�sec (1024 �sec). The ATIM window is a short time period immediately after the transmission of each beacon in an IBSS configuration. During the ATIM window, any station can indicate the need to transfer data to another station during the following data-transmission window. 
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
        /// Specifies the maximum period of time during which the transmitter should remain fixed on a channel. This interval is described in K�sec (1024 �sec).
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
