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
using System.Runtime.InteropServices;
using OpenNETCF.Win32;
using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.WindowsCE.Notification
{
	#region Notify
	/// <summary>
	/// Contains Notification related Methods.
	/// </summary>
	public static class Notify
	{

		#region Run App At Event
		/// <summary>   
		/// This function starts running an application when a specified event occurs.   
		/// </summary>   
		/// <param name="appName">Name of the application to be started.</param>   
		/// <param name="whichEvent">Event at which the application is to be started.</param>   
		/// <seealso cref="NotificationEvent"/>   
		public static void RunAppAtEvent(string appName, NotificationEvent whichEvent)
		{
			if (!NativeMethods.CeRunAppAtEvent(appName, (int)whichEvent))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Cannot Set Notification Handler");
			}
		}

		#endregion

		#region Run App At Time
		/// <summary>   
		/// This function prompts the system to start running a specified application at a specified time.   
		/// </summary>   
		/// <param name="appName">Name of the application to be started.</param>   
		/// <param name="time">DateTime at which to run application.</param>
		/// <remarks>To cancel an existing RunAppATime request pass the application name and DateTime.MinValue</remarks>
		public static void RunAppAtTime(string appName, System.DateTime time)
		{
			SYSTEMTIME st = new SYSTEMTIME();

			if (time != System.DateTime.MinValue)
			{
				//get native system time struct
				st = new SYSTEMTIME();
				st = SYSTEMTIME.FromDateTime(time);

				if (!NativeMethods.CeRunAppAtTime(appName, ref st))
				{
					throw new Win32Exception(Marshal.GetLastWin32Error(), "Cannot Set Notification Handler");
				}
			}
			else
			{
				if (!NativeMethods.CeRunAppAtTimeCancel(appName, null))
				{
					throw new Win32Exception(Marshal.GetLastWin32Error(), "Cannot Cancel Notification Handler");
				}
			}
		}

		/// <summary>
		/// This function will cause a named system event to be set at the given time.
		/// </summary>
		/// <remarks>If suspended, the device will wake to fulfill this notification</remarks>
		/// <param name="eventName">String name of the event to set</param>
		/// <param name="eventTime">Time at which to set the event</param>
		public static void SetNamedEventAtTime(string eventName, DateTime eventTime)
		{
			Notify.RunAppAtTime(string.Format(@"\\.\Notifications\NamedEvents\{0}", eventName), eventTime);
		}

		/// <summary>
		/// This function will cause a named system event to be set at the given time.
		/// </summary>
		/// <remarks>If suspended, the device will wake to fulfill this notification</remarks>
		/// <param name="eventName">String name of the event to set</param>
		/// <param name="timeFromNow">TimeSpan from Now for the event to be set</param>
		public static void SetNamedEventAtTime(string eventName, TimeSpan timeFromNow)
		{
			Notify.RunAppAtTime(string.Format(@"\\.\Notifications\NamedEvents\{0}", eventName), DateTime.Now.Add(timeFromNow));
		}

		#endregion


		#region Set User Notification
		/// <summary>
		/// Creates a new user notification.
		/// </summary>
		/// <param name="application">String that specifies the name of the application that owns this notification.</param>
		/// <param name="time">The time when the notification should occur.</param>
		/// <param name="notify">Notification object that describes the events that are to occur when the notification time is reached.</param>
		/// <returns>The handle to the notification indicates success.</returns>
		public static int SetUserNotification(string application, System.DateTime time, UserNotification notify)
		{
			return SetUserNotification(0, application, time, notify);
		}
		/// <summary>
		/// Edit an existing user notification.
		/// </summary>
		/// <param name="handle">Handle to the notification to overwrite.</param>
		/// <param name="application">String that specifies the name of the application that owns this notification.</param>
		/// <param name="time">The time when the notification should occur.</param>
		/// <param name="notify">Notification object that describes the events that are to occur when the notification time is reached.</param>
		/// <returns>The handle to the notification indicates success.</returns>
		public static int SetUserNotification(int handle, string application, System.DateTime time, UserNotification notify)
		{
			SYSTEMTIME st = SYSTEMTIME.FromDateTime(time);

			//call api function
			int outhandle = NativeMethods.CeSetUserNotification(handle, application, ref st, notify);

			//if invalid handle throw exception
			if (outhandle == 0)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Error setting UserNotification");
			}
			return outhandle;
		}
		/// <summary>
		/// This function creates a new user notification.
		/// </summary>
		/// <param name="trigger">A UserNotificationTrigger that defines what event activates a notification.</param>
		/// <param name="notification">A UserNotification that defines how the system should respond when a notification occurs.</param>
		/// <returns>Handle to the notification event if successful.</returns>
		public static int SetUserNotification(UserNotificationTrigger trigger, UserNotification notification)
		{
			int outhandle = NativeMethods.CeSetUserNotificationEx(0, trigger, notification);

			//throw on invalid handle
			if (outhandle == 0)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Error setting UserNotification");
			}
			return outhandle;
		}
		/// <summary>
		/// This function modifies an existing user notification.
		/// </summary>
		/// <param name="handle">Handle of the Notification to be modified</param>
		/// <param name="trigger">A UserNotificationTrigger that defines what event activates a notification.</param>
		/// <param name="notification">A UserNotification that defines how the system should respond when a notification occurs.</param>
		/// <returns>Handle to the notification event if successful.</returns>
		public static int SetUserNotification(int handle, UserNotificationTrigger trigger, UserNotification notification)
		{
			int outhandle = NativeMethods.CeSetUserNotificationEx(handle, trigger, notification);

			//throw on invalid handle
			if (outhandle == 0)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Error setting UserNotification");
			}
			return outhandle;
		}
		#endregion

		#region Clear User Notification
		/// <summary>
		/// Deletes a registered user notification that was created by a previous call to the SetUserNotification function.
		/// </summary>
		/// <param name="handle">Handle to the user notification to delete.</param>
		/// <returns>TRUE indicates success. FALSE indicates failure.</returns>
		/// <remarks>ClearNotification does not operate on notifications that have occurred.</remarks>
		public static bool ClearUserNotification(int handle)
		{
			return NativeMethods.CeClearUserNotification(handle);
		}
		#endregion

		#region Get User Notification
		/// <summary>
		/// Retrieves notification information associated with a handle.
		/// </summary>
		/// <param name="handle">Handle to the user notification to retrieve.</param>
		/// <returns>The requested <see cref="UserNotificationInfoHeader"/>.</returns>
		public static UserNotificationInfoHeader GetUserNotification(int handle)
		{
			//buffer size
			int size = 0;

			//first query for buffer size required
			NativeMethods.CeGetUserNotification(handle, 0, ref size, IntPtr.Zero);

			//create a marshallable buffer
			IntPtr buffer = Marshal.AllocHGlobal(size);

			//call native getter
			if (!NativeMethods.CeGetUserNotification(handle, (uint)size, ref size, buffer))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Error getting UserNotification");
			}

			//marshaller doesn't cope with marshalling the struct with nested bits using PtrToStructure, so do it manually
			UserNotificationInfoHeader nih = new UserNotificationInfoHeader();
			nih.hNotification = Marshal.ReadInt32(buffer, 0);
			nih.dwStatus = Marshal.ReadInt32(buffer, 4);
			IntPtr pTrigger = (IntPtr)Marshal.ReadInt32(buffer, 8);
			IntPtr pNotif = (IntPtr)Marshal.ReadInt32(buffer, 12);

			nih.pcent = new UserNotificationTrigger();
			// FIX: ArgumentNullException in GetUserNotifications (Bug #16)
			if (pTrigger != IntPtr.Zero)
			{
				nih.pcent.dwSize = Marshal.ReadInt32(pTrigger);
				nih.pcent.Type = (NotificationType)Marshal.ReadInt32(pTrigger, 4);
				nih.pcent.Event = (NotificationEvent)Marshal.ReadInt32(pTrigger, 8);
				nih.pcent.Application = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(pTrigger, 12));
				nih.pcent.Arguments = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(pTrigger, 16));
				nih.pcent.stStartTime =
					(SYSTEMTIME)Marshal.PtrToStructure((IntPtr)(pTrigger.ToInt32() + 20), typeof(SYSTEMTIME));
				nih.pcent.stEndTime =
					(SYSTEMTIME)Marshal.PtrToStructure((IntPtr)(pTrigger.ToInt32() + 36), typeof(SYSTEMTIME));
			}

			nih.pceun = new UserNotification();
			// FIX: ArgumentNullException in GetUserNotifications (Bug #16)
			if (pNotif != IntPtr.Zero)
			{
				nih.pceun.Action = (NotificationAction)Marshal.ReadInt32(pNotif, 0);
				nih.pceun.Title = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(pNotif, 4));
				nih.pceun.Text = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(pNotif, 8));
				nih.pceun.Sound = Marshal.PtrToStringUni((IntPtr)Marshal.ReadInt32(pNotif, 12));
				nih.pceun.MaxSound = Marshal.ReadInt32(pNotif, 16);
			}

			//free native memory
			Marshal.FreeHGlobal(buffer);

			return nih;
		}
		#endregion

		#region Get User Notification Handles
		/// <summary>
		/// Returns an array of currently stored notifications.
		/// </summary>
		/// <returns>Array of currently stored notifications.</returns>
		public static int[] GetUserNotificationHandles()
		{
			int size = 0;
			//get size required
			if (!NativeMethods.CeGetUserNotificationHandles(null, 0, ref size))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Error retrieving handles");
			}
			//create array to fill
			int[] handles = new int[size];
			//this time pass the buffer to be filled
			if (!NativeMethods.CeGetUserNotificationHandles(handles, size, ref size))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Error retrieving handles");
			}

			//return populated handles array
			return handles;
		}
		#endregion

		#region Get User Notification Preferences
		/// <summary>
		/// This function queries the user for notification settings by displaying a dialog box showing options that are valid for the current hardware platform.
		/// </summary>
		/// <param name="hWnd">Handle to the parent window for the notification settings dialog box.</param>
		/// <returns>A UserNotification structure containing the user's notification settings.</returns>
		public static UserNotification GetUserNotificationPreferences(IntPtr hWnd)
		{
			UserNotification template = new UserNotification();

			return GetUserNotificationPreferences(hWnd, template);

		}
		/// <summary>
		/// This function queries the user for notification settings by displaying a dialog box showing options that are valid for the current hardware platform.
		/// </summary>
		/// <param name="hWnd">Handle to the parent window for the notification settings dialog box.</param>
		/// <param name="template">UserNotification structure used to populate the default settings.</param>
		/// <returns>A UserNotification structure containing the user's notification settings.</returns>
		public static UserNotification GetUserNotificationPreferences(IntPtr hWnd, UserNotification template)
		{
			template.MaxSound = 260;
			template.Sound = new string('\0', 260);

			if (!NativeMethods.CeGetUserNotificationPreferences(hWnd, template))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Could not get user preferences");
			}
			return template;
		}
		#endregion

		#region Handle App Notifications
		/// <summary>
		/// This function marks as "handled" all notifications previously registered by the given application that have occurred.
		/// </summary>
		/// <param name="application">The name of the application whose events are to be marked as "handled".
		/// This must be the name that was passed in to <see cref="SetUserNotification(string, DateTime, UserNotification)"/> as the owner of the notification.</param>
		public static void HandleAppNotifications(string application)
		{
			//throw exception on failure
			if (!NativeMethods.CeHandleAppNotifications(application))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Error clearing Application Notifications");
			}
		}
		#endregion

	}
	#endregion

}
