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
using System.Net;
using System.Net.Sockets;

namespace OpenNETCF.Net.Ftp
{
	/// <summary>
	/// Summary description for FtpWebResponse.
	/// </summary>
	public class FtpWebResponse : WebResponse, IDisposable
	{
		private FtpDataStream dataStream;
		internal FtpWebResponse( FtpDataStream stream )
		{
			dataStream = stream;
		}

		public override void Close()
		{
			throw new NotSupportedException();
		}

		public override Stream GetResponseStream()
		{
			return dataStream;
		}

		public string BannerMessage
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long ContentLength
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public string ExitMessage
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override WebHeaderCollection Headers
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public DateTime LastModified
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override Uri ResponseUri
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public FtpStatusCode Status // TODO: Implement FtpStatusCode
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public string StatusDescription
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public string WelcomeMessage
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion
	}
}
