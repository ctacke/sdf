using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Windows.Forms
{
  /// <summary>
  /// Specifies the state of an item that is being drawn.
  /// </summary>
  public enum DrawItemState
  {
    /// <summary>
    /// The item currently has no state.
    /// </summary>
    None = 0,
    /// <summary>
    /// The item is selected.
    /// </summary>
    Selected = 1,
    /// <summary>
    /// The item is disabled.
    /// </summary>
    Disabled = 4,
    /// <summary>
    /// The item has focus.
    /// </summary>
    Focus = 16
  }
}
