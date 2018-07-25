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
    /// Specifies changes to watch for in a file or folder.
    /// </summary>
    /// <remarks>You can combine the members of this enumeration to watch for more than one kind of change. For example, you can watch for changes in the size of a file or folder, and for changes in security settings. This raises an event anytime there is a change in size or security settings of a file or folder.</remarks>
    /// <seealso cref="FileSystemMonitor"/>
    /// <seealso cref="FileSystemEventArgs"/>
    /// <seealso cref="FileSystemEventHandler"/>
    /// <seealso cref="RenamedEventArgs"/>
    /// <seealso cref="RenamedEventHandler"/>
    /// <seealso cref="WatcherChangeTypes"/>
    [Flags()]
    public enum NotifyFilters
    {
        /// <summary>
        /// The attributes of the file or folder.
        /// </summary>
        Attributes = 4,
        /// <summary>
        /// The time the file or folder was created.
        /// </summary>
        CreationTime = 64,
        /// <summary>
        /// The name of the directory.
        /// </summary>
        DirectoryName = 2,
        /// <summary>
        /// The name of the file.
        /// </summary>
        FileName = 1,
        /// <summary>
        /// The date the file or folder was last opened.
        /// </summary>
        LastAccess = 32,
        /// <summary>
        /// The date the file or folder last had anything written to it.
        /// </summary>
        LastWrite = 16,
        /// <summary>
        /// The security settings of the file or folder.
        /// </summary>
        Security = 256,
        /// <summary>
        /// The size of the file or folder.
        /// </summary>
        Size = 8,
    }
}
