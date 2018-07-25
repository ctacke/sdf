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