using System;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.IO;
using System.Runtime.InteropServices;
using OpenNETCF.Net.NetworkInformation;

namespace OpenNETCF.Net
{
    public class NdisPower : StreamInterfaceDriver
    {
        private const uint IOCTL_NPW_SAVE_POWER_STATE = 0x120800;
        private const uint IOCTL_NPW_QUERY_SAVED_POWER_STATE = 0x120801;
        private const string PMCLASS_NDIS_MINIPORT = "{98C5250D-C29A-4985-AE5F-AFE5367E5006}";
        private const int POWER_NAME = 0x00000001;

        public NdisPower()
            : base("NPW1:")
        {
        }

        public void SetAdapterPower(string adapterName, bool enabled)
        {
            if (!enabled)
            {
                // if we're disabling, first unbind
                NDIS.UnbindInterface(adapterName);
            }

            this.Open(System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);

            NDISPWR_SAVEPOWERSTATE state = new NDISPWR_SAVEPOWERSTATE(adapterName);
            state.CePowerState = enabled ? DevicePowerState.Unspecified : DevicePowerState.D4;

            DeviceIoControl(IOCTL_NPW_SAVE_POWER_STATE, (byte[])state);

            this.Close();

            var pmName = string.Format("{0}\\{1}", PMCLASS_NDIS_MINIPORT, adapterName);

            SetDevicePower(pmName, POWER_NAME, state.CePowerState);

            if (enabled)
            {
                // if we're enabling, bind
                NDIS.BindInterface(adapterName);
            }
        }

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern int SetDevicePower(string pvDevice, int dwDeviceFlags, DevicePowerState DeviceState);

        private enum DevicePowerState
        {
            Unspecified = -1,
            /// <summary>
            /// Full On
            /// </summary>
            D0 = 0,
            /// <summary>
            /// Low Power
            /// </summary>
            D1 = 1,
            /// <summary>
            /// Stand-by
            /// </summary>
            D2 = 2,
            /// <summary>
            /// Sleep
            /// </summary>
            D3 = 3,
            /// <summary>
            /// Off
            /// </summary>
            D4 = 4
        }

        private class NDISPWR_SAVEPOWERSTATE
        {
            //struct _NDISPWR_SAVEPOWERSTATE
            //{
            //    LPCWSTR pwcAdapterName;
            //    CEDEVICE_POWER_STATE CePowerState;
            //}

            private string m_name;
            private GCHandle m_hName;
            public DevicePowerState CePowerState;
            public int SizeOf { get; private set; }

            public NDISPWR_SAVEPOWERSTATE(string adapterName)
            {
                AdapterName = adapterName;
                SizeOf = 8;
            }

            ~NDISPWR_SAVEPOWERSTATE()
            {
                Release();
            }

            private void Release()
            {
                if (m_hName.IsAllocated)
                {
                    m_hName.Free();
                }
            }

            public string AdapterName
            {
                get 
                {
                    // the +8 here is due to we've pinned a string *object* so we want to pass the object header info and get the data
                    var result = Marshal.PtrToStringUni(new IntPtr(m_hName.AddrOfPinnedObject().ToInt32() + 8));
                    return result.TrimEnd('\0'); 
                }
                set
                {
                    Release();
                    m_name = value + ("\0\0"); // it's a multi-sz, so it need double-null termination
                    m_hName = GCHandle.Alloc(m_name, GCHandleType.Pinned);
                }
            }

            public static explicit operator byte[](NDISPWR_SAVEPOWERSTATE state)
            {
                // the +8 here is due to we've pinned a string *object* so we want to pass the object header info and get the data
                int addr = state.m_hName.AddrOfPinnedObject().ToInt32() + 8;
                var buffer = new byte[state.SizeOf];
                Buffer.BlockCopy(BitConverter.GetBytes(addr), 0, buffer, 0, 4);
                Buffer.BlockCopy(BitConverter.GetBytes((int)state.CePowerState), 0, buffer, 4, 4);
                return buffer;
            }
        }
    }
}
