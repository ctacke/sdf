using System;
using OpenNETCF.Win32;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using Microsoft.WindowsCE.Forms;

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
        public event WaveOpenHandler WaveOpen;
        /// <summary>
        /// Raised when the wave device is closed
        /// </summary>
        public event WaveCloseHandler WaveClose;
        /// <summary>
        /// Raised when the wave device has finished playback
        /// </summary>
        public event WaveDoneHandler DonePlaying;

        public event EventHandler PositionChanged;

        private int m_workers = 0;
        private WaveFormat2 m_format;
        private IntPtr m_hWaveOut = IntPtr.Zero;
        private bool m_playing = false;
        //private bool				m_paused		= false;

        private List<SoundMessageWindow> m_mwArray;
        private List<RiffStream> m_streamArray;
        private object m_syncRoot = new object();

        /// <summary>
        /// Constructs Player object on the default wave device
        /// </summary>
        public Player()
        {
            m_mwArray = new List<SoundMessageWindow>();
            m_streamArray = new List<RiffStream>();
            m_qBuffers = new Queue<WaveHeader>(MaxBuffers);
            m_HandleMap = new System.Collections.Hashtable(MaxBuffers);
            AutoCloseStreamAfterPlaying = true;
        }

        /// <summary>
        /// Constructs Player object on the given wave device
        /// </summary>
        /// <param name="AudioDeviceID">Wave device ID</param>
        public Player(int AudioDeviceID)
            : this()
        {
            m_deviceID = AudioDeviceID;
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
            Debug.WriteLine("+Player.Stop()");
            NativeMethods.waveOutReset(m_hWaveOut);
            m_playing = false;
            Debug.WriteLine("-Player.Stop()");
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
        public bool Playing { get { return m_playing; } }

        private float m_playbackRate = 1.0f;

        public float PlaybackRate
        {
            get
            {
                if (!Playing) return m_playbackRate;
                float rate = GetPlaybackRate();
                m_playbackRate = rate;
                return m_playbackRate;
            }
            set
            {
                if (value <= 0) throw new ArgumentException();

                m_playbackRate = value;

                if(!Playing)
                {
                    return;
                }
                SetPlaybackRate(value);

            }
        }

        private void SetPlaybackRate(float rate)
        {
            uint whole = (uint)Math.Floor(rate);
            float fraction = (rate - whole);

            uint rateVal = whole << 16;
            if ((fraction > 0.1f) && (fraction < 0.3f))
            {
                rateVal |= 0x4000;
            }
            else if (fraction < 0.6f)
            {
                rateVal |= 0x8000;
            }
            else if (fraction < 0.8f)
            {
                rateVal |= 0xc000;
            }

            int result = NativeMethods.waveOutSetPlaybackRate(m_hWaveOut, rateVal);
            Audio.CheckWaveError(result);
        }

        private float GetPlaybackRate()
        {
            uint rate;
            int result = NativeMethods.waveOutGetPlaybackRate(m_hWaveOut, out rate);
            Audio.CheckWaveError(result);

            float value = (float)(rate >> 16);

            if ((rate & 0xc000) == 0xc000)
            {
                value += 0.75f;
            }
            else if ((rate & 0x8000) == 0x8000)
            {
                value += 0.50f;
            }
            else if ((rate & 0x8000) == 0x4000)
            {
                value += 0.25f;
            }

            return value;
        }

        /// <summary>
        /// Plays waveform contained in the given stream. Stream is exepcted to contain full riff header
        /// </summary>
        /// <param name="playStream">Stream with the waveform</param>
        public void Play(Stream playStream)
        {
            Monitor.Enter(m_syncRoot);
            try
            {
                if (m_playing) return;

                if (playStream == null)
                {
                    throw new Exception("No valid WAV file has been opened");
                }

#if!NDOC

                if (m_qBuffers == null)
                    m_qBuffers = new Queue<WaveHeader>(MaxBuffers);
                if (m_HandleMap == null)
                    m_HandleMap = new System.Collections.Hashtable(MaxBuffers);

                // create a window to catch waveOutxxx messages
                SoundMessageWindow mw = new SoundMessageWindow();

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

                // see if we need to adjust playback rate
                if ((PlaybackRate < 0.9f) || (PlaybackRate > 1.1f))
                {
                    SetPlaybackRate(PlaybackRate);
                }

                RefillPlayBuffers();

                while (m_qBuffers.Count > 0)
                {
                    Monitor.Enter(m_qBuffers);
                    WaveHeader hdr = m_qBuffers.Dequeue();
                    Monitor.Exit(m_qBuffers);

                    // play the file
                    ThreadPool.QueueUserWorkItem(BufferWriteThreadProc, hdr);

                    m_playing = true;

                    if (m_qBuffers.Count <= 1)
                        RefillPlayBuffers();
                }
#endif
            }
            finally
            {
                Monitor.Exit(m_syncRoot);
            }
        }

        private void BufferWriteThreadProc(object header)
        {
            WaveHeader hdr = header as WaveHeader;

            Interlocked.Increment(ref m_workers);

            Debug.WriteLine(string.Format("Queued Worker Thread on header {0} - Flags == {1} [{2} running]", hdr.Pointer.ToString(), hdr.Flags.ToString(), m_workers));

            int ret = NativeMethods.waveOutWrite(m_hWaveOut, hdr.Pointer, hdr.HeaderLength);

            while ((hdr.Flags & WHDR_FLAGS.DONE) == 0)
            {
                Thread.Sleep(1);
            }

            Interlocked.Decrement(ref m_workers);

            Debug.WriteLine(string.Format("Worker Thread on header {0} Completed [{1} remain]", hdr.Pointer.ToString(), m_workers));

            CheckWaveError(NativeMethods.waveOutUnprepareHeader(m_hWaveOut, hdr.Pointer, hdr.HeaderLength));
            hdr.Dispose();
        }

        // Add more buffers to the playback queue - keep it full
        private void RefillPlayBuffers()
        {
            while (m_qBuffers.Count < MaxBuffers)
            {
                int cb = (int)(BufferLen * m_format.SamplesPerSec * m_format.Channels);
                byte[] data = new byte[cb];

                for (int i = 0; i < m_streamArray.Count; i++)
                {
                    if (m_streamArray[i] != null)
                    {
                        cb = ((Stream)m_streamArray[i]).Read(data, 0, cb);
                        break;
                    }
                }

                if (cb == 0)
                    break;

                WaveHeader hdr = new WaveHeader(data);
                Monitor.Enter(m_HandleMap.SyncRoot);
                m_HandleMap.Add(hdr.Pointer.ToInt32(), hdr);
                Monitor.Exit(m_HandleMap.SyncRoot);

                // prepare the header
                CheckWaveError(NativeMethods.waveOutPrepareHeader(m_hWaveOut, hdr.Pointer, hdr.HeaderLength));
                Monitor.Enter(m_qBuffers);
                m_qBuffers.Enqueue(hdr);
                Monitor.Exit(m_qBuffers);
            }
        }

        private void mw_WaveOpenMessage(object sender)
        {
            if (WaveOpen != null)
            {
                WaveOpen(this);
            }
        }

        public bool AutoCloseStreamAfterPlaying { get; set; }

        private void mw_WaveCloseMessage(object sender)
        {
#if !NDOC
            // remove the message window from the gloabal array
            for (int smw = 0; smw < m_mwArray.Count; smw++)
            {
                if (m_mwArray[smw].Hwnd == ((SoundMessageWindow)sender).Hwnd)
                {
                    // remove the messages
                    m_mwArray[smw].WaveOpenMessage -= new WaveOpenHandler(mw_WaveOpenMessage);
                    m_mwArray[smw].WaveCloseMessage -= new WaveCloseHandler(mw_WaveCloseMessage);
                    m_mwArray[smw].WaveDoneMessage -= new WaveDoneHandler(mw_WaveDoneMessage);

                    m_mwArray[smw].Dispose();
                    m_mwArray.RemoveAt(smw);
                    if (AutoCloseStreamAfterPlaying)
                    {
                        ((Stream)m_streamArray[smw]).Close();
                    }
                    m_streamArray.RemoveAt(smw);

                    break;
                }
            }
            if (WaveClose != null)
            {
                WaveClose(this);
            }
#endif
            m_playing = false;
        }

        private void RaiseDonePlaying(object o)
        {
            Message msg = (Message)o;
            try
            {
                DonePlaying(this, msg.WParam, msg.LParam);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in DonePlaying handler", ex);
            }
        }

        private void mw_WaveDoneMessage(object sender, IntPtr wParam, IntPtr lParam)
        {
            Monitor.Enter(m_syncRoot);
            try
            {
                if (m_hWaveOut == IntPtr.Zero) return;

                // Are there any pending buffers?
                if (m_qBuffers.Count == 0)
                {
                    try
                    {
                        // No more buffers - wait for all workers to end
                        while (m_workers > 0)
                        {
                            Thread.Sleep(10);
                        }

                        // Close the device
                        CheckWaveError(NativeMethods.waveOutClose(m_hWaveOut));
                        m_playing = false;
                        m_qBuffers = null;
                        m_hWaveOut = IntPtr.Zero;

                        // Notify clients
                        if (DonePlaying != null)
                        {
                            // On a separate thread in case the app calls Play in the Done handler.
                            ThreadPool.QueueUserWorkItem(RaiseDonePlaying, new Message { LParam = lParam, WParam = wParam });
                        }
                    }
                    catch (Exception e)
                    {
                        if (!e.Message.Equals("Sound still playing"))
                        {
                            throw e;
                        }
                    }
                }
                else
                {
                    //Get next buffer (already prepared)
                    Monitor.Enter(m_qBuffers);
                    WaveHeader hdr = m_qBuffers.Dequeue() as WaveHeader;
                    Monitor.Exit(m_qBuffers);

                    // play the file
                    ThreadPool.QueueUserWorkItem(BufferWriteThreadProc, hdr);

                    // Add more buffers
                    RefillPlayBuffers();
                }

                if (PositionChanged != null)
                    PositionChanged(this, EventArgs.Empty);
            }
            finally
            {
                Monitor.Exit(m_syncRoot);
            }
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
