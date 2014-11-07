using OpenNETCF.Net.NetworkInformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Testing.Support.SmartDevice;
using System.Collections;
using System.Collections.Generic;

namespace OpenNETCF.Net.NetworkInformation
{
    /// <summary>
    /// Summary description for AccessPointTest
    /// </summary>
    [TestClass]
    public class AccessPointTest : TestBase
    {
        private const int BSSID_BYTE_SIZE = 128;

        [TestMethod()]
        [WorkItem(174)]
        [Description("Proves that AccessPoint throws ArgumentNullException when null BSSID is passed")]
        public void ConstructorWithNullBSSID()
        {
            ArgumentNullException caughtException = null;
            try
            {
                AccessPoint accessPoint = new AccessPoint((WLANConfiguration)null);
            }
            catch (ArgumentNullException ex)
            {
                caughtException = ex;
            }

            Assert.IsNotNull(caughtException, "AccessPoint did not throw ArgumentNullException when null BSSID was passed");
        }

        private int GetExpectedFrequency(BSSID bssid)
        {
            if (bssid == null)
            {
                throw new ArgumentNullException("bssid");
            }

            int expectedChannel = bssid.Configuration.Frequency;
            if (expectedChannel > 14)
            {
                expectedChannel = (expectedChannel - 2407000) / 5000;
            }

            return expectedChannel;
        }

        /// <summary>
        /// Returns the expected decibel for a given bssid
        /// </summary>
        /// <param name="bssid">BSSID object</param>
        /// <returns></returns>
        private int GetExpectedDecibel(BSSID bssid)
        {
            if (bssid == null)
            {
                throw new ArgumentNullException("bssid");
            }

            int expectedDecibel;

            uint rssi = (uint)bssid.Rssi;
            if (((rssi & 0xFFFF0000) > 0) && ((rssi & 0xffff) == 0))
            {
                expectedDecibel = bssid.Rssi >> 16;
            }
            else
            {
                expectedDecibel = bssid.Rssi;
            }

            return expectedDecibel;
        }

        private BSSID GetBSSID()
        {
            BSSID bssid = null;
            byte[] byteArray = new byte[BSSID_BYTE_SIZE];
            Random random = new Random();

            //Fill byteArray with random numbers
            random.NextBytes(byteArray);

            bssid = new BSSID(byteArray, 0, true);

            return bssid;
        }

        [TestMethod()]
        [WorkItem(174)]
        [Description("Proves that AccessPoint object was constructed successfully")]
        public void ConstructorPositive()
        {
            BSSID bssid = GetBSSID();
            AccessPoint target = null;

            Exception caughtException = null;
            try
            {
                target = new AccessPoint(bssid);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            Assert.IsNull(caughtException, "Constructor threw exception. AccessPoint was not constructed successfully");
        }
        
        [TestMethod()]
        [WorkItem(174)]
        [Description("Proves that AccessPoint object was constructed and properties were set successfully")]
        public void ConstructorSetProperties()
        {            
            BSSID bssid = GetBSSID();            
            AccessPoint target = new AccessPoint(bssid);

            int expectedChannel = GetExpectedFrequency(bssid);

            int expectedDecibel = GetExpectedDecibel(bssid);            

            //Channel is set correctly
            Assert.AreEqual<int>(expectedChannel, target.Channel, "AccessPoint did not return expected Channel");
            
            //InfrastructureMode is set correctly
            Assert.AreEqual<InfrastructureMode>(bssid.InfrastructureMode, target.InfrastructureMode,"AccessPoint did not return expected InfrastructureMode");
            //Name is set correctly
            Assert.AreEqual<string>(bssid.SSID, target.Name, "AccessPoint did not return expected Name");
            //MacAddress is set correctly            
            Assert.AreEqual<int>(bssid.MacAddress.Length, target.PhysicalAddress.GetAddressBytes().Length,"AccessPoint did not return expected length for PhysicalAddress address length");

            bool addressByteMismatch = false;
            byte[] addressByteArray = target.PhysicalAddress.GetAddressBytes();
            
            for (int loopVariable = 0; loopVariable < bssid.MacAddress.Length; loopVariable++)
            {
                if (bssid.MacAddress[loopVariable] != addressByteArray[loopVariable])
                {
                    addressByteMismatch = true;
                    break;
                }                
            }
            Assert.IsFalse(addressByteMismatch,"AccessPoint did not return expected macaddress bytes");
            //NetworkTypeInUse is set correctly
            Assert.AreEqual<NetworkType>(bssid.NetworkTypeInUse, target.NetworkTypeInUse,"AccessPoint did not return expected NetworkTypeInUse");
            //Privacy is set correctly
            Assert.AreEqual<WEPStatus>((WEPStatus)bssid.Privacy, target.Privacy, "AccessPoint did not return expected Privacy");
            //SignalStrength is set correctly
            Assert.AreEqual<int>(expectedDecibel, target.SignalStrength.Decibels,"AccessPoint did not return expected Decibels");
            //SignalStrengthInDecibels is set correctly
            Assert.AreEqual<int>(expectedDecibel, target.SignalStrengthInDecibels,"AccessPoint did not return expected SignalStrengthInDecibels");                        
            
            //SupportedRates is set correctly            
            List<int> validRatesList = new List<int>();
            for (int loopVariable = 0; loopVariable < bssid.SupportedRates.Length; loopVariable++)
            {
                if (bssid.SupportedRates[loopVariable] > 0)
                {                    
                    validRatesList.Add(bssid.SupportedRates[loopVariable] * 500);
                }
            }
            Assert.AreEqual<int>(validRatesList.Count, target.SupportedRates.Length, "AccessPoint did not return the correct count of valid supportedrates");
            
            bool supportedRateByteMismatch = false;
            int[] supportedRates = target.SupportedRates;
            
            for (int loopVariable = 0; loopVariable < validRatesList.Count; loopVariable++)
            {
                if (validRatesList[loopVariable] != supportedRates[loopVariable])
                {
                    supportedRateByteMismatch = true;
                    break;
                }
            }
            Assert.IsFalse(supportedRateByteMismatch, "AccessPoint did not return expected supportedrates bytes");
        }

        [TestMethod()]
        [WorkItem(174)]
        [Description("Proves that AccessPoint's ToString() method returns SSID")]
        public void ToStringReturnsSSID()
        {
            BSSID bssid = GetBSSID();
            AccessPoint target = new AccessPoint(bssid);

            Assert.AreEqual<string>(bssid.SSID, target.ToString(), "ToString() method did not return SSID");
        }
    }
}
