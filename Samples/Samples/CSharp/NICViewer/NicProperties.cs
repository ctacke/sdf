#region --- Copyright Information --- 
/*
 *******************************************************************
|                                                                   |
|           OpenNETCF Smart Device Framework 2.2                    |
|                                                                   |
|                                                                   |
|       Copyright (c) 2000-2008 OpenNETCF Consulting LLC            |
|       ALL RIGHTS RESERVED                                         |
|                                                                   |
|   The entire contents of this file is protected by U.S. and       |
|   International Copyright Laws. Unauthorized reproduction,        |
|   reverse-engineering, and distribution of all or any portion of  |
|   the code contained in this file is strictly prohibited and may  |
|   result in severe civil and criminal penalties and will be       |
|   prosecuted to the maximum extent possible under the law.        |
|                                                                   |
|   RESTRICTIONS                                                    |
|                                                                   |
|   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           |
|   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          |
|   SECRETS OF OPENNETCF CONSULTING LLC THE REGISTERED DEVELOPER IS |
|   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    |
|   CONTROLS AS PART OF A COMPILED EXECUTABLE PROGRAM ONLY.         |
|                                                                   |
|   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      |
|   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        |
|   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       |
|   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  |
|   AND PERMISSION FROM OPENNETCF CONSULTING LLC                    |
|                                                                   |
|   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       |
|   ADDITIONAL RESTRICTIONS.                                        |
|                                                                   |
 ******************************************************************* 
*/
#endregion



using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Net.NetworkInformation;
using System.Net;
using System.Threading;

namespace NICViewer
{
  public partial class NicProperties : Form
  {
    private INetworkInterface m_nic;
    private bool m_dirty = false;

    public NicProperties()
    {
      InitializeComponent();

      dhcpEnabled.CheckStateChanged += new EventHandler(dhcpEnabled_CheckStateChanged);
      dhcpRenew.Click += new EventHandler(dhcpRenew_Click);

      currentIP.Validating += new CancelEventHandler(OnPropChange);
      currentGateway.Validating += new CancelEventHandler(OnPropChange);
      currentNetMask.Validating += new CancelEventHandler(OnPropChange);
    }

    void OnPropChange(object sender, CancelEventArgs e)
    {
      m_dirty = true;
    }

    void dhcpRenew_Click(object sender, EventArgs e)
    {
      try
      {
        DisableRenew();
        Cursor.Current = Cursors.WaitCursor;
        ThreadPool.QueueUserWorkItem( delegate(object o)
        {
          m_nic.DhcpRelease();
          m_nic.Refresh();
          RefreshProps();
          m_nic.DhcpRenew();
          Thread.Sleep(5000);
          m_nic.Refresh();
          RefreshProps();
          EnableRenew();
        });
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    void dhcpEnabled_CheckStateChanged(object sender, EventArgs e)
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        m_nic.GetIPProperties().GetIPv4Properties().IsDhcpEnabled = dhcpEnabled.Checked;
        m_nic.Rebind();
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
      RefreshProps();
    }

    private delegate void RefreshHandler();

    private void DisableRenew()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new RefreshHandler(DisableRenew));
        return;
      }
      dhcpRenew.Enabled = false;
    }

    private void EnableRenew()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new RefreshHandler(EnableRenew));
        return;
      }
      dhcpRenew.Enabled = true;
    }

    private void RefreshProps()
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new RefreshHandler(RefreshProps));
        return;
      }

      nicName.Text = m_nic.Name;
      nicDescription.Text = m_nic.Description;
      nicSpeed.Text = m_nic.Speed.ToString();
      nicStatus.Text = m_nic.OperationalStatus.ToString();
      nicMAC.Text = m_nic.GetPhysicalAddress().ToString();
      currentIP.Text = m_nic.CurrentIpAddress.ToString();
      currentNetMask.Text = m_nic.CurrentSubnetMask.ToString();
      IPInterfaceProperties ipprops = m_nic.GetIPProperties();

      if(ipprops.GatewayAddresses.Count > 0)
      {
        currentGateway.Text = ipprops.GatewayAddresses[0].Address.ToString();
      }
      else
      {
        currentGateway.Text = string.Empty;
      }

      dhcpEnabled.Checked = m_nic.GetIPProperties().GetIPv4Properties().IsDhcpEnabled;

      if (dhcpEnabled.Checked)
      {
        dhcpRenew.Enabled = true;
        currentIP.ReadOnly = true;
        currentNetMask.ReadOnly = true;
        currentGateway.ReadOnly = true;
        currentIP.BackColor = SystemColors.ControlLight;
        currentNetMask.BackColor = SystemColors.ControlLight;
        currentGateway.BackColor = SystemColors.ControlLight;
      }
      else
      {
        dhcpRenew.Enabled = false;
        currentIP.ReadOnly = false;
        currentNetMask.ReadOnly = false;
        currentGateway.ReadOnly = false;
        currentIP.BackColor = SystemColors.Window;
        currentNetMask.BackColor = SystemColors.Window;
        currentGateway.BackColor = SystemColors.Window;
      }
    }

    public INetworkInterface NIC 
    {
      set
      {
        m_nic = value;
        RefreshProps();
      }
    }

    private void ok_Click(object sender, EventArgs e)
    {
      if (m_dirty)
      {
        // save changes
        try
        {
          m_nic.CurrentIpAddress = IPAddress.Parse(currentIP.Text);
          m_nic.CurrentSubnetMask = IPAddress.Parse(currentNetMask.Text);
          m_nic.GetIPProperties().GatewayAddresses[0].Address = IPAddress.Parse(currentGateway.Text);
          m_nic.Rebind();
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Exception while saving");
        }
      }

      this.Close();
    }

    private void refresh_Click(object sender, EventArgs e)
    {
      refresh.Enabled = false;
      RefreshProps();
      refresh.Enabled = true;
    }

  }
}