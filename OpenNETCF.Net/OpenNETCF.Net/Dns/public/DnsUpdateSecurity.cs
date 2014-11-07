using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net
{
    [Flags]
	public enum DnsUpdateSecurity : int
	{
        /// <summary>
        /// Does not attempt secure dynamic updates if names cannot be registered anonymously.
        /// </summary>
        Off = 0x0010,
        /// <summary>
        /// Attempts nonsecure dynamic update. If refused, then attempts secure dynamic update. 
        /// </summary>
        On = 0x0020,
        /// <summary>
        /// Attempts secure dynamic updates only. Does not attempt anonymous name registration. 
        /// </summary>
        Only = 0x0100

        //#define DNS_UPDATE_SECURITY_OFF             0x00000010
        //#define DNS_UPDATE_SECURITY_ON              0x00000020
        //#define DNS_UPDATE_SECURITY_ONLY            0x00000100
	}
}
