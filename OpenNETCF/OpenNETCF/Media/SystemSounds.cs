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
using System.Runtime.InteropServices;

namespace OpenNETCF.Media
{
	/// <summary>
	/// Retrieves sounds associated with a set of Windows system sound event types.
	/// </summary>
	public static class SystemSounds
	{
		/// <summary>
		/// Gets the sound associated with the Beep program event.
		/// </summary>
		public static SystemSound Beep
		{
			get
			{
				return new SystemSound(0);
			}
		}


		/// <summary>
		/// Gets the sound associated with the Asterisk program event.
		/// </summary>
		public static SystemSound Asterisk
		{
			get
			{
				return new SystemSound(0x00000040);
			}
		}

		/// <summary>
		/// Gets the sound associated with the Exclamation program event.
		/// </summary>
		public static SystemSound Exclamation
		{
			get
			{
				return new SystemSound(0x00000030);
			}
		}

		
		/// <summary>
		/// Gets the sound associated with the Question program event.
		/// </summary>
		public static SystemSound Question
		{
			get
			{
				return new SystemSound(0x00000020);
			}
		}
		
		/// <summary>
		/// Gets the sound associated with the Hand program event.
		/// </summary>
		public static SystemSound Hand
		{
			get
			{
				return new SystemSound(0x00000010);
			}
		}
	}
}
