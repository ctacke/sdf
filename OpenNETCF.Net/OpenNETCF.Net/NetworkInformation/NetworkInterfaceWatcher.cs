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
using System.Threading;
using System.Collections;
using OpenNETCF.WindowsCE.Messaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Delegate for the loss or gain of a NetworkInterface
    /// </summary>
    /// <param name="interfaceName">The name of the interface lost or gained</param>
    public delegate void InterfaceEventDelegate(string interfaceName);
    /// <summary>
    /// Delegate for a status change in a NetworkInterface
    /// </summary>
    /// <param name="interfaceName">The name of the interface lost or gained</param>
    /// <param name="newStatus">The new status of the interface</param>
    public delegate void InterfaceStatusDelegate(string interfaceName, InterfaceStatus newStatus);

    /// <summary>
    /// This class alows an application to monitor the status of all NetworkInterfaces on the local device
    /// </summary>
    public class NetworkInterfaceWatcher : IDisposable
    {
        /// <summary>
        /// Fired when a NetworkInterface is removed from the system
        /// </summary>
        /// <remarks>
        /// This event is interface index based. Some interfaces (e.g. PPP) get a new index when 
        /// reconnected so this event may fire when a cable connection state changes or when the 
        /// device sleeps or wakes.
        /// </remarks>
        public event InterfaceEventDelegate InterfaceLost;
        /// <summary>
        /// Fired when a NetworkInterface is added to the system
        /// </summary>
        /// <remarks>
        /// This event is interface index based. Some interfaces (e.g. PPP) get a new index when 
        /// reconnected so this event may fire when a cable connection state changes or when the 
        /// device sleeps or wakes.
        /// </remarks>
        public event InterfaceEventDelegate InterfaceGained;
        /// <summary>
        /// Fired when the status of a NetworkInterface changes
        /// </summary>
        public event InterfaceStatusDelegate InterfaceStatusChange;

        private static int REFRESH_PERIOD = 2500;
        private bool m_stopThread = false;
        private Hashtable m_knownInterfaces;
        private Thread m_monitorThread;
        private P2PMessageQueue m_ndisQueue;

        private delegate void StringDelegate(string text);
        private Control m_invoker = new Control();

        private InterfaceEventDelegate m_gainedMarshaler;
        private InterfaceEventDelegate m_lostMarshaler;

        /// <summary>
        /// Creates an instance of a NetworkInterfaceWatcher
        /// </summary>
        public NetworkInterfaceWatcher()
        {
            m_gainedMarshaler = new InterfaceEventDelegate(OnInterfaceGained);
            m_lostMarshaler = new InterfaceEventDelegate(OnInterfaceLost);

            m_knownInterfaces = GetInterfaceList();
            m_monitorThread = new Thread(new ThreadStart(InterfaceGainLossThreadProc));
            m_monitorThread.IsBackground = true;
            m_monitorThread.Start();

            SetupQueueListener();
        }

        /// <summary>
        /// Finalizes a NetworkInterfaceWatcher
        /// </summary>
        ~NetworkInterfaceWatcher()
        {
            Dispose();
        }

        /// <summary>
        /// Disposed a NetworkInterfaceWatcher
        /// </summary>
        public void Dispose()
        {
            TeardownQueueListener();
            m_stopThread = true;
        }

        private void InterfaceGainLossThreadProc()
        {
            Hashtable updatedList;
            while (!m_stopThread)
            {
                Thread.Sleep(REFRESH_PERIOD);

                updatedList = GetInterfaceList();

                // see if anything was lost
                if (InterfaceLost != null)
                {
                    foreach(DictionaryEntry item in m_knownInterfaces)
                    {                      
                        if (updatedList[item.Key] == null)
                        {
                            m_invoker.Invoke(m_lostMarshaler, new object[] { (string)item.Key });
                        }
                    }
                }

                // see if anything was added
                if (InterfaceGained != null)
                {
                    foreach (DictionaryEntry item in updatedList)
                    {
                        if (m_knownInterfaces[item.Key] == null)
                        {
                            m_invoker.Invoke(m_gainedMarshaler, new object[] { (string)item.Key });
                        }
                    }
                }

                m_knownInterfaces = updatedList;
            }
        }

        private void OnInterfaceGained(string name)
        {
            InterfaceGained(name);
        }

        private void OnInterfaceLost(string name)
        {
            InterfaceLost(name);
        }

        private unsafe Hashtable GetInterfaceList()
        {
            Hashtable list = null;
            uint size;

            // get buffer size requirement
            NativeMethods.GetInterfaceInfo(null, out size);

            byte[] ifTable = new byte[size];

            // pin the table buffer
            fixed (byte* pifTable = ifTable)
            {
                byte* p = pifTable;

                // get the table data
                NativeMethods.GetInterfaceInfo(pifTable, out size);

                // get interface count
                int interfaceCount = *p;
                list = new Hashtable(interfaceCount);
                p += 4;

                // get each interface
                for (int i = 0; i < interfaceCount; i++)
                {
                    // get interface index
                    int index = (int)*((int*)p);
                    p += 4;

                    // get interface name
                    byte[] nameBytes = new byte[256];
                    Marshal.Copy(new IntPtr(p), nameBytes, 0, nameBytes.Length);
                    string name = Encoding.Unicode.GetString(nameBytes, 0, nameBytes.Length);
                    int nullIndex = name.IndexOf('\0');
                    if (nullIndex > 0)
                    {
                        name = name.Substring(0, nullIndex);
                    }
                    p += 256;

                    list.Add(name, index);
                }

                return list;
            }
        }

        private void SetupQueueListener()
        {
            if (m_ndisQueue != null)
            {
                m_ndisQueue = new P2PMessageQueue(true, null, NDISUIO_DEVICE_NOTIFICATION.Size, 0);
                m_ndisQueue.DataOnQueueChanged += new EventHandler(DataOnQueueChanged);

                NDISUIO.RequestNotifications(m_ndisQueue);
            }
        }

        private void TeardownQueueListener()
        {
            if (m_ndisQueue != null)
            {
                NDISUIO.CancelNotifications();
                m_ndisQueue.Close();
                m_ndisQueue = null;
            }
        }

        void DataOnQueueChanged(object sender, EventArgs e)
        {
            while (m_ndisQueue != null && m_ndisQueue.MessagesInQueueNow > 0)
            {
                // Each notification will be of this type.
                NDISUIO_DEVICE_NOTIFICATION notification = new NDISUIO_DEVICE_NOTIFICATION();

                // Read the event data.
                if (m_ndisQueue.Receive(notification, -1) == ReadWriteResult.OK)
                {
                    if (InterfaceStatusChange != null)
                    {
                        InterfaceStatusChange(notification.ptcDeviceName, (InterfaceStatus)(notification.dwNotificationType & 0x3F));
                    }
                }
                else
                {
                    TeardownQueueListener();
                    throw new System.IO.IOException("Fatal error receiving data from the NDIS notification queue");
                }
            }
        }
    }
}
