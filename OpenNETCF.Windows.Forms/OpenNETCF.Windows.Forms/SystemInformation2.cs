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
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.WindowsCE.Forms;

//Retrieve power and other system metrics. Peter Foot

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Provides information about the current system environment.
	/// </summary>
	/// <remarks>Enhances functionality available in <see cref="SystemInformation"/>.</remarks>
	public class SystemInformation2
	{
		private SystemInformation2(){}

		private static PowerStatus powerStatus;

        /// <summary>
        /// Gets the thickness, in pixels, of a flat-style window or system control border.
        /// </summary>
        public static Size BorderSize
        {
            get
            {
                return SystemInformation.BorderSize;
            }
        }

		/// <summary>
		/// Gets the height, in pixels, of the standard title bar area of a window.
		/// </summary>
		public static int CaptionHeight
		{
			get
			{
				return NativeMethods.GetSystemMetrics(NativeMethods.SM.CYCAPTION);
			}
		}

		/// <summary>
		/// Gets the maximum size, in pixels, that a cursor can occupy.
		/// </summary>
		public static Size CursorSize
		{
			get
			{
                return new Size(
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CXCURSOR), 
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CYCURSOR));
			}
		}

		/// <summary>
		/// Gets a value indicating whether the debug version of Windows CE is installed.
		/// </summary>
		public static bool DebugOS
		{
			get
			{
                return (NativeMethods.GetSystemMetrics(NativeMethods.SM.DEBUG) != 0);
			}
		}

		/// <summary>
		/// Gets the dimensions, in pixels, of the area within which the user must click twice for the operating system to consider the two clicks a double-click.
		/// </summary>
		public static Size DoubleClickSize
		{
			get
			{
                return new Size(
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CXDOUBLECLK), 
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CYDOUBLECLK));
			}
		}

		//for completeness wrap the framework property
		/// <summary>
		/// Gets the maximum number of milliseconds allowed between mouse clicks for a double-click to be valid.
		/// </summary>
		public static int DoubleClickTime
		{
			get
			{
				return SystemInformation.DoubleClickTime;
			}
		}
		/// <summary>
		/// Gets the thickness, in pixels, of the frame border of a window that has a caption and is not resizable.
		/// </summary>
		public static Size FixedFrameBorderSize
		{
			get
			{
                return new Size(
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CXDLGFRAME),
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CYDLGFRAME));
			}
		}

		/// <summary>
		/// Gets the width, in pixels, of the arrow bitmap on the horizontal scroll bar.
		/// </summary>
		public static int HorizontalScrollBarArrowWidth
		{
			get
			{
                return NativeMethods.GetSystemMetrics(NativeMethods.SM.CXHSCROLL);
			}
		}

		/// <summary>
		/// Gets the default height, in pixels, of the horizontal scroll bar.
		/// </summary>
		public static int HorizontalScrollBarHeight
		{
			get
			{
                return NativeMethods.GetSystemMetrics(NativeMethods.SM.CYHSCROLL);
			}
		}

		/// <summary>
		/// Gets the dimensions, in pixels, of the Windows default program icon size.
		/// </summary>
		public static Size IconSize
		{
			get
			{
                return new Size(
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CXICON),
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CYICON));
			}
		}

		/// <summary>
		/// Gets the size, in pixels, of the grid square used to arrange icons in a large-icon view.
		/// </summary>
		public static Size IconSpacingSize
		{
			get
			{
                return new Size(
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CXICONSPACING),
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CXICONSPACING));
			}
		}

		/// <summary>
		/// Gets the height, in pixels, of one line of a menu.
		/// </summary>
		public static int MenuHeight
		{
			get
			{
                return SystemInformation.MenuHeight;
			}
		}

		/// <summary>
		/// Gets the number of display monitors.
		/// </summary>
		public static int MonitorCount
		{
			get
			{
                int cmons = NativeMethods.GetSystemMetrics(NativeMethods.SM.CMONITORS);
				if(cmons == 0)
				{
					return 1;
				}

				return cmons;
			}
		}

		/// <summary>
		/// Gets a value indicating whether all the display monitors are using the same pixel color format.
		/// </summary>
		public static bool MonitorsSameDisplayFormat
		{
			get
			{
				if(MonitorCount > 1)
				{
                    return (NativeMethods.GetSystemMetrics(NativeMethods.SM.SAMEDISPLAYFORMAT) != 0);
				}
				return true;
			}
		}

		/// <summary>
		/// Gets a value indicating whether a mouse is installed.
		/// </summary>
		public static bool MousePresent
		{
			get
			{
                return (NativeMethods.GetSystemMetrics(NativeMethods.SM.MOUSEPRESENT) != 0);
			}
		}

		#region Power Status
		/// <summary>
		/// Gets the current system power status.
		/// </summary>
		public static PowerStatus PowerStatus
		{
			get
			{
				if(powerStatus==null)
				{
					powerStatus = new PowerStatus();
				}

				return powerStatus;

			}
		} 
		#endregion

		/// <summary>
		/// Gets the dimensions, in pixels, of the current video mode of the primary display.
		/// </summary>
		public static Size PrimaryMonitorSize
		{
			get
			{
                return new Size(
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CXSCREEN),
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CYSCREEN));
			}
		}

        #region Screen Orientation
        /// <summary>
        /// Gets the orientation of the screen.
        /// </summary>
        /// <value>The orientation of the screen, in degrees.</value>
        /// <seealso cref="Microsoft.WindowsCE.Forms.SystemSettings"/>
        public static ScreenOrientation ScreenOrientation
        {
            get
            {
                return (ScreenOrientation)Microsoft.WindowsCE.Forms.SystemSettings.ScreenOrientation;
            }
        }
        #endregion

		/// <summary>
		/// Gets the dimensions, in pixels, of a small icon.
		/// </summary>
		public static Size SmallIconSize
		{
			get
			{
                return new Size(
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CXSMICON),
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CYSMICON));
			}
		}

		/// <summary>
		/// Gets the height, in pixels, of the arrow bitmap on the vertical scroll bar.
		/// </summary>
		public static int VerticalScrollBarArrowHeight
		{
			get
			{
                return NativeMethods.GetSystemMetrics(NativeMethods.SM.CYVSCROLL);
			}
		}

		/// <summary>
		/// Gets the default width, in pixels, of the vertical scroll bar.
		/// </summary>
		public static int VerticalScrollBarWidth
		{
			get
			{
                return NativeMethods.GetSystemMetrics(NativeMethods.SM.CXVSCROLL);
			}
		}

		/// <summary>
		/// Gets the bounds of the virtual screen.
		/// </summary>
		public static Rectangle VirtualScreen
		{
			get
			{
                return new Rectangle(
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.XVIRTUALSCREEN),
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.YVIRTUALSCREEN),
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CXVIRTUALSCREEN),
                    NativeMethods.GetSystemMetrics(NativeMethods.SM.CYVIRTUALSCREEN));
			}
		}

		#region Working Area
		/// <summary>
		/// Gets the size, in pixels, of the working area of the screen.
		/// </summary>
		public static Rectangle WorkingArea
		{
			get
			{
				return Screen.PrimaryScreen.WorkingArea;
			}
		}
		#endregion

    }

}
