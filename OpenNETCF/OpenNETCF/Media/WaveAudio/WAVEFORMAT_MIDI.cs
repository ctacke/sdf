using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Media.WaveAudio
{
    internal sealed class WAVEFORMAT_MIDI
    {
        public WAVEFORMAT_MIDI()
        {
            WAVEFORMATEX = new WaveFormat2();
            WAVEFORMATEX.Extra = new byte[10];
        }

        public WaveFormat2 WAVEFORMATEX { get; private set; }

        public uint USecPerQuarterNote 
        {
            get { return BitConverter.ToUInt32(WAVEFORMATEX.Extra, 0); }
            set { Buffer.BlockCopy(BitConverter.GetBytes(value), 0, WAVEFORMATEX.Extra, 0, 4); }
        }

        public uint TicksPerQuarterNote
        {
            get { return BitConverter.ToUInt32(WAVEFORMATEX.Extra, 4); }
            set { Buffer.BlockCopy(BitConverter.GetBytes(value), 0, WAVEFORMATEX.Extra, 4, 4); }
        }

        public byte[] GetBytes()
        {
            return WAVEFORMATEX.GetBytes();
        }
    }
}
