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

namespace OpenNETCF.Drawing
{
	/// <summary>
	/// Specifies alignment of content on the drawing surface.
	/// </summary>
	public enum ContentAlignment2
	{
		/// <summary>
		/// Content is vertically aligned at the bottom, and horizontally aligned at the center.
		/// </summary>
		BottomCenter	= 0x200,
		/// <summary>
		/// Content is vertically aligned at the bottom, and horizontally aligned on the left.
		/// </summary>
		BottomLeft		= 0x100,
		/// <summary>
		/// Content is vertically aligned at the bottom, and horizontally aligned on the right.
		/// </summary>
		BottomRight		= 0x400,
		/// <summary>
		/// Content is vertically aligned in the middle, and horizontally aligned at the center.
		/// </summary>
		MiddleCenter	= 0x020,
		/// <summary>
		/// Content is vertically aligned in the middle, and horizontally aligned on the left.
		/// </summary>
		MiddleLeft		= 0x010,
		/// <summary>
		/// Content is vertically aligned in the middle, and horizontally aligned on the right.
		/// </summary>
		MiddleRight		= 0x040,
		/// <summary>
		/// Content is vertically aligned at the top, and horizontally aligned at the center.
		/// </summary>
		TopCenter		= 0x002,
		/// <summary>
		/// Content is vertically aligned at the top, and horizontally aligned on the left.
		/// </summary>
		TopLeft			= 0x001,
		/// <summary>
		/// Content is vertically aligned at the top, and horizontally aligned on the right.
		/// </summary>
		TopRight		= 0x004
	}
}