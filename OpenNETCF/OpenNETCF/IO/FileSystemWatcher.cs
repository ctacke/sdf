#region OpenNETCF Copyright Information
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.0                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2006 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion

using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices;
using OpenNETCF.Reflection;

namespace OpenNETCF.IO
{
    /// <summary>
    /// Listens to the file system change notifications and raises events when a directory, or file in a directory, changes.
    /// </summary>
    /// <remarks>
    /// The FileSystemWatcher requires the system to have aygshell.dll. Also, this class is not thread-safe. 
    /// </remarks>
    public class FileSystemWatcher : System.ComponentModel.Component, IDisposable
    {
        internal string m_directory = "";
        private string m_path = "";
        internal string m_filter = "*.*";
        private bool m_enableRaisingEvents = false;

        //hide from NDoc
#if !NDOC
        private WindowSink windowSink;
#endif

        /// <summary>
        /// Occurs when a file or directory in the specified <see cref="FileSystemWatcher.Path"/> is created.
        /// </summary>
        public event FileSystemEventHandler Created;
        /// <summary>
        /// Occurs when a file or directory in the specified <see cref="FileSystemWatcher.Path"/> is changed.
        /// </summary>
        public event FileSystemEventHandler Changed;
        /// <summary>
        /// Occurs when a file or directory in the specified <see cref="P:OpenNETCF.IO.FileSystemWatcher.Path"/> is deleted.
        /// </summary>
        public event FileSystemEventHandler Deleted;
        /// <summary>
        /// Occurs when a file or directory in the specified <see cref="FileSystemWatcher.Path"/> is renamed.
        /// </summary>
        public event RenamedEventHandler Renamed;

        /// <summary>
        /// Initializes a new instance of the <b>FileSystemWatcher</b> class.
        /// </summary>
        public FileSystemWatcher()
            : this("\\", "*.*")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <b>FileSystemWatcher</b> class, given the specified directory to monitor.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        public FileSystemWatcher(string path)
            : this(path, "*.*")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <b>FileSystemWatcher</b> class, given the specified directory and type of files to monitor.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        /// <param name="filter">The type of files to watch. For example, "*.txt" watches for changes to all text files.</param>
        public FileSystemWatcher(string path, string filter)
        {
            if (!Device.Win32Library("aygshell").Exists)
            {
                throw new PlatformNotSupportedException("The FileSystemWatcher required aygshell.dll in the platform OS.  Either add it or consider using the FileSystemMonitor class.");
            }

            if (path == null) throw new ArgumentNullException();

            IncludeSubdirectories = false;

            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.DirectoryName;
#if !NDOC
            windowSink = new WindowSink(this);
#endif
            this.Path = path;
            this.Filter = filter;

            // TODO: check for AYGSHELL
        }

        /// <summary>
        /// Gets or sets a value indicating whether the component is enabled.
        /// </summary>
        public bool EnableRaisingEvents
        {
            set
            {
                if (m_enableRaisingEvents != value)
                {
                    if (m_enableRaisingEvents == false)
                    {
                        if (!m_enableRaisingEvents)
                        {
                            Remove();
                            AddDir();
                        }
                    }
                    else
                    {
                        if (m_enableRaisingEvents)
                            Remove();
                    }
                    m_enableRaisingEvents = value;
                }
            }
            get
            {
                return m_enableRaisingEvents;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether subdirectories within the specified path should be monitored.
        /// </summary>
        public bool IncludeSubdirectories { get; set; }


        /// <summary>
        /// Gets or sets the type of changes to watch for.
        /// </summary>
        public NotifyFilters NotifyFilter { get; set; }


        /// <summary>
        /// Gets or sets the path of the directory to watch.
        /// </summary>
        public string Path
        {
            set
            {
                if (value == null) throw new ArgumentNullException();

                object[] obj;

                if (!(Directory.Exists(value)))
                {
                    obj = new Object[1];
                    obj[0] = value;
                    throw new ArgumentException("Invalid Directory Name", value);
                }
                this.m_directory = value;

                m_path = value;
                m_path += '\0';
            }
            get
            {
                return m_directory;

            }
        }

        /// <summary>
        /// Gets or sets the filter string, used to determine what files are monitored in a directory.
        /// </summary>
        public string Filter
        {
            set
            {
                if (value == null || value == String.Empty)
                    value = "*.*";
                if (String.Compare(this.m_filter, value, true) != 0)
                    this.m_filter = value;
            }
            get
            {
                return m_filter;

            }

        }

        internal void Remove()
        {
#if !NDOC
            NativeMethods.SHChangeNotifyDeregister(windowSink.Hwnd);
#endif
        }

        internal void AddDir()
        {
            NativeMethods.SHCHANGENOTIFYENTRY notEntry = new NativeMethods.SHCHANGENOTIFYENTRY();

            //Set mask
            notEntry.dwEventMask = (int)SHCNE_ALLEVENTS;
            //notEntry.dwEventMask = (int)SHCNE_ATTRIBUTES | (int)SHCNE_UPDATEDIR | (int)SHCNE_UPDATEITEM;
            //notEntry.dwEventMask = (int)SHCNE_UPDATEDIR | (int)SHCNE_UPDATEITEM;

            notEntry.fRecursive = BOOL(IncludeSubdirectories);

            //Set watch dir
            IntPtr lpWatchDir = Marshal2.StringToHGlobalUni(m_path);
            notEntry.pszWatchDir = lpWatchDir;

            //Call API
#if !NDOC
            int res = NativeMethods.SHChangeNotifyRegister(windowSink.Hwnd, ref notEntry);
#endif

        }

        #region virtual events

        protected virtual void OnChanged(FileSystemEventArgs e)
        {
            lock (this)
            {
                if (this.Changed != null)
                {
                    this.Changed(this, e);
                }
            }
        }

        protected virtual void OnCreated(FileSystemEventArgs e)
        {
            lock (this)
            {
                if (this.Created != null)
                    this.Created(this, e);
            }
        }

        protected virtual void OnDeleted(FileSystemEventArgs e)
        {
            lock (this)
            {
                if (this.Deleted != null)
                    this.Deleted(this, e);
            }
        }

        protected virtual void OnRenamed(RenamedEventArgs e)
        {
            lock (this)
            {
                if (this.Renamed != null)
                    this.Renamed(this, e);
            }
        }


        #endregion

        #region API Declarations

        internal static int BOOL(bool value)
        {
            if (value)
                return 1;
            else
                return 0;
        }




        //internal const long SHCNE_RENAME	        =  0x00000001L;   // GOING AWAY
        internal const long SHCNE_RENAMEITEM = 0x00000001L;
        internal const long SHCNE_CREATE = 0x00000002L;
        internal const long SHCNE_DELETE = 0x00000004L;
        internal const long SHCNE_MKDIR = 0x00000008L;
        internal const long SHCNE_RMDIR = 0x00000010L;
        internal const long SHCNE_MEDIAINSERTED = 0x00000020L;
        internal const long SHCNE_MEDIAREMOVED = 0x00000040L;
        internal const long SHCNE_DRIVEREMOVED = 0x00000080L;
        internal const long SHCNE_DRIVEADD = 0x00000100L;
        internal const long SHCNE_NETSHARE = 0x00000200L;
        internal const long SHCNE_NETUNSHARE = 0x00000400L;
        internal const long SHCNE_ATTRIBUTES = 0x00000800L;
        internal const long SHCNE_UPDATEDIR = 0x00001000L;
        internal const long SHCNE_UPDATEITEM = 0x00002000L;
        internal const long SHCNE_SERVERDISCONNECT = 0x00004000L;
        internal const long SHCNE_UPDATEIMAGE = 0x00008000L;
        internal const long SHCNE_DRIVEADDGUI = 0x00010000L;
        internal const long SHCNE_RENAMEFOLDER = 0x00020000L;
        internal const long SHCNE_ALLEVENTS = 0x7FFFFFFFL;

        internal const int WM_FILECHANGEINFO = (0x8000 + 0x101);


        internal struct FILECHANGENOTIFY
        {
            internal int dwRefCount;
            internal int cbSize;
            internal int wEventId;
            internal uint uFlags;
            internal int dwItem1;
            internal int dwItem2;
            internal int dwAttributes;
            internal int dwLowDateTime;
            internal int dwHighDateTime;
            internal uint nFileSize;

            private FILECHANGENOTIFY(int dwRefCount, int cbSize, int wEventId, uint uFlags, int dwItem1, int dwItem2,
                int dwAttributes, int dwLowDateTime, int dwHighDateTime, uint nFileSize)
            {
                this.dwRefCount = dwRefCount;
                this.cbSize = cbSize;
                this.wEventId = wEventId;
                this.uFlags = uFlags;
                this.dwItem1 = dwItem1;
                this.dwItem2 = dwItem2;
                this.dwAttributes = dwAttributes;
                this.dwLowDateTime = dwLowDateTime;
                this.dwHighDateTime = dwHighDateTime;
                this.nFileSize = nFileSize;
            }
        }


        #endregion


        #region IDisposable Members

        private bool disposed = false;

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="FileSystemWatcher"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // Release unmanaged resources. If disposing is false, 
                // only the following code is executed.
                if (m_enableRaisingEvents)
                    this.Remove();

            }
            disposed = true;
        }

        #endregion

        #region WindowSink
#if !NDOC
        internal class WindowSink : Microsoft.WindowsCE.Forms.MessageWindow
        {

            private FileSystemWatcher watcher;

            public WindowSink(FileSystemWatcher w)
            {
                watcher = w;
            }

            //helper function
            private bool ValidateByFilter(string fileName)
            {

                if (watcher.m_filter != "*.*")
                {
                    string filterName = System.IO.Path.GetFileNameWithoutExtension(watcher.m_filter);
                    string filterExt = System.IO.Path.GetExtension(watcher.m_filter);

                    if (filterName == "*") //check ext only
                    {
                        if (System.IO.Path.GetExtension(fileName).ToLower() != filterExt.ToLower())
                        {
                            return false;
                        }
                        else
                            return true;
                    }
                    else //we've got name in the filter
                    {
                        if (filterExt != ".*") //star in the ext
                        {
                            if (System.IO.Path.GetFileNameWithoutExtension(fileName).ToLower() != filterName.ToLower())
                            {
                                return false;
                            }
                            else
                                return true;
                        }
                        else //name and ext supplied
                        {
                            if (System.IO.Path.GetFileNameWithoutExtension(fileName).ToLower() != filterName.ToLower())
                            {
                                return false;
                            }
                            else
                                return true;

                        }
                    }

                }
                return true;

            }


            protected override void WndProc(ref Microsoft.WindowsCE.Forms.Message msg)
            {

                if (msg.Msg == WM_FILECHANGEINFO)
                {
                    if (msg.LParam == IntPtr.Zero)
                        return;


                    FILECHANGENOTIFY fchnot = (FILECHANGENOTIFY)Marshal.PtrToStructure(msg.LParam, typeof(FILECHANGENOTIFY));

                    string fullPath = Marshal.PtrToStringUni((IntPtr)fchnot.dwItem1);
                    string fullPath2 = Marshal.PtrToStringUni((IntPtr)fchnot.dwItem2);
                    string fileName = System.IO.Path.GetFileName(fullPath);
                    string dirName = System.IO.Path.GetDirectoryName(fullPath);
                    string newFileName = "";


                    if (ValidateByFilter(fileName))
                    {
                        FileSystemEventArgs args;
                        RenamedEventArgs renArgs;

                        switch (fchnot.wEventId)
                        {
                            case (int)SHCNE_CREATE:
                                if ((watcher.NotifyFilter & NotifyFilters.FileName) == NotifyFilters.FileName)
                                {
                                    args = new FileSystemEventArgs(WatcherChangeTypes.Created, dirName, fileName);
                                    watcher.OnCreated(args);
                                }
                                break;
                            case (int)SHCNE_MKDIR:
                                if ((watcher.NotifyFilter & NotifyFilters.DirectoryName) == NotifyFilters.DirectoryName)
                                {
                                    args = new FileSystemEventArgs(WatcherChangeTypes.Created, dirName, fileName);
                                    watcher.OnCreated(args);
                                }
                                break;
                            case (int)SHCNE_UPDATEDIR:
                                if ((watcher.NotifyFilter & NotifyFilters.DirectoryName) == NotifyFilters.DirectoryName)
                                {
                                    args = new FileSystemEventArgs(WatcherChangeTypes.Changed, dirName, fileName);
                                    watcher.OnChanged(args);
                                }
                                break;
                            case (int)SHCNE_RMDIR:
                                if ((watcher.NotifyFilter & NotifyFilters.DirectoryName) == NotifyFilters.DirectoryName)
                                {
                                    args = new FileSystemEventArgs(WatcherChangeTypes.Deleted, dirName, fileName);
                                    watcher.OnChanged(args);
                                }
                                break;
                            case (int)SHCNE_DELETE:
                                if ((watcher.NotifyFilter & NotifyFilters.FileName) == NotifyFilters.FileName)
                                {
                                    args = new FileSystemEventArgs(WatcherChangeTypes.Deleted, dirName, fileName);
                                    watcher.OnDeleted(args);
                                }
                                break;
                            case (int)SHCNE_UPDATEITEM:
                                if ((watcher.NotifyFilter & NotifyFilters.FileName) == NotifyFilters.FileName)
                                {
                                    args = new FileSystemEventArgs(WatcherChangeTypes.Changed, dirName, fileName);
                                    watcher.OnChanged(args);
                                }
                                break;
                            case (int)SHCNE_RENAMEFOLDER:
                                if ((watcher.NotifyFilter & NotifyFilters.DirectoryName) == NotifyFilters.DirectoryName)
                                {
                                    //switched around the paths - new and old were reversed
                                    newFileName = System.IO.Path.GetFileName(fullPath2);
                                    renArgs = new RenamedEventArgs(WatcherChangeTypes.Renamed, dirName, newFileName, fileName);
                                    watcher.OnRenamed(renArgs);
                                }
                                break;
                            case (int)SHCNE_RENAMEITEM:
                                if ((watcher.NotifyFilter & NotifyFilters.FileName) == NotifyFilters.FileName)
                                {
                                    //switched around the paths - new and old were reversed
                                    newFileName = System.IO.Path.GetFileName(fullPath2);
                                    renArgs = new RenamedEventArgs(WatcherChangeTypes.Renamed, dirName, newFileName, fileName);
                                    watcher.OnRenamed(renArgs);
                                }
                                break;

                            case (int)SHCNE_ATTRIBUTES:
                                if ((watcher.NotifyFilter & NotifyFilters.Attributes) == NotifyFilters.Attributes)
                                {
                                    args = new FileSystemEventArgs(WatcherChangeTypes.Changed, dirName, fileName);
                                    watcher.OnChanged(args);
                                }
                                break;

                        }
                    }

                    NativeMethods.SHChangeNotifyFree(msg.LParam);
                }

                msg.Result = (IntPtr)0;
                base.WndProc(ref msg);
            }

        }
#endif


        #endregion
    }
}
