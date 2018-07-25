
using System;
using OpenNETCF.Runtime.InteropServices;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Contains information about a specific network.
	/// </summary>
	public class DestinationInfo
	{
		private object syncRoot = new object();

		/// <summary>
		/// Size of the DestinationInfo structure in unmanaged memory.
		/// </summary>
		internal static int NativeSize = 272;

		/// <summary>
		/// The destination's GUID identifier.
		/// </summary>
		public Guid Guid;

		/// <summary>
		/// The destination's description.
		/// </summary>
		public string Description = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DestinationInfo()
		{
		}

		/// <summary>
		/// Creates a new instance of DestinationInfo at the specific memory address.
		/// </summary>
		/// <param name="baseAddr">Memory address where the DestinationInfo object should be created.</param>
		public DestinationInfo(IntPtr baseAddr)
		{
			lock(syncRoot)
			{
				Guid = new Guid(Marshal2.ReadByteArray(baseAddr, 0, 16));	

				//Bug 144 - Turns out that calling PtrToStringUni in quick succession was causing a coredll.dll exception
				//			Now using PtrToStringAuto and not searching for null char	
				if (Marshal2.IsSafeToRead(new IntPtr(baseAddr.ToInt32() + 16), 256))
				{
					Description = Marshal2.PtrToStringAuto(new IntPtr(baseAddr.ToInt32() + 16));
				}
				
			}
		}
	}
}
