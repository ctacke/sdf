using System;

using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.WindowsCE
{
  public class DeviceClassAtrribute : Attribute
  {
    public DeviceClassAtrribute(string guid) { Guid = new Guid(guid); }
    public Guid Guid { set; get; }
  }
}
