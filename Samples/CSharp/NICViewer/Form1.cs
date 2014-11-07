using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Net.NetworkInformation;

namespace NICViewer
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();

      nicList.Columns.Add("Name", -1, HorizontalAlignment.Left);
      nicList.Columns.Add("IP", -1, HorizontalAlignment.Left);

      nicList.ContextMenu = nicContext;

      RefreshNICList();
    }


    private void RefreshNICList()
    {
      nicList.BeginUpdate();
      nicList.Items.Clear();

      foreach (INetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
      {
        ListViewItem lvi = new ListViewItem(
          new string[] {
            ni.Name,
            ni.CurrentIpAddress.ToString()
          });

        lvi.Tag = ni;
        nicList.Items.Add(lvi);
      }
      nicList.EndUpdate();
    }

    NicProperties m_propsDialog = null;

    private void nicPropertiesMenuItem_Click(object sender, EventArgs e)
    {
      if (nicList.SelectedIndices.Count != 1) return;

      if (m_propsDialog == null)
      {
        // lazy load
        m_propsDialog = new NicProperties();
      }

      m_propsDialog.NIC = nicList.Items[nicList.SelectedIndices[0]].Tag as INetworkInterface;
      m_propsDialog.ShowDialog();

      // refresh list on return
      RefreshNICList();
    }
  }
}