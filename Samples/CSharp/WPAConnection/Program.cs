using System;

using System.Collections.Generic;
using System.Windows.Forms;

using OpenNETCF.Net.NetworkInformation;
using System.Diagnostics;
using System.Text;

namespace WPAConnection
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            WirelessZeroConfigNetworkInterface wzc = null;

            foreach (var intf in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (intf is WirelessZeroConfigNetworkInterface)
                {
                    wzc = (WirelessZeroConfigNetworkInterface)intf;
                    break;
                }
            }

            if (wzc == null)
            {
                Output("No WZC adapter found");
                Debugger.Break();
                return;
            }


            int PAD = 20;
            foreach (var ap in wzc.PreferredAccessPoints)
            {
                Output("AP: " + ap.Name);
                Output("\tPrivacy: ".PadRight(PAD, ' ') + ap.Privacy.ToString());
                Output("\tSignal: ".PadRight(PAD, ' ') + ap.SignalStrength);
                Output("\tChannel: ".PadRight(PAD, ' ') + ap.Channel);
            }

            foreach (var ap in wzc.NearbyAccessPoints)
            {
                Output("AP: " + ap.Name);
                Output("\tPrivacy: ".PadRight(PAD, ' ') + ap.Privacy.ToString());
                Output("\tSignal: ".PadRight(PAD, ' ') + ap.SignalStrength);
                Output("\tChannel: ".PadRight(PAD, ' ') + ap.Channel);

                if ((ap.Privacy == WEPStatus.WEPEnabled) && (ap.Name.ToLower().StartsWith("opennetcf")))
                {
                    continue;
                    string key = "badf00d";
                    wzc.AddPreferredNetwork(ap.Name, true, key, 1, AuthenticationMode.Shared, WEPStatus.WEPEnabled, null);
                }
                else if (ap.Privacy == WEPStatus.TKIPEnabled)
                {
                    //continue;

                    string key = "sharedkey";
                    EAPParameters eap = null;

                    wzc.AddPreferredNetwork(ap.Name, true, key, 1, AuthenticationMode.WPAPSK, WEPStatus.TKIPEnabled, eap);
                }
                else if (ap.Privacy == WEPStatus.AESEnabled)
                {
                    //continue;

                    string key = "sharedkey";
                    EAPParameters eap = null;

                    wzc.AddPreferredNetwork(ap.Name, true, key, 1, AuthenticationMode.WPA, WEPStatus.AESEnabled, eap);
                }
            }

            Output("Press <ENTER> to continue");

            Console.ReadLine();
//            Application.Run(new Form1());
        }

        static void Output(string data)
        {
            Debug.WriteLine(data);
            Console.WriteLine(data);
        }
    }

    public class WPA
    {
    }
}