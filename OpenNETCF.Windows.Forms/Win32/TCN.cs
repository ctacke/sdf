using System;

namespace OpenNETCF.Win32
{
	/// <summary>
	/// Notification Messages.
	/// </summary>
	public enum TCN: int
	{
		FIRST		= (0-550),
		SELCHANGED  = (FIRST - 1),
		SELCHANGING  = (FIRST - 2),
	}
}
