using System;
using System.Runtime.InteropServices;
using OpenNETCF.Win32;

namespace OpenNETCF.WindowsCE
{
    #region SystemInfo
    /// <summary>
    /// This structure contains information about the current computer system. This includes the processor type, page size, memory addresses, and OEM identifier.
    /// </summary>
    /// <seealso cref="NativeMethods.GetSystemInfo"/>
    public struct SystemInfo
    {
        /// <summary>
        /// The system's processor architecture.
        /// </summary>
        public ProcessorArchitecture ProcessorArchitecture;

        internal ushort wReserved;
        /// <summary>
        /// The page size and the granularity of page protection and commitment.
        /// </summary>
        public int PageSize;
        /// <summary>
        /// Pointer to the lowest memory address accessible to applications and dynamic-link libraries (DLLs). 
        /// </summary>
        public int MinimumApplicationAddress;
        /// <summary>
        /// Pointer to the highest memory address accessible to applications and DLLs.
        /// </summary>
        public int MaximumApplicationAddress;
        /// <summary>
        /// Specifies a mask representing the set of processors configured into the system. Bit 0 is processor 0; bit 31 is processor 31. 
        /// </summary>
        public int ActiveProcessorMask;
        /// <summary>
        /// Specifies the number of processors in the system.
        /// </summary>
        public int NumberOfProcessors;
        /// <summary>
        /// Specifies the type of processor in the system.
        /// </summary>
        public ProcessorType ProcessorType;
        /// <summary>
        /// Specifies the granularity with which virtual memory is allocated.
        /// </summary>
        public int AllocationGranularity;
        /// <summary>
        /// Specifies the system’s architecture-dependent processor level.
        /// </summary>
        public short ProcessorLevel;
        /// <summary>
        /// Specifies an architecture-dependent processor revision.
        /// </summary>
        public short ProcessorRevision;
    }
    #endregion

    public struct TZData
    {
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

			//jsm - Bug 235 Add Std/Daylight Dates to TZData
			pData = (IntPtr)(pData.ToInt32() + 4);
			byte[] buf = new byte[128];
			Marshal.Copy(pData, buf, 0, 128);
			StandardDate = new SystemTime(buf);
			DaylightDate = new SystemTime(buf, 16);
        }

        public readonly string Name;
        public readonly string DSTName;
        public readonly string ShortName;
        public readonly int GMTOffset;
        public readonly int DSTOffset;

		//jsm - Bug 235 Add Std/Daylight Dates to TZData
		public readonly SystemTime StandardDate;
		public readonly SystemTime DaylightDate;

        public override string ToString()
        {
            return ShortName;
        }

    }

}
