using System;
using OpenNETCF.Win32;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Represents a date selection range in a month calendar control.
	/// </summary>
	public class SelectionRange
	{
		byte[] m_data;

		/// <summary>
		/// Initializes a new instance of the <see cref="SelectionRange"/> class.
		/// </summary>
		public SelectionRange()
		{
			m_data = new byte[32];
			//start at minimum
			this.Start = new DateTime(1753, 1, 1);
			//end at max
			this.End = DateTime.MaxValue;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="SelectionRange"/> class with the specified beginning and ending dates.
		/// </summary>
		/// <param name="lower">The starting date in the <see cref="SelectionRange"/>.</param>
		/// <param name="upper">The ending date in the <see cref="SelectionRange"/>.</param>
		public SelectionRange(DateTime lower, DateTime upper) : this()
		{
			this.Start = lower;
			this.End = upper;
		}

		internal byte[] ToByteArray()
		{
			return m_data;
		}

		/// <summary>
		/// Gets or sets the starting date and time of the selection range.
		/// </summary>
		public DateTime Start
		{
			get
			{
				SystemTime st = new SystemTime(m_data, 0);
				return st.ToDateTime();
			}
			set
			{
				SystemTime st = new SystemTime(value);
				st.ToByteArray().CopyTo(m_data, 0);
			}
		}

		/// <summary>
		/// Gets or sets the ending date and time of the selection range.
		/// </summary>
		public DateTime End
		{
			get
			{
				SystemTime st = new SystemTime(m_data, 16);
				return st.ToDateTime();
			}
			set
			{
				SystemTime st = new SystemTime(value);
				st.ToByteArray().CopyTo(m_data, 16);
			}
		}
	}
}
