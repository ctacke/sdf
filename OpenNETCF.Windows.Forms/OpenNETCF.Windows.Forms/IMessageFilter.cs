using System;
#if !NDOC
using Microsoft.WindowsCE.Forms;
#endif

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Defines a message filter interface.
	/// </summary>
	/// <remarks>This interface allows an application to capture a message before it is dispatched to a control or form.
	/// <para>A class that implements the IMessageFilter interface can be added to the application's message pump to filter out a message or perform other operations before the message is dispatched to a form or control. To add the message filter to an application's message pump, use the <see cref="M:OpenNETCF.Windows.Forms.ApplicationEx.AddMessageFilter(OpenNETCF.Windows.Forms.IMessageFilter)"/> method in the <see cref="T:OpenNETCF.Windows.Forms.ApplicationEx"/> class.</para></remarks>
	public interface IMessageFilter
	{
		/// <summary>
		/// Filters out a message before it is dispatched.
		/// </summary>
		/// <param name="m">The message to be dispatched. You cannot modify this message.</param>
		/// <returns>true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.</returns>
		bool PreFilterMessage(ref Message m);
	}

#if NDOC
	/// <summary>
	/// Implements a Windows message.
	/// </summary>
	public struct Message
	{
		/// <summary>
		/// Gets or sets the window handle of the message.
		/// </summary>
		public IntPtr HWnd;
		/// <summary>
		/// Gets or sets the ID number for the message.
		/// </summary>
		public int Msg;
		/// <summary>
		/// Gets or sets the WParam field of the message.
		/// </summary>
		public IntPtr WParam;
		/// <summary>
		/// Specifies the LParam field of the message.
		/// </summary>
		public IntPtr LParam;
		/// <summary>
		/// Specifies the value that is returned to Windows in response to handling the message.
		/// </summary>
		public int Result;
	}
#endif

}
