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
		public Message(byte[] bytes, bool isAlert)
        {
            if ((bytes != null) && (bytes.Rank > 1))
            {
                throw new ArgumentException("MessageBytes must be a one-dimensional array");
            }
            
            mBytes = bytes;
			mIsAlert = isAlert;
		}

		/// <summary>
		/// Whether the message is an alert message or not
		/// When sending an alert message it is moved to the top of the queue 
		/// (overtaking existing messages in the queue that have not been read already)
		/// NOTE: If setting this property to true causes a native exception when sending, change the name of the queue. It can be an issue with some devices (nothing to do with this wrapper).
		/// </summary>
		public bool IsAlert
        {
			get { return mIsAlert; }
			set { mIsAlert = value; }
		}

		/// <summary>
		/// Gets|sets the bytes for this message.
		/// Your own types can inherit from Message and override this member for sending them over queues
		/// </summary>
		public virtual byte[] MessageBytes
        {
			get { return mBytes; }
			set 
            {
                if ((value != null) && (value.Rank > 1))
                {
                    throw new ArgumentException("MessageBytes must be a one-dimensional array");
                }
                mBytes = value; 
            }
		}
	}
}
