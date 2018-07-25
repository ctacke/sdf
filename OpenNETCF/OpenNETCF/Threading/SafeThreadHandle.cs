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
using OpenNETCF.Win32.SafeHandles;

namespace OpenNETCF.Threading
{
    /// <summary>
    /// This class is used a wrapper for the handle owned by a <c>ThreadEx</c> class
    /// </summary>
    internal class SafeThreadHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public delegate IntPtr CreateHandleDelegate();

        private CreateHandleDelegate m_createDelegate;

        /// <summary>
        /// Constructor for a SafeThreadHandle
        /// </summary>
        /// <param name="createHandleFunction"></param>
        public SafeThreadHandle(CreateHandleDelegate createHandleFunction)
            : base(true)
        {
            m_createDelegate = createHandleFunction;
        }

        /// <summary>
        /// Opens the instance of the SafeHandle by calling the <c>CreateHandleDelegate</c>
        /// </summary>
        public virtual void Open()
        {
            if (m_createDelegate != null)
            {
                handle = m_createDelegate();

                if (IsInvalid)
                {
                    throw new Exception("Created Handle is in list of Invalid Values");
                }
            }
            else
            {
                throw new Exception("No delegate avaialble to create SafeHandle");
            }
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseHandle(handle);
        }
    }
}
