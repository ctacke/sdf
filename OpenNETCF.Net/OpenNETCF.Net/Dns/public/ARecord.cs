using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace OpenNETCF.Net
{
    public class ARecord : DnsRecord
    {
        public IPAddress Address { get; set; }
    }
}
