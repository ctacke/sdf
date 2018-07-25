using System;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// The exception that is thrown when an error occurs while retrieving network information.
	/// </summary>
	public class NetworkInformationException : Exception
	{
		int errorCode;

		/// <summary>
		/// Initializes a new instance of the <see cref="NetworkInformationException"/> class.
		/// </summary>
		public NetworkInformationException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NetworkInformationException"/> class.
		/// </summary>
		/// <param name="errorCode">A Win32 error code.</param>
		public NetworkInformationException(int errorCode)
		{
			this.errorCode = errorCode;
		}

		/// <summary>
		/// Gets the Win32 error code for this exception.
		/// </summary>
		public int ErrorCode
		{
            get
            {
                return errorCode;            	
            }
		}
	}
}
