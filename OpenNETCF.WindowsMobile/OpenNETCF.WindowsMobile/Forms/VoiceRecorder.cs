#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion




using System;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;
using OpenNETCF.Runtime.InteropServices;
using OpenNETCF.Win32;
using OpenNETCF.Windows.Forms;

namespace OpenNETCF.WindowsMobile.Forms
{

	

	/// <summary>
    /// Allows voice recording and playback.
	/// </summary>
	public class VoiceRecorder : Control2
	{
        private VoiceRecorderData _vrd;
		private string _filename = null;
		private VoiceRecorderStyles _styles;

		/// <summary>
		/// Creates a new <see cref="VoiceRecorder">Voice Recorder</see> control.
		/// </summary>
		public VoiceRecorder()
		{
		}

#if !NDOC
		IntPtr _vr;

        /// <summary>
        /// If host control erase background message has been set then VoiceRecorder probably was closed.
        /// </summary>
        /// <param name="m"></param>
		protected override void OnEraseBkgndMessage(ref Microsoft.WindowsCE.Forms.Message m)
		{
            if (OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
                return;

            Hide();	
			CleanFileName();
		}
#endif

		/// <summary>
		/// Shows <see cref="VoiceRecorder">Voice Recorder</see> control for a specific filename.
		/// </summary>
		/// <param name="fileName"></param>
		public void Show(string fileName)
		{
			if (fileName == null) throw new ArgumentNullException(fileName, "FileName must be set");

			base.Show();

#if !NDOC
            if (!OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
            {

                _filename = fileName;
                CleanFileName();

                this.VoiceRecorderDataItem.lpszRecordFileName = Marshal2.StringToHGlobalUni(_filename);
                this.VoiceRecorderDataItem.hwndParent = this.Handle;
                this.VoiceRecorderDataItem.Styles = _styles;

                _vr = VoiceRecorder_Create(this.VoiceRecorderDataItem);
                ShowWindow(_vr, 1);
                UpdateWindow(_vr);

                Rectangle r = Win32.Win32Window.GetWindowRect(_vr);
                Width = r.Width;
                Height = r.Height;
            }
            else
            {
                Width = 127;
                Height = 26;
            }
#endif			
		}

        /// <summary>
        /// Added for designer support.  all _vrd variables have been changed to VoiceRecorderDataItem
        /// </summary>
        private VoiceRecorderData VoiceRecorderDataItem
        {
            get
            {
                if (this._vrd == null)
                    this._vrd = new VoiceRecorderData(OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this));

                return this._vrd;
            }
        }
		/// <summary>
		/// Show Voice Recorder with filename from <see cref="FileName" />.
		/// </summary>
		public new void Show()
		{
			Show(_filename);
		}

		/// <summary>
		/// Gets or sets the filename to which the voice recording is to be saved.
		/// </summary>
		public string FileName
		{
			get
			{
				return _filename;
			}
			set
			{
                _filename = value;				
			}
		}

		/// <summary>
		/// Gets or sets styles how the <see cref="VoiceRecorder">Voice Recorder</see> behaves.
		/// </summary>
		public VoiceRecorderStyles Styles
		{
			get
			{
				return _styles;
			}
			set
			{
				_styles = value;				
			}
		}

		private void CleanFileName()
		{
            if (OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
                return;

			//dispose native string
            if (this.VoiceRecorderDataItem.lpszRecordFileName != IntPtr.Zero)
			{
                Marshal.FreeHGlobal(this.VoiceRecorderDataItem.lpszRecordFileName);
			}
		}

		/// <summary>
		/// Releases the resources used by the control.
		/// </summary>
		/// <param name="disposing">True to release both managed and unmanaged resources; False to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
            if (OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
                return;

			CleanFileName();
			base.Dispose (disposing);
		}

		#region Events
		/// <summary>
		/// Occurs when started playback.
		/// </summary>
		public event EventHandler Play;
		/// <summary>
		/// Raises the <see cref="Play"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
		protected virtual void OnPlay(EventArgs e)
		{
			if(this.Play!=null)
			{
				this.Play(this, e);
			}
		}

		/// <summary>
		/// Occurs when started recording.
		/// </summary>
		public event EventHandler Record;
		/// <summary>
		/// Raises the <see cref="Record"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
		protected virtual void OnRecord(EventArgs e)
		{
			if(this.Record!=null)
			{
				this.Record(this, e);
			}
		}
		
		/// <summary>
		/// Occurs when ended playback/recording.
		/// </summary>
		public event EventHandler Stop;
		/// <summary>
		/// Raises the <see cref="Stop"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
		protected virtual void OnStop(EventArgs e)
		{
			if(this.Stop!=null)
			{
				this.Stop(this, e);
			}
		}

		/// <summary>
		/// Occurs when user selected OK to save any recording file.
		/// </summary>
		public event EventHandler Ok;
		/// <summary>
		/// Raises the <see cref="Ok"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
		protected virtual void OnOk(EventArgs e)
		{
			if(this.Ok!=null)
			{
				this.Ok(this, e);
			}
		}

		/// <summary>
		/// Occurs when user selected CANCEL.
		/// </summary>
		public event EventHandler Cancel;
		/// <summary>
		/// Raises the <see cref="Cancel"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
		protected virtual void OnCancel(EventArgs e)
		{
			if(this.Cancel!=null)
			{
				this.Cancel(this, e);
			}
		}

		/// <summary>
		/// Occurs when error happend. 
		/// </summary>
		public event EventHandler Error;
		/// <summary>
		/// Raises the <see cref="Error"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
		protected virtual void OnError(EventArgs e)
		{
			if(this.Error!=null)
			{
				this.Error(this, e);
			}
		}
		#endregion

#if !NDOC
		/// <summary>
		/// Handles notification messages from the native control
		/// </summary>
		/// <param name="m">Message</param>
		protected override void OnNotifyMessage(ref Microsoft.WindowsCE.Forms.Message m)
		{
            if(OpenNETCF.Windows.Forms.StaticMethods.IsDesignMode(this))
                return;

			//marshal data to custom nmhtml struct
			//Marshal.PtrToStructure doesn't work so marshalling items individually
			NM_VOICERECORDER nmvr = new NM_VOICERECORDER();
			nmvr.hwndFrom = (IntPtr)Marshal.ReadInt32(m.LParam, 0);
			nmvr.idFrom = Marshal.ReadInt32(m.LParam, 4);
			nmvr.code = (VoiceRecorderNotification)Marshal.ReadInt32(m.LParam, 8);
			nmvr.dwExtra = Marshal.ReadInt32(m.LParam, 12);

			//check the incoming message code and process as required
			switch(nmvr.code)
			{
				case VoiceRecorderNotification.VRN_PLAY_START:
					OnPlay(EventArgs.Empty);
					break;

				case VoiceRecorderNotification.VRN_PLAY_STOP:
				case VoiceRecorderNotification.VRN_RECORD_STOP:
					OnStop(EventArgs.Empty);
					break;

				case VoiceRecorderNotification.VRN_RECORD_START:
					OnRecord(EventArgs.Empty);
					break;

				case VoiceRecorderNotification.VRN_OK:
					OnOk(EventArgs.Empty);
					break;

				case VoiceRecorderNotification.VRN_CANCEL:
					OnCancel(EventArgs.Empty);
					break;

				case VoiceRecorderNotification.VRN_ERROR:
					OnError(EventArgs.Empty);
					break;
			}
		}
#endif

		[DllImport("voicectl.dll", EntryPoint="VoiceRecorder_Create")]
		private static extern IntPtr VoiceRecorder_Create(VoiceRecorderData lpVR);

		[DllImport("coredll.dll")]
		private static extern void ShowWindow(IntPtr hwnd, int shStatus);

		[DllImport("coredll.dll")]
		private static extern void UpdateWindow(IntPtr hwnd);

		[DllImport("coredll.dll")]
		private static extern bool IsWindow(IntPtr hwnd);

		/// <summary>
		/// Notification structure received from the voice control.
		/// </summary>
		private struct NM_VOICERECORDER
		{
			public IntPtr hwndFrom; 
			public int idFrom; 
			public VoiceRecorderNotification code;
			public int dwExtra;   
		}

		private enum VoiceRecorderNotification : int
		{
			VRN_FIRST           =   -860,    

			VRN_RECORD_START	=(VRN_FIRST-1),
			VRN_RECORD_STOP		=(VRN_FIRST-2),
			VRN_PLAY_START		=(VRN_FIRST-3),
			VRN_PLAY_STOP		=(VRN_FIRST-4),
			VRN_CANCEL			=(VRN_FIRST-5),
			VRN_OK				=(VRN_FIRST-6),
			VRN_ERROR			=(VRN_FIRST-7),
		}

		[Flags()]
		private enum PrivateVoiceRecorderStyle : int
		{
			VRS_NO_NOTIFY    = 0x0002,		// No parent Notifcation
			VRS_MODAL	     = 0x0004,		// Control is Modal     
			VRS_NO_MOVE		 = 0x0040		// Grip is removed and the control cannot be moved around by the user
		}

		private class VoiceRecorderData
		{
			private static int currentid = 1;

			private short						cb;                     
			private PrivateVoiceRecorderStyle	dwStyle;
			internal  int						xPos;
			internal  int						yPos;
			internal  IntPtr					hwndParent;
			private  int						id;
			internal  IntPtr					lpszRecordFileName;

			public VoiceRecorderStyles Styles
			{
				set
				{
                    dwStyle = (PrivateVoiceRecorderStyle)value | PrivateVoiceRecorderStyle.VRS_NO_MOVE;
				}
			}
			

			public VoiceRecorderData(bool isDesignMode)
			{
                if (isDesignMode)
                    return;

                currentid++;

				cb = (short)Marshal.SizeOf(this);
				id = currentid;

				xPos = 0;
				yPos = 0;
				lpszRecordFileName = IntPtr.Zero;
			}

		}
	}
}
