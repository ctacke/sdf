using System;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Runtime.InteropServices 
{
	/// <summary>
	/// Provides a collection of methods for allocating unmanaged memory, copying unmanaged memory blocks, and converting managed to unmanaged types, as well as other miscellaneous methods used when interacting with unmanaged code. 
	/// </summary>
    /// <seealso cref="Marshal"/>
	public static class Marshal2 
	{	
		#region Fields

		private static readonly int HIWORDMASK = -65536;//new IntPtr((long)-65536); 

		#endregion

		#region Read functions

		#region IntPtr
		/// <summary>
		/// Reads an IntPtr from an unmanaged pointer.   
		/// </summary>
		/// <param name="ptr">The address in unmanaged memory from which to read. </param>
		/// <param name="ofs">The offset from the ptr where the IntPtr is located.</param>
		/// <returns>The IntPtr read from the ptr parameter. </returns>
		public static IntPtr ReadIntPtr(IntPtr ptr, int ofs)
		{
			int i = Marshal.ReadInt32(ptr,ofs);
			return new IntPtr(i);
		}
		#endregion

		#region UInt32

		/// <summary>
		/// Reads a 32-bit unsigned integer from unmanaged memory.
		/// </summary>
		/// <param name="ptr">The base address in unmanaged memory from which to read.</param>
		/// <param name="ofs">An additional byte offset, added to the ptr parameter before reading.</param>
		/// <returns>The 32-bit unsigned integer read from the ptr parameter.</returns>
		[CLSCompliant(false)]
		public static UInt32 ReadUInt32(IntPtr ptr, int ofs)
		{
			byte[] data = new byte[4];
			Marshal.Copy(new IntPtr(ptr.ToInt32() + ofs), data, 0, 4);

			return BitConverter.ToUInt32(data,0);
		}
		#endregion

		#region UInt16


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ptr"></param>
		/// <param name="ofs"></param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public static UInt16 ReadUInt16(IntPtr ptr, int ofs)
		{
			byte[] data = new byte[2];
			Marshal.Copy(new IntPtr(ptr.ToInt32() + ofs), data, 0, 2);

			return BitConverter.ToUInt16(data,0);
		}
		#endregion

		#region String
		/// <summary>
		/// Reads a string from an unmanaged pointer.   
		/// </summary>
		/// <param name="ptr">The address in unmanaged memory from which to read. </param>
		/// <param name="ofs">The offset from the ptr where the string is located.</param>
		/// <param name="len">Length in characters.</param>
		/// <returns>The string read from the ptr parameter. </returns>
		public static string PtrToStringUni(IntPtr ptr, int ofs, int len)
		{
            return Marshal.PtrToStringUni(new IntPtr(ptr.ToInt32() + ofs), len);
		}

		/// <summary>
		/// Allocates a managed System.String, copies a specified number of characters from an unmanaged ANSI string into it, and widens each ANSI character to Unicode.
		/// </summary>
		/// <param name="ptr">The address of the first character of the unmanaged string.</param>
		/// <param name="ofs"></param>
		/// <param name="len">The byte count of the input string to copy.</param>
		/// <returns>A managed System.String that holds a copy of the native ANSI string.</returns>
		public static string PtrToStringAnsi(IntPtr ptr, int ofs, int len)
		{
			int cb = len;
			byte[] data = new byte[cb];
			Marshal.Copy(new IntPtr(ptr.ToInt32() + ofs), data, 0, cb);

			string s = Encoding.ASCII.GetString(data, 0, len);
			
			int nullpos = s.IndexOf('\0');
			if(nullpos > -1)
				s = s.Substring(0,nullpos);

			return s;
		}

		/// <summary>
		/// Copies all characters up to the first null from an unmanaged ANSI string to a managed System.String. Widens each ANSI character to Unicode.
		/// </summary>
		/// <param name="ptr">The address of the first character of the unmanaged string.</param>
		/// <returns>A managed <see cref="T:System.String"/> object that holds a copy of the unmanaged ANSI string.</returns>
		public static string PtrToStringAnsi(IntPtr ptr)
		{
			string returnval = "";
			byte thisbyte = 0;
			int offset = 0;

			//while more chars to read
			while(true)
			{
				//read current byte
				thisbyte = Marshal.ReadByte(ptr, offset);

				//if not null
				if(thisbyte == 0)
				{
					break;
				}

				//add the character
				returnval += ((char)thisbyte).ToString();
				
				//move to next position
				offset++;
			}

			return returnval;
		}

		/// <summary>
		/// Allocates a managed <see cref="T:System.String"/> and copies all characters up to the first null character from a string stored in unmanaged memory into it.
		/// </summary>
		/// <param name="ptr">The address of the first character.</param>
		/// <returns>A managed string that holds a copy of the unmanaged string.</returns>
		public static string PtrToStringAuto(IntPtr ptr)
		{
			if(ptr==IntPtr.Zero)
			{
				return null;
			}
			else
			{
				//final string value
				string returnval = "";

				//read first byte
				int firstbyte = Marshal.ReadByte(ptr, 0);
				//read second byte
				int secondbyte = Marshal.ReadByte(ptr, 1);

				//if first byte is non-zero continue
				if(firstbyte!=0)
				{
				
					//if second byte is zero we may have unicode or one byte string
					if(secondbyte==0)
					{
						//read third byte
						int thirdbyte = Marshal.ReadByte(ptr, 2);

						//if third byte is null this is a single byte string
						if(thirdbyte==0)
						{
							//single ascii char
							returnval = ((char)firstbyte).ToString();
						}
						else
						{
							//read unicode
							return Marshal.PtrToStringUni(ptr);
						}
					} 
					else
					{
						//else appears to be ASCII
						return PtrToStringAnsi(ptr);
					}
				}

				return returnval;
			}
		}
		#endregion

		#region Char
		/// <summary>
		/// Reads a single char from an unmanaged pointer.   
		/// </summary>
		/// <param name="ptr">The address in unmanaged memory from which to read. </param>
		/// <param name="ofs">The offset from the ptr where the char is located.</param>
		/// <returns>The char read from the ptr parameter. </returns>
		public static char ReadChar(IntPtr ptr, int ofs)
		{
			byte[] data = new byte[Marshal.SystemDefaultCharSize];
			Marshal.Copy(new IntPtr(ptr.ToInt32() + ofs),data, 0, data.Length);

			return BitConverter.ToChar(data,0);
		}
		#endregion

		#region Byte[]
		/// <summary>
		/// Reads a byte array from an unmanaged pointer.   
		/// </summary>
		/// <param name="ptr">The address in unmanaged memory from which to read. </param>
		/// <param name="ofs">The offset from the ptr where the byte array is located.</param>
		/// <param name="len">The length of the byte array to read.</param>
		/// <returns>The byte array read from the ptr parameter. </returns>
		public static byte[] ReadByteArray(IntPtr ptr, int ofs, int len)
		{
			byte[] data = new byte[len];
			Marshal.Copy(new IntPtr(ptr.ToInt32() + ofs), data, 0, len);

			return data;
		}
		#endregion

		#region UInt64
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ptr"></param>
		/// <param name="ofs"></param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public static UInt64 ReadUInt64(IntPtr ptr, int ofs)
		{
			byte[] data = new byte[8];
			Marshal.Copy(new IntPtr(ptr.ToInt32() + ofs), data, 0, 8);

			return BitConverter.ToUInt64(data,0);
		}
		#endregion

		#region Bool

		/// <summary>
		/// Reads a bool from an unmanaged pointer.   
		/// </summary>
		/// <param name="ptr">The address in unmanaged memory from which to read. </param>
		/// <param name="ofs">The offset from the ptr where the bool is located.</param>
		/// <returns>The bool read from the ptr parameter. </returns>
		public static bool ReadBool(IntPtr ptr, int ofs)
		{
			bool b = false;

			byte[] data = new byte[4];
			Marshal.Copy(new IntPtr(ptr.ToInt32() + ofs), data, 0, 4);

			b = Convert.ToBoolean(data);

			return b;
		}

		#endregion

		#endregion

		#region Write functions

		#region IntPtr

		/// <summary>
		/// Writes an IntPtr value to unmanaged memory.   
		/// </summary>
		/// <param name="ptr">The address in unmanaged memory from which to write. </param>
		/// <param name="ofs">The offset of the IntPtr from the ptr.</param>
		/// <param name="val">The value to write. </param>
		public static void WriteIntPtr(IntPtr ptr, int ofs, IntPtr val)
		{
            Marshal.WriteIntPtr(new IntPtr(ptr.ToInt32() + ofs), val);
		}
		
		#endregion

		#region UInt32

		/// <summary>
		/// Writes a UInt32 value to unmanaged memory.
		/// </summary>
		/// <param name="ptr">The base address in unmanaged memory from which to write.</param>
		/// <param name="ofs">An additional byte offset, added to the ptr parameter before writing.</param>
		/// <param name="val">The value to write.</param>
		[CLSCompliant(false)]
		public static void WriteUInt32(IntPtr ptr, int ofs, uint val)
		{
			byte[] data = BitConverter.GetBytes(val);
			Marshal.Copy(data, 0, new IntPtr(ptr.ToInt32() + ofs), data.Length);
		}

		#endregion

		#region UInt16

		/// <summary>
		/// Writes a 16-bit unsigned integer value to unmanaged memory.
		/// </summary>
		/// <param name="ptr">The base address in unmanaged memory from which to write.</param>
		/// <param name="ofs">An additional byte offset, added to the ptr parameter before writing.</param>
		/// <param name="val">The value to write.</param>
		[CLSCompliant(false)]
		public static void WriteUInt16(IntPtr ptr, int ofs, UInt16 val)
		{
			byte[] data = BitConverter.GetBytes(val);
			Marshal.Copy(data, 0, new IntPtr(ptr.ToInt32() + ofs), data.Length);
		}

		#endregion

		#region String

		#region ANSI String

		/// <summary>
		/// Copies the contents of a managed <see cref="System.String"/> into unmanaged memory, converting into ANSI format as it copies.
		/// </summary>
		/// <param name="s">A managed string to be copied. </param>
		/// <returns>The address, in unmanaged memory, to where s was copied, or 0 if a null reference (Nothing in Visual Basic) string was supplied.</returns>
		public static IntPtr StringToHGlobalAnsi(string s)
		{
			if(s == null)
				return IntPtr.Zero;

            byte[] data = Encoding.ASCII.GetBytes(s + '\0');
            
			IntPtr ptr = Marshal.AllocHGlobal(data.Length);

			Marshal.Copy(data, 0, ptr, data.Length);

			return ptr;
		}

		#endregion

		#region Unicode String		
		/// <summary>
		/// Copies the contents of a managed <see cref="System.String"/> into unmanaged memory.
		/// </summary>
		/// <param name="s">A managed string to be copied.</param>
		/// <returns>The address, in unmanaged memory, to where s was copied, or 0 if a null reference (Nothing in Visual Basic) string was supplied.</returns>
		public static IntPtr StringToHGlobalUni(string s)
		{
			if(s == null)
				return IntPtr.Zero;

            byte[] data = Encoding.Unicode.GetBytes(s + '\0');
            			
			IntPtr ptr = Marshal.AllocHGlobal(data.Length);

			Marshal.Copy(data, 0, ptr, data.Length);

			return ptr;
		}

		#endregion

		#endregion

		#region Char

		/// <summary>
		/// Writes a single char value to unmanaged memory.   
		/// </summary>
		/// <param name="ptr">The address in unmanaged memory from which to write. </param>
		/// <param name="ofs">The offset of the char from the ptr.</param>
		/// <param name="val">The value to write. </param>
		public static void WriteChar(IntPtr ptr, int ofs, char val)
		{
			byte[] data = BitConverter.GetBytes(val);
			Marshal.Copy(data, 0, new IntPtr(ptr.ToInt32() + ofs), data.Length);
		}

		#endregion

		#region Byte[]

		/// <summary>
		/// Writes a byte array to unmanaged memory.   
		/// </summary>
		/// <param name="ptr">The address in unmanaged memory from which to write. </param>
		/// <param name="ofs">The offset of the byte array from the ptr.</param>
		/// <param name="val">The value to write. </param>
		public static void WriteByteArray(IntPtr ptr, int ofs, byte[] val)
		{
			Marshal.Copy(val, 0, new IntPtr(ptr.ToInt32() + ofs), val.Length);
		}

		#endregion

		#region UInt64

		/// <summary>
		/// Writes a 64-bit unsigned integer value to unmanaged memory.
		/// </summary>
		/// <param name="ptr">The address in unmanaged memory from which to write.</param>
		/// <param name="ofs">An additional byte offset, added to the ptr parameter before writing.</param>
		/// <param name="val">The value to write.</param>
		[CLSCompliant(false)]
		public static void WriteUInt64(IntPtr ptr, int ofs, UInt64 val)
		{
			byte[] data = BitConverter.GetBytes(val);
			Marshal.Copy(data, 0, new IntPtr(ptr.ToInt32() + ofs), data.Length);
		}

		#endregion

		#region Bool

		/// <summary>
		/// Writes a bool value to unmanaged memory.   
		/// </summary>
		/// <param name="ptr">The address in unmanaged memory from which to write. </param>
		/// <param name="ofs">The offset of the bool from the ptr.</param>
		/// <param name="val">The value to write. </param>
		public static void WriteBool(IntPtr ptr, int ofs, bool val)
		{
			byte[] data = BitConverter.GetBytes(val);
			Marshal.Copy(data, 0, new IntPtr(ptr.ToInt32() + ofs), data.Length);
		}

		#endregion

		#endregion


		#region Memory Functions

		private static bool IsNotWin32Atom(IntPtr ptr)
		{
			long b;
			b = (long)ptr;
			return (((long) 0) != (b & (long)Marshal2.HIWORDMASK)); 
		}
		
		/// <summary>
		/// Converts a time_t value to a DateTime value.
		/// </summary>
		/// <param name="time_t">The time_t value to convert.</param>
		/// <returns>A DateTime value equivalent to the time_t suppled.</returns>
		[CLSCompliant(false)]
		public static DateTime Time_tToDateTime(uint time_t) 
		{
			long win32FileTime = 10000000*(long)time_t + 116444736000000000;
			return DateTime.FromFileTimeUtc(win32FileTime);
		}

		

		/// <summary>
		/// Returns the length of the string at the pointer
		/// </summary>
		/// <param name="ptr">The pointer to the string to measure.</param>
		/// <returns>The length of the string at the pointer.</returns>
		private static int lstrlenW(IntPtr ptr)
		{
			return String_wcslen(ptr);
		}

		#endregion

	
		#region GetHINSTANCE
		/// <summary>
		/// Returns the instance handle (HINSTANCE) for the specified module.
		/// </summary>
		/// <param name="m">The <see cref="System.Reflection.Module"/> whose HINSTANCE is desired.</param>
		/// <returns>The HINSTANCE for m; -1 if the module does not have an HINSTANCE.</returns>
		public static IntPtr GetHINSTANCE(System.Reflection.Module m )
		{
			IntPtr hinst = IntPtr.Zero;

			if(m.Assembly == System.Reflection.Assembly.GetCallingAssembly())
			{
				hinst = NativeMethods.GetModuleHandle(null);
			}
			else
			{
                hinst = NativeMethods.GetModuleHandle(m.Assembly.GetName().CodeBase);
			}

			if(hinst == IntPtr.Zero)
			{
				return new IntPtr(-1);
			}
			else
			{
				return hinst;
			}
		}
		#endregion


        /// <summary>
        /// Sets a region of unmanaged memory to a specified value
        /// </summary>
        /// <param name="destination">IntPtr address of the start of the region to set</param>
        /// <param name="value">The value to set for each byte in the reagion</param>
        /// <param name="length">Number of bytes to set</param>
        /// <exception cref="ArgumentException">Thrown if writing to destination for length bytes would be an invalid memory access</exception>
        public static void SetMemory(IntPtr destination, byte value, int length)
        {
            SetMemory(destination, value, length, true);
        }

        /// <summary>
        /// Sets a region of unmanaged memory to a specified value
        /// </summary>
        /// <param name="destination">IntPtr address of the start of the region to set</param>
        /// <param name="value">The value to set for each byte in the reagion</param>
        /// <param name="length">Number of bytes to set</param>
        /// <param name="boundsCheck">when true the function verifies that the requiested write operation is safe</param>
        /// <exception cref="ArgumentException">Thrown if writing to destination for length bytes would be an invalid memory access</exception>
        public static void SetMemory(IntPtr destination, byte value, int length, bool boundsCheck)
        {
            if ((boundsCheck) && (IsBadWritePtr(destination, length) != 0))
            {
                throw new ArgumentException("Unsafe SetMemory request");
            }
            memset(destination, (int)value, length);
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to another unmanaged memory pointer
        /// </summary>
        /// <param name="source">The memory pointer to copy from.</param>
        /// <param name="destination">The memory pointer to copy to.</param>
        /// <param name="length">The number of bytes to copy</param>
        /// <exception cref="ArgumentException">Thrown if either the requested write or read for length bytes would be an invalid memory access</exception>
        public static void Copy(IntPtr source, IntPtr destination, int length)
        {
            System.Runtime.InteropServices.Marshal.Copy(new IntPtr(0), new int[] { 0 }, 0, 0);
            Copy(source, destination, length, true);
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to another unmanaged memory pointer
        /// </summary>
        /// <param name="source">The memory pointer to copy from.</param>
        /// <param name="destination">The memory pointer to copy to.</param>
        /// <param name="length">The number of bytes to copy</param>
        /// <param name="boundsCheck">When true the function verifies that the requiested write and read operations are safe</param>
        /// <exception cref="ArgumentException">Thrown if either the requested write or read for length bytes would be an invalid memory access</exception>
        public static void Copy(IntPtr source, IntPtr destination, int length, bool boundsCheck)
        {
            if (boundsCheck)
            {
                if((IsBadReadPtr(source, length) != 0) || (IsBadWritePtr(destination, length) != 0))
                {
                    throw new ArgumentException("Unsafe SetMemory request");
                }
            }

            memcpy(destination, source, length);
        }

        /// <summary>
        /// Checks to determine if a write to an unmanaged memory pointer for a specied number of bytes is a safe operation
        /// </summary>
        /// <param name="destination">Unmanaged memory pointer to check</param>
        /// <param name="length">Number of bytes to check</param>
        /// <returns>Returns true if a write of the specifed length is safe, otherwise false</returns>
        public static bool IsSafeToWrite(IntPtr destination, int length)
        {
            return (IsBadWritePtr(destination, length) == 0);
        }

        /// <summary>
        /// Checks to determine if a read from an unmanaged memory pointer for a specied number of bytes is a safe operation
        /// </summary>
        /// <param name="source">Unmanaged memory pointer to check</param>
        /// <param name="length">Number of bytes to check</param>
        /// <returns>Returns true if a read from the specifed length is safe, otherwise false</returns>
        public static bool IsSafeToRead(IntPtr source, int length)
        {
            return (IsBadReadPtr(source, length) == 0);
        }

        [DllImport("coredll.dll", EntryPoint = "memset", SetLastError = true)]
        private static extern IntPtr memcpy(IntPtr dest, IntPtr src, int size);

        [DllImport("coredll.dll", EntryPoint = "memset", SetLastError = true)]
        private static extern IntPtr memset(IntPtr dest, int c, int count);

        [DllImport("coredll.dll", EntryPoint = "IsBadWritePtr", SetLastError = true)]
        private static extern int IsBadWritePtr(IntPtr lp, int ucb);

        [DllImport("coredll.dll", EntryPoint = "IsBadWritePtr", SetLastError = true)]
        private static extern int IsBadReadPtr(IntPtr lp, int ucb);

		#region API Prototypes

		[DllImport("mscoree.dll",EntryPoint="#1",SetLastError=true)]
		static extern int String_wcslen(IntPtr pws);

		#endregion
	}
}
