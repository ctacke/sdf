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
using System.Threading;
using System.Runtime.InteropServices;

namespace OpenNETCF.ToolHelp
{
	public partial class ThreadEntry
	{
        /// <summary>
        /// Either suspends the thread, or if the thread is already suspended, has no effect.
        /// </summary>
        public void Suspend()
        {
            ProcessAPI.SuspendThread(this.ThreadID);
        }

        /// <summary>
        /// Resumes a thread that has been suspended.
        /// </summary>
        public void Resume()
        {
            ProcessAPI.ResumeThread(this.ThreadID);
        }

        /// <summary>
        /// Sets a value indicating the scheduling priority of a thread.
        /// </summary>
        /// <param name="newPriority">One of the <see cref="System.Threading.ThreadPriority"/> values.</param> 
        public void SetPriority(ThreadPriority newPriority)
        {
            ProcessAPI.CeSetThreadPriority((uint)this.ThreadID, (int)newPriority);
        }
        /// <summary>
        /// Gets or sets a priority value outside of application priority space
        /// </summary>
        /// <param name="newPriority">The new Priority, from 0 to 255</param>
        /// <remarks>
        /// <b>WARNING:</b> Adjusting a thread priority with this property can lead to application and even device deadlock or unpredictability.  Use only with caution and strong knowledge of the target system.  Do <u>not</u> use this Property for normal Priority settings.
        /// </remarks>
        public void SetRealtimePriority(int newPriority)
        {
            if (Environment.OSVersion.Platform != PlatformID.WinCE)
            {
                throw new NotSupportedException("This method is supported only under Windows CE");
            }

            if ((newPriority < 0) || (newPriority > 255))
            {
                throw new ArgumentOutOfRangeException("Priority must be between 0 and 255");
            }
            ProcessAPI.CeSetThreadPriority((uint)this.ThreadID, newPriority);
        }

        /// <summary>
        /// Gets or sets a the thread's quantum in milliseconds.  Use zero for "run to completion". Unless modified by the OEM, the system default is 100ms
        /// </summary>
        /// <param name="newQuantum">The new thread quantum</param>
        /// <remarks>
        /// <b>WARNING:</b> Adjusting a thread quantum with this property can lead to application and even device deadlock or unpredictability.  Use only with caution and strong knowledge of the target system.
        /// </remarks>
        public void SetRealtimeQuantum(int newQuantum)
        {
            if (Environment.OSVersion.Platform != PlatformID.WinCE)
            {
                throw new NotSupportedException("This method is supported only under Windows CE");
            }

            if (newQuantum < 0)
            {
                throw new ArgumentOutOfRangeException("Quantum must be greater than zero");
            }

            ProcessAPI.CeSetThreadQuantum(this.ThreadID, newQuantum);
        }

    }
}
