#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.WindowsCE
{
    /// <summary>
    /// This class is used for general memory management of CE-based devices
    /// </summary>
    public static class MemoryManagement
    {
        private static int m_memoryPageSize = 0;

        static MemoryManagement()
        {
            int storePages = 0;
            int ramPages = 0;

            // get page size for future use - it never changes
            NativeMethods.GetSystemMemoryDivision(ref storePages, ref ramPages, ref m_memoryPageSize);
        }

        /// <summary>
        /// Gets or sets the amount of allocated memory for Storage, in kilobytes
        /// </summary>
        public static int SystemStorageMemory
        {
            get
            {
                int storePages = 0;
                int ramPages = 0;
                int pageSize = 0;

                NativeMethods.GetSystemMemoryDivision(ref storePages, ref ramPages, ref pageSize);

                return (storePages * (pageSize  >> 10));
            }
            set
            {
                NativeMethods.SetSystemMemoryDivision((value << 10) / m_memoryPageSize);
            }
        }

        /// <summary>
        /// Gets or sets the amount of allocated memory for Programs, in kilobytes
        /// </summary>
        public static int SystemProgramMemory
        {
            get
            {
                int storePages = 0;
                int ramPages = 0;
                int pageSize = 0;

                NativeMethods.GetSystemMemoryDivision(ref storePages, ref ramPages, ref pageSize);

                return (ramPages * (pageSize >> 10));
            }
            set
            {
                int storePages = 0;
                int ramPages = 0;
                int pageSize = 0;
                
                NativeMethods.GetSystemMemoryDivision(ref storePages, ref ramPages, ref pageSize);

                SystemStorageMemory = (storePages + ramPages) - (value << 10 / pageSize);
            }
        }

        /// <summary>
        /// Specifies a number between zero and 100 that gives a general idea of current memory utilization, in which zero indicates no memory use and 100 indicates full memory use.
        /// </summary>
        public static int MemoryLoad
        {
            get
            {
                NativeMethods.MemoryStatus ms = new NativeMethods.MemoryStatus();

                NativeMethods.GlobalMemoryStatus(out ms);

                return ms.MemoryLoad;
            }
        }

        /// <summary>
        /// Indicates the total number of bytes of physical memory.
        /// </summary>
        public static int TotalPhysicalMemory
        {
            get
            {
                NativeMethods.MemoryStatus ms = new NativeMethods.MemoryStatus();

                NativeMethods.GlobalMemoryStatus(out ms);

                return ms.TotalPhysical;
            }
        }

        /// <summary>
        /// Indicates the total number of bytes that can be described in the user mode portion of the virtual address space of the calling process.
        /// </summary>
        public static int TotalVirtualMemory
        {
            get
            {
                NativeMethods.MemoryStatus ms = new NativeMethods.MemoryStatus();

                NativeMethods.GlobalMemoryStatus(out ms);

                return ms.TotalVirtual;
            }
        }

        /// <summary>
        /// Indicates the number of bytes of physical memory available.
        /// </summary>
        public static int AvailablePhysicalMemory
        {
            get
            {
                NativeMethods.MemoryStatus ms = new NativeMethods.MemoryStatus();

                NativeMethods.GlobalMemoryStatus(out ms);

                return ms.AvailablePhysical;
            }
        }

        /// <summary>
        /// Indicates the number of bytes of unreserved and uncommitted memory in the user mode portion of the virtual address space of the calling process.
        /// </summary>
        public static int AvailableVirtualMemory
        {
            get
            {
                NativeMethods.MemoryStatus ms = new NativeMethods.MemoryStatus();

                NativeMethods.GlobalMemoryStatus(out ms);

                return ms.AvailableVirtual;
            }
        }
    }
}
