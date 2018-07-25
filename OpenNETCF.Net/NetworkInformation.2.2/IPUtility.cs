using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;

namespace OpenNETCF.Net.NetworkInformation
{
    internal static class IPUtility
    {
        internal unsafe static UnicastIPAddressInformationCollection ParseAddressesFromPointer(byte* pdata)
        {
            UnicastIPAddressInformationCollection addressList = new UnicastIPAddressInformationCollection();

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
