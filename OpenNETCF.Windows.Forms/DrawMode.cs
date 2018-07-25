using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Windows.Forms
{
    /// <summary>
    /// Specifies how the elements of a control are drawn.
    /// </summary>
    public enum DrawMode
    {
        /// <summary>
        /// All the elements in a control are drawn by the operating system and are of the same size.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// All the elements in the control are drawn manually and are of the same size.
        /// </summary>
        OwnerDrawFixed = 1,
        /// <summary>
        /// All the elements in the control are drawn manually and can differ in size.
        /// </summary>
        //OwnerDrawVariable = 2,
    }
}
