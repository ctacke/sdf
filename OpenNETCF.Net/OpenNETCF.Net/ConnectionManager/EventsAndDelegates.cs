using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net
{
    /// <summary>
    /// Represents the method that will handle an ConnectionStateChanged event
    /// </summary>
    /// <param name="source"></param>
    /// <param name="newStatus"></param>
    public delegate void ConnectionStateChangedHandler(object source, ConnectionStatus newStatus);

    public partial class ConnectionManager
    {
        /// <summary>
        /// Occurs when a connection is opened.
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        /// Occurs when a connection is closed.
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// Occurs when the connection state is changed.
        /// </summary>
        public event ConnectionStateChangedHandler ConnectionStateChanged;

        /// <summary>
        /// Occurs when a connection fails.
        /// </summary>
        public event EventHandler ConnectionFailed;
    }
}
