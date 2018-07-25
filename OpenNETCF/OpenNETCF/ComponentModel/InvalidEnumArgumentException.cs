#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



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