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
    /// The exception that is thrown when the <see cref="OpenNETCF.Threading.Semaphore.Release()"/> method is called on a semaphore whose count is already at the maximum.
    /// </summary>
    /// <remarks>The count on a semaphore is decremented each time a thread enters the semaphore, and incremented when a thread releases the semaphore.
    /// When the count is zero, subsequent requests block until other threads release the semaphore.
    /// When all threads have released the semaphore, the count is at the maximum value specified when the semaphore was created.
    /// If a programming error causes a thread to call the <see cref="OpenNETCF.Threading.Semaphore.Release()"/> method at this point, a <see cref="SemaphoreFullException"/> is thrown.</remarks>
    public class SemaphoreFullException : SystemException 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SemaphoreFullException"/> class with default values.
        /// </summary>
        public SemaphoreFullException() : base("Adding the given count to the semaphore would cause it to exceed it's maximum count.") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="SemaphoreFullException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public SemaphoreFullException(string message) : base(message) {}
        /// <summary>
        /// Initializes a new instance of the <see cref="SemaphoreFullException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.
        /// If the innerException parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception.</param>
        public SemaphoreFullException( string message, Exception innerException) : base(message,innerException) {}
    }
}
