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

namespace OpenNETCF.Phone.Sim
{
	/// <summary>
	/// Describes the current state of the SIM card.
	/// </summary>
	public enum LockedState
	{
		/// <summary>
		/// Locking state is unknown.
		/// </summary>
		Unknown					= (0x00000000),
		/// <summary>
		/// Not awaiting a password (unlocked).
		/// </summary>
		Ready					= (0x00000001), 
		/// <summary>
		/// Awaiting the SIM PIN.
		/// </summary>
		SimPIN					= (0x00000002),
		/// <summary>
		/// Awaiting the SIM PUK.
		/// </summary>
		SimPUK					= (0x00000003),
		/// <summary>
		/// Awaiting the Phone to SIM Personalization PIN
		/// </summary>
		PhoneSimPIN				= (0x00000004),
		/// <summary>
		/// Awaiting the Phone to first SIM Personalization PIN
		/// </summary>
		PhoneFirstSimPIN		= (0x00000005),
		/// <summary>
		/// Awaiting the Phone to first SIM Personalization PUK
		/// </summary>
		PhoneFirstSimPUK		= (0x00000006),
		/// <summary>
		/// Awaiting the SIM PIN2
		/// </summary>
		SimPIN2					= (0x00000007),
		/// <summary>
		/// Awaiting the SIM PUK2
		/// </summary>
		SimPUK2					= (0x00000008),
		/// <summary>
		/// Awaiting the Network Personalization PIN
		/// </summary>
		NetworkPIN				= (0x00000009),
		/// <summary>
		/// Awaiting the Network Personalization PUK
		/// </summary>
		NetworkPUK				= (0x0000000a),
		/// <summary>
		/// Awaiting the Network Subset Personalization PIN
		/// </summary>
		NetworkSubsetPIN		= (0x0000000b),
		/// <summary>
		/// Awaiting the Network Subset Personalization PUK
		/// </summary>
		NetworkSubsetPUK		= (0x0000000c),
		/// <summary>
		/// Awaiting the Service Provider Personalization PIN
		/// </summary>
		ServiceProviderPIN      = (0x0000000d),
		/// <summary>
		/// Awaiting the Service Provider Personalization PUK
		/// </summary>
		ServiceProviderPUK      = (0x0000000e),
		/// <summary>
		/// Awaiting the Corporate Personalization PIN
		/// </summary>
		CorporatePIN			= (0x0000000f),
		/// <summary>
		/// Awaiting the Corporate Personalization PUK
		/// </summary>
		CorporatePUK			= (0x00000010),

	}
}
