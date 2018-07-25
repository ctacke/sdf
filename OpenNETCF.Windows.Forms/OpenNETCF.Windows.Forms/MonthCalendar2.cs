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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenNETCF.Win32;

namespace OpenNETCF.Windows.Forms
{
    /// <summary>
    /// Extends the <see cref="MonthCalendar"/> control.
    /// </summary>
    public class MonthCalendar2 : System.Windows.Forms.MonthCalendar, IWin32Window
    {
        private MonthCalendarNativeWindow nativeWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthCalendar2"/> class.
        /// </summary>
        public MonthCalendar2()
        {
            if (!StaticMethods.IsDesignMode(this))
            {
                nativeWindow = new MonthCalendarNativeWindow(this);
            }
        }

        /// <summary>
        /// Occurs when the user makes an explicit date selection using the mouse.
        /// </summary>
        public event System.Windows.Forms.DateRangeEventHandler DateSelected;

        /// <summary>
        /// Raises the <see cref="DateSelected"/> event.
        /// </summary>
        /// <param name="drevent">A <see cref="System.Windows.Forms.DateRangeEventArgs"/> that contains the event data.</param>
        protected internal void OnDateSelected(System.Windows.Forms.DateRangeEventArgs drevent)
        {
            if (DateSelected != null)
            {
                this.DateSelected(this, drevent);
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (!StaticMethods.IsDesignMode(this))
            {
                if (this.Parent != null)
                {
                    nativeWindow.AssignHandle(this.Parent.Handle);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!StaticMethods.IsDesignMode(this))
            {
                nativeWindow.ReleaseHandle();
            }
            base.Dispose(disposing);
        }
    }

    internal class MonthCalendarNativeWindow : NativeWindow
    {
        private MonthCalendar2 parent;

        internal MonthCalendarNativeWindow(MonthCalendar2 parent)
        {
            this.parent = parent;
        }
        protected override void WndProc(ref Microsoft.WindowsCE.Forms.Message m)
        {
            if(m.Msg == (int)WM.NOTIFY)
			{
				//marshal notification data into NMHDR struct
				NMHDR hdr = (NMHDR)Marshal.PtrToStructure(m.LParam, typeof(NMHDR));

                if (hdr.hwndFrom == parent.Handle)
                {
                    switch (hdr.code)
                    {
                        //definite selection
                        case (int)MCN.SELECT:
                            //copy date range
                            SystemTime stStart = new SystemTime();
                            Marshal.Copy((IntPtr)((int)m.LParam + 12), stStart.ToByteArray(), 0, stStart.ToByteArray().Length);
                            SystemTime stEnd = new SystemTime();
                            Marshal.Copy((IntPtr)((int)m.LParam + 28), stEnd.ToByteArray(), 0, stEnd.ToByteArray().Length);
                            //raise datechanged event
                            parent.OnDateSelected(new DateRangeEventArgs(stStart.ToDateTime(), stEnd.ToDateTime()));
                            break;
                    }
                }
			}

            base.WndProc(ref m);
        }
    }
}
