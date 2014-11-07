// FileDriver.cpp : Defines the entry point for the DLL application.
//
#include "filedriver.h"
#include "winerror.h"

BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	  case DLL_PROCESS_ATTACH:
	  case DLL_THREAD_ATTACH:
	  case DLL_THREAD_DETACH:
	  case DLL_PROCESS_DETACH:
		  break;
	}
  return TRUE;
}

BOOL FIL_Deinit(DWORD hDeviceContext)
{
  return TRUE;
}

DWORD FIL_Init(LPCTSTR pContext, LPCVOID lpvBusContext)
{
  return TRUE;
}
  
VOID FIL_PowerUp(DWORD hDeviceContext )
{
}
 
VOID FIL_PowerDown(DWORD hDeviceContext)
{
}
 
DWORD FIL_Open(DWORD hDeviceContext, DWORD accessMode, DWORD shareMode)
{
  g_accessMode = accessMode;
  g_shareMode = shareMode;
  g_hFile = NULL;

  return hDeviceContext;
}
 
BOOL FIL_Close(DWORD hOpenContext)
{
  if(g_hFile != NULL)
  {
    CloseHandle(g_hFile);
  }

  return TRUE;
}
 
DWORD FIL_Read(DWORD hOpenContext, LPVOID pBuffer, DWORD Count)
{
  return 0;
}
 
DWORD FIL_Write(DWORD hOpenContext, LPCVOID pSourceBytes, DWORD numberOfBytes)
{
  if(g_hFile == NULL)
  {
    SetLastError(ERROR_NOT_READY);
    return -1;
  }
  DWORD written = 0;
  if(WriteFile(g_hFile, pSourceBytes, numberOfBytes, &written, NULL))
  {
    return written;
  }
  return -1;
}
 
DWORD FIL_Seek(DWORD hOpenContext, long Amount, DWORD Type)
{
  return 0;
}
 
BOOL FIL_IOControl(DWORD hOpenContext, DWORD dwCode, PBYTE pBufIn, DWORD dwLenIn, PBYTE pBufOut, DWORD dwLenOut, PDWORD pdwActualOut)
{
  TCHAR *testFileName;

  switch(dwCode)
  {
    case FIL_IOCTL_CREATE_FILE:
      if(
        (pBufIn == NULL) || 
        (dwLenIn == 0) ||
        (_tcslen((TCHAR*)pBufIn) > (MAX_PATH - 1))
        )
      {
        SetLastError(ERROR_INVALID_PARAMETER);
        return FALSE;
      }

      _tcscpy(g_fileName,(TCHAR*)pBufIn);
      
      g_hFile = CreateFile(g_fileName, g_accessMode, g_shareMode, NULL, CREATE_ALWAYS, 0, NULL);

      if(g_hFile == INVALID_HANDLE_VALUE)
      {
        g_hFile = NULL;
        return FALSE;
      }
      return TRUE;
    case FIL_IOCTL_GET_OPEN_FILE_NAME:
      *pdwActualOut = (_tcslen(g_fileName) + 1) * sizeof(TCHAR);
      if(
        (pBufOut == NULL) || 
        (dwLenOut == 0) ||
        (dwLenOut < *pdwActualOut)
        )
      {
        SetLastError(ERROR_INSUFFICIENT_BUFFER);
        return FALSE;
      }
      if(IsBadWritePtr(pBufOut, dwLenOut))
      {
        SetLastError(ERROR_INSUFFICIENT_BUFFER);
        return FALSE;
      }
      _tcscpy((TCHAR*)pBufOut, g_fileName);
      return TRUE;
    case FIL_IOCTL_GET_FILE_ATTRIBS:
      if(
        (pBufIn == NULL) || 
        (dwLenIn == 0) ||
        (_tcslen((TCHAR*)pBufIn) > (MAX_PATH - 1)) ||
        (pBufOut == NULL) ||
        (dwLenOut < 4)
        )
      {
        SetLastError(ERROR_INVALID_PARAMETER);
        return FALSE;
      }

      testFileName = (TCHAR*)pBufIn;
      *(DWORD*)pBufOut = GetFileAttributes(testFileName);
      *pdwActualOut = sizeof(DWORD);

      return TRUE;
    case FIL_IOCTL_DO_NOTHING:
      // do nothing but return true (used for testing case with no in or out variables
      return TRUE;
  }
  
  SetLastError(ERROR_BAD_COMMAND);

  return FALSE;
}

HANDLE g_hDevice = NULL;

void Load()
{
	BYTE	buffer[MAX_PATH];

  // Activate the driver
	_stprintf((TCHAR*)buffer, _T("Drivers\\BuiltIn\\%s"), DRIVER_NAME);
	g_hDevice = ActivateDeviceEx((TCHAR*)buffer, NULL, 0, NULL);

	if(!g_hDevice)
	{
		RETAILMSG(TRUE, (_T("  ActivateDevice failed (%i).\r\n"), GetLastError()));
	}
	else
	{
		DEBUGMSG(TRUE, (_T("  %s loaded.\r\n"), DRIVER_NAME));
	}
}

void Unload()
{
  if(g_hDevice != NULL)
  {
    DeactivateDevice(g_hDevice);
  }
}

void Register()
{
	HKEY	key		= 0;
	DWORD	ret		= 0;
	DWORD	val		= 0;
	DWORD	disp	= 0;
	HANDLE	hDev	= NULL;
	BYTE	buffer[MAX_PATH];
	
	// create the key
	_stprintf((TCHAR*)buffer, _T("Drivers\\BuiltIn\\%s"), DRIVER_NAME);
	ret = RegCreateKeyEx(HKEY_LOCAL_MACHINE, (TCHAR*)buffer, 0, NULL, 0, 0, NULL, &key, &disp);
	if(ret != ERROR_SUCCESS)
	{
		// failed to create!
		RETAILMSG(TRUE, (_T("  Unable to generate %s driver keys (%i)\r\n"), DRIVER_NAME, ret));
		goto exit;
	}
  
	// write the 'Dll' value
  ZeroMemory(buffer, MAX_PATH);
	_tcscpy((TCHAR*)buffer, DRIVER_LIBRARY);
	ret = RegSetValueEx(key, _T("Dll"), 0, REG_SZ, buffer, (_tcslen(DRIVER_LIBRARY) + 1) * sizeof(TCHAR));
	if(ret != ERROR_SUCCESS)
	{
		// failed to create!
		RETAILMSG(TRUE, (_T("  Unable to generate driver key %s (%i)\r\n"), DRIVER_LIBRARY, ret));
		goto exit;
	}

	// write the 'Prefix' value
  ZeroMemory(buffer, MAX_PATH);
	_tcscpy((TCHAR*)buffer, DRIVER_PREFIX);
	ret = RegSetValueEx(key, _T("Prefix"), 0, REG_SZ, buffer, (_tcslen(DRIVER_PREFIX) + 1) * sizeof(TCHAR));
	if(ret != ERROR_SUCCESS)
	{
		// failed to create!
		RETAILMSG(TRUE, (_T("  Unable to generate driver key %s (%i)\r\n"), DRIVER_PREFIX, ret));
		goto exit;
	}

	// write the 'Index' value
	val = DRIVER_INDEX;
	ret = RegSetValueEx(key, _T("Index"), 0, REG_DWORD, (BYTE*)&val, sizeof(DWORD));
	if(ret != ERROR_SUCCESS)
	{
		// failed to create!
		RETAILMSG(TRUE, (_T("  Unable to generate driver key 'Index' (%i)\r\n"), ret));
		goto exit;
	}

	// write the 'Order' value
	val = DRIVER_ORDER;
	ret = RegSetValueEx(key, _T("Order"), 0, REG_DWORD, (BYTE*)&val, sizeof(DWORD));
	if(ret != ERROR_SUCCESS)
	{
		// failed to create!
		RETAILMSG(TRUE, (_T("  Unable to generate driver key 'Order' (%i)\r\n"), ret));
		goto exit;
	}

exit:
	if(key)
		RegCloseKey(key);

	return;

}