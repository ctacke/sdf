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
using System.Threading;

namespace OpenNETCF.Threading
{
	/// <summary>
	/// Enhanced alternative to the <see cref="T:System.Threading.Monitor"/> class.
	/// Provides a mechanism that synchronizes access to objects.
	/// </summary>
	/// <seealso cref="T:System.Threading.Monitor"/>
	public sealed class Monitor2
	{
		/// <summary>
		/// The owner of the monitor, or null if it's not owned
		/// by any thread.
		/// </summary>
		Thread currentOwner=null;

		/// <summary>
		/// Number of levels of locking (0 for an unowned
		/// monitor, 1 after a single call to Enter, etc).
		/// </summary>
		int lockCount=0;

		/// <summary>
		/// Object to be used as a monitor for state changing.
		/// </summary>
		object stateLock = new object();

		/// <summary>
		/// AutoResetEvent used to implement Wait/Pulse/PulseAll.
		/// Initially not signalled, so that a call to Wait will
		/// block until the first pulse.
		/// </summary>
		AutoResetEvent waitPulseEvent = new AutoResetEvent(false);

		/// <summary>
		/// Number of threads waiting on this monitor.
		/// </summary>
		int waitCounter=0;

		/// <summary>
		/// Event used for Enter/Exit. Initially signalled
		/// to allow the first thread to come in.
		/// </summary>
		AutoResetEvent enterExitEvent = new AutoResetEvent(true);

		/// <summary>
		/// Creates a new monitor, not owned by any thread.
		/// </summary>
		public Monitor2()
		{
		}

		/// <summary>
		/// Enters the monitor (locks it), blocking until the
		/// lock is held. If the monitor is already held by the current thread,
		/// its lock count is incremented.
		/// </summary>
		public void Enter()
		{
			Thread currentThread = Thread.CurrentThread;
			while (true)
			{
				lock (stateLock)
				{
					if(currentOwner==currentThread)
					{
						lockCount++;
						return;
					}
				}
				
				enterExitEvent.WaitOne();
				lock (stateLock)
				{
					if (currentOwner==null)
					{
						currentOwner=currentThread;
						lockCount=1;
						enterExitEvent.Reset();
						return;
					}
				}
			}
		}

		/// <summary>
		/// Attempts to enter the monitor (locking it) but does not block
		/// if the monitor is already owned.
		/// </summary>
		/// <returns>Whether or not the current thread now owns the monitor.
		/// </returns>
		public bool TryEnter()
		{
			lock (stateLock)
			{
				if (currentOwner==null)
				{
					currentOwner=Thread.CurrentThread;
					lockCount=1;
					enterExitEvent.Reset();
					return true;
				}
				else if (currentOwner==Thread.CurrentThread)
				{
					lockCount++;
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// Releases a level of locking, unlocking the monitor itself
		/// if the lock count becomes 0.
		/// </summary>
		/// <exception cref="SynchronizationLockException">If the current 
		/// thread does not own the monitor.</exception>
		public void Exit()
		{
			lock (stateLock)
			{
				if (currentOwner != Thread.CurrentThread)
				{
					throw new 
						SynchronizationLockException
						("Cannot Exit a monitor owned by a different thread.");
				}
				lockCount--;
				if (lockCount==0)
				{
					currentOwner=null;
					enterExitEvent.Set();
				}
			}
		}

		/// <summary>
		/// Pulses the monitor once - a single waiting thread will be released
		/// and continue its execution after the current thread has exited the
		/// monitor. Unlike Pulse on the normal framework, no guarantee is
		/// made about which thread is woken.
		/// </summary>
		/// <exception cref="SynchronizationLockException">If the 
		/// current thread does not own the monitor.</exception>
		public void Pulse()
		{
			lock (stateLock)
			{
				if (currentOwner != Thread.CurrentThread)
				{
					throw new 
						SynchronizationLockException
						("Cannot Pulse a monitor owned by a different thread.");
				}
				// Don't bother setting the event if no-one's waiting - we'd only end
				// up having to reset the event manually.
				if (waitCounter==0)
				{
					return;
				}
				waitPulseEvent.Set();
				waitCounter--;
			}
		}

		/// <summary>
		/// Pulses the monitor such that all waiting threads are woken up.
		/// All threads will then try to regain the lock on this monitor.
		/// No order for regaining the lock is specified.
		/// </summary>
		/// <exception cref="SynchronizationLockException">If the current 
		/// thread does not own the monitor.</exception>
		public void PulseAll()
		{
			lock (stateLock)
			{
				if (currentOwner != Thread.CurrentThread)
				{
					throw new 
						SynchronizationLockException
						("Cannot Pulse a monitor owned by a different thread.");
				}
				for (int i=0; i < waitCounter; i++)
				{
					waitPulseEvent.Set();
				}
				waitCounter=0;
			}
		}

		/// <summary>
		/// Relinquishes the lock on this monitor (whatever the lock count is)
		/// and waits for the monitor to be pulsed. After the monitor has been 
		/// pulsed, the thread blocks again until it has regained the lock (at 
		/// which point it will have the same lock count as it had before), and 
		/// then the method returns.
		/// </summary>
		public void Wait()
		{
			int oldLockCount;
			lock (stateLock)
			{
				if (currentOwner != Thread.CurrentThread)
				{
					throw new 
						SynchronizationLockException
						("Cannot Wait on a monitor owned by a different thread.");
				}
				oldLockCount = lockCount;
				// Make Exit() set the enterExitEvent
				lockCount=1;
				Exit();
				waitCounter++;
			}
			waitPulseEvent.WaitOne();
			Enter();
			// By now we own the lock again
			lock (stateLock)
			{
				lockCount=oldLockCount;
			}
		}
	}

	/// <summary>
	/// Exception thrown by <see cref="T:OpenNETCF.Threading.Monitor2"/> when threading rules
	/// are violated (usually due to an operation being
	/// invoked on a monitor not owned by the current thread).
	/// </summary>
	public class SynchronizationLockException : SystemException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SynchronizationLockException"/> class with a specified error message.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		public SynchronizationLockException(string message) : 
			base(message)
		{
		}
	}
}
