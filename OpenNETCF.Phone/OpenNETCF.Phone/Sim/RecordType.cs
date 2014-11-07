using System;

namespace OpenNETCF.Phone.Sim
{
	/// <summary>
	/// Specifies different SIM file types.
	/// </summary>
	public enum RecordType : int
	{
		/// <summary>
		/// An unknown record type.
		/// </summary>
		Unknown          = 0x00000000,
		/// <summary>
		/// A single veriable lengthed record.
		/// </summary>
		Transparent      = 0x00000001,
		/// <summary>
		/// A cyclic set of records, each of the same length.
		/// </summary>
		Cyclic           = 0x00000002,
		/// <summary>
		/// A linear set of records, each of the same length.
		/// </summary>
		Linear           = 0x00000003, 
		/// <summary>
		/// Every SIM has a single master record, effectively the head node.
		/// </summary>
		Master           = 0x00000004,
		/// <summary>
		/// Effectively a "directory" file which is a parent of other records.
		/// </summary>
		Dedicated        = 0x00000005,
	}
}
