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
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.IO
{
    /// <summary>
    /// Specifies which action to take on files that exist, and which action to take when files do not exist.
    /// </summary>
    public enum FileCreateDisposition : int
    {
        /// <summary>
        /// Creates a new file.
        /// The function fails if the specified file already exists.
        /// </summary>
        CreateNew = 1,
        /// <summary>
        /// Creates a new file.
        /// If the file exists, the function overwrites the file and clears the existing attributes.
        /// </summary>
        CreateAlways = 2,
        /// <summary>
        /// Opens the file.
        /// The function fails if the file does not exist.
        /// </summary>
        OpenExisting = 3,
        /// <summary>
        /// Opens the file, if it exists.
        /// If the file does not exist, the function creates the file as if dwCreationDisposition were <b>CreateNew</b>.
        /// </summary>
        OpenAlways = 4,
        /// <summary>
        /// Opens the file.
        /// Once opened, the file is truncated so that its size is zero bytes. The calling process must open the file with at least Write access.
        /// </summary>
        TruncateExisting = 5,
        // <summary>
        // Not supported.
        // </summary>
        //OpenForLoader = 6,
    }
}
