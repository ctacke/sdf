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
