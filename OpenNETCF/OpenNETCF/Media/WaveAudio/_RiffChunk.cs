using System;
using System.IO;

namespace OpenNETCF.Media.WaveAudio
{
	internal class _RiffChunk: MMChunk
	{
		public _RiffChunk(FourCC fourCC, Stream st): base(fourCC, st)
		{
        }

		public void Write(byte[] data, int offset, int count)
		{
			_stm.Write(data, offset, count);
		}

		public void Write(byte[] data)
		{
			Write(data, 0, data.Length);
			m_posCurrent = _stm.Position;
		}

		public override long Size
		{
            get
            {
                //int size = base.GetSize() + 4; // + ckSize
                //foreach (MMChunk ck in m_chunkList)
                //{
                //    size += ck.GetSize();
                //}
                //return size;
                return m_nSize;
            }
            set
            {
                m_nSize = value;
            }
		}

        public override void CalculateSize()
        {
            long size = 8; // + ckSize
            foreach (MMChunk ck in m_chunkList)
            {
                ck.CalculateSize();
                size += ck.Size;
            }
            m_nSize = size;
        }

		public virtual void UpdateSize()
		{
			CalculateSize();
			long lPos = _stm.Position;
			_stm.Position = m_posStart + 4;
			_stm.Write(BitConverter.GetBytes(m_nSize), 0, 4);
			_stm.Position = lPos;
		}

		virtual public void LoadFromStream(Stream st)
		{
            byte[] data = new byte[4];
            m_posStart = st.Position - 4;
            st.Read(data, 0, 4);
            m_nSize = BitConverter.ToInt32(data, 0);
            st.Seek(m_nSize, SeekOrigin.Current);
        }


		public Stream BeginRead()
		{
			m_posCurrent = _stm.Position;
			_stm.Position = m_posStart + 8;
			return _stm;
		}

		public void EndRead()
		{
			_stm.Position = m_posCurrent;
		}

        public virtual Stream BeginWrite()
		{
			m_posCurrent = _stm.Position;
			_stm.Position = m_posStart + 8;
			return _stm;
		}
		virtual public void EndWrite()
		{
			_stm.Position = m_posStart;
			Write(BitConverter.GetBytes((int)m_fourCC));
			_stm.Write(BitConverter.GetBytes(Size), 0, 4);
			_stm.Position = m_posCurrent;
		}
	}
}
