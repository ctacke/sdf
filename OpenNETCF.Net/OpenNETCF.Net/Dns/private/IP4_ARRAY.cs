using System;

using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{
    [StructLayout(LayoutKind.Sequential)]
    internal class IP4_ARRAY
    {
        public int AddrCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
        public byte[] AddrArray;

        //typedef struct _IP4_ARRAY {
        //  DWORD AddrCount;
        //  IP4_ADDRESS AddrArray[1];
        //} IP4_ARRAY, *PIP4_ARRAY
    }
}
