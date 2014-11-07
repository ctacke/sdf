using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Enumerates all windows associated with a thread. In the future (for CF2.0), it should be based on the EnumThreadWindows function.
	/// </summary>
	internal class ThreadWindows
	{
		IntPtr _parent;
		internal ThreadWindows previousThreadWindows;


		#region P/Invokes
		private const uint GW_HWNDFIRST = 0; 
		private const uint GW_HWNDNEXT = 2; 

		[DllImport("coredll.dll")]
		static extern IntPtr EnableWindow(IntPtr hWnd, bool enable);

		[DllImport("coredll.dll")]
		static extern bool IsWindowEnabled(IntPtr hWnd);

		[DllImport("coredll.dll")]
		static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("coredll.dll")]
		static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

		[DllImport("coredll.dll")]
		static extern IntPtr GetParent(IntPtr hWnd);

		[DllImport("coredll.dll")]
		static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);
		#endregion

		/// <summary>
		/// Creates a new <see cref="ThreadWindows"/> object for a specific window handle.
		/// </summary>
		/// <param name="parent"></param>
		public ThreadWindows(IntPtr parent)
		{
			_windows = new IntPtr[16];
			_parent = parent;
			EnumThreadWindows();
		}

		/// <summary>
		/// Enables/Disables thread windows except parent window.
		/// </summary>
		/// <param name="state"></param>
		public void Enable(bool state)
		{
			foreach(IntPtr window in _windows)
			{
				EnableWindow(window, state);
			}
		}

		private IntPtr GetTopWindow(IntPtr hwnd)
		{
			IntPtr newHwnd;
			while(true)
			{
				newHwnd = GetParent(hwnd);
				if (newHwnd == IntPtr.Zero) return hwnd;
				hwnd = newHwnd;
			}
		}

		private void EnumThreadWindows()
		{
			ArrayList al = new ArrayList();
			IntPtr hwnd = GetTopWindow(_parent);
			int threadId = GetWindowThreadProcessId(_parent, IntPtr.Zero);
			hwnd = GetWindow(hwnd, GW_HWNDFIRST);
			while(true) 
			{
				// ignores parent window
				if (hwnd != _parent)
				{
					if (threadId == GetWindowThreadProcessId(hwnd, IntPtr.Zero) && IsWindowEnabled(hwnd) && IsWindowVisible(hwnd))
					{
						if (_windows.Length == _windowCount)
						{
							IntPtr[] ar = new IntPtr[this._windowCount * 2];
							Array.Copy(_windows, 0, ar, 0, _windowCount);
							_windows = ar;
						}
						_windows[_windowCount++] = hwnd;
					}
				}

				hwnd = GetWindow(hwnd, GW_HWNDNEXT);
				if (hwnd == IntPtr.Zero) break;
			}

			hwnd = IntPtr.Zero;
		}

		int _windowCount = 0;
		IntPtr [] _windows;
	}
}
