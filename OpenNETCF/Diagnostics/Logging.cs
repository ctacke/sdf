using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace OpenNETCF.Diagnostics
{
    /// <summary>
    /// This class enables you to turn on and off the creation of log files with diagnostic information about interoperability, loading the application, and networking
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// 
        /// </summary>
        public static bool LoggingEnabled
        {
            get 
            { 
                int value = (int)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging", "Enabled", 0);
                return (value == 1);
            }
            set 
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging",
                    "Enabled", (value) ? 1 : 0,
                    RegistryValueKind.DWord);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool InteropLoggingEnabled
        {
            get
            {
                int value = (int)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging\\Interop", "Enabled", 0);
                return (value == 1);
            }
            set
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging\\Interop",
                    "Enabled", (value) ? 1 : 0,
                    RegistryValueKind.DWord);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool LoaderLoggingEnabled
        {
            get
            {
                int value = (int)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging\\Loader", "Enabled", 0);
                return (value == 1);
            }
            set
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging\\Loader",
                    "Enabled", (value) ? 1 : 0,
                    RegistryValueKind.DWord);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool NetworkLoggingEnabled
        {
            get
            {
                int value = (int)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging\\Network", "Enabled", 0);
                return (value == 1);
            }
            set
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging\\Network", 
                    "Enabled", (value) ? 1 : 0,
                    RegistryValueKind.DWord);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string LogFilePath
        {
            get
            {
                return (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging\\Path", "Enabled",
                    System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase));
            }
            set
            {
                if (value == null)
                {
                    try
                    {
                        RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging\\Path", true);
                        key.DeleteValue("Path");
                        key.Close();
                    }
                    catch (Exception)
                    {
                        // failure is likely due to it not being there
                        // for this cut, ignore it
                    }
                }
                else
                {
                    Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging", "Path", value, RegistryValueKind.String);
                }
            }
        }

        /// <summary>
        ///  If there are two applications writing log files to the same directory, the older log file will always get overwritten with the newer log file when the second application is run. This key can be used as a differentiator for the log files.
        /// </summary>
        public static bool IncludeAppNameInLogFileName
        {
            get
            {
                int value = (int)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging", "UseApp", 0);
                return (value == 1);
            }
            set
            {
                Registry.SetValue(
                        "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging", 
                        "UseApp", 
                        (value) ? 1 : 0, 
                        RegistryValueKind.DWord);
            }
        }

        /// <summary>
        /// This key is useful if you want to run the same application but have separate logs. This adds the process ID to the log file name, so that each run of the same application creates a new log file with a different name.
        /// </summary>
        public static bool IncludeProcessIDInLogFileName
        {
            get
            {
                int value = (int)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging", "UsePid", 0);
                return (value == 1);
            }
            set
            {
                Registry.SetValue(
                        "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging",
                        "UsePid",
                        (value) ? 1 : 0,
                        RegistryValueKind.DWord);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool FlushLogFileAfterEachEvent
        {
            get
            {
                int value = (int)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging", "Flush", 0);
                return (value == 1);
            }
            set
            {
                Registry.SetValue(
                        "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETCompactFramework\\Diagnostics\\Logging",
                        "Flush",
                        (value) ? 1 : 0,
                        RegistryValueKind.DWord);
            }
        }
    }
}
