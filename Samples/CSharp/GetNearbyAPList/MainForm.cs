using System;
using System.Windows.Forms;
using OpenNETCF.Net.NetworkInformation;

namespace GetNearbyAPList
{
  public partial class MainForm : Form
  {
    private WirelessZeroConfigNetworkInterface m_wzc = null;

    public MainForm()
    {
      InitializeComponent();
      FindWirelessAdapter();
    }

    void FindWirelessAdapter()
    {
      bool foundWirelessNIC = false;
      foreach (INetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
      {
        if (ni is WirelessNetworkInterface)
        {
          // will be true for wireless or WZC
          adapterName.Text = ni.Name;
          adapterType.Text = ni.GetType().Name;
          foundWirelessNIC = true;

          if (ni is WirelessZeroConfigNetworkInterface)
          {
            m_wzc = ni as WirelessZeroConfigNetworkInterface;
            refresh.Enabled = true;
          }
          else
          {
            apList.Items.Add("No WZC adapter found");
            apList.Items.Add("Cannot retrieve nearby AP list");
            apList.Enabled = false;
          }

          break;
        }
      }

      if (foundWirelessNIC)
      {
        refresh_Click(null, null);
      }
      else
      {
        MessageBox.Show("Could not find a compatible wireless network adapter");
      }
    }

    private void refresh_Click(object sender, EventArgs e)
    {
      if (m_wzc == null) return;

      apList.Items.Clear();

      foreach (AccessPoint ap in m_wzc.NearbyAccessPoints)
      {
        apList.Items.Add(string.Format("{0} ({1}dB) [{2}]", ap.Name, ap.SignalStrength.Decibels, ap.Privacy == 0 ? "open" : "WEP")); 
      }
    }
  }
}