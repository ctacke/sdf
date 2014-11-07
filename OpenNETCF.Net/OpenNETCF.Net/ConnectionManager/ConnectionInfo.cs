
using System;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Summary description for ConnectionInfo.
	/// </summary>
    [StructLayout(LayoutKind.Sequential)]
	internal struct ConnectionInfo
	{
		public uint cbSize;
		public uint dwParams;
		public uint dwFlags;
		public uint dwPriority;
		public int bExclusive;
		public int bDisabled;
		public Guid guidDestNet;
		public IntPtr hWnd;
		public uint uMsg;
		public uint lParam;
        public uint ulMaxCost;
        public uint ulMinRcvBw;
        public uint ulMaxConnLatency;
	};
}
