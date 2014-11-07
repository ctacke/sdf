using System;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.WindowsCE.Messaging;

namespace OpenNETCF.WindowsCE
{
    internal class PowerBroadcast : Message
    {
        
        public PowerBroadcast(int size)
        {
            MessageBytes = new byte[size];
        }

        public PowerEventType Message 
        { 
            get { return (PowerEventType)BitConverter.ToInt32( MessageBytes, 0); } 
        }

        public PowerStateFlags Flags 
        {
            get { return (PowerStateFlags)BitConverter.ToInt32(MessageBytes, 4); } 
        }
        
        public int Length 
        {
            get { return BitConverter.ToInt32(MessageBytes, 8); } 
        }
        
        public byte[] SystemPowerState 
        { 
            get 
            { 
                byte[] data = new byte[Length];
                Buffer.BlockCopy(MessageBytes, 12, data, 0, Length); 
                return data; 
            } 
        }
    }
}
