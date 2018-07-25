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
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Threading
{
    /// <summary>
    /// The exception that is thrown when an attempt is made to open a system mutex or semaphore that does not exist.
    /// </summary>
    /// <remarks>Instances of the Mutex class and the Semaphore class can represent named system synchronization objects.
    /// When you use the <see cref="NamedMutex.OpenExisting"/> method or the <see cref="Semaphore.OpenExisting"/> method to open a named system object that does not exist, a <see cref="WaitHandleCannotBeOpenedException"/> is thrown.</remarks>
    public class WaitHandleCannotBeOpenedException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WaitHandleCannotBeOpenedException"/> class with default values.
        /// </summary>
        public WaitHandleCannotBeOpenedException() : base("No handle of the given name exists.")
        {
            base.HResult = -2146233044;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="WaitHandleCannotBeOpenedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        public WaitHandleCannotBeOpenedException(string message) : base(message)
        {
            base.HResult = -2146233044;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="WaitHandleCannotBeOpenedException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="innerException">The exception that is the cause of the current exception.
        /// If the innerException parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception.</param>
        public WaitHandleCannotBeOpenedException(string message, Exception innerException) : base(message, innerException)
        {
            base.HResult = -2146233044;
        }


    }
}
