using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.WindowsCE.Forms;
using System.Runtime.InteropServices;
using OpenNETCF.Windows.Forms;
using OpenNETCF.Win32;


namespace OpenNETCF.WindowsMobile.Forms
{
	/// <summary>
	/// Displays rich ink and voice content.
	/// </summary>
	/// <remarks>This class was previously named <b>OpenNETCF.Windows.Forms.InkX</b></remarks>
	public class InkX : Control2
	{

		//current settings
		private bool m_voicebar;
		private IntPtr mHInk = IntPtr.Zero;

		#region Constructor

        static InkX()
        {
            
        }
		/// <summary>
		/// Creates a new instance of RichInk.
		/// </summary>
		public InkX()
		{
		}
		#endregion

        protected override Control.ControlCollection CreateControlsInstance()
        {
            //In the constructor we don't know if we are in design mode yet.  We need to do this somewhere else
            if (!OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
            {
                //init inkx control
                //TODO Moved this here instead of leaving it in the static method.  Need to test to see if this has any other reprucusions with InitInkX pinvoke
                InitInkX();
            }

            return base.CreateControlsInstance();
        }
        /// <summary>
        /// Parameters for Control2 and wndproc
        /// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ClassName = "InkX";
				return cp;
			}
		}

		private IntPtr hInk
		{
			get
			{
                if (OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
                    return IntPtr.Zero;

				if(mHInk==IntPtr.Zero)
				{
					mHInk = Win32Window.GetWindow(childHandle, GW.CHILD);
				}
				return mHInk;
			}
		}

		#region Enable Voice Bar Property
		/// <summary>
		/// Gets or sets a value which determines whether the VoiceBar is displayed.
		/// </summary>
		/// <remarks>Setting this property to true will display the VoiceBar.</remarks>
		public bool EnableVoiceBar
		{
			get
			{
				return m_voicebar;
			}
			set
			{
				m_voicebar = value;
                if(OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
                    return;
			
                //send to control
				int intval = 0;
				if(value)
					intval = -1;

				Win32Window.SendMessage(childHandle, (int)InkXMessage.VoiceBar, intval, 0);
			}
		}
		#endregion
		
		#region Data Length Property
		/// <summary>
		/// Gets the length in bytes of the ink data in the main compose window.
		/// </summary>
		public int DataLength
		{
			get
			{
                if(OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
                    return 0;
				return (int)Win32Window.SendMessage(childHandle,(int)InkXMessage.GetDataLen, 0, 0);
			}
		}
		#endregion

		#region Data Property
		/// <summary>
		/// Gets or Sets the raw data contained in the compose window.
		/// </summary>
		public byte[] Data
		{
			get
			{
                if (OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
                {
                    return new byte[0];
                }
                else
                {
                    //get data length
                    int len = this.DataLength;
                    byte[] data = new byte[len];
                    //create native buffer
                    IntPtr ptr = Marshal.AllocHGlobal(len);
                    //send message to control
                    Win32Window.SendMessage(childHandle, (int)InkXMessage.GetData, len, (int)ptr);
                    //marshal data to byte array
                    Marshal.Copy(ptr, data, 0, len);
                    //free native memory
                    Marshal.FreeHGlobal(ptr);

                    return data;
                }
			}
			set
			{
                if (OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
                    return;

				int len = value.Length;
				IntPtr ptr = Marshal.AllocHGlobal(len);
				//copy to native memory
				Marshal.Copy(value, 0, ptr, len);
				//send message to control
				Win32Window.SendMessage(childHandle,(int)InkXMessage.SetData, len, (int)ptr);
				//free native memory
				Marshal.FreeHGlobal(ptr);
			}
		}
		#endregion

		#region IsPlaying Property
		/// <summary>
		/// Gets a value indicating whether the control is currently playing audio.
		/// </summary>
		/// <value>Returns TRUE if the control is playing voice audio, else returns FALSE.</value>
		public bool IsPlaying
		{
			get
			{
                if (OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
                    return false;
                else
                    return Convert.ToBoolean((int)Win32Window.SendMessage(childHandle, (int)InkXMessage.IsVoicePlaying, 0, 0));
			}
		}
		#endregion

		#region IsRecording Property
		/// <summary>
		/// Gets a value indicating whether the control is currently recording audio.
		/// </summary>
		/// <value>Returns TRUE if the control is recording voice audio, else returns FALSE.</value>
		public bool IsRecording
		{
			get
			{
                if (OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
                    return false;
                else
                    return Convert.ToBoolean((int)Win32Window.SendMessage(childHandle, (int)InkXMessage.IsVoiceRecording, 0, 0));
			}
		}
		#endregion


		#region Clear Method
		/// <summary>
		/// Clears the contents of the InkX control.
		/// </summary>
		public void Clear()
		{
            if (!OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
                Win32Window.SendMessage(childHandle, (int)InkXMessage.ClearAll, 0, 0);
		}
		#endregion

		#region Play Method
		/// <summary>
		/// Plays the currently selected voice object.
		/// </summary>
		public void Play()
		{
            if (!OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
			    Win32Window.SendMessage(childHandle,(int)InkXMessage.VoicePlay, 0, 0);
		}
		#endregion

		#region Stop Method
		/// <summary>
		/// Stops the currently playing audio (if applicable).
		/// </summary>
		public void Stop()
		{
            if (!OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
			    Win32Window.SendMessage(childHandle,(int)InkXMessage.VoiceStop, 0, 0);
		}
		#endregion

		#region Record Method
		/// <summary>
		/// Starts recording a new voice clip.
		/// </summary>
		public void Record()
		{
			//ensure audio is stopped first
            if (!OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
            {
                Stop();
                Win32Window.SendMessage(childHandle, (int)InkXMessage.VoiceRecord, 0, 0);
            }
		}
		#endregion

	
		#region InkX Control Functions

		[DllImport("inkx.dll", EntryPoint="InitInkX", SetLastError=true)] 
		private static extern void InitInkX();

		#endregion


		#region InkXMessage Enumeration

		private enum InkXMessage : int
		{
			/// <summary>
			/// Base message
			/// </summary>
			WM_USER = 1024,
			
			/// <summary>
			/// Required 
			/// </summary>
			WM_SETTEXT = 0x000C,
			
			/// <summary>
			/// Used to return the length of the data.
			/// </summary>
			GetDataLen  = (WM_USER + 514),

			/// <summary>
			/// Used to retrieve the data.
			/// </summary>
			GetData      = (WM_USER + 515),

			/// <summary>
			/// Used to set the data.
			/// Stores the Ink data from a previous IM_GETDATA call which will be inserted into the current compose window.
			/// </summary>
			SetData     = (WM_USER + 516),

			/// <summary>
			/// Used to erase all contents from the current compose window.
			/// </summary>
			ClearAll    = (WM_USER + 519),

			/// <summary>
			/// Sent to toggle the VoiceBar state
			/// </summary>
			VoiceBar    = (WM_USER + 530),

			/// <summary>
			/// Used to get the window handle to the RichInk control.
			/// </summary>
			GetRichInk   = (WM_USER + 532),

			/// <summary>
			/// Returns TRUE if we're currently recording
			/// </summary>
			IsVoiceRecording   = (WM_USER + 536),

			/// <summary>
			/// Causes the voicebar to halt playback or recording (if either is in progress).
			/// </summary>
			VoiceStop   = (WM_USER + 537),

			/// <summary>
			/// Plays a currently selected voice object
			/// </summary>
			VoicePlay  = (WM_USER + 540),

			/// <summary>
			/// This will begin recording to an inline embedded object.
			/// </summary>
			VoiceRecord = (WM_USER + 541),

			/// <summary>
			/// Returns TRUE if we're currently playing voice data.
			/// </summary>
			IsVoicePlaying = (WM_USER + 542),
		}
		#endregion

		#region InkXStyle Enumeration
		/// <summary>
		/// Window Styles for the InkX control.
		/// </summary>
		private enum InkXStyle : int
		{
			/// <summary>
			/// Don't create a VoiceBar.
			/// </summary>
			NoVoiceBar           = 0x0200,
			/// <summary>
			/// VoiceBar appears at bottom of the control.
			/// </summary>
			BottomVoiceBar       = 0x0400,
		}
		#endregion

		#region InkXNotification Enumeration
		/// <summary>
		/// Notifications received from the native control
		/// </summary>
		private enum InkXNotification : int
		{
			WM_USER = 1024,
		}
		#endregion


		#region INotifiable Members

#if !NDOC
        /// <summary>
        /// Message notification for InkX
        /// </summary>
        /// <param name="m"></param>
		protected override void OnNotifyMessage(ref Microsoft.WindowsCE.Forms.Message m)
		{
            if (!OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
            {
                //I wonder what was intended here...
                NMHDR nh = (NMHDR)Marshal.PtrToStructure(m.LParam, typeof(OpenNETCF.Win32.NMHDR));

                base.OnNotifyMessage(ref m);
            }
		}
#endif
		#endregion
	}
}
