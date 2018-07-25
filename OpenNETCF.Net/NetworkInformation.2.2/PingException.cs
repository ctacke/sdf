using System;

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// The exception that is thrown when a Send method calls a method that throws an exception.
	/// </summary>
	public class PingException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the PingException class using the specified message.
		/// </summary>
		/// <param name="message">A String that describes the error.</param>
		public PingException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the PingException class using the specified message and inner exception.
		/// </summary>
		/// <param name="message">A String that describes the error.</param>
		/// <param name="innerException">The exception that causes the current exception.</param>
		public PingException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
