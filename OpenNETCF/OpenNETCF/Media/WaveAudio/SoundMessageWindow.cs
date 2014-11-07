using System;
#if !NDOC
using Microsoft.WindowsCE.Forms;
using System.Diagnostics;
#endif

namespace OpenNETCF.Media.WaveAudio
{
#if!NDOC
	internal class SoundMessageWindow : MessageWindow
	{
		public event WaveOpenHandler		WaveOpenMessage;
		public event WaveCloseHandler		WaveCloseMessage;
		public event WaveDoneHandler		WaveDoneMessage;

		public const int WM_WOM_OPEN  = 0x03BB;
		public const int WM_WOM_CLOSE = 0x03BC;
		public const int WM_WOM_DONE  = 0x03BD;
		public const int MM_WIM_OPEN  = 0x03BE;
		public const int MM_WIM_CLOSE = 0x03BF;
		public const int MM_WIM_DATA  = 0x03C0;

        private int m_lastOpen = 0;
        private int m_lastDone = 0;

		public SoundMessageWindow()
		{
		}

		protected override void WndProc(ref Message msg)
		{
			switch(msg.Msg)
			{
				case WM_WOM_CLOSE:
				case MM_WIM_CLOSE:
                    m_lastDone = Environment.TickCount;
                    int now = Environment.TickCount;
                    Debug.WriteLine(string.Format("Wave Close - ETs: {0}, {1}", now - m_lastDone, now - m_lastOpen));
                    if (WaveCloseMessage != null)
					{
                        WaveCloseMessage(this);
					}
					break;
				case WM_WOM_OPEN:
				case MM_WIM_OPEN:
                    m_lastOpen = Environment.TickCount;
                    Debug.WriteLine("Wave Open");
                    if (WaveOpenMessage != null)
					{
						WaveOpenMessage(this);
					}
					break;
				case MM_WIM_DATA:
				case WM_WOM_DONE:
                    m_lastDone = Environment.TickCount;
                    Debug.WriteLine("Wave Done - ET: " + (m_lastDone - m_lastOpen).ToString());
                    if (WaveDoneMessage != null)
					{
						WaveDoneMessage(this, msg.WParam, msg.LParam);
					}
					break;
			}
 
			// call the base class WndProc for default message handling
			base.WndProc(ref msg);
		}
	}
#endif
}
