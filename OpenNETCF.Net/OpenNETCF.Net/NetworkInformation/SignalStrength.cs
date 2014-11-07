using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// The SignalStrength class provides a repository for the
    /// signal strength indication from an RF Ethernet network
    /// adapter.  From an instance of this class, you can get
    /// the signal strength in dB, an enumerated value of 
    /// type StrengthType indicating the relative strength, or
    /// a string representing the relative strength.
    /// </summary>
    public class SignalStrength
    {
        private int m_decibels;

        public SignalStrength(int decibels) 
        {
            m_decibels = decibels;
        }

        /// <summary>
        /// The Decibels property returns the signal strength
        /// in dB.
        /// </summary>
        public int Decibels
        {
            get { return m_decibels; }
        }

        /// <summary>
        /// The DBToStrength static conversion function can
        /// be used to take a dB strength value previously
        /// retrieved and generate a StrengthType enumeration
        /// value based on the relative strength.
        /// </summary>
        /// <param name="db">
        /// Signal strength in dB
        /// </param>
        /// <returns>
        /// StrengthType indicating strength that db parameter represents
        /// </returns>
        public static StrengthType DBToStrength(int db)
        {
            if (db == 0)
                return StrengthType.NotAWirelessAdapter;

            StrengthType retval;
            if (db < -90)
                retval = StrengthType.NoSignal;
            else if (db < -81)
                retval = StrengthType.VeryLow;
            else if (db < -71)
                retval = StrengthType.Low;
            else if (db < -67)
                retval = StrengthType.Good;
            else if (db < -57)
                retval = StrengthType.VeryGood;
            else
                retval = StrengthType.Excellent;

            return retval;
        }

        /// <summary>
        /// The DBToString static conversion function allows a
        /// signal strength value in dB previously retrieved
        /// to be converted into the string representation of
        /// its relative signal strength.
        /// </summary>
        /// <param name="db">
        /// Signal strength in dB.
        /// </param>
        /// <returns>
        /// String: "No Signal", "Very Low", "Low", "Good", 
        /// "Very Good", "Excellent", or 
        /// "Not a wireless adapter". 
        /// </returns>
        public static String DBToString(int db)
        {
            StrengthType st = DBToStrength(db);
            return StrengthToString(st);
        }

        /// <summary>
        /// The StrengthToString static conversion function
        /// allows a previously stored signal strength 
        /// enumeration value to be converted into its string
        /// representation.
        /// </summary>
        /// <param name="st">
        /// One of the SignalStrength enumeration values.
        /// </param>
        /// <returns>
        /// String: "No Signal", "Very Low", "Low", "Good", 
        /// "Very Good", "Excellent", or 
        /// "Not a wireless adapter". 
        /// </returns>
        public static String StrengthToString(StrengthType st)
        {
            string retval = string.Empty;

            switch (st)
            {
                case StrengthType.NotAWirelessAdapter:
                    return "Not a wireless adapter";
                case StrengthType.VeryLow:
                    return "Very Low";
                case StrengthType.Low:
                    return "Low";
                case StrengthType.Good:
                    return "Good";
                case StrengthType.VeryGood:
                    return "Very Good";
                case StrengthType.Excellent:
                    return "Excellent";
            }

            // If we don't catch the value above, empty is
            // probably the best string to return.
            return retval;
        }

        /// <summary>
        /// Strength of signal as enumerated type.
        /// </summary>
        public StrengthType Strength
        {
            get
            {
                return DBToStrength(m_decibels);
            }
        }

        /// <summary>
        /// Converts strength to string representing relative
        /// strength ("Good", "Low", etc.)
        /// </summary>
        /// <returns>
        /// String: "No Signal", "Very Low", "Low", "Good", 
        /// "Very Good", "Excellent", or 
        /// "Not a wireless adapter". 
        /// </returns>
        public override string ToString()
        {
            return DBToString(m_decibels);
        }
    }
}
