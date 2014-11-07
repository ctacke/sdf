using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net
{
        //#define DNS_TYPE_A          0x0001      //  1
        ////  RFC 1886    (IPv6 Address)
        //#define DNS_TYPE_AAAA       0x001c      //  28
        ////  RFC 2052    (Service location)
        //#define DNS_TYPE_SRV        0x0021      //  33
        internal enum DNS_TYPE : ushort
        {
            TypeA = 1,
            IPv4Address = 1,
            TypeAAAA = 0x1c,
            IPv6Address = 0x1c,
            ServiceLocation = 0x21,
        }
}
