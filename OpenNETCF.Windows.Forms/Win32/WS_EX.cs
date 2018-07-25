using System;

namespace OpenNETCF.Win32
{
    /*
     * CE 5.0 documented styles
     * 
WS_EX_ACCEPTFILES Specifies that a window created with this style accepts drag-drop files. 
WS_EX_CONTEXTMENU Adds a Help button in the caption bar. 
WS_EX_LAYOUTRTL Creates a window whose horizontal origin is on the right edge. Increasing horizontal values advance to the left.  
WS_EX_LTRREADING The window text is displayed using left-to-right reading-order properties. This is the default. 
WS_EX_NOINHERITLAYOUT A window created with this style does not pass its window layout to its child windows. 
WS_EX_PALETTEWINDOW Combines the WS_EX_WINDOWEDGE, WS_EX_TOOLWINDOW, and WS_EX_TOPMOST styles. 
WS_EX_RTLREADING If the shell language is Hebrew, Arabic, or another language that supports reading-order alignment, the window text is displayed using right-to-left reading-order properties. For other languages, the style is ignored. 
    */
    /// <summary>
	/// Extended control styles.
	/// </summary>
	[Flags()]
	public enum WS_EX
	{
		/// <summary>
		/// Specifies that a window has a border with a sunken edge. 
		/// </summary>
		CLIENTEDGE = 0x00000200, 
			
		/// <summary>
		/// Creates a window that has a double border; the window can, optionally, be created with a title bar by specifying the WS_CAPTION style in the dwStyle parameter. 
		/// </summary>
		DLGMODALFRAME = 0x00000001,
	
		/// <summary>
		/// A top-level window created with this style cannot be activated. If a child window has this style, tapping it does not cause its top-level parent to be activated. A window that has this style receives stylus events, but neither it nor its child windows can get the focus. Supported in Windows CE versions 2.0 and later. 
		/// </summary>
		NOACTIVATE = 0x08000000,
		
		/// <summary>
		/// Combines the CLIENTEDGE and WINDOWEDGE styles. 
		/// </summary>
		OVERLAPPEDWINDOW = WINDOWEDGE | CLIENTEDGE,
			
		/// <summary>
		/// Creates a window with a three-dimensional border style intended to be used for items that do not accept user input. 
		/// </summary>
		STATICEDGE = 0x00020000,
	
		/// <summary>
		/// Creates a tool window; that is, a window intended to be used as a floating toolbar. A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font. A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB. If a tool window has a system menu, its icon is not displayed on the title bar. However, you can display the system menu by right-clicking or by typing ALT+SPACE.  
		/// </summary>
		TOOLWINDOW = 0x00000080,
	
		/// <summary>
		/// Specifies that a window created with this style should be placed above all non-topmost windows and should stay above them, even when the window is deactivated. To add or remove this style, use the SetWindowPos function. 
		/// </summary>
		TOPMOST = 0x00000008,
		
		/// <summary>
		/// Specifies that a window has a border with a raised edge. 
		/// </summary>
		WINDOWEDGE = 0x00000100,

        /// <summary>
        /// Includes a question mark in the title bar of the window. When the user clicks the question mark, the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message. The child window should pass the message to the parent window procedure, which should call the WinHelp function using the HELP_WM_HELP command. The Help application displays a pop-up window that typically contains help for the child window.
        /// WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
        /// </summary>
		CONTEXTHELP = 0x00000400,

        /// <summary>
        /// Window includes an OK button in the title bar
        /// </summary>
		CAPTIONOKBUTTON = unchecked((int)0x80000000),
        
        /// <summary>
        /// A window created with this style cannot be dragged using a mouse or stylus
        /// </summary>
		NODRAG = 0x40000000,
		
        /// <summary>
        /// Places window above Start bar.  When combined with WS_EX.TOPMOST, generates a full-screen, top-most Window
        /// </summary>
        ABOVESTARTUP = 0x20000000,

        /// <summary>
        /// Indicates that no default beeping sound is generated on clicking on the window.
        /// </summary>
		INK = 0x10000000,

        /// <summary>
        /// A window created with this style does not show animated exploding and imploding rectangles, and does not have a button on the taskbar. Supported in Windows CE 2.0 and later.
        /// </summary>
		NOANIMATION = 0x04000000,

		/// <summary>
		/// Additional value for Completeness
		/// </summary>
		NONE = 0x00000000
	}

}