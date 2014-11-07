using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net
{
	public abstract class DnsRecord
	{
        public string Name { get; set; }
        public int TTL { get; set; }
    }
}
