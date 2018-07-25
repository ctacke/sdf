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
    // NOTE: as of 3/10/09 the MSDN docs on this online are *WRONG*.  Below is a paste from the actual wzcsapi.h header

    internal class BSSID
    {
        internal byte[] data;
        internal int offset;

        protected int WZC_CTLFLAGS_OFFSET = 0;

        protected int LengthOffset = 0;
        protected int MacAddressOffset = 0;
        protected int ReservedOffset = 0;
        protected int SsidOffset = 0;
        protected int PrivacyOffset = 0;	// The ssid length plus 32 character array.
        protected int RssiOffset = 0;
        protected int NetworkTypeInUseOffset = 0;
        protected int ConfigurationOffset = 0;	// It's an enum.  I hope it's four bytes.
        protected int InfrastructureModeOffset = 0;	// It's a structure, with another structure inside it.
        protected int SupportedRatesOffset = 0;	// Another enum.
        protected int KeyIndexOffset = 0;
        protected int KeyLengthOffset = 0;
        protected int KeyMaterialOffset = 0;
        protected int AuthenticationModeOffset = 0;

        public BSSID(byte[] d, int o, bool isWzc)
        {
            data = d;
            offset = o;

            // WZC geniously inserts a 4-byte item right after the length (see WZC_WLAN_CONFIG in wzcsapi.h)
            WZC_CTLFLAGS_OFFSET = (isWzc) ? 4 : 0;

            LengthOffset = 0;
            MacAddressOffset = LengthOffset + WZC_CTLFLAGS_OFFSET + 4;
            ReservedOffset = MacAddressOffset + 6;
            SsidOffset = ReservedOffset + 2;
            PrivacyOffset = SsidOffset + 36;	// The ssid length plus 32 character array.
            RssiOffset = PrivacyOffset + 4;
            NetworkTypeInUseOffset = RssiOffset + 4;
            ConfigurationOffset = NetworkTypeInUseOffset + 4;	// It's an enum.  I hope it's four bytes.
            InfrastructureModeOffset = ConfigurationOffset + 32;	// It's a structure, with another structure inside it.
            SupportedRatesOffset = InfrastructureModeOffset + 4;	// Another enum.
            KeyIndexOffset = SupportedRatesOffset + 8;
            KeyLengthOffset = KeyIndexOffset + 4;
            KeyMaterialOffset = KeyLengthOffset + 4;
            AuthenticationModeOffset = KeyMaterialOffset + 32;
        }

        public uint Length
        {
            get { return BitConverter.ToUInt32(data, offset + LengthOffset); }
        }

        public byte[] MacAddress
        {
            get
            {
                byte[] b = new byte[6];
                Array.Copy(data, offset + MacAddressOffset, b, 0, 6);
                return b;
            }
        }

        public String SSID
        {
            get
            {
                // Get the string length from the first four bytes.
                int c = BitConverter.ToInt32(data, offset + SsidOffset);

                // There are some adapters which cause the base
                // GetString() call below to fail because of the
                // value returned by them as the length of the 
                // SSID string.  We try to guess at what values
                // might make sense and so something that might
                // make sense also in those cases.

                // If, by chance, the length is negative, then do
                // something reasonable.
                if (c == -1)
                {
                    c = 32;	// 32 is the maximum length, so pick
                    // that value.
                }
                else if (c < 0)
                {
                    // At a guess, perhaps some drivers are returning
                    // the negative of the actual length, for some
                    // reason.
                    c = -c;
                }

                // Final range check.
                if (c > 32)
                {
                    // There is no valid value of length which
                    // is greater than 32.
                    return null;
                }

                // Convert the rest of the SSID stuff to a string.
                return System.Text.Encoding.ASCII.GetString(data, offset + SsidOffset + 4, c);
            }
        }

        public int Privacy
        {
            get { return BitConverter.ToInt32(data, offset + PrivacyOffset); }
        }

        public int Rssi
        {
            get
            {
                // There seems to be some confusion in the 
                // drivers about whether this should be a
                // negative number or not.  If it's greater
                // than zero, we negate it.
                int db = BitConverter.ToInt32(data, offset + RssiOffset);
                if (db > 0)
                    return -db;
                else
                    return db;
            }
            set
            {
                Buffer.BlockCopy(BitConverter.GetBytes(value), 0,
                    data, offset + RssiOffset, 4);
            }
        }

        public NetworkType NetworkTypeInUse
        {
            get
            {
                return (NetworkType)BitConverter.ToInt32(data, offset + NetworkTypeInUseOffset);
            }
        }

        public RadioConfiguration Configuration
        {
            get
            {
                return new RadioConfiguration(data, offset + ConfigurationOffset);
            }
        }

        public InfrastructureMode InfrastructureMode
        {
            get
            {
                return (InfrastructureMode)BitConverter.ToInt32(data, offset + InfrastructureModeOffset);
            }
        }

        public byte[] SupportedRates
        {
            get
            {
                byte[] b = new byte[8];
                Array.Copy(data, offset + SupportedRatesOffset, b, 0, 8);
                return b;
            }
        }

        public AuthenticationMode AuthenticationMode
        {
            get { return (AuthenticationMode)BitConverter.ToInt32(data, offset + AuthenticationModeOffset); }
        }
    }
}
