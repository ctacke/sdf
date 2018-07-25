using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{
    /*
    typedef struct
    {    
        LPWSTR          wszGuid;
        LPWSTR          wszDescr;
        ULONG           ulMediaState;
        ULONG           ulMediaType;
        ULONG           ulPhysicalMediaType;
        INT             nInfraMode;
        INT             nAuthMode;
        INT             nWepStatus;
        DWORD           dwCtlFlags;     // control flags (see INTFCTL_* defines)
        DWORD           dwCapabilities; // capabilities flags (see INTFCAP_* defines)
        RAW_DATA        rdSSID;         // encapsulates the SSID raw binary
        RAW_DATA        rdBSSID;        // encapsulates the BSSID raw binary
        RAW_DATA        rdBSSIDList;    // encapsulates one WZC_802_11_CONFIG_LIST structure
        RAW_DATA        rdStSSIDList;   // encapsulates one WZC_802_11_CONFIG_LIST structure
        RAW_DATA        rdCtrlData;     // data for various control actions on the interface
        BOOL            bInitialized;   //  To track caller that freeing
                                        //  the same structure more than one time..
        DWORD           nWPAMCastCipher;

    } INTF_ENTRY, *PINTF_ENTRY;
    */
    /// <summary>
    /// Interface Entry for WZC
    /// </summary>
    internal struct INTF_ENTRY : IDisposable, ICloneable
    {
        private IntPtr wszGuid;
        private IntPtr wszDescr;
        public uint ulMediaState;
        public uint ulMediaType;
        public uint ulPhysicalMediaType;
        public InfrastructureMode nInfraMode;
        public AuthenticationMode nAuthMode;
        public WEPStatus nWepStatus;

        /// <summary>
        /// control flags (see INTFCTL_* defines)
        /// </summary>
        public uint dwCtlFlags;
        /// <summary>
        /// capabilities flags (see INTFCAP_* defines)
        /// </summary>
        public uint dwCapabilities;
        /// <summary>
        /// encapsulates the SSID raw binary
        /// </summary>
        public RAW_DATA rdSSID;
        /// <summary>
        /// encapsulates the BSSID raw binary
        /// </summary>
        public RAW_DATA rdBSSID;
        /// <summary>
        /// encapsulates one WZC_802_11_CONFIG_LIST structure
        /// </summary>
        public RAW_DATA rdBSSIDList;
        /// <summary>
        /// encapsulates one WZC_802_11_CONFIG_LIST structure
        /// </summary>
        public RAW_DATA rdStSSIDList;
        /// <summary>
        /// data for various control actions on the interface
        /// </summary>
        public RAW_DATA rdCtrlData;
        /// <summary>
        /// To track caller that freeing the same structure more than one time..
        /// </summary>
        public int bInitialized;


        /// <summary>
        /// Creates a new entry with given name in memory 
        /// </summary>
        /// <param name="guid">Name</param>
        /// <returns>Entry</returns>
        public static INTF_ENTRY GetEntry(string guid)
        {
            INTF_ENTRY entry = new INTF_ENTRY();
            entry.Guid = guid;
            INTFFlags dwOutFlags;
            int uret = WZCPInvokes.WZCQueryInterface(null, INTFFlags.INTF_ALL, ref entry, out dwOutFlags);
            if (uret > 0)
                throw new AdapterException(uret, "WZCQueryInterface");
            return entry;
        }

        /// <summary>
        /// SSID
        /// </summary>
        public string SSID
        {
            get { return Encoding.ASCII.GetString(rdSSID.lpData, 0, rdSSID.lpData.Length); }
            set { rdSSID.lpData = Encoding.ASCII.GetBytes(value); }
        }
        public string BSSID
        {
            get { return BitConverter.ToString(rdBSSID.lpData, 0); }
        }

        /// <summary>
        /// Entry name
        /// </summary>
        public string Guid
        {
            get
            {
                return Marshal.PtrToStringUni(wszGuid);
            }
            set
            {
                if (wszGuid != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(wszGuid);
                }
                int len = (value.Length + 1) * 2;
                wszGuid = Marshal.AllocHGlobal(len);
                OpenNETCF.Runtime.InteropServices.Marshal2.SetMemory(wszGuid, 0, len, false);
                byte[] chars = Encoding.Unicode.GetBytes(value + '\0');
                Marshal.Copy(chars, 0, wszGuid, chars.Length);
            }
        }

        /// <summary>
        /// Entry description
        /// </summary>
        public string Description
        {
            get
            {
                return Marshal.PtrToStringUni(wszDescr);
            }
            set
            {
                if (wszDescr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(wszDescr);
                }
                int len = (value.Length + 1) * 2;
                wszDescr = Marshal.AllocHGlobal(len);
                OpenNETCF.Runtime.InteropServices.Marshal2.SetMemory(wszGuid, 0, len, false);
                byte[] chars = Encoding.Unicode.GetBytes(value + '\0');
                Marshal.Copy(chars, 0, wszDescr, chars.Length);
            }
        }
        /// <summary>
        /// Overriden
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Guid;
        }


        #region IDisposable Members

        public void Dispose()
        {
            if (wszDescr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.wszDescr);
            }
            if (wszGuid != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.wszGuid);
            }
            WZCPInvokes.WZCDeleteIntfObj(ref this);
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            INTF_ENTRY entry = (INTF_ENTRY)MemberwiseClone();
            entry.rdBSSID.Clear();
            entry.rdBSSIDList.Clear();
            entry.rdCtrlData.Clear();
            entry.rdSSID.Clear();
            entry.rdStSSIDList.Clear();
            entry.rdBSSID.lpData = this.rdBSSID.lpData;
            entry.rdBSSIDList.lpData = this.rdBSSIDList.lpData;
            entry.rdCtrlData.lpData = this.rdCtrlData.lpData;
            entry.rdSSID.lpData = this.rdSSID.lpData;
            entry.rdStSSIDList.lpData = this.rdStSSIDList.lpData;
            return entry;
        }

        #endregion
    }

    /// <summary>
    /// Encapsulates generic data blob
    /// </summary>
    internal struct RAW_DATA : IDisposable
    {
        //byte[] m_data;
        private uint m_cbData;
        private IntPtr m_lpData;

        public RAW_DATA(byte[] data)
        {
            //m_data = new byte[0];
            m_lpData = IntPtr.Zero;
            m_cbData = (uint)data.Length;
            lpData = data;
        }
        public uint cbData { get { return m_cbData; } }
        public byte[] lpData
        {
            get
            {
                if (m_lpData == IntPtr.Zero)
                    return null;
                byte[] data = new byte[m_cbData];
                Marshal.Copy(m_lpData, data, 0, (int)m_cbData);
                return data;
            }
            set
            {
                FreeMemory();
                m_lpData = Marshal.AllocHGlobal(value.Length);
                OpenNETCF.Runtime.InteropServices.Marshal2.SetMemory(m_lpData, 0, value.Length, false);
                Marshal.Copy(value, 0, m_lpData, value.Length);
            }
        }

        public IntPtr lpDataDirect
        {
            get
            {
                return m_lpData;
            }
        }

        internal void Clear()
        {
            m_lpData = IntPtr.Zero;
            m_cbData = 0;
        }
        #region IDisposable Members

        public void Dispose()
        {
            FreeMemory();
        }

        private void FreeMemory()
        {
            if (m_lpData != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(m_lpData);
                m_lpData = IntPtr.Zero;
            }
        }
        #endregion
    }
    /// <summary>
    /// List of interface names
    /// </summary>
    internal struct INTFS_KEY_TABLE : IDisposable
    {
        private uint dwNumIntfs;
        public IntPtr pData;

        public INTFS_KEY_TABLE(uint size)
        {
            dwNumIntfs = size;
            if (size != 0)
            {
                int len = Marshal.SizeOf(typeof(IntPtr)) * (int)size;
                pData = Marshal.AllocHGlobal(len);
                OpenNETCF.Runtime.InteropServices.Marshal2.SetMemory(pData, 0, len, false);
            }
            else
                pData = IntPtr.Zero;
        }

        public string this[uint i]
        {
            get
            {
                return Marshal.PtrToStringUni((IntPtr)(Marshal.ReadInt32(pData, (int)i * 4)));
            }
        }

        public uint Count
        {
            get { return dwNumIntfs; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (pData != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pData);
            }
        }

        #endregion
    }
}
