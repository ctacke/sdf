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

namespace OpenNETCF.ToolHelp
{
    internal static class ToolhelpAPI
    {
        public const int TH32CS_SNAPHEAPLIST = 0x00000001;
        public const int TH32CS_SNAPPROCESS = 0x00000002;
        public const int TH32CS_SNAPTHREAD = 0x00000004;
        public const int TH32CS_SNAPMODULE = 0x00000008;
        internal const int TH32CS_SNAPNOHEAPS = 0x40000000;

        private static IToolhelpAPI m_api;

        static ToolhelpAPI()
        {
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                m_api = new ToolhelpAPICE();
            }
            else
            {
                m_api = new ToolhelpAPIWin32();
            }
        }

        public static IntPtr CreateToolhelp32Snapshot(uint flags, uint processid)
        {
            return m_api.CreateToolhelp32Snapshot(flags, processid);
        }

        public static int CloseToolhelp32Snapshot(IntPtr handle)
        {
            return m_api.CloseToolhelp32Snapshot(handle);
        }

        public static int Process32First(IntPtr handle, byte[] pe)
        {
            return m_api.Process32First(handle, pe);
        }

        public static int Process32Next(IntPtr handle, byte[] pe)
        {
            return m_api.Process32Next(handle, pe);
        }

        public static int Thread32First(IntPtr handle, byte[] te)
        {
            return m_api.Thread32First(handle, te);
        }

        public static int Thread32Next(IntPtr handle, byte[] te)
        {
            return m_api.Thread32Next(handle, te);
        }

        public static int Module32First(IntPtr handle, byte[] me)
        {
            return m_api.Module32First(handle, me);
        }

        public static int Module32Next(IntPtr handle, byte[] me)
        {
            return m_api.Module32Next(handle, me);
        }

        #region --- IToolhelpAPI interface ---
        private interface IToolhelpAPI
        {
            IntPtr CreateToolhelp32Snapshot(uint flags, uint processid);
            int CloseToolhelp32Snapshot(IntPtr handle);
            int Process32First(IntPtr handle, byte[] pe);
            int Process32Next(IntPtr handle, byte[] pe);
            int Thread32First(IntPtr handle, byte[] te);
            int Thread32Next(IntPtr handle, byte[] te);
            int Module32First(IntPtr handle, byte[] me);
            int Module32Next(IntPtr handle, byte[] me);
        }
        #endregion

        #region --- CE Implementation ---
        internal class ToolhelpAPICE : IToolhelpAPI
        {
            private const string DLL = "toolhelp.dll";

            public IntPtr CreateToolhelp32Snapshot(uint flags, uint processid)
            {
                return CreateToolhelp32SnapshotAPI(flags, processid);
            }

            public int CloseToolhelp32Snapshot(IntPtr handle)
            {
                return CloseToolhelp32SnapshotAPI(handle);
            }

            public int Process32First(IntPtr handle, byte[] pe)
            {
                return Process32FirstAPI(handle, pe);
            }

            public int Process32Next(IntPtr handle, byte[] pe)
            {
                return Process32NextAPI(handle, pe);
            }

            public int Thread32First(IntPtr handle, byte[] te)
            {
                return Thread32FirstAPI(handle, te);
            }

            public int Thread32Next(IntPtr handle, byte[] te)
            {
                return Thread32NextAPI(handle, te);
            }

            public int Module32First(IntPtr handle, byte[] me)
            {
                return Module32FirstAPI(handle, me);
            }

            public int Module32Next(IntPtr handle, byte[] me)
            {
                return Module32NextAPI(handle, me);
            }

            [DllImport(DLL, SetLastError = true, EntryPoint = "CreateToolhelp32Snapshot")]
            private static extern IntPtr CreateToolhelp32SnapshotAPI(uint flags, uint processid);
            [DllImport(DLL, SetLastError = true, EntryPoint = "CloseToolhelp32Snapshot")]
            private static extern int CloseToolhelp32SnapshotAPI(IntPtr handle);
            [DllImport(DLL, SetLastError = true, EntryPoint = "Process32First")]
            private static extern int Process32FirstAPI(IntPtr handle, byte[] pe);
            [DllImport(DLL, SetLastError = true, EntryPoint = "Process32Next")]
            private static extern int Process32NextAPI(IntPtr handle, byte[] pe);
            [DllImport(DLL, SetLastError = true, EntryPoint = "Thread32First")]
            private static extern int Thread32FirstAPI(IntPtr handle, byte[] te);
            [DllImport(DLL, SetLastError = true, EntryPoint = "Thread32Next")]
            private static extern int Thread32NextAPI(IntPtr handle, byte[] te);
            [DllImport(DLL, SetLastError = true, EntryPoint = "Module32First")]
            private static extern int Module32FirstAPI(IntPtr handle, byte[] me);
            [DllImport(DLL, SetLastError = true, EntryPoint = "Module32Next")]
            private static extern int Module32NextAPI(IntPtr handle, byte[] me);
        }
        #endregion

        #region --- Desktop implementation ---
        internal class ToolhelpAPIWin32 : IToolhelpAPI
        {
            private const string DLL = "kernel32.dll";

            public IntPtr CreateToolhelp32Snapshot(uint flags, uint processid)
            {
                return CreateToolhelp32SnapshotAPI(flags, processid);
            }

            public int CloseToolhelp32Snapshot(IntPtr handle)
            {
                return CloseHandleAPI(handle);
            }

            public int Process32First(IntPtr handle, byte[] pe)
            {
                return Process32FirstAPI(handle, pe);
            }

            public int Process32Next(IntPtr handle, byte[] pe)
            {
                return Process32NextAPI(handle, pe);
            }

            public int Thread32First(IntPtr handle, byte[] te)
            {
                return Thread32FirstAPI(handle, te);
            }

            public int Thread32Next(IntPtr handle, byte[] te)
            {
                return Thread32NextAPI(handle, te);
            }

            public int Module32First(IntPtr handle, byte[] me)
            {
                return Module32FirstAPI(handle, me);
            }

            public int Module32Next(IntPtr handle, byte[] me)
            {
                return Module32NextAPI(handle, me);
            }

            [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
            public static extern int CloseHandleAPI(IntPtr hObject);
            [DllImport(DLL, SetLastError = true, EntryPoint = "CreateToolhelp32Snapshot")]
            private static extern IntPtr CreateToolhelp32SnapshotAPI(uint flags, uint processid);
            [DllImport(DLL, SetLastError = true, EntryPoint = "Process32First")]
            private static extern int Process32FirstAPI(IntPtr handle, byte[] pe);
            [DllImport(DLL, SetLastError = true, EntryPoint = "Process32Next")]
            private static extern int Process32NextAPI(IntPtr handle, byte[] pe);
            [DllImport(DLL, SetLastError = true, EntryPoint = "Thread32First")]
            private static extern int Thread32FirstAPI(IntPtr handle, byte[] te);
            [DllImport(DLL, SetLastError = true, EntryPoint = "Thread32Next")]
            private static extern int Thread32NextAPI(IntPtr handle, byte[] te);
            [DllImport(DLL, SetLastError = true, EntryPoint = "Module32First")]
            private static extern int Module32FirstAPI(IntPtr handle, byte[] me);
            [DllImport(DLL, SetLastError = true, EntryPoint = "Module32Next")]
            private static extern int Module32NextAPI(IntPtr handle, byte[] me);
        }
        #endregion
    }
}
