using System;
using System.Threading;
using OpenNETCF.WindowsCE;
using OpenNETCF.Diagnostics;

namespace OpenNETCF.Net.NetworkInformation
{
    public delegate void InterfaceEventDelegate(NetworkInterface networkInterface, object oldValue, object newValue);

    [Flags]
    public enum InterfaceWatchField
    {
        Speed = 0x01,
        AdminStatus = 0x02,
        OperationalStatus = 0x04,
        ReceivedOctets = 0x08,
        ReceivedUnicastPackets = 0x10,
        ReceivedNonUnicastPackets = 0x20,
        ReceivedDiscardedPackets = 0x40,
        ReceivedErrorPackets = 0x80,
        SentOctets = 0x08,
        SentUnicastPackets = 0x10,
        SentNonUnicastPackets = 0x20,
        SentDiscardedPackets = 0x40,
        SentErrorPackets = 0x80,

    }

    public class NetworkInterfaceWatcher
    {
        private struct WatchFields
        {
            public int speed;
            public OperationalStatus operationalStatus;
            public InterfaceOperationalStatus interfaceOperationalStatus;
            public uint receivedOctets;
            public int receivedUnicastPackets;
            public int receivedNonUnicastPackets;
            public int receivedDiscardedPackets;
            public int receivedErrorPackets;
            public uint sentOctets;
            public int sentUnicastPackets;
            public int sentNonUnicastPackets;
            public int sentDiscardedPackets;
            public int sentErrorPackets;
        }

        public event InterfaceEventDelegate SpeedChange;
        public event InterfaceEventDelegate OperationalStatusChange;
        public event InterfaceEventDelegate InterfaceOperationalStatusChange;
        public event InterfaceEventDelegate ReceivedOctetsChange;
        public event InterfaceEventDelegate ReceivedUnicastPacketsChange;
        public event InterfaceEventDelegate ReceivedNonUnicastPacketsChange;
        public event InterfaceEventDelegate ReceivedErrorChange;
        public event InterfaceEventDelegate ReceivedDiscardedChange;
        public event InterfaceEventDelegate SentOctetsChange;
        public event InterfaceEventDelegate SentUnicastPacketsChange;
        public event InterfaceEventDelegate SentNonUnicastPacketsChange;
        public event InterfaceEventDelegate SentErrorChange;
        public event InterfaceEventDelegate SentDiscardedChange;

        private Thread m_monitorThread;
        private NetworkInterface m_interface;
        private int m_interval = 5000;
        private WatchFields m_previousState;
        private bool m_stopThread = false;

        public NetworkInterfaceWatcher(NetworkInterface networkInterface) : this(networkInterface, 5000) {}

#if DEBUG
        private TraceSwitch m_log;
        private ManualResetEvent m_debugwait = new ManualResetEvent(false);

        internal NetworkInterfaceWatcher(NetworkInterface networkInterface, int refreshInterval, TraceSwitch debugLog)
            : this( networkInterface, refreshInterval)
        {
            m_log = debugLog;
            m_debugwait.Set();
            
        }
#endif
        public NetworkInterfaceWatcher(NetworkInterface networkInterface, int refreshInterval)
        {
            m_interface = networkInterface;
            m_interface.IsBeingWatched = true;
            m_interval = refreshInterval;
            m_monitorThread = new Thread(new ThreadStart(MonitorThreadProc));
            m_monitorThread.IsBackground = true;
            m_monitorThread.Start();
        }

        /*
        int lastPowerUp = 0;
        ManualResetEvent m_threadStopped = new ManualResetEvent(false);

        void PowerManagement_PowerUp()
        {
            m_stopThread = true;

            // wait for existing thread to complete
            m_threadStopped.WaitOne();
            m_threadStopped.Reset();

            int now = Environment.TickCount;

            if ((now - lastPowerUp) < 500)
            {
                return;
            }

            // refresh the interface list
            NetworkInterfaceCollection c = new NetworkInterfaceCollection();
            c.Refresh();

            foreach (NetworkInterface ni in c)
            {
                if (ni.PhysicalAddressString.CompareTo(m_interface.PhysicalAddressString) == 0)
                {
                    if (IndexChange != null)
                    {
                        IndexChange(m_interface, m_interface.Index, ni.Index);
                    }

                    m_interface = ni;

                    m_monitorThread = new Thread(new ThreadStart(MonitorThreadProc));
                    m_monitorThread.IsBackground = true;
                    m_monitorThread.Start();
                    return;
                }
            }

            if (InterfaceLost != null)
            {
                InterfaceLost(m_interface, null, null);
            }
        }
*/
        private void MonitorThreadProc()
        {
#if DEBUG
            // make sure the trace listener is set before continuing
            m_debugwait.WaitOne();
            Trace2.WriteLineIf(m_log.TraceInfo, "+NetworkInterfaceWatcher.MonitorThreadProc()");
            Trace2.Indent();
#endif
            m_previousState.operationalStatus = m_interface.OperationalStatus;
            m_previousState.interfaceOperationalStatus = m_interface.InterfaceOperationalStatus;
            m_previousState.receivedDiscardedPackets = m_interface.DiscardedIncomingPackets;
            m_previousState.receivedErrorPackets = m_interface.ErrorIncomingPackets;
            m_previousState.receivedNonUnicastPackets = m_interface.NonUnicastPacketsReceived;
            m_previousState.receivedOctets = m_interface.OctetsReceived;
            m_previousState.receivedUnicastPackets = m_interface.UnicastPacketsReceived;
            m_previousState.sentDiscardedPackets = m_interface.DiscardedOutgoingPackets;
            m_previousState.sentErrorPackets = m_interface.ErrorOutgoingPackets;
            m_previousState.sentNonUnicastPackets = m_interface.NonUnicastPacketsSent;
            m_previousState.sentOctets = m_interface.OctetsSent;
            m_previousState.sentUnicastPackets = m_interface.UnicastPacketsSent;
            m_previousState.speed = m_interface.Speed;

            m_stopThread = false;

            while (!m_stopThread)
            {
#if DEBUG
                Trace2.WriteIf(m_log.TraceInfo, string.Format("Refreshing interface {0} with GetIfEntry...", m_interface.Index));
#endif
                if (NativeMethods.GetIfEntry(m_interface) == NativeMethods.NO_ERROR)
                {
#if DEBUG
                    Trace2.WriteLineIf(m_log.TraceInfo, "ok");
#endif
                    if ((SpeedChange != null) && (m_previousState.speed != m_interface.Speed))
                    {
#if DEBUG
                        Trace2.WriteLineIf(m_log.TraceInfo, string.Format("Interface {0} speed changed", m_interface.Index));
#endif
                        SpeedChange(m_interface, m_previousState.speed, m_interface.Speed);
                        m_previousState.speed = m_interface.Speed;
                    }

                    if ((OperationalStatusChange != null) && (m_previousState.operationalStatus != m_interface.OperationalStatus))
                    {
#if DEBUG
                        Trace2.WriteLineIf(m_log.TraceInfo, string.Format("Interface {0} AdministratorStatus changed", m_interface.Index));
#endif
                        OperationalStatusChange(m_interface, m_previousState.operationalStatus, m_interface.OperationalStatus);
                        m_previousState.operationalStatus = m_interface.OperationalStatus;
                    }

                    if ((InterfaceOperationalStatusChange != null) && (m_previousState.interfaceOperationalStatus != m_interface.InterfaceOperationalStatus))
                    {
#if DEBUG
                        Trace2.WriteLineIf(m_log.TraceInfo, string.Format("Interface {0} OperationalStatus changed", m_interface.Index));
#endif
                        InterfaceOperationalStatusChange(m_interface, m_previousState.interfaceOperationalStatus, m_interface.OperationalStatus);
                        m_previousState.interfaceOperationalStatus = m_interface.InterfaceOperationalStatus;
                    }

                    if ((ReceivedDiscardedChange != null) && (m_previousState.receivedDiscardedPackets != m_interface.DiscardedIncomingPackets))
                    {
#if DEBUG
                        Trace2.WriteLineIf(m_log.TraceInfo, string.Format("Interface {0} DiscardedIncomingPackets changed", m_interface.Index));
#endif
                        ReceivedDiscardedChange(m_interface, m_previousState.receivedDiscardedPackets, m_interface.DiscardedIncomingPackets);
                        m_previousState.receivedDiscardedPackets = m_interface.DiscardedIncomingPackets;
                    }

                    if ((ReceivedErrorChange != null) && (m_previousState.receivedErrorPackets != m_interface.ErrorIncomingPackets))
                    {
#if DEBUG
                        Trace2.WriteLineIf(m_log.TraceInfo, string.Format("Interface {0} ErrorIncomingPackets changed", m_interface.Index));
#endif
                        ReceivedErrorChange(m_interface, m_previousState.receivedErrorPackets, m_interface.ErrorIncomingPackets);
                        m_previousState.receivedErrorPackets = m_interface.ErrorIncomingPackets;
                    }

                    if ((ReceivedNonUnicastPacketsChange != null) && (m_previousState.receivedNonUnicastPackets != m_interface.NonUnicastPacketsReceived))
                    {
#if DEBUG
                        Trace2.WriteLineIf(m_log.TraceInfo, string.Format("Interface {0} NonUnicastPacketsReceived changed", m_interface.Index));
#endif
                        ReceivedNonUnicastPacketsChange(m_interface, m_previousState.receivedNonUnicastPackets, m_interface.NonUnicastPacketsReceived);
                        m_previousState.receivedNonUnicastPackets = m_interface.NonUnicastPacketsReceived;
                    }

                    if ((ReceivedOctetsChange != null) && (m_previousState.receivedOctets != m_interface.OctetsReceived))
                    {
#if DEBUG
                        Trace2.WriteLineIf(m_log.TraceInfo, string.Format("Interface {0} OctetsReceived changed", m_interface.Index));
#endif
                        ReceivedOctetsChange(m_interface, m_previousState.receivedOctets, m_interface.OctetsReceived);
                        m_previousState.receivedOctets = m_interface.OctetsReceived;
                    }

                    if ((ReceivedUnicastPacketsChange != null) && (m_previousState.receivedUnicastPackets != m_interface.UnicastPacketsReceived))
                    {
#if DEBUG
                        Trace2.WriteLineIf(m_log.TraceInfo, string.Format("Interface {0} UnicastPacketsReceived changed", m_interface.Index));
#endif
                        ReceivedUnicastPacketsChange(m_interface, m_previousState.receivedUnicastPackets, m_interface.UnicastPacketsReceived);
                        m_previousState.receivedUnicastPackets = m_interface.UnicastPacketsReceived;
                    }

                    if ((SentDiscardedChange != null) && (m_previousState.sentDiscardedPackets != m_interface.DiscardedOutgoingPackets))
                    {
#if DEBUG
                        Trace2.WriteLineIf(m_log.TraceInfo, string.Format("Interface {0} DiscardedOutgoingPackets changed", m_interface.Index));
#endif
                        SentDiscardedChange(m_interface, m_previousState.sentDiscardedPackets, m_interface.DiscardedOutgoingPackets);
                        m_previousState.sentDiscardedPackets = m_interface.DiscardedOutgoingPackets;
                    }

                    if ((SentErrorChange != null) && (m_previousState.sentErrorPackets != m_interface.ErrorOutgoingPackets))
                    {
#if DEBUG
                        Trace2.WriteLineIf(m_log.TraceInfo, string.Format("Interface {0} ErrorOutgoingPackets changed", m_interface.Index));
#endif
                        SentErrorChange(m_interface, m_previousState.sentErrorPackets, m_interface.ErrorOutgoingPackets);
                        m_previousState.sentErrorPackets = m_interface.ErrorOutgoingPackets;
                    }

                    if ((SentNonUnicastPacketsChange != null) && (m_previousState.sentNonUnicastPackets != m_interface.NonUnicastPacketsSent))
                    {
#if DEBUG
                        Trace2.WriteLineIf(m_log.TraceInfo, string.Format("Interface {0} OctetsSent changed", m_interface.Index));
#endif
                        SentNonUnicastPacketsChange(m_interface, m_previousState.sentNonUnicastPackets, m_interface.NonUnicastPacketsSent);
                        m_previousState.sentNonUnicastPackets = m_interface.NonUnicastPacketsSent;
                    }

                    if ((SentOctetsChange != null) && (m_previousState.sentOctets != m_interface.OctetsSent))
                    {
#if DEBUG
                        Trace2.WriteLineIf(m_log.TraceInfo, string.Format("Interface {0} OctetsSent changed", m_interface.Index));
#endif
                        SentOctetsChange(m_interface, m_previousState.sentOctets, m_interface.OctetsSent);
                        m_previousState.sentOctets = m_interface.OctetsSent;
                    }

                    if ((SentUnicastPacketsChange != null) && (m_previousState.sentUnicastPackets != m_interface.UnicastPacketsSent))
                    {
#if DEBUG
                        Trace2.WriteLineIf(m_log.TraceInfo, string.Format("Interface {0} UnicastPacketsSent changed", m_interface.Index));
#endif
                        SentUnicastPacketsChange(m_interface, m_previousState.sentUnicastPackets, m_interface.UnicastPacketsSent);
                        m_previousState.sentUnicastPackets = m_interface.UnicastPacketsSent;
                    }
                }
                else
                {
                    // interface is no longer valid
#if DEBUG
                    Trace2.WriteLineIf(m_log.TraceInfo, string.Format("GetIfEntry failed! LastError = {0}", System.Runtime.InteropServices.Marshal.GetLastWin32Error()));
#endif
                    return;
                }

                Thread.Sleep(m_interval);
            }

#if DEBUG
            Trace2.Unindent();
            Trace2.WriteLineIf(m_log.TraceInfo, "-NetworkInterfaceWatcher.MonitorThreadProc()");
#endif
        }

    }
}
