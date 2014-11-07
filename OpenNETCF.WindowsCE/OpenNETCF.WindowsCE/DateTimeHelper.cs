using System;
using System.Runtime.InteropServices;
using OpenNETCF.Win32;

namespace OpenNETCF.WindowsCE
{
    #region DateTimeHelper Class
    /// <summary>
	/// Provides additional <see cref="T:System.DateTime"/> functions
	/// </summary>
	public class DateTimeHelper
	{
		private DateTimeHelper(){}

		#region Time Zone Functions
		/// <summary>
		/// This function sets the current time-zone parameters. These parameters control translations from Coordinated Universal Time (UTC) to local time
		/// </summary>
		/// <param name="tzi"></param>
		public static void SetTimeZoneInformation( TimeZoneInformation tzi )
		{
			// Call CE function (implicit conversion occurs to
			// byte[]).
            if (!NativeMethods.SetTimeZoneInformation(tzi))
			{
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Cannot Set Time Zone");
			}
		}

		/// <summary>
		/// This function gets the time-zone parameters for the active
		/// time-zone. These parameters control translations from Coordinated 
		/// Universal Time (UTC) to local time.
		/// </summary>
		/// <param name="tzi"></param>
		public static TimeZoneState GetTimeZoneInformation( ref TimeZoneInformation tzi )
		{
			// Call CE function (implicit conversion occurs to
			// byte[]).
			TimeZoneState	stat;
            if ((stat = NativeMethods.GetTimeZoneInformation(tzi)) == 
				TimeZoneState.Unknown )
			{
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Cannot Get Time Zone");
			}

			return stat;
		}
		#endregion

		#region Local Time
        /// <summary>
        /// Sets or gets the device's local time
        /// </summary>
		public static DateTime LocalTime
		{
            set
            {
                SystemTime st = OpenNETCF.Win32.SystemTime.FromDateTime(value);
                if (!NativeMethods.SetLocalTime(st.ToByteArray()))
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Cannot Set Local Time");
                }
            }
            get
            {
                SystemTime st = new SystemTime();
                if (!NativeMethods.GetLocalTime(st.ToByteArray()))
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Cannot Get Local Time");
                }
                return st;
            }
		}
		#endregion

		#region System Time
        /// <summary>
        /// Sets or gets the device's system time
        /// </summary>
        public static DateTime SystemTime
		{
            set
            {
                SystemTime st = OpenNETCF.Win32.SystemTime.FromDateTime(value);

                if (!NativeMethods.SetSystemTime(st.ToByteArray()))
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Cannot Set System Time");
                }
            }
            get
            {
                SystemTime st = new SystemTime();

                if (!NativeMethods.GetSystemTime(st.ToByteArray()))
                {
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Cannot Set System Time");
                }

                return st;
            }
		}
		#endregion		
	}
	#endregion
}
