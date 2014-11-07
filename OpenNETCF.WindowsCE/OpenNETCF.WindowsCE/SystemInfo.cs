using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.WindowsCE
{
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
}
