using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Security.Cryptography.Internal
{
	//http://smartdevices.microsoftdev.com/Learn/Articles/500.aspx#netcfadvinterop_topic3
	internal class Mem
	{
		/// <summary>
		/// The CryptMemAlloc function allocates memory for a buffer. 
		/// It is used by all Crypt32.lib functions that return allocated buffers.
		/// </summary>
		/// <param name="cbSize">Number of bytes to be allocated. </param>
		/// <returns>Returns a pointer to the buffer allocated. 
		/// If the function fails, NULL is returned. </returns>
		//LPVOID WINAPI CryptMemAlloc(ULONG cbSize);
		[DllImport("crypt32.dll", EntryPoint="CryptMemAlloc", SetLastError=true)]
		public static extern IntPtr CryptMemAlloc(int cbSize);
		
		/// <summary>
		/// The CryptMemFree function frees memory allocated by 
		/// CryptMemAlloc or CryptMemRealloc.
		/// </summary>
		/// <param name="pv">Pointer to the buffer to be freed. </param>
		//void WINAPI CryptMemFree(LPVOID pv);
		[DllImport("crypt32.dll", EntryPoint="CryptMemFree", SetLastError=true)]
		public static extern void CryptMemFree(IntPtr pv);
		
		/// <summary>
		/// The CryptMemRealloc function frees the memory currently allocated for a buffer 
		/// and allocates memory for a new buffer.
		/// </summary>
		/// <param name="pv">Pointer to a currently allocated buffer. </param>
		/// <param name="cbSize">Number of bytes to be allocated. </param>
		/// <returns>Returns a pointer to the buffer allocated. 
		/// If the function fails, NULL is returned. </returns>
		//LPVOID WINAPI CryptMemRealloc(LPVOID pv, ULONG cbSize);
		[DllImport("crypt32.dll", EntryPoint="CryptMemRealloc", SetLastError=true)]
		public static extern IntPtr CryptMemRealloc(IntPtr pv, int cbSize);

	}
}
