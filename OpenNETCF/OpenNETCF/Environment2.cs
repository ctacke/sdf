using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using OpenNETCF.Win32;

namespace OpenNETCF
{
	/// <summary>
	/// Extends the functionality of <see cref="T:System.Environment"/>
	/// </summary>
	/// <seealso cref="T:System.Environment">System.Environment Class</seealso>
	public static class Environment2
	{
		//maximum supported length of a file path
		private const int MaxPath = 260;

		#region New Line
		/// <summary>
		/// Gets the newline string defined for this environment.
		/// </summary>
		/// <value>A string containing "\r\n".</value>
		/// <remarks>The property value is a constant customized specifically for the current platform.
		/// This value is automatically appended to text when using WriteLine methods, such as <see cref="M:T:System.Console.WriteLine(System.String)">Console.WriteLine</see>.</remarks>
		/// <seealso cref="P:System.Environment.NewLine">System.Environment.NewLine Property</seealso>
		public static string NewLine
		{
			get
			{
				return "\r\n";
			}
		}
		#endregion

		#region System Directory
		/// <summary>
		/// Gets the fully qualified path of the system directory.
		/// </summary>
		/// <value>A string containing a directory path.</value>
		/// <remarks>An example of the value returned is the string "\Windows".</remarks>
		/// <seealso cref="P:System.Environment.SystemDirectory">System.Environment.SystemDirectory Property</seealso>
		public static string SystemDirectory
		{
			get
			{
				try
				{
					return GetFolderPath(SpecialFolder.Windows);
				}
				catch
				{
					return "\\Windows";
				}
			}
		}
		#endregion

		#region Get Folder Path
		/// <summary>
		/// Gets the path to the system special folder identified by the specified enumeration.
		/// </summary>
		/// <param name="folder">An enumerated constant that identifies a system special folder.</param>
		/// <returns>The path to the specified system special folder, if that folder physically exists on your computer; otherwise, the empty string ("").
		/// A folder will not physically exist if the operating system did not create it, the existing folder was deleted, or the folder is a virtual directory, such as My Computer, which does not correspond to a physical path.</returns>
		/// <seealso cref="M:System.Environment.GetFolderPath(System.Environment.SpecialFolder)">System.Environment.GetFolderPath Method</seealso>
		public static string GetFolderPath(SpecialFolder folder)
		{
            /*//check valid enum member
            if (!Enum.IsDefined(typeof(Environment.SpecialFolder), folder))
            {
                throw new OpenNETCF.ComponentModel.InvalidEnumArgumentException("folder",(int)folder, typeof(SpecialFolder));
            }*/

			StringBuilder path = new StringBuilder(MaxPath + 2);

            NativeMethods.SHGetSpecialFolderPath(IntPtr.Zero, path, (int)folder, 0);

			return path.ToString();
		}

	
		#endregion

		#region Special Folder
		/// <summary>
		/// Specifies enumerated constants used to retrieve directory paths to system special folders.
		/// </summary>
		/// <remarks>Not all platforms support all of these constants.</remarks>
		/// <seealso cref="T:System.Environment.SpecialFolder">System.Environment.SpecialFolder Enumeration</seealso>
		public enum SpecialFolder
		{
			// <summary>
            // The logical Desktop rather than the physical file system location.
			// </summary>
			//Desktop		= 0x00,
			/// <summary>
			/// The directory that contains the user's program groups.
			/// </summary>
			Programs		= 0x02,       
			// <summary>
			// control panel icons
			// </summary>
			//Controls		= 0x03,
			// <summary>
			// printers folder
			// </summary>
			//Printers		= 0x04,
			/// <summary>
			/// The directory that serves as a common repository for documents.
			/// </summary>
			Personal		= 0x05,
            /// <summary>
            /// The "My Documents" folder.
            /// </summary>
            MyDocuments = 0x5,
			/// <summary>
			/// The directory that serves as a common repository for the user's favorite items.
			/// </summary>
			Favorites		= 0x06,	
			/// <summary>
			/// The directory that corresponds to the user's Startup program group.
            /// The system starts these programs whenever a user starts Windows CE.
			/// </summary>
			Startup			= 0x07,
			/// <summary>
			/// The directory that contains the user's most recently used documents.
            /// <para><b>Not supported in Windows Mobile.</b></para>
			/// </summary>
			Recent			= 0x08,	
			// <summary>
			// The directory that contains the Send To menu items.
			// </summary>
			//SendTo			= 0x09,
			// <summary>
			// Recycle bin.
			// </summary>
			//RecycleBin		= 0x0A,
			/// <summary>
			/// The directory that contains the Start menu items.
			/// </summary>
			StartMenu		= 0x0B, 
            /// <summary>
            /// The "My Music" folder.
            /// <para><b>Supported only on Windows Mobile.</b></para>
            /// </summary>
            MyMusic = 0xd,
            // <summary>
            // The "My Video" folder.
            // <para><b>Supported only on Windows Mobile.</b></para>
            // </summary>
            //MyVideo = 0xe,
			/// <summary>
			/// The directory used to physically store file objects on the desktop.
            /// Do not confuse this directory with the desktop folder itself, which is a virtual folder.
            /// <para><b>Not supported in Windows Mobile.</b></para>
			/// </summary>
			DesktopDirectory = 0x10,
			// <summary>
			// The "My Computer" folder.
			// </summary>
			//MyComputer		= 0x11,
			// <summary>
			// Network Neighbourhood
            // <para><b>Not supported in Windows Mobile.</b></para></summary>
			//NetworkNeighborhood = 0x12,
			/// <summary>
			/// The Fonts folder.
			/// </summary>
			Fonts			= 0x14,
			/// <summary>
			/// The directory that serves as a common repository for application-specific data for the current user.
			/// </summary>
			ApplicationData	= 0x1a,
			/// <summary>
			/// The Windows folder.
			/// </summary>
			Windows			= 0x24,
			/// <summary>
			/// The program files directory.
			/// </summary>
			ProgramFiles	= 0x26,
            /// <summary>
            /// The "My Pictures" folder.
            /// <para><b>Supported only on Windows Mobile.</b></para>
            /// </summary>
            MyPictures = 0x27,
		}
		#endregion

		#region Get Logical Drives
		/// <summary>
		/// Returns an array of string containing the names of the logical drives on the current computer.
		/// </summary>
		/// <returns>An array of string where each element contains the name of a logical drive.</returns>
		public static string[] GetLogicalDrives()
		{
			//storage cards are directories with the temporary attribute
			System.IO.FileAttributes attrStorageCard = System.IO.FileAttributes.Directory | System.IO.FileAttributes.Temporary;

			ArrayList drives = new ArrayList();

			drives.Add("\\");

			DirectoryInfo rootDir = new DirectoryInfo(@"\");
			
			foreach(DirectoryInfo di in rootDir.GetDirectories() )
			{
				//if directory and temporary
				if ( (di.Attributes & attrStorageCard) == attrStorageCard )
				{
					//add to collection of storage cards
					drives.Add(di.Name);
				}
			}
			return (string[])drives.ToArray(typeof(string));

		}
		#endregion

		#region Machine Name
		/// <summary>
		/// Gets the name of this local device.
		/// </summary>
		public static string MachineName
		{
			get
			{
				string machineName = "";

				try
				{
					RegistryKey ident = Registry.LocalMachine.OpenSubKey("Ident");
					machineName = ident.GetValue("Name").ToString();
					ident.Close();
				}
				catch
				{
					throw new PlatformNotSupportedException();
				}
				
				return machineName;
			}
		}
		#endregion

		#region User Name
		/// <summary>
		/// Gets the user name of the person who started the current thread.
		/// </summary>
		/// <remarks>Supported only on Windows Mobile platforms.</remarks>
		public static string UserName
		{
			get
			{
				string userName = string.Empty;

				try
				{
					RegistryKey ownerKey = Registry.CurrentUser.OpenSubKey("ControlPanel\\Owner");
                    
                    //new style string registry key = smartphone and v5
                    userName = (string)ownerKey.GetValue("Name",string.Empty);
                    if(userName!=string.Empty)
                    {
                        return userName;
                    }

                    //old style pocket pc bytes
					byte[] ownerData = (byte[])ownerKey.GetValue("Owner", null);
                    if (ownerData != null)
                    {
                        userName = System.Text.Encoding.Unicode.GetString(ownerData, 0, 72);
                        userName = userName.Substring(0, userName.IndexOf("\0"));
                    }
				}
				catch
				{
				}

				return userName;
			}
		}
		#endregion

		#region Sdf Version
		/// <summary>
		/// Gets a <see cref="System.Version"/> object that contains the version of the Smart Device Framework in use. 
		/// </summary>
		public static Version SdfVersion
		{
			get
			{
				return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			}
		}
		#endregion

		#region Working Set
		/// <summary>
		/// Gets the amount of physical memory mapped to the process context.
		/// </summary>
		public static long WorkingSet
		{
			get
			{
				//wrapper to GlobalMemoryStatus
                NativeMethods.MemoryStatus ms = new NativeMethods.MemoryStatus();
                NativeMethods.GlobalMemoryStatus(out ms);
				return Convert.ToInt64(ms.AvailablePhysical);
			}
		}

		#endregion


		#region Wrappers to System.Environment

		/// <summary>
		/// Gets an <see cref="System.OperatingSystem"/> object that contains the current platform identifier and version number. 
		/// </summary>
		public static OperatingSystem OSVersion
		{
			get
			{
				return System.Environment.OSVersion;
			}
		}

		/// <summary>
		/// Gets the number of milliseconds elapsed since the system started.
		/// </summary>
		public static int TickCount
		{
			get
			{
				return System.Environment.TickCount;
			}
		}

		/// <summary>
		/// Gets a <see cref="System.Version"/> object that describes the major, minor, build, and revision numbers of the common language runtime.
		/// </summary>
		public static Version Version
		{
			get
			{
				return System.Environment.Version;
			}
		}
		#endregion
	}
}
