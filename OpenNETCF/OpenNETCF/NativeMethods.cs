using System;
using System.Runtime.InteropServices;
using OpenNETCF.Diagnostics;
using OpenNETCF.Win32;

namespace OpenNETCF
{
    internal static class NativeMethods
    {
        internal const int INFINITE = -1;
        internal const int INVALID_HANDLE_VALUE = -1;


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

        #region SystemInfo
        /// <summary>
        /// This structure contains information about the current computer system. This includes the processor type, page size, memory addresses, and OEM identifier.
        /// </summary>
        /// <seealso cref="GetSystemInfo"/>
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
        #endregion

        //hKeys for the root keys
        internal enum RootKeys : uint
        {
            ClassesRoot = 0x80000000,
            CurrentUser = 0x80000001,
            LocalMachine = 0x80000002,
            Users = 0x80000003
        }

        [Flags]
        internal enum MMTimerEventType
        {
            OneShot = 0x0000,
            Periodic = 0x0001,
            Callback = 0x0000,
            EventSet = 0x0010,
            EventPulse = 0x0020,
            KillSynchronous = 0x0100
        }

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

        #region KeyDisposition Enumeration
        // <summary>
        // Key disposition for RegCreateKey(Ex)
        // </summary>
        internal enum KeyDisposition : int
        {
            CreatedNewKey = 1,
            OpenedExistingKey = 2
        }
        #endregion

        #region Format Message Flags Enumeration
        /// <summary>
        /// Specifies aspects of the formatting process and how to interpret the lpSource parameter.
        /// </summary>
        /// <remarks>The low-order byte of dwFlags specifies how the function handles line breaks in the output buffer.
        /// The low-order byte can also specify the maximum width of a formatted output line.</remarks>
        [Flags]
        public enum FormatMessageFlags : int
        {
            /// <summary>
            /// The function allocates a buffer large enough to hold the formatted message, and places a pointer to the allocated buffer at the address specified by lpBuffer.
            /// </summary>
            AllocateBuffer = 0x00000100,
            /// <summary>
            /// Insert sequences in the message definition are to be ignored and passed through to the output buffer unchanged.
            /// </summary>
            IgnoreInserts = 0x00000200,
            /// <summary>
            /// Specifies that lpSource is a pointer to a null-terminated message definition.
            /// </summary>
            FromString = 0x00000400,
            /// <summary>
            /// Specifies that lpSource is a module handle containing the message-table resource(s) to search.
            /// </summary>
            FromHModule = 0x00000800,
            /// <summary>
            /// Specifies that the function should search the system message-table resource(s) for the requested message.
            /// </summary>
            FromSystem = 0x00001000,
            /// <summary>
            /// Specifies that the Arguments parameter is not a va_list structure, but instead is just a pointer to an array of 32-bit values that represent the arguments.
            /// </summary>
            ArgumentArray = 0x00002000,
            /// <summary>
            /// Use the <b>MaxWidthMask</b> constant and bitwise Boolean operations to set and retrieve this maximum width value.
            /// </summary>
            MaxWidthMask = 0x000000FF,
        }
        #endregion

        #region Key State Flags
        /// <summary>
        /// KeyStateFlags for Keyboard methods
        /// </summary>
        [Flags()]
        internal enum KeyStateFlags : int
        {
            /// <summary>
            /// Key is toggled.
            /// </summary>
            Toggled = 0x0001,
            /// <summary>
            /// 
            /// </summary>
            AsyncDown = 0x0002,		//	 went down since last GetAsync call.
            /// <summary>
            /// Key was previously down.
            /// </summary>
            PrevDown = 0x0040,
            /// <summary>
            /// Key is currently down.
            /// </summary>
            Down = 0x0080,
            /// <summary>
            /// Left or right CTRL key is down.
            /// </summary>
            AnyCtrl = 0x40000000,
            /// <summary>
            /// Left or right SHIFT key is down.
            /// </summary>
            AnyShift = 0x20000000,
            /// <summary>
            /// Left or right ALT key is down.
            /// </summary>
            AnyAlt = 0x10000000,
            /// <summary>
            /// VK_CAPITAL is toggled.
            /// </summary>
            Capital = 0x08000000,
            /// <summary>
            /// Left CTRL key is down.
            /// </summary>
            LeftCtrl = 0x04000000,
            /// <summary>
            /// Left SHIFT key is down.
            /// </summary>
            LeftShift = 0x02000000,
            /// <summary>
            /// Left ALT key is down.
            /// </summary>
            LeftAlt = 0x01000000,
            /// <summary>
            /// Left Windows logo key is down.
            /// </summary>
            LeftWin = 0x00800000,
            /// <summary>
            /// Right CTRL key is down.
            /// </summary>
            RightCtrl = 0x00400000,
            /// <summary>
            /// Right SHIFT key is down
            /// </summary>
            RightShift = 0x00200000,
            /// <summary>
            /// Right ALT key is down
            /// </summary>
            RightAlt = 0x00100000,
            /// <summary>
            /// Right Windows logo key is down.
            /// </summary>
            RightWin = 0x00080000,
            /// <summary>
            /// Corresponding character is dead character.
            /// </summary>
            Dead = 0x00020000,
            /// <summary>
            /// No characters in pCharacterBuffer to translate.
            /// </summary>
            NoCharacter = 0x00010000,
            /// <summary>
            /// Use for language specific shifts.
            /// </summary>
            Language1 = 0x00008000,
            /// <summary>
            /// NumLock toggled state.
            /// </summary>
            NumLock = 0x00001000,
        }
        #endregion

        #region Keyboard P/Invokes
        [DllImport("coredll.dll", EntryPoint = "keybd_event", SetLastError = true)]
        internal static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("coredll.dll", EntryPoint = "PostKeybdMessage", SetLastError = true)]
        internal static extern bool PostKeybdMessage(IntPtr hwnd, uint vKey, KeyStateFlags flags, uint cCharacters, KeyStateFlags[] pShiftStateBuffer, uint[] pCharacterBuffer);

        #endregion

        #region Registry P/Invokes
        // RAM based
        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern bool RegCopyFile(string lpszFile);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern bool RegRestoreFile(string lpszFile);

        // Hive based
        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern int RegSaveKey(uint hKey, string lpFile, IntPtr lpSecurityAttributes);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern bool RegReplaceKey(uint hKey, IntPtr lpSubKey, string lpNewFile, IntPtr lpOldFile);

        [DllImport("coredll.dll", EntryPoint = "RegCreateKeyEx", SetLastError = true)]
        internal static extern int RegCreateKeyEx(
            uint hKey,
            string lpSubKey,
            int lpReserved,
            string lpClass,
            int dwOptions,
            int samDesired,
            IntPtr lpSecurityAttributes,
            ref uint phkResult,
            ref KeyDisposition lpdwDisposition);

        [DllImport("coredll.dll", EntryPoint = "RegCloseKey", SetLastError = true)]
        internal static extern int RegCloseKey(
            uint hKey);
        #endregion

        // p/invoke declarations

        [DllImport("coredll.dll", EntryPoint = "GetSystemInfo", SetLastError = true)]
        internal static extern void GetSystemInfo(out SystemInfo pSI);

        [DllImport("mmtimer.dll", EntryPoint = "timeSetEvent", SetLastError = true)]
        internal static extern int timeSetEvent(int uDelay, int uResolution, IntPtr fptc, int dwUser, MMTimerEventType fuEvent);

        [DllImport("mmtimer.dll", EntryPoint = "timeKillEvent", SetLastError = true)]
        internal static extern int timeKillEvent(int uTimerID);

        [DllImport("coredll.dll", EntryPoint = "GlobalMemoryStatus", SetLastError = false)]
        public static extern void GlobalMemoryStatus(out MemoryStatus msce);

        #region Environment P/Invokes

        [DllImport("coredll.dll", EntryPoint = "SHGetSpecialFolderPath", SetLastError = false)]
        internal static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner, System.Text.StringBuilder lpszPath, int nFolder, int fCreate);

        #endregion

        #region Process P/Invokes

        [DllImport("coredll.dll", EntryPoint = "CloseHandle", SetLastError = true)]
        public static extern int CloseHandle(IntPtr hObject);
        
        [DllImport("coredll.dll", EntryPoint = "OpenProcess", SetLastError = true)]
        public extern static IntPtr OpenProcess(uint fdwAccess, bool fInherit, uint IDProcess);

        [DllImport("coredll.dll", EntryPoint = "OpenProcess", SetLastError = true)]
        public extern static IntPtr OpenProcess(uint fdwAccess, bool fInherit, int IDProcess);
        
        [DllImport("coredll.dll", EntryPoint = "TerminateProcess", SetLastError = true)]
        public extern static bool TerminateProcess(IntPtr hProcess, int uExitCode);

        #endregion

        /// <summary>
        /// This function returns the address of the specified exported DLL function.
        /// </summary>
        /// <param name="hModule">Handle to the DLL module that contains the function.
        /// The <see cref="LoadLibrary"/> or <see cref="GetModuleHandle"/> function returns this handle.</param>
        /// <param name="procName">string containing the function name, or specifies the function's ordinal value.
        /// If this parameter is an ordinal value, it must be in the low-order word; the high-order word must be zero.</param>
        /// <returns></returns>
        [DllImport("coredll.dll", EntryPoint = "GetProcAddressW", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        /// <summary>
        /// This function returns a module handle for the specified module if the file is mapped into the address space of the calling process.
        /// </summary>
        /// <param name="moduleName">string that contains the name of the module, which must be a DLL file.</param>
        /// <returns>A handle to the specified module indicates success. IntPtr.Zero indicates failure.</returns>
        [DllImport("coredll.dll", EntryPoint = "GetModuleHandleW", SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string moduleName);

        /// <summary>
        /// This function determines whether the calling process has read access to the memory at the specified address.
        /// </summary>
        /// <param name="fn">Pointer to an address in memory.</param>
        /// <returns>Zero indicates that the calling process has read access to the specified memory.
        /// Nonzero indicates that the calling process does not have read access to the specified memory.</returns>
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool IsBadCodePtr(IntPtr fn);

        [DllImport("coredll.dll", EntryPoint = "LoadLibraryW", SetLastError = true)]
        internal static extern IntPtr LoadLibrary(string lpszLib);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern int FreeLibrary(IntPtr hModule);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern int QueryPerformanceFrequency(ref Int64 lpFrequency);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern int QueryPerformanceCounter(ref Int64 lpPerformanceCount);

        [DllImport("coredll.dll", EntryPoint = "FormatMessageW", SetLastError = false)]
        internal static extern int FormatMessage(FormatMessageFlags dwFlags, int lpSource, int dwMessageId, int dwLanguageId, out IntPtr lpBuffer, int nSize, int[] Arguments);

        [DllImport("coredll.dll", EntryPoint = "FileTimeToSystemTime", SetLastError = true)]
        internal static extern int FileTimeToSystemTime(
            ref long lpFileTime,
            byte[] lpSystemTime);

        [DllImport("coredll.dll", EntryPoint = "SystemTimeToFileTime", SetLastError = true)]
        internal static extern int SystemTimeToFileTime(
            byte[] lpSystemTime,
            ref long lpFileTime);


        public static bool NativeLibraryExists(string libraryName)
        {
            IntPtr hLib = LoadLibrary(libraryName);
            if (hLib != IntPtr.Zero)
            {
                FreeLibrary(hLib);
                return true;
            }

            return false;
        }

        public static bool NativeEntryPointExists(string libraryName, string functionName)
        {
            IntPtr hLib = LoadLibrary(libraryName);
            if (hLib != IntPtr.Zero)
            {
                try
                {
                    IntPtr entry = GetProcAddress(hLib, functionName);

                    return entry != IntPtr.Zero;
                }
                finally
                {
                    FreeLibrary(hLib);
                }
            }

            return false;
        }
    }
}
