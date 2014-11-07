using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using OpenNETCF.Threading;

using EventWaitHandle = OpenNETCF.Threading.EventWaitHandle;
using EventResetMode = OpenNETCF.Threading.EventResetMode;

namespace OpenNETCF.IO
{
  /// <summary>
  /// Listens to the file system change notifications and raises events when a directory, or file in a directory, changes.
  /// </summary>
  /// <remarks>
  /// The FileSystemMonitor is thread-safe and does not depend on aygshell.dll being present in the system, 
  /// therefore, it is safe to use this for file system notifications when no GUI is present. 
  /// </remarks>
  public class FileSystemMonitor : Component, IDisposable
  {
    private string path = "";
    internal string filter = "*.*";
    private bool enableRaisingEvents = false;
    private bool includeSubdirectories = false;
    internal NotifyFilters notifyFilter;
    private bool active = false;

    //hide from NDoc
#if !NDOC
    private FileNotificationMonitor notificationMonitor;
#endif
    internal string directory = "";

    /// <summary>
    /// Occurs when a file or directory in the specified <see cref="FileSystemMonitor.Path"/> is created.
    /// </summary>
    public event FileSystemEventHandler Created;
    /// <summary>
    /// Occurs when a file or directory in the specified <see cref="FileSystemMonitor.Path"/> is changed.
    /// </summary>
    public event FileSystemEventHandler Changed;
    /// <summary>
    /// Occurs when a file or directory in the specified <see cref="P:OpenNETCF.IO.FileSystemWatcher.Path"/> is deleted.
    /// </summary>
    public event FileSystemEventHandler Deleted;
    /// <summary>
    /// Occurs when a file or directory in the specified <see cref="FileSystemMonitor.Path"/> is renamed.
    /// </summary>
    public event RenamedEventHandler Renamed;

    /// <summary>
    /// Initializes a new instance of the <b>FileSystemMonitor</b> class.
    /// </summary>
    public FileSystemMonitor()
      : this("\\", "*.*")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <b>FileSystemMonitor</b> class, given the specified directory to monitor.
    /// </summary>
    /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
    public FileSystemMonitor(string path)
      : this(path, "*.*")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <b>FileSystemMonitor</b> class, given the specified directory and type of files to monitor.
    /// </summary>
    /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
    /// <param name="filter">The type of files to watch. For example, "*.txt" watches for changes to all text files.</param>
    public FileSystemMonitor(string path, string filter)
    {
      notifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.DirectoryName;
#if !NDOC
      notificationMonitor = new FileNotificationMonitor(this);
#endif

      this.Path = path;
      this.Filter = filter;

    }

    /// <summary>
    /// Gets or sets a value indicating whether the component is enabled.
    /// </summary>
    public bool EnableRaisingEvents
    {
      set
      {
        if (enableRaisingEvents == value)
        { // no change
          return;
        }

        if (value)
        {
          AddDir();
        }
        else
        {
          Remove();
        }

        enableRaisingEvents = value;
      }
      get
      {
        return enableRaisingEvents;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether subdirectories within the specified path should be monitored.
    /// </summary>
    public bool IncludeSubdirectories
    {
      set
      {
        includeSubdirectories = value;
      }
      get
      {
        return includeSubdirectories;

      }
    }


    /// <summary>
    /// Gets or sets the type of changes to watch for.
    /// </summary>
    public NotifyFilters NotifyFilter
    {
      set
      {
        notifyFilter = value;
      }
      get
      {
        return notifyFilter;

      }
    }


    /// <summary>
    /// Gets or sets the path of the directory to watch.
    /// </summary>
    public string Path
    {
      set
      {
        object[] obj;

        if (value == null) throw new ArgumentNullException();

        if (!(Directory.Exists(value)))
        {
          obj = new Object[1];
          obj[0] = value;
          throw new ArgumentException("Invalid Directory Name", value);
        }
        this.directory = value;

        path = value;

        // we need to re-set up the events if we're already looking
        if (enableRaisingEvents)
        {
          AddDir();
        }
      }
      get
      {
        return directory;

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
        if (String.Compare(this.filter, value, true) != 0)
          this.filter = value;
      }
      get
      {
        return filter;

      }

    }

    internal void Remove()
    {
      if (!active)
        return;
#if !NDOC
      notificationMonitor.StopListen();
#endif
      active = false;
    }

    internal void AddDir()
    {
      if (active)
        Remove();


      //Call API
#if !NDOC
      notificationMonitor.Listen(this.path);
#endif
      active = true;
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

    //Destructor
    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations before the <see cref="FileSystemMonitor"/> is reclaimed by garbage collection.
    /// </summary>
    ~FileSystemMonitor()
    {
      this.Dispose(true);
    }

    private bool disposed = false;

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="FileSystemMonitor"/> and optionally releases the managed resources.
    /// </summary>
    /// <param name="finalizing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected override void Dispose(bool finalizing)
    {
      // Check to see if Dispose has already been called.
      if (!this.disposed)
      {
        // Release unmanaged resources. If disposing is false, 
        // only the following code is executed.
        //if (enableRaisingEvents)
        this.Remove();

      }
      if (!finalizing)
      {
        GC.SuppressFinalize(this);
      }
      disposed = true;
    }

    public new void Dispose()
    {
      Dispose(false);
    }
    #endregion

    #region WindowSink
    internal class FileNotificationMonitor : IDisposable
    {
      private const int InvalidHandle = 0;	// Queue handle must be non-zero
      private const int WAIT_OBJECT_0 = 0;
      private const int TimeoutInfinite = -1;
      const int maxData = 1024;

      private bool m_quit = false;
      private bool m_listening = false;

      //MA Not sure but if there are multiple FSWs running a watcher thread and we
      //leave the WaitHandleNameQuit constant so we can signal a named waithandle to quit wouldn't that signal all of them in the process since 
      //everything would be running in the same process and they are all using the same name??
#if CF1
            private string WaitHandleName = GuidEx.NewGuid().ToString(); //"FileWatcherEventWaitHandle";
            private string WaitHandleNameQuit = GuidEx.NewGuid().ToString(); //"FileWatcherEventWaitHandleQuit";
#else
      private string WaitHandleName = Guid.NewGuid().ToString(); //"FileWatcherEventWaitHandle";
      private string WaitHandleNameQuit = Guid.NewGuid().ToString(); //"FileWatcherEventWaitHandleQuit";
#endif
      private string m_path = @"\My Documents";
      private FileNotificationChangeType filter = FileNotificationChangeType.CEGetInfo | FileNotificationChangeType.Attributes | FileNotificationChangeType.Create | FileNotificationChangeType.DirectoryName | FileNotificationChangeType.FileName | FileNotificationChangeType.LastAccess | FileNotificationChangeType.LastWrite | FileNotificationChangeType.Security | FileNotificationChangeType.Size;

      private FileSystemMonitor m_fsw;

      public FileNotificationMonitor(FileSystemMonitor monitor)
      {
        m_fsw = monitor;
      }

      public void Listen(string path)
      {
        if (m_listening)
          return;

        m_path = path;
        m_quit = false;

        new Thread(new ThreadStart(ThreadProc)).Start();

        m_listening = true;
      }

      public void StopListen()
      {
        bool createdNew = false;
        EventWaitHandle ewh = new EventWaitHandle(false, EventResetMode.ManualReset, WaitHandleName, out createdNew);
        if (ewh != null)
        {
          m_quit = true;
          ewh.Set();
          EventWaitHandle quit = new EventWaitHandle(false, EventResetMode.ManualReset, WaitHandleNameQuit, out createdNew);
          quit.WaitOne(5000, true);
        }

        m_listening = false;
      }

      private bool disposed = false;

      ~FileNotificationMonitor()
      {
        this.Dispose(true);
      }

      protected void Dispose(bool finalizing)
      {
        // Check to see if Dispose has already been called.
        if (!this.disposed)
        {
          // kill the worker thread
          m_quit = true;
          EventWaitHandle ewh = new EventWaitHandle(false, EventResetMode.ManualReset, WaitHandleName);
          ewh.Set();
        }
        if (!finalizing)
        {
          GC.SuppressFinalize(this);
        }
        disposed = true;
      }

      public void Dispose()
      {
        Dispose(false);
      }

      private unsafe void ThreadProc()
      {
        IntPtr notifyHandle = NativeMethods.FindFirstChangeNotification(m_path, true, filter);
        FileNotifyInformation notifyData = new FileNotifyInformation();
        uint returned = 0;
        uint available = 0;

        EventWaitHandle ewh = new EventWaitHandle(false, EventResetMode.ManualReset, WaitHandleName);
        EventWaitHandle quitWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, WaitHandleNameQuit);
        while (!m_quit)
        {
          ewh.Reset();

          if (EventWaitHandle.WaitAny(new IntPtr[] { notifyHandle, ewh.Handle }, TimeoutInfinite, false) != EventWaitHandle.WaitTimeout)
          {

            if (m_quit)
              break;

            IntPtr ptr = IntPtr.Zero;
            if (NativeMethods.CeGetFileNotificationInfo(notifyHandle, 0, ref ptr, 0, ref returned, ref available))
            {
              if (available > 0)
              {
                int maxData = 2048;
                fixed (byte* pData = new byte[maxData])
                {
                  // get data 
                  if (NativeMethods.CeGetFileNotificationInfo(notifyHandle, 0, pData, maxData, ref returned, ref available))
                  {
                    notifyData = new FileNotifyInformation(pData, 0);

                    // handle data in notifyData
                    if (ValidateByFilter(notifyData.Filename))
                    {
                      RaiseEvents(notifyData.Action, notifyData.Filename);
                    }

                    int offset = 0;
                    offset += notifyData.NextEntryOffset;

                    while (notifyData.NextEntryOffset > 0)
                    {
                      notifyData = new FileNotifyInformation(pData, offset);

                      if (ValidateByFilter(notifyData.Filename))
                      {
                        RaiseEvents(notifyData.Action, notifyData.Filename);
                      }
                      offset += notifyData.NextEntryOffset;
                    }

                  }
                }
              }
              else
              {
                //Seems that subdirectories don't return anything but then the notifyHandle is never reset and if available data is 0 notifyHandle never resets
                NativeMethods.FindCloseChangeNotification(notifyHandle);
                notifyHandle = NativeMethods.FindFirstChangeNotification(m_path, true, filter);
              }
            }
          } // if( waithandle...
        } // while (!m_quit)

        NativeMethods.FindCloseChangeNotification(notifyHandle);
        quitWaitHandle.Set();
      }

      private static string oldName;

      private void RaiseEvents(FileAction action, string fileName)
      {
        FileSystemEventArgs args;
        RenamedEventArgs renArgs;

        switch (action)
        {
          case FileAction.Added:
            if ((m_fsw.notifyFilter & NotifyFilters.FileName) == NotifyFilters.FileName)
            {
              args = new FileSystemEventArgs(WatcherChangeTypes.Created, m_path, fileName);
              m_fsw.OnCreated(args);
            }
            break;
          case FileAction.Removed:
            if (((m_fsw.notifyFilter & NotifyFilters.FileName) == NotifyFilters.FileName) ||
                ((m_fsw.notifyFilter & NotifyFilters.DirectoryName) == NotifyFilters.DirectoryName))
            {
              // There's no way to determine whether the object 
              // just deleted was a file or a directory so treat 
              // them both the same.
              args = new FileSystemEventArgs(WatcherChangeTypes.Deleted, m_path, fileName);
              m_fsw.OnDeleted(args);
            }
            break;
          case FileAction.Modified:
            args = new FileSystemEventArgs(WatcherChangeTypes.Changed, m_path, fileName + " (Modified)");
            m_fsw.OnChanged(args);
            break;
          case FileAction.RenamedNewName:
            if (((m_fsw.notifyFilter & NotifyFilters.FileName) == NotifyFilters.FileName) ||
                ((m_fsw.notifyFilter & NotifyFilters.DirectoryName) == NotifyFilters.DirectoryName))
            {
              renArgs = new RenamedEventArgs(WatcherChangeTypes.Renamed, m_path, fileName, oldName);
              m_fsw.OnRenamed(renArgs);
              oldName = null;
            }
            break;
          case FileAction.RenamedOldName:
            if (((m_fsw.notifyFilter & NotifyFilters.FileName) == NotifyFilters.FileName) ||
                ((m_fsw.notifyFilter & NotifyFilters.DirectoryName) == NotifyFilters.DirectoryName))
            {
              oldName = fileName;
            }
            break;
          case FileAction.ChangeComplete:
            args = new FileSystemEventArgs(WatcherChangeTypes.Changed, m_path, fileName);
            m_fsw.OnChanged(args);
            break;
        }
      }

      //helper function
      private bool ValidateByFilter(string fileName)
      {

        if (m_fsw.filter != "*.*")
        {
          string filterName = System.IO.Path.GetFileNameWithoutExtension(m_fsw.filter);
          string filterExt = System.IO.Path.GetExtension(m_fsw.filter);

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

      #region Native Methods
      internal abstract class NativeMethods
      {
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern IntPtr FindFirstChangeNotification(string lpPathName, bool bWatchSubtree, FileNotificationChangeType dwNotifyFilter);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool FindNextChangeNotification(IntPtr hChangeHandle);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool FindCloseChangeNotification(IntPtr hChangeHandle);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool CeGetFileNotificationInfo(
            IntPtr h, uint dwFlags, byte[] lpBuffer,
            int nBufferLength, ref uint lpBytesReturned, ref uint lpBytesAvailable);

        [DllImport("coredll.dll", SetLastError = true)]
        public unsafe static extern bool CeGetFileNotificationInfo(
            IntPtr h, uint dwFlags, byte* lpBuffer,
            int nBufferLength, ref uint lpBytesReturned, ref uint lpBytesAvailable);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool CeGetFileNotificationInfo(
            IntPtr h, uint dwFlags, ref IntPtr lpBuffer,
            int nBufferLength, ref uint lpBytesReturned, ref uint lpBytesAvailable);
      }

      #endregion

      #region Enums
      internal enum Win32Error
      {
        //winerror.h
        ERROR_INSUFFICIENT_BUFFER = 122,
        ERROR_MORE_DATA = 234
      }

      public enum FileAction
      {
        Added = 1,
        Removed = 2,
        Modified = 3,
        RenamedOldName = 4,
        RenamedNewName = 5,
        ChangeComplete = 0x10000
      }

      [Flags]
      public enum FileNotificationChangeType : uint
      {
        FileName = 0x0001,
        DirectoryName = 0x0002,
        Attributes = 0x0004,
        Size = 0x0008,
        LastWrite = 0x0010,
        LastAccess = 0x0020,
        Create = 0x0040,
        Security = 0x0100,
        CEGetInfo = 0x80000000
      }

      #endregion

      #region FileNotifyInformation
      internal class FileNotifyInformation
      {
        public const int BufferLength = (260 * 2) + 12; // MAX_PATH plus header data
        private byte[] m_data = new byte[BufferLength];

        private const int NEXT_ENTRY_OFFSET = 0;
        private const int ACTION_OFFSET = 4;
        private const int NAME_LENGTH_OFFSET = 8;
        private const int FILENAME_OFFSET = 12;

        public static implicit operator byte[](FileNotifyInformation fni)
        {
          return fni.m_data;
        }

        public static implicit operator FileNotifyInformation(byte[] data)
        {
          return new FileNotifyInformation(data);
        }

        public FileNotifyInformation() { }

        internal FileNotifyInformation(byte[] data)
        {
          Buffer.BlockCopy(data, 0, m_data, 0, BufferLength);
        }

        internal FileNotifyInformation(byte[] data, int offset)
        {
          Buffer.BlockCopy(data, offset, m_data, 0, BufferLength);
        }

        private unsafe void memcpy(byte* dest, byte* src, int count)
        {
          int* psrc4 = (int*)src;
          int* pdest4 = (int*)dest;
          byte* psrc1 = src;
          byte* pdest1 = dest;

          int dwords = count / 4;
          int remainder = count % 4;

          for (int i = 0; i < dwords; i++)
          {
            *pdest4 = *psrc4;
            pdest4++;
            psrc4++;
          }

          psrc1 += (dwords * 4);
          pdest1 += (dwords * 4);
          for (int i = 0; i < remainder; i++)
          {
            *pdest1 = *psrc1;
            pdest1++;
            psrc1++;
          }
        }

        internal unsafe FileNotifyInformation(byte* data, int offset)
        {
          fixed (byte* dest = m_data)
          {
            byte* pSrc = data;
            pSrc += offset;

            memcpy(dest, pSrc, BufferLength);
          }
        }

        public int Size
        {
          // this will erase any existing data
          set { m_data = new byte[value]; }
          get { return m_data.Length; }
        }

        public int NextEntryOffset
        {
          get { return BitConverter.ToInt32(m_data, NEXT_ENTRY_OFFSET); }
          set { Buffer.BlockCopy(BitConverter.GetBytes(value), 0, m_data, NEXT_ENTRY_OFFSET, 4); }
        }

        public FileAction Action
        {
          get { return (FileAction)BitConverter.ToInt32(m_data, ACTION_OFFSET); }
          set { Buffer.BlockCopy(BitConverter.GetBytes((int)value), 0, m_data, ACTION_OFFSET, 4); }
        }

        private int FileNameLength
        {
          get { return BitConverter.ToInt32(m_data, NAME_LENGTH_OFFSET); }
        }

        public string Filename
        {
          get
          {
            return System.Text.Encoding.Unicode.GetString(m_data, FILENAME_OFFSET, FileNameLength);
          }
        }

      }
      #endregion
    }
    #endregion
  }
}
