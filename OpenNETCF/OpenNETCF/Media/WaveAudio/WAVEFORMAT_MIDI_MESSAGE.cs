using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Media.WaveAudio
{
    //typedef struct _WAVEFORMAT_MIDI_MESSAGE {
    //  UINT32 DeltaTicks;
    //  DWORD MidiMsg;
    //} WAVEFORMAT_MIDI_MESSAGE;
    internal struct WAVEFORMAT_MIDI_MESSAGE
    {
        public uint DeltaTicks;
        public uint MidiMsg;

        public byte[] GetBytes()
        {
            byte[] data = new byte[8];
            Buffer.BlockCopy(BitConverter.GetBytes(DeltaTicks), 0, data, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(MidiMsg), 0, data, 4, 4);
            return data;
        }

        public static int Length { get { return 8; } }
    }
}
