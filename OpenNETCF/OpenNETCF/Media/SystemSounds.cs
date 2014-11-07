using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Media
{
	/// <summary>
	/// Retrieves sounds associated with a set of Windows system sound event types.
	/// </summary>
	public static class SystemSounds
	{
		/// <summary>
		/// Gets the sound associated with the Beep program event.
		/// </summary>
		public static SystemSound Beep
		{
			get
			{
				return new SystemSound(0);
			}
		}


		/// <summary>
		/// Gets the sound associated with the Asterisk program event.
		/// </summary>
		public static SystemSound Asterisk
		{
			get
			{
				return new SystemSound(0x00000040);
			}
		}

		/// <summary>
		/// Gets the sound associated with the Exclamation program event.
		/// </summary>
		public static SystemSound Exclamation
		{
			get
			{
				return new SystemSound(0x00000030);
			}
		}

		
		/// <summary>
		/// Gets the sound associated with the Question program event.
		/// </summary>
		public static SystemSound Question
		{
			get
			{
				return new SystemSound(0x00000020);
			}
		}
		
		/// <summary>
		/// Gets the sound associated with the Hand program event.
		/// </summary>
		public static SystemSound Hand
		{
			get
			{
				return new SystemSound(0x00000010);
			}
		}
	}
}
