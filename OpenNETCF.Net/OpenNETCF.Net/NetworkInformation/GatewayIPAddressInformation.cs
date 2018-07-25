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
using System.Net;

namespace OpenNETCF.Net.NetworkInformation
{
  internal delegate void AddressChangedHandler();

  /// <summary>
  /// Represents the IP address of the network gateway. This class cannot be instantiated.
  /// </summary>
  public class GatewayIPAddressInformation
  {
    private IPAddress m_gateway;
    internal event AddressChangedHandler Changed;

    /// <summary>
    /// Initializes the members of this class.
    /// </summary>
    protected GatewayIPAddressInformation() { }

    internal GatewayIPAddressInformation(IPAddress gatewayAddress)
    {
      m_gateway = gatewayAddress;
    }

    /// <summary>
    /// Get the IP address of the gateway.
    /// </summary>
    public IPAddress Address
    {
      get { return m_gateway; }
      set
      {
        m_gateway = value;
        if(Changed != null)
        {
          Changed();
        }
      }
    }


    /// <summary>
    /// Determines if this address is equivalent to the address of another GatewayIPAddressInformation instance
    /// </summary>
    /// <param name="obj">A GatewayIPAddressInformation instance</param>
    /// <returns>true if equivalent, otherwise false</returns>
    public override bool Equals(object obj)
    {
      if (obj == null || GetType() != obj.GetType())
      {
        return false;
      }

      GatewayIPAddressInformation ipi = (GatewayIPAddressInformation)obj;

      return ipi.Address.Equals(this.Address);
    }

    /// <summary>
    /// Returns a hash value for a GatewayIPAddressInformation
    /// </summary>
    /// <returns>A hash value</returns>
    public override int GetHashCode()
    {
      return this.Address.GetHashCode();
    }

  }
}
