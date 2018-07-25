using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.ToolHelp
{
    /*
    typedef struct tagMODULEENTRY32 {
        DWORD   dwSize;
        DWORD   th32ModuleID;
        DWORD   th32ProcessID;
        DWORD   GlblcntUsage;
        DWORD   ProccntUsage;
        BYTE   *modBaseAddr;
        DWORD   modBaseSize;
        HMODULE hModule;
        TCHAR   szModule[MAX_PATH];
        TCHAR   szExePath[MAX_PATH];
        DWORD	dwFlags;
    } MODULEENTRY32, *PMODULEENTRY32, *LPMODULEENTRY32;
    */
    internal class MODULEENTRY32
    {
        // constants for structure definition
        private const int SIZE_OFFSET = 0;
        private const int SIZE_LEN = 4;
        private const int MODULE_ID_OFFSET = SIZE_OFFSET + SIZE_LEN;
        private const int MODULE_ID_LEN = 4;
        private const int PROCESS_ID_OFFSET = MODULE_ID_OFFSET + MODULE_ID_LEN;
        private const int PROCESS_ID_LEN = 4;
        private const int GLOBAL_COUNT_OFFSET = PROCESS_ID_OFFSET + PROCESS_ID_LEN;
        private const int GLOBAL_COUNT_LEN = 4;
        private const int PROCESS_COUNT_OFFSET = GLOBAL_COUNT_OFFSET + GLOBAL_COUNT_LEN;
        private const int PROCESS_COUNT_LEN = 4;
        private const int BASE_ADDR_OFFSET = PROCESS_COUNT_OFFSET + PROCESS_COUNT_LEN;
        private const int BASE_ADDR_LEN = 4;
        private const int BASE_SIZE_OFFSET = BASE_ADDR_OFFSET + BASE_ADDR_LEN;
        private const int BASE_SIZE_LEN = 4;
        private const int HMODULE_OFFSET = BASE_SIZE_OFFSET + BASE_SIZE_LEN;
        private const int HMODULE_LEN = 4;
        private const int MODULE_NAME_OFFSET = HMODULE_OFFSET + HMODULE_LEN;
        private const int MODULE_NAME_LEN = MAX_PATH * 2;
        private const int EXE_PATH_OFFSET = MODULE_NAME_OFFSET + MODULE_NAME_LEN;
        private const int EXE_PATH_LEN = MAX_PATH * 2;
        private const int FLAGS_OFFSET = EXE_PATH_OFFSET + EXE_PATH_LEN;
        private const int FLAGS_LEN = 4;

        private const int MAX_PATH = 260;

        private const int Size = FLAGS_OFFSET + FLAGS_LEN;

        // data members
        public uint dwSize;         
        public uint th32ModuleID;  
        public uint th32ProcessID;  
        public uint GlblcntUsage;  
        public uint ProccntUsage;  
        public uint modBaseAddr;  
        public uint modBaseSize;  
        public uint hModule;
        public string szModule;
        public string szExePath;
        public uint dwFlags;

		public MODULEENTRY32()
		{
		}

        // create a MODULEENTRY32 instance based on a byte array		
        public MODULEENTRY32(byte[] aData)
		{
            dwSize = Util.GetUInt(aData, SIZE_OFFSET);
            th32ModuleID = Util.GetUInt(aData, MODULE_ID_OFFSET);
            th32ProcessID = Util.GetUInt(aData, PROCESS_ID_OFFSET);
            GlblcntUsage = Util.GetUInt(aData, GLOBAL_COUNT_OFFSET);
            ProccntUsage = Util.GetUInt(aData, PROCESS_COUNT_OFFSET);
            modBaseAddr = Util.GetUInt(aData, BASE_ADDR_OFFSET);
            modBaseSize = Util.GetUInt(aData, SIZE_OFFSET);
            hModule = Util.GetUInt(aData, HMODULE_OFFSET);
            szModule = Util.GetString(aData, MODULE_NAME_OFFSET, MODULE_NAME_LEN).TrimEnd('\0');
            szExePath = Util.GetString(aData, EXE_PATH_OFFSET, EXE_PATH_LEN).TrimEnd('\0');
            dwFlags = Util.GetUInt(aData, HMODULE_OFFSET);
        }

		// create an initialized data array
		public byte[] ToByteArray()
		{
			byte[] aData;
			aData = new byte[Size];
			//set the Size member
            Util.SetUInt(aData, SIZE_OFFSET, Size);
			return aData;
		}
    }
}
