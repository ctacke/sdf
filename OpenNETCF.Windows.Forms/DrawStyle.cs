using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Windows.Forms
{
    /// <summary>
    /// Represents the form in which to render the bar portion of the ProgressBar2 control.
    /// </summary>
    public enum DrawStyle
    {
        /// <summary>
        /// Use a solid color for the progress bar
        /// </summary>
        Solid,
        /// <summary>
        /// Use a gradient color for the progress bar.  <seealso cref="OpenNETCF.Windows.Forms.ProgressBar2.BarGradientColor"/>
        /// </summary>
        Gradient,
    }
}
