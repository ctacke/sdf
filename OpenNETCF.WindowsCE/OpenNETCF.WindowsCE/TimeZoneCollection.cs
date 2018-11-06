using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System;

namespace OpenNETCF.WindowsCE
{
	/// <summary>
	/// Define a TimeZoneCollection which can return a list of all
	/// of the TimeZones known to the OS.  This should help make
	/// time zone picker controls, etc.
	/// </summary>
	public class TimeZoneCollection : System.Collections.ArrayList 
	{
        /// <summary>
        /// All timezones
        /// </summary>
		public const int ALL_TIMEZONES_LIST = 999;

		/// <summary>
		/// Creates a new instance of TimeZoneCollection
		/// </summary>
		public TimeZoneCollection()
		{
		}

		/// <summary>
		/// Refreshes the contents of the TimeZoneCollection
		/// </summary>
		public void Refresh()
		{
			// Clear the collection and reinitialize.
			this.Clear();

			this.Initialize();
		}

		/// <summary>
		/// The Initialize() method is equivalent to calling
		/// Initialize( ALL_TIMEZONES_LIST ).  It populates the
		/// collection with all OS-known timezones.
		/// </summary>
		public void Initialize()
		{
			this.Initialize( ALL_TIMEZONES_LIST );
		}

		private void InitFromRegistry( int gmtOffset )
		{
			TimeZoneInformation mTimeZoneInformation = new TimeZoneInformation();

			RegistryKey	baseKey;

			baseKey = Registry.LocalMachine.OpenSubKey("Time Zones", false);
			if ( baseKey != null )
			{
				// Enumerate all keys under the base timezone key.
				//string	cls;
				string[] subkeynames = baseKey.GetSubKeyNames();
				foreach(string thiskeyname in subkeynames)
				{
					// Open the enumerated key.
					RegistryKey	tzKey = baseKey.OpenSubKey(thiskeyname, false);
					if ( tzKey  != null )
					{
						// Get the display name value for the timezone.
						string			dispName;

                        // FIX: TimeZoneInformation.DisplayName contains null character (Bug #29)
						dispName = tzKey.GetValue("Display").ToString().Trim();

						if(dispName != null)
						{
							// Extract the offset from GMT from the display string.
							// The basic format of the string is:
							// (GMT-%02d:%02d) <Name>
							// The problem is when you are *at* GMT, where the sign 
							// and the numbers aren't present.
							int	hoursOffset = 999, minutesOffset = 999;

							string	hours;
							string	minutes;
							hours = dispName.Substring( 4, 3 );
							minutes = dispName.Substring( 8, 2 );

							// For the case of GMT itself, there is no
							// offset in the display name, so we
							// set the offset to zero, in that case.
							if ( ( hours[ 0 ] == '-' ) ||
								( hours[ 0 ] == '+' ) )
							{
								hoursOffset = System.Int32.Parse( hours );
								minutesOffset = System.Int32.Parse( minutes );
							}
							else
							{
								hoursOffset = 0;
								minutesOffset = 0;
							}

							// Compute the timezone's total offset in minutes
							int offset = hoursOffset * 60;
								
							if ( hoursOffset < 0 )
							{
								offset -= minutesOffset;
							}
							else
							{
								offset += minutesOffset;
							}
								
							// We now have a value which we can compare to the 
							// value that the user gave us.
							if ( ( gmtOffset == offset ) || 
								( gmtOffset == ALL_TIMEZONES_LIST ) )
							{
								// Match.  Add timezone to list.  So far, we have
								// the standard time name and the display name.  
								// We still need to get the DST name and the 
								// TIME_ZONE_INFORMATION.
								string	dstName;
								dstName = tzKey.GetValue("Dlt").ToString();
								var stdName = tzKey.GetValue( "Std" ).ToString();

								// Read the time zone information from the 
								// registry.  Unfortunately, this is something
								// that only the Control Panel code actually 
								// knows about, so, if they change it, we break.
								TZREG					tzr = new TZREG();
								int						tzrSize = tzr.ToByteArray().Length;
								TimeZoneInformation	tzi = new TimeZoneInformation();
								byte[]					btzr;
								btzr = (byte[])tzKey.GetValue("TZI");
								tzr = new TZREG( btzr );
								tzi.Bias = tzr.Bias;
								tzi.StandardBias = tzr.StandardBias;
								tzi.DaylightBias = tzr.DaylightBias;
								tzi.StandardDate = tzr.StandardDate;
								tzi.DaylightDate = tzr.DaylightDate;

								// Don't forget to copy the standard name and
								// daylight name to the structure.
								tzi.DaylightName = dstName;
								tzi.StandardName = stdName;

								// Copy the display name from the registry to
								// the class.
								tzi.DisplayName = dispName;

								this.Add( tzi );
							}
						}//endif dispname null
						// close the timezone key.
						tzKey.Close();
					}//endif tzkey null
				}//end foreach subkeynames
				// Close the key.
				baseKey.Close();
			}
		}

        private void InitFromCityDB(int gmtOffset)
        {
            TimeZoneInformation mTimeZoneInformation = new TimeZoneInformation();

            NativeMethods.InitCityDb();
            NativeMethods.ClockLoadAllTimeZoneData();
            int nZones = NativeMethods.ClockGetNumTimezones();
            IntPtr pZone = IntPtr.Zero;
            TZData tmpTZ;

            for (int i = 0; i < nZones; i++)
            {
                int tzIndex;
                pZone = NativeMethods.ClockGetTimeZoneDataByOffset(i, out tzIndex);
                if (pZone != IntPtr.Zero)
                {
                    tmpTZ = new TZData(pZone);
                    if ((gmtOffset == tmpTZ.GMTOffset) ||
                        (gmtOffset == ALL_TIMEZONES_LIST))
                    {
                        TimeZoneInformation tzi = new TimeZoneInformation();


                        tzi.Bias = tmpTZ.GMTOffset;
                        tzi.DaylightBias = tmpTZ.DSTOffset;
						tzi.StandardBias = tmpTZ.StdOffset;

                        // Don't forget to copy the standard name and
                        // daylight name to the structure.
                        tzi.DaylightName = tmpTZ.DSTName;
                        tzi.StandardName = tmpTZ.Name;

						// Copy dates of time's transition
						tzi.StandardDate = tmpTZ.StdDate;
						tzi.DaylightDate = tmpTZ.DSTDate;

                        // Copy the display name from the registry to
                        // the class.
                        tzi.DisplayName = tmpTZ.ShortName;
                        this.Add(tzi);
                    }
                }
            }
            NativeMethods.ClockFreeAllTimeZoneData();
            NativeMethods.UninitCityDb();
        }

		/// <summary>
		/// Use the Initialize method with the gmtOffset parameter
		/// to create a list of timezones which have a specific
		/// offset value.  For example, for all timezones at 
		/// GMT-5:00, pass -300 (minutes).  To get all of the time
		/// zones known to the OS, pass ALL_TIMEZONES_LIST or call
		/// the method with no parameters.
		/// </summary>
		/// <param name="gmtOffset">Offset of the target timezones
		/// from GMT (in minutes).</param>
		public void Initialize( int gmtOffset )
		{
            if (!System.IO.File.Exists("\\windows\\citydb.dll"))//OpenNETCF.Environment2.PlatformName.IndexOf("Pocket")==-1)
            {
                InitFromRegistry(gmtOffset);
            }
            else
            {
                try
                {
                    InitFromCityDB(gmtOffset);
                }
                catch 
                {
                    InitFromRegistry(gmtOffset);
                }
            }
		}
	}
}
 