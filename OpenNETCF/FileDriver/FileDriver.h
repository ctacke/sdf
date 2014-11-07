#include "windows.h"
#include <Devload.h>

#define DRIVER_NAME		  _T("FileDriver")
#define DRIVER_LIBRARY	_T("FileDriver.dll")
#define DRIVER_PREFIX	  _T("FIL")
#define DRIVER_INDEX	  1
#define DRIVER_ORDER	  9

#pragma comment(linker, "/nodefaultlib:libc.lib")
#pragma comment(linker, "/nodefaultlib:libcd.lib")

#ifdef WIN32_PLATFORM_PSPC
  // PPC 2003-specific
  #pragma comment(lib, "secchk.lib")
#endif


#define FIL_IOCTL_CREATE_FILE           1 // in
#define FIL_IOCTL_GET_OPEN_FILE_NAME    2 // out
#define FIL_IOCTL_GET_FILE_ATTRIBS      3 // in/out
#define FIL_IOCTL_DO_NOTHING            4 // void

HANDLE  g_hFile;
DWORD   g_accessMode;
DWORD   g_shareMode;
TCHAR   g_fileName[MAX_PATH];

/*
BOOL WINAPI DllEntry(HINSTANCE DllInstance, ULONG Reason, LPVOID Reserved);
BOOL FIL_Deinit( DWORD hDeviceContext );
DWORD FIL_Init(ULONG   RegistryPath);
VOID FIL_PowerUp(DWORD hDeviceContext );
VOID FIL_PowerDown(DWORD hDeviceContext);
DWORD FIL_Open(DWORD hDeviceContext, DWORD AccessCode, DWORD ShareMode);
BOOL FIL_Close(DWORD hOpenContext);
DWORD FIL_Read(DWORD hOpenContext, LPVOID pBuffer, DWORD Count);
DWORD FIL_Write(DWORD hOpenContext, LPCVOID pSourceBytes, DWORD NumberOfBytes);
DWORD FIL_Seek(DWORD hOpenContext, long Amount, DWORD Type);
BOOL FIL_IOControl(DWORD hOpenContext, DWORD dwCode, PBYTE pBufIn, DWORD dwLenIn, PBYTE pBufOut, DWORD dwLenOut, PDWORD pdwActualOut);
void RegisterAndLoad();
*/