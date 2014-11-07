using System;
using System.ComponentModel;

namespace OpenNETCF.Net
{
    public partial class ConnectionManager
    {
        /// <summary>
        /// Establish a connection with Connection Manager using the specified connection.
        /// </summary>
        /// <returns>A new instance of <see cref="ConnectionManager" />.</returns>
        static public ConnectionManager EstablishConnection(string connectionName)
        {
            ConnectionManager connMgr = new ConnectionManager();
            connMgr.Connect(connectionName);
            return connMgr;
        }

        /// <summary>
        /// Establish a connection asynchronously with Connection Manager using the specified connection.
        /// </summary>
        /// <returns>A new instance of <see cref="ConnectionManager" />.</returns>
        static public ConnectionManager EstablishConnectionAsync(string connectionName)
        {
            ConnectionManager connMgr = new ConnectionManager();
            connMgr.Connect(connectionName, ConnectionMode.Asynchronous);
            return connMgr;
        }

        /// <summary>
        /// Establish a connection with Connection Manager using the specified connection.
        /// </summary>
        /// <param name="connectionName">The name of the connection to use, e.g. "My GPRS Connection"</param>
        public void Connect(string connectionName)
        {
            this.Connect(connectionName, false, ConnectionMode.Synchronous);
        }

        /// <summary>
        /// Establish a connection with Connection Manager using the specified connection.
        /// </summary>
        /// <param name="connectionName">The name of the connection to use, e.g. "My GPRS Connection"</param>
        /// <param name="exclusive">Determines whether the connection should be exclusive or not.</param>
        public void Connect(string connectionName, bool exclusive)
        {
            this.Connect(connectionName, exclusive, ConnectionMode.Synchronous);
        }

        /// <summary>
        /// Establish a connection with Connection Manager using the specified connection.
        /// </summary>
        /// <param name="connectionName">The name of the connection to use, e.g. "My GPRS Connection"</param>
        /// <param name="mode">Determines how the connection is to be made: either Synchronous or Asynchronous</param>
        public void Connect(string connectionName, ConnectionMode mode)
        {
            this.Connect(connectionName, false, mode);
        }

        /// <summary>
        /// Establish a connection with Connection Manager using the specified connection.
        /// </summary>
        /// <param name="connectionName">The name of the connection to use, e.g. "My GPRS Connection"</param>
        /// <param name="exclusive">Determines whether the connection should be exclusive or not.</param>
        /// <param name="mode">Determines how the connection is to be made: either Synchronous or Asynchronous</param>
        public void Connect(string connectionName, bool exclusive, ConnectionMode mode)
        {
            if (String.IsNullOrEmpty(connectionName))
            {
                throw new ArgumentNullException("connectionName");
            }

            Guid destGuid = Guid.Empty;

            if ((SafeNativeMethods.ConnMgrMapConRef(ConnMgrConRefType.NAP, connectionName, out destGuid)) != 0)
            {
                int hr = SafeNativeMethods.ConnMgrMapConRef(ConnMgrConRefType.Proxy, connectionName, out destGuid);
                if (hr != 0)
                {
                    throw new Win32Exception(hr, "An exception occurred when mapping the connection reference.");
                }
            }

            this.Connect(destGuid, exclusive, mode);
        }
    }
}
