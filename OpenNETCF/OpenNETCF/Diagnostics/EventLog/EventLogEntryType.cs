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

namespace OpenNETCF.Diagnostics
{
	/// <summary>
	/// Specifies the event type of an event log entry.
	/// </summary>
	/// <remarks>
	/// The type of an event log entry is used to indicate the severity of an event log entry.
	/// Each event must be of a single type, which the application indicates when it reports the event.
	/// </remarks>
	public enum EventLogEntryType
	{
		/// <summary>
		/// An error event. This indicates a significant problem the user should know about; usually a loss of functionality or data.
		/// </summary>
		Error,
		/// <summary>
		/// A failure audit event. This indicates a security event that occurs when an audited access attempt fails; for example, a failed attempt to open a file.
		/// </summary>
		FailureAudit,
		/// <summary>
		/// An information event. This indicates a significant, successful operation.
		/// </summary>
		Information,
		/// <summary>
		/// A success audit event. This indicates a security event that occurs when an audited access attempt is successful; for example, logging on successfully.
		/// </summary>
		SuccessAudit,
		/// <summary>
		/// A warning event. This indicates a problem that is not immediately significant, but that may signify conditions that could cause future problems.
		/// </summary>
		Warning
	}
}
