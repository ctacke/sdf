using System;
using System.Xml;
using System.Reflection;
//using System.Data;
using System.IO;


namespace OpenNETCF.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
	public delegate void EntryWrittenEventHandler(object sender, EventLogEntry e);
	/// <summary>
	/// Class is similar to the System.Diagnostics.EventLog in the full framework with a few differences.
	/// 1. Since CE.Net and PPC have no event log we write the log to the application root directory as an XML file
	/// 2. Every Application will have it's own log.  There will not be one log system/device wide
	/// 3. Attempted to keep as close as possible to the full framework class but some things are missing
	/// </summary>
	public class EventLog
	{
		/// <summary>
		/// Public event to notify listeners that the log has changed
		/// </summary>
		public event EventHandler LogChanged;
		/// <summary>
		/// Public event to notify listeners that the log display name has changed
		/// </summary>
		public event EventHandler LogDisplayNameChanged;
		/// <summary>
		/// Public event to notify listeners that the log has been closed
		/// </summary>
		public event EventHandler LogClosed;
		/// <summary>
		/// Public event to notify listeners that the log has been cleared
		/// </summary>
		public event EventHandler LogCleared;
		/// <summary>
		/// Public event to notify listeners that the source has changed
		/// </summary>
		public event EventHandler SourceChanged;
		/// <summary>
		/// Notifies listeners if there is a new log in the eventLog
		/// </summary>
		public event EventHandler EventLogAdded;

		/// <summary>
		/// Occurs when an entry is written to an event log on the local computer
		/// </summary>
		public event EntryWrittenEventHandler EntryWritten;		
		private bool enableRaisingEvents = false;
		private EventLogWriterType eventLogWriterType = EventLogWriterType.XML;
		private	IEventLogWriter eventLogWriter;

		/// <summary>
		/// Overloaded constructor were a custom IEventLogWriter can be specified
		/// </summary>
		/// <param name="log">Indicates the log item</param>
		/// <param name="source">Indicates what logged the event</param>
		/// <param name="customEventLogWriter">Custom event log writter which implements IEventLogWriter</param>
		public EventLog(string log, string source, IEventLogWriter customEventLogWriter)
		{
			string logFileName = "";
			string logPath = "";
			//Setup the file and path
			if(logFileName.Length==0)
			{
				logFileName = Assembly.GetCallingAssembly().GetName().CodeBase + ".Log";
				logFileName = logFileName.Substring(logFileName.LastIndexOf("\\")+1);
			}
			if(logPath.Length==0)
			{
				logPath = Assembly.GetCallingAssembly().GetName().CodeBase;
				logPath = Path.GetDirectoryName(logPath) + "\\";
			}

			//Load the log file
			this.eventLogWriterType = EventLogWriterType.Custom;
			this.eventLogWriter = customEventLogWriter;
			this.CreateEventLogWriter(log, source, logPath, logFileName);
		}
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="log">Indicates the log item</param>
		/// <param name="source">Indicates what logged the event</param>
		/// <param name="type"></param>
		public EventLog(string log, string source, EventLogWriterType type)
		{
			string logFileName = "";
			string logPath = "";
			//Setup the file and path
			if(logFileName.Length==0)
			{
				logFileName = Assembly.GetCallingAssembly().GetName().CodeBase + ".Log";
				logFileName = logFileName.Substring(logFileName.LastIndexOf("\\")+1);
			}
			if(logPath.Length==0)
			{
				logPath = Assembly.GetCallingAssembly().GetName().CodeBase;
				logPath = Path.GetDirectoryName(logPath) + "\\";
			}

			//Load the log file
			this.eventLogWriterType = type;
			this.CreateEventLogWriter(log, source, logPath, logFileName);
		}

		/// <summary>
		/// Overloaded constructor
		/// </summary>
		/// <param name="log">Indicates the log item</param>
		/// <param name="source">Indicates what logged the event</param>
		/// <param name="path"></param>
		/// <param name="fileName"></param>
		/// <param name="type"></param>
		public EventLog(string log, string source,string path, string fileName, EventLogWriterType type)
		{
			//Load the log file
			this.eventLogWriterType = type;
			this.CreateEventLogWriter(log,source,path,fileName);
		}
		

		#region	Public Properties
		/// <summary>
		/// Gets or sets a value indicating whether the EventLog receives EntryWritten event notifications.
		/// </summary>
		public bool EnableRaisingEvents
		{
			get
			{
				return this.enableRaisingEvents;
			}
			set
			{
				this.enableRaisingEvents = value;
			}
		}

		/// <summary>
		/// Gets the contents of the event log.
		/// </summary>
		public EventLogEntryCollection Entries
		{
			get
			{
				return this.eventLogWriter.Entries;
			}
		}

		/// <summary>
		/// Gets or sets the name of the log to read from or write to.
		/// </summary>
		public string Log
		{
			get
			{
				return this.eventLogWriter.Log;
			}
			set
			{
				if(value.Length>0)
				{
					this.eventLogWriter.Log = value;
					this.OnLogChanged();
				}
				else
					throw new Exception("Log name cannot be blank.");
			}
		}

		/// <summary>
		/// Gets the event log's friendly name.
		/// </summary>
		public string LogDisplayName
		{
			get
			{
				return this.eventLogWriter.LogDisplayName;
			}
			set
			{
				this.eventLogWriter.LogDisplayName = value;
				this.OnLogDisplayNameChanged();
			}
		}

		/// <summary>
		/// Gets or sets the source name to register and use when writing to the event log.
		/// </summary>
		public string Source
		{
			get
			{
				return this.eventLogWriter.Source;
			}
			set
			{
				if(value.Length>0)
				{
					this.eventLogWriter.Source = value;
					this.OnSourceChanged();
				}
				else
					throw new Exception("Source cannot be blank.");
			}
		}

		/// <summary>
		/// Gets the file name the log is stored under.  Defaults to the calling assembly name with ".Log" appended
		/// </summary>
		public string LogFileName
		{
			get
			{				
				return this.eventLogWriter.LogFileName;				
			}
		}

		/// <summary>
		/// Gets the path of where the log file is stored
		/// </summary>
		public string LogPath
		{
			get
			{
				return this.eventLogWriter.LogPath;
			}
		}

		/// <summary>
		/// Gets the eventLogWriterType
		/// </summary>
		public EventLogWriterType EventLogWriterType
		{
			get
			{
				return this.eventLogWriterType;
			}
		}
		#endregion
		
		#region Public Methods
		/// <summary>
		/// Removes an event log from the local file.
		/// </summary>
		/// <param name="logName">The name of the log to delete.</param>
		public void Delete(string logName)
		{
			if(this.eventLogWriter.Log.Equals(logName))
				throw new Exception("Cannot delete the log because it is currently open.  You must close the log before deleting.");

			this.eventLogWriter.Delete(logName);
		}

		/// <summary>
		/// Searches for all event logs on the local file and creates an array of EventLog objects that contain the list.
		/// </summary>
		/// <returns>An array of type EventLog that represents the logs on the local computer.</returns>
		public EventLog[] GetEventLogs()
		{
			return this.eventLogWriter.GetEventLogs();
		}

		/// <summary>
		/// Determines whether the log exists on the local file.
		/// </summary>
		/// <param name="logName">The name of the log to search for.</param>
		/// <remarks>The full framework defines this method as static.  Since this is not a system wide log but an application specific log this method will only search for a Log Item with the current XML file.</remarks>
		public bool Exists(string logName)
		{
			return this.eventLogWriter.Exists(logName);
		}
		/// <summary>
		/// Removes all entries from the event log.
		/// </summary>
		public void Clear()
		{
			//Check to see if the log was closed
			if(this.eventLogWriter.Log.Length==0)
				throw new Exception("Unable to clear log because it has been closed.");
			this.eventLogWriter.Clear();
			this.OnLogCleared();			
		}

		/// <summary>
		/// Closes the event log and releases read and write handles.
		/// </summary>
		public void Close()
		{
			//Check to see if the log was closed
			if(this.eventLogWriter.Log.Length==0)
				throw new Exception("Log has already been closed");
			this.eventLogWriter.Close();
			this.OnLogClosed();
		}

		
		/// <summary>
		/// Writes an information type entry, with the given message text, to the event log.
		/// </summary>
		/// <param name="message">The string to write to the event log.</param>
		public void WriteEntry(string message)
		{
			this.CheckForValidLog();
			this.eventLogWriter.WriteEntry(message);
		}
		/// <summary>
		/// Writes an error, warning, information, success audit, or failure audit entry with the given message text to the event log.
		/// </summary>
		/// <param name="message">The string to write to the event log.</param>
		/// <param name="type">One of the <see cref="EventLogEntryType">EventLogEntryType</see> values.</param>
		public void WriteEntry(string message, EventLogEntryType type)
		{
			this.CheckForValidLog();
			this.eventLogWriter.WriteEntry(message,type);
		}
		/// <summary>
		/// Writes an information type entry with the given message text to the event log, using the specified event source.
		/// </summary>
		/// <param name="source">The source by which the application is registered. </param>
		/// <param name="message">The string to write to the event log.</param>
		public void WriteEntry(string source, string message)
		{
			this.CheckForValidLog();
			if(source.Length>0)
				this.eventLogWriter.WriteEntry(source, message);
			else
				this.eventLogWriter.WriteEntry(this.eventLogWriter.Source, message);
		}
		/// <summary>
		/// Writes an entry with the given message text and application-defined event identifier to the event log.
		/// </summary>
		/// <param name="message">The string to write to the event log. </param>
		/// <param name="type">One of the <see cref="EventLogEntryType">EventLogEntryType</see> values. </param>
		/// <param name="eventID">The application-specific identifier for the event. </param>
		public void WriteEntry(string message, EventLogEntryType type, int eventID)
		{
			this.CheckForValidLog();
			this.eventLogWriter.WriteEntry(message,type, eventID);
		}
		/// <summary>
		/// Writes an error, warning, information, success audit, or failure audit entry with the given message text to the event log, using the specified event source.
		/// </summary>
		/// <param name="source">The source by which the application is registered.</param>
		/// <param name="message">The string to write to the event log.</param>
		/// <param name="type">One of the <see cref="EventLogEntryType">EventLogEntryType</see> values.</param>
		public void WriteEntry(string source, string message, EventLogEntryType type)
		{
			this.CheckForValidLog();
			if(source.Length>0)
				this.eventLogWriter.WriteEntry(source, message,type);
			else
				this.eventLogWriter.WriteEntry(this.eventLogWriter.Source, message,type);
		}
		/// <summary>
		/// Writes an entry with the given message text, application-defined event identifier, and application-defined category to the event log.
		/// </summary>
		/// <param name="message">The string to write to the event log.</param>
		/// <param name="type">One of the <see cref="EventLogEntryType">EventLogEntryType</see> values.</param>
		/// <param name="eventID">The application-specific identifier for the event.</param>
		/// <param name="category">The application-specific subcategory associated with the message.</param>
		public void WriteEntry(string message, EventLogEntryType type, int eventID, short category)
		{
			this.CheckForValidLog();
			this.eventLogWriter.WriteEntry(message,type,eventID, category);
		}
		/// <summary>
		/// Writes an entry with the given message text and application-defined event identifier to the event log, using the specified registered event source.
		/// </summary>
		/// <param name="source">The source by which the application is registered on the specified computer. </param>
		/// <param name="message">the string to write to the event log.</param>
		/// <param name="type">One of the <see cref="EventLogEntryType">EventLogEntryType</see> values.</param>
		/// <param name="eventID">The application-specific identifier for the event.</param>
		public void WriteEntry(string source, string message, EventLogEntryType type, int eventID)
		{
			this.CheckForValidLog();
			if(source.Length>0)
				this.eventLogWriter.WriteEntry(source, message, type, eventID);
			else
				this.eventLogWriter.WriteEntry(this.eventLogWriter.Source, message, type, eventID);
		}
		/// <summary>
		/// Writes an entry with the given message text, application-defined event identifier, and application-defined category to the event log, and appends binary data to the message.
		/// </summary>
		/// <param name="message">the string to write to the event log.</param>
		/// <param name="type">One of the <see cref="EventLogEntryType">EventLogEntryType</see> values.</param>
		/// <param name="eventID">The application-specific identifier for the event.</param>
		/// <param name="category">The application-specific subcategory associated with the message. </param>
		/// <param name="rawData">An array of bytes that holds the binary data associated with the entry. </param>
		public void WriteEntry(string message, EventLogEntryType type, int eventID, short category, byte[] rawData)
		{
			this.CheckForValidLog();
			this.eventLogWriter.WriteEntry(message,type, eventID, category, rawData);
		}
		/// <summary>
		/// Writes an entry with the given message text, application-defined event identifier, and application-defined category to the event log, using the specified registered event source. The category can be used to filter events in the log.
		/// </summary>
		/// <param name="source">The source by which the application is registered on the specified computer. </param>
		/// <param name="message">the string to write to the event log.</param>
		/// <param name="type">One of the <see cref="EventLogEntryType">EventLogEntryType</see> values.</param>
		/// <param name="eventID">The application-specific identifier for the event.</param>
		/// <param name="category">The application-specific subcategory associated with the message. </param>
		public void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category)
		{
			this.CheckForValidLog();
			if(source.Length>0)
				this.eventLogWriter.WriteEntry(source, message,type, eventID, category);
			else
				this.eventLogWriter.WriteEntry(this.eventLogWriter.Source, message,type, eventID, category);
		}
		/// <summary>
		/// Writes an entry with the given message text, application-defined event identifier, and application-defined category to the event log (using the specified registered event source) and appends binary data to the message.
		/// </summary>
		/// <param name="source">The source by which the application is registered on the specified computer. </param>
		/// <param name="message">the string to write to the event log.</param>
		/// <param name="type">One of the <see cref="EventLogEntryType">EventLogEntryType</see> values.</param>
		/// <param name="eventID">The application-specific identifier for the event.</param>
		/// <param name="category">The application-specific subcategory associated with the message. </param>
		/// <param name="rawData">An array of bytes that holds the binary data associated with the entry. </param>
		public void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category, byte[] rawData)
		{
			this.CheckForValidLog();
			if(source.Length>0)
				this.eventLogWriter.WriteEntry(source,message,type,eventID, category, rawData);
			else
				this.eventLogWriter.WriteEntry(this.eventLogWriter.Source,message,type,eventID, category, rawData);
		}
		

		#endregion	

		#region Private Methods

		/// <summary>
		/// Checks to see if the source is set
		/// </summary>
		/// <returns></returns>
		private void CheckForValidLog()
		{
			//Check to see if the log was closed
			if(this.eventLogWriter.Log.Length==0)
				throw new Exception("Unable to write to the log because it has been closed.");

			//Check to see if the source is set.
			if(this.eventLogWriter.Source.Length==0)
				throw new Exception("Source must be set before writting to the log.");
		}
		/// <summary>
		/// Creates the event log Writer object
		/// </summary>
		private void CreateEventLogWriter(string log, string source, string logPath, string logFileName)
		{
			if(this.eventLogWriterType!=EventLogWriterType.Custom)
			{
				//Create the log Writer
				if(this.eventLogWriterType == EventLogWriterType.XML)
					this.eventLogWriter = new XMLEventLogWriter(log, source, logPath, logFileName);
				else
					throw new NotSupportedException("Only XMLEventLogWriter is currently supported.");
			}
			//Begin listening for the event
			this.eventLogWriter.EntryWritten+=new EntryWrittenEventHandler(eventLogWriter_EntryWritten);
			this.eventLogWriter.EventLogCollectionUpdated +=new EventHandler(eventLogWriter_EventLogCollectionUpdated);
		}

		/// <summary>
		/// Notifies any listeners that an entry was written to the log
		/// </summary>
		/// <param name="e">the entry item that was written to the log</param>
		private void OnEntryWritten(EventLogEntry e)
		{
			if(this.enableRaisingEvents)
			{
				if(EntryWritten!=null)
					EntryWritten(this,e);
			}
		}

		/// <summary>
		/// Notifies any listeners that the log was cleared
		/// </summary>
		private void OnLogCleared()
		{
			if(this.enableRaisingEvents)
			{
				if(LogCleared!=null)
					LogCleared(this,EventArgs.Empty);
			}
		}

		/// <summary>
		/// Notifies any listeners that log display name was changed
		/// </summary>
		private void OnLogDisplayNameChanged()
		{
			if(this.enableRaisingEvents)
			{
				if(LogDisplayNameChanged!=null)
					LogDisplayNameChanged(this,EventArgs.Empty);
			}
		}

		/// <summary>
		/// Notifies any listeners that log was closed
		/// </summary>
		private void OnLogClosed()
		{
			if(this.enableRaisingEvents)
			{
				if(LogClosed!=null)
					LogClosed(this,EventArgs.Empty);
			}
		}

		/// <summary>
		/// Notifies any listeners that log was changed
		/// </summary>
		private void OnLogChanged()
		{
			if(this.enableRaisingEvents)
			{
				if(LogChanged!=null)
					LogChanged(this,EventArgs.Empty);
			}
		}

		/// <summary>
		/// Notifies any listeners that source was changed
		/// </summary>
		private void OnSourceChanged()
		{
			if(this.enableRaisingEvents)
			{
				if(SourceChanged!=null)
					SourceChanged(this,EventArgs.Empty);
			}
		}

		/// <summary>
		/// Notifies any listeners that a log was added to the event log
		/// </summary>
		private void OnEventLogAdded()
		{
			if(this.enableRaisingEvents)
			{
				if(EventLogAdded!=null)
					EventLogAdded(this,EventArgs.Empty);
			}
		}

		/// <summary>
		/// Handler for the entry written event in the eventLogWriter
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void eventLogWriter_EntryWritten(object sender, EventLogEntry e)
		{
			this.OnEntryWritten(e);
		}
		/// <summary>
		/// event listeners when the eventlog collection is updated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void eventLogWriter_EventLogCollectionUpdated(object sender, EventArgs e)
		{
			this.OnEventLogAdded();
		}
		#endregion

		
	}
}
