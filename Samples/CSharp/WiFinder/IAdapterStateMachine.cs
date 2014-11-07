using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Net.NetworkInformation;

namespace WiFinder
{
  internal interface IAdapterStateMachine
  {
    event AttachedAPChangeHandler AttachedAPChanged;
    event APChangeHandler APInRange;
    event APChangeHandler APOutOfRange;
    event APChangeHandler APInfoChange;

    IWirelessNetworkInterface WirelessInterface { get; }

    void Start();
    void Stop();
    void StateFunction();
    bool Enabled { get; set; }

  }
}
