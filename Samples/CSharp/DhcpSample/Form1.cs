using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Net.NetworkInformation;

namespace DhcpSample
{
    public partial class Form1 : Form
    {
        INetworkInterface NIC { get; set; }

        public Form1()
        {
            InitializeComponent();

            INetworkInterface @interface = (from ni in NetworkInterface.GetAllNetworkInterfaces()
                                           where ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                                           select ni).FirstOrDefault();

            if (@interface == null)
            {
                eventList.Items.Insert(0, "No Ethernet adapter found");
                return;
            }

            NIC = @interface;

            eventList.Items.Insert(0, string.Format("Ethernet Adapter {0} found", NIC.Name));

            UpdateDhcpState();
        }

        private void UpdateDhcpState()
        {
            bool isEnabled = NIC.GetIPProperties().GetIPv4Properties().IsDhcpEnabled;
            eventList.Items.Insert(0, string.Format("DHCP is currently {0}", isEnabled ? "enabled" : "disabled"));
            enabled.Checked = isEnabled;
        }

        private void enabled_CheckStateChanged(object sender, EventArgs e)
        {
            if (NIC == null)
            {
                eventList.Items.Insert(0, "No NIC.  Cannot change DHCP");
                return;
            }

            bool newState = enabled.Checked;

            if (newState == NIC.GetIPProperties().GetIPv4Properties().IsDhcpEnabled)
            {
                return;
            }

            eventList.Items.Insert(0, string.Format("Changing DHCP to {0}...", newState ? "enabled" : "disabled"));

            try
            {
                NIC.GetIPProperties().GetIPv4Properties().IsDhcpEnabled = newState;
            }
            catch (Exception ex)
            {
                eventList.Items.Insert(0, string.Format("{0} changing DHCP: {1}...", ex.GetType().Name, ex.Message));
                return;
            }

            eventList.Items.Insert(0, "Rebinding NIC...");

            try
            {
                NIC.Rebind();
            }
            catch (Exception ex)
            {
                eventList.Items.Insert(0, string.Format("{0} rebinding: {1}...", ex.GetType().Name, ex.Message));
                return;
            }

            eventList.Items.Insert(0, "DHCP Changed.");
        }


    }
}