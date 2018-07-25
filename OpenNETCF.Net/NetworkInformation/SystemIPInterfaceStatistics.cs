using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net.NetworkInformation
{
	internal class SystemIPInterfaceStatistics : IPInterfaceStatistics
	{
        MibIfRow mif;

		internal SystemIPInterfaceStatistics(ulong index)
		{
        	mif = new MibIfRow();

			mif.Index = (uint)index;

			int s = MibIfRow.GetSize();
			int code = GetIfEntry(mif.getBytes(), ref s);
			if (code != 0) 
				throw new NetworkInformationException(code);
		}

		public override long BytesReceived
		{
            get
            {
                return mif.InOctets;            	
            }
		}

		public override long BytesSent
		{
			get
			{
				return mif.OutOctets;
				
			}
		}

		public override long IncomingPacketsDiscarded
		{
			get
			{
				return mif.InDiscards;
			}
		}

		public override long IncomingPacketsWithErrors
		{
			get
			{
				return mif.InErrors;
			}
		}

		public override long IncomingUnknownProtocolPackets
		{
			get
			{
				return mif.InUnknownProtos;
			}
		}

		public override long NonUnicastPacketsReceived
		{
			get
			{
				return mif.InUcastPkts;
			}
		}

		public override long NonUnicastPacketsSent
		{
			get
			{
				return mif.InNUcastPkts;
			}
		}

		public override long OutgoingPacketsDiscarded 
		{ 
			get
			{
				return mif.OutDiscards;
			}
		}

		public override long OutgoingPacketsWithErrors
		{
			get
			{
				return mif.OutErrors;
			}
		}

		public override long UnicastPacketsReceived
		{
			get
			{
				return mif.OutUcastPkts;
			}
		}

		public override long UnicastPacketsSent
		{
			get
			{
				return mif.OutNUcastPkts;
			}
		}

		public override long OutputQueueLength
		{
			get
			{
				return mif.OutQueueLength;
			}
		}




		internal long Mtu
		{
			get
			{
				throw new System.NotSupportedException();
			}
		}

		internal OperationalStatus OperationalStatus
		{
			get
			{
				throw new System.NotSupportedException();
			}
		}

		internal long Speed
		{
			get
			{
				throw new System.NotSupportedException();
			}
		}

		[DllImport ("iphlpapi.dll", SetLastError=true)]
		internal static extern int GetIfEntry( byte[] ip, ref int size );
	}
}
