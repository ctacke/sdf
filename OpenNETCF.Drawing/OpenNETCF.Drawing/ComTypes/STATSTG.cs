using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Runtime.InteropServices.ComTypes
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct STATSTG
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pwcsName;
        public int type;
        public long cbSize;
        public FILETIME mtime;
        public FILETIME ctime;
        public FILETIME atime;
        public int grfMode;
        public int grfLocksSupported;
        public Guid clsid;
        public int grfStateBits;
        public int reserved;
    }
}
