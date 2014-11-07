using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.WindowsMobile
{
	/// <summary>
	/// Controls vibration on the device.
	/// </summary>
	/// <remarks>Supported only on Smartphone. Previously named <b>OpenNETCF.Notification.Vibrate</b>. Vibration equipped Pocket PC devices emulate the Vibration motor as a notification Led.</remarks>
	/// <platform><os>Windows Mobile 2003 for Smartphone</os></platform>
	public class Vibrate
	{
		private Vibrate(){}

		/// <summary>
		/// Plays the default looping vibration on the device.
		/// </summary>
		/// <returns>TRUE if vibration started successfully else FALSE.</returns>
		/// <platform><os>Pocket PC Phone Edition, Windows Mobile 2003 for Smartphone</os></platform>
		/// <preliminary/>
		public static bool Play()
		{
            int result = NativeMethods.VibratePlay(0, IntPtr.Zero, 0xffffffff, 0xffffffff);

			if(result!=0)
			{

				return false;
			}
			return true;
		}

		


		/// <summary>
		/// Stops any current vibration.
		/// </summary>
		/// <returns>TRUE on success else FALSE.</returns>
		/// <platform><os>Pocket PC Phone Edition, Windows Mobile 2003 for Smartphone</os></platform>
		/// <preliminary/>
		public static bool Stop()
		{
            int result = NativeMethods.VibrateStop();

			if(result!=0)
			{
				return false;
			}
			return true;
		}

		


		/// <summary>
		/// Gets the current device vibration capabilities.
		/// </summary>
		/// <param name="caps">Member of the VibrationCapabilities enumeration identifying what capability to query.</param>
		/// <returns>0 if the capability is not supported, 1 if a single level is supported or a value between 2 and 7 if multiple levels are supported.</returns>
		/// <remarks>This function returns the number of vibration steps (a number from 0 to 7) that the device hardware supports for the requested vibration capability.</remarks>
		/// <platform><os>Pocket PC Phone Edition, Windows Mobile 2003 for Smartphone</os></platform>
		/// <preliminary/>
		public static int GetDeviceCaps(VibrationCapabilities caps)
		{
			return NativeMethods.VibrateGetDeviceCaps(caps);
		}

		


		/// <summary>
		/// Used by the GetDeviceCaps function to query the device vibration capabilities.
		/// </summary>
		/// <platform><os>Pocket PC Phone Edition, Windows Mobile 2003 for Smartphone</os></platform>
		/// <preliminary/>
		public enum VibrationCapabilities: int
		{
			/// <summary>
			/// Query the amplitude that the device supports.
			/// </summary>
			Amplitude,
			/// <summary>
			/// Query the frequency that the device supports.
			/// </summary>
			Frequency,

			//Last
		}


		/*public struct VibrateNote
		{
			public short Duration;
			public byte Amplitude;
			public byte Frequency;
		}*/
	}
}
