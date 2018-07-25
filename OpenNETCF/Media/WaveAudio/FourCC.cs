using System;
using System.IO;
using System.Runtime.InteropServices;

namespace OpenNETCF.Media.WaveAudio
{
	[StructLayout(LayoutKind.Sequential)]
    public struct FourCC
	{
        public byte m_c1;
        public byte m_c2;
        public byte m_c3;
        public byte m_c4;
		
        public int GetFourCC()
		{
			return (int)( (Convert.ToInt32(m_c4) << 24) | (Convert.ToInt32(m_c3) << 16) | (Convert.ToInt32(m_c2) << 8) | Convert.ToInt32(m_c1) );
		}

		public FourCC(int fcc)
		{
			m_c4 = Convert.ToByte((fcc >> 24) & 0xff);
            m_c3 = Convert.ToByte((fcc >> 16) & 0xff);
            m_c2 = Convert.ToByte((fcc >> 8) & 0xff);
            m_c1 = Convert.ToByte((fcc) & 0xff); 
		}


		public FourCC(char c1, char c2, char c3, char c4)
		{
            m_c1 = (byte)c1; m_c2 = (byte)c2; m_c3 = (byte)c3; m_c4 = (byte)c4;
		}

		static public FourCC Riff = new FourCC('R', 'I', 'F', 'F');
		static public FourCC Wave = new FourCC('W', 'A', 'V', 'E');
		static public FourCC Fmt = new FourCC('f', 'm', 't', ' ');
		static public FourCC Data = new FourCC('d', 'a', 't', 'a');

        public void Write(Stream stm)
        {
            stm.WriteByte(m_c1);
            stm.WriteByte(m_c2);
            stm.WriteByte(m_c3);
            stm.WriteByte(m_c4);
        }

		public override bool Equals(object obj)
		{
			if ( obj is FourCC )
			{
				FourCC fcc = (FourCC)obj;
				//return fcc.m_c1 = c1 && fcc.m_c2 = c2 && fcc.m_c3 = c3 && fcc.m_c4 = c4;
				return fcc.GetFourCC() == GetFourCC();
			}
			else
				return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return GetFourCC();
		}
		public static implicit operator int(FourCC fcc)
		{
			return fcc.GetFourCC();
		}
		public static implicit operator FourCC(int val)
		{
			return new FourCC(val);
		}

	}
}
