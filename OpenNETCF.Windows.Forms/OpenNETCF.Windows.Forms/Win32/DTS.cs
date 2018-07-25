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
	/// DateTimePicker Styles.
	/// </summary>
	[Flags()]
	internal enum DTS : int
	{
		UPDOWN = 0x0001, // use UPDOWN instead of MONTHCAL
		SHOWNONE = 0x0002, // allow a NONE or checkbox selection
		SHORTDATEFORMAT = 0x0000, // use the short date format (app must forward WM_WININICHANGE messages)
		LONGDATEFORMAT = 0x0004, // use the long date format (app must forward WM_WININICHANGE messages)
		TIMEFORMAT = 0x0009, // use the time format (app must forward WM_WININICHANGE messages)
		APPCANPARSE = 0x0010, // allow user entered strings (app MUST respond to DTN_USERSTRING)
		RIGHTALIGN = 0x0020, // right-align popup instead of left-align it
		NONEBUTTON = 0x0080, // use NONE button instead of checkbox
	}
}
