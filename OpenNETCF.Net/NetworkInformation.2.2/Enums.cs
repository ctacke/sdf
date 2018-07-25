using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    internal enum NDIS_OID : uint
    {
        BSSID = 0x0D010101,
        SSID = 0x0D010102,
        NETWORK_TYPES_SUPPORTED = 0x0D010203,
        NETWORK_TYPE_IN_USE = 0x0D010204,
        TX_POWER_LEVEL = 0x0D010205,
        RSSI = 0x0D010206,
        RSSI_TRIGGER = 0x0D010207,
        INFRASTRUCTURE_MODE = 0x0D010108,
        FRAGMENTATION_THRESHOLD = 0x0D010209,
        RTS_THRESHOLD = 0x0D01020A,
        NUMBER_OF_ANTENNAS = 0x0D01020B,
        RX_ANTENNA_SELECTED = 0x0D01020C,
        TX_ANTENNA_SELECTED = 0x0D01020D,
        SUPPORTED_RATES = 0x0D01020E,
        DESIRED_RATES = 0x0D010210,
        CONFIGURATION = 0x0D010211,
        STATISTICS = 0x0D020212,
        ADD_WEP = 0x0D010113,
        REMOVE_WEP = 0x0D010114,
        DISASSOCIATE = 0x0D010115,
        POWER_MODE = 0x0D010216,
        BSSID_LIST = 0x0D010217,
        AUTHENTICATION_MODE = 0x0D010118,
        PRIVACY_FILTER = 0x0D010119,
        BSSID_LIST_SCAN = 0x0D01011A,
        WEP_STATUS = 0x0D01011B,

        ENCRYPTION_STATUS = WEP_STATUS,
        RELOAD_DEFAULTS = 0x0D01011C,
    }

    [Flags]
    internal enum PerAdapterFlags
    {
        DnsEligible = 0x01,
        Transient = 0x02,
    }

    [Flags]
    internal enum AdapterAddressFlags
    {
        SkipUnicast = 0x01,
        SkipAnycast = 0x02,
        SkipMulticast = 0x04,
        SkipDnsServer = 0x08,
        IncludePrefix = 0x10,
        SkipFriendlyName = 0x20
    }

}
