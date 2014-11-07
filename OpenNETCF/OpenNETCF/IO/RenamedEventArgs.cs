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
