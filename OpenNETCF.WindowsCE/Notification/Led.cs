using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace OpenNETCF.WindowsCE.Notification
{
	/// <summary>
	/// Represents the collection of Notification Leds on the device.
	/// </summary>
	/// <remarks>Support varies depending on the device but all devices should include at least 1 notification LED</remarks>
	public class Led
	{
		private const int NLED_COUNT_INFO_ID = 0;
		private const int NLED_SUPPORTS_INFO_ID	= 1;
		private const int NLED_SETTINGS_INFO_ID	= 2;

		private int m_count;

		/// <summary>
		/// Initialise the LED collection
		/// </summary>
		public Led()
		{
			NLED_COUNT_INFO CountStruct = new NLED_COUNT_INFO();
            if (!NativeMethods.NLedGetDeviceCount(NLED_COUNT_INFO_ID, ref CountStruct))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error(), "Error Initialising LED's");
			}

			m_count = (int)CountStruct.cLeds;
		}

		/// <summary>
		/// Returns the number of notification Leds in the system
		/// </summary>
		public int Count
		{
			get
			{
				return m_count;
			}
		}

		/// <summary>
		/// Set the state of the specified LED
		/// </summary>
		/// <param name="led">0 based index of the LED</param>
		/// <param name="newState">New state of the LED - see LedState enumeration</param>
		public void SetLedStatus(int led, LedState newState)
		{
			NLED_SETTINGS_INFO nsi = new NLED_SETTINGS_INFO();

			nsi.LedNum = led;
			nsi.OffOnBlink = (int)newState;
			bool bSuccess = NativeMethods.NLedSetDevice(NLED_SETTINGS_INFO_ID, ref nsi);
		}


			
		internal struct NLED_COUNT_INFO
		{
			public uint cLeds;
		}


		internal struct NLED_SETTINGS_INFO
		{
			public int LedNum; // LED number, 0 is first LED
			public int OffOnBlink; // 0 == off, 1 == on, 2 == blink
			public int TotalCycleTime; // total cycle time of a blink in microseconds
			public int OnTime; // on time of a cycle in microseconds
			public int OffTime; // off time of a cycle in microseconds
			public int MetaCycleOn; // number of on blink cycles
			public int MetaCycleOff; // number of off blink cycles
		}

		internal struct NLED_SUPPORTS_INFO
		{
#pragma warning disable 0649
			public uint	LedNum;			// LED number, 0 is first LED
			public int	lCycleAdjust;	// Granularity of cycle time adjustments (microseconds)
			public bool	fAdjustTotalCycleTime;// LED has an adjustable total cycle time
			public bool	fAdjustOnTime;	// LED has separate on time
			public bool	fAdjustOffTime; // LED has separate off time
			public bool	fMetaCycleOn;	// LED can do blink n, pause, blink n, ...
			public bool	fMetaCycleOff;	// LED can do blink n, pause n, blink n, ...
#pragma warning restore 0649
        }

		/// <summary>
		/// Defines the possible states for an LED
		/// </summary>
		public enum LedState : int
		{
			/// <summary>
			/// LED is off
			/// </summary>
			Off = 0,
			/// <summary>
			/// LED is on
			/// </summary>
			On = 1,
			/// <summary>
			/// LED cycles between On and Off
			/// </summary>
			Blink = 2
		}
	}
}