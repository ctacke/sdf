using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;
#if !NDOC
using Microsoft.WindowsCE.Forms;
using OpenNETCF.Win32;
#endif
using OpenNETCF.Threading;
using System.Threading;

using EventWaitHandle = OpenNETCF.Threading.EventWaitHandle;
using EventResetMode = OpenNETCF.Threading.EventResetMode;
using System.Diagnostics;

namespace OpenNETCF.Windows.Forms
{
	internal struct MSG
	{
		public IntPtr hwnd;
		public int message;
		public IntPtr wParam;
		public IntPtr lParam;
		public int time;
		public int pt_x;
		public int pt_y;
	}

	/// <summary>
	/// Provides static (Shared in Visual Basic) methods and properties to manage an application, such as methods to start and stop an application, to process Windows messages, and properties to get information about an application. This class cannot be inherited.
	/// </summary>
	public static class Application2
	{
		/// <summary>
		/// Occurs when Application2.Run exits
		/// <seealso cref="Exit"/>
		/// </summary>
		public static event EventHandler ThreadExit;
		/// <summary>
		/// Occurs when the application is about to shut down.
		/// </summary>
		public static event EventHandler ApplicationExit;
		private static ArrayList messageFilters = new ArrayList();
		private static bool messageLoop = false;
		private static Form mainForm = null;
		private static MSG msg = new MSG();
		private static bool process;
		private static bool exitFlag;
		private static ThreadWindows threadWindows = null;

        private static CurrentFormMessageFilter currentFormFilter;
        private static Thread remoteActivateThread;
        private static Control m_eventMarshalingControl = null;
     
		private static void LocalModalMessageLoop()
		{
			exitFlag = false;
			while(NativeMethods.GetMessage(out msg, IntPtr.Zero, 0, 0))
			{
				if (exitFlag)
				{
					exitFlag = false;
					break;
				}

				process = true;

				// iterate any filters
				foreach(IMessageFilter mf in messageFilters)
				{
#if !NDOC && !DESIGN
					Message m = Message.Create(msg.hwnd, msg.message, msg.wParam, msg.lParam);					

					// if *any* filter says not to process, we won't
					process = process ? !(mf.PreFilterMessage(ref m)) : false;
#endif
				}

				// if we're supposed to process the message, do so
				if(process)
				{
                    NativeMethods.TranslateMessage(out msg);
                    NativeMethods.DispatchMessage(ref msg);
				}
			}			
		}

		static void ModalForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{			
			ModalForm_Closed(sender, e);

			// we don't want to let Close actually run, since that will
			// dispose the form.  The only time this event handler is 
			// used is when the form should not be disposed.
			e.Cancel = true;
		}

		static void ModalForm_Closed(object sender, EventArgs e)
		{
			exitFlag = true;
			if (threadWindows != null) threadWindows.Enable(true);
		}

		public static DialogResult ShowDialog(Form form)
		{
			return ShowDialog(form, true);
		}

		public static DialogResult ShowDialog(Form form, bool disposeForm)
		{
			DialogResult result = DialogResult.OK;

#if !NDOC && !DESIGN
			if (form == null)
			{
				throw new ArgumentNullException("form");
			}

			IntPtr lastWnd = Win32Window.GetActiveWindow();

			// it seems that Form.Close() method processes Window Queue itself that is why it is 
			// only way to be noticed the a dialog form was closed
			// If we're not supposed to dispose the form, we need to listen
			// for the closing event instead, as the form will already be disposed
			// by the time Closed is raised.
			if (disposeForm)
			{
				form.Closed += new EventHandler(ModalForm_Closed);
			}
			else
			{
				form.Closing += new System.ComponentModel.CancelEventHandler(ModalForm_Closing);					
			}

			form.Show();
			form.Capture = true;
			IntPtr hwnd = Win32Window.GetCapture();
			form.Capture = false;

			ThreadWindows previousThreadWindows = threadWindows;
			threadWindows = new ThreadWindows(hwnd);
			threadWindows.previousThreadWindows = previousThreadWindows;
			threadWindows.Enable(false);

			// enters dialog window loop
			LocalModalMessageLoop();
			
			result = form.DialogResult;
           
			if(threadWindows != null)
			{
				threadWindows = threadWindows.previousThreadWindows;
			}

			if(disposeForm)
			{
				form.Closed -= new EventHandler(ModalForm_Closed);	
				form.Dispose();
			}
			else
			{
				form.Closing -= new System.ComponentModel.CancelEventHandler(ModalForm_Closing);
			}

            if (NativeMethods.IsWindow(lastWnd) && NativeMethods.IsWindowVisible(lastWnd))
			{
                NativeMethods.SetActiveWindow(lastWnd);
			}
#endif
			return result;
		}


		private static bool Pump()
		{
            ArrayList MyMessageFilters = new ArrayList();
				// there are, so get the top one
            if (NativeMethods.GetMessage(out msg, IntPtr.Zero, 0, 0))
			{
				process = true;
                MyMessageFilters = (ArrayList)(messageFilters.Clone());
				// iterate any filters
                lock (messageFilters.SyncRoot)
                {
                    foreach (IMessageFilter mf in MyMessageFilters)
				{
#if !NDOC && !DESIGN
					Message m = Message.Create(msg.hwnd, msg.message, msg.wParam, msg.lParam);					

					// if *any* filter says not to process, we won't
					process = process ? !(mf.PreFilterMessage(ref m)) : false;
#endif
				}

				// if we're supposed to process the message, do so
				if(process)
				{
                    NativeMethods.TranslateMessage(out msg);
                    NativeMethods.DispatchMessage(ref msg);
				}
			}
            }
			else
			{
				return false;
			}

			return true;
		}

        private static bool RunMessageLoop(bool showForm)
        {
            m_eventMarshalingControl = new Control();

			if(mainForm != null)
			{
				// if we have a form, show it
				mainForm.Visible = showForm;
			}

			// start up the message pump
			messageLoop = true;
			while(Pump()) {};
			messageLoop = false;

			// exit cleanly
			ExitThread();

			return true;
		}

		/// <summary>
		/// Gets a value indicating whether a message loop exists on this thread. 
		/// </summary>
		public static bool MessageLoop
		{
			get
			{
				return messageLoop;
			}
		}

		private static void ExitThread()
		{
			// fire the ThreadExit event if it's wired
			if(ThreadExit != null)
			{
				ThreadExit(mainForm, null);
			}

			// dispose the main form if it exists
			if(mainForm != null)
			{
				mainForm.Dispose();
			}

			if(ApplicationExit != null)
			{
				ApplicationExit(null, null);
			}

			// let the GC know it can collect
			GC.GetTotalMemory(true);
		}

        private static void RemoteActivateThreadProc()
        {
            EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset, Application2.StartupPath);

            while(true)
            {
                wh.WaitOne();

                currentFormFilter.ActivateCurrentForm();
            }
        }

        /// <summary>
        /// Begins running a standard application message loop on the current thread, without a form.  
        /// </summary>
        /// <param name="runAsSingletonApp">When <b>true</b>, if an existing instance of the app is already running, the current application instance will simply exit</param>
        public static void Run(bool runAsSingletonApp)
        {
            bool isNew = true;

            NamedMutex m = new NamedMutex(true, Application2.StartupPath, out isNew);

            if (isNew)
            {
                Run();
            }
        }

        /// <summary>
        /// Begins running a standard application message loop on the current thread, and makes the specified form visible.
        /// </summary>
        /// <param name="mainForm">Form on which main message loop runs</param>
        /// <param name="runAsSingletonApp">When <b>true</b>, if an existing instance of the app is already running, the current application instance will simply exit and the already running app will come to the fore</param>
        public static void Run(Form mainForm, bool runAsSingletonApp)
        {
            Run(mainForm, runAsSingletonApp, true);
        }

        /// <summary>
        /// Begins running a standard application message loop on the current thread, and makes the specified form visible.
        /// </summary>
        /// <param name="mainForm">Form on which main message loop runs</param>
        /// <param name="runAsSingletonApp">When <b>true</b>, if an existing instance of the app is already running, the current application instance will simply exit and the already running app will come to the fore</param>
        /// <param name="displayMainForm">When set to true, the main form will be automatically displayed, else the app will be responsible for showing the Form</param>
        public static void Run(Form mainForm, bool runAsSingletonApp, bool displayMainForm)
        {
            bool isNew = true;

            NamedMutex m = new NamedMutex(true, Application2.StartupPath, out isNew);

            if (runAsSingletonApp)
            {
                if (!isNew)
                {
                    // activate the existing instance
                    EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset, Application2.StartupPath);
                    wh.Set();
                    return;
                }
                else
                {
                    //create a thread to wait for any subsequent instances to signal
                    remoteActivateThread = new Thread(new System.Threading.ThreadStart(RemoteActivateThreadProc));
                    remoteActivateThread.IsBackground = true;
                    remoteActivateThread.Name = "SDF Application2 UI thread";
                    remoteActivateThread.Start();

                    // create a filter to track the currently active form so if 
                    // we get reactivated from another instance attempting to run
                    // we know what Form to put topmost
                    currentFormFilter = new CurrentFormMessageFilter();
                    AddMessageFilter(currentFormFilter);

                }
            } // if (runAsSingletonApp)

            mainForm.Closed += new EventHandler(MainFormExit);
            Application2.mainForm = mainForm;
            RunMessageLoop(displayMainForm);
        }

		/// <summary>
		/// Begins running a standard application message loop on the current thread, without a form
		/// </summary>
		public static void Run()
		{
			RunMessageLoop(false);
		}

		/// <summary>
		/// Begins running a standard application message loop on the current thread, and makes the specified form visible.
		/// <seealso cref="System.Windows.Forms.Form"/>
		/// </summary>
		/// <param name="mainForm">Form on which main message loop runs</param>
		public static void Run(Form mainForm)
		{
            Run(mainForm, false, true);
		}

		/// <summary>
		/// Informs all message pumps that they must terminate, and then closes all application windows after the messages have been processed.
		/// </summary>
        public static void Exit()
        {
            if (m_eventMarshalingControl.InvokeRequired)
            {
                m_eventMarshalingControl.Invoke(new EventHandler(
                    delegate
                    {
                        NativeMethods.PostQuitMessage(0);
                    }));
                return;
            }

            NativeMethods.PostQuitMessage(0);
        }

		/// <summary>
		/// Processes all Windows messages currently in the message queue.
		/// </summary>
		public static void DoEvents()
		{
            while (NativeMethods.PeekMessage(out msg, IntPtr.Zero, 0, 0, 0))
			{
				Pump();
			}
		}

		/// <summary>
		/// Adds a message filter to monitor Windows messages as they are routed to their destinations
		/// <seealso cref="IMessageFilter"/>
		/// </summary>
		/// <param name="value">The implementation of the IMessageFilter interface you want to install</param>
		public static void AddMessageFilter(IMessageFilter value)
		{
			messageFilters.Add(value);
		}

		/// <summary>
		/// Removes a message filter from the message pump of the application
		/// <seealso cref="IMessageFilter"/>
		/// </summary>
		/// <param name="value">The implementation of the IMessageFilter to remove from the application.</param>
		public static void RemoveMessageFilter(IMessageFilter value)
		{
			messageFilters.Remove(value);
		}

		/// <summary>
		/// Gets the path for the currently executing assembly file, not including the executable name.
		/// </summary>
        /// <value>The path for the executable file that started the application.</value>
		public static string StartupPath
		{
			get
			{
                //fix possible issue if called from non .exe code
                return System.IO.Path.GetDirectoryName(OpenNETCF.Reflection.Assembly2.GetEntryAssembly().GetName().CodeBase);
				//return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase);
			}
		}

        /// <summary>
        /// Gets the path for the executable file that started the application, including the executable name.
        /// </summary>
        /// <value>The path and executable name for the executable file that started the application.</value>
        public static string ExecutablePath
        {
            get
            {
                return OpenNETCF.Reflection.Assembly2.GetEntryAssembly().GetName().CodeBase;    
            }
        }

		private static void MainFormExit(object sender, EventArgs e)
		{
            NativeMethods.PostQuitMessage(0);
		}
	}
}
