using System;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Win32;

namespace OpenNETCF.WindowsCE
{
    /// <summary>
    /// Time Zone information
    /// </summary>
    /// <remarks>Wraps the native <b>TIME_ZONE_INFORMATION</b> structure.</remarks>
    public class TimeZoneInformation
    {
        // Declare simple string to be used as the display name
        // for the timezone.  This would be used when creating
        // a collection of all known timezones.
        private string displayName;

        // Declare array of bytes to represent the structure
        // contents in a form which can be marshalled.
        private byte[] flatStruct = new byte[4 + 64 + 16 + 4 + 64 + 16 + 4];

        // Now, declare the offsets of the various fields within
        // that array of bytes.
        #region Flat structure offset constants
        private const int biasOffset = 0;
        private const int standardNameOffset = 4;
        private const int standardNameLengthBytes = 64;
        private const int standardDateOffset = 4 + 64 /* sizeof( WCHAR ) * 32 */;
        private const int standardBiasOffset = 4 + 64 + 16 /* sizeof( SYSTEMTIME ) */;
        private const int daylightNameOffset = 4 + 64 + 16 + 4;
        private const int daylightNameLengthBytes = 64;
        private const int daylightDateOffset = 4 + 64 + 16 + 4 + 64;
        private const int daylightBiasOffset = 4 + 64 + 16 + 4 + 64 + 16;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of the <b>TimeZoneInformation</b> class.
        /// </summary>
        public TimeZoneInformation()
        {
        }
        #endregion

        #region Constructor (Byte[])
        /// <summary>
        /// Create a new instance of the TimeZoneInformation class based on data in the supplied Byte Array.
        /// </summary>
        /// <param name="bytes">Byte Array containing <b>TIME_ZONE_INFORMATION</b> data.</param>
        public TimeZoneInformation(byte[] bytes)
            :
            this(bytes, 0)
        {
        }
        #endregion

        /// <summary>
        /// Creates a TimeZoneInformation instance
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        public TimeZoneInformation(byte[] bytes, int offset)
        {
            // Dump the byte array into our array.
            Buffer.BlockCopy(bytes, offset, flatStruct, 0, flatStruct.Length);
        }

        // ???? need constructors with various elements for 
        // creating 'custom' timezones.

        #region To String
        /// <summary>
        /// Returns a String representing this instance of <b>TimeZoneInformation</b>.
        /// </summary>
        /// <returns>A string containing the name of the Time Zone.</returns>
        public override string ToString()
        {
            // Set the string to the default description of the
            // timezone.
            return this.DisplayName;
        }
        #endregion

        /// <summary>
        /// Returns the raw structure in a <see cref="T:System.Byte[]"/>.
        /// </summary>
        /// <returns>Byte array containing the <b>SYSTEMTIME</b> data.</returns>
        public byte[] ToByteArray()
        {
            return flatStruct;
        }

        /// <summary>
        /// Provides a native-code supported byte array representation of a TimeZoneInformation
        /// </summary>
        /// <param name="tzi"></param>
        /// <returns></returns>
        public static implicit operator byte[](TimeZoneInformation tzi)
        {
            return tzi.ToByteArray();
        }

        /// <summary>
        /// Creates a TimeZoneInformation instance from a byte array representation
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static implicit operator TimeZoneInformation(byte[] data)
        {
            return new TimeZoneInformation(data);
        }

#region Display Name
        /// <summary>
        /// Name used to describe the Time Zone.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return displayName;
            }
            set
            {
                displayName = value;
            }
        }
        #endregion

        #region Bias
        /// <summary>
        /// Specifies the current bias, in minutes, for local time translation on this computer.
        /// </summary>
        /// <remarks>The bias is the difference, in minutes, between Coordinated Universal Time (UTC) and local time.
        /// All translations between UTC and local time are based on the following formula:
        /// <para>UTC = local time + bias</para></remarks>
        public int Bias
        {
            get
            {
                return BitConverter.ToInt32(flatStruct, biasOffset);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, flatStruct, biasOffset, 4);
            }
        }
        #endregion

        #region Standard Name
        /// <summary>
        /// Name associated with standard time on this device.
        /// </summary>
        /// <remarks>For example, this member could contain “EST” to indicate Eastern Standard Time.</remarks>
        public string StandardName
        {
            get
            {
                String sReturn = System.Text.Encoding.Unicode.GetString(flatStruct, standardNameOffset, standardNameLengthBytes /* in BYTES */ );
                sReturn = sReturn.Substring(0, sReturn.IndexOf('\0'));
                return sReturn;
            }
            set
            {
                byte[] bytes = System.Text.Encoding.Unicode.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, flatStruct, standardNameOffset, Math.Min(bytes.Length, standardNameLengthBytes));
            }
        }
        #endregion

        #region Standard Date
        /// <summary>
        /// The date and local time when the transition from Daylight time to Standard time occurs.
        /// </summary>
        /// <remarks>This member supports two date formats.
        /// Absolute format specifies an exact date and time when standard time begins.
        /// In this form, the wYear, wMonth, wDay, wHour, wMinute, wSecond, and wMilliseconds members of the SYSTEMTIME structure are used to specify an exact date.
        /// <para>Day-in-month format is specified by setting the wYear member to zero, setting the wDayOfWeek member to an appropriate weekday, and using a wDay value in the range 1 through 5 to select the correct day in the month.
        /// Using this notation, the first Sunday in April can be specified, as can the last Thursday in October (5 is equal to “the last”).</para></remarks>
        public SystemTime StandardDate
        {
            get
            {
                return new SystemTime(flatStruct, standardDateOffset);
            }
            set
            {
                byte[] bytes = value.ToByteArray();
                Buffer.BlockCopy(bytes, 0, flatStruct, standardDateOffset, 16);
            }
        }
        #endregion

        #region Standard Bias
        /// <summary>
        /// Specifies a bias value to be used during local time translations that occur during standard time.
        /// </summary>
        /// <remarks>This member is ignored if a value for the StandardDate member is not supplied.
        /// This value is added to the value of the Bias member to form the bias used during standard time.
        /// In most time zones, the value of this member is zero.</remarks>
        public int StandardBias
        {
            get
            {
                return BitConverter.ToInt32(flatStruct, standardBiasOffset);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, flatStruct, standardBiasOffset, 4);
            }
        }
        #endregion

        #region Daylight Name
        /// <summary>
        /// Specifies a string associated with daylight time.
        /// </summary>
        public string DaylightName
        {
            get
            {
                String sReturn = System.Text.Encoding.Unicode.GetString(flatStruct, daylightNameOffset, daylightNameLengthBytes /* in BYTES */ );
                sReturn = sReturn.Substring(0, sReturn.IndexOf('\0'));
                return sReturn;
            }
            set
            {
                value = value.TrimEnd(new char[] { (char)0 });
                byte[] bytes = System.Text.Encoding.Unicode.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, flatStruct, daylightNameOffset, Math.Min(bytes.Length, daylightNameLengthBytes));
            }
        }
        #endregion

        #region Daylight Date
        /// <summary>
        /// Specifies a date and local time when the transition from standard time to daylight time occurs. 
        /// </summary>
        public SystemTime DaylightDate
        {
            get
            {
                return new SystemTime(flatStruct, daylightDateOffset);
            }
            set
            {
                byte[] bytes = value.ToByteArray();
                Buffer.BlockCopy(bytes, 0, flatStruct, daylightDateOffset, 16);
            }
        }
        #endregion

        #region Daylight Bias
        /// <summary>
        /// Specifies a bias value to be used during local time translations that occur during daylight time.
        /// </summary>
        public int DaylightBias
        {
            get
            {
                return BitConverter.ToInt32(flatStruct, daylightBiasOffset);
            }
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Buffer.BlockCopy(bytes, 0, flatStruct, daylightBiasOffset, 4);
            }
        }
        #endregion
    }
}
