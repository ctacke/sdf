

using System;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;

using OpenNETCF.Runtime.InteropServices;

namespace OpenNETCF.Net
{
	/// <summary>
	/// Summary description for Network.
	/// </summary>
	public class Network
	{

		/// <summary>
		/// Maps the network resouce to the specified share name
		/// </summary>
		/// <param name="hwnd">Owner window handle</param>
		/// <param name="netRes">Network resource to connect</param>
		/// <param name="shareName">Share name</param>
		/// <param name="userName">User name</param>
		/// <param name="password">Password</param>
		public static void MapDrive(IntPtr hwnd, string netRes, string shareName, string userName, string password)
		{
			NETRESOURCE NetRes = new NETRESOURCE();			
			NetRes.dwScope = RESOURCE_GLOBALNET | RESOURCE_REMEMBERED;
			NetRes.dwType = RESOURCETYPE_DISK;
			NetRes.dwDisplayType = RESOURCEDISPLAYTYPE_SHARE;
			NetRes.dwUsage = RESOURCEUSAGE_CONNECTABLE;
			NetRes.lpRemoteName = Marshal2.StringToHGlobalUni(netRes);
			NetRes.lpLocalName = Marshal2.StringToHGlobalUni(shareName);
			NetRes.lpComment = IntPtr.Zero;
			NetRes.lpProvider = IntPtr.Zero;
				
			int ret = WNetAddConnection3(hwnd, NetRes, password, userName, 1);

			if (ret != 0)
			{
				throw new System.ComponentModel.Win32Exception(ret, ((NetworkErrors)ret).ToString());
			}
				
		}

		/// <summary>
		/// Disconnects the network resource
		/// </summary>
		/// <param name="shareName">Local share or remote name</param>
		/// <param name="force">Force disconnect</param>
		public static void Disconnect(string shareName, bool force)
		{
			if ((shareName!=null) && (shareName != String.Empty))
			{
				int ret = WNetCancelConnection2(shareName, 1, (force)?1:0);

				if (ret != 0)
				{
					throw new System.ComponentModel.Win32Exception(ret, ((NetworkErrors)ret).ToString());
				}
			}

		}

		/// <summary>
		/// Returns name of the network resource
		/// </summary>
		/// <param name="shareName"></param>
		/// <returns>Network resource</returns>
		public static string GetRemoteName(string shareName)
		{
			StringBuilder sb = new StringBuilder(255);
			int lenBuff = 255;
			
			int ret = WNetGetConnection(shareName, sb, ref lenBuff);
			
			if (ret != 0)
			{
				throw new System.ComponentModel.Win32Exception(ret, ((NetworkErrors)ret).ToString());
			}

			return sb.ToString();
		}

		/// <summary>
		/// Enumerates and returns all connected network resources.
		/// </summary>
		/// <returns>Array of NetworkResource class</returns>
		public static NetworkResource[] GetConnectedResources()
		{
			NETRESOURCE netRes = new NETRESOURCE();	
			IntPtr hEnum = IntPtr.Zero;

			int ret = WNetOpenEnum(RESOURCE_CONNECTED, RESOURCETYPE_ANY, 0, IntPtr.Zero, ref hEnum);
			
			if (ret != 0)
			{
				throw new System.ComponentModel.Win32Exception(ret, ((NetworkErrors)ret).ToString());
			}
			
			//Allocate memory for NETRESOURCE array
			int bufferSize = 16384;
            IntPtr buffer = Marshal.AllocHGlobal(bufferSize);
            OpenNETCF.Runtime.InteropServices.Marshal2.SetMemory(buffer, 0, bufferSize, false);

			if (buffer == IntPtr.Zero)
			{
				throw new OutOfMemoryException("There's not enough native memory.");
			}
			
			uint c = 0xFFFFFFFF;

			int count = (int)c;
			int size = Marshal.SizeOf(typeof(NETRESOURCE));
			ArrayList arrList = new ArrayList();

			ret = WNetEnumResource(hEnum, ref count, buffer, ref bufferSize);
			if (ret == 0)
			{
				IntPtr currPtr = buffer;
				for(int i=0;i<count;i++)
				{
					netRes = (NETRESOURCE)Marshal.PtrToStructure(currPtr, typeof(NETRESOURCE));
					NetworkResource res = new NetworkResource(Marshal.PtrToStringUni(netRes.lpLocalName), Marshal.PtrToStringUni(netRes.lpRemoteName));
					//res.RemoteName = Marshal.PtrToStringUni(netRes.lpRemoteName);
					//res.ShareName = Marshal.PtrToStringUni(netRes.lpLocalName);
					arrList.Add(res);
					currPtr = new IntPtr((int)currPtr + size);

				}
			}
			else
			{
				//clean up
                Marshal.FreeHGlobal(buffer);
			}

			//clean up
            Marshal.FreeHGlobal(buffer);

			return (NetworkResource[])arrList.ToArray(typeof(NetworkResource));


		}

		/// <summary>
		/// Enumerates network resources.
		/// </summary>
		/// <param name="remoteName">The name of the server</param>
		/// <returns>Array of NetworkResource class</returns>
		public static NetworkResource[] GetNetworkResources(string remoteName)
		{
			NETRESOURCE netRes = new NETRESOURCE();			
			netRes.dwScope = RESOURCE_GLOBALNET;
			netRes.dwType = RESOURCETYPE_DISK;
			netRes.dwUsage = RESOURCEUSAGE_CONTAINER;
			netRes.lpRemoteName = Marshal2.StringToHGlobalUni(remoteName);
			netRes.lpLocalName = Marshal2.StringToHGlobalUni("");
			netRes.lpComment = IntPtr.Zero;
			netRes.lpProvider = IntPtr.Zero;
			
			IntPtr hEnum = IntPtr.Zero;

			int ret = WNetOpenEnum(RESOURCE_GLOBALNET, RESOURCETYPE_ANY, 0, netRes, ref hEnum);
			
			if (ret != 0)
			{
				throw new System.ComponentModel.Win32Exception(ret, ((NetworkErrors)ret).ToString());
			}
			
			//Allocate memory for NETRESOURCE array
			int bufferSize = 16384;
            IntPtr buffer = Marshal.AllocHGlobal(bufferSize);
            OpenNETCF.Runtime.InteropServices.Marshal2.SetMemory(buffer, 0, bufferSize, false);

			if (buffer == IntPtr.Zero)
			{
				throw new OutOfMemoryException("There's not enough native memory.");
			}
			
			uint c = 0xFFFFFFFF;

			int count = (int)c;
			int size = Marshal.SizeOf(typeof(NETRESOURCE));
			ArrayList arrList = new ArrayList();

			ret = WNetEnumResource(hEnum, ref count, buffer, ref bufferSize);
			if (ret == 0)
			{
				IntPtr currPtr = buffer;
				for(int i=0;i<count;i++)
				{
					netRes = (NETRESOURCE)Marshal.PtrToStructure(currPtr, typeof(NETRESOURCE));
					NetworkResource res = new NetworkResource("", Marshal.PtrToStringUni(netRes.lpRemoteName));
					//res.RemoteName = Marshal.PtrToStringUni(netRes.lpRemoteName);
					
					arrList.Add(res);
					currPtr = new IntPtr((int)currPtr + size);

				}
			}
			else
			{
				//clean up
                Marshal.FreeHGlobal(buffer);
				throw new System.ComponentModel.Win32Exception(ret, ((NetworkErrors)ret).ToString());
			}

			//clean up
            Marshal.FreeHGlobal(buffer);

			return (NetworkResource[])arrList.ToArray(typeof(NetworkResource));

		}

        /// <summary>
        /// Possible network errors
        /// </summary>
		public enum NetworkErrors
		{
            /// <summary>
            /// No errors
            /// </summary>
			NoError = 0,
            /// <summary>
            /// Access is denied
            /// </summary>
			AccessDenied = 5,
            /// <summary>
            /// The handle provided was invalid
            /// </summary>
			InvalidHandle = 6,
            /// <summary>
            /// Not enough memory to perform desired command
            /// </summary>
			NotEnoughMemory = 8,
            /// <summary>
            /// Call not supported
            /// </summary>
			NotSupported = 50,
            /// <summary>
            /// Unexpected network error
            /// </summary>
			UnexpectedNetError	 = 59,
            /// <summary>
            /// Invalid password
            /// </summary>
			InvalidPassword = 86,
            /// <summary>
            /// Invalid parameter
            /// </summary>
			InvalidParameter = 87,
            /// <summary>
            /// Invalid Level
            /// </summary>
			InvalidLevel = 124,
            /// <summary>
            /// Busy
            /// </summary>
			Busy = 170,
            /// <summary>
            /// More data is available
            /// </summary>
			MoreData = 234,
            /// <summary>
            /// The address provided is not valid
            /// </summary>
			InvalidAddress = 487,
            /// <summary>
            /// A connection is not available
            /// </summary>
			ConnectionUnavailable = 1201,
            /// <summary>
            /// The device has already been remembered
            /// </summary>
			DeviceAlreadyRemembered = 1202,
            /// <summary>
            /// Extended error information is available
            /// </summary>
			ExtentedError = 1208,
            /// <summary>
            /// Call cancelled
            /// </summary>
			Cancelled = 1223,
            /// <summary>
            /// Retry
            /// </summary>
			Retry = 1237,
            /// <summary>
            /// Username is invalid
            /// </summary>
			BadUsername = 2202,
            /// <summary>
            /// No network is available
            /// </summary>
			NoNetwork = 1222									
		}

		#region P/Invokes

		const int RESOURCE_CONNECTED  =    0x00000001;
		const int RESOURCE_GLOBALNET  =    0x00000002;
		const int RESOURCE_REMEMBERED =    0x00000003;

		const int RESOURCETYPE_ANY   =     0x00000000;
		const int RESOURCETYPE_DISK  =     0x00000001;
		const int RESOURCETYPE_PRINT =     0x00000002;

		const int RESOURCEDISPLAYTYPE_GENERIC   =     0x00000000;
		const int RESOURCEDISPLAYTYPE_DOMAIN    =     0x00000001;
		const int RESOURCEDISPLAYTYPE_SERVER    =     0x00000002;
		const int RESOURCEDISPLAYTYPE_SHARE     =     0x00000003;
		const int RESOURCEDISPLAYTYPE_FILE      =     0x00000004;
		const int RESOURCEDISPLAYTYPE_GROUP     =     0x00000005;

		const int  RESOURCEUSAGE_CONNECTABLE =  0x00000001;
		const int RESOURCEUSAGE_CONTAINER   =  0x00000002;

		[DllImport("coredll.dll")]
		private static extern int WNetAddConnection3(
			IntPtr hwndOwner, 
			NETRESOURCE lpNetResource, 
			string lpPassword, 
			string lpUserName, 
			int dwFlags);

		[DllImport("coredll.dll")]
		private static extern int WNetCancelConnection2(
			string lpName, 
			int dwFlags, 
			int fForce);

		[DllImport("coredll.dll")]
		private static extern int WNetGetConnection( 
			string lpLocalName, 
			StringBuilder lpRemoteName, 
			ref int lpnLength);

		[DllImport("coredll.dll")]
		private static extern int WNetOpenEnum(
			int dwScope, 
			int dwType, 
			int dwUsage, 
			NETRESOURCE lpNetResource, 
			ref IntPtr lphEnum);

		[DllImport("coredll.dll")]
		private static extern int WNetOpenEnum(
			int dwScope, 
			int dwType, 
			int dwUsage, 
			IntPtr lpNetResource, 
			ref IntPtr lphEnum);

		[DllImport("coredll.dll")]
		private static extern int WNetEnumResource( 
			IntPtr hEnum, 
			ref int lpcCount, 
			IntPtr lpBuffer, 
			ref int lpBufferSize 
			);

		private class NETRESOURCE 
		{
			public int dwScope; 
			public int dwType; 
			public int dwDisplayType; 
			public int dwUsage; 
			public IntPtr lpLocalName; 
			public IntPtr lpRemoteName; 
			public IntPtr lpComment; 
			public IntPtr lpProvider; 
		}
		#endregion
	}
	
	/// <summary>
	/// Implements NetworkResouce class
	/// </summary>
	public class NetworkResource
	{
		private string shareName;
		private string remoteName;

        /// <summary>
        /// Creates an instance of a NetworkResource class
        /// </summary>
        /// <param name="shareName">Share Name</param>
        /// <param name="remoteName">Remote Name</param>
		internal NetworkResource(string shareName, string remoteName)
		{
			this.shareName = shareName;
			this.remoteName = remoteName;
		}

		/// <summary>
		/// Gets ShareName
		/// </summary>
		public string ShareName
		{
			get
			{
				return shareName;
			}
		}
		
		/// <summary>
		/// Gets Remote name.
		/// </summary>
		public string RemoteName
		{
			get
			{
				return remoteName;
			}
		}
	}
}
