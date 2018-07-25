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