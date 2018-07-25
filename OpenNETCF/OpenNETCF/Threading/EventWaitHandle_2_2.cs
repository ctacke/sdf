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
using System.Threading;
using System.Runtime.InteropServices;

namespace OpenNETCF.Threading
{
    public partial class EventWaitHandle : WaitHandle
    {
        /// <summary>
        /// Sets the state of the event to signaled, allowing one or more waiting threads to proceed.
        /// </summary>
        /// <param name="data">Data that can be read by the event recipient</param>
        /// <returns>true if the function succeeds; otherwise, false.</returns>
        public bool Set(int data)
        {
            SetEventData(this.Handle, data);
            return NativeMethods.EventModify(this.Handle, NativeMethods.EVENT.SET);
        }

        /// <summary>
        /// Provides a single operation that sets to signaled the state of the specified event object and then resets it to nonsignaled after releasing the appropriate number of waiting threads.
        /// </summary>
        /// <remarks>
        /// For a manual-reset event object, all waiting threads that can be released immediately are released. The function then resets the event object's state to nonsignaled and returns. 
        /// For an auto-reset event object, the function resets the state to nonsignaled and returns after releasing a single waiting thread, even if multiple threads are waiting. 
        /// If no threads are waiting, or if no thread can be released immediately, PulseEvent simply sets the event object's state to nonsignaled and returns. 
        /// For a thread using the multiple-object wait functions to wait for all specified objects to be signaled, PulseEvent can set the event object's state to signaled and reset it to nonsignaled without causing the wait function to return. This happens if not all of the specified objects are simultaneously signaled. 
        /// </remarks>
        /// <returns>true on success, otherwise false</returns>
        public bool Pulse()
        {
            return NativeMethods.EventModify(this.Handle, NativeMethods.EVENT.PULSE);
        }

        /// <summary>
        /// Gets the data associated (provided with a call to Set) with this event.
        /// </summary>
        /// <returns>The associated data</returns>
        public int GetData()
        {
            return GetEventData(this.Handle);
        }

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int GetEventData(IntPtr hEvent);

        [DllImport("coredll.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetEventData(IntPtr hEvent, int dwData);
    }
}
