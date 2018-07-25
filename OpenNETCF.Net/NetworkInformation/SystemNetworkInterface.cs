using System;
using System.Collections;
using System.Text;
using OpenNETCF.Net;

namespace OpenNETCF.Net.NetworkInformation
{
	internal class SystemNetworkInterface : NetworkInterface
	{
		internal uint index;
		SystemIPInterfaceProperties interfaceProperties;

		private SystemNetworkInterface(IpAdapterInfo adapterInfo)
		{
			this.index = adapterInfo.index;
			this.interfaceProperties = new SystemIPInterfaceProperties(adapterInfo);
		}

		internal static SystemNetworkInterface[] GetNetworkInterfaces()
		{
            ArrayList al = new ArrayList();

            AdapterCollection ac = Networking.GetAdapters();
			foreach(Adapter a in ac)
			{
				IpAdapterInfo i = new IpAdapterInfo();
				i.index = (uint)a.Index;
				al.Add(new SystemNetworkInterface(i));
			}

			return (SystemNetworkInterface[])al.ToArray(typeof(SystemNetworkInterface));
		}

		public override IPInterfaceProperties GetIPInterfaceProperties()
		{
			return this.interfaceProperties;
		}
	}
}
