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
