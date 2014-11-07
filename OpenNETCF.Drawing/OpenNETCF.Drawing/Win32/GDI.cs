using System;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// GDI P/Invokes and definitions.
	/// </summary>
	public class GDI
	{
		private GDI(){}

		internal const uint	CLR_INVALID				= 0xFFFFFFFF;
		

		/// <summary>
		/// Region type
		/// </summary>
		public enum RegionFlags : int
		{
            /// <summary>
            /// 
            /// </summary>
			ERROR = 0,
			/// <summary>
			/// 
			/// </summary>
			NULLREGION = 1,
			/// <summary>
			/// 
			/// </summary>
			SIMPLEREGION = 2,
			/// <summary>
			/// 
			/// </summary>
			COMPLEXREGION = 3
		}

		/// <summary>
		/// Background mode
		/// </summary>
		public enum BackgroundMode : int
		{
			/// <summary>
			/// Background is Transparent.
			/// </summary>
			TRANSPARENT = 1,
			/// <summary>
			/// Background is Opaque.
			/// </summary>
			OPAQUE= 2
		}

		#region --------------- GDI API Calls ---------------

        private static void CheckHandle(IntPtr handle)
        {
            if ((handle == IntPtr.Zero) || (handle.ToInt32() == -1))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "File operation failed: invalid handle");
            }
        }

		/// <summary>
		/// Set the forecolor of text in the selected DC
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="crColor"></param>
		/// <returns></returns>
		public static int SetTextColor(IntPtr hdc, int crColor)
		{
			uint prevColor;
			
			CheckHandle(hdc);

			prevColor = SetTextColorCE(hdc, crColor);

			if(prevColor == CLR_INVALID)
			{
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Failed to set color");
			}

			return Convert.ToInt32(prevColor);
		}

		/// <summary>
		/// Get the forecolor of text in the selected DC
		/// </summary>
		/// <param name="hdc"></param>
		/// <returns></returns>
		public static int GetTextColor(IntPtr hdc)
		{
            CheckHandle(hdc);

			return GetTextColorCE(hdc);
		}

		/// <summary>
		/// Set the backcolor in the selected DC
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="crColor"></param>
		/// <returns></returns>
		public static int SetBkColor(IntPtr hdc, int crColor)
		{
			uint prevColor;

            CheckHandle(hdc);

			prevColor = SetBkColorCE(hdc, crColor);

			if(prevColor == CLR_INVALID)
			{
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Failed to set color.");
			}

			return Convert.ToInt32(prevColor);
		}
		
		/// <summary>
		/// Set the backmode in the selected DC
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="iBkMode"></param>
		/// <returns></returns>
		public static BackgroundMode SetBkMode(IntPtr hdc, BackgroundMode iBkMode)
		{
			int prevMode;

            CheckHandle(hdc);

			prevMode = SetBkModeCE(hdc, (int)iBkMode);

			if(prevMode == 0)
			{
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Failed to set BackgroundMode");
			}

			return (BackgroundMode)prevMode;
		}

		/// <summary>
		/// Select a system object (FONT, DC, etc.)
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="hgdiobj"></param>
		/// <returns></returns>
		public static IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj)
		{
			IntPtr ptr = IntPtr.Zero;

            CheckHandle(hdc);
            CheckHandle(hgdiobj);

			ptr = SelectObjectCE(hdc, hgdiobj);

			if(ptr == IntPtr.Zero)
			{
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Failed. Selected object is not a region.");
			}

			return ptr;
		}

		/// <summary>
		/// Release a Device Context
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="hDC"></param>
		public static void ReleaseDC(IntPtr hWnd, IntPtr hDC)
		{
            CheckHandle(hDC);

			if(ReleaseDCCE(hWnd, hDC) == 0)
			{
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Failed to release DC.");
			}
		}

		/// <summary>
		/// Get the DC for the specified window
		/// </summary>
		/// <param name="hWnd">Native window handle of the window.</param>
		/// <returns>Device Context Handle for specified window.</returns>
		public static IntPtr GetWindowDC(IntPtr hWnd)
		{
			IntPtr ptr = IntPtr.Zero;

			ptr = GetWindowDCCE(hWnd);

			if(ptr == IntPtr.Zero)
			{
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Failed to get DC.");
			}

			return ptr;
		}

		/// <summary>
		/// Get the DC for the specified window handle
		/// </summary>
		/// <param name="hWnd"></param>
		/// <returns></returns>
		public static IntPtr GetDC(IntPtr hWnd)
		{
			IntPtr ptr = IntPtr.Zero;

			ptr = GetDCCE(hWnd);

			if(ptr == IntPtr.Zero)
			{
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Failed to get DC.");
			}

			return ptr;
		}

		/// <summary>
		/// Draw a rectangle in a DC
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="nLeftRect"></param>
		/// <param name="nTopRect"></param>
		/// <param name="nRightRect"></param>
		/// <param name="nBottomRect"></param>
		public static void Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect)
		{
            CheckHandle(hdc);

			if(Convert.ToBoolean(RectangleCE(hdc, nLeftRect, nTopRect, nRightRect, nBottomRect)) == false)
			{
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Failed to draw rectangle.");
			}
		}
		#endregion ---------------------------------------------

		#region GDI P/Invokes

				
		[DllImport ("coredll.dll", EntryPoint="SetTextColor", SetLastError=true)]
		private static extern uint SetTextColorCE(IntPtr hdc, int crColor);

		[DllImport ("coredll.dll", EntryPoint="GetTextColor", SetLastError=true)]
		private static extern int GetTextColorCE(IntPtr hdc);

		[DllImport ("coredll.dll", EntryPoint="SetBkColor", SetLastError=true)]
		private static extern uint SetBkColorCE(IntPtr hdc, int crColor);

		[DllImport ("coredll.dll", EntryPoint="SetBkMode", SetLastError=true)]
		private static extern int SetBkModeCE(IntPtr hdc, int iBkMode);

		[DllImport ("coredll.dll", EntryPoint="SelectObject", SetLastError=true)]
		private static extern IntPtr SelectObjectCE(IntPtr hdc, IntPtr hgdiobj);

		[DllImport("coredll.dll", EntryPoint="ReleaseDC", SetLastError=true)]
		private static extern int ReleaseDCCE(IntPtr hWnd, IntPtr hDC);

		[DllImport("coredll.dll", EntryPoint="GetWindowDC", SetLastError=true)]
		private static extern IntPtr GetWindowDCCE(IntPtr hWnd);

		[DllImport("coredll.dll", EntryPoint="GetDC", SetLastError=true)]
		private static extern IntPtr GetDCCE(IntPtr hWnd);

		[DllImport("coredll.dll", EntryPoint="Rectangle", SetLastError=true)]
		private static extern uint RectangleCE(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

		/// <summary>
		/// This function obtains information about a specified graphics object.
		/// </summary>
		/// <param name="hObj">Handle to the graphics object of interest.</param>
		/// <param name="cb">Specifies the number of bytes of information to be written to the buffer.</param>
		/// <param name="objdata">a buffer that is to receive the information about the specified graphics object.</param>
		/// <returns>If the function succeeds, and lpvObject is a valid pointer, the return value is the number of bytes stored into the buffer.</returns>
		[DllImport("coredll.dll", EntryPoint="GetObject", SetLastError=true)]
		public extern static int GetObject(IntPtr hObj, int cb, byte[] objdata);
		/// <summary>
		/// This function obtains information about a specified graphics object.
		/// </summary>
		/// <param name="hObj">Handle to the graphics object of interest.</param>
		/// <param name="cb">Specifies the number of bytes of information to be written to the buffer.</param>
		/// <param name="objdata">a buffer that is to receive the information about the specified graphics object.</param>
		/// <returns>If the function succeeds, and lpvObject is a valid pointer, the return value is the number of bytes stored into the buffer.</returns>
		[DllImport("coredll.dll", EntryPoint="GetObject", SetLastError=true)]
		public extern static int GetObject(IntPtr hObj, int cb, DibSection objdata);

		/// <summary>
		/// This function creates a logical brush that has the specified solid color.
		/// </summary>
		/// <param name="crColor">Specifies the color of the brush.</param>
		/// <returns>A handle that identifies a logical brush indicates success.</returns>
		[DllImport("coredll.dll", EntryPoint="CreateSolidBrush", SetLastError=true)]
		public static extern IntPtr CreateSolidBrush( int crColor );

		#endregion
	}
}
