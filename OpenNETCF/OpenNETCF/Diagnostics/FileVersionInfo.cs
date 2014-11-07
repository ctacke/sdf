using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace OpenNETCF.Diagnostics
{
	/// <summary>
	/// Provides version information for a physical file in storage memory.
	/// </summary>
	public sealed class FileVersionInfo
	{
		//the name of the file (sans path)
		private string m_filename;

		//the language independant version info
		private byte[] m_fixedversioninfo;

		#region Constructor
		private FileVersionInfo(string fileName)
		{
			//get the filename sans path
			m_filename = System.IO.Path.GetFileName(fileName);

			int handle = 0;
			int len = 0;

			//get size of version info
			len = GetFileVersionInfoSize(fileName, ref handle);

			if(len > 0)
			{
				//allocate buffer
				IntPtr buffer = Marshal.AllocHGlobal(len);
				//get version information
				if(GetFileVersionInfo(fileName, handle, len, buffer))
				{
					IntPtr fixedbuffer = IntPtr.Zero;
					int fixedlen = 0;
					//get language independant version info
					//this is a pointer within the main buffer so don't free it
					if(VerQueryValue(buffer, "\\", ref fixedbuffer, ref fixedlen))
					{
						//allocate managed memory
						m_fixedversioninfo = new byte[fixedlen];
						//copy to managed memory
						Marshal.Copy(fixedbuffer, m_fixedversioninfo, 0, fixedlen);
					}
					else
					{
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Error retrieving language independant version");
					}
				}
				else
				{
                    throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Error retrieving FileVersionInformation");
				}

				//free native buffer
				Marshal.FreeHGlobal(buffer);
			}

		}
		#endregion

		#region Get Version Info
		/// <summary>
		/// Returns a <see cref="FileVersionInfo"/> representing the version information associated with the specified file.
		/// </summary>
		/// <param name="fileName">The fully qualified path and name of the file to retrieve the version information for.</param>
		/// <returns>A <see cref="FileVersionInfo"/> containing information about the file.
		/// If the file information was not found, the <see cref="FileVersionInfo"/> contains only the name of the file requested.</returns>
		/// <exception cref="System.IO.FileNotFoundException">The file specified cannot be found.</exception>
		public static FileVersionInfo GetVersionInfo(string fileName)
		{
			//check if file exists first
			if(System.IO.File.Exists(fileName))
			{
				return new FileVersionInfo(fileName);
			}
			else
			{
				throw new System.IO.FileNotFoundException("The specified file was not found");
			}
		}
		#endregion

		#region File Name
		/// <summary>
		/// Gets the name of the file that this instance of <see cref="FileVersionInfo"/> describes.
		/// </summary>
		/// <value>The name of the file described by this instance of <see cref="FileVersionInfo"/>.</value>
		public string FileName
		{
			get
			{
				return m_filename;
			}
		}
		#endregion


		#region FileMajorPart
		/// <summary>
		/// Gets the major part of the version number.
		/// </summary>
		/// <value>A value representing the major part of the version number.</value>
		/// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
		/// A file version number is a 64-bit number that holds the version number for a file as follows: 
		/// <list type="bullet"><item>The first 16 bits are the <see cref="FileMajorPart"/> number.</item> 
		/// <item>The next 16 bits are the <see cref="FileMinorPart"/> number.</item> 
		/// <item>The third set of 16 bits are the <see cref="FileBuildPart"/> number.</item>
		/// <item>The last 16 bits are the <see cref="FilePrivatePart"/> number.</item></list>
		/// This property gets the first set of 16 bits.</remarks>
		public int FileMajorPart
		{
			get
			{
				return Convert.ToInt32(BitConverter.ToInt16(m_fixedversioninfo, 10));
			}
		}
		#endregion

		#region FileMinorPart
		/// <summary>
		/// Gets the minor part of the version number.
		/// </summary>
		/// <value>A value representing the minor part of the version number of the file.</value>
		/// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
		/// A file version number is a 64-bit number that holds the version number for a file as follows: 
		/// <list type="bullet"><item>The first 16 bits are the <see cref="FileMajorPart"/> number.</item> 
		/// <item>The next 16 bits are the <see cref="FileMinorPart"/> number.</item> 
		/// <item>The third set of 16 bits are the <see cref="FileBuildPart"/> number.</item>
		/// <item>The last 16 bits are the <see cref="FilePrivatePart"/> number.</item></list>
		/// This property gets the second set of 16 bits.</remarks>
		public int FileMinorPart
		{
			get
			{
				return Convert.ToInt32(BitConverter.ToInt16(m_fixedversioninfo, 8));
			}
		}
		#endregion

		#region FileBuildPart
		/// <summary>
		/// Gets the build number of the file.
		/// </summary>
		/// <value>A value representing the build number of the file.</value>
		/// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
		/// A file version number is a 64-bit number that holds the version number for a file as follows: 
		/// <list type="bullet"><item>The first 16 bits are the <see cref="FileMajorPart"/> number.</item> 
		/// <item>The next 16 bits are the <see cref="FileMinorPart"/> number.</item> 
		/// <item>The third set of 16 bits are the <see cref="FileBuildPart"/> number.</item>
		/// <item>The last 16 bits are the <see cref="FilePrivatePart"/> number.</item></list>
		/// This property gets the third set of 16 bits.</remarks>
		public int FileBuildPart
		{
			get
			{
				return Convert.ToInt32(BitConverter.ToInt16(m_fixedversioninfo, 14));
			}
		}
		#endregion

		#region FilePrivatePart
		/// <summary>
		/// Gets the file private part number.
		/// </summary>
		/// <value>A value representing the file private part number.</value>
		/// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
		/// A file version number is a 64-bit number that holds the version number for a file as follows: 
		/// <list type="bullet"><item>The first 16 bits are the <see cref="FileMajorPart"/> number.</item> 
		/// <item>The next 16 bits are the <see cref="FileMinorPart"/> number.</item> 
		/// <item>The third set of 16 bits are the <see cref="FileBuildPart"/> number.</item>
		/// <item>The last 16 bits are the <see cref="FilePrivatePart"/> number.</item></list>
		/// This property gets the last set of 16 bits.</remarks>
		public int FilePrivatePart
		{
			get
			{
				return Convert.ToInt32(BitConverter.ToInt16(m_fixedversioninfo, 12));
			}
		}
		#endregion


		#region ProductMajorPart
		/// <summary>
		/// Gets the major part of the version number for the product this file is associated with.
		/// </summary>
		/// <value>A value representing the major part of the product version number.</value>
		/// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
		/// A product version number is a 64-bit number that holds the version number for a product as follows: 
		/// <list type="bullet"><item>The first 16 bits are the <see cref="ProductMajorPart"/> number.</item> 
		/// <item>The next 16 bits are the <see cref="ProductMinorPart"/> number.</item> 
		/// <item>The third set of 16 bits are the <see cref="ProductBuildPart"/> number.</item>
		/// <item>The last 16 bits are the <see cref="ProductPrivatePart"/> number.</item></list>
		/// This property gets the first set of 16 bits.</remarks>
		public int ProductMajorPart
		{
			get
			{
				return Convert.ToInt32(BitConverter.ToInt16(m_fixedversioninfo, 18));
			}
		}
		#endregion

		#region ProductMinorPart
		/// <summary>
		/// Gets the minor part of the version number for the product the file is associated with.
		/// </summary>
		/// <value>A value representing the minor part of the product version number.</value>
		/// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
		/// A product version number is a 64-bit number that holds the version number for a product as follows: 
		/// <list type="bullet"><item>The first 16 bits are the <see cref="ProductMajorPart"/> number.</item> 
		/// <item>The next 16 bits are the <see cref="ProductMinorPart"/> number.</item> 
		/// <item>The third set of 16 bits are the <see cref="ProductBuildPart"/> number.</item>
		/// <item>The last 16 bits are the <see cref="ProductPrivatePart"/> number.</item></list>
		/// This property gets the second set of 16 bits.</remarks>
		public int ProductMinorPart
		{
			get
			{
				return Convert.ToInt32(BitConverter.ToInt16(m_fixedversioninfo, 16));
			}
		}
		#endregion

		#region ProductBuildPart
		/// <summary>
		/// Gets the build number of the product this file is associated with.
		/// </summary>
		/// <value>A value representing the build part of the product version number.</value>
		/// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
		/// A product version number is a 64-bit number that holds the version number for a product as follows: 
		/// <list type="bullet"><item>The first 16 bits are the <see cref="ProductMajorPart"/> number.</item> 
		/// <item>The next 16 bits are the <see cref="ProductMinorPart"/> number.</item> 
		/// <item>The third set of 16 bits are the <see cref="ProductBuildPart"/> number.</item>
		/// <item>The last 16 bits are the <see cref="ProductPrivatePart"/> number.</item></list>
		/// This property gets the third set of 16 bits.</remarks>
		public int ProductBuildPart
		{
			get
			{
				return Convert.ToInt32(BitConverter.ToInt16(m_fixedversioninfo, 22));
			}
		}
		#endregion

		#region ProductPrivatePart
		/// <summary>
		/// Gets the private part number of the product this file is associated with.
		/// </summary>
		/// <value>A value representing the private part of the product version number.</value>
		/// <remarks>Typically, a version number is displayed as "major number.minor number.build number.private part number".
		/// A product version number is a 64-bit number that holds the version number for a product as follows: 
		/// <list type="bullet"><item>The first 16 bits are the <see cref="ProductMajorPart"/> number.</item> 
		/// <item>The next 16 bits are the <see cref="ProductMinorPart"/> number.</item> 
		/// <item>The third set of 16 bits are the <see cref="ProductBuildPart"/> number.</item>
		/// <item>The last 16 bits are the <see cref="ProductPrivatePart"/> number.</item></list>
		/// This property gets the last set of 16 bits.</remarks>
		public int ProductPrivatePart
		{
			get
			{
				return Convert.ToInt32(BitConverter.ToInt16(m_fixedversioninfo, 20));
			}
		}
		#endregion


		#region IsDebug
		#endregion

		#region IsPatched
		#endregion

		#region IsPreRelease
		#endregion

		#region IsPrivateBuild
		#endregion

		#region IsSpecialBuild
		#endregion

		#region P/Invokes

		[DllImport("coredll.dll", EntryPoint="GetFileVersionInfo", SetLastError=true)]
		private static extern bool GetFileVersionInfo(string filename, int handle, int len, IntPtr buffer);

		[DllImport("coredll.dll", EntryPoint="GetFileVersionInfoSize", SetLastError=true)]
		private static extern int GetFileVersionInfoSize(string filename, ref int handle);

		[DllImport("coredll.dll", EntryPoint="VerQueryValue", SetLastError=true)]
		private static extern bool VerQueryValue(IntPtr buffer, string subblock, ref IntPtr blockbuffer, ref int len);

		#endregion
	}
}
