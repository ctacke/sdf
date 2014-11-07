using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;

namespace OpenNETCF.Net
{
    public partial class ConnectionManager
    {
        // For thread safety
        private static readonly object syncRoot = new object();

        private ConnectionInfo connInfo;
        private IntPtr hConnection; 
        internal IntPtr hWnd;
        private uint dwTimeout = 0xFFFFFFFF; // Fixes Bug #2: sets timeout to INFINITE by default
#if !NDOC
        private MessagePump mwnd = null;
#endif     
        private const int CONNMGR_PARAM_GUIDDESTNET = (0x1);
        private const int CONNMGR_FLAG_PROXY_HTTP = 0x1;
        private const int WM_APP_CONNMGR = 0x400 + 0;

        private readonly Guid IID_DestNetInternet = new Guid("436ef144-b4fb-4863-a041-8f905a62c572");

        #region Properties

        /// <summary>
        /// Returns a handle to the current connection.
        /// </summary>
        [Obsolete("Do not use ConnectionManager.Handle", true)]
        public IntPtr Handle
        {
            get
            {
                return IntPtr.Zero;
            }
        }
        // ---------------------------------------------------------------
        /// <summary>
        /// Describes the current state of the connection
        /// </summary>
        public ConnectionStatus Status
        {
            get
            {
                ConnectionStatus status = ConnectionStatus.Unknown;

                lock (syncRoot)
                {
                    uint dwStatus;

                    if(hConnection == IntPtr.Zero)
                    {
                        return ConnectionStatus.Disconnected;
                    }

                    long result = SafeNativeMethods.ConnMgrConnectionStatus(hConnection, out dwStatus);
                    if (result == 0x800700006) 
                    {
                        throw new InvalidOperationException("Invalid handle");
                    }
                    if (result == 0)
                    {
                        status = (ConnectionStatus)dwStatus;
                    }
                }
                return status;
            }
        }

        /// <summary>
        /// Specifies the timeout for a synchronous connection attempt
        /// </summary>
        [CLSCompliant(false)]
        public uint Timeout
        {
            get
            {
                uint i;
                lock (syncRoot)
                {
                    i = dwTimeout;
                }
                return i;
            }
            set
            {
                lock (syncRoot)
                {
                    dwTimeout = value;
                }
            }
        }

        #endregion

        #region Constructor/Destructor
        

        public ConnectionManager()
        {
            DoCellcoreCheck();

#if !NDOC
			mwnd = new MessagePump(this);
			hWnd = mwnd.Hwnd;
            
#endif
            ManualResetEvent mre = new ManualResetEvent(false);
            mre.Handle = SafeNativeMethods.ConnMgrApiReadyEvent();
            mre.WaitOne();
            SafeNativeMethods.CloseHandle(mre.Handle);
        }
        
#if !SDF22
        ~ConnectionManager()
        {
            RequestDisconnect();
        }
#endif
        #endregion

        #region Public Methods
        
        /// <summary>
        /// Makes an exclusive, asynchronous connection with Connection Manager using the system default destination. 
        /// </summary>
        public void Connect()
        {
            Connect(IID_DestNetInternet, true, ConnectionMode.Asynchronous);
        }

        /// <summary>
        /// Makes an asynchronous connection with Connection Manager using the system default destination.
        /// </summary>
        /// <param name="exclusive">True creates an exclusive connection; false creates a non-exclusive connection.</param>
        public void Connect(bool exclusive)
        {
            Connect(IID_DestNetInternet, exclusive, ConnectionMode.Asynchronous);
        }

        /// <summary>
        /// Makes an exclusive connection with Connection Manager using the system default destination.
        /// </summary>
        /// <param name="mode">States how the connection is to be made: either Synchronous or Asynchronous</param>
        public void Connect(ConnectionMode mode)
        {
            Connect(IID_DestNetInternet, true, mode);
        }

        /// <summary>
        /// Makes a connection with Connection Manager using the system default destination.
        /// </summary>
        /// <param name="exclusive">True creates an exclusive connection; false creates a non-exclusive connection.</param>
        /// <param name="mode">States how the connection is to be made: either Synchronous or Asynchronous</param>
        public void Connect(bool exclusive, ConnectionMode mode)
        {
            Connect(IID_DestNetInternet, exclusive, mode);
        }
        
        /// <summary>
        /// Makes a connection with Connection Manager using the specified destination.
        /// </summary>
        /// <param name="destGuid">The destination to connect to.</param>
        /// <param name="exclusive">Determines whether the connection should be exclusive or not.</param>
        /// <param name="mode">Determines how the connection is to be made: either Synchronous or Asynchronous</param>
        public void Connect(Guid destGuid, bool exclusive, ConnectionMode mode)
        {
            lock (syncRoot)
            {
                Guid destNet = MapUrl("http://www.opennetcf.com/");

                connInfo.guidDestNet = destGuid;
                connInfo.cbSize = (uint)Marshal.SizeOf(connInfo);
                connInfo.dwParams = CONNMGR_PARAM_GUIDDESTNET;
                connInfo.dwFlags = 0; // CONNMGR_FLAG_PROXY_HTTP;
                connInfo.dwPriority = (int)ConnectionPriority.UserInteractive;
                connInfo.bExclusive = (exclusive) ? 1 : 0;
                connInfo.bDisabled = 0;
                connInfo.hWnd = hWnd;
                connInfo.uMsg = WM_APP_CONNMGR;
                connInfo.lParam = 0;

                if (mode == ConnectionMode.Synchronous)
                {
                    uint dwStatus;
                    int result = SafeNativeMethods.ConnMgrEstablishConnectionSync(ref connInfo, out hConnection, dwTimeout, out dwStatus);
                    if (result != 0)
                    {
                        throw new InvalidOperationException("Failed to make a connection. ConnMgr returned status " + ((ConnectionStatus)dwStatus));
                    }
                }
                else
                {
                    SafeNativeMethods.ConnMgrEstablishConnection(ref connInfo, out hConnection);
                }
            }
        }
        
        /// <summary>
        /// Disconnect the connection whose handle is hConnection.
        /// </summary>
        [Obsolete("Please use RequestDisconnect", false)]
        public void Disconnect()
        {
            RequestDisconnect();
        }
        
        /// <summary>
        /// Disconnect the connection whose handle is hConnection.
        /// </summary>
        public void RequestDisconnect()
        {
            lock (syncRoot)
            {
                if (hConnection == IntPtr.Zero) return;

                int result = SafeNativeMethods.ConnMgrReleaseConnection(hConnection, Convert.ToInt32(false));
                if (result == 0)
                {
                    SafeNativeMethods.CloseHandle(hConnection);

                    // The connection handle has been freed, so zero it to prevent handle leaks
                    hConnection = IntPtr.Zero;

                    // Raise the OnDisconnect event since the WndProc can't/won't do it for us
                    if(Disconnected != null)
                    Disconnected(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Maps a URL to a destination GUID.
        /// </summary>
        /// <param name="Url">The URL to map.</param>
        /// <returns>The GUID the URL is mapped to.</returns>
        public Guid MapUrl(string Url)
        {
            Guid urlGuid = Guid.Empty;

            lock (syncRoot)
            {
                IntPtr idx = IntPtr.Zero;
                SafeNativeMethods.ConnMgrMapURL(Url, ref urlGuid, idx);
            }
            return urlGuid;
        }

        /// <summary>
        /// Returns a collection of destinations specified within the system.
        /// </summary>
        /// <returns>A DestinationInfo collection with details of all the destinations in the system.</returns>
        public DestinationInfoCollection EnumDestinations()
        {
            DestinationInfoCollection dests = new DestinationInfoCollection();

            int i = 0;

            lock (syncRoot)
            {
                do
                {
                    IntPtr hDestInfo = Marshal.AllocHGlobal(DestinationInfo.NativeSize);

                    long ret = SafeNativeMethods.ConnMgrEnumDestinations(i++, hDestInfo);
                    if (ret != 0) 
                    {
                        break;
                    }
                    DestinationInfo cm = new DestinationInfo(hDestInfo);
                    dests.Add(cm);

                    Marshal.FreeHGlobal(hDestInfo);
                }
                while (true);
            }

            return dests;
        }
        
        #endregion
    }
}
