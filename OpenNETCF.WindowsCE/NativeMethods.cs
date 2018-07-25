using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using OpenNETCF.Win32;

namespace OpenNETCF.WindowsCE
{
    internal static class NativeMethods
    {
        public const Int32 ERROR_NOT_SUPPORTED = 0x32;
        public const Int32 ERROR_INSUFFICIENT_BUFFER = 0x7A;
        internal const Int32 METHOD_BUFFERED = 0;
        internal const Int32 FILE_ANY_ACCESS = 0;
        internal const Int32 FILE_DEVICE_HAL = 0x00000101;
        internal const Int32 IOCTL_HAL_REBOOT = 0x101003C;

        internal static Int32 IOCTL_HAL_GET_DEVICEID = ((FILE_DEVICE_HAL) << 16) |
            ((FILE_ANY_ACCESS) << 14) | ((21) << 2) | (METHOD_BUFFERED);

        #region SystemParametersInfoAction Enumeration
        /// <summary>
        /// Specifies the system-wide parameter to query or set.
        /// </summary>
        public enum SystemParametersInfoAction : int
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
            /// Sets the size of the work area — the portion of the screen not obscured by the system taskbar or by toolbars displayed on the desktop by applications.
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
            /// Retrieves a contrast value that is used in smoothing text displayed using Microsoft® ClearType®.
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

            //SETTINGCHANGE_START  		0x3001
            //SETTINGCHANGE_RESET  		0x3002
            //SETTINGCHANGE_END    		0x3003

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

        #region SystemParametersInfoFlags Enumeration
        /// <summary>
        /// Specifies whether the user profile is to be updated, and if so, whether the WM_SETTINGCHANGE message is to be broadcast to all top-level windows to notify them of the change.
        /// </summary>
        public enum SystemParametersInfoFlags : int
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

        #region System Power Status



        /// <summary>
        /// The remaining battery power is unknown.
        /// </summary>
        public const byte BatteryPercentageUnknown = 0xFF;

        internal const uint BatteryLifeUnknown = 0xFFFFFFFF;



        #region Memory Status
        /// <summary>
        /// This structure contains information about current memory availability. The GlobalMemoryStatus function uses this structure.
        /// </summary>
        public struct MemoryStatus
        {
            internal uint dwLength;
            /// <summary>
            /// Specifies a number between 0 and 100 that gives a general idea of current memory utilization, in which 0 indicates no memory use and 100 indicates full memory use.
            /// </summary>
            public int MemoryLoad;
            /// <summary>
            /// Indicates the total number of bytes of physical memory.
            /// </summary>
            public int TotalPhysical;
            /// <summary>
            /// Indicates the number of bytes of physical memory available.
            /// </summary>
            public int AvailablePhysical;
            /// <summary>
            /// Indicates the total number of bytes that can be stored in the paging file. Note that this number does not represent the actual physical size of the paging file on disk.
            /// </summary>
            public int TotalPageFile;
            /// <summary>
            /// Indicates the number of bytes available in the paging file.
            /// </summary>
            public int AvailablePageFile;
            /// <summary>
            /// Indicates the total number of bytes that can be described in the user mode portion of the virtual address space of the calling process.
            /// </summary>
            public int TotalVirtual;
            /// <summary>
            /// Indicates the number of bytes of unreserved and uncommitted memory in the user mode portion of the virtual address space of the calling process.
            /// </summary>
            public int AvailableVirtual;
        }
        #endregion

        [DllImport("coredll.dll", EntryPoint = "GlobalMemoryStatus", SetLastError = false)]
        internal static extern void GlobalMemoryStatus(out MemoryStatus msce);

        [DllImport("coredll.dll", EntryPoint = "GetSystemInfo", SetLastError = true)]
        internal static extern void GetSystemInfo(out SystemInfo pSI);

        [DllImport("coredll.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
        internal static extern bool SystemParametersInfo(SystemParametersInfoAction action, int size, byte[] buffer, SystemParametersInfoFlags winini);

        #endregion

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern void SetCleanRebootFlag();

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern void GwesPowerOff();
        
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool KernelIoControl(int dwIoControlCode, byte[] inBuf, int inBufSize, byte[] outBuf, int outBufSize, ref int bytesReturned);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool KernelIoControl(int dwIoControlCode, IntPtr inBuf, int inBufSize, IntPtr outBuf, int outBufSize, ref int bytesReturned);

        internal const int POWER_FORCE = (0x00001000);
        internal const int ERROR_SUCCESS = 0;

        // allows a null psState
        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern int SetSystemPowerState(IntPtr psState, PowerStateFlags flags, uint Options);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern int SetSystemPowerState(string psState, PowerStateFlags flags, uint Options);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern int GetSystemPowerState(string pBuffer, uint dwBufChars, ref uint pdwFlags);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern void SystemIdleTimerReset();

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern void TouchCalibrate();

        [DllImport("coredll.dll", EntryPoint = "CeRunAppAtEvent", SetLastError = true)]
        internal static extern bool CeRunAppAtEvent(string pwszAppName, NotificationEvent lWhichEvent);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern IntPtr RequestPowerNotifications(IntPtr hMsgQ, PowerEventType Flags);

		[DllImport("coredll.dll", SetLastError = true)]
		public static extern bool StopPowerNotifications(IntPtr hNotifHandle);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int GetStdioPath(StdIoStream id, StringBuilder pwszBuf, int lpdwLength);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern int SetStdioPath(StdIoStream id, string pwszPath);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool GetSystemMemoryDivision(ref int lpdwStorePages, ref int lpdwRamPages, ref int lpdwPageSize);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool SetSystemMemoryDivision(int dwStorePages); 

        public struct MsgQueueInfo
        {
            public Int32 dwSize;
            public Int32 dwFlags;
            public Int32 dwMaxMessages;
            public Int32 cbMaxMessage;
            public Int32 dwCurrentMessages;
            public Int32 dwMaxQueueMessages;
            public Int16 wNumReaders;
            public Int16 wNumWriters;
        }

        [DllImport("coredll.dll", EntryPoint = "GetTimeZoneInformation", SetLastError = true)]
        internal static extern TimeZoneState GetTimeZoneInformation(byte[] tzice);

        [DllImport("coredll.dll", EntryPoint = "SetTimeZoneInformation", SetLastError = true)]
        internal static extern bool SetTimeZoneInformation(byte[] tzice);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern bool SetLocalTime(byte[] st);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern bool SetSystemTime(byte[] st);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern bool GetLocalTime(byte[] st);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern bool GetSystemTime(byte[] st);

        [DllImport("citydb.dll", SetLastError = true)]
        internal extern static void InitCityDb();

        [DllImport("citydb.dll", SetLastError = true)]
        internal extern static void UninitCityDb();

        [DllImport("citydb.dll", SetLastError = true)]
        internal extern static int ClockGetNumTimezones();

        [DllImport("citydb.dll", SetLastError = true)]
        internal extern static void ClockLoadAllTimeZoneData();

        [DllImport("citydb.dll", SetLastError = true)]
        internal extern static void ClockFreeAllTimeZoneData();

        [DllImport("citydb.dll", SetLastError = true)]
        internal extern static IntPtr ClockGetTimeZoneDataByOffset(int nOffset, out int tzIndex);

        [DllImport("citydb.dll", SetLastError = true)]
        internal extern static IntPtr ClockGetTimeZoneData(int nOffset);

        /*
        HANDLE
        SetPowerRequirement(
            PVOID                   pvDevice,
            CEDEVICE_POWER_STATE    DeviceState,	
            ULONG                   DeviceFlags,
            PVOID                   pvSystemState,
            ULONG                   StateFlags    
            );

        DWORD
        ReleasePowerRequirement(
            HANDLE hPowerReq
            );

        HANDLE
        RequestPowerNotifications(
            HANDLE  hMsgQ,
            DWORD   Flags
            );

        DWORD
        StopPowerNotifications(
            HANDLE h
            );

        DWORD
        DevicePowerNotify(
            PVOID                   pvDevice,
            CEDEVICE_POWER_STATE    DeviceState,
            DWORD                   Flags
            );

        DWORD
        SetDevicePower(
            PVOID pvDevice, 
            DWORD dwDeviceFlags, 
            CEDEVICE_POWER_STATE dwState
            );

        DWORD
        GetDevicePower(
            PVOID pvDevice, 
            DWORD dwDeviceFlags,
            PCEDEVICE_POWER_STATE pdwState    
            );
        HANDLE
        RegisterPowerRelationship(
            PVOID pvParent, 
            PVOID pvChild,
            PPOWER_CAPABILITIES pCaps,
            DWORD Flags
            );

        DWORD
        ReleasePowerRelationship(
            HANDLE hChild
            );

        /*
        //
        // Power Requirement Flags
        //
        POWER_NAME              (DWORD)(0x00000001) // default

        //
        // POWER IOCTLS
        //
        // We are NOT APCI, we just borrow this unused code from winioctl.h
        FILE_DEVICE_POWER   FILE_DEVICE_ACPI    

        /*
        Required
        InBuf:  PPOWER_RELATIONSHIP - defines the target device for parent/bus drivers, else NULL
        OutBuf: PPOWER_CAPABILITIES - defines the devices power caps

        If a driver fails this ioctl the PM assumes the target driver does not support power ioctls.

        IOCTL_POWER_CAPABILITIES    \
            CTL_CODE(FILE_DEVICE_POWER, 0x400, METHOD_BUFFERED, FILE_ANY_ACCESS)

        /* ++
        Required
        InBuf:  PPOWER_RELATIONSHIP   - defines the target device for parent/bus drivers, else NULL
        OutBuf: PCEDEVICE_POWER_STATE - returns the device's current state (Dx).

        PM will only send this ioctl to drivers that support the power ioctls.
        -- 
        IOCTL_POWER_GET             \
            CTL_CODE(FILE_DEVICE_POWER, 0x401, METHOD_BUFFERED, FILE_ANY_ACCESS)

        /* ++
        Required
        InBuf:  PPOWER_RELATIONSHIP   - defines the target device for parent/bus drivers, else NULL
        OutBuf: PCEDEVICE_POWER_STATE - device state (Dx) in which to put the device.

        If the driver does not support the proposed Dx then it should write it's adjusted Dx
        into the OutBuf (Dx).

        PM will only send this ioctl to drivers that support the power ioctls.
        -- 
        IOCTL_POWER_SET             \
            CTL_CODE(FILE_DEVICE_POWER, 0x402, METHOD_BUFFERED, FILE_ANY_ACCESS)

        /*++
        Required
        InBuf:  PPOWER_RELATIONSHIP   - defines the target device for parent/bus drivers, else NULL
        OutBuf: PCEDEVICE_POWER_STATE - device state (Dx) that the system is querying for a 
                                        pending IOCTL_POWER_SET operation.
                                
        To veto the query the driver should write PwrDeviceUnspecified (-1)
        into the OutBuf (Dx), else PM assumes the driver accepted.

        PM will only send this ioctl to drivers that support the power ioctls.
        -- 
        IOCTL_POWER_QUERY           \
            CTL_CODE(FILE_DEVICE_POWER, 0x403, METHOD_BUFFERED, FILE_ANY_ACCESS)

        /*
        Required
        InBuf:  NULL 
        OutBuf: NULL

        PM does not care the return value from this IOCTL.   It's there to let the Parent device
        to register all devices it controls.

        IOCTL_REGISTER_POWER_RELATIONSHIP    \
            CTL_CODE(FILE_DEVICE_POWER, 0x406, METHOD_BUFFERED, FILE_ANY_ACCESS)


        //*****************************************************************************
        // T Y P E D E F S
        //*****************************************************************************

        //
        // Device Power States
        //

        DX_MASK(Dx)  (0x00000001 << Dx)

        VALID_DX(dx)  ( dx > PwrDeviceUnspecified && dx < PwrDeviceMaximum)

        //
        // Device or OAL Power Capabilities
        //
        typedef struct _POWER_CAPABILITIES {
            UCHAR DeviceDx;
            UCHAR WakeFromDx;
            UCHAR InrushDx;
            DWORD Power[PwrDeviceMaximum];
            DWORD Latency[PwrDeviceMaximum];
            DWORD Flags;
        } POWER_CAPABILITIES, *PPOWER_CAPABILITIES;

        POWER_CAP_PARENT        0x00000001      // parent/bus driver

        //
        // Defines the target of IOCTL_POWER_Xxx commands to parent/bus drivers
        // if there is a relationship established via RegisterPowerRelationship.
        //
        typedef struct _POWER_RELATIONSHIP {
            HANDLE  hParent;    // Handle to parent node
            LPCWSTR pwsParent;  // Named parent node, e.g. "NDS0:"
            HANDLE  hChild;     // Handle to child node, returned from RegisterPowerRelationship
            LPCWSTR pwsChild;   // Named child node, e.g. "NE20001"

        } POWER_RELATIONSHIP, *PPOWER_RELATIONSHIP;



        // This bitmask indicates that an application would like to receive all
        // types of power notifications.
        POWER_NOTIFY_ALL        0xFFFFFFFF


        // This structure is used instead of a string name (SystemPowerState) in
        // the POWER_BROADCAST, if the broadcast is of type PBT_POWERINFOCHANGE.
        //
        // For example:
        //  PPOWER_BROADCAST ppb;
        //  PPOWER_BROADCAST_POWER_INFO ppbpi = 
        //        (PPOWER_BROADCAST_POWER_INFO) ppb->SystemPowerState;
        //
        typedef struct _POWER_BROADCAST_POWER_INFO {
            // levels available in battery flag fields, see BatteryDrvrGetLevels()
            DWORD       dwNumLevels;

            // see GetSystemPowerStatusEx2()
            DWORD       dwBatteryLifeTime;
            DWORD       dwBatteryFullLifeTime;
            DWORD       dwBackupBatteryLifeTime;
            DWORD       dwBackupBatteryFullLifeTime;
            BYTE        bACLineStatus;
            BYTE        bBatteryFlag;
            BYTE        bBatteryLifePercent;
            BYTE        bBackupBatteryFlag;
            BYTE        bBackupBatteryLifePercent;
        } POWER_BROADCAST_POWER_INFO, *PPOWER_BROADCAST_POWER_INFO;
    

        
*/

       
    }

    [Flags]
    internal enum QS
    {
        KEY = 0x0001,
        MOUSEMOVE = 0x0002,
        MOUSEBUTTON = 0x0004,
        POSTMESSAGE = 0x0008,
        TIMER = 0x0010,
        PAINT = 0x0020,
        SENDMESSAGE = 0x0040,
        MOUSE = (MOUSEMOVE | MOUSEBUTTON),
        INPUT = (MOUSE | KEY),
        ALLEVENTS = (INPUT | POSTMESSAGE | TIMER | PAINT),
        ALLINPUT = (INPUT | POSTMESSAGE | TIMER | PAINT | SENDMESSAGE),
    }
}
