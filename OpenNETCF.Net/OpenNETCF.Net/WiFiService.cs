using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Net.NetworkInformation;
using System.Threading;
using Microsoft.Win32;
using System.Diagnostics;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{
    public class SignalEventArgs : EventArgs
    {
        public StrengthType Strength { get; private set; }

        internal SignalEventArgs(StrengthType strength)
        {
            Strength = strength;
        }
    }

    public delegate void ScanCompleteDelegate(IAccessPoint[] newNetworks, IAccessPoint[] lostNetworks, IAccessPoint[] stillAvailableNetworks);

    public class WiFiService : IDisposable
    {
        public event EventHandler InitializationComplete;
        public event PropertyChangedEventHandler PropertyChanged;
        public event ScanCompleteDelegate ScanComplete;

        public event EventHandler WiFiEnabledChanged;
        public event EventHandler<SignalEventArgs> SignalChanged;

        private List<IAccessPoint> m_knownAPs = new List<IAccessPoint>();
        private const int SSIDScanPeriod = 5000;
        private int SignalScanPeriod = 10000;
        private int SignalScanStartupDelay = 20000;
        private object m_syncRoot = new object();
        private AutoResetEvent m_stopScanEvent = new AutoResetEvent(false);
        private WirelessZeroConfigNetworkInterface m_wzc;
        private Timer m_signalTimer;
        private StrengthType m_strength;
        private INetworkInterface[] m_nicList;
        private bool m_wifiSupported;
        private string m_candidateAdapterName;

        public int ScanPeriod { get; set; }
        public bool Scanning { get; private set; }
        public bool Initialized { get; set; }
        public bool IsDisposed { get; protected set; }

        public WiFiService(string adapterName)
        {
            // disable WZC popup
            try
            {
                using (var key = Registry.LocalMachine.CreateSubKey(@"\Drivers\BuiltIn\Ethman\Popup"))
                {
                    key.SetValue("Popup", 0, RegistryValueKind.DWord);
                    key.Flush();
                }
            }
            catch { }

            // start scanning early and often if debugging
            if (Debugger.IsAttached)
            {
                SignalScanPeriod = 5000;
                SignalScanStartupDelay = 1000;
            }
            ScanPeriod = SSIDScanPeriod;

            m_signalTimer = new Timer(SignalWatcherProc, null, Timeout.Infinite, Timeout.Infinite);

            m_wifiSupported = false;

            // Queue up the signal timer initialization.  Moved here to make sure the WifiSupported and
            // WifiEnabled flags have been set before the thread fires
            ThreadPool.QueueUserWorkItem(InitializeAsync);

            Debug.WriteLine("WiFiService created");
        }

        public void Dispose()
        {
            ReleaseManagedResources();
            ReleaseNativeResources();
            GC.SuppressFinalize(this);
            IsDisposed = true;
        }

        ~WiFiService()
        {
            ReleaseNativeResources();
        }

        protected void ReleaseNativeResources()
        {
            StopScanning();
            m_knownAPs.Clear();
        }

        protected void ReleaseManagedResources()
        {
            try
            {
                if (m_signalTimer != null)
                {
                    m_signalTimer.Dispose();
                }
            }
            catch (ObjectDisposedException) { }
        }
        
        public bool WiFiSupported
        {
            get
            {
                // once supported, always supported
                if (m_wifiSupported) return true;

                try
                {
                    // Does the OS report that a wireless adapter exists
                    m_wzc = GetFirstWZCInterface();

                    // Is there a NDIS power setting for a wireless adapter?
                    bool ndisExists = false;

                    if (m_candidateAdapterName != null)
                    {
                        ndisExists = DoesAdapterNdisPowerExist(m_candidateAdapterName);
                    }

                    // WifiSupported = true if there is an adapter reported back from the OS or
                    // WifiSupported = true if there is no adapter, but there is an NDIS power setting
                    m_wifiSupported = m_wzc != null || (m_wzc == null && !ndisExists);
                }
                catch
                {
                    m_wifiSupported = false;
                }

                return m_wifiSupported;
            }
        }

        private WirelessZeroConfigNetworkInterface GetFirstWZCInterface()
        {
            m_nicList = null;

            foreach (var intf in NetworkInterfaces)
            {
                var i = intf as WirelessZeroConfigNetworkInterface;
                if (i != null) return i;
            }

            return null;
        }

        private INetworkInterface[] NetworkInterfaces
        {
            get
            {
                // only query this once, as it's unlikely to change
                // TODO: register with NDIS for nic attach/detach and refresh then
                if (m_nicList == null)
                {
                    m_nicList = NetworkInterface.GetAllNetworkInterfaces();
                }
                return m_nicList;
            }
        }

        private bool DoesAdapterNdisPowerExist(string adapter)
        {
            bool isEnabled = false;
            try
            {
                using (var key = Registry.LocalMachine.CreateSubKey("Comm\\NdisPower"))
                {
                    var v = key.GetValue(adapter);
                    if (v == null)
                    {
                        isEnabled = true;
                    }
                    else if (((int)v) != 4)
                    {
                        isEnabled = true;
                    }
                }
            }
            catch (Exception)
            {
                isEnabled = false;
            }

            return isEnabled;
        }

        public bool WiFiEnabled
        {
            set
            {
                if (!WiFiSupported) return;

                if (m_candidateAdapterName != null)
                {
                    using (var power = new OpenNETCF.Net.NdisPower())
                    {
                        power.SetAdapterPower(m_candidateAdapterName, value);
                    }
                }

                if (value)
                {
                    m_wzc = GetFirstWZCInterface();

                    if (!Scanning) StartScanning();
                }
                else
                {
                    m_wzc = null;

                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        if (ScanComplete != null)
                            ScanComplete(new IAccessPoint[] { }, m_knownAPs.ToArray(), new IAccessPoint[] { });

                        m_knownAPs.Clear();
                    });

                    StopScanning();
                }

                if (WiFiEnabledChanged != null)
                {
                    WiFiEnabledChanged(this, EventArgs.Empty);
                }

            }
            get
            {
                try
                {
                    if (!WiFiSupported) return false;
                    bool enabled = false;
                    using (var key = Registry.LocalMachine.CreateSubKey("Comm\\NdisPower"))
                    {
                        var v = key.GetValue(m_candidateAdapterName);
                        if (v == null)
                        {
                            enabled = true;
                        }
                        else if (((int)v) != 4)
                        {
                            enabled = true;
                        }

                        return enabled;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        public string AdapterName
        {
            get { return m_wzc == null ? null : m_wzc.Name; }
        }

        public IAccessPoint[] KnownAPs
        {
            get { return m_knownAPs.ToArray(); }
        }

        public string AssociatedAP
        {
            get { return m_wzc == null ? null : m_wzc.AssociatedAccessPoint; }
        }

        public StrengthType CurrentSignalStrength
        {
            get { return m_strength; }
            private set
            {
                if (value == m_strength) return;
                m_strength = value;
                var handler = SignalChanged;
                if (handler != null)
                {
                    handler(this, new SignalEventArgs(m_strength));
                }
                RaisePropertyChanged("CurrentSignalStrength");
            }
        }

        private void SignalWatcherProc(object o)
        {
            try
            {
                if ((m_wzc == null) || (m_wzc.SignalStrength == null))
                {
                    CurrentSignalStrength = StrengthType.NoSignal;
                    Debug.WriteLine("No Signal");
                }
                else
                {
                    if (m_wzc.AssociatedAccessPoint == null)
                    {
                        Debug.WriteLine("No Signal (not connected)");
                    }
                    else
                    {
                        CurrentSignalStrength = m_wzc.SignalStrength.Strength;
                        Debug.WriteLine("Signal " + CurrentSignalStrength.ToString());
                    }
                }
            }
            catch
            {
                CurrentSignalStrength = StrengthType.NoSignal;
                Debug.WriteLine("No Signal (error)");
            }
        }

        private void InitializeAsync(object state)
        {
            // we want to store key material in the registry so it survives a restart
            using (var key = Registry.LocalMachine.OpenSubKey("\\init\\bootvars", true))
            {
                key.SetValue("MasterKeysInRegistry", 1, RegistryValueKind.DWord);
                key.Flush();
            }

            m_wzc = GetFirstWZCInterface();

            RaisePropertyChanged("AdapterName");

            Initialized = true;

            var handler = InitializationComplete;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(property));
        }

        public void StartScanning()
        {
            if (Scanning)
            {
                Debug.WriteLine("WiFiService: Already scanning");
                return;
            }
            if (!WiFiSupported)
            {
                Debug.WriteLine("WiFiService: Not Supported");
                return;
            }

            if (!WiFiEnabled)
            {
                Debug.WriteLine("WiFiService: Not Enabled");
                return;
            }

            m_signalTimer.Change(SignalScanStartupDelay, SignalScanPeriod);

            new Thread(ScanThreadProc) { IsBackground = true }
            .Start();
        }

        public void StopScanning()
        {
            if (!Scanning) return;

            try
            {
                m_signalTimer.Change(Timeout.Infinite, Timeout.Infinite);
                m_stopScanEvent.Set();
                m_knownAPs.Clear();
            }
            catch (ObjectDisposedException)
            {
                // ignore - we've likely been disposed
            }
        }

        public bool ConnectToOpenNetwork(string ssid)
        {
            return ConnectToOpenNetwork(ssid, false);
        }

        public bool ConnectToOpenNetwork(string ssid, bool adhoc)
        {
            if (!WiFiSupported) throw new NotSupportedException("No WZC WiFi Adapter detected on this device");

            return ConnectToNetwork(ssid, null, adhoc, AuthenticationMode.Open, WEPStatus.WEPDisabled);
        }

        public bool ConnectToOpenNetwork(IAccessPoint accessPoint, bool adhoc)
        {
            return ConnectToOpenNetwork(accessPoint.Name, adhoc);
        }

        public bool ConnectToOpenNetwork(IAccessPoint accessPoint)
        {
            if (!WiFiSupported) throw new NotSupportedException("No WZC WiFi Adapter detected on this device");

            return m_wzc.AddPreferredNetwork(accessPoint);
        }

        public bool ConnectToWEPNetwork(string ssid, string wepKey)
        {
            return ConnectToWEPNetwork(ssid, wepKey, false);
        }

        public bool ConnectToWEPNetwork(string ssid, string wepKey, bool adhoc)
        {
            if (!WiFiSupported) throw new NotSupportedException("No WZC WiFi Adapter detected on this device");

            return ConnectToNetwork(ssid, wepKey, adhoc, AuthenticationMode.Open, WEPStatus.WEPEnabled);
        }

        public bool ConnectToWEPNetwork(IAccessPoint accessPoint, string wepKey)
        {
            return ConnectToWEPNetwork(accessPoint, wepKey, false);
        }

        public bool ConnectToWEPNetwork(IAccessPoint accessPoint, string wepKey, bool adhoc)
        {
            if (!WiFiSupported) throw new NotSupportedException("No WZC WiFi Adapter detected on this device");

            return ConnectToWEPNetwork(accessPoint.Name, wepKey, adhoc);
        }

        /// <summary>
        /// This connects to a WPA network using TKIP encryption.
        /// </summary>
        /// <param name="ssid"></param>
        /// <param name="passphrase"></param>
        /// <returns></returns>
        public bool ConnectToWPANetwork(string ssid, string passphrase)
        {
            return ConnectToWPANetwork(ssid, passphrase, false);
        }

        public bool ConnectToWPANetwork(string ssid, string passphrase, bool adhoc)
        {
            if (!WiFiSupported) throw new NotSupportedException("No WZC WiFi Adapter detected on this device");

            return ConnectToNetwork(ssid, passphrase, adhoc, AuthenticationMode.WPAPSK, WEPStatus.TKIPEnabled);
        }

        /// <summary>
        /// This method will only connect to a WPA2 network using AES.
        /// If you need fallback to WPA (using TKIP), use the overload that takes in an AccessPoint
        /// </summary>
        /// <param name="ssid"></param>
        /// <param name="passphrase"></param>
        /// <returns></returns>
        public bool ConnectToWPA2Network(string ssid, string passphrase)
        {
            return ConnectToWPA2Network(ssid, passphrase, false);
        }

        public bool ConnectToWPA2Network(string ssid, string passphrase, bool adhoc)
        {
            if (!WiFiSupported) throw new NotSupportedException("No WZC WiFi Adapter detected on this device");

            return ConnectToNetwork(ssid, passphrase, adhoc, AuthenticationMode.WPA2PSK, WEPStatus.AESEnabled);
        }

        /// <summary>
        /// Connects to a WPA or WPA2 (AES or TKIP)
        /// </summary>
        /// <param name="accessPoint"></param>
        /// <param name="passphrase"></param>
        /// <returns></returns>
        public bool ConnectToWPANetwork(IAccessPoint accessPoint, string passphrase)
        {
            return ConnectToWPANetwork(accessPoint, passphrase, false);
        }

        public bool ConnectToWPANetwork(IAccessPoint accessPoint, string passphrase, bool adhoc)
        {
            if (!WiFiSupported) throw new NotSupportedException("No WZC WiFi Adapter detected on this device");

            // quick validation
            var valid = accessPoint.AuthenticationMode == AuthenticationMode.WPA
                || accessPoint.AuthenticationMode == AuthenticationMode.WPAPSK
                || accessPoint.AuthenticationMode == AuthenticationMode.WPA2
                || accessPoint.AuthenticationMode == AuthenticationMode.WPA2PSK;

            if (!valid)
            {
                throw new InvalidOperationException("The provided AP is not set up for WPA");
            }

            return ConnectToNetwork(accessPoint.Name, passphrase, adhoc, accessPoint.AuthenticationMode, accessPoint.Privacy);
        }

        private AccessPointCollection m_currentAPs;

        private void ScanThreadProc()
        {
            lock (m_syncRoot)
            {
                Debug.WriteLine("+WiFiService.ScanThreadProc");
                Scanning = true;

                do
                {
                    if (m_wzc == null) break;
                    if (!WiFiEnabled) break;

                    try
                    {
                        var et = Environment.TickCount;

                        if (m_currentAPs == null)
                        {
                            m_currentAPs = m_wzc.NearbyAccessPoints;
                        }
                        else
                        {
                            m_currentAPs.Refresh();
                        }

                        if (!WiFiEnabled) break;

                        var added = m_currentAPs.Except(m_knownAPs);
                        var lost = m_knownAPs.Except(m_currentAPs);
                        var updated = m_knownAPs.Intersect(m_currentAPs);

                        m_knownAPs = m_currentAPs.ToList();
                        RaisePropertyChanged("KnownAPs");

                        var handler = ScanComplete;

                        et = Environment.TickCount - et;
                        Debug.WriteLine(string.Format("Network scan took {0}ms", et));

                        if (!WiFiEnabled) break;

                        if (handler != null)
                        {
                            ThreadPool.QueueUserWorkItem(delegate
                            {
                                handler(added.ToArray(), lost.ToArray(), updated.ToArray());
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(string.Format("Exception in WiFi network scan.  If you disabled the adapter, you can ignore this error: ", ex.Message));

                        // if we disable the WiFi adapter while this loop is executing, bad things can happen
                        // it's better to just catch around the whole thing and ignore
                        if (!WiFiEnabled) break;
                    }
                } while (!m_stopScanEvent.WaitOne(ScanPeriod, false));

                Debug.WriteLine("-WiFiService.ScanThreadProc");
                Scanning = false;
                m_currentAPs = null;
            }
        }

        private bool SanityCheckPassphrase(string passphrase, AuthenticationMode mode, WEPStatus encryption)
        {
            if (encryption == WEPStatus.WEPEnabled)
            {
                // A WEP key can be 10, 26 or 32 hex digits OR 5, 13, or 16 ascii characters
                switch (passphrase.Length)
                {
                    // HEX
                    case 10:
                    case 26:
                    case 32:
                        try
                        {
                            for (int i = 0; i < passphrase.Length; i += 2)
                            {
                                var test = int.Parse(passphrase.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                            }
                        }
                        catch
                        {
                            return false;
                        }

                        return true;
                    // ASCII
                    case 5:
                    case 13:
                    case 16:
                        return true;
                    default:
                        return false;
                }
            }

            if ((encryption == WEPStatus.AESEnabled) || (encryption == WEPStatus.TKIPEnabled))
            {
                // The encryption key for WPA-PSK/TKIP must be between 8 and 63 characters
                if ((passphrase.Length < 8) || (passphrase.Length > 63))
                {
                    return false;
                }
            }

            return true;
        }

        public void ConnectToNetwork(IAccessPoint ap, string passcode)
        {
            if (ap.AuthenticationMode == AuthenticationMode.Open)
            {
                if (ap.Privacy == WEPStatus.WEPEnabled)
                {
                    ConnectToWEPNetwork(ap, passcode, ap.InfrastructureMode == InfrastructureMode.AdHoc);
                }
                else
                {
                    ConnectToOpenNetwork(ap, ap.InfrastructureMode == InfrastructureMode.AdHoc);
                }
            }
            else if ((ap.AuthenticationMode == AuthenticationMode.WPA) || (ap.AuthenticationMode == AuthenticationMode.WPAPSK))
            {
                ConnectToWPANetwork(ap, passcode);
            }
            else if ((ap.AuthenticationMode == AuthenticationMode.WPA2) || (ap.AuthenticationMode == AuthenticationMode.WPA2PSK))
            {
                ConnectToWPA2Network(ap.Name, passcode);
            }
            else if (ap.AuthenticationMode == AuthenticationMode.WPAAdHoc)
            {
                ConnectToWPA2Network(ap.Name, passcode, true);
            }
            else
            {
                Debug.WriteLine("!!Unsupported network type");
                if (Debugger.IsAttached) Debugger.Break();
            }
        }

        private bool ConnectToNetwork(string ssid, string passphrase, bool adhoc, AuthenticationMode mode, WEPStatus encryption)
        {
            Debug.WriteLine("+WiFiService.ConnectToNetwork");

            bool success = false;

            if (!SanityCheckPassphrase(passphrase, mode, encryption))
            {
                Debug.WriteLine("Invalid passphrase for requested WiFi network type");
                return success;
            }

            EAPParameters eap = null;

            switch (mode)
            {
                case AuthenticationMode.WPA:
                case AuthenticationMode.WPAPSK:
                case AuthenticationMode.WPA2:
                case AuthenticationMode.WPA2PSK:
                    eap = new EAPParameters()
                    {
                        Enable8021x = true,
                        EapType = EAPType.Default,
                        EapFlags = EAPFlags.Enabled,
                    };
                    break;
            }

            // stop scanning while connecting
            var wasScanning = Scanning;
            StopScanning();
            Thread.Sleep(ScanPeriod);
            int retries = 5;
            bool retry = false;

            try
            {
                do
                {
                    if (!Monitor.TryEnter(m_syncRoot))
                    {
                        Debug.WriteLine("Unable to get WiFiService SyncRoot: " + retries);
                        retry = (retries-- > 0);
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Debug.WriteLine("WiFiService.Calling AddPreferredNetwork");
                        try
                        {
                            if (!string.IsNullOrEmpty(m_wzc.AssociatedAccessPoint))
                            {
                                m_wzc.RemovePreferredNetwork(m_wzc.AssociatedAccessPoint);
                            }

                            success = m_wzc.AddPreferredNetwork(ssid,
                                !adhoc,
                                passphrase,
                                1,
                                mode,
                                encryption,
                                eap);

                            return success;
                        }
                        finally
                        {
                            Monitor.Exit(m_syncRoot);
                        }
                    }
                } while (retry);

                return success;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("WiFiService.ConnectToNetwork threw: " + ex.Message);
                return false;
            }
            finally
            {
                if (wasScanning) StartScanning();
                if (success)
                {
                    var handler = PropertyChanged;
                    if (handler != null)
                    {
                        handler(this, new PropertyChangedEventArgs("AssociatedAP"));
                    }
                }

                SaveRegsitry();

                Debug.WriteLine("-WiFiService.ConnectToNetwork");
            }
        }

        private void SaveRegsitry()
        {
            RegFlushKey(HKEY_LOCAL_MACHINE);
        }

        private const uint HKEY_LOCAL_MACHINE = 0x80000002;
        [DllImport("coredll.dll")]
        private static extern int RegFlushKey(uint hKey);

    }
}

