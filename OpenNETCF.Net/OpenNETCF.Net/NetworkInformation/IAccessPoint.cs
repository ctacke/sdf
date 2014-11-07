using System;

namespace OpenNETCF.Net.NetworkInformation
{
    public interface IAccessPoint
    {
        AuthenticationMode AuthenticationMode { get; }
        InfrastructureMode InfrastructureMode { get; }
        string Name { get; }
        WEPStatus Privacy { get; }
        SignalStrength SignalStrength { get; }
    }
}
