#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



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
