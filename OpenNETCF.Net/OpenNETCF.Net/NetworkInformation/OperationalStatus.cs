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

namespace OpenNETCF.Net.NetworkInformation
{
	/// <summary>
	/// Specifies the operational state of a network interface.
	/// </summary>
	public enum OperationalStatus
	{
		/// <summary>
		/// The network interface is not in a condition to transmit data packets; it is waiting for an external event.
		/// </summary>
		Dormant			= 5,
		/// <summary>
		/// The network interface is unable to transmit data packets.
		/// </summary>
		Down			= 2,
		/// <summary>
		/// The network interface is unable to transmit data packets because it runs on top of one or more other interfaces, and at least one of these "lower layer" interfaces is down.
		/// </summary>
		LowerLayerDown	= 7,
		/// <summary>
		/// The network interface is unable to transmit data packets because of a missing component, typically a hardware component.
		/// </summary>
		NotPresent		= 6,
		/// <summary>
		/// The network interface is running tests.
		/// </summary>
		Testing			= 3,
		/// <summary>
		/// The network interface status is not known.
		/// </summary>
		Unknown			= 4,
		/// <summary>
		/// The network interface is up; it can transmit data packets.
		/// </summary>
		Up				= 1
	}
}
