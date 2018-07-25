using System;
#if !NDOC
using Microsoft.WindowsCE.Forms;
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
 
		public SoundMessageWindow()
		{
		}

		protected override void WndProc(ref Message msg)
		{
			switch(msg.Msg)
			{
				case WM_WOM_CLOSE:
				case MM_WIM_CLOSE:
					if(WaveCloseMessage != null)
					{
						WaveCloseMessage(this);
					}
					break;
				case WM_WOM_OPEN:
				case MM_WIM_OPEN:
					if(WaveOpenMessage != null)
					{
						WaveOpenMessage(this);
					}
					break;
				case MM_WIM_DATA:
				case WM_WOM_DONE:
					if(WaveDoneMessage != null)
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
