using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenNETCF.IO
{
    /// <summary>
    /// Provides access to information on a drive.
    /// </summary>
    /// <remarks>This class models a drive and provides methods and properties to query for drive information.
    /// Use <see cref="DriveInfo"/> to determine what drives are available, and the capacity and available free space on the drive.</remarks>
    public class DriveInfo
    {
        private string m_root;
        private STORAGE_IDENTIFICATION m_storageID;

        /// <summary>
        /// Provides access to information on the specified drive.
        /// </summary>
        /// <param name="driveName"></param>
        /// <remarks>Use this class to obtain information on drives.
        /// The drive name must be a valid Windows CE volume path e.g. "\Storage Card".
        /// You cannot use this method to obtain information on drive names that are a null reference (Nothing in Visual Basic) or use UNC (\\server\share) paths.</remarks>
        /// <exception cref="ArgumentNullException">The drive name cannot be a null reference (Nothing in Visual Basic).</exception>
        public DriveInfo(string driveName)
        {
            if (string.IsNullOrEmpty(driveName)) throw new ArgumentNullException("driveName");
            if (!Directory.Exists(driveName)) throw new ArgumentException("driveName does not exist");

            this.m_root = driveName;

            bool success = NativeMethods.GetDiskFreeSpaceEx(driveName, ref available, ref size, ref total);

            if (!success)
            {
                throw new System.ComponentModel.Win32Exception();
            }

            IntPtr invalidHandle = new IntPtr(-1);

            IntPtr hFile = NativeMethods.CreateFile(string.Format("{0}\\Vol:", driveName), NativeMethods.GENERIC_READ, 0, 0, NativeMethods.OPEN_EXISTING, 0, 0);

            if (hFile == invalidHandle) return;

            try
            {
                byte[] data = new byte[512];
                int returned;
                int result = NativeMethods.DeviceIoControl(hFile, IOCTL_DISK_GET_STORAGEID, IntPtr.Zero, 0, data, data.Length, out returned, IntPtr.Zero);
                if (result != 0)
                {
                    m_storageID = STORAGE_IDENTIFICATION.FromBytes(data);
                }
            }
            finally
            {
                NativeMethods.CloseHandle(hFile);
            }

        }

        /// <summary>
        /// Gets the root directory of a drive.
        /// </summary>
        public DirectoryInfo RootDirectory
        {
            get { return new DirectoryInfo(m_root); }
        }

        /// <summary>
        /// Gets the manufacturer ID of the volume (if available)
        /// </summary>
        public string ManufacturerID
        {
            get
            {
                if (m_storageID == null) return null;
                return m_storageID.ManufacturerID;
            }
        }

        /// <summary>
        /// Gets the serial number of the volume (if available)
        /// </summary>
        public string SerialNumber
        {
            get
            {
                if (m_storageID == null) return null;
                return m_storageID.SerialNumber;
            }
        }

        private long available;
        /// <summary>
        /// Indicates the amount of available free space on a drive.
        /// </summary>
        public long AvailableFreeSpace
        {
            get { return available; }
        }

        private long total;
        /// <summary>
        /// Gets the total amount of free space available on a drive.
        /// </summary>
        public long TotalFreeSpace
        {
            get { return total; }
        }

        private long size;
        /// <summary>
        /// Gets the total size of storage space on a drive.
        /// </summary>
        public long TotalSize
        {
            get { return size; }
        }

        /// <summary>
        /// Returns a drive name as a string.
        /// </summary>
        /// <returns>The name of the drive.</returns>
        public override string ToString()
        {
            return m_root;
        }

        #region Get Drives
        /// <summary>
        /// Retrieves the drive names of all logical drives on a computer.
        /// </summary>
        /// <returns></returns>
        public static DriveInfo[] GetDrives()
        {
            //storage cards are directories with the temporary attribute
            System.IO.FileAttributes attrStorageCard = System.IO.FileAttributes.Directory | System.IO.FileAttributes.Temporary;

            ArrayList drives = new ArrayList();

            //add the root (Object Store)
            drives.Add(new DriveInfo("\\"));

            DirectoryInfo rootDir = new DirectoryInfo("\\");

            //add all removable drives
            foreach (DirectoryInfo di in rootDir.GetDirectories())
            {
                //if directory and temporary
                if ((di.Attributes & attrStorageCard) == attrStorageCard)
                {
                    //add to collection of storage cards
                    drives.Add(new DriveInfo("\\" + di.Name));
                }
            }
            return (DriveInfo[])drives.ToArray(typeof(DriveInfo));
        }
        #endregion

        //#define CTL_CODE(t,f,m,a) (((t)<<16)|((a)<<14)|((f)<<2)|(m))
        //#define FILE_DEVICE_DISK	7
        //#define METHOD_BUFFERED	0
        //#define FILE_ANY_ACCESS     0
        //
        //#define IOCTL_DISK_BASE           FILE_DEVICE_DISK 
        //#define IOCTL_DISK_GET_STORAGEID  CTL_CODE(IOCTL_DISK_BASE, 0x709, METHOD_BUFFERED, FILE_ANY_ACCESS) 

        // ((7)<<16) |((0)<<14) | ((0x709)<<2) | (0)
        //  0x70000  |    0     |   0x1C24     |  0
        private const uint IOCTL_DISK_GET_STORAGEID = 0x71c24;

        //#define MANUFACTUREID_INVALID     0x01 
        //#define SERIALNUM_INVALID         0x02 

        //// This structure is only defined in Platform Builder, so we have to 
        //// redefine it here 
        //typedef struct _STORAGE_IDENTIFICATION 
        //{ 
        //        DWORD    dwSize; 
        //        DWORD    dwFlags; 
        //        DWORD    dwManufactureIDOffest; 
        //        DWORD    dwSerialNumOffset; 
        //} STORAGE_IDENTIFICATION, *PSTORAGE_IDENTIFICATION; 

        private class STORAGE_IDENTIFICATION
        {
            private byte[] m_data;

            private STORAGE_IDENTIFICATION(byte[] data)
            {
                int size = BitConverter.ToInt32(data, 0);

                m_data = new byte[size];
                Buffer.BlockCopy(data, 0, m_data, 0, size);
            }

            public int Flags
            {
                get { return BitConverter.ToInt32(m_data, 4); }
            }

            public string ManufacturerID
            {
                get
                {
                    int moffset = BitConverter.ToInt32(m_data, 8);
                    int snoffset = BitConverter.ToInt32(m_data, 12);

                    string id = Encoding.ASCII.GetString(m_data, moffset, snoffset - moffset);
                    return id.TrimEnd(new char[] { '\0' });
                }
            }

            public string SerialNumber
            {
                get
                {
                    int snoffset = BitConverter.ToInt32(m_data, 12);

                    string sn = Encoding.ASCII.GetString(m_data, snoffset, m_data.Length - snoffset);
                    return sn.TrimEnd(new char[] { '\0' });
                }
            }

            public static STORAGE_IDENTIFICATION FromBytes(byte[] data)
            {
                return new STORAGE_IDENTIFICATION(data);
            }

            private const uint GENERIC_READ = 0x80000000;
            private const uint OPEN_EXISTING = 3;

            [DllImport("coredll.dll", SetLastError = true)]
            static extern void CloseHandle(IntPtr hFile);

            [DllImport("coredll.dll", SetLastError = true)]
            static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

            [DllImport("coredll.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
            internal static extern int DeviceIoControl(
                IntPtr hDevice,
                uint dwIoControlCode,
                IntPtr lpInBuffer,
                int nInBufferSize,
                byte[] lpOutBuffer,
                int nOutBufferSize,
                out int lpBytesReturned,
                IntPtr lpOverlapped);
        }
    }
}
