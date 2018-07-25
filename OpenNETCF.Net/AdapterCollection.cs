using System;
using System.Collections;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Class that represents a collection of network adapters 
	/// connected to the Windows CE device.  Each adapter can 
	/// be queried for information such as the signal strength 
	/// (RF only), the activity state, etc.
	/// </summary>
    [Obsolete("This class is obsolete and will be removed in a future version of the SDF.  Consider using OpenNETCF.Net.NetworkInformation.NetworkInterface or one of its subclasses instead", false)]
    public class AdapterCollection : CollectionBase
	{
		/// <summary>
		/// Constructor loads the list by querying the 
		/// operating system for the list of adapters which
		/// are currently connected.
		/// </summary>
		internal AdapterCollection()
		{
			this.Refresh();
		}

		/// <summary>
		/// Clears and reconstructs the list of adapters,
		/// based on the current hardware connections to the
		/// device.
		/// </summary>
		unsafe void Refresh()
		{
			// Clear the existing list, if any.
			this.Clear();

			// Get the list of adapters, in the form of an
			// IP_ADAPTER_INFO object.
			IP_ADAPTER_INFO	ipinfo = new IP_ADAPTER_INFO();

			// For each adapter index, get the adapter 
			// information.  This is done in the fixed 
			// context, as the base address is saved when
			// you do this.
			Adapter	adap = ipinfo.FirstAdapter();
			while ( adap != null )
			{
				// Add the new item to our list.
				this.List.Add( adap );

				// Get the next adapter information.
				adap = ipinfo.NextAdapter();
			}
		}

		/// <summary>
		/// Indexer for contained Adapters
		/// </summary>
		public Adapter this[int index]
		{
			get
			{
				return (Adapter)List[ index ];;
			}
		}
	}
}