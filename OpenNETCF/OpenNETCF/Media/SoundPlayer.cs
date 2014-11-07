using System;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using OpenNETCF.Media.WaveAudio;
using OpenNETCF.Threading;
using System.Diagnostics;

using EventWaitHandle = OpenNETCF.Threading.EventWaitHandle;
using EventResetMode = OpenNETCF.Threading.EventResetMode;

namespace OpenNETCF.Media
{
	/// <summary>
	/// Controls playback of a sound from a .wav file.
	/// </summary>
	public class SoundPlayer : Component
	{
		//path to the file
		private string mSoundLocation = "";
		private Stream mStream;
		private byte[] mBuffer;

		/// <summary>
		/// Initializes a new instance of the <see cref="SoundPlayer"/> class.
		/// </summary>
		public SoundPlayer(){}

		/// <summary>
		/// Initializes a new instance of the <see cref="SoundPlayer"/> class, attaches the .wav file within the specified <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream"></param>
		public SoundPlayer(Stream stream)
		{
			Stream = stream;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SoundPlayer"/> class and attaches the specified .wav file.
		/// </summary>
		/// <param name="soundLocation">The location of a .wav file to load.</param>
		/// <remarks>The string passed to the soundLocation parameter must be a path to a .wav file.
		/// If the path is invalid, the <see cref="SoundPlayer"/> object will still be constructed, but subsequent calls to a load or play method will fail.</remarks>
		public SoundPlayer(string soundLocation)
		{
			//set the path
			SoundLocation = soundLocation;
		}

		/// <summary>
		/// Gets or sets the file path of the .wav file to load.
		/// </summary>
		/// <value>The file path from which to load a .wav file, or <see cref="System.String.Empty"/> if no file path is present.
		/// The default is <see cref="System.String.Empty"/>.</value>
		public string SoundLocation
		{
			get
			{
				return mSoundLocation;
			}
			set
			{
				if(File.Exists(value))
				{
					mSoundLocation = value;
					Stream = null;
				}
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="Stream"/> from which to load the .wav file.
		/// </summary>
		/// <remarks>This property is set to null when the <see cref="SoundLocation"/> property is set to a new and valid sound location.</remarks>
		public Stream Stream
		{
			get
			{
				return mStream;
			}
			set
			{
				mStream = value;

				if(value==null)
				{
					mBuffer = null;
				}
				else
				{	
					mBuffer = new byte[value.Length];
					value.Read(mBuffer, 0, mBuffer.Length);
					value.Close();
					mSoundLocation = string.Empty;
				}
			}
		}

		/// <summary>
		/// Plays the .wav file using a new thread.
		/// </summary>
		/// <remarks>The Play method plays the sound using a new thread.
		/// If the .wav file has not been specified or it fails to load, the Play method will play the default beep sound.</remarks>
		public void Play()
		{
			//play async
            PlaySound(OpenNETCF.Media.WaveAudio.NativeMethods.SoundFlags.Async);
		}

		/// <summary>
		/// Plays the .wav file using the UI thread.
		/// </summary>
		/// <remarks>The PlaySync method uses the current thread to play a .wav file, preventing the thread from handling other messages until the load is complete.
		/// After a .wav file is successfully loaded from a Stream or URL path, future calls to playback methods for the SoundPlayer object will not need to reload the .wav file until the path for the sound changes.
		/// If the .wav file has not been specified or it fails to load, the PlaySync method will play the default beep sound.</remarks>
		public void PlaySync()
		{
			//play sync
            PlaySound(OpenNETCF.Media.WaveAudio.NativeMethods.SoundFlags.Sync);
		}

		/// <summary>
		/// Plays and loops the .wav file using a new thread and loads the .wav file first if it has not been loaded.
		/// </summary>
		/// <remarks>The PlayLooping method plays and loops the sound using a new thread.
		/// If the .wav file has not been specified or it fails to load, the PlayLooping method will play the default beep sound.</remarks>
		public void PlayLooping()
		{
			//play looping
            PlaySound(OpenNETCF.Media.WaveAudio.NativeMethods.SoundFlags.Async | OpenNETCF.Media.WaveAudio.NativeMethods.SoundFlags.Loop);
		}

		// helper function
        private void PlaySound(OpenNETCF.Media.WaveAudio.NativeMethods.SoundFlags flags)
		{
			if(mBuffer!=null)
			{
				GCHandle wHandle = GCHandle.Alloc(mBuffer, GCHandleType.Pinned);
                OpenNETCF.Media.WaveAudio.NativeMethods.PlaySound((IntPtr)(wHandle.AddrOfPinnedObject().ToInt32()), IntPtr.Zero, OpenNETCF.Media.WaveAudio.NativeMethods.SoundFlags.Memory | flags);
				wHandle.Free();
			}
			else
			{
                OpenNETCF.Media.WaveAudio.NativeMethods.PlaySound(mSoundLocation, IntPtr.Zero, OpenNETCF.Media.WaveAudio.NativeMethods.SoundFlags.FileName | flags);
			}
		}

		


		/// <summary>
		/// Stops playback of the sound if playback is occurring.
		/// </summary>
		public void Stop()
		{
            OpenNETCF.Media.WaveAudio.NativeMethods.PlaySound(null, IntPtr.Zero, OpenNETCF.Media.WaveAudio.NativeMethods.SoundFlags.NoDefault);
		}

		/// <summary>
		/// Stores additional data.
		/// </summary>
		public object Tag
		{
			get
			{
				return mTag;
			}
			set
			{
				mTag = value;
			}
		}
		private object mTag;	

        internal const int WAVEFORMAT_MIDI_EXTRASIZE = 10; //(sizeof(WAVEFORMAT_MIDI)-sizeof(WAVEFORMATEX)) 
        internal const int CALLBACK_EVENT = 0x00050000; // from mmsystem.h

        public static void PlayTone(Tone tone)
        {
            PlayTone(new Tone[] { tone });
        }

        public static void PlayTone(Tone[] tones)
        {
            WAVEFORMAT_MIDI wfm = new WAVEFORMAT_MIDI();
            wfm.WAVEFORMATEX.FormatTag = FormatTag.MIDI;
            wfm.WAVEFORMATEX.Channels = 1;
            wfm.WAVEFORMATEX.BlockAlign = 8; // sizeof(WAVEFORMAT_MIDI_MESSAGE);
            wfm.WAVEFORMATEX.Size = WAVEFORMAT_MIDI_EXTRASIZE;

            // trial & error seems to be the way to get this - tested using an Axim x51
            // maybe expose these if it varies from hardware to hardware
            wfm.USecPerQuarterNote = 100000;
            wfm.TicksPerQuarterNote = 15;

            IntPtr hWaveOut;
            EventWaitHandle hEvent = new EventWaitHandle(false, EventResetMode.ManualReset);

            byte[] d = wfm.GetBytes();

            int result = WaveAudio.NativeMethods.waveOutOpen(out hWaveOut, 0, d, hEvent.Handle, 0, CALLBACK_EVENT);
            Audio.CheckWaveError(result);

            WAVEFORMAT_MIDI_MESSAGE[] messages = new WAVEFORMAT_MIDI_MESSAGE[2 * tones.Length];
            int totalDuration = 0;
            for (int i = 0; i < tones.Length; i++)
            {
                messages[i * 2].DeltaTicks = 0;
                messages[i * 2].MidiMsg = (uint)(0x7F0090 | (tones[i].MIDINote << 8)); // Note on
                messages[i * 2 + 1].DeltaTicks = (uint)(tones[i].Duration);
                messages[i*2 + 1].MidiMsg = (uint)(0x7F0080 | (tones[i].MIDINote << 8)); // Note off

                totalDuration += tones[i].Duration;
            }

            byte[] headerData = new byte[messages.Length * WAVEFORMAT_MIDI_MESSAGE.Length];
            for (int i = 0; i < messages.Length; i++)
            {
                Buffer.BlockCopy(messages[i].GetBytes(), 0, headerData, i * WAVEFORMAT_MIDI_MESSAGE.Length, WAVEFORMAT_MIDI_MESSAGE.Length);
            }
            WaveHeader header = new WaveHeader(headerData);

            result = WaveAudio.NativeMethods.waveOutPrepareHeader(hWaveOut, header.Pointer, header.HeaderLength);
            Audio.CheckWaveError(result);

            result = WaveAudio.NativeMethods.waveOutWrite(hWaveOut, header.Pointer, header.HeaderLength);
            Audio.CheckWaveError(result);

            if (!hEvent.WaitOne(totalDuration * 20, false))
            {
                Debugger.Break();
            }

            result = WaveAudio.NativeMethods.waveOutUnprepareHeader(hWaveOut, header.Pointer, header.HeaderLength);
            Audio.CheckWaveError(result);

            result = WaveAudio.NativeMethods.waveOutClose(hWaveOut);
            Audio.CheckWaveError(result);
        }
    }
}
