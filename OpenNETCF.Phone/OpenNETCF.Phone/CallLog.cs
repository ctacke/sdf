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
using System.Runtime.InteropServices;

namespace OpenNETCF.Phone
{
	/// <summary>
	/// Provides access to the Call Log on the device which records incoming and outgoing calls made.
	/// </summary>
	public class CallLog : IList, IDisposable
	{
		//call log handle
		private int m_handle;

		//item count
		private int m_count;

		#region Constructor
		/// <summary>
		/// Initialize a new instance of <see cref="CallLog"/>.
		/// </summary>
		public CallLog()
		{
			//instantiate new handle
			m_handle = 0;
			int hresult = PhoneOpenCallLog(ref m_handle);

			if(hresult != 0)
			{
				throw new ExternalException("Error opening Call Log");
			}

			//get count
			m_count = Seek(CallLogSeek.End, 0) +1;

			//return
			Seek(CallLogSeek.Beginning, 0);
		}
		#endregion

		#region Finalize
		~CallLog()
		{
			//close log if open
			this.Close();
		}
		#endregion

		#region GetEntry
		/// <summary>
		/// Retrieves the current <see cref="CallLogEntry"/> from the call log.
		/// </summary>
		/// <returns></returns>
		public CallLogEntry GetEntry()
		{
			if(m_handle != 0)
			{
				byte[] buffer = new byte[48];
				//add sizeof to buffer
				BitConverter.GetBytes(buffer.Length).CopyTo(buffer, 0);

				//fill buffer
				int hresult = PhoneGetCallLogEntry(m_handle, buffer);

				if(hresult != 0)
				{
					return null;
				}
				return new CallLogEntry(buffer);
			}
			else
			{
				throw new ObjectDisposedException("CallLog closed");
			}
		}
		#endregion

		#region Seek
		/// <summary>
		/// Initiates a search that ends at a given entry in a call log.
		/// </summary>
		/// <param name="seek">Location within the call log where the search will begin.</param>
		/// <param name="iRecord">The zero-based index value of an entry in the call log, starting at the beginning of the log if seek is CallLogSeek.Beginning and at the end if seek = CallLogSeek.End. </param>
		/// <returns>zero-based index value from the beginning of the seek pointer after the search is completed.</returns>
		public int Seek(CallLogSeek seek, int iRecord)
		{
			if(m_handle != 0)
			{
				int precord = 0;

				int hresult = PhoneSeekCallLog(m_handle, seek, iRecord, ref precord);
			
				//if(hresult != 0)
				//{
				//	throw new ExternalException("Error seeking Call Log");
				//}

				return precord;
			}
			else
			{
				throw new ObjectDisposedException("Call Log closed");
			}
		}
		#endregion

		#region Close
		/// <summary>
		/// Closes the Call Log
		/// </summary>
		public void Close()
		{
			if(m_handle!=0)
			{
				//close call log
				int hresult = PhoneCloseCallLog(m_handle);

				if(hresult != 0)
				{
					throw new ExternalException("Error closing Call Log");
				}
				else
				{
					//clear handle
					m_handle = 0;
				}
			}
		}
		#endregion

		#region Call Log API Methods

		[DllImport("phone.dll", SetLastError=true)]
		private static extern int PhoneOpenCallLog(
			ref int handle);

		[DllImport("phone.dll", SetLastError=true)]
		private static extern int PhoneCloseCallLog(
			int handle);

		[DllImport("phone.dll", SetLastError=true)]
		private static extern int PhoneGetCallLogEntry(
			int handle,
			byte[] pentry );

		[DllImport("phone.dll", SetLastError=true)]
		private static extern int PhoneSeekCallLog(
			int handle,
			CallLogSeek seek,
			int iRecord,
			ref int piRecord );

		#endregion


		#region IDisposable Members

		public void Dispose()
		{
			//close (if open)
			this.Close();
		}

		#endregion

		#region IList Members

		bool IList.IsReadOnly
		{
			get
			{
				// CallLog is ALWAYS Read-Only
				return true;
			}
		}

		#region this
		/// <summary>
		/// Returns the <see cref="T:OpenNETCF.Phone.CallLogEntry"/> at the specified zero-based index
		/// </summary>
		public CallLogEntry this[int index]
		{
			get
			{
				if(Seek(CallLogSeek.Beginning, index)==index)
				{
					return this.GetEntry();
				}
				else
				{
					return null;
				}
			}
		}
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				//do nothing
			}
		}
		#endregion

		void IList.RemoveAt(int index)
		{
			// do nothing
		}

		void IList.Insert(int index, object value)
		{
			// do nothing
		}

		void IList.Remove(object value)
		{
			// do nothing
		}

		bool IList.Contains(object value)
		{
			// TODO:  Add CallLog.Contains implementation
			return false;
		}

		void IList.Clear()
		{
			// do nothing
		}

		int IList.IndexOf(object value)
		{
			// TODO:  Add CallLog.IndexOf implementation
			return 0;
		}

		int IList.Add(object value)
		{
			// do nothing
			return 0;
		}

		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region ICollection Members

		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public int Count
		{
			get
			{
				return m_count;
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			// TODO:  Add CallLog.CopyTo implementation
		}

		object ICollection.SyncRoot
		{
			get
			{
				// TODO:  Add CallLog.SyncRoot getter implementation
				return null;
			}
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			// return an enumerator
			return new CallLogEnumerator(this);
		}

		private class CallLogEnumerator : IEnumerator
		{
			private CallLog m_parent;
			private int m_index;
			private CallLogEntry m_entry;

			internal CallLogEnumerator(CallLog parent)
			{
				m_index = -1;
				m_parent = parent;
			}
			#region IEnumerator Members

			public void Reset()
			{
				//return to top of list
				m_parent.Seek(CallLogSeek.Beginning, 0);
				m_index = -1;
			}

			public object Current
			{
				get
				{
					return m_entry;
				}
			}

			public bool MoveNext()
			{
				//increment index
				m_index++;

				m_entry = m_parent.GetEntry();

				//if there is a record at that position
				if(m_entry!=null)
				{
					return true;
				}
				else
				{
					//no more records
					return false;
				}
			}

			#endregion
		}

		#endregion
	}
}
