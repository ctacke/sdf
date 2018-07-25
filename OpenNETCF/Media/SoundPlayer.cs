using System;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using OpenNETCF.Media.WaveAudio;


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
	}
}
