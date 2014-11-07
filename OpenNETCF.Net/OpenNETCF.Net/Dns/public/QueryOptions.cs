using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net
{
    [Flags]
    public enum QueryOptions : int
    {
        Standard = 0x00000000,
        AcceptTruncatedRespone = 0x00000001,
        UseTcpOnly = 0x00000002,
        NoRecursion = 0x00000004,
        BypassCache = 0x00000008,
        NoWireQuery = 0x00000010,
        NoLocalName = 0x00000020,
        NoHostsFile = 0x00000040,
        NoNetBT = 0x00000080,
        WireOnly = 0x00000100,
        ReturnMessage = 0x00000200,
        TreatAsFQDN = 0x00001000,
        DontResetTtleValues = 0x00100000

        //#define DNS_QUERY_STANDARD                  0x00000000
        //#define DNS_QUERY_ACCEPT_TRUNCATED_RESPONSE 0x00000001
        //#define DNS_QUERY_USE_TCP_ONLY              0x00000002
        //#define DNS_QUERY_NO_RECURSION              0x00000004
        //#define DNS_QUERY_BYPASS_CACHE              0x00000008

        //#define DNS_QUERY_NO_WIRE_QUERY             0x00000010
        //#define DNS_QUERY_NO_LOCAL_NAME             0x00000020
        //#define DNS_QUERY_NO_HOSTS_FILE             0x00000040
        //#define DNS_QUERY_NO_NETBT                  0x00000080

        //#define DNS_QUERY_WIRE_ONLY                 0x00000100
        //#define DNS_QUERY_RETURN_MESSAGE            0x00000200

        //#define DNS_QUERY_TREAT_AS_FQDN             0x00001000
        //#define DNS_QUERY_DONT_RESET_TTL_VALUES     0x00100000
    }
}
