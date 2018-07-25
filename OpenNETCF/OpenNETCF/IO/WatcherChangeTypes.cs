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
    /// Changes that might occur to a file or directory.
    /// </summary>
    /// <remarks>Each <see cref="WatcherChangeTypes"/> member is associated with an event in <see cref="FileSystemMonitor"/>.
    /// For more information on the events, see <see cref="FileSystemMonitor.Changed"/>, <see cref="FileSystemMonitor.Created"/>, <see cref="FileSystemMonitor.Deleted"/> and <see cref="FileSystemMonitor.Renamed"/>.</remarks>
    [Flags()]
    public enum WatcherChangeTypes
    {
        /// <summary>
        /// The creation, deletion, change, or renaming of a file or folder. 
        /// </summary>
        All = 15,
        /// <summary>
        /// The change of a file or folder. The types of changes include: changes to size, attributes, security settings, last write, and last access time.
        /// </summary>
        Changed = 4,
        /// <summary>
        /// The creation of a file or folder.
        /// </summary>
        Created = 1,
        /// <summary>
        /// The deletion of a file or folder.
        /// </summary>
        Deleted = 2,
        /// <summary>
        /// The renaming of a file or folder.
        /// </summary>
        Renamed = 8
    }
}
