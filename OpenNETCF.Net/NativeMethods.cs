using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{
    internal static class NativeMethods
    {
        [DllImport("winsock.dll", SetLastError=true, EntryPoint="htonl")]
        public static extern uint htonl(uint hostlong);

        [DllImport("winsock.dll", SetLastError = true, EntryPoint = "htons")]
        public static extern ushort htons(ushort hostshort);

        [DllImport("winsock.dll", SetLastError = true, EntryPoint = "ntohl")]
        public static extern uint ntohl(uint hostlong);

        [DllImport("winsock.dll", SetLastError = true, EntryPoint = "ntohs")]
        public static extern ushort ntohs(ushort hostshort);

    }
}
