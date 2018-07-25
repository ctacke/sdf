using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /*
    typedef struct _MIB_TCPROW {
      DWORD dwState; 
      DWORD dwLocalAddr; 
      DWORD dwLocalPort; 
      DWORD dwRemoteAddr; 
      DWORD dwRemotePort; 
    } MIB_TCPROW, *PMIB_TCPROW;
     */
    internal struct MIB_TCPROW
    {
        public TcpState dwState;
        public uint dwLocalAddr;
        public uint dwLocalPort;
        public uint dwRemoteAddr;
        public uint dwRemotePort;
    }
}
