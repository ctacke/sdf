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
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace OpenNETCF.Windows.Forms
{ 
    /// <summary>
    /// Specifies constants indicating which elements of the Help file to display.
    /// </summary>
    /// <seealso cref="T:System.Windows.Forms.HelpNavigator">System.Windows.Forms.HelpNavigator Enum</seealso>
    public enum HelpNavigator
    {
        /*/// <summary>
        /// Specifies that the index for a specified topic is performed in the specified URL.
        /// </summary>
        AssociateIndex,*/
        /// <summary>
        /// Specifies that the search page of a specified URL is displayed.
        /// </summary>
        Find,
        /*/// <summary>
        /// Specifies that the index of a specified URL is displayed.
        /// </summary>
        Index,*/
        /*/// <summary>
        /// Specifies a keyword to search for and the action to take in the specified URL.
        /// </summary>
        KeywordIndex,*/
        /// <summary>
        /// Specifies that the table of contents of the specfied URL is displayed.
        /// </summary>
        TableOfContents,
        /// <summary>
        /// Specifies that the topic referenced by the specified URL is displayed.
        /// </summary>
        Topic,
    }
}
