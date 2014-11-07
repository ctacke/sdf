using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{

    internal class CONNMGR_CONNECTION_DETAILED_STATUS
    {
        public IntPtr pNext;

        public int dwVer;                // @field Structure version; current is CONNMGRDETAILEDSTATUS_VERSION.
        public DetailStatusParam dwParams;             // @field Combination of CONNMGRDETAILEDSTATUS_PARAM_* values.

        public ConnectionType dwType;               // @field One of CM_CONNTYPE_* values.
        //NOTE: the actual subtype is retrevied in ConnectionDetail 
        public int dwSubtype;            // @field One of CM_CONNSUBTYPE_* values.

        public ConnectionOption dwFlags;              // @field Combination of CM_DSF_* flags.
        public int dwSecure;             // @field Secure level (0 == not-secure) of connection.

        public Guid guidDestNet;           // @field GUID of destination network.
        public Guid guidSourceNet;         // @field GUID of source network.

        [MarshalAs(UnmanagedType.LPTStr)]
        public string szDescription;       // @field Name of connection, 0-terminated string or NULL if N/A.
        [MarshalAs(UnmanagedType.LPTStr)]
        public string szAdapterName;       // @field Name of adapter, 0-terminated or NULL if N/A.

        public ConnectionStatus dwConnectionStatus;   // @field One of CONNMGR_STATUS_*.
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public short[] LastConnectTime; // @field Time connection was last established.
        public int dwSignalQuality;      // @field Signal quality normalized in the range 0-255.

        public IntPtr pIPAddr; // @field Available IP addrs, or NULL if N/A.
        // End of version 1 fields.
    }
}
