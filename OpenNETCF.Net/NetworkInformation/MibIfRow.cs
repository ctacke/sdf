using System;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
	internal class MibIfRow
	{
		byte [] data;

		internal MibIfRow()
		{
			this.data = new byte[GetSize()];
		}

		internal static int GetSize()
		{
			return 640;
		}

		const int NameOffset = 0;
		internal string InterfaceName
		{
			get
			{
				char[] ch = new char[1];
				return Encoding.Unicode.GetString(data, NameOffset, (MAX_INTERFACE_NAME_LEN * 2)).TrimEnd(ch);
			}
		}

		const int IndexOffset = NameOffset + MAX_INTERFACE_NAME_LEN * 2;
		/// <summary>
		/// Interface index.
		/// </summary>
		internal uint Index
		{
			get { return BitConverter.ToUInt32( data, IndexOffset ); }
			set 
			{ 
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, data, IndexOffset, 4 );
			}
		}

		const int TypeOffset = IndexOffset + 4;
		/// <summary>
		/// Specifies the type of interface.
		/// </summary>
		internal InterfaceType Type
		{
			get
			{
				return (InterfaceType)BitConverter.ToUInt32( data, TypeOffset );
			}
		}

		const int MtuNOtherOffset = TypeOffset + 4 + (3 * 4) + MAXLEN_PHYSADDR;
		const int AdminStatus = MtuNOtherOffset;
		const int OperationalStatusOffset = AdminStatus + 4;
		/// <summary>
		/// Specifies the operational status of the interface.
		/// </summary>
		internal OperationalStatus OperationalStatus
		{
			get
			{
				return (OperationalStatus)BitConverter.ToUInt32( data, OperationalStatusOffset );
			}
		}

		const int LastChangeOffset = OperationalStatusOffset + 4;

		const int InOctetsOffset = LastChangeOffset + 4;
		/// <summary>
		/// Specifies the number of octets of data received through this interface.
		/// </summary>
		internal uint InOctets
		{
			get 
			{ 
				return BitConverter.ToUInt32( data, InOctetsOffset ); 
			}
		}

		const int InUcastPktsOffset = InOctetsOffset + 4;
		/// <summary>
		/// Specifies the number of unicast packets received through this interface.
		/// </summary>
		internal uint InUcastPkts
		{
			get 
			{ 
				return BitConverter.ToUInt32( data, InUcastPktsOffset ); 
			}
		}

		const int InNUcastPktsOffset = InUcastPktsOffset + 4;
		/// <summary>
		/// Specifies the number of non-unicast packets received through this interface. This includes broadcast and multicast packets.
		/// </summary>
		internal uint InNUcastPkts
		{
			get 
			{ 
				return BitConverter.ToUInt32( data, InNUcastPktsOffset ); 
			}
		}

		const int InDiscardsOffset = InNUcastPktsOffset + 4;
		/// <summary>
		/// Specifies the number of incoming packets that were discarded even though they did not have errors.
		/// </summary>
		internal uint InDiscards
		{
			get 
			{ 
				return BitConverter.ToUInt32( data, InDiscardsOffset ); 
			}
		}

		const int InErrorsOffset = InDiscardsOffset + 4;
		/// <summary>
		/// Specifies the number of incoming packets that were discarded because of errors.
		/// </summary>
		internal uint InErrors
		{
			get 
			{ 
				return BitConverter.ToUInt32( data, InErrorsOffset ); 
			}
		}

		const int InUnknownProtosOffset = InErrorsOffset + 4;
		/// <summary>
		/// Specifies the number of incoming packets that were discarded because the protocol was unknown.
		/// </summary>
		internal uint InUnknownProtos
		{
			get
			{
				return BitConverter.ToUInt32( data, InUnknownProtosOffset ); 
			}
		}


		const int OutOctetsOffset = InUnknownProtosOffset + 4;
		/// <summary>
		/// Specifies the number of octets of data sent through this interface.
		/// </summary>
		internal uint OutOctets
		{
			get 
			{ 
				return BitConverter.ToUInt32( data, OutOctetsOffset ); 
			}
		}

		const int OutUcastPktsOffset = OutOctetsOffset + 4;
		/// <summary>
		/// Specifies the number of unicast packets sent through this interface.
		/// </summary>
		internal uint OutUcastPkts
		{
			get 
			{ 
				return BitConverter.ToUInt32( data, OutUcastPktsOffset ); 
			}
		}

		const int OutNUcastPktsOffset = OutUcastPktsOffset + 4;
		/// <summary>
		/// Specifies the number of non-unicast packets sent through this interface. This includes broadcast and multicast packets.
		/// </summary>
		internal uint OutNUcastPkts
		{
			get 
			{ 
				return BitConverter.ToUInt32( data, OutNUcastPktsOffset ); 
			}
		}

		const int OutDiscardsOffset = OutNUcastPktsOffset + 4;
		/// <summary>
		/// Specifies the number of outgoing packets that were discarded even though they did not have errors.
		/// </summary>
		internal uint OutDiscards
		{
			get 
			{ 
				return BitConverter.ToUInt32( data, OutDiscardsOffset ); 
			}
		}

		const int OutErrorsOffset = OutDiscardsOffset + 4;
		/// <summary>
		/// Specifies the number of outgoing packets that were discarded because of errors.
		/// </summary>
		internal uint OutErrors
		{
			get 
			{ 
				return BitConverter.ToUInt32( data, OutErrorsOffset ); 
			}
		}

		const int OutQueueLengthOffset = OutErrorsOffset + 4;
		/// <summary>
		/// Specifies outgoing queue length.
		/// </summary>
		internal uint OutQueueLength
		{
			get 
			{ 
				return BitConverter.ToUInt32( data, OutQueueLengthOffset ); 
			}
		}

		internal byte[] getBytes()
		{
			return data;
		}

		const int MAX_INTERFACE_NAME_LEN = 256;
		const int MAXLEN_PHYSADDR = 8;
		const int MAXLEN_IFDESCR = 256;
	}
}
