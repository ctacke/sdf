using System;
namespace OpenNETCF.Net
{
   
    [Flags]
    public enum DetailStatusParam
    {
        Type                = 0x00000001,
        Subtype             = 0x00000002,
        DestinationNetwork  = 0x00000004,
        SourceNetwork       = 0x00000008,
        Flags               = 0x00000010,
        Secure              = 0x00000020,
        Description         = 0x00000040,
        AdapterName         = 0x00000080,
        COnnectionStatus    = 0x00000100,
        LastConnect         = 0x00000200,
        SignalQuality       = 0x00000400,
        IPAddress           = 0x00000800,
    }

    public enum ConnectionType
    {
        Unknown = 0,
        Cellular = 1,
        NIC = 2,
        Bluetooth = 3,
        Unimodem = 4,
        VPN = 5,
        Proxy = 6,
        PC = 7,
    }

    public enum ConnectionSubType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,

        // Cellular
        /// <summary>
        /// GPRS
        /// </summary>
        GPRS = 3,
        /// <summary>
        /// Not distinct from GPRS.
        /// </summary>
        XRTT= 4,
        /// <summary>
        /// Not distinct from GPRS.
        /// </summary>
        EVDO = 5,
        /// <summary>
        /// Not distinct from GPRS.
        /// </summary>
        XEVDV = 6,
        /// <summary>
        /// Not distinct from GPRS.
        /// </summary>
        EDGE = 7,
        /// <summary>
        /// Not distinct from GPRS.
        /// </summary>
        UMTS = 8,
        /// <summary>
        /// Voice
        /// </summary>
        Voice = 9,
        /// <summary>
        /// Push-to-Talk (not supported)
        /// </summary>
        PTT = 10,
        /// <summary>
        /// High-Speed Downlink Packet Access (3.5G).
        /// </summary>
        HSDPA = 11,

        // NIC
        /// <summary>
        /// Wired Ethernet
        /// </summary>
        Ethernet = 14,
        /// <summary>
        /// Wireless
        /// </summary>
        WiFi = 15,

        // Bluetooth
        /// <summary>
        /// Remote Access Services
        /// </summary>
        RAS = 18,
        /// <summary>
        /// Personal Area Network
        /// </summary>
        PAN = 19,

        // Unimodem
        CSD = 22,
        OutOfBandCSD = 23,
        /// <summary>
        /// Direct Cable Connect (DCC)
        /// </summary>
        NullModem = 24,
        /// <summary>
        /// Serial port attached modem
        /// </summary>
        ExternalModem = 25,
        InternalModem = 26,
        PCMCIAModem = 27,
        /// <summary>
        /// DCC over Irda
        /// </summary>
        IRCommModem = 28,
        /// <summary>
        /// Bluetooth modem
        /// </summary>
        DynamicModem = 29,
        /// <summary>
        /// DCC over Bluetooth
        /// </summary>
        DynamicPort = 30,


        // VPN
        L2TP = 33,
        PPTP = 34,

        // PROXY
        NullProxy = 37,
        HTTP = 38,
        WAP = 39,
        Sockets4 = 40,
        Sockets5 = 41,

        // PC
        DesktopPassthrough = 44,
        IR = 45,
        ModemLink = 46,
    }

    [Flags]
    public enum ConnectionOption
    {
        /// <summary>
        /// Connection is billed by time.
        /// </summary>
        BilledByTime = 0x00000001,
        /// <summary>
        /// Connection is always on.
        /// </summary>
        AlwaysOn = 0x00000002,
        /// <summary>
        /// Connection is suspend/resume capable.
        /// </summary>
        SuspendResumeCapable = 0x00000004,
    }
}
