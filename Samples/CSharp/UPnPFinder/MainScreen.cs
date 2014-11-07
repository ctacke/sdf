//----------------------------------------------------------------------------
//  This file is part of the OpenNETCF Smart Device Framework Code Samples.
// 
//  Copyright (C) OpenNETCF Consulting, LLC.  All rights reserved.
// 
//  This source code is intended only as a supplement to Smart Device 
//  Framework and/or on-line documentation.  
// 
//  THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
//----------------------------------------------------------------------------

using System;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace UPnPFinder
{
    public partial class MainScreen : Form
    {
        ImageList il;
        public AutoResetEvent _evtDeviceFound;
        public AutoResetEvent _evtStopSeacrh;
        static private Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");
        static private Guid CLSID_UPnPFinder = new Guid("E2085F28-FEB7-404A-B8E7-E659BDEAAA02");
        int _asyncFind;
        IUPnPDeviceFinder _finder;
        IUPnPDeviceFinderCallback _finderCallback;
        GCHandle _hCallback;
        bool fClosing = false;
        
        public MainScreen()
        {
            InitializeComponent();
            il = new ImageList();
            il.ImageSize = new Size(64, 64);
            lvDevices.LargeImageList = il;
            _evtDeviceFound = new AutoResetEvent(false);
            _evtStopSeacrh = new AutoResetEvent(false);
        }

        #region UI Event handlers
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Initiate search. It will go on until the form is closed
            SearchForDevices();
            return;
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            //fClosing = true;
            //(_finderCallback as Callback).StopEvents();
            //_evtStopSeacrh.WaitOne(-1, false);
        }

        private void lvDevices_ItemActivate(object sender, EventArgs e)
        {
            // Browse the selected device
            IUPnPDevice device = (IUPnPDevice)lvDevices.Items[lvDevices.SelectedIndices[0]].Tag;
            BrowseDevice(device);
        }

        #endregion

        /// <summary>
        /// Helper method to get the bitmap from a given url
        /// </summary>
        /// <param name="iconUrl">Bitmap url</param>
        /// <returns>Bitmap</returns>
        private Image GetIcon(string iconUrl)
        {
            HttpWebRequest rq = HttpWebRequest.Create(iconUrl) as HttpWebRequest;
            HttpWebResponse rsp = rq.GetResponse() as HttpWebResponse;
            try
            {
                return new Bitmap(rsp.GetResponseStream());
            }
            finally
            {
                rsp.Close();
            }
        }

        // Checks if device supports Content Directory service and opens a browser on it
        private void BrowseDevice(IUPnPDevice device)
        {
            // Get service enumerator
            IntPtr pE = device.Services._NewEnum;
            IEnumUnknown pEnum = (IEnumUnknown)Marshal.GetObjectForIUnknown(pE);
            Marshal.Release(pE);
            
            // Enumerate services. Look for one that has ContentDirectory in its description
            uint cItems;
            IUPnPService svc = null;
            IUPnPService svcContent = null;
            bool couldBrowse = false;
            while (pEnum.Next(1, out pE, out cItems) == 0 && cItems == 1)
            {
                svc = (IUPnPService)Marshal.GetObjectForIUnknown(pE);
                Marshal.Release(pE);

                object results;// = new object[3];
                object retVal;
                if (svc.ServiceTypeIdentifier.IndexOf("ContentDirectory", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Check if the service supports Browse verb
                    // This is not really neccessary, but just a precaution
                    svcContent = svc;
                    int hr = svc.InvokeAction("Browse", new object[] { "0", "BrowseDirectChildren", "*", 0, 0, "" }, out results, out retVal);
                    if (hr == 0)
                        couldBrowse = true;
                    hr = 0;
                }
            }

            if (!couldBrowse || svcContent == null)
                MessageBox.Show("Found no browsable content");
            else
            {
                // We have found the Content Directory
                Browser browser = new Browser(svcContent);
                browser.ShowDialog();
            }
        }

        // Initiate UPNP discovery
        void SearchForDevices()
        {
            IntPtr pCB;
            // Create an instance of a UPNPFinder
            Type tFinder = Type.GetTypeFromCLSID(CLSID_UPnPFinder);
            object oInst = Activator.CreateInstance(tFinder);
            _finder = (IUPnPDeviceFinder)oInst;

            // Create a callback object
            _finderCallback = new Callback(this);
            pCB = Marshal.GetComInterfaceForObject(_finderCallback, typeof(IUPnPDeviceFinderCallback));
            
            // Pin the callback to ensure it does not get garbage-collected
            _hCallback = GCHandle.Alloc(_finderCallback, GCHandleType.Pinned);
            
            // Start search
            _asyncFind = _finder.CreateAsyncFind("upnp:rootdevice", 0, pCB);
            Debug.WriteLine("Starting search");
            _finder.StartAsyncFind(_asyncFind);
        }


        // Handles the device discovery event
        public void DeviceFound(object sender, IntPtr pDev)
        {
            // Marshal interface to the current thread
            IUPnPDevice device = (IUPnPDevice)Marshal.GetObjectForIUnknown(pDev);
            Debug.WriteLine("Found device: " + device.FriendlyName);
            
            // Check if the device already in the list
            // Devices are compared by UniqueDeviceName
            foreach (ListViewItem item in lvDevices.Items)
                if ((item.Tag as IUPnPDevice).UniqueDeviceName == device.UniqueDeviceName)
                {
                    Debug.WriteLine("Already known");
                    return;
                }

            // Get device icon
            // For simplicity we always ask for a 64x64 icon
            string url;
            Debug.WriteLine("Getting icon");
            int ret = device.IconURL("image/png", 64, 64, 16, out url);
            Image imgIcon = new Bitmap(64, 64);
            if (ret == 0 && url != null)
                imgIcon = GetIcon(url);
            Debug.WriteLine("Adding");
            il.Images.Add(imgIcon);
            ListViewItem newItem = new ListViewItem(device.FriendlyName);
            newItem.Tag = device;
            newItem.ImageIndex = il.Images.Count - 1;
            lvDevices.Items.Add(newItem);
            _evtDeviceFound.Set();
        }

        public void DeviceGone(object sender, string device)
        {
            foreach (ListViewItem item in lvDevices.Items)
                if ((item.Tag as IUPnPDevice).UniqueDeviceName == device)
                {
                    lvDevices.Items.Remove(item);
                    return;
                }
        }

        public void SearchComplete(object sender, EventArgs e)
        {
            if (!fClosing)
                // Just repeat search
                SearchForDevices();
        }

    }

    delegate void DeviceFoundHandler(object sender, IntPtr pDev);
    delegate void DeviceGoneHandler(object sender, string device);

    /// <summary>
    /// An implementation of a IUPnPDeviceFinderCallback interface
    /// Used to receive UPNP search events
    /// </summary>
    [ComVisible(true)]
    [ComDefaultInterface(typeof(IUPnPDeviceFinderCallback))]
    public class Callback : IUPnPDeviceFinderCallback
    {
        MainScreen m_frm;
        bool fStopEvents;
        public Callback(MainScreen frm)
        {
            m_frm = frm;
            fStopEvents = false;
        }

        public void StopEvents()
        {
            fStopEvents = true;
        }

        #region IUPnPDeviceFinderCallback Members

        public IntPtr DeviceAdded(int lFindData, IUPnPDevice pDevice)
        {
            try
            {
                Debug.WriteLine("DeviceAdded");
                if (!fStopEvents)
                {
                    m_frm._evtDeviceFound.Reset();
                    m_frm.Invoke(new DeviceFoundHandler(m_frm.DeviceFound), new object[] { this, Marshal.GetIUnknownForObject(pDevice) });
                    m_frm._evtDeviceFound.WaitOne();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return IntPtr.Zero;
        }

        public IntPtr DeviceRemoved(int lFindData, string bstrUDN)
        {
            try
            {
                Debug.WriteLine("DeviceRemoved");
                if (!fStopEvents)
                    m_frm.Invoke(new DeviceGoneHandler(m_frm.DeviceGone), this, bstrUDN);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return IntPtr.Zero;
        }

        public IntPtr SearchComplete(int lFindData)
        {
            try
            {
                Debug.WriteLine("Done");
                m_frm._evtStopSeacrh.Set();
                if (!fStopEvents)
                    m_frm.Invoke(new EventHandler(m_frm.SearchComplete));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return IntPtr.Zero;
        }

        #endregion
    }



}