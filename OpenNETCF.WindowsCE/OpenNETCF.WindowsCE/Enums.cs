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
    /// Defines for the current Stdio Stream
    /// </summary>
    public enum StdIoStream
    {
        /// <summary>
        /// The standard input stream (typically the console)
        /// </summary>
        Input = 0,
        /// <summary>
        /// The standard output stream (typically the console)
        /// </summary>
        Output = 1,
        /// <summary>
        /// The standard error output stream
        /// </summary>
        ErrorOutput = 2
    }

    [Flags]
    internal enum PowerEventType
    {
        PBT_TRANSITION = 0x00000001,
        PBT_RESUME = 0x00000002,
        PBT_POWERSTATUSCHANGE = 0x00000004,
        PBT_POWERINFOCHANGE = 0x00000008,
        PBT_SUSPENDKEYPRESSED = 0x00000100,
        PBT_OEMBASE = 0x00010000 //OEMS may define power notifications starting with this ID
    }
/*
    [Flags]
    internal enum PowerState
    {
        POWER_STATE_ON = (0x00010000),
        POWER_STATE_OFF = (0x00020000),

        POWER_STATE_CRITICAL = (0x00040000),
        POWER_STATE_BOOT = (0x00080000),
        POWER_STATE_IDLE = (0x00100000),
        POWER_STATE_SUSPEND = (0x00200000),
        POWER_STATE_RESET = (0x00800000),
    }
*/
    #region SystemParametersInfoAction Enumeration
    /// <summary>
    /// Specifies the system-wide parameter to query or set.
    /// </summary>
    internal enum SystemParametersInfoAction : int
    {
        /// <summary>
        /// Retrieves the two mouse threshold values and the mouse speed.
        /// </summary>
        GetMouse = 3,
        /// <summary>
        /// Sets the two mouse threshold values and the mouse speed.
        /// </summary>
        SetMouse = 4,

        /// <summary>
        /// For Windows CE 2.12 and later, sets the desktop wallpaper.
        /// </summary>
        SetDeskWallpaper = 20,
        /// <summary>
        /// 
        /// </summary>
        SetDeskPattern = 21,

        /// <summary>
        /// Sets the size of the work area � the portion of the screen not obscured by the system taskbar or by toolbars displayed on the desktop by applications.
        /// </summary>
        SetWorkArea = 47,
        /// <summary>
        /// Retrieves the size of the work area on the primary screen.
        /// </summary>
        GetWorkArea = 48,

        /// <summary>
        /// Retrieves whether the show sounds option is on or off.
        /// </summary>
        GetShowSounds = 56,
        /// <summary>
        /// Turns the show sounds accessibility option on or off.
        /// </summary>
        SetShowSounds = 57,

        /// <summary>
        /// Gets the number of lines to scroll when the mouse wheel is rotated.
        /// </summary>
        GetWheelScrollLines = 104,
        /// <summary>
        /// Sets the number of lines to scroll when the mouse wheel is rotated.
        /// </summary>
        SetWheelScrollLines = 105,

        /// <summary>
        /// Retrieves a contrast value that is used in smoothing text displayed using Microsoft� ClearType�.
        /// </summary>
        GetFontSmoothingContrast = 0x200C,
        /// <summary>
        /// Sets the contrast value used when displaying text in a ClearType font.
        /// </summary>
        SetFontSmoothingContrast = 0x200D,

        /// <summary>
        /// Retrieves the screen saver time-out value, in seconds.
        /// </summary>
        GetScreenSaveTimeout = 14,
        /// <summary>
        /// Sets the screen saver time-out value to the value of the uiParam parameter.
        /// </summary>
        SetScreenSaveTimeout = 15,

        /// <summary>
        /// Sets the amount of time that Windows CE will stay on with battery power before it suspends due to user inaction.
        /// </summary>
        SetBatteryIdleTimeout = 251,
        /// <summary>
        /// Retrieves the amount of time that Windows CE will stay on with battery power before it suspends due to user inaction.
        /// </summary>
        GetBatteryIdleTimeout = 252,

        /// <summary>
        /// Sets the amount of time that Windows CE will stay on with AC power before it suspends due to user inaction.
        /// </summary>
        SetExternalIdleTimeout = 253,
        /// <summary>
        /// Retrieves the amount of time that Windows CE will stay on with AC power before it suspends due to user inaction.
        /// </summary>
        GetExternalIdleTimeout = 254,

        /// <summary>
        /// Sets the amount of time that Windows CE will stay on after a user notification that reactivates the suspended device.
        /// </summary>
        SetWakeupIdleTimeout = 255,
        /// <summary>
        /// Retrieves the amount of time that Windows CE will stay on after a user notification that reactivates a suspended device.
        /// </summary>
        GetWakeupIdleTimeout = 256,

        // @CESYSGEN ENDIF

        //The following flags also used with WM_SETTINGCHANGE
        // so don't use the values for future SPI_*

        //#define SETTINGCHANGE_START  		0x3001
        //#define SETTINGCHANGE_RESET  		0x3002
        //#define SETTINGCHANGE_END    		0x3003

        /// <summary>
        /// Get the platform name e.g. PocketPC, Smartphone etc.
        /// </summary>
        GetPlatformType = 257,
        /// <summary>
        /// Get OEM specific information.
        /// </summary>
        GetOemInfo = 258,

    }
    #endregion
    #region ProcessorArchitecture
    /// <summary>
    /// Processor Architecture values (GetSystemInfo)
    /// </summary>
    /// <seealso cref="M:OpenNETCF.WinAPI.Core.GetSystemInfo(OpenNETCF.WinAPI.Core.SYSTEM_INFO)"/>
    public enum ProcessorArchitecture : short
    {
        /// <summary>
        /// Processor is Intel x86 based.
        /// </summary>
        Intel = 0,
        /// <summary>
        /// Processor is MIPS based.
        /// </summary>
        MIPS = 1,
        /// <summary>
        /// Processor is Alpha based.
        /// </summary>
        Alpha = 2,
        /// <summary>
        /// Processor is Power PC based.
        /// </summary>
        PPC = 3,
        /// <summary>
        /// Processor is SH3, SH4 etc.
        /// </summary>
        SHX = 4,
        /// <summary>
        /// Processor is ARM based.
        /// </summary>
        ARM = 5,
        /// <summary>
        /// Processor is Intel 64bit.
        /// </summary>
        IA64 = 6,
        /// <summary>
        /// Processor is Alpha 64bit.
        /// </summary>
        Alpha64 = 7,
        /// <summary>
        /// Unknown processor architecture.
        /// </summary>
        Unknown = -1,
    }
    #endregion

    #region Processor Type
    /// <summary>
    /// Processor type values (GetSystemInfo)
    /// </summary>
    /// <seealso cref="M:OpenNETCF.Win32.Core.GetSystemInfo(OpenNETCF.Win32.Core.SYSTEM_INFO)"/>
    public enum ProcessorType : int
    {
        /// <summary>
        /// Processor is Intel 80386.
        /// </summary>
        Intel_386 = 386,
        /// <summary>
        /// Processor is Intel 80486.
        /// </summary>
        Intel_486 = 486,
        /// <summary>
        /// Processor is Intel Pentium (80586).
        /// </summary>
        Intel_Pentium = 586,
        /// <summary>
        /// Processor is Intel Pentium II (80686).
        /// </summary>
        Intel_PentiumII = 686,
        /// <summary>
        /// Processor is Intel 64bit (IA64).
        /// </summary>
        Intel_IA64 = 2200,
        /// <summary>
        /// Processor is MIPS R4000.
        /// </summary>
        MIPS_R4000 = 4000,
        /// <summary>
        /// Processor is Alpha 21064.
        /// </summary>
        Alpha_21064 = 21064,
        /// <summary>
        /// Processor is Power PC 403.
        /// </summary>
        PPC_403 = 403,
        /// <summary>
        /// Processor is Power PC 601.
        /// </summary>
        PPC_601 = 601,
        /// <summary>
        /// Processor is Power PC 603.
        /// </summary>
        PPC_603 = 603,
        /// <summary>
        /// Processor is Power PC 604.
        /// </summary>
        PPC_604 = 604,
        /// <summary>
        /// Processor is Power PC 620.
        /// </summary>
        PPC_620 = 620,
        /// <summary>
        /// Processor is Hitachi SH3.
        /// </summary>
        Hitachi_SH3 = 10003,
        /// <summary>
        /// Processor is Hitachi SH3E.
        /// </summary>
        Hitachi_SH3E = 10004,
        /// <summary>
        /// Processor is Hitachi SH4.
        /// </summary>
        Hitachi_SH4 = 10005,
        /// <summary>
        /// Processor is Motorola 821.
        /// </summary>
        Motorola_821 = 821,
        /// <summary>
        /// Processor is SH3.
        /// </summary>
        SHx_SH3 = 103,
        /// <summary>
        /// Processor is SH4.
        /// </summary>
        SHx_SH4 = 104,
        /// <summary>
        /// Processor is StrongARM.
        /// </summary>
        StrongARM = 2577,
        /// <summary>
        /// Processor is ARM 720.
        /// </summary>
        ARM720 = 1824,
        /// <summary>
        /// Processor is ARM 820.
        /// </summary>
        ARM820 = 2080,
        /// <summary>
        /// Processor is ARM 920.
        /// </summary>
        ARM920 = 2336,
        /// <summary>
        /// Processor is ARM 7 TDMI.
        /// </summary>
        ARM_7TDMI = 70001
    }
    #endregion

    #region SystemParametersInfoFlags Enumeration
    /// <summary>
    /// Specifies whether the user profile is to be updated, and if so, whether the WM_SETTINGCHANGE message is to be broadcast to all top-level windows to notify them of the change.
    /// </summary>
    internal enum SystemParametersInfoFlags : int
    {
        /// <summary>
        /// No notifications are sent on settings changed.
        /// </summary>
        None = 0,
        /// <summary>
        /// Writes the new system-wide parameter setting to the user profile.
        /// </summary>
        UpdateIniFile = 0x0001,
        /// <summary>
        /// Broadcasts the WM_SETTINGCHANGE message after updating the user profile.
        /// </summary>
        SendChange = 0x0002,
    }
    #endregion

    [Flags]
    internal enum PowerStateFlags
    {
        On = (0x00010000),        // on state
        Off = (0x00020000),        // no power, full off
        CriticalOff = (0x00040000),        // critical off
        Boot = (0x00080000),        // boot state
        Idle = (0x00100000),        // idle state
        Suspend = (0x00200000),        // suspend state
        Reset = (0x00800000),        // reset state
        PasswordProtected = (0x10000000)         // This state is password protected.
    }

    /// <summary>
    /// Device Power State
    /// </summary>
    public enum DevicePowerState
    {
        /// <summary>
        /// The device power state is not specified
        /// </summary>
        Unsepecified = -1,
        /// <summary>
        /// Set to full on (D0)
        /// </summary>
        FullOn = 0, // D0
        /// <summary>
        /// Set to low power mode (D1)
        /// </summary>
        LowPower = 1, // D1,
        /// <summary>
        /// Set to standby mode (D2)
        /// </summary>
        Standby = 2, // D2
        /// <summary>
        /// Set to Sleep mode (D3)
        /// </summary>
        Sleep = 3, // D3,
        /// <summary>
        /// Power off (D4)
        /// </summary>
        Off = 4, // D4
    }

    /// <summary>
    /// Available notification events
    /// </summary>
    public enum NotificationEvent : int
    {
        /// <summary>   
        /// No events�remove all event registrations for this application.   
        /// </summary>   
        None = 0x00,
        /// <summary>   
        /// When the system time is changed.   
        /// </summary>   
        TimeChange = 0x01,
        /// <summary>   
        /// When data synchronization finishes.   
        /// </summary>   
        SyncEnd = 0x02,
        /// <summary>
        /// The user turned the AC power on.
        /// </summary>
        OnACPower = 0x03,
        /// <summary>
        /// The user turned the alternating current (AC) power off.
        /// </summary>
        OffACPower = 0x04,
        /// <summary>
        /// The device connected to a network.
        /// </summary>
        NetConnect = 0x05,
        /// <summary>
        /// The device disconnected from a network.
        /// </summary>
        NetDisconnect = 0x06,
        /// <summary>   
        /// When a PC Card device is changed.   
        /// </summary>   
        DeviceChange = 0x07,
        /// <summary>
        /// The device discovered a server by using infrared communications.
        /// </summary>
        IRDiscovered = 0x08,
        /// <summary>   
        /// When an RS232 connection is made.   
        /// </summary>   
        RS232Detected = 0x09,
        /// <summary>   
        /// When a full device data restore completes.   
        /// </summary>   
        RestoreEnd = 0x0A,
        /// <summary>   
        /// When the device wakes up.   
        /// </summary>   
        Wakeup = 0x0B,
        /// <summary>   
        /// When the time zone is changed.   
        /// </summary>   
        TimeZoneChange = 0x0C,
        /// <summary>
        /// When the machines name changes.
        /// Requires Windows CE.NET 4.2.
        /// </summary>
        MachineNameChange = 0x0D,
        /// <summary>
        /// RNDISFN interface is instantiated.
        /// Requires Windows CE 5.0.
        /// </summary>
        RndisFNDetected = 0x0E,
        /// <summary>
        /// The Internet Proxy used by the device has changed.
        /// Requires Windows CE 5.0.
        /// </summary>
        InternetProxyChange = 0x0f,
    }

    /// <summary>
    /// Return values from <see cref="M:OpenNETCF.Win32.DateTime2.GetTimeZoneInformation"/>.
    /// </summary>
    public enum TimeZoneState : int
    {
        /// <summary>
        /// The system cannot determine the current time zone.
        /// This value is returned if daylight savings time is not used in the current time zone, because there are no transition dates.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The system is operating in the range covered by the StandardDate member of the <see cref="OpenNETCF.WindowsCE.TimeZoneInformation"/> structure.
        /// </summary>
        Standard = 1,
        /// <summary>
        /// The system is operating in the range covered by the DaylightDate member of the <see cref="OpenNETCF.WindowsCE.TimeZoneInformation"/> structure.
        /// </summary>
        Daylight = 2
    }

}
