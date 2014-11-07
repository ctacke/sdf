using System;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Power line status.
	/// </summary>
	/// <remarks>Used by <see cref="PowerStatus"/> class.</remarks>
	public enum PowerLineStatus : byte
	{
		/// <summary>
		/// AC power is offline.
		/// </summary>
		Offline         = 0x00,
		/// <summary>
		/// AC power is online.
		/// </summary>
		Online			= 0x01,
		/// <summary>
		/// Unit is on backup power.
		/// </summary>
		BackupPower		= 0x02,
		/// <summary>
		/// AC line status is unknown.
		/// </summary>
		Unknown			= 0xFF,
	}
}
