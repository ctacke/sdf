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

namespace OpenNETCF.Net.NetworkInformation
{
  /// <summary>
  /// The SSID class represents a single SSID value which
  /// an adapter might be receiving data from.  It can be
  /// queried for SSID-specific information for the
  /// associated adapter such as signal strength.
  /// </summary>
  public class AccessPoint
  {
    /// <summary>
    /// The AP's operating channel
    /// </summary>
    public int Channel { get; private set; }

    /// <summary>
    /// The SSID name string.
    /// </summary>
    public String Name { get; private set; }

    /// <summary>
    /// The privacy mask for the adapter.
    /// </summary>
    public WEPStatus Privacy { get; private set; }

    /// <summary>
    /// The authentication mode for the adapter.
    /// </summary>
    public AuthenticationMode AuthenticationMode { get; private set; }

    /// <summary>
    /// Returns the strength of the RF Ethernet signal
    /// being received by the adapter for the SSID, in dB.
    /// </summary>
    /// <returns>
    /// integer strength in dB; zero, if adapter is not
    /// an RF adapter or an error occurred
    /// </returns>
    internal int SignalStrengthInDecibels { get; private set; }

    /// <summary>
    /// Returns the current type of network in use in
    /// the form of an element of the 
    /// Ndis80211NetworkType enumeration.
    /// </summary>
    /// <returns>
    /// Ndis80211NetworkType network type
    /// </returns>
    public NetworkType NetworkTypeInUse { get; private set; }

    /// <summary>
    /// Returns the current infrastructure in use by the
    /// adapter.
    /// </summary>
    /// <returns>
    /// Ndis80211NetworkInfrastructure type
    /// </returns>
    public InfrastructureMode InfrastructureMode { get; private set; }

    internal AccessPoint(WLANConfiguration config)
    {
        if (config == null) throw new ArgumentNullException("config");

        Name = config.SSID;
        macaddr = config.MACAddress;
        Privacy = config.Privacy;
        AuthenticationMode = config.AuthenticationMode;

        Channel = config.Configuration.Frequency;
        if (Channel > 14)
        {
            Channel = (Channel - 2407000) / 5000;
        }

        // see if the rssi is in the HIWORD or LOWORD
        uint ssi = (uint)config.Rssi;
        if (((ssi & 0xFFFF0000) > 0) && ((ssi & 0xffff) == 0))
        {
            // hiword
            SignalStrengthInDecibels = config.Rssi >> 16;
        }
        else if (ssi == 0)
        {
            SignalStrengthInDecibels = -99;
        }
        else
        {
            // loword
            SignalStrengthInDecibels = config.Rssi;
        }

        supportedrates = config.Rates;
        NetworkTypeInUse = config.NetworkTypeInUse;
        InfrastructureMode = config.InfrastructureMode;
    }

    internal AccessPoint(BSSID bssid)
    {
      if (bssid == null) throw new ArgumentNullException("bssid");

      Name = bssid.SSID;
      macaddr = bssid.MacAddress;
      Privacy = (WEPStatus)bssid.Privacy;
      AuthenticationMode = bssid.AuthenticationMode;

      Channel = bssid.Configuration.Frequency;
      if (Channel > 14)
      {
        Channel = (Channel - 2407000) / 5000;
      }

      // see if the rssi is in the HIWORD or LOWORD
      uint ssi = (uint)bssid.Rssi;
      if (((ssi & 0xFFFF0000) > 0) && ((ssi & 0xffff) == 0))
      {
        // hiword
        SignalStrengthInDecibels = bssid.Rssi >> 16;
      }
      else
      {
        // loword
        SignalStrengthInDecibels = bssid.Rssi;
      }

      supportedrates = bssid.SupportedRates;
      NetworkTypeInUse = bssid.NetworkTypeInUse;
      InfrastructureMode = bssid.InfrastructureMode;
    }


    internal byte[] macaddr;
    /// <summary>
    /// The hardware address for the network adapter.
    /// </summary>
    public PhysicalAddress PhysicalAddress
    {
      get { return new PhysicalAddress(macaddr); }
    }

    /// <summary>
    /// Returns the strength of the RF Ethernet signal
    /// being received by the adapter for the SSID, in dB.
    /// </summary>
    /// <returns>
    /// SignalStrength instance containing the strength
    /// </returns>
    public SignalStrength SignalStrength
    {
      get
      {
        return new SignalStrength(SignalStrengthInDecibels);
      }
    }

    internal byte[] supportedrates;
    /// <summary>
    /// Returns the list of supported signaling rates for
    /// the adapter.  Each value indicates a single rate.
    /// </summary>
    /// <returns>
    /// An array of supported rates in kilobits per second
    /// </returns>
    public int[] SupportedRates
    {
      get
      {
        ArrayList list = new ArrayList(supportedrates.Length);

        for (int i = 0; i < supportedrates.Length; i++)
        {
          if (supportedrates[i] > 0)
          {
            list.Add(supportedrates[i] * 500);
          }
        }
        return (int[])list.ToArray(typeof(int));
      }
    }

    /// <summary>
    /// Return the name of the AccessPoint
    /// </summary>
    /// <returns>
    /// string name of the access point
    /// </returns>
    public override string ToString()
    {
      return this.Name;
    }

    /// <summary>
    /// Determines if two access points are the same based on a MAC/Name comparison
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
      AccessPoint ap = obj as AccessPoint;

      if (ap == null) return false;

      if (!ap.PhysicalAddress.Equals(this.PhysicalAddress)) return false;

      // check the name, just in case they've MAC spoofed.  Sure, they could be named the same too, but it's one more layer of checking
      if (ap.Name != this.Name) return false;

      return true;
    }

    public override int GetHashCode()
    {
      return this.PhysicalAddress.GetHashCode();
    }
  }
}
