using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Media.WaveAudio
{
  [Flags]
  internal enum WHDR_FLAGS
  {
    DONE = 0x01,
    PREPARED = 0x02,
    BEGINLOOP = 0x04,
    ENDLOOP = 0x08,
    INQUEUE = 0x10
  }
}
