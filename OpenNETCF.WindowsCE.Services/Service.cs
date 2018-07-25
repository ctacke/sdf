using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace OpenNETCF.WindowsCE.Services
{
    public class Service
    {
        private string m_prefix = "";
        private IntPtr m_handle = IntPtr.Zero;
        private ServiceState m_state = ServiceState.Unknown;
        private string m_dll = "";
        private ServiceCollection m_parent;

        internal Service(ServiceEnumInfo info, ServiceCollection parent)
        {
            m_prefix = info.Prefix;
            m_handle = info.Handle;
            m_state = info.State;
            m_dll = info.DLLName;
            m_parent = parent;
        }

        /// <summary>
        /// Gets the service's handle
        /// </summary>
        public IntPtr Handle
        {
            get { return m_handle; }
        }

        /// <summary>
        /// Gets the string associated with the service 
        /// </summary>
        public string Prefix
        {
            get { return m_prefix; }
        }

        /// <summary>
        /// Gets the services current state
        /// </summary>
        public ServiceState State
        {
            get 
            {                 
                int status;
                CallIOCTL(NativeMethods.ServiceIoctl.Status, out status);
                return (ServiceState)status;
            }
        }

        /// <summary>
        /// Name of the DLL or module that contains the service
        /// </summary>
        public string ModuleName
        {
            get { return m_dll; }
        }

        /// <summary>
        /// Loads the service
        /// </summary>
        public void Load()
        {
            if (NativeMethods.ActivateService(this.Prefix, 0) == IntPtr.Zero)
            {
                throw new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());
            }
        }

        /// <summary>
        /// Unloads the service
        /// </summary>
        public void Unload()
        {
            IntPtr hSvc = NativeMethods.GetServiceHandle(m_prefix, IntPtr.Zero, 0);

            if (hSvc == NativeMethods.INVALID_HANDLE_VALUE)
            {
                throw new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());
            }

            if (NativeMethods.DeregisterService(hSvc) == 0)
            {
                throw new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());
            }

            m_parent.Remove(this);
        }

        /// <summary>
        /// Start the service that has been in inactive state
        /// </summary>
        public void Start()
        {
            CallIOCTL(NativeMethods.ServiceIoctl.Start);
        }

        /// <summary>
        /// Stop service, but do not unload service's DLL
        /// </summary>
        public void Stop()
        {
            CallIOCTL(NativeMethods.ServiceIoctl.Stop);
        }

        /// <summary>
        /// Refresh service's state from registry or other configuration storage
        /// </summary>
        public void Refresh()
        {
            CallIOCTL(NativeMethods.ServiceIoctl.Refresh);
        }

        /// <summary>
        /// Have service configure or remove its registry entries for auto-load
        /// </summary>
        public bool AutoLoad
        {
            set
            {
                if (value)
                {
                    RegistryKey key = GetServiceKey();
                    key.SetValue("Flags", 0);
                    key.Close();
                }
                else
                {
                    RegistryKey key = GetServiceKey();
                    key.SetValue("Flags", 4);
                    key.Close();
                }
            }
        }

        private RegistryKey GetServiceKey()
        {
            RegistryKey root = Registry.LocalMachine.OpenSubKey("Services");
            RegistryKey svcKey = null;

            string[] subkeyNames = root.GetSubKeyNames();

            foreach(string name in subkeyNames)
            {
                svcKey = root.OpenSubKey(name, true);
                if(((string)svcKey.GetValue("Prefix")).ToUpper().Substring(0, 3) == 
                    this.Prefix.ToUpper().Substring(0, 3))
                {
                    root.Close();
                    return svcKey;
                }
                svcKey.Close();
            }
            root.Close();
            return null;
        }

        /// <summary>
        /// Set service's debug zone mask 
        /// </summary>
        /// <param name="mask">Mask value to set</param>
        public void SetDebugMask(uint mask)
        {
            CallIOCTL(NativeMethods.ServiceIoctl.Start, mask);
        }

        /// <summary>
        /// Set the service's console (if it has one) on or off 
        /// </summary>
        public bool ConsoleEnabled
        {
            set
            {
                if (value)
                {
                    CallIOCTL(NativeMethods.ServiceIoctl.Start, "on");
                }
                else
                {
                    CallIOCTL(NativeMethods.ServiceIoctl.Start, "off");
                }
            }
        }

        private void CallIOCTL(NativeMethods.ServiceIoctl ioctl)
        {
            int ret;
            if (NativeMethods.ServiceIoControl(this.Handle, ioctl, IntPtr.Zero, 0, IntPtr.Zero, 0, out ret, IntPtr.Zero) == 0)
            {
                throw new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());
            }
        }

        private void CallIOCTL(NativeMethods.ServiceIoctl ioctl, uint output)
        {
            int ret;
            if(NativeMethods.ServiceIoControl(this.Handle, ioctl, IntPtr.Zero, 0, out ret, 4, out ret, IntPtr.Zero) == 0)
            {
                throw new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());
            }
        }

        private void CallIOCTL(NativeMethods.ServiceIoctl ioctl, string output)
        {
            int ret;
            if(NativeMethods.ServiceIoControl(this.Handle, ioctl, output, (output.Length + 1) * 2 /* Unicode length */, IntPtr.Zero, 0, out ret, IntPtr.Zero) == 0)
            {
                throw new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());
            }
        }

        private void CallIOCTL(NativeMethods.ServiceIoctl ioctl, out int returnData)
        {
            int ret;
            if (NativeMethods.ServiceIoControl(this.Handle, ioctl, IntPtr.Zero, 0, out returnData, 4, out ret, IntPtr.Zero) == 0)
            {
                throw new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());
            }
        }
    }
}
