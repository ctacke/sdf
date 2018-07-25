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

namespace OpenNETCF.Net
{
	/// <summary>
	/// The AdapterException class indicates an exception during
	/// an adapter query, modification, or other operation.
	/// </summary>
	public class AdapterException : System.Exception
	{
		/// <summary>
		/// Basic constructor.  No message or error code number.
		/// </summary>
		public AdapterException() : base() {}

		/// <summary>
		/// Basic constructor using the message string of the base
		/// class.
		/// </summary>
		/// <param name="message">
		/// Message string for base class
		/// </param>
		public AdapterException(string message) : base(message) {}

		/// <summary>
		/// Constructor to which additional error code information,
		/// perhaps from a Windows Zero Config call, might be passed.
		/// </summary>
		/// <param name="errcode">
		/// Error code, available for return from HRESULT member.
		/// </param>
		public AdapterException(int errcode) : base()
		{
            this.HResult = errcode;
		}

		/// <summary>
		/// Constructor which takes both string message (passed to
		/// base Exception class), and error code value.
		/// </summary>
		/// <param name="errcode">
		/// Error code, available for return from HRESULT member.
		/// </param>
		/// <param name="message">
		/// Message string for base class
		/// </param>
		public AdapterException(int errcode, string message) : base(message)
		{
			this.HResult = errcode;
		}
	}
}
