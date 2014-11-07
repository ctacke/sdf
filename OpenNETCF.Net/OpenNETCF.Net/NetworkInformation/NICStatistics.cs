using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net.NetworkInformation
{
	#region Enumerations

	internal enum NDISMediaTypes
	{
		NdisMedium802_3,
		NdisMedium802_5,
		NdisMediumFddi,
		NdisMediumWan,
		NdisMediumLocalTalk,
		NdisMediumDix,              // defined for convenience, not a real medium
		NdisMediumArcnetRaw,
		NdisMediumArcnet878_2,
		NdisMediumAtm,
		NdisMediumWirelessWan,
		NdisMediumIrda,
		NdisMediumBpc,
		NdisMediumCoWan,
		NdisMedium1394,
		NdisMediumInfiniBand,
		NdisMediumTunnel,
		NdisMediumNative802_11,
		NdisMediumMax               // Not a real medium, defined as an upper-bound
	}

	internal enum NDISMediaStates
	{
		MEDIA_STATE_CONNECTED = 0,
		MEDIA_STATE_DISCONNECTED = 1,
		MEDIA_STATE_UNKNOWN = -1
	}

	internal enum NDISDeviceStates
	{
		DEVICE_STATE_CONNECTED = 1,
		DEVICE_STATE_DISCONNECTED = 0
	}

	internal enum NDISPhysicalMedium
	{
		NdisPhysicalMediumUnspecified,
		NdisPhysicalMediumWirelessLan,
		NdisPhysicalMediumCableModem,
		NdisPhysicalMediumPhoneLine,
		NdisPhysicalMediumPowerLine,
		NdisPhysicalMediumDSL,      // includes ADSL and UADSL (G.Lite)
		NdisPhysicalMediumFibreChannel,
		NdisPhysicalMedium1394,
		NdisPhysicalMediumWirelessWan,
		NdisPhysicalMediumNative802_11,
		NdisPhysicalMediumMax       // Not a real physical type, defined as an upper-bound
	}

	#endregion

	internal class NICStatistics : IDisposable
	{
		/*
4    ULONG               Size;               //	Of this structure.
4	 PTCHAR				 ptcDeviceName;		//	The device name to be queried..
4    ULONG               DeviceState;        //	DEVICE_STATE_XXX above
4    ULONG               MediaType;          //	NdisMediumXXX
4    ULONG               MediaState;			//	MEDIA_STATE_XXX above
4    ULONG               PhysicalMediaType;
4    ULONG               LinkSpeed;          //	In 100bits/s. 10Mb/s = 100000
8    ULONGLONG           PacketsSent;
8    ULONGLONG           PacketsReceived;
4    ULONG               InitTime;           //	In milliseconds
4    ULONG               ConnectTime;        //	In seconds
8    ULONGLONG           BytesSent;          //	0 - Unknown (or not supported)
8    ULONGLONG           BytesReceived;      //	0 - Unknown (or not supported)
8    ULONGLONG           DirectedBytesReceived;
8    ULONGLONG           DirectedPacketsReceived;
4    ULONG               PacketsReceiveErrors;
4    ULONG               PacketsSendErrors;
4    ULONG               ResetCount;
4    ULONG               MediaSenseConnectCount;
4    ULONG               MediaSenseDisconnectCount;
		*/

		protected byte[] m_data;
        protected int ourSize;

		protected const int SizeOffset = 0;
        public int Size
        {
            get { return ourSize; }
        }

        protected const int NIC_STATISTICS_SIZE = 112;
		public NICStatistics()
        {
			ourSize = NIC_STATISTICS_SIZE;
            m_data = new byte[ourSize];
			byte[] bytes = BitConverter.GetBytes((UInt32)0);
			Buffer.BlockCopy(bytes, 0, m_data, SizeOffset, 4);
        }

        protected const int DeviceNameOffset = SizeOffset + 4;
		protected IntPtr deviceName = IntPtr.Zero;

		/// <summary>
		/// DeviceName must be filled-in prior to requesting the statistics from
		/// NDISUIO.  It should be set to the interface name (the adapter name).
		/// </summary>
        public unsafe string DeviceName
        {
            get
            {
				return Marshal.PtrToStringUni(deviceName);
            }
            set
            {
				if (deviceName != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(deviceName);
				}

				deviceName = Marshal.AllocHGlobal((value.Length + 1) * 2);
				byte[] chars = Encoding.Unicode.GetBytes(value + '\0');
				Marshal.Copy(chars, 0, deviceName, chars.Length);

				// Store the pointer in the byte-based data array.
				byte[] bytes = BitConverter.GetBytes((uint)deviceName);
				Array.Copy(bytes, 0, m_data, DeviceNameOffset, 4);
            }
        }

        protected const int DeviceStateOffset = DeviceNameOffset + 4;
        public NDISDeviceStates DeviceState
        {
            get
            {
                return (NDISDeviceStates)BitConverter.ToUInt32(m_data, DeviceStateOffset);
            }
        }

		protected const int MediaTypeOffset = DeviceStateOffset + 4;
		public NDISMediaTypes MediaType
		{
			get
			{
				return (NDISMediaTypes)BitConverter.ToUInt32(m_data, MediaTypeOffset);
			}
		}

		protected const int MediaStateOffset = MediaTypeOffset + 4;
		public NDISMediaStates MediaState
		{
			get
			{
				return (NDISMediaStates)BitConverter.ToUInt32(m_data, MediaStateOffset);
			}
		}

		protected const int PhysicalMediaTypeOffset = MediaStateOffset + 4;
		public NDISPhysicalMedium PhysicalMediaType
		{
			get
			{
				return (NDISPhysicalMedium)BitConverter.ToUInt32(m_data, PhysicalMediaTypeOffset);
			}
		}

		protected const int LinkSpeedOffset = PhysicalMediaTypeOffset + 4;
		/// <summary>
		/// LinkSpeed is the speed of physical network connections, in 
		/// bits-per-second.  For example, 10BaseT Ethernet would return 
		/// 10,000,000.
		/// </summary>
		public uint LinkSpeed
		{
			get
			{
				return BitConverter.ToUInt32(m_data, LinkSpeedOffset) * 100;
			}
		}

		protected const int PacketsSentOffset = LinkSpeedOffset + 4;	// 32
		public ulong PacketsSent
		{
			get
			{
				return BitConverter.ToUInt64(m_data, PacketsSentOffset);
			}
		}

		protected const int PacketsReceivedOffset = PacketsSentOffset + 8;	// 40
		public ulong PacketsReceived
		{
			get
			{
				return BitConverter.ToUInt64(m_data, PacketsReceivedOffset);
			}
		}

		protected const int InitTimeOffset = PacketsReceivedOffset + 8;	// 48
		public uint InitTime
		{
			get
			{
				return BitConverter.ToUInt32(m_data, InitTimeOffset);
			}
		}

		protected const int ConnectTimeOffset = InitTimeOffset + 4;	// 52
		public uint ConnectTime
		{
			get
			{
				return BitConverter.ToUInt32(m_data, ConnectTimeOffset);
			}
		}

		// These are the remaining fields.  We do not use these values, at this
		// time, so the property values have not been created and the code to 
		// retrieve the values has not been generated.  The offsets could be
		// used for that, however.
		protected const int BytesSentOffset = ConnectTimeOffset + 4;	// 56
		protected const int BytesReceivedOffset = BytesSentOffset + 8;	// 64
		protected const int DirectedBytesReceivedOffset = BytesReceivedOffset + 8;	// 72
		protected const int DirectedPacketsReceivedOffset = DirectedBytesReceivedOffset + 8;	// 80
		protected const int PacketsReceiveErrorsOffset = DirectedPacketsReceivedOffset + 8;	// 88
		protected const int PacketsSendErrorsOffset = PacketsReceiveErrorsOffset + 4;	// 92
		protected const int ResetCountOffset = PacketsSendErrorsOffset + 4;	// 96
		protected const int MediaSenseConnectCountOffset = ResetCountOffset + 4;	// 100
		protected const int MediaSenseDisconnectCountOffset = MediaSenseConnectCountOffset + 4;	// 104

		// Methods used to get the data from the class.
		public byte[] getBytes()
        {
            return m_data;
        }

		public static implicit operator byte[](NICStatistics stat)
        {
            return stat.m_data;
        }

		#region IDisposable Members

		public void Dispose()
		{
			if (deviceName != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.deviceName);
			}
		}

		#endregion
	}

}
