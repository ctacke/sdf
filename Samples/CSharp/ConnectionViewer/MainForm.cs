using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using OpenNETCF.Net;

namespace ConnectionViewer
{
    public partial class MainForm : Form
    {
        private ConnectionManager m_manager;
        private DestinationInfoCollection m_destinations;
        private ConnectionDetailCollection m_detailCollection;

        public MainForm()
        {
            InitializeComponent();

            try
            {
                m_manager = new OpenNETCF.Net.ConnectionManager();
            }
            catch (PlatformNotSupportedException)
            {
            }

            if (m_manager.SupportsStatusNotifications)
            {
                m_detailCollection = m_manager.GetConnectionDetailItems();
                m_detailCollection.ConnectionDetailItemsChanged += new ConnectionStateChangedHandler(m_detailCollection_ConnectionDetailItemsChanged);
            }
            else
            {
                // TODO: notify user / maybe spawn a watcher thread?
                m_manager.ConnectionStateChanged += new ConnectionStateChangedHandler(m_manager_ConnectionStateChanged);
            }

            RefreshDestinations();
        }

        void m_detailCollection_ConnectionDetailItemsChanged(object source, ConnectionStatus newStatus)
        {
            // TODO: Add something here
        }

        void m_manager_ConnectionStateChanged(object source, ConnectionStatus newStatus)
        {
            // TODO: Add something here
        }

        private void RefreshDestinations()
        {
            m_destinations = m_manager.EnumDestinations();


            destinationList.Items.Clear();
            destinationList.BeginUpdate();

            foreach (DestinationInfo dest in m_destinations)
            {
                ListViewItem lvi = new ListViewItem(new string[] { 
                    dest.Description,
                    "<unknown>",
                    dest.Guid.ToString()
                    } );

                lvi.Tag = dest;

                destinationList.Items.Add(lvi);
            }

            destinationList.EndUpdate();
        }
    }
}