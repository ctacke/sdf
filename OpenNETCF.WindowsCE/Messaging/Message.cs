using System;

namespace OpenNETCF.WindowsCE.Messaging 
{
    /// <summary>
    /// Base class for messages sent through a P2PMessageQueue
    /// </summary>
	public class Message
    {
		private byte[] mBytes;
		private bool mIsAlert;

		/// <summary>
		/// Creates a new Message (where IsAlert=false).
		/// Do not send a message with no bytes over a queue or an exception is raised
		/// </summary>
		public Message() : this (null, false){}

		/// <summary>
		/// Creates a new Message (where IsAlert=false)
		/// </summary>
		/// <param name="bytes">the bytes making up the message</param>
		public Message(byte[] bytes) : this(bytes, false){}

		/// <summary>
		/// Creates a new Message
		/// </summary>
		/// <param name="bytes">the bytes making up the message</param>
		/// <param name="isAlert">whether the message is a higher priority than existing ones in the queue</param>
		public Message(byte[] bytes, bool isAlert){
			mBytes = bytes;
			mIsAlert = isAlert;
		}

		/// <summary>
		/// Whether the message is an alert message or not
		/// When sending an alert message it is moved to the top of the queue 
		/// (overtaking existing messages in the queue that have not been read already)
		/// NOTE: If setting this property to true causes a native exception when sending, change the name of the queue. It can be an issue with some devices (nothing to do with this wrapper).
		/// </summary>
		public bool IsAlert{
			get {
				return mIsAlert;
			}
			set {
				mIsAlert = value;
			}
		}

		/// <summary>
		/// Gets|sets the bytes for this message.
		/// Your own types can inherit from Message and override this member for sending them over queues
		/// </summary>
		public virtual byte[] MessageBytes{
			get{
				return mBytes;
			}
			set{
				mBytes = value;
			}
		}
	}
}
