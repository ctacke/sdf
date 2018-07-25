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
using System.Runtime.InteropServices;
using System.Net;

namespace OpenNETCF.Net.NetworkInformation
{
    internal static class IPUtility
    {
        internal unsafe static UnicastIPAddressInformationCollection ParseAddressesFromPointer(byte* pdata, string adapterName)
        {
            UnicastIPAddressInformationCollection addressList = new UnicastIPAddressInformationCollection(adapterName);

            byte* p = pdata;
            uint pNext = 0;

            do
            {
                UnicastIPAddressInformation address = new UnicastIPAddressInformation();

                pNext = (uint)*((uint*)p);
                p += 4;  // skip pNext

                // once for the IP, once for the mask
                for (int index = 0; index < 2; index++)
                {
                    string addressString;

                    // see if there's an IP at all
                    if (*p == '\0')
                    {
                        addressString = string.Empty;
                        
                        // if the address is empty skip the whole thing,
                        // but if the mask is empty, just skip it
                        if (index == 0)
                        {
                            System.Diagnostics.Debug.WriteLine("ParseAddressesFromPointer: Empty address detected");
                            p += 32;
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("ParseAddressesFromPointer: Empty netmask detected");
                            address.IPv4Mask = IPAddress.None;
                            p += 16;
                        }
                    }
                    else
                    {
                        byte[] stringBytes = new byte[16];
                        for (int i = 0; i < stringBytes.Length; i++)
                        {
                            stringBytes[i] = *p;
                            p++;
                        }
                        addressString = Encoding.ASCII.GetString(stringBytes, 0, stringBytes.Length);
                        // trim nulls
                        int nullIndex = addressString.IndexOf('\0');
                        if (nullIndex >= 0)
                        {
                            addressString = addressString.Substring(0, nullIndex);
                        }

                        if (addressString != string.Empty)
                        {
                            // store the result
                            if (index == 0)
                            {
                                address.Address = IPAddress.Parse(addressString);
                            }
                            else
                            {
                                address.IPv4Mask = IPAddress.Parse(addressString);
                            }
                        }
                    } // else
                } // for
                address.NetworkTableEntryContext = *((int*)p);
                p += 4; // skip the Context

                if (address.Address != null)
                {
                    addressList.InternalAdd(address);
                }
            } while (pNext != 0);

            return addressList;
        }
    }
}
