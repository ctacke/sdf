namespace OpenNETCF.Net
{
    using System;
    using System.Runtime.InteropServices;

    public partial class ConnectionManager
    {
        internal enum ConnMgrConRefType
        {
            NAP = 0,
            Proxy
        }

        partial class SafeNativeMethods
        {
            [DllImport("cellcore.dll", EntryPoint = "ConnMgrMapConRef", SetLastError = true)]
            public static extern int ConnMgrMapConRef(ConnMgrConRefType refType, string szConRef, out Guid guid);
        }
    }
}
