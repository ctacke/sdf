using System;
using System.Diagnostics;
using System.IO;
using OpenNETCF.Win32;
using System.Threading;
using System.Collections.Generic;

namespace OpenNETCF.Media.WaveAudio
{
    /// <summary>
    /// Recorder class. Wraps low-level WAVE API for recording purposes
    /// </summary>
    public class Recorder : Audio
    {
        /// <summary>
        /// Handles the event that is fired when wave device is successfully opened
        /// </summary>
        public event WaveOpenHandler WaveOpen;
        /// <summary>
        /// Handles the event that is fired when wave device is successfully closed
        /// </summary>
        public event WaveCloseHandler WaveClose;
        /// <summary>
        /// Handles the event that is fired when recording is stopped (on timer or by calling <see cref="Recorder.Stop">Stop</see> method
        /// </summary>
        public event WaveFinishedHandler DoneRecording;

        public event EventHandler PositionChanged;

        private IntPtr m_hWaveIn = IntPtr.Zero;
#if !NDOC
        private SoundMessageWindow m_recmw;
#endif
        private bool recording = false;
        private bool recordingFinished = false;
        private int m_recBufferSize;
        private WaveFormat2 m_recformat;
        private RiffStream m_streamRecord;
        // Recoding timer
        private Timer m_recTimer;

        /// <summary>
        /// Whether the Recorder is presently recording
        /// </summary>
        public bool Recording { get { return recording; } }

        /// <summary>
        /// Creates Recorder object and attaches it to the default wave device
        /// </summary>
        public Recorder()
        {
#if NDOC
	throw new Exception("This is an NDOC Build, not a build for use!");
#endif
            m_qBuffers = new Queue<WaveHeader>(MaxBuffers);
            m_HandleMap = new System.Collections.Hashtable(MaxBuffers);
        }

        /// <summary>
        /// Creates Recorder object and attaches it to the given wave device
        /// </summary>
        /// <param name="AudioDeviceID">Wave device ID</param>
        public Recorder(int AudioDeviceID)
            : this()
        {
            m_deviceID = AudioDeviceID;
        }

        /// <summary>
        /// A list of PCM wave formats supported by the default device
        /// </summary>
        /// <returns>SoundFormats collection</returns>
        public SoundFormats SupportedRecordingFormats()
        {
            return SupportedRecordingFormats(0);
        }

        /// <summary>
        /// A list of PCM wave formats supported by the given device
        /// </summary>
        /// <param name="DeviceID">Wave device</param>
        /// <returns>SoundFormats collection</returns>
        public SoundFormats SupportedRecordingFormats(int DeviceID)
        {
            WaveInCaps wic = new WaveInCaps();

            int ret = NativeMethods.waveInGetDevCaps(DeviceID, wic.ToByteArray(), wic.ToByteArray().Length);
            CheckWaveError(ret);

            return (SoundFormats)wic.Formats;
        }

        /// <summary>
        /// Number of wave input devices in the system
        /// </summary>
        public static int NumDevices { get { return NativeMethods.waveInGetNumDevs(); } }

        /// <summary>
        /// Stop recording operation currently in progress. 
        /// Throws an error if no recording operation is in progress
        /// </summary>
        public void Stop()
        {
            if (!recordingFinished)
            {
                if (m_recTimer != null)
                    m_recTimer.Dispose();
                CheckWaveError(NativeMethods.waveInReset(m_hWaveIn));
                recordingFinished = true;
            }
        }

        /// <summary>
        /// Record sound data for specified number of seconds at 11025 sps and 1 channel
        /// The stream will be a properly formatted RIFF file
        /// </summary>
        /// <param name="st">Stream into which recorded samples are written</param>
        /// <param name="Seconds">Seconds of data to record</param>
        public void RecordFor(Stream st, short Seconds)
        {
            RecordFor(st, Seconds, SoundFormats.Mono8bit11kHz);
        }

        public void RecordFor(Stream st, short Seconds, SoundFormats SoundFormat)
        {
            RecordFor(st, Seconds, WaveFormat2.GetPCMWaveFormat(SoundFormat));
        }
        /// <summary>
        /// Record sound data for specified number of seconds using given wave format
        /// The stream will be a properly formatted RIFF file
        /// </summary>
        /// <param name="st">Stream into which recorded samples are written</param>
        /// <param name="Seconds">Seconds of data to record</param>
        /// <param name="format">Sound format to record in.</param>
        public RiffStream RecordFor(Stream st, short Seconds, WaveFormat2 format)
        {
            // only allow 1 recording session at a time
            if (recording)
            {
                throw new InvalidOperationException("Already recording");
            }

            m_hWaveIn = IntPtr.Zero;

            // set our global flag
            recording = true;

            if (m_qBuffers == null)
                m_qBuffers = new Queue<WaveHeader>(MaxBuffers);
            if (m_HandleMap == null)
                m_HandleMap = new System.Collections.Hashtable(MaxBuffers);

            m_recformat = new WaveFormat2();
#if !NDOC
            // create the callback message window
            m_recmw = new SoundMessageWindow();
            m_recmw.WaveDoneMessage += new WaveDoneHandler(m_recmw_WaveDoneMessage);
            m_recmw.WaveCloseMessage += new WaveCloseHandler(mw_WaveCloseMessage);
            m_recmw.WaveOpenMessage += new WaveOpenHandler(mw_WaveOpenMessage);
#endif
            bool append = st.Length >= 42;
            if (format.FormatTag == FormatTag.PCM)
            {
                m_recformat = format;
                if (append)
                    m_streamRecord = RiffStream.Append(st);
                else
                    m_streamRecord = RiffStream.OpenWrite(st, m_recformat);
            }
            else
            {
                m_recformat = WaveFormat2.GetPCMWaveFormat(SoundFormats.Mono8bit11kHz);
                if (append)
                    m_streamRecord = ACMStream.Append(st, m_recformat);
                else
                    m_streamRecord = ACMStream.OpenWrite(st, m_recformat, format);
            }
#if !NDOC
            // check for support of selected format
            CheckWaveError(NativeMethods.waveInOpen(out m_hWaveIn, WAVE_MAPPER, m_recformat.GetBytes(), IntPtr.Zero, 0, WAVE_FORMAT_QUERY));

            // open wave device
            CheckWaveError(NativeMethods.waveInOpen(out m_hWaveIn, (uint)m_deviceID, m_recformat.GetBytes(), m_recmw.Hwnd, 0, CALLBACK_WINDOW));

            m_recBufferSize = (int)(Math.Min((int)Seconds, BufferLen) * m_recformat.AvgBytesPerSec);

            for (int i = 0; i < 2; i++)
            {
                WaveHeader hdr = GetNewRecordBuffer(m_recBufferSize);

                // send the buffer to the device
                CheckWaveError(NativeMethods.waveInAddBuffer(m_hWaveIn, hdr.Pointer, hdr.HeaderLength));
            }

            // begin recording
            CheckWaveError(NativeMethods.waveInStart(m_hWaveIn));
            recordingFinished = false;
            m_recTimer = new Timer(new TimerCallback(RecTimerCallback), this, Seconds * 1000, Timeout.Infinite);
#endif
            return m_streamRecord;
        }

        private void RecTimerCallback(object state)
        {
            if (recording)
            {
                Stop();
            }
        }

        /// <summary>
        /// Creates a recording buffer
        /// </summary>
        /// <param name="dwBufferSize"></param>
        /// <returns>new buffer as WaveHeader</returns>
        private WaveHeader GetNewRecordBuffer(int dwBufferSize)
        {
            WaveHeader hdr = new WaveHeader(dwBufferSize);
            Monitor.Enter(m_HandleMap.SyncRoot);
            m_HandleMap.Add(hdr.Pointer.ToInt32(), hdr);
            Monitor.Exit(m_HandleMap.SyncRoot);
            // prepare the header
            CheckWaveError(NativeMethods.waveInPrepareHeader(m_hWaveIn, hdr.Pointer, hdr.HeaderLength));
            return hdr;
        }

        private void DumpRecordBuffers()
        {
            while (m_qBuffers.Count > 0)
            {
                Monitor.Enter(m_qBuffers);
                WaveHeader hdr = (WaveHeader)m_qBuffers.Dequeue();
                Monitor.Exit(m_qBuffers);


                try
                {
                    m_streamRecord.Write(hdr.BufferData, 0, hdr.BytesRecorded);
                    //          m_streamRecord.Write(hdr.GetData(), 0, hdr.BytesRecorded);
                }
                catch (Exception ex)
                {
                    Debug.Write("Exception in stream.Write: " + ex.ToString());
                }
                hdr.Dispose();
            }
        }

        private void mw_WaveOpenMessage(object sender)
        {
            if (WaveOpen != null)
            {
                WaveOpen(this);
            }
        }

        private void mw_WaveCloseMessage(object sender)
        {
            if (WaveClose != null)
            {
                WaveClose(this);
            }
        }

        private void m_recmw_WaveDoneMessage(object sender, IntPtr wParam, IntPtr lParam)
        {
            Debug.WriteLine("received WaveDone");
#if !NDOC
            // Retrieve Waveheader object by the lpHeader pointer
            Monitor.Enter(m_HandleMap.SyncRoot);
            WaveHeader hdr = m_HandleMap[lParam.ToInt32()] as WaveHeader;

            m_HandleMap.Remove(hdr.Pointer.ToInt32());
            Monitor.Exit(m_HandleMap.SyncRoot);

            // unprepare the header
            CheckWaveError(NativeMethods.waveInUnprepareHeader(m_hWaveIn, hdr.Pointer, hdr.HeaderLength));
            //      hdr.RetrieveHeader();
            m_qBuffers.Enqueue(hdr);

            if (recordingFinished) // last chunk
            {
                DumpRecordBuffers();
                CheckWaveError(NativeMethods.waveInClose(m_hWaveIn));
                m_streamRecord.Flush();
                m_streamRecord.Close();
                mw_WaveCloseMessage(this);
                // clean up the messageWindow
                m_recmw.Dispose();

                // reset the global flag
                recording = false;
                // set our event
                if (DoneRecording != null)
                {
                    DoneRecording();
                }

                foreach (WaveHeader whdr in m_HandleMap.Values)
                    whdr.Dispose();
                m_HandleMap.Clear();
            }
            else
            {
                hdr = GetNewRecordBuffer(m_recBufferSize);
                CheckWaveError(NativeMethods.waveInAddBuffer(m_hWaveIn, hdr.Pointer, hdr.HeaderLength));
                DumpRecordBuffers();
            }

            if (PositionChanged != null)
                PositionChanged(this, EventArgs.Empty);
#endif
        }
    }
}
