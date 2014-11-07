using System;
using System.IO;
using System.Collections.Generic;

namespace OpenNETCF.Media.WaveAudio
{
	/// <summary>
	/// Handles generic wave device Open event
	/// </summary>
	public delegate void WaveOpenHandler(object sender);
	/// <summary>
	/// Handles generic wave device Close event
	/// </summary>
	public delegate void WaveCloseHandler(object sender);
	/// <summary>
	/// Handles generic wave device Operation Complete event (Record/Play)
	/// </summary>
	public delegate void WaveDoneHandler(object sender, IntPtr wParam, IntPtr lParam);
	/// <summary>
	/// Handles wave device Recording Complete event
	/// </summary>
	public delegate void WaveFinishedHandler();

	/// <summary>
	/// Flags for the supported audio formats for recording and playback devices
	/// </summary>
	[FlagsAttribute()]
	public enum SoundFormats : int
	{
		/// <summary>
		/// Format is not valid
		/// </summary>
		InvalidFormat		= 0x0000,
		/// <summary>
		/// Mono, 8 bit, 11025 Hz
		/// </summary>
		Mono8bit11kHz		= 0x0001,
		/// <summary>
		/// Stereo, 8 bit, 11025 Hz
		/// </summary>
		Stereo8bit11kHz		= 0x0002,
		/// <summary>
		/// Mono, 16 bit, 11025 Hz
		/// </summary>
		Mono16bit11kHz		= 0x0004,
		/// <summary>
		/// Stereo, 16 bit, 11025 Hz
		/// </summary>
		Stereo16bit11kHz	= 0x0008,
		/// <summary>
		/// Mono, 8 bit, 22050 Hz
		/// </summary>
		Mono8bit22kHz		= 0x0010,
		/// <summary>
		/// Stereo, 8 bit, 22050 Hz
		/// </summary>
		Stereo8bit22kHz		= 0x0020,
		/// <summary>
		/// Mono, 16 bit, 22050 Hz
		/// </summary>
		Mono16bit22kHz		= 0x0040,
		/// <summary>
		/// Stereo, 16 bit, 22050 Hz
		/// </summary>
		Stereo16bit22kHz	= 0x0080,
		/// <summary>
		/// Mono, 8 bit, 44100 Hz
		/// </summary>
		Mono8bit44kHz		= 0x0100,
		/// <summary>
		/// Stereo, 8 bit, 44100 Hz
		/// </summary>
		Stereo8bit44kHz		= 0x0200,
		/// <summary>
		/// Mono, 16 bit, 44100 Hz
		/// </summary>
		Mono16bit44kHz		= 0x0400,
		/// <summary>
		/// Stereo, 16 bit, 44100 Hz
		/// </summary>
		Stereo16bit44kHz	= 0x0800
	}

	/// <summary>
	/// Base class for Player/Recorder
	/// </summary>
	public class Audio
	{
		private const int   MMSYSERR_BASE			= 0;
		private const int   MMSYSERR_NOERROR		= 0;
		private const int   MMSYSERR_ERROR			= (MMSYSERR_BASE + 1);
		private const int   MMSYSERR_BADDEVICEID	= (MMSYSERR_BASE + 2);
		private const int   MMSYSERR_NOTENABLED		= (MMSYSERR_BASE + 3);
		private const int   MMSYSERR_ALLOCATED		= (MMSYSERR_BASE + 4);
		private const int   MMSYSERR_INVALHANDLE	= (MMSYSERR_BASE + 5);
		private const int   MMSYSERR_NODRIVER		= (MMSYSERR_BASE + 6);
		private const int   MMSYSERR_NOMEM			= (MMSYSERR_BASE + 7);
		private const int   MMSYSERR_NOTSUPPORTED	= (MMSYSERR_BASE + 8);
		private const int   MMSYSERR_BADERRNUM		= (MMSYSERR_BASE + 9);
		private const int   MMSYSERR_INVALFLAG		= (MMSYSERR_BASE + 10);
		private const int   MMSYSERR_INVALPARAM		= (MMSYSERR_BASE + 11);
		private const int   MMSYSERR_HANDLEBUSY		= (MMSYSERR_BASE + 12);
		private const int   MMSYSERR_INVALIDALIAS	= (MMSYSERR_BASE + 13);
		private const int   MMSYSERR_BADDB			= (MMSYSERR_BASE + 14);
		private const int   MMSYSERR_KEYNOTFOUND	= (MMSYSERR_BASE + 15);
		private const int   MMSYSERR_READERROR		= (MMSYSERR_BASE + 16);
		private const int   MMSYSERR_WRITEERROR		= (MMSYSERR_BASE + 17);
		private const int   MMSYSERR_DELETEERROR	= (MMSYSERR_BASE + 18);
		private const int   MMSYSERR_VALNOTFOUND	= (MMSYSERR_BASE + 19);
		private const int   MMSYSERR_NODRIVERCB		= (MMSYSERR_BASE + 20);
		private const int	WAVERR_BADFORMAT		= 32;
		private const int   WAVERR_STILLPLAYING		= 33;
		private const int   WAVERR_UNPREPARED		= 34;
		private const int   WAVERR_SYNC				= 35;
		
		internal const int	MaxBuffers			= 10;
		internal const int	BufferLen			= 5;
		internal const int	WAVE_FORMAT_PCM		= 1;
		internal const int	CALLBACK_WINDOW		= 0x0010000; 
		internal const uint	WAVE_MAPPER			= uint.MaxValue;
		internal const int	WAVE_FORMAT_QUERY	= 0x00000001;

		internal Queue<WaveHeader>		m_qBuffers;
		internal System.Collections.Hashtable	m_HandleMap;
		internal int		m_deviceID		= 0;
		

		internal static void CheckWaveError(int ErrorNumber)
		{
			switch(ErrorNumber)
			{
				case MMSYSERR_NOERROR:
					return;
				case MMSYSERR_ERROR:
					throw new Exception("General multimedia error");
				case MMSYSERR_BADDEVICEID:
					throw new Exception("Bad or invalid wave device");
				case MMSYSERR_NOTENABLED:
					throw new Exception("Device not enabled");
				case MMSYSERR_ALLOCATED:
					throw new Exception("Device already allocated");
				case MMSYSERR_INVALHANDLE:
					throw new Exception("Invalid device handle");
				case MMSYSERR_NODRIVER:
					throw new Exception("No device driver found");
				case MMSYSERR_NOMEM:
					throw new Exception("Not enough memory for requested operation");
				case MMSYSERR_NOTSUPPORTED:
					throw new Exception("Request not supported");
				case MMSYSERR_BADERRNUM:
					throw new Exception("Bad error number");
				case MMSYSERR_INVALFLAG:
					throw new Exception("Invalid flag");
				case MMSYSERR_INVALPARAM:
					throw new Exception("Invalid parameter");
				case MMSYSERR_HANDLEBUSY:
					throw new Exception("Handle is currently busy");
				case MMSYSERR_INVALIDALIAS:
					throw new Exception("Invalid Alias");
				case MMSYSERR_BADDB:
					throw new Exception("Bad DB");
				case MMSYSERR_KEYNOTFOUND:
					throw new Exception("Key not found");
				case MMSYSERR_READERROR:
					throw new Exception("Read error");
				case MMSYSERR_WRITEERROR:
					throw new Exception("Write error");
				case MMSYSERR_DELETEERROR:
					throw new Exception("Delete error");
				case MMSYSERR_VALNOTFOUND:
					throw new Exception("Value not found");
				case MMSYSERR_NODRIVERCB:
					throw new Exception("No driver");
				case WAVERR_BADFORMAT:
					throw new Exception("Unsupported wave format");
				case WAVERR_STILLPLAYING:
					throw new Exception("Sound still playing");
				case WAVERR_UNPREPARED:
					throw new Exception("Wave header has not been prepared");
				case WAVERR_SYNC:
					throw new Exception("Wave sync error");
				default:
					throw new Exception("General multimedia error");
			}
		}
	}
}
