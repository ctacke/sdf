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
using System.Collections;

namespace OpenNETCF.Diagnostics
{
	/// <summary>
	/// Summary description for EventLogEntryCollection.
	/// </summary>
	public class EventLogEntryCollection:CollectionBase
	{
		internal EventLogEntryCollection()
		{	
		}


		/// <summary>
		/// Overloaded method.  To clear the event log use EventLog.Clear();
		/// </summary>
		/// <exception cref="System.NotSupportedException" />
		public new void Clear()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Overloaded method.
		/// </summary>
		/// <param name="index"></param>
		/// <exception cref="System.NotSupportedException" />
		private new void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Gets the EventLogEntry by index
		/// </summary>
		public EventLogEntry this[int index]
		{
			get
			{
				return (EventLogEntry)this.List[index];
			}
			set
			{
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public EventLogEntry this[string ID]
		{
			get
			{
				for(int x=0;x<this.List.Count;x++)
				{
					if(((EventLogEntry)this.List[x]).ID == ID)
						return (EventLogEntry)this.List[x];
				}
				return null;
			}
			set
			{
			}
		}

		/// <summary>
		/// Add an eventlog entry to the collection
		/// </summary>
		/// <param name="eventLogEntry"></param>
		/// <returns></returns>
		public int Add(EventLogEntry eventLogEntry)
		{
			if(this.List.Contains(eventLogEntry))
				return this.List.IndexOf(eventLogEntry);
			else
				return this.List.Add(eventLogEntry);
		}
	}

}
