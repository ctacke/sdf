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

namespace OpenNETCF.Phone
{
	#region Call Type Enumeration
	/// <summary>
	/// The type of call represented in the <see cref="T:OpenNETCF.Phone.CallLogEntry"/>.
	/// </summary>
	public enum CallType : int
	{
		/// <summary>
		/// An unanswered (missed) incoming call.
		/// </summary>
		Missed,
		/// <summary>
		/// An answered incoming call.
		/// </summary>
		Incoming,
		/// <summary>
		/// An outgoing call.
		/// </summary>
		Outgoing,
	}
	#endregion

	#region CallerID Type Enumeration
	/// <summary>
	/// Specifies the availability of Caller ID.
	/// </summary>
	public enum CallerIDType : int
	{
		/// <summary>
		/// The Caller ID is unavailable.
		/// </summary>
		Unavailable,
		/// <summary>
		/// The Caller ID is blocked.
		/// </summary>
		Blocked,
		/// <summary>
		/// The Caller ID is available.
		/// </summary>
		Available,
	}
	#endregion

	#region Call Log Seek Enumeration
	/// <summary>
	/// Specifies the location within the <see cref="T:OpenNETCF.Phone.CallLog"/> where a search will begin.
	/// </summary>
	public enum CallLogSeek : int
	{ 
		/// <summary>
		/// The search will begin at the start of the call log.
		/// </summary>
		Beginning = 2,
		/// <summary>
		/// The search will begin at the end of the call log.
		/// </summary>
		End = 4
	}   
	#endregion

}
 

 