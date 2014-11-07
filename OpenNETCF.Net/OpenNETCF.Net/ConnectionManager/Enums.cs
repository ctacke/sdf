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
