using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using OpenNETCF.Win32;

namespace OpenNETCF.IO
{

    #region FileHelper
    /// <summary>
	/// Provides additional file related functionality.
	/// </summary>
	public class FileHelper
	{        
        
		private FileHelper()
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
        }

		/// <summary>
		/// Maximum length of Filepath string (in characters)
		/// </summary>
		public const int	MaxPath				= 260;
		

		/// <summary>
		/// Represents an invalid native operating system handle.
		/// </summary>
		public const int	InvalidHandle	= -1;

        #region Read All
        /// <summary>
        /// Opens a text file, reads all text from the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>A string containing all of the file.</returns>
        public static string ReadAllText(string path)
        {
            CheckPath(path);

            StreamReader sr = File.OpenText(path);
            string contents = sr.ReadToEnd();
            sr.Close();
            return contents;
        }

        /// <summary>
        /// Opens a file, reads all text from the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <param name="encoding">The encoding applied to the contents of the file.</param>
        /// <returns>A string containing all of the file.</returns>
        public static string ReadAllText(string path, System.Text.Encoding encoding)
        {
            CheckPath(path);

            StreamReader sr = new StreamReader(path, encoding);
            string contents = sr.ReadToEnd();
            sr.Close();
            return contents;
        }

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>A string array containing all of the file.</returns>
        public static string[] ReadAllLines(string path)
        {
            CheckPath(path);

            System.Collections.ArrayList al = new System.Collections.ArrayList();
            StreamReader sr = new StreamReader(path);
            string line = sr.ReadLine();
            while(!string.IsNullOrEmpty(line))
            {
                al.Add(line);
                line = sr.ReadLine();
            }
            sr.Close();
            return (string[])al.ToArray(typeof(string));
        }

        /// <summary>
        /// Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <param name="encoding">The encoding applied to the contents of the file.</param>
        /// <returns>A string array containing all of the file.</returns>
        public static string[] ReadAllLines(string path, System.Text.Encoding encoding)
        {
            CheckPath(path);

            System.Collections.ArrayList al = new System.Collections.ArrayList();
            StreamReader sr = new StreamReader(path, encoding);
            string line = sr.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                al.Add(line);
                line = sr.ReadLine();
            }
            sr.Close();
            return (string[])al.ToArray(typeof(string));
        }
#endregion

        #region Write All
        /// <summary>
        /// Creates a new file, writes the specified string array to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string to write to the file.</param>
        public static void WriteAllText(string path, string contents)
        {
            CheckPath(path);

            StreamWriter sw = new StreamWriter(path, false);
            sw.Write(contents);
            sw.Close();
        }

        /// <summary>
        /// Creates a new file, writes the specified string array to the file using the specified encoding, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string to write to the file.</param>
        /// <param name="encoding">An <see cref="System.Text.Encoding"/> object that represents the encoding to apply to the string array.</param>
        public static void WriteAllText(string path, string contents, System.Text.Encoding encoding)
        {
            CheckPath(path);

            StreamWriter sw = new StreamWriter(path, false, encoding);
            sw.Write(contents);
            sw.Close();
        }

        /// <summary>
        /// Creates a new file, write the specified string array to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string array to write to the file.</param>
        public static void WriteAllLines(string path, string[] contents)
        {
            CheckPath(path);

            StreamWriter sw = new StreamWriter(path, false);
            foreach (string line in contents)
            {
                sw.WriteLine(line);
            }
            sw.Close();
        }

        /// <summary>
        /// Creates a new file, writes the specified string array to the file using the specified encoding, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string array to write to the file.</param>
        /// <param name="encoding">An <see cref="System.Text.Encoding"/> object that represents the character encoding applied to the string array.</param>
        public static void WriteAllLines(string path, string[] contents, System.Text.Encoding encoding)
        {
            CheckPath(path);

            StreamWriter sw = new StreamWriter(path, false, encoding);
            foreach (string line in contents)
            {
                sw.WriteLine(line);
            }
            sw.Close();
        }
        #endregion

        #region FileAttributes Get/Set
        /// <summary>
		/// Gets the FileAttributes of the file on the path.
		/// <seealso cref="FileAttributes"/>
		/// </summary>
		/// <param name="path">The path to the file.</param>
		/// <returns>The FileAttributes of the file on the path, or -1 if the path or file is not found.</returns>
		/// <exception cref="ArgumentException">path is empty, contains only white spaces, or contains invalid characters.</exception>
		/// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
		/// <exception cref="NotSupportedException">path is in an invalid format.</exception>
		/// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
		public static FileAttributes GetAttributes(string path)
		{
            CheckPath(path);

            uint attr = NativeMethods.GetFileAttributes(path);

			if(attr ==  0xFFFFFFFF)
			{
				int e = Marshal.GetLastWin32Error();
				if((e == 2) || (e == 3))
				{
					throw new FileNotFoundException();
				}
				else
				{
					throw new Win32Exception(e,"Unmanaged Error: " + e);
				}
			}
			return (FileAttributes)attr;
		}

		/// <summary>
		/// Sets the specified FileAttributes of the file on the specified path.
		/// </summary>
		/// <param name="path">The path to the file.</param>
		/// <param name="fileAttributes"><seealso cref="FileAttributes"/>The desired FileAttributes, such as Hidden, ReadOnly, Normal, and Archive.</param>
		public static void SetAttributes(string path, FileAttributes fileAttributes)
		{
            CheckPath(path);
			
			if(! NativeMethods.SetFileAttributes(path, (uint)fileAttributes))
			{
				int e = Marshal.GetLastWin32Error();
				if((e == 2) || (e == 3))
				{
					throw new FileNotFoundException();
				}
				else
				{
					throw new Exception("Unmanaged Error: " + e);
				}
			}
		}
		#endregion

        //Checks a path for validity before passing to native APIs
        internal static void CheckPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException();

            if (path.Length > MaxPath)
                throw new PathTooLongException();

            if (path.Trim().Length == 0)
                throw new ArgumentException();
        }

		#region FileTime Sets
		/// <summary>
		/// Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
		/// <param name="path">The file for which to set the creation date and time information.</param>
		/// <param name="creationTimeUtc">A DateTime containing the value to set for the creation date and time of path. This value is expressed in UTC time.</param>
		public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
		{
			SetCreationTime(path, creationTimeUtc.ToLocalTime());
		}

		/// <summary>
		/// Sets the date and time, in local time, that the file was created.</summary>
		/// <param name="path">The file for which to set the creation date and time information.</param>
		/// <param name="creationTime">A DateTime containing the value to set for the creation date and time of path. This value is expressed in local time.</param>
		public static void SetCreationTime(string path, DateTime creationTime)
		{
            byte[] ft = BitConverter.GetBytes(creationTime.ToFileTime());

			//FILETIME	ft;
			IntPtr		hFile	= IntPtr.Zero;

            CheckPath(path);
			

			hFile = CreateFile(path, FileAccess.Write, FileShare.Write, FileCreateDisposition.OpenExisting, 0);

			if((int)hFile == InvalidHandle)
			{
				int e = Marshal.GetLastWin32Error();
				if((e == 2) || (e == 3))
				{
					throw new FileNotFoundException();
				}
				else
				{
                    throw new Win32Exception(e, "Unmanaged Error: " + e.ToString());
				}
			}

			//ft = new FILETIME(creationTime.ToFileTime());

            NativeMethods.SetFileTime(hFile, ft, null, null);

			CloseHandle(hFile);
		}

		/// <summary>
		/// Sets the date and time, in coordinated universal time (UTC), that the file was last accessed.</summary>
		/// <param name="path">The file for which to set the creation date and time information.</param>
		/// <param name="lastAccessTimeUtc">A DateTime containing the value to set for the last access date and time of path. This value is expressed in UTC time.</param>
		public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
		{
			SetLastAccessTime(path, lastAccessTimeUtc.ToLocalTime());
		}

		/// <summary>
		/// Sets the date and time, in local time, that the file was last accessed.</summary>
		/// <param name="path">The file for which to set the creation date and time information.</param>
		/// <param name="lastAccessTime">A DateTime containing the value to set for the last access date and time of path. This value is expressed in local time.</param>
		public static void SetLastAccessTime(string path, DateTime lastAccessTime)
		{
            byte[] ft = BitConverter.GetBytes(lastAccessTime.ToFileTime());
			//FILETIME	ft;
			IntPtr		hFile	= IntPtr.Zero;

            CheckPath(path);

			hFile = CreateFile(path, FileAccess.Write, FileShare.Write, FileCreateDisposition.OpenExisting, 0);

			if((int)hFile == InvalidHandle)
			{
				int e = Marshal.GetLastWin32Error();
				if((e == 2) || (e == 3))
				{
					throw new FileNotFoundException();
				}
				else
				{
                    throw new Win32Exception(e, "Unmanaged Error: " + e.ToString());
				}
			}

			//ft = new FILETIME(lastAccessTime.ToFileTime());

            NativeMethods.SetFileTime(hFile, null, ft, null);

			CloseHandle(hFile);
		}

		/// <summary>
		/// Sets the date and time, in coordinated universal time (UTC), that the file was last updated or written to.</summary>
		/// <param name="path">The file for which to set the creation date and time information.</param>
		/// <param name="lastWriteTimeUtc">A DateTime containing the value to set for the last write date and time of path. This value is expressed in UTC time.</param>
		public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
		{
            SetLastWriteTime(path, lastWriteTimeUtc.ToLocalTime());
		}

		/// <summary>
		/// Sets the date and time, in local time, that the file was last updated or written to.</summary>
		/// <param name="path">The file for which to set the creation date and time information.</param>
		/// <param name="lastWriteTime">A DateTime containing the value to set for the last write date and time of path. This value is expressed in local time.</param>
		public static void SetLastWriteTime(string path, DateTime lastWriteTime)
		{
            byte[] ft = BitConverter.GetBytes(lastWriteTime.ToFileTime());
			//FILETIME	ft;
			IntPtr		hFile	= IntPtr.Zero;

            CheckPath(path);

			hFile = CreateFile(path, FileAccess.Write, FileShare.Write, FileCreateDisposition.OpenExisting, 0);

			if((int)hFile == InvalidHandle)
			{
				int e = Marshal.GetLastWin32Error();
				if((e == 2) || (e == 3))
				{
					throw new FileNotFoundException();
				}
				else
				{
					throw new Win32Exception(e,"Unmanaged Error: " + e.ToString());
				}
			}

            NativeMethods.SetFileTime(hFile, null, null, ft);

			CloseHandle(hFile);
		}
		#endregion

		#region --------------- File API Calls ---------------

		#region Create File
		/// <summary>
		/// Wrapper around the CreateFile API
		/// </summary>
		/// <param name="fileName">Path to the file or CE port name</param>
        /// <param name="desiredFileAccess">Specifies the type of access to the object. An application can obtain read access, write access, read-write (All) access.</param>
		/// <param name="shareMode">Specifies how the object can be shared.</param>
		/// <param name="creationDisposition">Specifies which action to take on files that exist, and which action to take when files do not exist.</param>
		/// <param name="flagsAndAttributes">Specifies the file attributes and flags for the file.</param>
		/// <returns>Handle to the created file</returns>
		[CLSCompliant(false)]
		public static IntPtr CreateFile(string fileName,
			FileAccess desiredFileAccess,
			FileShare shareMode,
			FileCreateDisposition creationDisposition,
			int flagsAndAttributes)
		{
            FileAccess2 desiredAccess;
            switch (desiredFileAccess)
            {
                case FileAccess.Read:
                    desiredAccess = FileAccess2.Read;
                    break;
                case FileAccess.ReadWrite:
                    desiredAccess = FileAccess2.Read | FileAccess2.Write;
                    break;
                case FileAccess.Write:
                    desiredAccess = FileAccess2.Write;
                    break;
                default:
                    desiredAccess = FileAccess2.Execute; //it will never get here and in fact Execute is never currently used. Left as a back door.
                    break;
            }

			IntPtr hFile = IntPtr.Zero;

			hFile = NativeMethods.CreateFile(fileName, (uint)desiredAccess, (uint)shareMode, 0, (uint)creationDisposition, (uint)flagsAndAttributes, 0);

			if((int)hFile == InvalidHandle)
			{
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Failed to Create File");
			}

			return hFile;
		}
		#endregion

		#region Write File
		/// <summary>
		/// This function writes data to a file.
		/// </summary>
		/// <remarks> WriteFile starts writing data to the file at the position indicated by the file pointer. After the write operation has been completed, the file pointer is adjusted by the number of bytes actually written.</remarks>
		/// <param name="hFile">Handle to the file to be written to. The file handle must have been created with GENERIC_WRITE access to the file.</param>
		/// <param name="lpBuffer">Buffer containing the data to be written to the file.</param>
		/// <param name="nNumberOfBytesToWrite">Number of bytes to write to the file.</param>
		/// <param name="lpNumberOfBytesWritten">Number of bytes written by this function call. WriteFile sets this value to zero before doing any work or error checking.</param>
		public static void WriteFile(IntPtr hFile, byte[] lpBuffer, int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten)
		{
			bool b;

			CheckHandle(hFile);

			b = NativeMethods.WriteFile(hFile, lpBuffer, nNumberOfBytesToWrite, ref lpNumberOfBytesWritten, IntPtr.Zero);

			if(!b)
			{
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Write Failed");
			}
		}
		#endregion

		#region ReadFile
		/// <summary>
		/// This function reads data from a file, starting at the position indicated by the file pointer. After the read operation has been completed, the file pointer is adjusted by the number of bytes actually read.
		/// </summary>
		/// <param name="hFile">Handle to the file to be read. The file handle must have been created with GENERIC_READ access to the file. This parameter cannot be a socket handle.</param>
		/// <param name="lpBuffer">Buffer that receives the data read from the file.</param>
		/// <param name="nNumberOfBytesToRead">Number of bytes to be read from the file.</param>
		/// <param name="lpNumberOfBytesRead">number of bytes read. ReadFile sets this value to zero before doing any work or error checking.</param>
		public static void ReadFile(IntPtr hFile, byte[] lpBuffer, int nNumberOfBytesToRead, ref int lpNumberOfBytesRead)
		{
			bool b;

			CheckHandle(hFile);

			b = NativeMethods.ReadFile(hFile, lpBuffer, nNumberOfBytesToRead, ref lpNumberOfBytesRead, IntPtr.Zero);

			if(!b)
			{
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Read Failed");
			}
		}

		#endregion

        private static void CheckHandle(IntPtr handle)
        {
            if ((handle == IntPtr.Zero) || (handle.ToInt32() == -1))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "File operation failed: invalid handle");
            }
        }

		#region CloseHandle
		/// <summary>
		/// This function closes an open object handle
		/// </summary>
		/// <param name="hObject">Object Handle, Could be any of the following Objects:- Communications device, Mutex, Database, Process, Event, Socket, File or Thread</param>
		public static void CloseHandle(IntPtr hObject)
		{
			bool b;

			CheckHandle(hObject);

			b = OpenNETCF.IO.NativeMethods.CloseHandle(hObject);

			if(!b)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to close file handle");
			}
		}
		#endregion

		#endregion ---------------------------------------------

    }
	#endregion
}
