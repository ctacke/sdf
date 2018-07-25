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
    /// Represents the method that will handle the <see cref="FileSystemMonitor.Renamed"/> event of a <see cref="FileSystemMonitor"/> class.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RenamedEventArgs"/> that contains the event data.</param>
    /// <seealso cref="RenamedEventArgs"/>
    /// <seealso cref="FileSystemEventHandler"/>
    /// <seealso cref="FileSystemEventArgs"/>
    public delegate void RenamedEventHandler(object sender, RenamedEventArgs e);


    /// <summary>
    /// Provides data for the Renamed event.
    /// </summary>
    public class RenamedEventArgs : FileSystemEventArgs
    {
        private string oldFullPath;
        private string oldName;


        public RenamedEventArgs(
            WatcherChangeTypes changeType,
            string directory,
            string name,
            string oldName
            )
            : base(changeType, directory, name)
        {
            this.oldFullPath = directory;
            this.oldName = oldName;
        }

        /// <summary>
        /// Gets the previous fully qualified path of the affected file or directory.
        /// </summary>
        public string OldFullPath
        {
            get
            {
                return oldFullPath;
            }
        }

        /// <summary>
        /// Gets the old name of the affected file or directory.
        /// </summary>
        public string OldName
        {
            get
            {
                return oldName;
            }
        }

    }
}
