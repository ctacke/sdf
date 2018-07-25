using System;
using OpenNETCF.Win32;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace OpenNETCF.Media.WaveAudio
{
	/// <summary>
	/// Wave Audio player class. Supports PCM waveform playback from the stream
	/// </summary>
	public class Player : Audio, IDisposable
	{
		/// <summary>
		/// Raised when the wave device is opened
		/// </summary>
		public event WaveOpenHandler		WaveOpen;
		/// <summary>
		/// Raised when the wave device is closed
		/// </summary>
		public event WaveCloseHandler		WaveClose;
		/// <summary>
		/// Raised when the wave device has finished playback
		/// </summary>
		public event WaveDoneHandler		DonePlaying;

        public event EventHandler PositionChanged;

		private WaveFormat2		m_format;
		private IntPtr				m_hWaveOut		= IntPtr.Zero;
		private bool				m_playing		= false;
		//private bool				m_paused		= false;

		private List<SoundMessageWindow> m_mwArray;
		private List<RiffStream>	m_streamArray;

		/// <summary>
		/// Constructs Player object on the default wave device
		/// </summary>
		public Player()
		{
            m_mwArray = new List<SoundMessageWindow>();
            m_streamArray = new List<RiffStream>();
			m_qBuffers = new Queue<WaveHeader>(MaxBuffers);
            m_HandleMap = new System.Collections.Hashtable(MaxBuffers);
		}

		/// <summary>
		/// Constructs Player object on the given wave device
		/// </summary>
		/// <param name="AudioDeviceID">Wave device ID</param>
		public Player(int AudioDeviceID) : this()
		{			
			m_deviceID =AudioDeviceID;
		}

		/// <summary>
		/// Number of the output wave devices in the system
		/// </summary>
        public static int NumDevices { get { return NativeMethods.waveOutGetNumDevs(); } }

		/// <summary>
		/// Restart a paused wave file
		/// </summary>
        public void Restart() { NativeMethods.waveOutRestart(m_hWaveOut); }

		/// <summary>
		/// Pause Play
		/// </summary>
        public void Pause() { NativeMethods.waveOutPause(m_hWaveOut); }

		/// <summary>
		/// Stop Play
		/// </summary>
		public void Stop() 
		{
            NativeMethods.waveOutReset(m_hWaveOut); 
			m_playing = false;
		}

		/// <summary>
		/// Gets or sets playback volume on the current wave device
		/// </summary>
		public int Volume
		{
			get
			{
				int vol = 0;
                CheckWaveError(NativeMethods.waveOutGetVolume(m_hWaveOut, ref vol));
				return vol;
			}
			set
			{
                CheckWaveError(NativeMethods.waveOutSetVolume(m_hWaveOut, Convert.ToInt32(value)));
			}
		}

		/// <summary>
		/// True, if the player is currently playing. False otherwise
		/// </summary>
		public bool Playing { get{ return m_playing; } }


		/// <summary>
		/// Plays waveform contained in the given stream. Stream is exepcted to contain full riff header
		/// </summary>
		/// <param name="playStream">Stream with the waveform</param>
		public void Play( Stream playStream )
		{
			if(m_playing) return;
			
			if ( playStream == null )
			{
				throw new Exception("No valid WAV file has been opened");
			}

#if!NDOC

			if ( m_qBuffers == null )
				m_qBuffers = new Queue<WaveHeader>(MaxBuffers);
			if ( m_HandleMap == null )
                m_HandleMap = new System.Collections.Hashtable(MaxBuffers);

			// create a window to catch waveOutxxx messages
			SoundMessageWindow	mw = new SoundMessageWindow();

			// wire in events
			mw.WaveOpenMessage += new WaveOpenHandler(mw_WaveOpenMessage);
			mw.WaveCloseMessage += new WaveCloseHandler(mw_WaveCloseMessage);
			mw.WaveDoneMessage += new WaveDoneHandler(mw_WaveDoneMessage);

			// add it to the global array
            m_mwArray.Add(mw);
            int i = m_mwArray.Count - 1;
            if (playStream is ACMStream)
                m_streamArray.Add(playStream as ACMStream);
            else if (playStream is RiffStream)
                m_streamArray.Add(playStream as RiffStream);
            else
                m_streamArray.Add(ACMStream.OpenRead(playStream));
            m_format = (m_streamArray[m_streamArray.Count - 1] as RiffStream).Format;

			// open the waveOut device and register the callback
            CheckWaveError(NativeMethods.waveOutOpen(out m_hWaveOut, m_deviceID, m_format.GetBytes(), m_mwArray[i].Hwnd, 0, CALLBACK_WINDOW));

			RefillPlayBuffers();

            // FIX: Audio buffer starvation in OpenNETCF.Media.WaveAudio (Bug #30)
            while (m_qBuffers.Count > 0)
            {
                Monitor.Enter(m_qBuffers);
                WaveHeader hdr = m_qBuffers.Dequeue();
                Monitor.Exit(m_qBuffers);

                // play the file
                int ret = NativeMethods.waveOutWrite(m_hWaveOut, hdr.Header, hdr.HeaderLength);
                CheckWaveError(ret);

                m_playing = true;

                // FIX: Player only plays 25 seconds or so of audio (Bug #114)
                if (m_qBuffers.Count <= 1)
                    RefillPlayBuffers();
            }
#endif
		}

		// Add more buffers to the playback queue - keep it full
		private void RefillPlayBuffers()
		{
			while( m_qBuffers.Count < MaxBuffers )
			{
				int cb = (int)(BufferLen * m_format.SamplesPerSec * m_format.Channels);
				byte[] data = new byte[cb];

				for(int i = 0 ; i < m_streamArray.Count ; i++)
				{
					if( m_streamArray[i] != null )
					{
						cb = ((Stream)m_streamArray[i]).Read(data, 0, cb);
						break;
					}
				}

				if ( cb == 0 )
					break;

				WaveHeader hdr = new WaveHeader(data);
				Monitor.Enter(m_HandleMap.SyncRoot);
				m_HandleMap.Add(hdr.Header.ToInt32(), hdr);
				Monitor.Exit(m_HandleMap.SyncRoot);

				// prepare the header
                CheckWaveError(NativeMethods.waveOutPrepareHeader(m_hWaveOut, hdr.Header, hdr.HeaderLength));
				Monitor.Enter(m_qBuffers);
				m_qBuffers.Enqueue(hdr);
				Monitor.Exit(m_qBuffers);
			}
		}

		private void mw_WaveOpenMessage(object sender)
		{
			if(WaveOpen != null)
			{
				WaveOpen(this);
			}
		}

		private void mw_WaveCloseMessage(object sender)
		{
#if !NDOC
			// remove the message window from the gloabal array
			for(int smw = 0 ; smw < m_mwArray.Count ; smw++)
			{
				if(m_mwArray[smw].Hwnd == ((SoundMessageWindow)sender).Hwnd)
				{
					// remove the messages
					m_mwArray[smw].WaveOpenMessage -= new WaveOpenHandler(mw_WaveOpenMessage);
					m_mwArray[smw].WaveCloseMessage -= new WaveCloseHandler(mw_WaveCloseMessage);
					m_mwArray[smw].WaveDoneMessage -= new WaveDoneHandler(mw_WaveDoneMessage);

					m_mwArray[smw].Dispose();
					m_mwArray.RemoveAt(smw);
					((Stream)m_streamArray[smw]).Close();
					m_streamArray.RemoveAt(smw);

					break;
				}
			}
			if(WaveClose != null)
			{
				WaveClose(this);
			}
#endif
			m_playing = false;
		}
		private void mw_WaveDoneMessage(object sender, IntPtr wParam, IntPtr lParam)
		{
			// free the header
			Monitor.Enter(m_HandleMap.SyncRoot);
			WaveHeader hdr = m_HandleMap[lParam.ToInt32()] as WaveHeader;
			m_HandleMap.Remove(lParam.ToInt32());
			Monitor.Exit(m_HandleMap.SyncRoot);
            CheckWaveError(NativeMethods.waveOutUnprepareHeader(m_hWaveOut, lParam, hdr.HeaderLength));
			hdr.Dispose();

			// Check if we got here because of waveOutReset
			if ( !m_playing )
			{
				// Cleanup - free buffers and headers
				Monitor.Enter(m_qBuffers);
				while( m_qBuffers.Count > 0 )
				{
                    hdr = m_qBuffers.Dequeue();
                    CheckWaveError(NativeMethods.waveOutUnprepareHeader(m_hWaveOut, hdr.Header, hdr.HeaderLength));
					m_HandleMap.Remove(hdr.Header.ToInt32());
					hdr.Dispose();
				}
				Monitor.Exit(m_qBuffers);
			}

			// Are there any pending buffers?
			if ( m_qBuffers.Count == 0 )
			{
                // FIX: Audio buffer starvation in OpenNETCF.Media.WaveAudio (Bug #30)
				try
				{
				    // No more buffers
				    // Close the device
				    CheckWaveError(NativeMethods.waveOutClose(m_hWaveOut));
				    m_playing = false;
				    m_qBuffers = null;

				    // Notify clients
				    if (DonePlaying != null)
				    {
				        DonePlaying(this, wParam, lParam);
				    }
				}
                catch(Exception e)
                {
                    if(!e.Message.Equals("Sound still playing"))
                    {
                        throw e;
                    }
                }
			}
			else
			{
				//Get next buffer (already prepared)
				Monitor.Enter(m_qBuffers);
				hdr = (WaveHeader)m_qBuffers.Dequeue();
				Monitor.Exit(m_qBuffers);

				// play the file
                CheckWaveError(NativeMethods.waveOutWrite(m_hWaveOut, hdr.Header, hdr.HeaderLength));
				
				// Add more buffers
				RefillPlayBuffers();
			}

            if (PositionChanged != null)
                PositionChanged(this, EventArgs.Empty);
		}


        #region IDisposable Members

        public void Dispose()
        {
            foreach (Stream stm in m_streamArray)
                stm.Close();
            m_streamArray.Clear();
        }

        #endregion
    }
}
