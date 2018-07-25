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

namespace OpenNETCF.Win32
{
	/// <summary>
	/// MonthCalendar Messages.
	/// </summary>
	internal enum MCM : int
	{
		FIRST				= 0x1000,
		GETCURSEL			= (FIRST + 1),
		SETCURSEL			= (FIRST + 2),
		GETMAXSELCOUNT		= (FIRST + 3),
		SETMAXSELCOUNT		= (FIRST + 4),
		GETSELRANGE			= (FIRST + 5),
		SETSELRANGE			= (FIRST + 6),
		GETMONTHRANGE		= (FIRST + 7),
		SETDAYSTATE			= (FIRST + 8),
		GETMINREQRECT		= (FIRST + 9),
		SETCOLOR            = (FIRST + 10),
		GETCOLOR            = (FIRST + 11),
		SETTODAY			= (FIRST + 12),
		GETTODAY			= (FIRST + 13),
		HITTEST				= (FIRST + 14),
		SETFIRSTDAYOFWEEK	= (FIRST + 15),
		GETFIRSTDAYOFWEEK	= (FIRST + 16),
		GETRANGE			= (FIRST + 17),
		SETRANGE			= (FIRST + 18),
		GETMONTHDELTA		= (FIRST + 19),
		SETMONTHDELTA		= (FIRST + 20),
		GETMAXTODAYWIDTH	= (FIRST + 21),
		GETMAXNONEWIDTH		= (FIRST + 22),
	}
}
