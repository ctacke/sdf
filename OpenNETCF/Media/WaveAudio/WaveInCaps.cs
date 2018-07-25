using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenNETCF.Media.WaveAudio
{
	/// <summary>
	/// Class for getting audio device capabilities.
	/// </summary>
	public class WaveInCaps
	{
		private const int MAXPNAMELEN = 32;
			
		private const int wMIDOffset			= 0;
		private const int wPIDOffset			= wMIDOffset + 2;
		private const int vDriverVersionOffset	= wPIDOffset + 2;
		private const int szPnameOffset			= vDriverVersionOffset + 4;
		private const int dwFormatsOffset		= szPnameOffset + MAXPNAMELEN * 2;
		private const int wChannelsOffset		= dwFormatsOffset + 4;
		private const int wReserved1Offset		= wChannelsOffset + 2;

		private byte[] flatStruct = new byte[2 + 2 + 4 + MAXPNAMELEN * 2 + 4 + 2 + 2];

		public byte[] ToByteArray()
		{
			return flatStruct;
		}

		public static implicit operator byte[]( WaveInCaps wic )
		{
			return wic.flatStruct;
		}

		public WaveInCaps()
		{
			Array.Clear(flatStruct, 0, flatStruct.Length);
		}

		public WaveInCaps( byte[] bytes ) : this( bytes, 0 )
		{
		}

		public WaveInCaps( byte[] bytes, int offset )
		{
			Buffer.BlockCopy( bytes, offset, flatStruct, 0, flatStruct.Length );
		}
			
		public short MID
		{
			get
			{
				return BitConverter.ToInt16(flatStruct, wMIDOffset);
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, wMIDOffset, Marshal.SizeOf(value));
			}
		}

		public short PID
		{
			get
			{
				return BitConverter.ToInt16(flatStruct, wPIDOffset);
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, wPIDOffset, Marshal.SizeOf(value));
			}
		}

		public int DriverVersion
		{
			get
			{
				return BitConverter.ToInt32(flatStruct, vDriverVersionOffset);
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, vDriverVersionOffset, Marshal.SizeOf(value));
			}
		}

		public string szPname
		{
			get
			{
				return Encoding.Unicode.GetString(flatStruct, szPnameOffset, MAXPNAMELEN * 2).Trim('\0');
			}
		}
		
		public int Formats
		{
			get
			{
				return BitConverter.ToInt32(flatStruct, dwFormatsOffset);
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, dwFormatsOffset, Marshal.SizeOf(value));
			}
		}

		public short Channels
		{
			get
			{
				return BitConverter.ToInt16(flatStruct, wChannelsOffset);
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, wChannelsOffset, Marshal.SizeOf(value));
			}
		}

		public short wReserved1
		{
			get
			{
				return BitConverter.ToInt16(flatStruct, wReserved1Offset);
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, wReserved1Offset, Marshal.SizeOf(value));
			}
		}
	}
}
