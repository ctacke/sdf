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

namespace OpenNETCF.Phone.Sim
{
	/// <summary>
	/// Represents a collection of <see cref="Message"/> items on the SIM card.
	/// </summary>
	public class MessageStorage : IList
	{
		private Sim m_sim;
		private MessageStorageType m_storage;

		internal MessageStorage(Sim parent, MessageStorageType storage)
		{
			m_sim = parent;
			m_storage = storage;
		}

		/// <summary>
		/// Returns the maximum number of Messages which can be stored in the collection.
		/// </summary>
		public int Capacity
		{
			get
			{
				int total = 0;
				int used = 0;

				int hresult = SimGetSmsStorageStatus(m_sim.Handle, m_storage, ref used, ref total);

				if(hresult != 0)
				{
					throw new ExternalException("Failure retrieving SIM message storage status");
				}

				return total;
			}
		}

		/// <summary>
		/// Returns the <see cref="Message"/> at the specified index.
		/// </summary>
		public Message this[int index]
		{
			get
			{
				Message result = new Message();
				int hresult = SimReadMessage(m_sim.Handle, m_storage, index, result.ToByteArray());

				//TODO: add more detailed error checking
				if(hresult != 0)
				{
					throw new ExternalException("Error retrieving message entry");
				}

				return result;
			}
		}

		#region IList Members

		bool IList.IsReadOnly
		{
			get
			{
				return false;
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
			}
		}

		public void RemoveAt(int index)
		{
			// TODO:  Add SimMessageCollection.RemoveAt implementation
		}

		void IList.Insert(int index, object value)
		{
			// TODO:  Add SimMessageCollection.Insert implementation
		}

		void IList.Remove(object value)
		{
			// TODO:  Add SimMessageCollection.Remove implementation
		}

		bool IList.Contains(object value)
		{
			// TODO:  Add SimMessageCollection.Contains implementation
			return false;
		}

		void IList.Clear()
		{
			// TODO:  Add SimMessageCollection.Clear implementation
		}

		int IList.IndexOf(object value)
		{
			// TODO:  Add SimMessageCollection.IndexOf implementation
			return 0;
		}

		int IList.Add(object value)
		{
			// TODO:  Add SimMessageCollection.Add implementation
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
				// TODO:  Add SimMessageCollection.IsSynchronized getter implementation
				return false;
			}
		}

		/// <summary>
		/// Returns the number of Messages contained in the collection.
		/// </summary>
		public int Count
		{
			get
			{
				int total = 0;
				int used = 0;

				int hresult = SimGetSmsStorageStatus(m_sim.Handle, m_storage, ref used, ref total);

				
				if(hresult != 0)
				{
					throw new ExternalException("Failure retrieving SIM message storage status");
				}

				return used;
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			// TODO:  Add SimMessageCollection.CopyTo implementation
		}

		object ICollection.SyncRoot
		{
			get
			{
				// TODO:  Add SimMessageCollection.SyncRoot getter implementation
				return null;
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return new MessageEnumerator(this);
		}

		private class MessageEnumerator : IEnumerator
		{
			private MessageStorage m_parent;
			private int m_index;
			private int m_count;

			internal MessageEnumerator(MessageStorage parent)
			{
				m_count = 0;
				m_parent = parent;
				m_index = -1;
				
			}
			#region IEnumerator Members

			public void Reset()
			{
				//return to top of list
				m_index = -1;
				m_count = 0;
			}

			public object Current
			{
				get
				{
					Message m = m_parent[m_index];
					while(m==null)
					{	
						if(MoveNext())
						{
							m = m_parent[m_index];
						}
						else
						{
							break;
						}
					}
					//increment valid records
					if(m!=null)
					{
						m_count++;

						return m;
					}
					else
					{
						return new Message();
					}

				}
			}

			public bool MoveNext()
			{
				//increment index
				m_index++;

				//if there is a record at that position
				if(m_count < m_parent.Count)
				{
					return true;
				}
				
				//no more records
				return false;
			}

			#endregion
		}
		#endregion

		#region API Members

		[DllImport("cellcore.dll", EntryPoint="SimReadMessage", SetLastError=true)]
		private static extern int SimReadMessage(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			MessageStorageType dwStorage,                        // @parm A SIM_SMSSTORAGE_* constant
			int dwIndex,                          // @parm Index of the entry to retrieve
			byte[] lpSimMessage               // @parm Points to an SMS message structure
			);

		[DllImport("cellcore.dll", EntryPoint="SimWriteMessage", SetLastError=true)]
		private static extern int SimWriteMessage(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			MessageStorageType dwStorage,                        // @parm A SIM_SMSSTORAGE_* constant
			ref int lpdwIndex,                      // @parm Set to the index where the message was written
			byte[] lpSimMessage               // @parm Points to an SMS message structure
			);

		[DllImport("cellcore.dll", EntryPoint="SimDeleteMessage", SetLastError=true)]
		private static extern int SimDeleteMessage(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			MessageStorageType dwStorage,                        // @parm A SIM_SMSSTORAGE_* constant
			int dwIndex                           // @parm Index of the entry to retrieve
			);

		[DllImport("cellcore.dll", EntryPoint="SimGetSmsStorageStatus", SetLastError=true)]
		private static extern int SimGetSmsStorageStatus(
			IntPtr hSim,
			MessageStorageType dwStorage,
			ref int lpdwUsed,
			ref int lpdwTotal);


		#endregion
	}

	/// <summary>
	/// Specifies the types of <see cref="MessageStorage"/> available.
	/// </summary>
	[Flags()]
	public enum MessageStorageType : int
	{
		/// <summary>
		/// Broadcast message storage location.
		/// </summary>
		Broadcast        = 0x00000001,
		/// <summary>
		/// SIM storage location.
		/// </summary>
		Sim              = 0x00000002,
	}
}
