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
using System.IO;
using System.Text;

namespace OpenNETCF.Rss
{
	
	internal class ReceiveAsyncResult : IAsyncResult
	{
		private Stream stream;
		private FeedHttpConnection connection;

		public ReceiveAsyncResult(FeedHttpConnection connection, Stream stream)
		{
			this.connection = connection;
			this.stream = stream;
		}

		public Stream DataStream
		{
			get
			{
				return stream;
			}
		}

		public FeedHttpConnection Connection
		{
			get
			{
				return connection;
			}
		}



		#region IAsyncResult Members

		public object AsyncState
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public System.Threading.WaitHandle AsyncWaitHandle
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public bool CompletedSynchronously
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public bool IsCompleted
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		#endregion
}
}
