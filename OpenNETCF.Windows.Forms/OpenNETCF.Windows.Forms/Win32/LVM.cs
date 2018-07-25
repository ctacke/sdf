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
	/// ListView Messages.
	/// </summary>
	public enum LVM: int
	{
		FIRST = 0x1000,
		GETHEADER = FIRST + 31,
		SETICONSPACING = FIRST + 53,
		GETSUBITEMRECT = FIRST + 56,
		GETITEMSTATE = FIRST + 44,
		GETITEMTEXTW = FIRST + 115, 
		INSERTITEMA = FIRST + 7,
		INSERTITEMW = FIRST + 77,
		INSERTCOLUMNA = FIRST + 27,
		INSERTCOLUMNW = FIRST + 97,
		DELETECOLUMN = FIRST + 28, 
		GETCOLUMNA = FIRST + 25,
		GETCOLUMNW = FIRST + 95, 
		SETEXTENDEDLISTVIEWSTYLE = FIRST + 54, 
		SETITEMA = FIRST + 6,
		SETITEMW = FIRST + 76,
		EDITLABELA = FIRST + 23,
		EDITLABELW = FIRST + 118,
		DELETEITEM = FIRST + 8,
		SETBKCOLOR = FIRST + 1,
		GETBKCOLOR = FIRST + 0, 
		GETTEXTBKCOLOR = FIRST + 37,
		SETTEXTBKCOLOR = FIRST + 38,
		DELETEALLITEMS = FIRST + 9,
		GETNEXTITEM = FIRST + 12,
		SETITEMCOUNT = FIRST + 47,
		GETITEMCOUNT = FIRST + 4,
		SETCOLUMNWIDTH = FIRST + 30,
		GETITEMRECT = FIRST + 14,
		EDITLABEL = FIRST + 23,
	}

}
