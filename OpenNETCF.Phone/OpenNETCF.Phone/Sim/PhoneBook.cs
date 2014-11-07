using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace OpenNETCF.Phone.Sim
{
	/// <summary>
	/// Supports working with the phonebook entries on a Sim.
	/// </summary>
	public class Phonebook : IList
	{
		private Sim m_sim;
		private PhonebookStorage m_storage;

		private const uint SIM_PBINDEX_FIRSTAVAILABLE = 0xffffffff;

		/// <summary>
		/// Create a new instance of Phonebook
		/// </summary>
		/// <param name="sim">Parent Sim</param>
		/// <param name="storage">Storage identifier</param>
		internal Phonebook(Sim sim, PhonebookStorage storage)
		{
			m_sim = sim;
			m_storage = storage;
		}

		/// <summary>
		/// Gets the <see cref="Sim"/> object to which this Phonebook belongs.
		/// </summary>
		public Sim Sim
		{
			get
			{
				return m_sim;
			}
		}

		/// <summary>
		/// Returns the <see cref="PhonebookEntry"/> at the specified index.
		/// </summary>
		public PhonebookEntry this[int index]
		{
			get
			{
				PhonebookEntry result = new PhonebookEntry();
				uint hresult = SimReadPhonebookEntry(m_sim.Handle, m_storage, (uint)index + 1, result.ToByteArray());

				if(hresult != 0)
				{
					//empty slot
					if(hresult==0x88000345)
					{
						return null;
					}
					else
					{
						throw new ExternalException("Error retrieving phonebook entry");
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Gets the maximum number of slots in the Phonebook storage.
		/// </summary>
		public int Capacity
		{
			get
			{
				int used = 0;
				int total = 0;

				uint hresult = SimGetPhonebookStatus(m_sim.Handle, m_storage, ref used, ref total);

				if(hresult != 0)
				{
					throw new ExternalException("Error getting phonebook status");
				}

				return total;
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
				//do nothing
			}
		}

		/// <summary>
		/// Removes the PhonebookEntry at the specified zero-based index.
		/// </summary>
		/// <param name="index">zero-based index of the entry to delete.</param>
		public void RemoveAt(int index)
		{
			//converted to 1 based indexing
			uint hresult = SimDeletePhonebookEntry(m_sim.Handle, m_storage, (uint)index + 1);

			if(hresult != 0)
			{
				throw new ExternalException("Error deleting phonebook entry");
			}
		}

		void IList.Insert(int index, object value)
		{
			throw new NotSupportedException("Phonebook does not support IList.Insert");
		}

		void IList.Remove(object value)
		{
			throw new NotSupportedException("Phonebook does not support IList.Remove");
		}

		bool IList.Contains(object value)
		{
			// TODO:  Add SimPhonebook.Contains implementation
			return false;
		}

		void IList.Clear()
		{
			throw new NotSupportedException("Phonebook does not support IList.Clear");
		}

		int IList.IndexOf(object value)
		{
			// TODO:  Add SimPhonebook.IndexOf implementation
			return 0;
		}

		int IList.Add(object value)
		{
			if(value.GetType()== typeof(PhonebookEntry))
			{
				PhonebookEntry pe = (PhonebookEntry)value;

				uint result = SimWritePhonebookEntry(m_sim.Handle, m_storage, SIM_PBINDEX_FIRSTAVAILABLE, pe.ToByteArray());
			}
			else
			{
				throw new ArgumentException("value must be of type PhonebookEntry");
			}

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

		/// <summary>
		/// Returns the total number of <see cref="PhonebookEntry"/> items contain in this <see cref="Phonebook"/>.
		/// </summary>
		public int Count
		{
			get
			{
				int used = 0;
				int total = 0;

				uint hresult = SimGetPhonebookStatus(m_sim.Handle, m_storage, ref used, ref total);

				if(hresult != 0)
				{
					throw new ExternalException("Error getting phonebook status");
				}

				return used;
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			// TODO:  Add SimPhonebook.CopyTo implementation
		}

		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return new PhonebookEnumerator(this);
		}

		private class PhonebookEnumerator : IEnumerator
		{
			private Phonebook m_parent;
			private int m_index;
			private int m_count;

			internal PhonebookEnumerator(Phonebook parent)
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
					PhonebookEntry pbe = m_parent[m_index];
					while(pbe==null)
					{	
						if(MoveNext())
						{
							pbe = m_parent[m_index];
						}
						else
						{
							break;
						}
					}
					//increment valid records
					if(pbe!=null)
					{
						m_count++;

						return pbe;
					}
					else
					{
						return new PhonebookEntry();
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

		[DllImport("cellcore.dll")]
		private static extern uint SimReadPhonebookEntry(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			PhonebookStorage dwLocation,                       // @parm A SIMPBSTORAGE_* Constant
			uint dwIndex,                          // @parm Index of the entry to retrieve
			byte[] lpPhonebookEntry    // @parm Points to a phonebook entry structure
			);

		[DllImport("cellcore.dll")]
		private static extern uint SimGetPhonebookStatus(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			PhonebookStorage dwLocation,                       // @parm A SIMPBSTORAGE_* Constant
			ref int lpdwUsed,                       // @parm Nubmer of used locations
			ref int lpdwTotal                       // @parm Total number of locations
			);

		[DllImport("cellcore.dll")]
		private static extern uint SimWritePhonebookEntry(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			PhonebookStorage dwLocation,                       // @parm A SIMPBSTORAGE_* Constant
			uint dwIndex,                          // @parm Index of the entry to retrieve (may be SIM_PBINDEX_FIRSTAVAILABLE)
			byte[] lpPhonebookEntry    // @parm Points to a phonebook entry structure
			);

		[DllImport("cellcore.dll")]
		private static extern uint SimDeletePhonebookEntry(
			IntPtr hSim,                              // @parm Points to a valid HSIM handle
			PhonebookStorage dwLocation,                       // @parm A SIMPBSTORAGE_* Constant
			uint dwIndex                           // @parm Index of the entry to retrieve
			);

		#endregion
	}
}
