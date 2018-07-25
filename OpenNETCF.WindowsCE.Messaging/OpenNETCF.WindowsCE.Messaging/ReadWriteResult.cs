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

 

 