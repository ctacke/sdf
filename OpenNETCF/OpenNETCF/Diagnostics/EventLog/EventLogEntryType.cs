using System;

namespace OpenNETCF.Diagnostics
{
	/// <summary>
	/// Specifies the event type of an event log entry.
	/// </summary>
	/// <remarks>
	/// The type of an event log entry is used to indicate the severity of an event log entry.
	/// Each event must be of a single type, which the application indicates when it reports the event.
	/// </remarks>
	public enum EventLogEntryType
	{
		/// <summary>
		/// An error event. This indicates a significant problem the user should know about; usually a loss of functionality or data.
		/// </summary>
		Error,
		/// <summary>
		/// A failure audit event. This indicates a security event that occurs when an audited access attempt fails; for example, a failed attempt to open a file.
		/// </summary>
		FailureAudit,
		/// <summary>
		/// An information event. This indicates a significant, successful operation.
		/// </summary>
		Information,
		/// <summary>
		/// A success audit event. This indicates a security event that occurs when an audited access attempt is successful; for example, logging on successfully.
		/// </summary>
		SuccessAudit,
		/// <summary>
		/// A warning event. This indicates a problem that is not immediately significant, but that may signify conditions that could cause future problems.
		/// </summary>
		Warning
	}
}
