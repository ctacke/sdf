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
