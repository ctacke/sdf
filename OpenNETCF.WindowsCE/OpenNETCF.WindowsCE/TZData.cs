using System;
using System.Runtime.InteropServices;

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
            pData = (IntPtr)(pData.ToInt32() + 8);
            DSTOffset = Marshal.ReadInt32(pData);
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
        /// Timezone's DST offset (in minutes)
        /// </summary>
        public readonly int DSTOffset;

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
