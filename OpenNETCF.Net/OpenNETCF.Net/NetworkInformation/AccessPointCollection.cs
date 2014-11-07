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
    public class AccessPointCollection : IEnumerable<IAccessPoint>
    {
        private WirelessZeroConfigNetworkInterface m_adapter = null;
        private List<IAccessPoint> m_aps = new List<IAccessPoint>();

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
                System.Threading.Thread.Sleep(20);
            }

            m_aps.Clear();

            if (m_adapter is WirelessZeroConfigNetworkInterface)
            {
                m_aps.AddRange(WZC.GetAPs(m_adapter.Name));
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

			// Dispose of INTF_ENTRY
			ie.Dispose();
        }

        /// <summary>
        /// Indexer for contained AccessPoints
        /// </summary>
        public AccessPoint this[int index]
        {
            get
            {
                return m_aps[index] as AccessPoint;
            }
        }

        /// <summary>
        /// Refresh the list of SSID values, asking the 
        /// adapter to scan for new ones, also.
        /// </summary>
        public void Refresh()
        {
            this.RefreshList(false);
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
                    return m_aps[i] as AccessPoint;
                }
            }

            return null;
        }

        public new IEnumerator<IAccessPoint> GetEnumerator()
        {
            return m_aps.GetEnumerator() as IEnumerator<IAccessPoint>;
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
            return m_aps.GetEnumerator() as IEnumerator<IAccessPoint>;
        }
    }
}
