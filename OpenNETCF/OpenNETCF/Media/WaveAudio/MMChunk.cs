using System;
using System.IO;
using System.Collections;

namespace OpenNETCF.Media.WaveAudio
{
	internal class MMChunk
	{
		protected FourCC m_fourCC;
		protected long m_nSize;
		protected bool m_bDirty;
		protected Stream _stm;
		protected ArrayList m_chunkList;
		protected long m_posCurrent, m_posStart;

		public FourCC FourCC { get { return m_fourCC; } }
		public MMChunk this[int index] { get { return m_chunkList[index] as MMChunk; } }
		public MMChunk FindChunk(FourCC fcc)
		{
			foreach(MMChunk ck in m_chunkList)
			{
				if ( ck.FourCC == fcc )
				{
					return ck;
				}
			}
			return null;
		}

		public MMChunk(FourCC fourCC, Stream st)
		{
			m_fourCC = fourCC;
			m_nSize = 0;
			m_bDirty = false;
			_stm = st;
			m_posCurrent = st.Position;
			//m_posStart = st.Position;
			m_chunkList = new ArrayList();

            m_posStart = st.Position;
            //byte[] data = new byte[4];
            //st.Read(data, 0, 4);
            //m_nSize = BitConverter.ToInt32(data, 0);
        }

		virtual public void Flush()
		{
			if ( !IsDirty() )
				return;
		}

		virtual public bool IsDirty()
		{
			return m_bDirty;
		}

        public virtual void CalculateSize()
        {
        }

		virtual public MMChunk CreateSubChunk(FourCC fourCC, Stream st)
		{
			if ( fourCC == FourCC.Riff )
			{
				MMChunk ck = new RiffChunk(st);
				m_chunkList.Add(ck);
				return ck;
			}
			else if ( fourCC == FourCC.Fmt )
			{
				MMChunk ck = new FmtChunk(st, null);
				m_chunkList.Add(ck);
				return ck;
			}
			else if ( fourCC == FourCC.Data )
			{
				MMChunk ck = new DataChunk(st);
				m_chunkList.Add(ck);
				return ck;
			}
			
			return null;
		}

        public virtual void AppendChunk(MMChunk ck)
        {
            ck.m_fourCC.Write(_stm);
            m_chunkList.Add(ck);
        }

		static public MMChunk FromStream(Stream st)
		{
			if ( !st.CanRead )
				return null;
			if ( st.Position > st.Length - 4 )
				return null;
			byte[] data = new byte[4];
			st.Read(data, 0, 4);
			FourCC fourCC = BitConverter.ToInt32(data, 0);
			if ( fourCC == FourCC.Riff )
			{
				RiffChunk ck = new RiffChunk(st);
				ck.LoadFromStream(st);
				return ck;
			}
			else if ( fourCC == FourCC.Fmt )
			{
				FmtChunk ck = new FmtChunk(st, null);
				ck.LoadFromStream(st);
				return ck;
			}
            else if (fourCC == FourCC.Data)
            {
                DataChunk ck = new DataChunk(st);
                ck.LoadFromStream(st);
                return ck;
            }
            else
            {
                _RiffChunk ck = new _RiffChunk(fourCC, st);
                ck.LoadFromStream(st);
                return ck;
            }
		}

		public virtual long Size
		{
            get
            {
			    return 4; //FourCC
            }
            set
            {
                throw new NotSupportedException();
            }
		}

        public long Start
        {
            get {return m_posStart;}
            set { m_posStart = value; }
        }
    }
}
