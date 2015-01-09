using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Windows.Forms
{
    /// <summary>
    /// Data passed on by the KeyboardHook
    /// </summary>
    public struct KeyData
    {
        /// <summary>
        /// The key code
        /// </summary>
        public int KeyCode { get; set; }
        /// <summary>
        /// The hardware scan code
        /// </summary>
        public int ScanCode { get; set; }
        /// <summary>
        /// The Environement.Tickcount time at which the key was intercepted
        /// </summary>
        public int TimeStamp { get; set; }
    }

    /// <summary>
    /// Handler for KeyboardHook key events
    /// </summary>
    /// <param name="keyMessage">The key message (up, down, sysup or sysdown)</param>
    /// <param name="keyData">The key data associated with the event</param>
    /// <returns>Return <b>true</b> to pass the key data on to the next hook, <b>false</b> to prevent further system processing</returns>
    public delegate void KeyHookEventHandler(Win32.WM keyMessage, KeyData keyData);

    /// <summary>
    /// Used for system-wide hooking of keyboard events
    /// </summary>
    public class KeyboardHook : IDisposable
    {
        private HookProc m_keyboardHookProc;
        private IntPtr m_hHook;

        /// <summary>
        /// Fired when keyboard data is present in the system
        /// </summary>
        public event KeyHookEventHandler KeyDetected;

        /// <summary>
        /// Gets or sets the state of the hook
        /// </summary>
        public bool Enabled
        {
            get
            {
                return (m_hHook != IntPtr.Zero);
            }
            set
            {
                if (value)
                {
                    // only enable if we're not already
                    if (m_hHook == IntPtr.Zero)
                    {
#if DESKTOP
                        m_hHook = NativeMethods.SetWindowsHookEx(OpenNETCF.Windows.Forms.NativeMethods.HookType.Keyboard, m_keyboardHookProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
#else
                        m_hHook = NativeMethods.SetWindowsHookEx(OpenNETCF.Windows.Forms.NativeMethods.HookType.KeyboardLowLevel, m_keyboardHookProc, NativeMethods.GetModuleHandle(null), 0);
#endif
                        if (m_hHook == IntPtr.Zero)
                        {
                            // failure
                            System.Diagnostics.Debugger.Break();
                            throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }
                }
                else
                {
                    // only unhook if we're hooked
                    if (m_hHook != IntPtr.Zero)
                    {
                         bool result = NativeMethods.UnhookWindowsHookEx(m_hHook);
                        if (!result)
                        {
                            // failure
                            System.Diagnostics.Debugger.Break();
                            throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                        }

                        m_hHook = IntPtr.Zero;
                    }
                }
            }
        }

        /// <summary>
        /// Set to <b>false</b> to prevent the system from forwarding the key data to further hooks or the target control.  Defaults to <b>true</b>
        /// </summary>
        public bool PassOnKeys { get; set; }

        /// <summary>
        /// Creates an instance of a KeyboardHook
        /// </summary>
        public KeyboardHook()
        {
            m_keyboardHookProc = new HookProc(KeyboardHookProc);
            PassOnKeys = true;
        }

        /// <summary>
        /// Disposes the KeyboardHook
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free any managed objects
                if (m_hHook != IntPtr.Zero)
                {
                    NativeMethods.UnhookWindowsHookEx(m_hHook);
                    m_hHook = IntPtr.Zero;
                }
            }

            // make sure the hook is unhooked
            Enabled = false;
        }

        /// <summary>
        /// Finalizes the KeyboardHook
        /// </summary>
        ~KeyboardHook()
        {
            Dispose(false);
        }

        internal int KeyboardHookProc(OpenNETCF.Windows.Forms.NativeMethods.HookCode nCode, IntPtr wParam, IntPtr lParam)
        {
            if ((nCode < 0) | (KeyDetected == null))
            {
                return NativeMethods.CallNextHookEx(m_hHook, nCode, wParam, lParam);
            }

            bool callNextHook = true;
            //Marshal the data from the callback.
            NativeMethods.KeyboardHookStruct hs = (NativeMethods.KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(NativeMethods.KeyboardHookStruct));
            KeyData data = new KeyData();
            data.KeyCode = hs.vkCode;
            data.ScanCode = hs.scanCode;
            data.TimeStamp = hs.time;
            KeyDetected((OpenNETCF.Win32.WM)wParam, data);
            if (PassOnKeys)
            {
                return NativeMethods.CallNextHookEx(m_hHook, nCode, wParam, lParam);
            }

            // per the docs: "If the hook procedure processed the message, it may return a nonzero value to prevent the system from passing the message to the rest of the hook chain or the target window procedure."
            return -1;
        }
    }
}