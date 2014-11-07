using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Net.NetworkInformation;
using OpenNETCF.Windows.Forms;

namespace WiFinder
{
  public partial class APView : Form, IAPView
  {
    public IAPPresenter Presenter { get; set; }

    public APView()
    {
      InitializeComponent();

      interfaceName.Text = "{No Interface}";

      nearbyAPList.DrawMode = DrawMode.OwnerDrawFixed;
      nearbyAPList.DrawItem += new DrawItemEventHandler(nearbyAPList_DrawItem);
      nearbyAPList.MouseDown += new MouseEventHandler(nearbyAPList_MouseDown);
      nearbyAPList.MouseUp += new MouseEventHandler(nearbyAPList_MouseUp);
      
//      apContextMenu.Popup += new EventHandler(apContextMenu_Popup);
      apConnectMenuItem.Click += new EventHandler(apConnectMenuItem_Click);
    }

    void apConnectMenuItem_Click(object sender, EventArgs e)
    {
      AccessPoint ap = m_selectedItem.Tag as AccessPoint;

      WirelessZeroConfigNetworkInterface wzc = Presenter.WirelessInterface as WirelessZeroConfigNetworkInterface;

      if (apConnectMenuItem.Text == "Connect")
      {
        // is it in our preferred list?  If so, add it        
        if ((wzc == null) || (Presenter.IsPreferredAP(ap)))
        {
          Presenter.WirelessInterface.Connect(ap.Name);
        }
        else
        {
          // is it an open network?
          if (ap.Privacy == 0)
          {
            wzc.AddPreferredNetwork(ap.Name, true, (byte[])null, 0, AuthenticationMode.Open, WEPStatus.WEPDisabled, null);
          }
          else
          {
            // TODO:
            MessageBox.Show("Need to get WEP key from user", "Not Implemented");
          }
        }
      }
      else
      {
      }
    }

    void apContextMenu_Popup(object sender, EventArgs e)
    {
      AccessPoint ap = m_selectedItem.Tag as AccessPoint;

      if (m_attachedAPItem == m_selectedItem)
      {
        apConnectMenuItem.Text = "Disconnect";
      }
      else
      {
        apConnectMenuItem.Text = "Connect";
      }
    }

    void nearbyAPList_MouseUp(object sender, MouseEventArgs e)
    {
      // re-start the state machine
      Presenter.APStateMachineEnabled = true;
    }

    private ListItem m_selectedItem = null;
    void nearbyAPList_MouseDown(object sender, MouseEventArgs e)
    {
      // stop the state machine so the list doesn't move around
      Presenter.APStateMachineEnabled = false;

      // determine the "selected" item
      int index = nearbyAPList.TopIndex + (e.Y / nearbyAPList.ItemHeight);
      if(index > (nearbyAPList.Items.Count - 1))
      {
        m_selectedItem = null;
        this.ContextMenu = null;
      }
      else
      {
        m_selectedItem = nearbyAPList.Items[index];
        this.ContextMenu = apContextMenu;

        // we can connect to an AP if we're WZC or if it's not privacy enabled (I think on #2 - not certain and no HW to test)
        apConnectMenuItem.Enabled = (
          (Presenter.WirelessInterface is WirelessZeroConfigNetworkInterface)
          || ((m_selectedItem.Tag as AccessPoint).Privacy == 0));

        this.ContextMenu.Show(nearbyAPList, new Point(e.X, e.Y));
      }
    }

    public void OnAPInfoChange(AccessPoint ap)
    {
      // find the AP in the list
      bool found = false;

      foreach (ListItem item in nearbyAPList.Items)
      {
        if (ap.Equals(item.Tag as AccessPoint))
        {
          item.Tag = ap;
          found = true;
          break;
        }
      }

      // if it's not it the list, it should be
      if (!found)
      {
        AddNearbyAP(ap);
      }

      nearbyAPList.Refresh();
    }

    public void OnInterfaceGained()
    {
      interfaceName.Text = Presenter.WirelessInterface.Name;
    }

    public void OnInterfaceLost()
    {
      interfaceName.Text = "{No Interface}";
    }

    public void OnInterfaceStatusChange(InterfaceStatus newStatus)
    {

    }

    private ListItem m_attachedAPItem;

    public void OnAttachedAPChange(string apName, string apMAC)
    {
      if (string.IsNullOrEmpty(apName))
      {
        m_attachedAPItem = null;
        return;
      }

      bool needsAdd = (m_attachedAPItem == null);

      m_attachedAPItem = new ListItem(apName);
      m_attachedAPItem.Tag = apMAC;

      if (needsAdd)
      {
        nearbyAPList.Items.Add(m_attachedAPItem);
      }
      else
      {
        nearbyAPList.Items[0] = m_attachedAPItem;
      }
    }

    public void RemoveNearbyAP(AccessPoint ap)
    {
      // TODO:
      for(int i = 0 ; i < nearbyAPList.Items.Count ; i++)
      {
        AccessPoint itemAP = nearbyAPList.Items[i].Tag as AccessPoint;
        if ((itemAP != null) && (itemAP.Equals(ap)))
        {
          nearbyAPList.Items.RemoveAt(i);
        }
      }
    }

    public void AddNearbyAP(AccessPoint ap)
    {
      // only add if it's *not* the attached AP.  If it's the attached, just set the tag
      if ((m_attachedAPItem != null) && 
        (m_attachedAPItem.Text == ap.Name) && 
        (ap.PhysicalAddress.ToString() == m_attachedAPItem.Tag.ToString()))
      {
        m_attachedAPItem.Tag = ap;
      }
      else
      {
        ListItem item = new ListItem(ap.Name);
        item.Tag = ap;
        nearbyAPList.Items.Add(item);
      }
    }

    private void interfaceInfoBtn_Click(object sender, EventArgs e)
    {
      MessageBox.Show("Show interface info");
    }

  }
}