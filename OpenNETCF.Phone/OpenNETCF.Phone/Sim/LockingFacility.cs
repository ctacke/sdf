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
	/// Specifies the phones locking behavior.
	/// </summary>
	public enum LockingFacility : int
	{
		/// <summary>
		/// Lock control surface
		/// </summary>
		Control							= (0x00000001),
		/// <summary>
		/// Lock phone to SIM card
		/// </summary>
		PhoneToSim						= (0x00000002),
		/// <summary>
		/// Lock phone to first SIM card
		/// </summary>
		PhoneToFirstSim					= (0x00000004), 
		/// <summary>
		/// Lock SIM card
		/// </summary>
		Sim								= (0x00000008),
		/// <summary>
		/// Lock SIM card
		/// </summary>
		SimPin2							= (0x00000010),
		/// <summary>
		/// SIM fixed dialing memory
		/// </summary>
		SimFixedDialing					= (0x00000020), 
		/// <summary>
		/// Network personalization
		/// </summary>
		NetworkPersonalization			= (0x00000040),
		/// <summary>
		/// Network subset personalization
		/// </summary>
		NetworkSubsetPersonalization    = (0x00000080),
		/// <summary>
		/// Service provider personalization
		/// </summary>
		ServiceProviderPersonalization  = (0x00000100),
		/// <summary>
		/// Corporate personalization
		/// </summary>
		CorporatePersonalization        = (0x00000200), 

		//#define SIM_NUMLOCKFACILITIES               10         // @constdefine Number of locking facilities
	}
}
