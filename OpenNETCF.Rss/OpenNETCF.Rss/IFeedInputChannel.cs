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
using System.Collections;
using System.Text;

using OpenNETCF.Rss.Data;

namespace OpenNETCF.Rss
{
    /// <summary>
	/// Defines the members for a feed input channel. 
    /// </summary>
	public interface IFeedInputChannel : IFeedChannel
    {
        /// <summary>
		/// Starts an asynchronous receive operation. 
        /// </summary>
        /// <param name="callback">An System.AsyncCallback that represents the callback method.</param>
        /// <param name="state">An object that can be used to access state information of the asynchronous operation.</param>
		/// <returns>The System.IAsyncResult that identifies the asynchronous request. </returns>
		IAsyncResult BeginReceive(AsyncCallback callback, object state);

		/// <summary>
		/// Ends a pending asynchronous receive operation. 
		/// </summary>
		/// <param name="result">The System.IAsyncResult that identifies the asynchronous receive operation to finish.</param>
		/// <returns>A Feed object that is received. </returns>
        Feed EndReceive(IAsyncResult result);

		/// <summary>
		/// Receives a message on the channel. 
		/// </summary>
		/// <returns>A Feed object that is received.</returns>
        Feed Receive();
    }
}
