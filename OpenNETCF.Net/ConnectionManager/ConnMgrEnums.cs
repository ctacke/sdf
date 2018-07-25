using System;
namespace OpenNETCF.Net
{
    /// <summary>
    /// 
    /// </summary>
    public enum ConnectionPriority : int
    {
        /// <summary>
        /// 
        /// </summary>
        Voice = 0x20000,
        /// <summary>
        /// 
        /// </summary>
        UserInteractive = 0x08000,
        /// <summary>
        /// 
        /// </summary>
        UserBackground = 0x02000,
        /// <summary>
        /// 
        /// </summary>
        UserIdle = 0x0800,
        /// <summary>
        /// 
        /// </summary>
        HighPriorityBackground = 0x0200,
        /// <summary>
        /// 
        /// </summary>
        IdleBackground = 0x0080,
        /// <summary>
        /// 
        /// </summary>
        ExternalInteractive = 0x0020,
        /// <summary>
        /// 
        /// </summary>
        LowBackground = 0x0008,
        /// <summary>
        /// 
        /// </summary>
        Cached = 0x0002,
    };

    /// <summary>
    /// Describes the current status of the connection
    /// </summary>
    public enum ConnectionStatus : int
    {
        /// <summary>
        /// Unknown status
        /// </summary>
        Unknown = 0x00,
        /// <summary>
        /// Connection is up
        /// </summary>
        Connected = 0x10,
        Suspended = 0x11, 
        /// <summary>
        /// Connection is disconnected
        /// </summary>
        Disconnected = 0x20,
        /// <summary>
        /// Connection failed and cannot not be re-established
        /// </summary>
        ConnectionFailed = 0x21,
        /// <summary>
        /// User aborted connection
        /// </summary>
        ConnectionCancelled = 0x22,
        /// <summary>
        /// Connection is ready to connect but disabled
        /// </summary>
        ConnectionDisabled = 0x23,
        /// <summary>
        /// No path could be found to destination
        /// </summary>
        NoPathToDestination = 0x24,
        /// <summary>
        /// Waiting for a path to the destination
        /// </summary>
        WaitingForPath = 0x25,
        /// <summary>
        /// Voice call is in progress
        /// </summary>
        WaitingForPhone = 0x26,
        PhoneOff = 0x27, 
        ExclusiveConflict = 0x28,
        NoResources = 0x29,
        ConnectionLinFailed = 0x2a,
        AuthenticationFailed = 0x2b,
        /// <summary>
        /// Attempting to connect
        /// </summary>
        WaitingConnection = 0x40,
        /// <summary>
        /// Resource is in use by another connection
        /// </summary>
        WaitingForResource = 0x41,
        /// <summary>
        /// No path could be found to destination
        /// </summary>
        WaitingForNetwork = 0x42,
        /// <summary>
        /// Connection is being brought down
        /// </summary>
        WaitingDisconnection = 0x80,
        /// <summary>
        /// Aborting connection attempt
        /// </summary>
        WaitingConnectionAbort = 0x81,
    };

    /// <summary>
    /// The type of connection to establish
    /// </summary>
    public enum ConnectionMode : int
    {
        /// <summary>
        /// Connect synchronously
        /// </summary>
        Synchronous = 0,
        /// <summary>
        /// Connect asynchronously
        /// </summary>
        Asynchronous = 1,
    }

}
