using System;

namespace OpenNETCF.Windows.Forms
{
    /// <summary>
    /// Identifies the chemistry of the devices main battery.
    /// </summary>
    /// <remarks>This enumeration is used by the <see cref="PowerStatus.BatteryChemistry"/> property.</remarks>
    public enum BatteryChemistry : byte
    {
        /// <summary>
        /// Alkaline battery.
        /// </summary>
        Alkaline = 0x01,
        /// <summary>
        /// Nickel Cadmium battery.
        /// </summary>
        NiCad = 0x02,
        /// <summary>
        /// Nickel Metal Hydride battery.
        /// </summary>
        NiMH = 0x03,
        /// <summary>
        /// Lithium Ion battery.
        /// </summary>
        Lion = 0x04,
        /// <summary>
        /// Lithium Polymer battery.
        /// </summary>
        LiPoly = 0x05,
        /// <summary>
        /// Zinc Air battery.
        /// </summary>
        ZincAir = 0x06,
        /// <summary>
        /// Battery chemistry is unknown.
        /// </summary>
        Unknown = 0xFF,
    }
}
