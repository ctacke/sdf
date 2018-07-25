using System;

namespace OpenNETCF.Net.NetworkInformation
{

	internal class SystemIPInterfaceProperties : IPInterfaceProperties
	{
		uint index;

		public SystemIPInterfaceProperties(IpAdapterInfo adapter)
		{
			index = adapter.index;
		}

		public override IPInterfaceStatistics GetIPInterfaceStatistics()
		{
			return new SystemIPInterfaceStatistics((ulong)this.index);
		}
	}
}
