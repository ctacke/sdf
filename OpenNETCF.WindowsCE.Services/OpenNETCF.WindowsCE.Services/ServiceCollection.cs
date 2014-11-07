using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace OpenNETCF.WindowsCE.Services
{


    public class ServiceCollection : IEnumerable<Service>, IDisposable
    {
        private List<Service> m_services = new List<Service>();
        internal const uint OPEN_EXISTING = 3;
        internal const uint IOCTL_SERVICE_STATUS = 0x1040020;
        private bool _disposed;

        [DllImport("coredll.dll", EntryPoint = "CreateFile", SetLastError = true)]
        internal static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            int lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            int hTemplateFile);

        [DllImport("coredll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool DeviceIoControl(
            IntPtr hDevice, uint dwIoControlCode, [In] int[] inBuffer, int nInBufferSize, [Out] int[] outBuffer,
            int nOutBufferSize, ref int pBytesReturned, IntPtr lpOverlapped);

        //[DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        //internal static extern int DeviceIoControlCE(
        //    int hDevice,
        //    int dwIoControlCode,
        //    byte[] lpInBuffer,
        //    int nInBufferSize,
        //    byte[] lpOutBuffer,
        //    int nOutBufferSize,
        //    ref int lpBytesReturned,
        //    IntPtr lpOverlapped);

        [DllImport("coredll.dll")]
        internal static extern int CloseHandle(IntPtr hObject);

        public IEnumerator<Service> GetEnumerator()
        {
            return m_services.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_services.GetEnumerator();
        }

        private void Add(Service service)
        {
            m_services.Add(service);
        }

        internal void Remove(Service service)
        {
            for (int i = 0; i < m_services.Count; i++)
            {
                if (m_services[i].Prefix == service.Prefix)
                {
                    m_services.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Gets a collection of Services supported on the device
        /// </summary>
        /// <returns></returns>
        public static ServiceCollection GetServices()
        {
            ServiceCollection collection = new ServiceCollection();

            if (Environment.OSVersion.Version.Major > 5)
            {
                // jsm - in CE 6, GetServiceHandle is now deprecated

                // 1. Open registry to inspect services
                string DLLName = String.Empty;

                using (Microsoft.Win32.RegistryKey root = Registry.LocalMachine.OpenSubKey("Services"))
                {
                    string[] subkeyNames = root.GetSubKeyNames();

                    foreach (string name in subkeyNames)
                    {
                        using (RegistryKey svcKey = root.OpenSubKey(name, true))
                        {
                            string prefixVal = (string)svcKey.GetValue("Prefix");
                            int svcIndex = (int)svcKey.GetValue("Index");
                            DLLName = (string)svcKey.GetValue("Dll");

                            if (!String.IsNullOrEmpty(prefixVal))
                            {
                                string prefixFull = String.Format("{0}:", prefixVal.ToUpper().Substring(0, 3) + svcIndex.ToString());

                                // 2. Get handles by way of CreateFile
                                IntPtr svcHandle = CreateFile(prefixFull, 0, 0, 0, OPEN_EXISTING, 0, 0);

                                int numBytesReturned = 0;
                                int lastError = 0;

                                int[] outBuffer = new int[1];
                                int outSize = sizeof(int) * outBuffer.Length;

                                if (!DeviceIoControl(svcHandle, IOCTL_SERVICE_STATUS, null, 0, outBuffer, outSize, ref numBytesReturned, IntPtr.Zero))
                                {
                                    lastError = Marshal.GetLastWin32Error();
                                }
                                else
                                {
                                    ServiceState svcState = (ServiceState)outBuffer[0];
                                    collection.Add(new Service(new ServiceEnumInfo(prefixFull, DLLName, svcHandle, svcState), collection));
                                }
                            }
                        }
                    }
                }

                return collection;
            }

            else
            {

                byte[] data = new byte[0];
                int count;
                int size = 0;

                NativeMethods.EnumServices(data, out count, ref size);

                data = new byte[size];

                NativeMethods.EnumServices(data, out count, ref size);

                for (int i = 0; i < count; i++)
                {
                    ServiceEnumInfo info = new ServiceEnumInfo(data, i * ServiceEnumInfo.SIZE);
                    collection.Add(new Service(info, collection));
                }
            }

            return collection;
        }

        // jsm - Defect 282: Changed service collection behavior for CE 6.0 and later

        #region Disposing

        /// <summary>
        /// Disposes internally allocated resources
        /// </summary>
        public void Dispose()
        {
            if (this._disposed)
            {
                return;
            }
            this._disposed = true;

            if (Environment.OSVersion.Version.Major > 5)
            {
                foreach (Service s in m_services)
                {
                    if (s.Handle != IntPtr.Zero)
                    {
                        CloseHandle(s.Handle);
                    }
                }
            }
        }

        /// <summary>
        /// Finalizes the ServiceCollection instance
        /// </summary>
        ~ServiceCollection()
        {
            Dispose();
        }

        #endregion

    }

}
