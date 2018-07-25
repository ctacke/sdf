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
using System.Runtime.InteropServices;

namespace OpenNETCF.WindowsCE.Messaging
{
    /// <summary>
    /// This class encapsulates a point to point message queue, which can be used for on-device interprocess communication
    /// </summary>
	public class P2PMessageQueue 
    {

		#region Fields
        /// <summary>
        /// When waiting, sending or receiving, wait forever
        /// </summary>
		public const int TimeoutInfinite = -1;
        /// <summary>
        /// Used in construction for unlimited queue length
        /// </summary>
		public const int InfiniteQueueSize = 0;


		private const int InvalidHandle = 0;	// Queue handle must be non-zero
		private const int DefaultMaxMsgLength = 4096; //you can change this default or override in the ctor
		private readonly bool mIsForReading = false;		// set at construction
		private readonly string mName = null;	// For named queues
		private IntPtr hQue = IntPtr.Zero;		// Handle to queue
        private IntPtr hStopEvt = IntPtr.Zero;  // Stop event handle
		private byte[] mReadBuffer = null;		// Read buffer, reused in Receive methods
		#endregion

		#region DataOnQueueChanged Event
		/// <summary>
		/// For a read queue, raised when it is not empty
		/// For a write queue, raised when it is not full
		/// NOTE: Not applicable for Alert Messages!
		/// </summary>
		public event EventHandler DataOnQueueChanged;
		#endregion

		#region Read only properties
		/// <summary>
		/// Native handle to the queue.
		/// </summary>
		public IntPtr Handle
        {
			get { return hQue; }
		}

		/// <summary>
		/// true denotes a reading queue. 
		/// The return value will not change throught the lifetime of the object
		/// </summary>
		public bool CanRead
        {
			get { return mIsForReading; }
		}

		/// <summary>
		/// true denotes a writing queue. 
		/// The return value will not change throught the lifetime of the object
		/// </summary>
		public bool CanWrite
        {
			get { return !mIsForReading; }
		}

		/// <summary>
		/// Null or the name of a named queue 
		/// The return value will not change throught the lifetime of the object
		/// </summary>
		public string QueueName
        {
			get { return mName; }
		}

		/// <summary>
		/// Maximum number of messages allowed in queue, if it is zero, then no restriction on the number of messages.
		/// The return value will not change throught the lifetime of the object.
		/// </summary>
		public int MaxMessagesAllowed
        {
			get { return this.GetInfo().dwMaxMessages; }
		}

		/// <summary>
		/// Maximum length of a message in bytes.
		/// The return value will not change throught the lifetime of the object.
		/// </summary>
		public int MaxMessageLength
        {
			get { return this.GetInfo().cbMaxMessage; }
		}

		/// <summary>
		/// Maximum number of messages that have ever been in the queue at one time.
		/// </summary>
		public int MostMessagesSoFar
        {
			get { return this.GetInfo().dwMaxQueueMessages; }
		}

		/// <summary>
		/// Number of messages currently existing in the queue.
		/// Alert messages are not included in this count.
		/// </summary>
		public int MessagesInQueueNow
        {
			get { return this.GetInfo().dwCurrentMessages; }
		}

		/// <summary>
		/// Number of readers attached to the queue for reading.
		/// </summary>
		public short CurrentReaders
        {
			get { return this.GetInfo().wNumReaders; }
		}

		/// <summary>
		/// Number of writers attached to the queue for writing.
		/// </summary>
		public short CurrentWriters
        {
			get { return this.GetInfo().wNumWriters; }
		}
		#endregion

		#region creation | destruction
		// Called from OpenExisting
		private P2PMessageQueue(IntPtr hwnd, bool forReading)
        {
			if (hwnd.Equals(InvalidHandle))
            {
				throw new System.ComponentModel.Win32Exception();
			}
			hQue = hwnd;
			mIsForReading = forReading;

            if (mIsForReading)
            {
                this.StartEventThread();
            }
		}

		/// <summary>
		/// Creates an unnamed queue with unlimited messages and message length.
		/// </summary>
		/// <param name="forReading">true for a read only queue; false for writing queue</param>
		public P2PMessageQueue(bool forReading) 
            : this(forReading, null, DefaultMaxMsgLength, InfiniteQueueSize)
        {
        }

		/// <summary>
		/// Creates a named queue with unlimited messages and message length.
		/// </summary>
		/// <param name="forReading">true for a read only queue; false for writing queue</param>
		/// <param name="name">Name of queue</param>
		public P2PMessageQueue(bool forReading, string name) 
            : this(forReading, name, DefaultMaxMsgLength, InfiniteQueueSize)
        {
        }

		/// <summary>
		/// Creates a named queue.
		/// </summary>
		/// <param name="forReading">true for a read only queue; false for writing queue</param>
		/// <param name="name">Name of queue</param>
		/// <param name="maxMessageLength">Maximum length of a message in bytes.</param>
		/// <param name="maxMessages">Maximum number of messages allowed in queue, if it is zero, then no restriction on the number of messages.</param>
		public P2PMessageQueue(bool forReading, string name, int maxMessageLength, int maxMessages)
        {
			if (name != null && name.Length > NativeMethods.MAX_PATH)
            {
				throw new ArgumentException("name too long");
			}

			if (maxMessageLength <= 0)
            {
                throw new ArgumentException("maxMessageLength must be positive");
			}

			if (maxMessages < 0)
            {
                throw new ArgumentException("maxMessages must be positive or zero (unbounded)");
			}

			// setup options for creation
            MsgQueueOptions opt = new MsgQueueOptions(forReading, maxMessageLength, maxMessages);

			// configure queue behaviour
			opt.dwFlags = GetBehaviourFlag(); 
		
			try
            {
                hQue = NativeMethods.CreateMsgQueue(name, opt);
			}
            catch (MissingMethodException)
            {
                throw new PlatformNotSupportedException();
			}

			if (hQue.Equals(InvalidHandle))
            {
				throw new ApplicationException("Could not create queue " + name + ", NativeError: " + Marshal.GetLastWin32Error());
			}

			mIsForReading = forReading;
			mName = name;
			
			if (forReading)
            {
				this.mReadBuffer = new byte[maxMessageLength];
                this.StartEventThread();
            }
		}
		
		/// <summary>
		/// Creates a named queue.
		/// </summary>
		/// <param name="forReading">true for a read only queue; false for writing queue</param>
		/// <param name="name">Name of queue</param>
		/// <param name="maxMessageLength">Maximum length of a message in bytes.</param>
		/// <param name="maxMessages">Maximum number of messages allowed in queue, if it is zero, then no restriction on the number of messages.</param>
		/// <param name="createdNew">true, if named queue already existed, otherwise false</param>
		public P2PMessageQueue(bool forReading, string name, int maxMessageLength, int maxMessages, out bool createdNew) 
            : this(forReading, name, maxMessageLength, maxMessages)
        {
            createdNew = (Marshal.GetLastWin32Error() != NativeMethods.ERROR_ALREADY_EXISTS);
		}

		/// <summary>
		/// Frees all resources allocated by the queue. Do not use the object after this
		/// </summary>
		public void Close()
        {
			this.Dispose(false);
		}

		// Called from Close and the Finalizer
		private void Dispose(bool finalizing)
        {
			if (hQue.Equals(InvalidHandle))
            {
				return;
			}
            NativeMethods.EventModify(hStopEvt, (int)NativeMethods.EventFlags.EVENT_SET);
			IntPtr toClose = hQue;
			hQue = IntPtr.Zero;
            NativeMethods.CloseMsgQueue(toClose);
			if (!finalizing)
            {
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
        /// Finalizer for the P2PMessageQueue
		/// </summary>
		~P2PMessageQueue()
        {
			try
            {
				this.Dispose(true);
			}
            catch {/* just swallow any exception on shutdown */}
		}
		#endregion

		#region static OpenExisting
		/// <summary>
		/// Creates a new P2PMessageQueue based on the input parameters
		/// </summary>
		/// <param name="forReading">true for a read only queue; false for writing queue</param>
		/// <param name="processHandle">Native handle to a source process that owns the queueHandle message queue handle.</param>
		/// <param name="queueHandle">Native handle to a message queue returned by the CreateMsgQueue function or a P2PMessageQueue object.</param>
		/// <returns>a new P2PMessageQueue or null if the queue could not be opened</returns>
		public static P2PMessageQueue OpenExisting(bool forReading, IntPtr processHandle, IntPtr queueHandle)
        {
            if ((processHandle.ToInt32() == 0 ) || 
                (processHandle.ToInt32() == -1 ) ||
                 (queueHandle.ToInt32() == 0 ) ||
                (queueHandle.ToInt32() == -1 ))

            {
                throw new ArgumentException();
            }

            IntPtr h = IntPtr.Zero;
            try
            {
                h = NativeMethods.OpenMsgQueue(processHandle, queueHandle, new MsgQueueOptions(forReading));
			}
            catch (MissingMethodException)
            {
                throw new PlatformNotSupportedException();
			}

            if (h.Equals(InvalidHandle))
            {
                return null;
            }

			P2PMessageQueue mq = new P2PMessageQueue(h, forReading);
			if (forReading)
            {
				mq.mReadBuffer = new byte[mq.MaxMessageLength];
			}
			return mq;
		}
		#endregion

		#region Read
		/// <summary>
		/// Reads the next message from the queue. Blocks until there is a message available.
		/// The queue must have been created with forReading = true.
		/// </summary>
		/// <param name="msg">The Message from the queue. Read its bytes if the result is OK.</param>
		/// <returns>a value from the ReadWriteResult enumeration</returns>
		public ReadWriteResult Receive(Message msg)
        {
			return this.Receive(msg, TimeoutInfinite);
		}

		/// <summary>
		/// Reads the next message from the queue.
		/// The queue must have been created with forReading = true.
		/// </summary>
		/// <param name="msg">The Message from the queue. Read its bytes if the result is OK.</param>
		/// <param name="ts">A timespan for which to wait for a message on the queue</param>
		/// <returns></returns>
		public ReadWriteResult Receive(Message msg, TimeSpan ts)
        {
			return this.Receive(msg, ts.Milliseconds); 
		}
		

		/// <summary>
		/// Reads the next message from the queue.
		/// The queue must have been created with forReading = true.
		/// </summary>
		/// <param name="msg">The Message from the queue. Read its bytes if the result is OK.</param>
        /// <param name="timeoutMilliseconds">a timeout in milliseconds for which to block (0 not to block at all). Use TimeoutInfinite to block until the queue has a message.</param>
		/// <returns>a value from the ReadWriteResult enumeration</returns>
		public ReadWriteResult Receive(Message msg, int timeoutMilliseconds)
        {
			if (hQue.Equals(InvalidHandle)){
				throw new ApplicationException("Invalid Handle. Please use new queue");
			}

			int flags;
			int bytesRead;

			// We are re-using the mReadBuffer rather than create a new buffer each time. 
			// The implication is that the function is not thread safe (the whole class isn't but here it isn't by design)
            if (NativeMethods.ReadMsgQueue(hQue, mReadBuffer, mReadBuffer.GetLength(0), out bytesRead, timeoutMilliseconds, out flags))
            {
				msg.MessageBytes = new byte[bytesRead];
				Buffer.BlockCopy(mReadBuffer, 0, msg.MessageBytes, 0, bytesRead);
                msg.IsAlert = (flags == NativeMethods.MSGQUEUE_MSGALERT ? true : false);
				return ReadWriteResult.OK;

			}
            else
            {//Receive failed, get extended info and map it to our returned enum
				msg = null;
				int err = Marshal.GetLastWin32Error();
				switch (err){
                    case NativeMethods.ERROR_INSUFFICIENT_BUFFER:
						return ReadWriteResult.BufferFail;
                    case NativeMethods.ERROR_PIPE_NOT_CONNECTED://no writers if NOT MSGQUEUE_ALLOW_BROKEN
						return ReadWriteResult.Disconnected;
                    case NativeMethods.ERROR_TIMEOUT:
						return ReadWriteResult.Timeout;
                    case NativeMethods.INVALID_HANDLE_ERROR: //This will happen if the caller was blocked on a Read and then subsequently closed the queue
						this.Close();
						return ReadWriteResult.InvalidHandle;
					default:
						throw new ApplicationException("Failed to read: " + err.ToString());
				}
			}
		}
		#endregion

		#region Send
		/// <summary>
		/// Adds a message to the queue (blocking if the queue is full).
		/// The queue must have been created with forReading = false.
		/// </summary>
		/// <param name="msg">The Message to send (contains the bytes)</param>
		/// <returns>a value from the ReadWriteResult enumeration</returns>
		public ReadWriteResult Send(Message msg)
        {
			return this.Send(msg, TimeoutInfinite);
		}

		/// <summary>
		/// Adds a message to the queue.
		/// The queue must have been created with forReading = false.
		/// </summary>
		/// <param name="msg">The Message to send (contains the bytes)</param>
		/// <param name="ts">The TimeSpan for which to wait until the queue is not full</param>
		/// <returns>a value from the ReadWriteResult enumeration</returns>
		public ReadWriteResult Send(Message msg, TimeSpan ts)
        {
			return this.Send(msg, ts.Milliseconds);
		}

		/// <summary>
		/// Adds a message to the queue.
		/// The queue must have been created with forReading = false.
		/// </summary>
		/// <param name="msg">The Message to send (contains the bytes)</param>
        /// <param name="timeoutMilliseconds">a timeout in milliseconds for which to block (0 not to block at all). Use TimeoutInfinite to block until the queue is not full.</param>
		/// <returns>a value from the ReadWriteResult enumeration</returns>
		public ReadWriteResult Send(Message msg, int timeoutMilliseconds)
        {
            if (msg == null) throw new ArgumentNullException("msg");

			if (hQue.Equals(InvalidHandle))
            {
				throw new ApplicationException("Invalid Handle. Please use new queue");
			}

			byte[] bytes = msg.MessageBytes;
			if ((bytes == null) || (bytes.Length == 0))
            {
				throw new ArgumentException("Message must contain bytes");
			}
            if (NativeMethods.WriteMsgQueue(hQue, bytes, bytes.GetLength(0), timeoutMilliseconds, msg.IsAlert ? NativeMethods.MSGQUEUE_MSGALERT : 0))
            {
				return ReadWriteResult.OK;
			}
            else
            {
				msg = null;
				int err = Marshal.GetLastWin32Error();
				switch (err)
                {
                    case NativeMethods.ERROR_INSUFFICIENT_BUFFER:
						return ReadWriteResult.BufferFail;
                    case NativeMethods.ERROR_PIPE_NOT_CONNECTED://no readers if NOT MSGQUEUE_ALLOW_BROKEN
						return ReadWriteResult.Disconnected;
                    case NativeMethods.ERROR_TIMEOUT: //queue is full you get this
						return ReadWriteResult.Timeout;
                    case NativeMethods.ERROR_OUTOFMEMORY: //if MSGQUEUE_NOPRECOMMIT
						return ReadWriteResult.Timeout;
                    case NativeMethods.INVALID_HANDLE_ERROR:
						this.Close();
						return ReadWriteResult.InvalidHandle;
					default:
						throw new ApplicationException("Failed to write: " + err.ToString());
				}
			}
		}
		#endregion

		#region Purge
		/// <summary>
		/// Deletes all the messages contained in the queue.
		/// Applicable only for read queues (CanRead = true)
		/// </summary>
		public void Purge()
        {
			if (this.CanWrite)
            {
				throw new ApplicationException("Queue is write only. Purge not applicable");
			}

			if (hQue.Equals(InvalidHandle))
            {
				throw new ApplicationException("Invalid Handle. Please use new queue");
			}

			ReadWriteResult rwr = ReadWriteResult.OK;
			while (rwr == ReadWriteResult.OK)
            {
				Message msg = new Message();
				rwr = this.Receive(msg, 0);
			}
		}
		#endregion

		#region Private/Protected helper methods
		// On its own thread, monitors the queue handle and raises the DataOnQueueChanged event accordingly
		private void RxMonitorHandle()
        {
            hStopEvt = NativeMethods.CreateEvent(0, true, false, null);
			int res;
			while (!hQue.Equals(InvalidHandle))
            {
                res = NativeMethods.WaitForMultipleObjects(2, new IntPtr[] { hQue, hStopEvt }, 0, TimeoutInfinite);
                if (res == NativeMethods.WAIT_OBJECT_0 + 1)
                {
					return; // queue closed
				}
                if (res == NativeMethods.WAIT_OBJECT_0)
                {
					if (DataOnQueueChanged != null)
                    {
						if (this.CanRead //catches the case where a writer gets created/closed
								&& this.MessagesInQueueNow > 0)
                        { //sideeffect: the event is not raised for Alert messages
							DataOnQueueChanged(this, EventArgs.Empty);//queue not empty
						}
                        else if (this.CanWrite)
                        {
							DataOnQueueChanged(this, EventArgs.Empty);//queue not full
						}
					}
				}
                else
                {
					return; //no point waiting on this handle anymore
				}
			}								  
		}

		// Helper function, called by the various properties
        private NativeMethods.MsgQueueInfo GetInfo()
        {
			if (hQue.Equals(InvalidHandle))
            {
				throw new ApplicationException("Invalid Handle. Please use new queue");
			}

            NativeMethods.MsgQueueInfo qi = new NativeMethods.MsgQueueInfo();
			qi.dwSize = 28;
            if (NativeMethods.GetMsgQueueInfo(hQue, ref qi))
            {
				return qi;
			}
			throw new ApplicationException("Failed to get queue info. NativeError = " + Marshal.GetLastWin32Error());
		}

		/// <summary>
		/// Starts the thread that is responsible for raising the DataOnQueueChanged event
		/// For efficiency, if the client is not going to be catching events, create a 
		/// subclass and override thie method with an empty body
		/// </summary>
		protected virtual void StartEventThread()
        {
			System.Threading.Thread t;
			t = new System.Threading.Thread(new System.Threading.ThreadStart(RxMonitorHandle));
            t.IsBackground = true;
            t.Name = "SDF P2PQueue handle-monitoring thread";
			t.Start();
		}


		/// <summary>
		/// Sets the MsgQueueInfo.dwFlags. See the MSDN documentation for a detailed description.
		/// Inherit and override to change the hardcoded behaviour. Default MSGQUEUE_ALLOW_BROKEN
		/// </summary>
		/// <returns>0, MSGQUEUE_ALLOW_BROKEN or MSGQUEUE_NOPRECOMMIT</returns>
		protected virtual int GetBehaviourFlag()
        {
			//allow readers|writers without writers|readers. Least management.
			//if you care you can always query CurrentWriters, CurrentReaders
            return NativeMethods.MSGQUEUE_ALLOW_BROKEN;
			// If the MSGQUEUE_ALLOW_BROKEN flag is not specified and either the read or write exists, assuming a single read and writer, the queue will be deleted from memory and only the open handle to the queue will exist. 
			// The only option at this point is to close the remaining open handle and reopen the queue if necessary.
		}
		#endregion		
	}
}
