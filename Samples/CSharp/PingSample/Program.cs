using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Net.NetworkInformation;
using System.Net;
using System.Diagnostics;

namespace PingSample
{
  class Program
  {
    static void Main(string[] args)
    {
      IPHostEntry hostEnt = Dns.GetHostEntry("www.opennetcf.com");
      IPAddress ip = hostEnt.AddressList[0];
      Ping ping = new Ping();

      byte[] sendBuffer = new byte[] 
      {
        0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
        0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10,
        0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18,
        0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20,
      };

      Debug.WriteLine(string.Format("Pinging 'www.opennetcf.com' [{0}] with 32 bytes of data:", ip.ToString()));

      for (int i = 0; i < 4; i++)
      {
        try
        {
          PingReply reply = ping.Send(ip, sendBuffer, 200, null);
          if (reply.Status == IPStatus.Success)
          {
            Debug.WriteLine(string.Format("Reply from {0}: bytes=32 time={1}ms TTL={2}", reply.Address.ToString(), reply.RoundTripTime, reply.Options.Ttl));
          }
          else
          {
            Debug.WriteLine(string.Format("Failed: {0}", reply.Status.ToString()));
          }
        }
        catch(Exception ex)
        {
          Debug.WriteLine(ex.Message);
          break;
        }
      }
    }
  }
}
