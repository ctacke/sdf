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
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Class that represents a collection of the SSID values
    /// that a given network adapter can hear over the 
    /// airwaves.  For each SSID, you can get the signal
    /// strength and random other information.
    /// </summary>
    public class AccessPointCollection : IEnumerable<AccessPoint>
    {
        private WirelessZeroConfigNetworkInterface m_adapter = null;
        private List<AccessPoint> m_aps = new List<AccessPoint>();

        /// <summary>
        /// The Adapter instance with which the SSID instance
        /// is associated.
        /// </summary>
        public WirelessZeroConfigNetworkInterface AssociatedAdapter
        {
            get { return m_adapter; }
        }

        internal AccessPointCollection(WirelessZeroConfigNetworkInterface intf, bool nearbyOnly)
        {
            if (intf == null) throw new ArgumentNullException();

            m_adapter = intf;

            this.RefreshListPreferred(nearbyOnly);
        }

        internal AccessPointCollection(WirelessZeroConfigNetworkInterface a)
        {
            if (a == null) throw new ArgumentNullException();

            m_adapter = a;

            this.RefreshList(true);
        }

        internal unsafe void ClearCache()
        {
            string name = m_adapter.Name;
            if (!NDISUIO.SetOID(NDIS_OID.BSSID_LIST_SCAN, name))
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Unable to clear adapter's NDIS cache");
            }
        }

        private void GetNDISAPs()
        {
            // Retrieve a list of NDIS_802_11_BSSID_LIST 
            // structures from the driver.  We'll parse that
            // list and populate ourselves based on the data
            // that we find there.
            string name = m_adapter.Name;
            byte[] data = NDISUIO.QueryOID(NDIS_OID.BSSID_LIST, name);
            if (data != null)
            {
                // Figure out how many SSIDs there are.
                NDIS_802_11_BSSID_LIST rawlist = new NDIS_802_11_BSSID_LIST(data, false);

                for (int i = 0; i < rawlist.NumberOfItems; i++)
                {
                    // Get the next raw item from the list.
                    BSSID bssid = rawlist.Item(i);

                    // Using the raw item, create a cooked 
                    // SSID item.
                    AccessPoint ssid = new AccessPoint(bssid);

                    // Add the new item to this.
                    m_aps.Add(ssid);
                }
            }
            else
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error(), "Unable to get BSSID List");
            }
        }

        private void GetWZCAPs()
        {
            INTF_ENTRY entry = new INTF_ENTRY();
            entry.Guid = m_adapter.Name;
            INTFFlags flags = 0;
            AuthenticationMode mode;

            int result = WZCPInvokes.WZCQueryInterface(null, INTFFlags.INTF_ALL, ref entry, out flags);

            if (result != 0)
            {
                throw new Exception("WZCQueryInterface failed for " + m_adapter.Name);
            }

            try
            {
                // Figure out how many SSIDs there are.
                if (entry.rdBSSIDList.cbData == 0)
                {
                    // list is empty
                    return;
                }

                NDIS_802_11_BSSID_LIST rawlist = new NDIS_802_11_BSSID_LIST(entry.rdBSSIDList.lpData, true);

                for (int i = 0; i < rawlist.NumberOfItems; i++)
                {
                    // Get the next raw item from the list.
                    BSSID bssid = rawlist.Item(i);

                    // Using the raw item, create a cooked 
                    // SSID item.
                    AccessPoint ssid = new AccessPoint(bssid);

                    // Add the new item to this.
                    m_aps.Add(ssid);
                }
            }
            finally
            {
                WZCPInvokes.WZCDeleteIntfObj(ref entry);
            }
        }

        internal unsafe void RefreshList(Boolean clearCache)
        {
            // If we are to clear the driver's cache of SSID
            // values, call the appropriate method.
            //Console.WriteLine("Entering RefreshList");
            if (clearCache)
            {
                this.ClearCache();

                // This seems to be needed to avoid having
                // a list of zero elements returned.
                System.Threading.Thread.Sleep(1000);
            }

            m_aps.Clear();

            if (m_adapter is WirelessZeroConfigNetworkInterface)
            {
                GetWZCAPs();
            }
            else
            {
                GetNDISAPs();
            }

        }

        internal unsafe void RefreshListPreferred(bool nearbyOnly)
        {
            // If the caller wants only the local preferred APs,
            // we check nearby list and, if the AP is not there,
            // we don't add it to our own preferred list.
            AccessPointCollection apc = null;
            if (nearbyOnly)
            {
                apc = m_adapter.NearbyAccessPoints;
            }

            // First step is to get the INTF_ENTRY for the adapter.
            // This includes the list of preferred SSID values.
            INTF_ENTRY ie = INTF_ENTRY.GetEntry(this.m_adapter.Name);

            // The field rdStSSIDList is the preferred list.  It comes
            // in the form of a WZC_802_11_CONFIG_LIST.
            RAW_DATA rd = ie.rdStSSIDList;
            WLANConfigurationList cl = new WLANConfigurationList(rd);

            // Step through the list and add a new AP to the
            // collection for each entry.
            for (int i = 0; i < cl.NumberOfItems; i++)
            {
                WLANConfiguration c = cl.Item(i);

                //Debug.WriteLine(c.SSID);
                //for (int d = 1; d <= c.Data.Length; d++)
                //{
                //    Debug.Write(string.Format("{0:x2}{1}", c.Data[d - 1], (d%8 == 0) ? "\r\n" : " "));
                //}
                //Debug.WriteLine(string.Empty);

                // If we're only showing those which we can hear,
                // see if the current SSID is in the nearby list.
                if (nearbyOnly)
                {
                    // Find the currently active AP with the SSID
                    // to match the one we're working on.
                    AccessPoint activeAP = apc.FindBySSID(c.SSID);
                    int ss;

                    // If the given SSID is not in range, don't add
                    // an entry to the list.
                    if (activeAP != null)
                    {
                        // Update signal strength.
                        ss = activeAP.SignalStrengthInDecibels;

                        // Copy the signal strength value to the 
                        // NDIS_WLAN_BSSID structure for the 
                        // preferred list entry.
                        c.Rssi = ss;

                        // Create the AP instance and add it to the
                        // preferred list.
                        AccessPoint ap = new AccessPoint(c);
                        m_aps.Add(ap);
                    }
                }
                else
                {
                    // Create the AP instance and add it to the
                    // preferred list.  The signal strength will 
                    // not necessarily be valid.
                    AccessPoint ap = new AccessPoint(c);
                    m_aps.Add(ap);
                }
            }
        }

        /// <summary>
        /// Indexer for contained AccessPoints
        /// </summary>
        public AccessPoint this[int index]
        {
            get
            {
                return m_aps[index]; ;
            }
        }

        /// <summary>
        /// Refresh the list of SSID values, asking the 
        /// adapter to scan for new ones, also.
        /// </summary>
        public void Refresh()
        {
            this.RefreshList(true);
        }

        /// <summary>
        /// Find a given access point in the collection by
        /// looking for a matching SSID value.
        /// </summary>
        /// <param name="ssid">
        /// String SSID to search for.
        /// </param>
        /// <returns>
        /// First AccessPoint in the collection with the 
        /// indicated SSID, or null, if none was found.
        /// </returns>
        public AccessPoint FindBySSID(String ssid)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (m_aps[i].Name == ssid)
                {
                    return m_aps[i];
                }
            }

            return null;
        }

        public new IEnumerator<AccessPoint> GetEnumerator()
        {
            return m_aps.GetEnumerator() as IEnumerator<AccessPoint>;
        }

        public bool Contains(AccessPoint accessPoint)
        {
            if (accessPoint == null) throw new ArgumentNullException();

            return m_aps.Contains(accessPoint);
        }

        public int Count
        {
            get { return m_aps.Count; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_aps.GetEnumerator() as IEnumerator<AccessPoint>;
        }
    }
}
