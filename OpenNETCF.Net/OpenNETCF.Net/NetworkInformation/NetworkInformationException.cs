using System;
using System.ComponentModel;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// The exception that is thrown when an error occurs while retrieving network
    /// information.
    /// </summary>
    public class NetworkInformationException : Exception
    {
        int m_errorCode = 0;

        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.NetworkInformationException
        /// class.
        /// </summary>
        public NetworkInformationException()
            : base() 
        {
            m_errorCode = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
        }

        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.NetworkInformationException
        /// class with the specified error code.
        /// </summary>
        /// <param name="error">A Win32 error code.</param>
        public NetworkInformationException(int error)
            : base() { m_errorCode = error; }

        /// <summary>
        /// Gets the Win32 error code for this exception. 
        /// </summary>
        public int ErrorCode
        {
            get { return m_errorCode; }
        }
    }
}
