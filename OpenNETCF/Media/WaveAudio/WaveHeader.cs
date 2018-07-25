using System;
using OpenNETCF.Win32;
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Media.WaveAudio
{
	/// <summary>
	/// Internal wrapper around WAVEHDR
	/// Facilitates asynchronous operations
	/// </summary>
	internal class WaveHeader: IDisposable
	{
		private WaveHdr m_hdr;
		private IntPtr m_lpData;
		private int m_cbdata;
		private int m_cbHeader;
		private IntPtr m_lpHeader;


		public WaveHeader(byte[] data)
		{
			InitFromData(data, data.Length);
		}

		/// <summary>
		/// Creates WaveHeader and fills it with wave data
		/// <see cref="WaveHdr"/>
		/// </summary>
		/// <param name="data">wave data bytes</param>
		/// <param name="datalength">length of Wave data</param>
		public WaveHeader(byte[] data, int datalength)
		{
			InitFromData(data, datalength);
		}
		
		/// <summary>
		/// Constructor for WaveHeader class
		/// Allocates a buffer of required size
		/// </summary>
		/// <param name="BufferSize"></param>
		public WaveHeader(int BufferSize)
		{
			InitFromData(null, BufferSize);
		}

		internal void InitFromData(byte[] data, int datalength)
		{
			m_cbdata = datalength;
			m_lpData = Marshal.AllocHGlobal(m_cbdata);
			if ( data != null )
				Marshal.Copy(data, 0, m_lpData, m_cbdata);
			m_hdr = new WaveHdr((int)m_lpData.ToInt32(), m_cbdata);
			m_cbHeader = m_hdr.ToByteArray().Length;
			m_lpHeader = Marshal.AllocHGlobal(m_cbHeader);
			byte[] hdrbits = m_hdr.ToByteArray();
			Marshal.Copy(hdrbits, 0, m_lpHeader, m_cbHeader);
		}

		///<summary>Ptr to WAVEHDR in the unmanaged memory</summary>
		public IntPtr Header { get { return m_lpHeader; } }
		///<summary>Ptr to wave data in the unmanaged memory</summary>
		public IntPtr Data { get { return m_lpData; } }
		///<summary>Wave data size</summary>
		public int DataLength { get { return m_cbdata; } }
		public int HeaderLength { get { return m_cbHeader; } }
		public WaveHdr waveHdr { get { return m_hdr; } }
		public byte[] GetData()
		{
			byte [] data = new byte[m_cbdata];
			Marshal.Copy(m_lpData, data, 0, m_cbdata);
			return data;
		}
		public void RetrieveHeader()
		{
			byte[] headerBits = new byte[m_cbHeader];
			Marshal.Copy(m_lpHeader, headerBits, 0, m_cbHeader);
			m_hdr = new WaveHdr(headerBits);
		}
		#region IDisposable Members

		public void Dispose()
		{
			Marshal.FreeHGlobal(m_lpData);
			Marshal.FreeHGlobal(m_lpHeader);
		}

		#endregion
	}
}
