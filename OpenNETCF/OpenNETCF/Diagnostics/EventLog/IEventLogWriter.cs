using System;

namespace OpenNETCF.Diagnostics
{
	/// <summary>
	/// Defines the interface that will be used by EventLog to write to a log
	/// </summary>
	public interface IEventLogWriter
	{
		/// <summary>
		/// 
		/// </summary>
		event EntryWrittenEventHandler EntryWritten;
		/// <summary>
		/// 
		/// </summary>
		event EventHandler EventLogCollectionUpdated;

		/// <summary>
		/// 
		/// </summary>
		string Source{get;set;}
		/// <summary>
		/// 
		/// </summary>
		string Log{get;set;}
		/// <summary>
		/// 
		/// </summary>
		string LogDisplayName{get;set;}
		/// <summary>
		/// 
		/// </summary>
		string LogFileName{get;}
		/// <summary>
		/// 
		/// </summary>
		string LogPath{get;}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="logName"></param>
		void Delete(string logName);
		/// <summary>
		/// 
		/// </summary>
		void Clear();
		/// <summary>
		/// 
		/// </summary>
		void Close();
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		EventLog[] GetEventLogs();
		/// <summary>
		/// 
		/// </summary>
		EventLogEntryCollection Entries{get;}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logName"></param>
		/// <returns></returns>
		bool Exists(string logName);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		void WriteEntry(string message);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="type"></param>
		void WriteEntry(string message, EventLogEntryType type);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="message"></param>
		void WriteEntry(string source, string message);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="type"></param>
		/// <param name="eventID"></param>
		void WriteEntry(string message, EventLogEntryType type, int eventID);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="message"></param>
		/// <param name="type"></param>
		void WriteEntry(string source, string message, EventLogEntryType type);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="type"></param>
		/// <param name="eventID"></param>
		/// <param name="category"></param>
		void WriteEntry(string message, EventLogEntryType type, int eventID, short category);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="message"></param>
		/// <param name="type"></param>
		/// <param name="eventID"></param>
		void WriteEntry(string source, string message, EventLogEntryType type, int eventID);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="type"></param>
		/// <param name="eventID"></param>
		/// <param name="category"></param>
		/// <param name="rawData"></param>
		void WriteEntry(string message, EventLogEntryType type, int eventID, short category, byte[] rawData);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="message"></param>
		/// <param name="type"></param>
		/// <param name="eventID"></param>
		/// <param name="category"></param>
		void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="message"></param>
		/// <param name="type"></param>
		/// <param name="eventID"></param>
		/// <param name="category"></param>
		/// <param name="rawData"></param>
		void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category, byte[] rawData);
		
	}
}
