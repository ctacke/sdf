using System;

namespace OpenNETCF.Diagnostics
{
	/// <summary>
	/// Summary description for EventLogEntry.
	/// </summary>
	public class EventLogEntry
	{
		private short categoryNumber;
		private byte[] data;
		private EventLogEntryType entryType;
		private int eventID;
		private int index;
		private string machineName;
		private string message;
		private string source;
		private DateTime timeGenerated;
		private DateTime timeWritten;
		private string userName;
		private string id;

		internal EventLogEntry(short categoryNumber, byte[] data, EventLogEntryType entryType,
			int eventID, int index, string machineName, string message,
			string source, DateTime timeGenerated, DateTime timeWritten,
			string userName, string id)
		{
            this.categoryNumber = categoryNumber;
			this.data = data;
			this.entryType = entryType;
			this.eventID = eventID;
			this.index = index;
			this.machineName = machineName;
			this.message = message;
			this.source = source;
			this.timeGenerated = timeGenerated;
			this.timeWritten = timeWritten;
			this.userName = userName;
			this.id = id;
		}


		#region Public Properties

		/// <summary>
		/// Gets the ID value that uniquely identifies the item in the log
		/// </summary>
		public string ID
		{
			get
			{
				return this.id;
			}
		}

		/// <summary>
		/// Gets the text associated with the <see cref="CategoryNumber">CategoryNumber</see> for this entry.
		/// </summary>
		/// <value>The application-specific category text.</value>
		/// <remarks>
		/// Each application (event source) can define its own numbered categories and the text strings to which they are mapped. The text strings associated with the category are stored in the XML Log.
		/// </remarks>
		public string Category
		{
			get
			{
				return this.categoryNumber.ToString();
			}
		}

		/// <summary>
		/// Gets the entry's category number.
		/// </summary>
		/// <value>The application-specific category number for this entry .</value>
		/// <remarks>Each application (event source) can define its own numbered categories and the text strings to which they are mapped.</remarks>
		public short CategoryNumber
		{
			get
			{
				return this.categoryNumber;
			}
		}

		/// <summary>
		/// Gets the binary data associated with the entry.
		/// </summary>
		/// <value>An array of bytes that holds the binary data associated with the entry.</value>
		/// <remarks>Getting this property creates an array that holds a copy of the entry's event-specific binary data. Event-specific data is sometimes used to store information that the application will process independently of the Event Viewer, for example, to make reports from the log file.</remarks>
		public byte[] Data
		{
			get
			{
				return this.data;
			}
		}

		/// <summary>
		/// Gets the event type of this entry.
		/// </summary>
		/// <value>The <seealso cref="EventLogEntryType">EventLogEntryType</seealso> that indicates the event type associated with the entry in the event log. This is usually used to indicate the severity of the event log entry.</value>
		public EventLogEntryType EntryType
		{
			get
			{
				return this.entryType;
			}
		}

		/// <summary>
		/// Gets the application-specific event identifier of this event entry.
		/// </summary>
		/// <value>The application-specific identifier for the event.</value>
		/// <remarks>Event identifiers, together with the event source, uniquely identify an event.</remarks>
		public int EventID
		{
			get
			{
				return this.eventID;
			}
		}

		/// <summary>
		/// Gets the index of this entry in the event log.
		/// </summary>
		/// <value>The index of this entry in the event log.</value>
		/// <remarks>This number is not necessarily zero based.</remarks>
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		/// <summary>
		/// Gets the name of the computer on which this entry was generated.
		/// </summary>
		/// <value>The name of the computer that contains the event log.</value>
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
		}

		/// <summary>
		/// Gets the localized message associated with this event entry.
		/// </summary>
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		/// <summary>
		/// Gets the name of the application that generated this event.
		/// </summary>
		/// <remarks>The event source indicates what logged the event. It is often the name of the application, or the name of a subcomponent of the application if the application is large. Applications and services usually write to (and therefore are sources for) the Application log or a custom log. Device drivers usually write to the System log.</remarks>
		public string Source
		{
			get
			{
				return this.source;
			}
		}

		/// <summary>
		/// Gets the local time at which this event was generated.
		/// </summary>
		/// <value>A <see cref="System.DateTime">DateTime</see> that represents the local time at which this event was generated.</value>
		/// <remarks>This member holds the time that an event was generated. This might not be the same as the time when the event information was written to the event log. For the latter, read the <see cref="TimeWritten">TimeWritten</see> property.There’s almost always going to be a lag between the time something happens and the time it is logged, if only milliseconds. Usually, it is more important to know when the event was generated, unless you want to see if there is a significant lag in logging. That can happen if your log files are on a different server and you are experiencing a bottleneck.</remarks>
		public DateTime TimeGenerated
		{
			get
			{
				return this.timeGenerated;
			}
		}

		/// <summary>
		/// Gets the local time at which this event was written to the log.
		/// </summary>
		/// <value>A <see cref="System.DateTime">DateTime</see> that represents the local time at which this event was written to the log.</value>
		/// <remarks>This member holds the time that an event's information is written to the event log. This might not be the same time as when the event was generated. For the latter, read the <see cref="TimeGenerated">TimeGenerated</see> property.</remarks>
		public DateTime TimeWritten
		{
			get
			{
				return this.timeWritten;
			}
		}
		
		/// <summary>
		/// Gets the name of the user who's responsible for this event.
		/// </summary>
		public string UserName
		{
			get
			{
				return this.userName;
			}
		}


		#endregion

		#region Public Methods
		#endregion
	}
}
