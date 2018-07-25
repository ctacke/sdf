using System;
using System.Windows.Forms;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Allows a control to act like a button on a form.
	/// </summary>
	public interface IButtonControl
	{
		/// <summary>
		/// Gets or sets the value returned to the parent form when the button is clicked.
		/// </summary>
		/// <value>One of the <see cref="T:System.Windows.Forms.DialogResult"/> values.</value>
		DialogResult DialogResult {get; set;}

		/// <summary>
		/// Notifies a control that it is the default button so that its appearance and behavior are adjusted accordingly.
		/// </summary>
		/// <param name="value"><b>true</b> if the control should behave as a default button; otherwise, <b>false</b>.</param>
		void NotifyDefault(bool value);

		/// <summary>
		/// Generates a Click event for the control.
		/// </summary>
		void PerformClick();
	}
}