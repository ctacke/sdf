using System;

namespace OpenNETCF.Diagnostics
{
	/// <summary>
	/// Event arguments for the last entry written to the log EntryWrittenEventArgs.
	/// </summary>
	public class EntryWrittenEventArgs : System.EventArgs
	{
		/// <summary>
		/// The entry that was just written to the log
		/// </summary>
		private EventLogEntry eventLogEntry;

		/// <summary>
		/// Default constructor
		/// </summary>
		public EntryWrittenEventArgs(EventLogEntry eventLogEntry)
		{
			this.eventLogEntry = eventLogEntry;
		}

		/// <summary>
		/// The event log entry that was written to the log
		/// </summary>
		public EventLogEntry EventLogEntry
		{
			get
			{
				return this.eventLogEntry;
			}
		}

	}
}
