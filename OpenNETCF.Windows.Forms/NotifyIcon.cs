using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using OpenNETCF.Win32;
using Microsoft.WindowsCE.Forms;

namespace OpenNETCF.Windows.Forms
{
	/// <summary>
	/// Specifies a component that creates an icon in the status area
	/// </summary>
	/// <remarks>Icons in the status area are short cuts to processes that are running in the background of a computer, such as a virus protection program or a volume control.
	/// These processes do not come with their own user interfaces.
	/// The <see cref="NotifyIcon"/> class provides a way to program in this functionality.
	/// The Icon property defines the icon that appears in the status area.
	/// Pop-up menus for an icon are addressed with the ContextMenu property.
	/// The <see cref="Text"/> property assigns ToolTip text (Tooltips are not supported by the Pocket PC interface).
	/// In order for the icon to show up in the status area, the <see cref="Visible"/> property must be set to true.</remarks>
	public sealed class NotifyIcon : System.ComponentModel.Component
	{
		//count unique id - used to identify control
		private static uint id = 9;

		//byte array used for marshalling
		private NOTIFYICONDATA data;
		
#if !NDOC
		//messagewindow
		private NotifyIconMessageWindow messageWindow;
#endif

		//doubleclick
		private bool doubleclick  = false;


		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="NotifyIcon"/> class.
		/// </summary>
		public NotifyIcon()
		{
#if !NDOC
			//create new messagewindow
			messageWindow = new NotifyIconMessageWindow(this);
#endif
			//create new array
            data = new NOTIFYICONDATA();
            data.cbSize = Marshal.SizeOf(data);
			
			//write standard contents to data
            data.uID = id++;
            //write notification message
            data.uCallbackMessage = (int)WM.NOTIFY;
#if !NDOC
            data.hWnd = messageWindow.Hwnd;
#endif
            data.uFlags = NIF.MESSAGE;
		}
		#endregion

		#region Dispose
		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="NotifyIcon"/> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources;
		/// false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			//remove the icon
			this.Visible = false;
            if (icon != null)
            {
                icon.Dispose();
                icon = null;
            }
			base.Dispose(disposing);
		}

		#endregion

		#region Properties

		#region Context Menu
        /*
		private ContextMenu menu;
		/// <summary>
		/// Gets or sets the shortcut menu for the icon.
		/// </summary>
		public ContextMenu ContextMenu
		{
			get
			{
				return menu;
			}
			set
			{
				menu = value;
			}
		}*/
		#endregion

		#region Icon
        private Icon icon;

		/// <summary>
		/// The <see cref="Icon"/> displayed by the NotifyIcon component.
		/// </summary>
		public Icon Icon
		{
			get
			{
				return icon;
			}
			set
			{
				icon = value;
                data.hIcon = icon.Handle;
                data.uFlags |= NIF.ICON;
                
				
                if(visible)
				{
					Shell_NotifyIcon(NIM.MODIFY, ref data);
				}
			}
		}
		#endregion

		#region Text
		/// <summary>
		/// Gets or sets the ToolTip text displayed when the mouse hovers over a status area icon.
		/// </summary>
		/// <remarks>The Pocket PC interface does not display tooptips.</remarks>
		/// <value>The ToolTip text displayed when the mouse hovers over a status area icon.</value>
		/// <exception cref="ArgumentException">ToolTip text must be less than 64 characters long.</exception>
		public string Text
		{
			get
			{
				//return string extracted from array
                return data.szTip;
			}
			set
			{
                if (value.Length > 64)
                {
                    throw new ArgumentException("value","Text must be 64 characters or less");
                }
                data.szTip = value;
                data.uFlags |= NIF.TIP;
                if (visible)
                {
                    Shell_NotifyIcon(NIM.MODIFY, ref data);
                }
			}
		}
		#endregion

		#region Visible
        private bool visible;

		/// <summary>
		/// Gets or sets a value indicating whether the icon is visible in the status notification area of the taskbar.
		/// </summary>
		/// <value>true if the icon is visible in the status area; otherwise, false. The default value is false.</value>
		/// <remarks>Since the default value is false, in order for the icon to show up in the status area, you must set the Visible property to true.</remarks>
		public bool Visible
		{
			get
			{
				return visible;
			}
			set
			{
				if(visible != value)
				{
					if(value)
					{
						if(Shell_NotifyIcon(NIM.ADD, ref data))
						{
							//set state to visible
							visible=true;
						}
						else
						{
							//add failed
							throw new ExternalException("Error adding NotifyIcon");
						}
					}
					else
					{
						if(Shell_NotifyIcon(NIM.DELETE, ref data))
						{
							//set state to invisible
							visible=false;
						}
						else
						{
							//delete failed
							throw new ExternalException("Error deleting NotifyIcon");
						}
					}

				}
			}
		}
		#endregion

		#endregion

		#region Events

		#region Click
		/// <summary>
		/// Occurs when the user clicks the icon in the status area.
		/// </summary>
		public event EventHandler Click;

		private void OnClick(EventArgs e)
		{
			/*if(Control.MouseButtons == MouseButtons.Right)
			{
                if (menu != null)
                {
                    menu.Show(null, Form.MousePosition);
                }

			}*/
			if(this.Click != null)
			{
				//fire Click event
				this.Click(this, e);
			}
		}
		#endregion

		#region Double Click
		/// <summary>
		/// Occurs when the user double-clicks the icon in the status notification area of the taskbar.
		/// </summary>
		public event EventHandler DoubleClick;

		private void OnDoubleClick(EventArgs e)
		{
			if(this.DoubleClick != null)
			{
				//fire DoubleClick event
				this.DoubleClick(this, e);
			}
		}
		#endregion

		#region Mouse Up
		/// <summary>
		/// Occurs when the user releases the mouse button while the pointer is over the icon in the status notification area of the taskbar.
		/// </summary>
		public event MouseEventHandler MouseUp;

		private void OnMouseUp(MouseEventArgs e)
		{
			if(this.MouseUp != null)
			{
				//fire Mouse Up event
				this.MouseUp(this, e);
			}
			//only fire click if this is not the second click of a doubleclick sequence
			if(!this.doubleclick)
			{
				this.OnClick(new EventArgs());
			}

			//reset doubleclick flag
			this.doubleclick = false;
		}
		#endregion

		#region Mouse Down
		/// <summary>
		/// Occurs when the user presses the mouse button while the pointer is over the icon in the status notification area of the taskbar.
		/// </summary>
		public event MouseEventHandler MouseDown;

		private void OnMouseDown(MouseEventArgs e)
		{
			if(this.MouseDown != null)
			{
				//fire Mouse Down event
				this.MouseDown(this, e);
			}
		}
		#endregion

		#endregion

		#region NotifyIcon MessageWindow
#if !NDOC	
		private sealed class NotifyIconMessageWindow : MessageWindow
		{	
			//reference to parent control
			private NotifyIcon parent;

			// <summary>
			// Creates a new instance of the NotifyIconMessageWindow class.
			// </summary>
			// <param name="parent">NotifyIcon for which this MessageWindow is operating.</param>
			internal NotifyIconMessageWindow(NotifyIcon parent)
			{
				this.parent = parent;
			}

			// <summary>
			// Handles incoming windows Messages and acts accordingly
			// </summary>
			// <param name="m">Windows Message</param>
			protected override void WndProc(ref Microsoft.WindowsCE.Forms.Message m)
			{
				//notification message received
				if(m.Msg==(int)WM.NOTIFY)
				{
					//switch on action
					switch((int)m.LParam)
					{
						case 0x0201:
							//left button mouse down
							parent.OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
							break;
						case 0x0202:
							//left button mouse up
							parent.OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
							//only fire click if this is not the second click of a doubleclick sequence
							break;
						case 0x0203:
							//left button doubleclick
							parent.OnMouseDown(new MouseEventArgs(MouseButtons.Left, 2, 0, 0, 0));
							parent.doubleclick = true;
							parent.OnDoubleClick(new EventArgs());
							break;
						case 0x204:
							//r button down
							parent.OnMouseDown(new MouseEventArgs(MouseButtons.Right, 1, 0, 0, 0));
							break;
						case 0x0205:
							//r button up
							parent.OnMouseUp(new MouseEventArgs(MouseButtons.Right, 1, 0, 0, 0));
							break;
					}
				}

				//let windows handle any other messages
				base.WndProc (ref m);
			}
		}
		
#endif
#endregion

        #region Native

        private enum NIM : uint
		{
			ADD    = 0x00000000,
			MODIFY = 0x00000001,
			DELETE = 0x00000002,
		}

		[Flags]
		private enum NIF : uint
		{
			MESSAGE = 0x00000001,
			ICON    = 0x00000002,
			TIP = 0x00000004,
		}

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        private struct NOTIFYICONDATA
        { 
            public int cbSize;
            public IntPtr hWnd;
            public uint uID;
            public NIF uFlags;
            public int uCallbackMessage;
            public IntPtr hIcon; 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=64)]
            public string szTip; 
        } 

		[DllImport("coredll.dll", EntryPoint="Shell_NotifyIcon", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
		private extern static bool Shell_NotifyIcon(NIM dwMessage, ref NOTIFYICONDATA lpData);
		
        #endregion

	}
}
