using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.WindowsCE.Services
{
    /*
        typedef struct _ServiceEnumInfo {
	        WCHAR   szPrefix[6];     
	        WCHAR   *szDllName;
	        HANDLE  hServiceHandle;
	        DWORD   dwServiceState;   // one of SERVICE_STATE_XXX values above.
        } ServiceEnumInfo;
    */

    internal class ServiceEnumInfo
    {
        private const int PREFIX_OFFSET = 0;
        private const int PREFIX_LENGTH = 12;
        private const int DLL_OFFSET = PREFIX_OFFSET + PREFIX_LENGTH;
        private const int DLL_LENGTH = 4;
        private const int HANDLE_OFFSET = DLL_OFFSET + DLL_LENGTH;
        private const int HANDLE_LENGTH = 4;
        private const int STATE_OFFSET = HANDLE_OFFSET + HANDLE_LENGTH;
        private const int STATE_LENGTH = 4;

        // jsm - Defect 282: Changed service collection behavior for CE 6.0 and later

        private string m_ce60Prefix = String.Empty;
        private string m_ce60DLLName = String.Empty;
        private IntPtr m_ce60Handle = IntPtr.Zero;
        private ServiceState m_ce60state = ServiceState.NotInitialized;

        public const int SIZE = STATE_OFFSET + STATE_LENGTH;

        private byte[] m_data = new byte[SIZE];

        internal ServiceEnumInfo(byte[] data, int srcOffset)
        {
            Buffer.BlockCopy(data, srcOffset, m_data, 0, SIZE);
        }

        internal ServiceEnumInfo(byte[] data)
        {
            Buffer.BlockCopy(data, 0, m_data, 0, SIZE);
        }

        internal ServiceEnumInfo(string prefix, string dll, IntPtr handle, ServiceState state)
        {
            m_ce60Prefix = prefix;
            m_ce60DLLName = dll;
            m_ce60state = state;
            m_ce60Handle = handle;
        }

        public static implicit operator byte[](ServiceEnumInfo sei) { return sei.m_data; }
        public static implicit operator ServiceEnumInfo(byte[] data) { return new ServiceEnumInfo(data); }

        public string Prefix
        {
            get 
            {
                if (Environment.OSVersion.Version.Major > 5)
                {
                    return m_ce60Prefix;
                }
                else
                {
                    string prefix = Encoding.Unicode.GetString(m_data, PREFIX_OFFSET, PREFIX_LENGTH);
                    return prefix.Substring(0, prefix.IndexOf('\0'));
                }
            }
        }

        public unsafe string DLLName
        {
            get
            {
                if (Environment.OSVersion.Version.Major > 5)
                {
                    return m_ce60DLLName;
                }
                else
                {
                    int address = BitConverter.ToInt32(m_data, DLL_OFFSET);
                    string name = Marshal.PtrToStringUni(new IntPtr(address));
                    return name;
                }
            }
        }

        public IntPtr Handle
        {
            get
            {
                if (Environment.OSVersion.Version.Major > 5)
                {
                    return m_ce60Handle;
                }
                else
                {
                    return new IntPtr(BitConverter.ToInt32(m_data, HANDLE_OFFSET));
                }
            }
        }

        public ServiceState State
        {
            get
            {
                if (Environment.OSVersion.Version.Major > 5)
                {
                    return m_ce60state;
                }
                else
                {
                    return (ServiceState)BitConverter.ToInt32(m_data, STATE_OFFSET);
                }
            }
        }
    }
}
