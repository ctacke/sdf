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
using System.Collections;
using System.Text;

namespace OpenNETCF.Rss.Data
{
	/// <summary>
	/// Identifies the OPML item.
	/// </summary>
	public class OpmlItem
	{
		/// <summary>
		/// Initializes a new instance of the OpmlItem class.
		/// </summary>
		public OpmlItem()
		{
			Title = String.Empty;
			HtmlUrl = String.Empty;
			XmlUrl = String.Empty;
			Type = String.Empty;
		}
		
		#region properties
		/// <summary>
		/// Gets or sets OPML item's sub items
		/// </summary>
		public OpmlItem[] Items { get; set; }

		/// <summary>
		/// Gets or sets OPML item's Title
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets OPML item's HtmlUrl
		/// </summary>
		public string HtmlUrl { get; set; }

		/// <summary>
		/// Gets or sets OPML item's XmlUrl
		/// </summary>
		public string XmlUrl { get; set; }

		/// <summary>
		/// Gets or sets OPML item's Type
		/// </summary>
		public string Type { get; set; }

        #endregion

	
	}
}
