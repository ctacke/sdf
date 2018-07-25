using System;
using System.IO;

namespace OpenNETCF.Media.WaveAudio
{
	/// <summary>
	/// This class defines the format of waveform-audio data. 
	/// Only format information common to all waveform-audio data formats is included in this class. 
	/// For formats that require additional information, this class is included 
	/// as the first member in another class, along with the additional information
	/// </summary>
	/// <remarks>Equivalent to native <b>WAVEFORMATEX</b> structure.</remarks>
	public sealed class WaveFormat2
	{
		public FormatTag FormatTag;      
		public Int16 Channels;       
		public Int32 SamplesPerSec;  
		public Int32 AvgBytesPerSec; 
		public Int16 BlockAlign;     
		public Int16 BitsPerSample;  
		public Int16 Size;
        public byte[] Extra;
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public WaveFormat2()
		{
            Extra = new byte[] { };
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public WaveFormat2(byte[] data)
		{
			if ( data.Length < 16 )
				throw new InvalidOperationException("Not enough data");
			FormatTag = (FormatTag)BitConverter.ToInt16(data, 0);
			Channels =  BitConverter.ToInt16(data, 2);
			SamplesPerSec = BitConverter.ToInt32(data, 4);
			AvgBytesPerSec = BitConverter.ToInt32(data, 8);
			BlockAlign = BitConverter.ToInt16(data, 12);
			BitsPerSample = BitConverter.ToInt16(data, 14);
            if (data.Length == 16)
            {
                Size = 0;
                Extra = new byte[0];
                return;
            }
            Size = BitConverter.ToInt16(data, 16);
            Extra = new byte[Size];
            if (data.Length - 18 < Size)
                throw new InvalidOperationException("Not enough data");
            Buffer.BlockCopy(data, 18, Extra, 0, Size);
		}

        /// <summary>
        /// Default constructor
        /// </summary>
        public static WaveFormat2 FromStream(Stream stm)
        {
            byte[] data = new byte[18];
            stm.Read(data, 0, data.Length);
            WaveFormat2 fmt = new WaveFormat2();
            fmt.FormatTag = (FormatTag)BitConverter.ToInt16(data, 0);
            fmt.Channels = BitConverter.ToInt16(data, 2);
            fmt.SamplesPerSec = BitConverter.ToInt32(data, 4);
            fmt.AvgBytesPerSec = BitConverter.ToInt32(data, 8);
            fmt.BlockAlign = BitConverter.ToInt16(data, 12);
            fmt.BitsPerSample = BitConverter.ToInt16(data, 14);
            fmt.Size = BitConverter.ToInt16(data, 16);
            fmt.Extra = new byte[fmt.Size];
            stm.Read(fmt.Extra, 0, fmt.Size);
            return fmt;
        }

        public static WaveFormat2 GetPCMWaveFormat(int SamplesPerSecond, short Channels, short BitPerSample)
        {
            WaveFormat2 fmt = new WaveFormat2();
            fmt.Extra = new byte[0];
            fmt.BitsPerSample = BitPerSample;
            fmt.Channels = Channels;
            fmt.FormatTag = FormatTag.PCM;
            fmt.SamplesPerSec = SamplesPerSecond;
            fmt.BlockAlign = (short)(Channels * BitPerSample / 8);
            fmt.AvgBytesPerSec = SamplesPerSecond * Channels * BitPerSample / 8;
            return fmt;
        }

        public static WaveFormat2 GetPCMWaveFormat(SoundFormats SoundFormat)
        {
            WaveFormat2 fmt = new WaveFormat2();
            #region long format switch()
            switch (SoundFormat)
            {
                case SoundFormats.Mono16bit11kHz:
                    fmt.Channels = 1;
                    fmt.SamplesPerSec = 11025;
                    fmt.BitsPerSample = 16;
                    break;
                case SoundFormats.Mono16bit22kHz:
                    fmt.Channels = 1;
                    fmt.SamplesPerSec = 22050;
                    fmt.BitsPerSample = 16;
                    break;
                case SoundFormats.Mono16bit44kHz:
                    fmt.Channels = 1;
                    fmt.SamplesPerSec = 44100;
                    fmt.BitsPerSample = 16;
                    break;
                case SoundFormats.Mono8bit11kHz:
                    fmt.Channels = 1;
                    fmt.SamplesPerSec = 11025;
                    fmt.BitsPerSample = 8;
                    break;
                case SoundFormats.Mono8bit22kHz:
                    fmt.Channels = 1;
                    fmt.SamplesPerSec = 22050;
                    fmt.BitsPerSample = 8;
                    break;
                case SoundFormats.Mono8bit44kHz:
                    fmt.Channels = 1;
                    fmt.SamplesPerSec = 44100;
                    fmt.BitsPerSample = 8;
                    break;
                case SoundFormats.Stereo16bit11kHz:
                    fmt.Channels = 2;
                    fmt.SamplesPerSec = 11025;
                    fmt.BitsPerSample = 16;
                    break;
                case SoundFormats.Stereo16bit22kHz:
                    fmt.Channels = 2;
                    fmt.SamplesPerSec = 22050;
                    fmt.BitsPerSample = 16;
                    break;
                case SoundFormats.Stereo16bit44kHz:
                    fmt.Channels = 2;
                    fmt.SamplesPerSec = 44100;
                    fmt.BitsPerSample = 16;
                    break;
                case SoundFormats.Stereo8bit11kHz:
                    fmt.Channels = 2;
                    fmt.SamplesPerSec = 11025;
                    fmt.BitsPerSample = 8;
                    break;
                case SoundFormats.Stereo8bit22kHz:
                    fmt.Channels = 2;
                    fmt.SamplesPerSec = 22050;
                    fmt.BitsPerSample = 8;
                    break;
                case SoundFormats.Stereo8bit44kHz:
                    fmt.Channels = 2;
                    fmt.SamplesPerSec = 44100;
                    fmt.BitsPerSample = 8;
                    break;
            }
            #endregion long format switch()
            return GetPCMWaveFormat(fmt.SamplesPerSec, fmt.Channels, fmt.BitsPerSample);
        }

        /// <summary>
		/// Get bytes
		/// </summary>
		/// <returns>byte array representation of this instance</returns>
		public byte[] GetBytes()
		{
			byte[] data = new byte[18 + Size];

			BitConverter.GetBytes((ushort)FormatTag).CopyTo(data, 0);
			BitConverter.GetBytes(Channels).CopyTo(data, 2);
			BitConverter.GetBytes(SamplesPerSec).CopyTo(data, 4);
			BitConverter.GetBytes(AvgBytesPerSec).CopyTo(data, 8);
			BitConverter.GetBytes(BlockAlign).CopyTo(data, 12);
			BitConverter.GetBytes(BitsPerSample).CopyTo(data, 14);
			BitConverter.GetBytes(Size).CopyTo(data, 16);
            Buffer.BlockCopy(Extra, 0, data, 18, Size);

			return data;
		}

        public override string ToString()
        {
            return string.Format("{0} {1}x{2} {3}", FormatTag, SamplesPerSec, BitsPerSample, Channels == 1? "Mono": "Stereo");
        }

        public override bool Equals(object obj)
        {
            if (obj is WaveFormat2)
            {
                WaveFormat2 fmt = obj as WaveFormat2;
                return fmt.FormatTag == FormatTag && fmt.Channels == Channels && fmt.SamplesPerSec == SamplesPerSec && fmt.BitsPerSample == BitsPerSample;
            }
            return base.Equals(obj);
        }

        public static bool operator ==(WaveFormat2 x, WaveFormat2 y)
        {
            if ((object)x == null)
                return (object)y == null;
            return x.Equals(y);
        }

        public static bool operator !=(WaveFormat2 x, WaveFormat2 y)
        {
            if ((object)x == null)
                return (object)y != null;
            return !x.Equals(y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private enum MatchType
        {
            Bits = 1,
            Channels = 0x10,
            SamplesPerSecond = 0x100,
        }
        public static WaveFormat2 GetSupportedFormat(FormatTag tag, int SamplesPerSecond, short Channels, short BitsPerSample)
        {
            MatchType bestMatchType = 0;
            WaveFormat2 bestMatch = null;
            foreach (AcmDriverInfo driverInfo in ACMSupport.SupportedDrivers)
            {
                foreach (AcmFormatInfo fmtInfo in driverInfo.Formats)
                {
                    MatchType matchType = 0;
                    if (fmtInfo.FormatTag != tag)
                        continue;
                    if (fmtInfo.Format.BitsPerSample == BitsPerSample)
                        matchType |= MatchType.Bits;
                    if (fmtInfo.Format.Channels == Channels)
                        matchType |= MatchType.Channels;
                    if (fmtInfo.Format.SamplesPerSec == SamplesPerSecond)
                        matchType |= MatchType.SamplesPerSecond;
                    if ((int)matchType > (int)bestMatchType)
                    {
                        bestMatch = fmtInfo.Format;
                        bestMatchType = matchType;
                    }
                }
            }

            return bestMatch;
        }

        public void CopyFrom(WaveFormat2 wf)
        {
            this.AvgBytesPerSec = wf.AvgBytesPerSec;
            this.BitsPerSample = wf.BitsPerSample;
            this.BlockAlign = wf.BlockAlign;
            this.Channels = wf.Channels;
            this.Extra = wf.Extra.Clone() as byte[];
            this.FormatTag = wf.FormatTag;
            this.SamplesPerSec = wf.SamplesPerSec;
            this.Size = wf.Size;
        }
	}
}
