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
	/// Specifies the numbering plan used in the <see cref="PhonebookEntry.Address"/>.
	/// </summary>
	public enum NumberPlan : int
	{
		/// <summary>
		/// Unknown numbering.
		/// </summary>
		Unknown             =(0x00000000),
		/// <summary>
		/// ISDN/telephone numbering plan (E.164/E.163)
		/// </summary>
		Telephone           =(0x00000001),
		/// <summary>
		/// Data numbering plan (X.121)
		/// </summary>
		Data                =(0x00000002),
		/// <summary>
		/// Telex numbering plan
		/// </summary>
		Telex               =(0x00000003),
		/// <summary>
		/// National numbering plan
		/// </summary>
		National            =(0x00000004),
		/// <summary>
		/// Private numbering plan
		/// </summary>
		Private             =(0x00000005),
		/// <summary>
		/// ERMES numbering plan (ETSI DE/PS 3 01-3)
		/// </summary>
		Ermes               =(0x00000006),
	}
}
