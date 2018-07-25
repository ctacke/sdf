using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    internal class BSSID
    {
        internal byte[] data;
        internal int offset;

        protected const int LengthOffset = 0;
        protected const int MacAddressOffset = LengthOffset + 4;
        protected const int ReservedOffset = MacAddressOffset + 6;
        protected const int SsidOffset = ReservedOffset + 2;
        protected const int PrivacyOffset = SsidOffset + 36;	// The ssid length plus 32 character array.
        protected const int RssiOffset = PrivacyOffset + 4;
        protected const int NetworkTypeInUseOffset = RssiOffset + 4;
        protected const int ConfigurationOffset = NetworkTypeInUseOffset + 4;	// It's an enum.  I hope it's four bytes.
        protected const int InfrastructureModeOffset = ConfigurationOffset + 32;	// It's a structure, with another structure inside it.
        protected const int SupportedRatesOffset = InfrastructureModeOffset + 4;	// Another enum.
        protected const int KeyIndexOffset = SupportedRatesOffset + 8;
        protected const int KeyLengthOffset = KeyIndexOffset + 4;
        protected const int KeyMaterialOffset = KeyLengthOffset + 4;
        protected const int AuthenticationModeOffset = KeyMaterialOffset + 32;

        public BSSID(byte[] d, int o)
        {
            data = d;
            offset = o;
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
