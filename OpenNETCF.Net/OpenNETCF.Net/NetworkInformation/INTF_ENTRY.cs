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
using System.Runtime.InteropServices;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Interface Entry for WZC
    /// </summary>
    internal struct INTF_ENTRY : IDisposable, ICloneable
    {
        private IntPtr wszGuid;
        private IntPtr wszDescr;
        public uint ulMediaState;
        public uint ulMediaType;
        public uint ulPhysicalMediaType;
        public InfrastructureMode nInfraMode;
        public AuthenticationMode nAuthMode;
        public WEPStatus nWepStatus;

        /// <summary>
        /// control flags (see INTFCTL_* defines)
        /// </summary>
        public uint dwCtlFlags;
        /// <summary>
        /// capabilities flags (see INTFCAP_* defines)
        /// </summary>
        public uint dwCapabilities;
        /// <summary>
        /// encapsulates the SSID raw binary
        /// </summary>
        public RAW_DATA rdSSID;
        /// <summary>
        /// encapsulates the BSSID raw binary
        /// </summary>
        public RAW_DATA rdBSSID;
        /// <summary>
        /// encapsulates one WZC_802_11_CONFIG_LIST structure
        /// </summary>
        public RAW_DATA rdBSSIDList;
        /// <summary>
        /// encapsulates one WZC_802_11_CONFIG_LIST structure
        /// </summary>
        public RAW_DATA rdStSSIDList;
        /// <summary>
        /// data for various control actions on the interface
        /// </summary>
        public RAW_DATA rdCtrlData;
        /// <summary>
        /// To track caller that freeing the same structure more than one time..
        /// </summary>
        public int bInitialized;


        /// <summary>
        /// Creates a new entry with given name in memory 
        /// </summary>
        /// <param name="guid">Name</param>
        /// <returns>Entry</returns>
        public static INTF_ENTRY GetEntry(string guid)
        {
            INTF_ENTRY entry = new INTF_ENTRY();
            entry.Guid = guid;
            INTF_FLAGS dwOutFlags;
            int uret = NativeMethods.WZCQueryInterface(null, INTF_FLAGS.INTF_ALL, ref entry, out dwOutFlags);
            if (uret > 0)
                throw new NetworkInformationException(uret);
            return entry;
        }

        /// <summary>
        /// SSID
        /// </summary>
        public string SSID
        {
            get { return Encoding.ASCII.GetString(rdSSID.lpData, 0, rdSSID.lpData.Length); }
            set { rdSSID.lpData = Encoding.ASCII.GetBytes(value); }
        }
        public string BSSID
        {
            get { return BitConverter.ToString(rdBSSID.lpData, 0); }
        }

        /// <summary>
        /// Entry name
        /// </summary>
        public string Guid
        {
            get
            {
                return Marshal.PtrToStringUni(wszGuid);
            }
            set
            {
                if (wszGuid != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(wszGuid);
                }
                wszGuid = Marshal.AllocHGlobal((value.Length + 1) * 2);
                byte[] chars = Encoding.Unicode.GetBytes(value + '\0');
                Marshal.Copy(chars, 0, wszGuid, chars.Length);
            }
        }

        /// <summary>
        /// Entry description
        /// </summary>
        public string Description
        {
            get
            {
                return Marshal.PtrToStringUni(wszDescr);
            }
            set
            {
                if (wszDescr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(wszDescr);
                }
                wszDescr = Marshal.AllocHGlobal((value.Length + 1) * 2);
                byte[] chars = Encoding.Unicode.GetBytes(value + '\0');
                Marshal.Copy(chars, 0, wszDescr, chars.Length);
            }
        }
        /// <summary>
        /// Overriden
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Guid;
        }


        #region IDisposable Members

        public void Dispose()
        {
            if (wszDescr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.wszDescr);
            }
            if (wszGuid != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.wszGuid);
            }
            NativeMethods.WZCDeleteIntfObj(ref this);
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            INTF_ENTRY entry = (INTF_ENTRY)MemberwiseClone();
            entry.rdBSSID.Clear();
            entry.rdBSSIDList.Clear();
            entry.rdCtrlData.Clear();
            entry.rdSSID.Clear();
            entry.rdStSSIDList.Clear();
            entry.rdBSSID.lpData = this.rdBSSID.lpData;
            entry.rdBSSIDList.lpData = this.rdBSSIDList.lpData;
            entry.rdCtrlData.lpData = this.rdCtrlData.lpData;
            entry.rdSSID.lpData = this.rdSSID.lpData;
            entry.rdStSSIDList.lpData = this.rdStSSIDList.lpData;
            return entry;
        }

        #endregion
    }

    // used for Reset only at this point
    internal struct INTF_ENTRY_EX
    {
        private IntPtr wszGuid;
        private IntPtr wszDescr;
        public uint ulMediaState;
        public uint ulMediaType;
        public uint ulPhysicalMediaType;
        public InfrastructureMode nInfraMode;
        public AuthenticationMode nAuthMode;
        public WEPStatus nWepStatus;
        public uint dwCtlFlags;
        public uint dwCapabilities;
        public RAW_DATA rdSSID;
        public RAW_DATA rdBSSID;
        public RAW_DATA rdBSSIDList;
        public RAW_DATA rdStSSIDList;
        public RAW_DATA rdCtrlData;


        public int bInitialized;


        public string SSID
        {
            get { return Encoding.ASCII.GetString(rdSSID.lpData, 0, rdSSID.lpData.Length); }
            set { rdSSID.lpData = Encoding.ASCII.GetBytes(value); }
        }

        /// <summary>
        /// Entry name
        /// </summary>
        public string Guid
        {
            get
            {
                return Marshal.PtrToStringUni(wszGuid);
            }
            set
            {
                if (wszGuid != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(wszGuid);
                }
                wszGuid = Marshal.AllocHGlobal((value.Length + 1) * 2);
                byte[] chars = Encoding.Unicode.GetBytes(value + '\0');
                Marshal.Copy(chars, 0, wszGuid, chars.Length);
            }
        }
    }
}
