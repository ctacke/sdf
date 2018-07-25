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
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Encapsulates a non-WZC wireless network interface
    /// </summary>
    public class WirelessNetworkInterface : NetworkInterface, IWirelessNetworkInterface
    {
        internal WirelessNetworkInterface(int index, string name)
            : base(index, name)
        {
        }

        /// <summary>
        /// Attempts to connect to any available preferred access point
        /// </summary>
        public virtual void Connect()
        {
            Connect("");
        }

        /// <summary>
        /// Attempts to connect to a specific Access Point
        /// </summary>
        /// <param name="SSID">The Service Set Identified or the Access Point to which a connection should be made</param>
        public virtual void Connect(string SSID)
        {
            if(SSID.Length > 32)
            {
                throw new ArgumentException("SSID Max Length is 32 characters");
            }

            if (!NDISUIO.SetOID(NDIS_OID.SSID, this.Name, Encoding.ASCII.GetBytes(SSID)))
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Returns the currently-attached SSID for RF
        /// Ethernet adapters.
        /// </summary>
        /// <returns>
        /// Instance of SSID class (or null if not associated).
        /// </returns>
        public unsafe virtual string AssociatedAccessPoint
        {
            get
            {
                string ssid = "";

                byte[] data = null;
                try
                {
                    data = NDISUIO.QueryOID(NDIS_OID.SSID, this.Name);
                }
                catch
                {
                    // no associated AP
                    return null;
                }

                int len = BitConverter.ToInt32(data, 0);
                if (len > 0)
                {
                    ssid = System.Text.Encoding.ASCII.GetString(data, 4, len);
                }

                return ssid;

            }
        }

        /// <summary>
        /// Returns the strength of the RF Ethernet signal
        /// being received by the adapter, in dB.
        /// </summary>
        /// <returns>
        /// integer strength in dB; zero, if adapter is not
        /// an RF adapter or an error occurred
        /// </returns>
        private unsafe int SignalStrengthInDecibels
        {
            get
            {
                int db = 0;

                if (System.Environment.OSVersion.Version.Major < 4)
                {
                    throw new NotSupportedException("CE 3.0 and earlier not supported");
                }

                byte[] data = NDISUIO.QueryOID(NDIS_OID.RSSI, this.Name);
                if (data.Length > 0)
                {
                    db = BitConverter.ToInt32(data, 0);
                }

                return db;
            }
        }

        /// <summary>
        /// Returns a SignalStrength class representing the current strength
        /// of the signal.
        /// </summary>
        /// <returns>
        ///	SignalStrength
        /// </returns>
        public virtual SignalStrength SignalStrength
        {
            get
            {
                return (new SignalStrength(this.SignalStrengthInDecibels));
            }
        }

        /// <summary>
        /// Sets or gets how this adapter connects to the network. Setting this will also reset 
        /// the network association algorithm.
        /// </summary>
        public virtual InfrastructureMode InfrastructureMode
        {
            get
            {
                byte[] data = NDISUIO.QueryOID(NDIS_OID.INFRASTRUCTURE_MODE, this.Name);
                return (InfrastructureMode)BitConverter.ToInt32(data, 0);
            }
            set
            {
                if (!NDISUIO.SetOID(NDIS_OID.INFRASTRUCTURE_MODE, this.Name, BitConverter.GetBytes((uint)value)))
                {
                    throw new NotSupportedException();
                }
            }
        }

        /// <summary>
        /// Sets the IEEE 802.11 authentication mode.
        /// </summary>
        public virtual AuthenticationMode AuthenticationMode
        {
            get
            {
                byte[] data = NDISUIO.QueryOID(NDIS_OID.AUTHENTICATION_MODE, this.Name);
                return (AuthenticationMode)BitConverter.ToInt32(data, 0);
            }
            set
            {
                if (!NDISUIO.SetOID(NDIS_OID.AUTHENTICATION_MODE, this.Name, BitConverter.GetBytes((uint)value)))
                {
                    throw new NotSupportedException();
                }
            }
        }

        /// <summary>
        /// Gets the MAC address of the currently associated Access point
        /// </summary>
        public PhysicalAddress AssociatedAccessPointMAC
        {
            get
            {
                byte[] data = null;

                try
                {
                    data = NDISUIO.QueryOID(NDIS_OID.BSSID, this.Name);
                }
                catch
                {
                    // this happens if there is no associated AP
                    return PhysicalAddress.None;
                }

                if(data == null)
                    return PhysicalAddress.None;

                if (data.Length > 6)
                {
                    byte[] shortenedData = new byte[6];
                    Array.Copy(data, shortenedData, 6);
                    return new PhysicalAddress(shortenedData);
                }

                return new PhysicalAddress(data);
            }
        }

        /// <summary>
        /// Gets the currently connected network type or sets the network type the driver should use on the next connection
        /// </summary>
        public NetworkType NetworkType
        {
            get
            {
                byte[] data = NDISUIO.QueryOID(NDIS_OID.NETWORK_TYPE_IN_USE, this.Name);
                if (data == null)
                {
                    throw new NotSupportedException();
                }
                return (NetworkType)BitConverter.ToInt32(data, 0);
            }
            set
            {
                if (!NDISUIO.SetOID(NDIS_OID.NETWORK_TYPE_IN_USE, this.Name, BitConverter.GetBytes((uint)value)))
                {
                    throw new NotSupportedException();
                }
            }
        }

        /// <summary>
        /// Gets an array defining the data rates, in kilobits per second (kbps), that the radio is capable of running at
        /// </summary>
        public int[] SupportedRates
        {
            get
            {
                byte[] data = NDISUIO.QueryOID(NDIS_OID.SUPPORTED_RATES, this.Name);

                if (data == null)
                {
                    throw new NotSupportedException();
                }

                ArrayList list = new ArrayList(data.Length);

                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] > 0)
                    {
                        list.Add(data[i] * 500);
                    }
                }
                return (int[])list.ToArray(typeof(int)); ;
            }
        }

        /// <summary>
        /// Gets the configuration information for the radio
        /// </summary>
        public RadioConfiguration RadioConfiguration
        {
            get
            {
                byte[] data = NDISUIO.QueryOID(NDIS_OID.CONFIGURATION, this.Name);

                if (data == null)
                {
                    throw new NotSupportedException();
                }

                RadioConfiguration config = new RadioConfiguration(data, 0);

                return config;
            }
        }

        /// <summary>
        /// Gets the interface's WEP status
        /// </summary>
        public virtual WEPStatus WEPStatus
        {
            get
            {
                byte[] data = NDISUIO.QueryOID(NDIS_OID.WEP_STATUS, this.Name);
                if (data == null)
                {
                    throw new NotSupportedException();
                }
                return (WEPStatus)BitConverter.ToInt32(data, 0);
            }
        }
    }
}
