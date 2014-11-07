namespace OpenNETCF.WindowsCE.Messaging
{
	/// <summary>
	/// Returned by Send and Read methods of P2PMessageQueue
	/// If OK, the message was sent/read
	/// </summary>
	public enum ReadWriteResult{
		/// <summary>
		/// The Message was read from the queue OR added tot he queue succesfully
		/// </summary>
		OK,

		/// <summary>
		/// Receive: no message on the queue
		/// Send: the queue is full
		/// </summary>
		Timeout,

		/// <summary>
		/// There is no reader/writer on the other end
		/// </summary>
		Disconnected,

		/// <summary>
		/// Message on queue larger than the buffer allocated
		/// </summary>
		BufferFail,

		/// <summary>
		/// Not enough memory to allocate buffer for message
		/// </summary>
		OutOfMemory,

		/// <summary>
		/// Returned if you block on a Receive/Send and the queue is closed
		/// </summary>
		InvalidHandle
	}
}

 

 