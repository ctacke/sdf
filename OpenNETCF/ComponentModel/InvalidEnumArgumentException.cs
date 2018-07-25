using System;
using System.Globalization;

namespace OpenNETCF.ComponentModel
{
	/// <summary>
	/// The exception thrown when using invalid arguments that are enumerators.
	/// </summary>
	public class InvalidEnumArgumentException : System.ArgumentException
	{
		/// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumArgumentException"/> class without a message.
		/// </summary>
		public InvalidEnumArgumentException() {	}

		/// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumArgumentException"/> class with the specified message.
		/// </summary>
		/// <param name="message">The message to display with this exception.</param>
		public InvalidEnumArgumentException(string message) : base(message) {	}

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumArgumentException"/> class with the specified detailed description and the specified exception.
        /// </summary>
        /// <param name="message">A detailed description of the error.</param>
        /// <param name="innerException">A reference to the inner exception that is the cause of this exception.</param>
        public InvalidEnumArgumentException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumArgumentException"/> class with a message generated from the argument, the invalid value, and an enumeration class.
        /// </summary>
        /// <param name="argumentName">The name of the argument that caused the exception.</param>
        /// <param name="invalidValue">The value of the argument that failed.</param>
        /// <param name="enumClass">A <see cref="Type"/> that represents the enumeration class with the valid values.</param>
        public InvalidEnumArgumentException(string argumentName, int invalidValue, Type enumClass)
            : base(string.Format("The value of argument '{0}' ({1}) is invalid for Enum type '{2}'.",
 new object[] { argumentName, invalidValue.ToString(CultureInfo.CurrentCulture), enumClass.Name }), argumentName)
        {
        }

 


 

	}
}