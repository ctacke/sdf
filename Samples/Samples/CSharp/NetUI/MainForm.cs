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

using OpenNETCF.Windows.Forms;
using OpenNETCF.Net;
using System.Reflection;
using System.Drawing.Imaging;

namespace NetUI
{
    public partial class MainForm : Form
    {
        private enum CurrentView
        {
            Adapters = 0,
            Networks
        }

        private CurrentView m_currentView = CurrentView.Adapters;
        private AdapterCollection m_adapters;
        private Adapter m_selectedAdapter = null;
        private AccessPointCollection m_lastAPList = null;
        private Timer m_refreshTimer = new Timer();

        public MainForm()
        {
            InitializeComponent();

            itemList.DrawItem += new DrawItemEventHandler(itemList_DrawItem);
            itemList.SelectedIndexChanged += new EventHandler(itemList_SelectedIndexChanged);
            itemList.MouseDown += new MouseEventHandler(itemList_MouseDown);
            itemList.MouseUp += new MouseEventHandler(itemList_MouseUp);
            itemList.LineColor = Color.Black;
            itemList.ItemHeight = 90;
            itemList.ShowLines = true;
            itemList.ContextMenu = adapterMenu;
            itemList.DrawMode = DrawMode.OwnerDrawFixed;

            m_refreshTimer.Interval = 3000;
            m_refreshTimer.Tick += new EventHandler(m_refreshTimer_Tick);
            m_refreshTimer.Enabled = true;

            wirelessNetworkMenu.Click += new EventHandler(wirelessNetworkMenu_Click);

            RefreshAdapterList();

            adaptersMenu.Click += new EventHandler(adaptersMenu_Click);

            releaseMenu.Click += new EventHandler(releaseMenu_Click);
            renewMenu.Click += new EventHandler(renewMenu_Click);
        }

        void itemList_MouseUp(object sender, MouseEventArgs e)
        {
            m_refreshTimer.Enabled = true;
        }

        void itemList_MouseDown(object sender, MouseEventArgs e)
        {
            m_refreshTimer.Enabled = false;
        }

        void m_refreshTimer_Tick(object sender, EventArgs e)
        {
            m_refreshTimer.Enabled = false;

            if (m_currentView == CurrentView.Adapters)
            {
                RefreshAdapterList();                
            }
            else
            {
                RefreshNetworkList();
            }

            m_refreshTimer.Enabled = true;
        }

        void renewMenu_Click(object sender, EventArgs e)
        {
            m_selectedAdapter.DhcpRenew();
            macLabel.Text = gatewayLabel.Text = dhcpLabel.Text = subnetLabel.Text = "";
            RefreshAdapterList();
        }

        void releaseMenu_Click(object sender, EventArgs e)
        {
            m_selectedAdapter.DhcpRelease();
            macLabel.Text = gatewayLabel.Text = dhcpLabel.Text = subnetLabel.Text = "";
            RefreshAdapterList();
        }

        void adaptersMenu_Click(object sender, EventArgs e)
        {
            titleLabel.Text = "Network Adapters";
            m_currentView = CurrentView.Adapters;
            RefreshAdapterList();
        }

        void itemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_currentView == CurrentView.Adapters)
            {
                if ((itemList.SelectedIndex < 0) || (itemList.SelectedIndex >= m_adapters.Count))
                {
                    macLabel.Text = gatewayLabel.Text = dhcpLabel.Text = subnetLabel.Text = "";
                    wirelessNetworkMenu.Enabled = false;
                }
                else
                {
                    m_selectedAdapter = m_adapters[itemList.SelectedIndex];

                    if (m_selectedAdapter.DhcpEnabled)
                    {
                        itemList.ContextMenu = adapterMenu;
                        dhcpLabel.Text = m_selectedAdapter.DhcpServer;
                    }
                    else
                    {
                        itemList.ContextMenu = null;
                        dhcpLabel.Text = "N/A";
                    }
                    gatewayLabel.Text = m_selectedAdapter.Gateway;
                    subnetLabel.Text = m_selectedAdapter.CurrentSubnetMask;
                    macLabel.Text = MACToString(m_selectedAdapter.MacAddress);

                    if (m_selectedAdapter.IsWireless)
                    {
                        wirelessNetworkMenu.Enabled = true;
                    }
                    else
                    {
                        wirelessNetworkMenu.Enabled = false;
                    }
                }
            }
            else
            {
                if (itemList.SelectedIndex < 0)
                {
                }
                else
                {
                }
            }
        }

        void wirelessNetworkMenu_Click(object sender, EventArgs e)
        {
            if(itemList.SelectedIndex >= 0)
            {
                titleLabel.Text = "Wireless Networks";

                m_currentView = CurrentView.Networks;

                RefreshNetworkList();
            }
        }

        Font m_itemTitleFont = new Font(FontFamily.GenericSansSerif, 10F, FontStyle.Bold);
        Brush m_itemTitleBrush = new SolidBrush(Color.Blue);
        Font m_itemDetailFont = new Font(FontFamily.GenericSansSerif, 8F, FontStyle.Regular);
        Font m_itemDetailBoldFont = new Font(FontFamily.GenericSansSerif, 8F, FontStyle.Bold);
        Brush m_itemDetailBrush = new SolidBrush(Color.Black);
        int m_titleHeight = 0;
        Pen m_blackPen = new Pen(Color.Black);

        void itemList_DrawItem(object sender, DrawItemEventArgs e)
        {
            StringBuilder sb = new StringBuilder(1024);

            // custom draw the item
            e.Graphics.Clip = new Region(e.Bounds);

            // draw a box around the item
            e.Graphics.DrawRectangle(m_blackPen, e.Bounds);

            // "select" the item if it should be
            if (itemList.SelectedIndex == e.Index)
            {
                e.DrawBackground(Color.Goldenrod);
            }
            else
            {
                e.DrawBackground(Color.White);
            }

            if (m_currentView == CurrentView.Adapters)
            {
                Adapter currentAdapter = m_adapters[e.Index];
                
                string properties1a = string.Format(
                    "Type: {0}  IP: ",
                    currentAdapter.Type.ToString());

                string properties1b = currentAdapter.CurrentIpAddress;

                if (currentAdapter.IsWireless)
                {
                    // TODO: Need to look at the Adapter src as this seems to change for the same adapter
//                    sb.Append(currentAdapter.IsWirelessZeroConfigCompatible ? "[WZC] " : "[Non-WZC] ");
                    if (currentAdapter.AssociatedAccessPoint != null)
                    {
                        sb.Append(string.Format("Connected to {0} ({1}db)", currentAdapter.AssociatedAccessPoint, currentAdapter.SignalStrengthInDecibels));
                    }

                }
                else
                {
                    sb.Append("[Not wireless]");
                }

                string properties2 = sb.ToString();
                
                //Draw the data
                if (e.Index <= itemList.Items.Count - 1)
                {
                    if (m_titleHeight == 0)
                    {
                        m_titleHeight = ((int)(e.Graphics.MeasureString(currentAdapter.Name, m_itemTitleFont).Height));
                    }

                    e.Graphics.DrawString(currentAdapter.Name, m_itemTitleFont, m_itemTitleBrush, 2, e.Bounds.Top);
                    e.Graphics.DrawString(properties1a, m_itemDetailFont, m_itemDetailBrush, 2, e.Bounds.Top + m_titleHeight);
                    e.Graphics.DrawString(properties1b, m_itemDetailBoldFont, m_itemDetailBrush, 2 + e.Graphics.MeasureString(properties1a, m_itemDetailFont).Width, e.Bounds.Top + m_titleHeight);
                    e.Graphics.DrawString(properties2, m_itemDetailFont, m_itemDetailBrush, 2, e.Bounds.Top + (2 * m_titleHeight));
                }
            }
            else if (m_currentView == CurrentView.Networks)
            {
                AccessPoint ap = m_lastAPList[e.Index];

                if (m_titleHeight == 0)
                {
                    m_titleHeight = ((int)(e.Graphics.MeasureString(ap.Name, m_itemTitleFont).Height));
                }

                string properties1 = MACToString(ap.MacAddress);

                // Begin bitmap section
                int bars = 0;
                if (ap.SignalStrengthInDecibels < -80)
                    bars = 5;
                else if (ap.SignalStrengthInDecibels < -70)
                    bars = 4;
                else if (ap.SignalStrengthInDecibels < -60)
                    bars = 3;
                else if (ap.SignalStrengthInDecibels < -50)
                    bars = 2;
                else
                    bars = 1;

                string bmpName = string.Format("NetUI.Resource.{0}Bars{1}.bmp", bars, ap.Privacy != 0 ? "L" : "");

                Bitmap strength = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(bmpName));

                // TODO: cache these instead of calculating every time
                int bmpWidth = strength.Width;
                ImageAttributes transparentAttributes = new ImageAttributes();
                Color transparentColor = strength.GetPixel(0, 0);
                transparentAttributes.SetColorKey(transparentColor, transparentColor);

                Rectangle destinationRectangle = new Rectangle(e.Bounds.Left, e.Bounds.Top, strength.Width, strength.Height);
                e.Graphics.DrawImage(strength, destinationRectangle, 0, 0, strength.Width, strength.Height, GraphicsUnit.Pixel, transparentAttributes);

                // End bitmap section
                string properties2 = string.Format(
                    "{0} mode - Privacy {1}",
                    ap.InfrastructureMode.ToString(),
                    ap.Privacy != 0 ? "Enabled" : "Disabled");

                if (e.Index <= itemList.Items.Count - 1)
                {
                    e.Graphics.DrawString(string.Format("{0} ({1}dB)", ap.Name, ap.SignalStrengthInDecibels), m_itemTitleFont, m_itemTitleBrush, bmpWidth, e.Bounds.Top);
                    e.Graphics.DrawString(properties1, m_itemDetailFont, m_itemDetailBrush, bmpWidth, e.Bounds.Top + m_titleHeight);
                    e.Graphics.DrawString(properties2, m_itemDetailFont, m_itemDetailBrush, bmpWidth, e.Bounds.Top + (2 * m_titleHeight));
                }
            }

            e.Graphics.ResetClip();
        }

        private string MACToString(byte[] mac)
        {
            StringBuilder sb = new StringBuilder(100);

            for (int b = 0; b < mac.Length; b++)
            {
                if (b < mac.Length - 1)
                {
                    sb.Append(string.Format("{0:X2}:", mac[b]));
                }
                else
                {
                    sb.Append(string.Format("{0:X2}", mac[b]));
                }
            }
            return sb.ToString();
        }

        private void RefreshNetworkList()
        {
            if (propertiesFrame.Visible)
            {
                propertiesFrame.Visible = false;
                itemList.Height = this.Height - 3;
            }

            itemList.ContextMenu = null;
            itemList.BeginUpdate();
            itemList.Items.Clear();

            m_lastAPList = m_selectedAdapter.NearbyAccessPoints;

            foreach (AccessPoint ap in m_lastAPList)
            {
                itemList.Items.Add(new ListItem());
            }

            itemList.EndUpdate();
        }

        private void RefreshAdapterList()
        {
            itemList.ContextMenu = adapterMenu;
            if (!propertiesFrame.Visible)
            {
                propertiesFrame.Visible = true;
                itemList.Height = this.Height - propertiesFrame.Height - 3;
            }

            Adapter lastSelected = null;
            int index = 0;
            int select = -1;

            if ((itemList.SelectedIndex >= 0) && (itemList.SelectedIndex < m_adapters.Count))
            {
                lastSelected = m_adapters[itemList.SelectedIndex];
            }
            else
            {
                macLabel.Text = gatewayLabel.Text = dhcpLabel.Text = subnetLabel.Text = "";
                wirelessNetworkMenu.Enabled = false;
            }

            itemList.BeginUpdate();

            m_adapters = Networking.GetAdapters();

            itemList.Items.Clear();

            foreach(Adapter adapter in m_adapters)
            {
                itemList.Items.Add(new ListItem());

                if ((lastSelected != null) && (MACToString(lastSelected.MacAddress) == MACToString(adapter.MacAddress)))
                {
                    select = index;
                }
                index++;
            }

            itemList.SelectedIndex = select;

            itemList.EndUpdate();
        }
    }
}