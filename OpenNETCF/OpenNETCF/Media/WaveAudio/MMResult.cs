using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Media.WaveAudio
{
    internal class MMResult
    {
        public static implicit operator MMResult(int val)
        {
            if (val != 0)
                throw new InvalidOperationException("Bad MMResult: " + val.ToString("X"));
            return new MMResult();
        }


    }
}
