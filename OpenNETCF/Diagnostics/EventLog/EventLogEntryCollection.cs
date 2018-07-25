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
