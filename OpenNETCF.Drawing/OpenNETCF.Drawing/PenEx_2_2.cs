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
using System.Drawing;
using OpenNETCF.Drawing.Drawing2D;

namespace OpenNETCF.Drawing
{
	public partial class PenEx
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="PenEx"/> class with the <see cref="Color"/>.  
		/// </summary>
		/// <param name="color">The <see cref="Color"/> of the <see cref="PenEx"/>.</param>
		/// <param name="width">The <see cref="Width"/> of the <see cref="PenEx"/>.</param>
		/// <param name="style">The <see cref="DashStyle"/> of the <see cref="PenEx"/>.</param>
		public PenEx(Color color, DashStyle style, int width)
		{
			this.color = color;
			this.penStyle = style;
			this.width = width;
			hPen = GDIPlus.CreatePen((int)style, width, ColorTranslator.ToWin32(color)/*GDIPlus.RGB(color)*/);
		}

		#endregion


	}

	
}
