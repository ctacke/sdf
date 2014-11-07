using System;
using System.Windows.Forms;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Provides an interface to expose Win32 HWND handles.
	/// </summary>
	/// <remarks>This interface is implemented on objects that expose Win32 HWND handles.
	/// The resultant handle can be used with Win32 API calls.
	/// <para>Unlike the desktop .NET Framework this interface is not implemented in the base <see cref="Control"/> class.
	/// However you can implement it on any class which derives from <see cref="Control"/> or <see cref="Form"/>.
	/// The <see cref="Handle"/> property can then be passed to native API functions such as those contained in the <see cref="OpenNETCF.Win32.Win32Window"/> class.</para></remarks>
	/// <example>
	/// <code>
	/// [VB]
	/// Imports OpenNETCF.Windows.Forms
	/// Imports OpenNETCF.Win32
	/// 
	/// Public Class MyControl
	///		Inherits System.Windows.Forms.Control
	///		Implements IWin32Window
	///		
	///		Overridable ReadOnly Property Handle() As System.IntPtr
	///			Get
	///				Me.Capture = True
	///				Dim thishandle As IntPtr
	///				thishandle = Win32Window.GetCapture()
	///				Me.Capture = False
	///
	///				Handle = thishandle
	///			End Get
	///		End Property
	///		
	/// End Class
	/// </code>
	/// <code>
	/// [C#]
	/// using OpenNETCF.Windows.Forms;
	/// using OpenNETCF.Win32;
	/// 
	/// public class MyControl : Control, IWin32Window
	/// {
	///		
	///		public IntPtr Handle
	///		{
	///			get
	///			{
	///				this.Capture = true;
	///				IntPtr thishandle = Win32Window.GetCapture();
	///				this.Capture = false;
	///				
	///				return thishandle;
	///			}
	///		}
	/// }</code>
	/// </example>
	public interface IWin32Window
	{
		/// <summary>
		/// Gets the handle to the window represented by the implementer.
		/// </summary>
		IntPtr Handle
		{
			get;
		}
	}
}
