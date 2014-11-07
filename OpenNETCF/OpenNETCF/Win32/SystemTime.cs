using System;
using System.Runtime.InteropServices;


namespace OpenNETCF.Win32
{
	/// <summary>   
	/// This structure represents a date and time using individual members for the month, day, year, weekday, hour, minute, second, and millisecond.   
	/// </summary>
	/// <remarks>Wraps the native <b>SYSTEMTIME</b> structure.</remarks>
	public class SystemTime
	{
		protected byte[] flatStruct = new byte[ 16 ];

		#region Flat structure offset constants
		/// <summary>
		/// Offset within structure to Year value.
		/// </summary>
		protected const	short wYearOffset = 0;
		/// <summary>
		/// Offset within structure to Month value.
		/// </summary>
		protected const short wMonthOffset = 2; 
		/// <summary>
		/// Offset within structure to DayOfWeek value.
		/// </summary>
		protected const short wDayOfWeekOffset = 4; 
		/// <summary>
		/// Offset within structure to Day value.
		/// </summary>
		protected const short wDayOffset = 6; 
		/// <summary>
		/// Offset within structure to Hour value.
		/// </summary>
		protected const short wHourOffset = 8; 
		/// <summary>
		/// Offset within structure to Minute value.
		/// </summary>
		protected const short wMinuteOffset = 10; 
		/// <summary>
		/// Offset within structure to Second value.
		/// </summary>
		protected const short wSecondOffset = 12; 
		/// <summary>
		/// Offset within structure to Millisecond value.
		/// </summary>
		protected const short wMillisecondsOffset = 14; 
		#endregion

		/// <summary>
		/// Represents an empty SystemTime structure.
		/// </summary>
		public static readonly SystemTime Empty = new SystemTime(0, 0, 0, 0, 0, 0);

		#region ctor(byte[])
		/// <summary>
		/// Construct a SystemTime from a byte array
		/// </summary>   
		/// <remarks>This is used when setting a time zone,   
		/// which contains two embedded SystemTime structures.</remarks>   
		/// <param name="bytes">Byte Array containing SystemTime data.</param> 
		public SystemTime( byte[] bytes ) : this( bytes, 0 )
		{
		}
		#endregion

		#region ctor(byte[], int)
		/// <summary>
		/// Construct a SystemTime from a portion of a byte array.
		/// </summary>   
		/// <remarks>This is used when setting a time zone,   
		/// which contains two embedded SystemTime structures.</remarks>   
		/// <param name="bytes">Byte Array containing SystemTime data.</param>   
		/// <param name="offset">Offset (in bytes) to SystemTime data.</param> 
		public SystemTime( byte[] bytes, int offset )
		{
			// Dump the byte array into our array.
			Buffer.BlockCopy( bytes, offset, flatStruct, 0, flatStruct.Length );
		}
		#endregion

		#region ctor(ushort, ushort, ushort, ushort, ushort, ushort)   
		/// <summary>
		/// Initializes a new SystemTime object with the specified parameters.                          /// Initializes a new SYSTEMTIME object with the specified parameters. 
		/// <param name="year">Specifies the current year.</param>
		/// <param name="month">Specifies the current month; January = 1, February = 2, and so on</param>
		/// <param name="day">Specifies the current day of the month.</param>
		/// <param name="hour">Specifies the current hour.</param>
		/// <param name="minute">Specifies the current minute.</param>
		/// <param name="second">Specifies the current second.</param>  
		/// </summary>
		public SystemTime(short year, short month, short day, short hour, short minute, short second)
		{
			Year = year;
			Month = month;
			DayOfWeek = 0;
			Day = day;
			Hour = hour;
			Minute = minute;
			Second = second;
			Milliseconds = 0;
		}
		#endregion

		#region ToByteArray
		/// <summary>
		/// Method to extract marshal-compatible 'structure' from the class.   
		/// </summary>   
		/// <returns>Byte Array containing the SystemTime data.</returns>   
		public byte[] ToByteArray()
		{
			return flatStruct;
		}
		#endregion

		/// <summary>
		/// Create a new SystemTime instance from an existing DateTime instance.
		/// </summary>
		/// <param name="dt">DateTime to create SystemTime from.</param>
		public SystemTime(System.DateTime dt) : this((short)dt.Year, (short)dt.Month, (short)dt.Day, (short)dt.Hour, (short)dt.Minute, (short)dt.Second)
		{
		}

		/// <summary>
		/// Create a new empty SystemTime instance.
		/// </summary>
		public SystemTime() : this(new byte[16])
		{
		}

		/// <summary>
		/// Converts a SystemTime structure to a DateTime object.
		/// </summary>
		/// <param name="st">A SystemTime structure.</param>
		/// <returns>Equivalent date in the form of a <see cref="DateTime"/></returns>
		public static implicit operator System.DateTime(SystemTime st)
		{
			return st.ToDateTime();
		}

		/// <summary>
		/// Converts a 64bit FileTime value to a SystemTime structure.
		/// </summary>
		/// <param name="FileTime">FileTime.</param>
		/// <returns>A SystemTime structure.</returns>
		public static implicit operator SystemTime(long FileTime)
		{
			byte[] bytes = new byte[16];

			NativeMethods.FileTimeToSystemTime(ref FileTime, bytes);

			SystemTime st = new SystemTime(bytes);
			return st;
		}

		/// <summary>
		/// Converts a SystemTime structure to the equivalent FileTime 64bit integer.
		/// </summary>
		/// <param name="st"></param>
		/// <returns></returns>
		public static implicit operator long(SystemTime st)
		{
			byte[] bytes = new byte[16];
			bytes = st.ToByteArray();
			long ft = new long();

            NativeMethods.SystemTimeToFileTime(bytes, ref ft);

			return ft;
		}

		#region From FileTime
		/// <summary>
		/// Returns a SystemTime equivalent to the specified operating system file timestamp.
		/// </summary>
		/// <param name="fileTime">A Windows file time.</param>
		/// <returns>A SystemTime value representing the date and time of fileTime.</returns>
		public static SystemTime FromFileTime(long fileTime)
		{
			SystemTime st = new SystemTime();

            NativeMethods.FileTimeToSystemTime(ref fileTime, st.flatStruct);

			return st;
		}
		#endregion

		#region ToFileTime
		/// <summary>
		/// Converts the value of this instance to the format of a local operating system file time.
		/// </summary>
		/// <returns>The value of this SystemTime in the format of a local operating system file time.</returns>
		public long ToFileTime()
		{
			long ft = new long();

            NativeMethods.SystemTimeToFileTime(this.flatStruct, ref ft);

			return ft;
		}
		#endregion

		/// <summary>
		/// Converts a SystemTime structure to the equivalent binary data.
		/// </summary>
		/// <param name="st"></param>
		/// <returns></returns>
		public static implicit operator byte[]( SystemTime st )
		{
			return st.ToByteArray();
		}

		#region From DateTime
		/// <summary>   
		/// Creates a new instance of SystemTime from an existing System.DateTime object   
		/// </summary>   
		/// <param name="dt">DateTime object to copy.</param>   
		/// <returns>SystemTime class matching the DateTime object.</returns>   
		public static SystemTime FromDateTime(System.DateTime dt)   
		{   
			return new SystemTime(Convert.ToInt16(dt.Year), Convert.ToInt16(dt.Month), Convert.ToInt16(dt.Day), Convert.ToInt16(dt.Hour), Convert.ToInt16(dt.Minute), Convert.ToInt16(dt.Second));   
		}   
		#endregion   
    
		#region To DateTime
		/// <summary>   
		/// Returns a <see cref="T:System.DateTime"/> object with the same Date and time as this instance.   
		/// </summary>   
		/// <returns>A <see cref="T:System.DateTime"/> copy of the SystemTime object.</returns>   
		public System.DateTime ToDateTime()   
		{   
			try
			{
				return new System.DateTime(Year, Month, Day, Hour, Minute, Second, Milliseconds);   
			}
			catch
			{
				//catch invalid date
				return DateTime.MinValue;
			}
		}   
		#endregion 

		#region Year
		/// <summary>   
		/// Gets the year component of the date represented by this instance.   
		/// </summary> 
		public short Year
		{
			get
			{
				return BitConverter.ToInt16( flatStruct, wYearOffset );
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, wYearOffset, 2 );
			}
		}
		#endregion

		#region Month
		/// <summary>   
		/// Gets the month component of the date represented by this instance.   
		/// </summary> 
		public short Month
		{
			get
			{
				return BitConverter.ToInt16( flatStruct, wMonthOffset );
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, wMonthOffset, 2 );
			}
		}
		#endregion   
    
		#region DayOfWeek
		/// <summary>   
		/// The Day of the week. Sunday = 0, Monday = 1, and so on.   
		/// </summary>   
		/// <remarks>Because the numbering scheme matches the System.DayOfWeek enumeration,   
		/// it is possible to cast this field to DayOfWeek.</remarks> 
		public short DayOfWeek
		{
			get
			{
				return BitConverter.ToInt16( flatStruct, wDayOfWeekOffset );
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, wDayOfWeekOffset, 2 );
			}
		}
		#endregion   
    
		#region Day
		/// <summary>   
		/// Gets the day of the month represented by this instance.   
		/// </summary> 
		public short Day
		{
			get
			{
				return BitConverter.ToInt16( flatStruct, wDayOffset );
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, wDayOffset, 2 );
			}
		}
		#endregion   
    
		#region Hour
		/// <summary>   
		/// Gets the hour component of the date represented by this instance.   
		/// </summary> 
		public short Hour
		{
			get
			{
				return BitConverter.ToInt16( flatStruct, wHourOffset );
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, wHourOffset, 2 );
			}
		}
		#endregion   
    
		#region Minute
		/// <summary>   
		/// Gets the minute component of the date represented by this instance.   
		/// </summary> 
		public short Minute
		{
			get
			{
				return BitConverter.ToInt16( flatStruct, wMinuteOffset );
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, wMinuteOffset, 2 );
			}
		}
		#endregion   
    
		#region Second
		/// <summary>   
		/// Gets the seconds component of the date represented by this instance.   
		/// </summary> 
		public short Second
		{
			get
			{
				return BitConverter.ToInt16( flatStruct, wSecondOffset );
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, wSecondOffset, 2 );
			}
		}
		#endregion   
    
		#region Milliseconds
		/// <summary>   
		/// Gets the milliseconds component of the date represented by this instance.   
		/// </summary> 
		public short Milliseconds
		{
			get
			{
				return BitConverter.ToInt16( flatStruct, wMillisecondsOffset );
			}
			set
			{
				byte[]	bytes = BitConverter.GetBytes( value );
				Buffer.BlockCopy( bytes, 0, flatStruct, wMillisecondsOffset, 2 );
			}
		}
		#endregion
	}

}
