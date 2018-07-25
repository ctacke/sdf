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
using System.Text;
using OpenNETCF.Runtime.InteropServices;
using System.Runtime.InteropServices;

namespace OpenNETCF.Net
{
    //NEW to 3.5
    /// <summary>
    /// This class contains detailed information about the status of available connections.
    /// </summary>
    /// <remarks>
    /// When ConnectionDetail contains information about the status of a Wi-Fi connection, the Description field contains the name of the service set identifier (SSID) used to connect to the network.
    /// </remarks>
    public class ConnectionDetail
    {

        private int m_version;
        private DetailStatusParam m_params;
        private ConnectionType m_connectionType;
        private ConnectionSubType m_connSubType;
        private ConnectionOption m_connOption;
        private bool m_secure;
        private Guid m_destinationNetwork;
        private Guid m_SourceNetwork;
        private string m_description;
        private string m_name;
        private ConnectionStatus m_connStatus;
        private DateTime m_lastConnTime;
        private int m_signalQuality;
        private IntPtr m_pIPAddress;


        internal ConnectionDetail(CONNMGR_CONNECTION_DETAILED_STATUS item)
        {
            this.Version = item.dwVer;
            this.AdapterName = item.szAdapterName;
            this.ConnectionOption = item.dwFlags;
            this.ConnectionStatus = item.dwConnectionStatus;
            this.ConnectionSubType = GetConnSubtype(item) ;
            this.ConnectionType = item.dwType;
            this.Description = item.szDescription;
            this.DestinatonNetwork = item.guidDestNet;
            this.DetailStatusParam = item.dwParams;
            this.IPAddress = item.pIPAddr;
            SYSTEMTIME st = new SYSTEMTIME();
            st.wYear = item.LastConnectTime[0];
            st.wMonth = item.LastConnectTime[1];
            st.wDay = item.LastConnectTime[3];
            st.wHour = item.LastConnectTime[4];
            st.wMinute = item.LastConnectTime[5];
            st.wSecond = item.LastConnectTime[6];
            this.LastConnectionTime = st.ToDateTime();
                    
        }

        private ConnectionSubType GetConnSubtype(CONNMGR_CONNECTION_DETAILED_STATUS item)
        {
            ConnectionType conType = item.dwType;
            int subType = (int)item.dwSubtype;
                    #region Case Statement to deterimne subconnection type
                    switch (conType)
                    {
                        case ConnectionType.Bluetooth:
                            switch (subType)
                            {
                                case 1:
                                    return ConnectionSubType.RAS;
                                case 2:
                                    return ConnectionSubType.PAN;
                                default:
                                    return ConnectionSubType.Unknown;
                            }
                        case ConnectionType.Cellular:
                            switch (subType)
                            {
                                case 1:
                                    return ConnectionSubType.CSD;
                                case 2:
                                    return ConnectionSubType.GPRS;
                                case 3:
                                    return ConnectionSubType.XRTT;
                                case 4:
                                    return ConnectionSubType.EVDO;
                                case 5:
                                    return ConnectionSubType.XEVDV;
                                case 6:
                                    return ConnectionSubType.EDGE;
                                case 7:
                                    return ConnectionSubType.UMTS;
                                case 8:
                                    return ConnectionSubType.Voice;
                                case 9:
                                    return ConnectionSubType.PTT;
                                case 10:
                                    return ConnectionSubType.HSDPA;
                                default:
                                    return ConnectionSubType.Unknown;
                            }
                        case ConnectionType.NIC:
                            switch (subType)
                            {
                                case 1:
                                    return ConnectionSubType.Ethernet;
                                case 2:
                                    return ConnectionSubType.WiFi;
                                default:
                                    return ConnectionSubType.Unknown;
                            }
                        case ConnectionType.PC:
                            switch (subType)
                            {
                                case 1:
                                    return ConnectionSubType.DesktopPassthrough;
                                case 2:
                                    return ConnectionSubType.IR;
                                case 3:
                                    return ConnectionSubType.ModemLink;
                                default:
                                    return ConnectionSubType.Unknown;
                            }
                        case ConnectionType.Proxy:
                            switch (subType)
                            {
                                case 1:
                                    return ConnectionSubType.NullProxy;
                                case 2:
                                    return ConnectionSubType.HTTP;
                                case 3:
                                    return ConnectionSubType.WAP;
                                case 4:
                                    return ConnectionSubType.Sockets4;
                                case 5:
                                    return ConnectionSubType.Sockets5;
                                default:
                                    return ConnectionSubType.Unknown;
                            }
                        case ConnectionType.Unimodem:
                            switch (subType)
                            {
                                case 1:
                                    return ConnectionSubType.CSD;
                                case 2:
                                    return ConnectionSubType.OutOfBandCSD;
                                case 3:
                                    return ConnectionSubType.NullModem;
                                case 4:
                                    return ConnectionSubType.ExternalModem;
                                case 5:
                                    return ConnectionSubType.InternalModem;
                                case 6:
                                    return ConnectionSubType.PCMCIAModem;
                                case 7:
                                    return ConnectionSubType.IRCommModem;
                                case 8:
                                    return ConnectionSubType.DynamicModem;
                                case 9:
                                    return ConnectionSubType.DynamicPort;
                                default:
                                    return ConnectionSubType.Unknown;
                            }
                        case ConnectionType.VPN:
                            switch (subType)
                            {
                                case 1:
                                    return ConnectionSubType.L2TP;
                                case 2:
                                    return ConnectionSubType.PPTP;
                                default:
                                    return ConnectionSubType.Unknown;
                            }
                        default:
                            return ConnectionSubType.Unknown;


                    }
                    #endregion
        }


        /// <summary>
        /// Version of the structure. The current version is set to CONNMGRDETAILEDSTATUS_VERSION. 
        /// </summary>
        private int Version
        {
            get { return m_version; }
            set {m_version = value;}
        }

        /// <summary>
        /// Combination of Connection Manager detailed status parameter constants. See DetailStatusParam
        /// </summary>
        public DetailStatusParam DetailStatusParam
        {
            get { return m_params; }
            internal set { m_params = value; }
        }

        /// <summary>
        /// Available IP addresses, set to NULL, if no IP address is available. 
        /// </summary>
        private IntPtr IPAddress
        {
            get { return m_pIPAddress; }
            set { m_pIPAddress = value; }
        }
	
        /// <summary>
        /// Quality of the signal, can be a value between 0 and 255. 
        /// </summary>
        public int SignalQuality
        {
            get { return m_signalQuality; }
            internal set { m_signalQuality = value; }
        }
	
        /// <summary>
        /// Time the connection was last established. 
        /// </summary>
        public DateTime LastConnectionTime
        {
            get { return m_lastConnTime; }
            internal set { m_lastConnTime = value; }
        }

        /// <summary>
        /// One of the Connection Manager status constants. For possible values, see ConnectionStatus
        /// </summary>
        public ConnectionStatus ConnectionStatus
        {
            get { return m_connStatus; }
            internal set { m_connStatus = value; }
        }
	
        /// <summary>
        /// Null-terminated name of the adapter. If no adapter name is available, this parameter must be set to NULL. 
        /// </summary>
        public string AdapterName
        {
            get { return m_name; }
            internal set { m_name = value; }
        }
	
        /// <summary>
        /// Name of the connection specified in a null-terminated string. If no name is available, this parameter must be set to NULL. 
        /// </summary>
        public string Description
        {
            get { return m_description; }
            internal set { m_description = value; }
        }

	    /// <summary>
        /// GUID of the source network. 
	    /// </summary>
        public Guid SourceNetwork
        {
            get { return m_SourceNetwork; }
            internal set { m_SourceNetwork = value; }
        }
	
        /// <summary>
        /// GUID of the destination network. 
        /// </summary>
        public Guid DestinatonNetwork
        {
            get { return m_destinationNetwork; }
            internal set { m_destinationNetwork = value; }
        }
	
        /// <summary>
        /// The security level of the connection. The security level corresponds with the connection type. If the security level is greater than zero, the connection is secure.  
        /// </summary>
        public bool Secure
        {
            get { return m_secure; }
            internal set { m_secure = value; }
        }
	
        /// <summary>
        /// One or more connection options. For possible values, see ConnectionOption
        /// </summary>
        public ConnectionOption ConnectionOption
        {
            get { return m_connOption; }
            internal set { m_connOption = value; }
        }

        /// <summary>
        /// One of the connection subtype constants. For possible values, see ConnectionSubType
        /// </summary>
        public ConnectionSubType ConnectionSubType
        {
            get { return m_connSubType; }
            internal set { m_connSubType = value; }
        }

        /// <summary>
        /// One of the connection type constants. For possible values, see ConnectionType
        /// </summary>
        public ConnectionType ConnectionType
        {
            get { return m_connectionType; }
            internal set { m_connectionType = value; }
        }
	

    }
}
