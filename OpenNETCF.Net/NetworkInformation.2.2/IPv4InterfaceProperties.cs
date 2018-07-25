using System;
using System.Text;
using Microsoft.Win32;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Provides information about network interfaces that support Internet Protocol
    /// version 4 (IPv4).
    /// </summary>
    public class IPv4InterfaceProperties
    {
        private IPInterfaceProperties m_props;
        private int m_mtu = 0;

        /// <summary>
        /// Initializes a new instance of the System.Net.NetworkInformation.IPv4InterfaceProperties
        /// class.
        /// </summary>
        internal IPv4InterfaceProperties(IPInterfaceProperties props, int mtu)
        {
            m_props = props;
            m_mtu = mtu;
        }

        /// <summary>
        /// Gets the interface index for the Internet Protocol version 4 (IPv4) address.
        /// </summary>
        public int Index 
        {
            get { return m_props.m_info.Index; }
        }

        /// <summary>
        /// Gets a System.Boolean value that indicates whether this interface has an
        /// automatic private IP addressing (APIPA) address.
        /// </summary>
        public bool IsAutomaticPrivateAddressingActive 
        {
            get 
            {
                m_props.RefreshPerAdapterInfo();
                return m_props.m_autoConfigActive; 
            }
        }

        /// <summary>
        /// Gets a System.Boolean value that indicates whether this interface has automatic
        /// private IP addressing (APIPA) enabled.
        /// </summary>
        public bool IsAutomaticPrivateAddressingEnabled
        {
            get
            {
                m_props.RefreshPerAdapterInfo();
                return m_props.m_autoConfigEnabled;
            }
        }

        /// <summary>
        /// Gets or Sets a System.Boolean value that indicates whether the interface is configured
        /// to use a Dynamic Host Configuration Protocol (DHCP) server to obtain an IP
        /// address.
        /// </summary>
        /// <remarks>After Setting this property, you must Rebind the adapter for it to take effect</remarks>
        public bool IsDhcpEnabled
        {
            get { return m_props.m_info.DHCPEnabled; }
            set
            {
                // Update the local copy of the state.
                // Well, on second thought, maybe we should have to
                // be refreshed to get this updated.
                //				dhcpenabled = value;

                // Modify the registry keys associated with this
                // adapter to enable DHCP.  Once that is done, we
                // have to rebind the network adapter to actually
                // make the change from static to DHCP.  We try to
                // only do this if there has been a change.
                string regName = "\\comm\\" + m_props.m_info.AdapterName + "\\Parms\\Tcpip";

                // Open the base key for the adapter.
                RegistryKey tcpipkey = Registry.LocalMachine.OpenSubKey(regName, true);

				//jsm - Bug 198: Unnecessary cast to UInt32 should have been signed Int

                // Get the current value of the DHCPEnabled value.  If
                // it already matches the value we're trying to set, we
                // don't have to change it.
                int oldEnableDHCP = (Int32)tcpipkey.GetValue("EnableDHCP", 1);
                bool oldEnableDHCPB = (oldEnableDHCP != 0) ? true : false;

                if (oldEnableDHCPB != value)
                {
                    // Make the change.
                    tcpipkey.SetValue("EnableDHCP", value ? 1 : 0);
                }

                tcpipkey.Close();
            }
        }

        /// <summary>
        /// Gets the maximum transmission unit (MTU) for this network interface.
        /// </summary>
        public int Mtu
        {
            get { return m_mtu; } 
        }

        /// <summary>
        /// Gets a System.Boolean value that indicates whether an interface uses Windows
        /// Internet Name Service (WINS).
        /// </summary>
        public bool UsesWins
        {
            get { return m_props.m_info.HaveWINS; } 
        }
    }
}
