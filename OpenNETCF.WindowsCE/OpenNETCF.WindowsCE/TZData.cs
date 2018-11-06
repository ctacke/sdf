using System;
using System.Runtime.InteropServices;
using OpenNETCF.Win32;

namespace OpenNETCF.WindowsCE
{
    /// <summary>
    /// Timezone Data
    /// </summary>
    public struct TZData
    {
        /// <summary>
        /// Creates a TZData instance
        /// </summary>
        /// <param name="pData"></param>
        public TZData(IntPtr pData)
        {
            Name = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(pData));
            pData = (IntPtr)(pData.ToInt32() + 4);
            ShortName = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(pData));
            pData = (IntPtr)(pData.ToInt32() + 4);
            DSTName = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(pData));
            pData = (IntPtr)(pData.ToInt32() + 4);
            GMTOffset = Marshal.ReadInt32(pData);
            pData = (IntPtr)(pData.ToInt32() + 4);
            StdOffset = Marshal.ReadInt32(pData);
            pData = (IntPtr)(pData.ToInt32() + 4);
            DSTOffset = Marshal.ReadInt32(pData);

            byte[] bbSystemTime = new byte[16];

            pData = (IntPtr)( pData.ToInt32() + 4 );
            Marshal.Copy( pData, bbSystemTime, 0, bbSystemTime.Length );
            StdDate = new SystemTime( bbSystemTime );

            pData = (IntPtr)( pData.ToInt32() + bbSystemTime.Length );
            Marshal.Copy( pData, bbSystemTime, 0, bbSystemTime.Length );
            DSTDate = new SystemTime( bbSystemTime );
        }

        /// <summary>
        /// Timezone's full name
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// Timezone's DST name
        /// </summary>
        public readonly string DSTName;
        /// <summary>
        /// Timezone's short name
        /// </summary>
        public readonly string ShortName;
        /// <summary>
        /// Timezone's offset from GMT (in minutes)
        /// </summary>
        public readonly int GMTOffset;
        /// <summary>
        /// StandardBias (in minutes)
        /// </summary>
        public readonly int StdOffset;
        /// <summary>
        /// Timezone's DST offset (in minutes)
        /// </summary>
        public readonly int DSTOffset;
        /// <summary>
        /// StandardDate
        /// </summary>
        public readonly SystemTime StdDate;
        /// <summary>
        /// DaylightDate
        /// </summary>
        public readonly SystemTime DSTDate;

        /// <summary>
        /// Returns the timezone's short name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ShortName;
        }
    }
}
