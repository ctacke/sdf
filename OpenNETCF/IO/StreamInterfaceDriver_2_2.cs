using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace OpenNETCF.IO
{
    public abstract partial class StreamInterfaceDriver : IDisposable
    {
        public static void ActivateDevice(string registryPath)
        {
            lock (m_syncRoot)
            {
                IntPtr ptr = NativeMethods.ActivateDevice(registryPath, 0);

                if (ptr == IntPtr.Zero)
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(error);
                }
            }
        }

        public static void DeactivateDevice(string registryPath)
        {
            // since we use a "feature" of the OS that CE 6.0 blocks, make sure we validate the OS version
            if (Environment.OSVersion.Version.Major > 5)
            {
                throw new Exception("This function is viable only for CE 5.0 and earlier");
            }

            RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath);
            if (key == null)
            {
                throw new ArgumentException("No driver information found at provided path");
            }

            // get the prefix
            string prefix = key.GetValue("Prefix") as string;
            key.Close();
            if (prefix == null)
            {
                throw new ArgumentException("No driver information found at provided path");
            }

            key = Registry.LocalMachine.OpenSubKey("Drivers\\Active");
            string[] activeNames = key.GetSubKeyNames();
            key.Close();

            foreach (string subkey in activeNames)
            {
                key = Registry.LocalMachine.OpenSubKey(string.Format("Drivers\\Active\\{0}", subkey));
                string driverName = key.GetValue("Name") as string;
                if ((driverName != null) && (driverName.StartsWith(prefix)))
                {
                    int driverHandle = Convert.ToInt32(key.GetValue("Hnd", 0));
                    NativeMethods.DeactivateDevice(new IntPtr(driverHandle));
                    key.Close();
                    return;
                }
                key.Close();
            }
        }

		/// <summary>
		/// Call a device specific IOControl
		/// </summary>
		/// <param name="controlCode">The IOCTL code</param>
		/// <param name="inData">Data to pass into the IOCTL</param>
        [CLSCompliant(false)]
        protected void DeviceIoControl(uint controlCode, byte[] inData)
        {
            DeviceIoControl(controlCode, inData, null);
        }

        /// <summary>
        /// Call a device specific IOControl
        /// </summary>
        /// <param name="controlCode">The IOCTL code</param>
        [CLSCompliant(false)]
        protected void DeviceIoControl(uint controlCode)
        {
            DeviceIoControl(controlCode, null, null);
        }
    }
}
