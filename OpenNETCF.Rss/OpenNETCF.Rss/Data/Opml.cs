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
	/// Identifies the OPML.
	/// </summary>
	public class Opml
	{
		//<title>01.opml</title>
		//<dateCreated>Mon, 01 Aug 2005 07:50:52 GMT</dateCreated>
		//<dateModified>Mon, 01 Aug 2005 14:32:40 GMT</dateModified>
		//<ownerName>Dave Winer</ownerName>
		//<ownerEmail>dave@scripting.com</ownerEmail>

		#region fields
		
		private string title;
		private DateTime dateCreated;
		private DateTime dateModified;
		private string ownerName;
		private string ownerEmail;
		private OpmlItem[] items; 

		#endregion

		#region contructor
		/// <summary>
		/// Initializes a new instance of the Opml class.
		/// </summary>
		public Opml()
		{
			title = String.Empty;
			dateCreated = DateTime.MinValue;
			dateModified = DateTime.MinValue;
			ownerName = String.Empty;
			ownerEmail = String.Empty;
		} 
		#endregion

		#region properties
		/// <summary>
		/// Gets or sets opml items.
		/// </summary>
		public OpmlItem[] Items
		{
			get
			{
				return items;
			}
			set
			{
				items = value;
			}
		}

		/// <summary>
		/// Gets or sets opml Title.
		/// </summary>
		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
			}
		}

		/// <summary>
		/// Gets or sets opml DateCreated.
		/// </summary>
		public DateTime DateCreated
		{
			get
			{
				return dateCreated;
			}
			set
			{
				dateCreated = value;
			}
		}

		/// <summary>
		/// Gets or sets opml DateModified.
		/// </summary>
		public DateTime DateModified
		{
			get
			{
				return dateModified;
			}
			set
			{
				dateModified = value;
			}
		}

		/// <summary>
		/// Gets or sets opml OwnerName.
		/// </summary>
		public string OwnerName
		{
			get
			{
				return ownerName;
			}
			set
			{
				ownerName = value;
			}
		}

		/// <summary>
		/// Gets or sets opml OwnerEmail.
		/// </summary>
		public string OwnerEmail
		{
			get
			{
				return ownerEmail;
			}
			set
			{
				ownerEmail = value;
			}
		} 
		#endregion

	}
}
