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

namespace OpenNETCF.Net.NetworkInformation
{
  public interface INetworkInterface
  {
    void Bind();
    System.Net.IPAddress CurrentIpAddress { get; set; }
    System.Net.IPAddress CurrentSubnetMask { get; set; }
    string Description { get; }
    DateTime DhcpLeaseExpires { get; }
    DateTime DhcpLeaseObtained { get; }
    void DhcpRelease();
    void DhcpRenew();
    OpenNETCF.Net.NetworkInformation.IPInterfaceProperties GetIPProperties();
    OpenNETCF.Net.NetworkInformation.IPv4InterfaceStatistics GetIPv4Statistics();
    OpenNETCF.Net.NetworkInformation.PhysicalAddress GetPhysicalAddress();
    string Id { get; }
    OpenNETCF.Net.NetworkInformation.InterfaceOperationalStatus InterfaceOperationalStatus { get; }
    string Name { get; }
    OpenNETCF.Net.NetworkInformation.NetworkInterfaceType NetworkInterfaceType { get; }
    OpenNETCF.Net.NetworkInformation.OperationalStatus OperationalStatus { get; }
    void Rebind();
    void Refresh();
    int Speed { get; }
    string ToString();
    void Unbind();
  }
}
