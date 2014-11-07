using OpenNETCF.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Testing.Support.SmartDevice;
using System.Text;
using System.Runtime.InteropServices;

namespace OpenNETCF.Test
{
    [TestClass()]
    public class Marshal2Test : TestBase
    {
        [TestMethod()]
        [Description("Ensures that StringToHGlobalUni actually allocates and populates a memory region properly")]
        public unsafe void StringToHGlobalUniTest()
        {
            string input = "Input string";
            byte[] inputBytes = Encoding.Unicode.GetBytes(input + '\0');

            IntPtr pString = Marshal2.StringToHGlobalUni(input);

            byte* p = (byte*)pString;

            for (int i = 0; i < inputBytes.Length; i++)
            {
                Assert.AreEqual(*p, inputBytes[i]);
                p++;
            }

            Marshal.FreeHGlobal(pString);            
        }

        [TestMethod()]
        [Description("Ensures that StringToHGlobalAnsi actually allocates and populates a memory region properly")]
        public unsafe void StringToHGlobalAnsiTest()
        {
            string input = "Input string";
            byte[] inputBytes = Encoding.ASCII.GetBytes(input + '\0');

            IntPtr pString = Marshal2.StringToHGlobalAnsi(input);

            byte* p = (byte*)pString;

            for (int i = 0; i < inputBytes.Length; i++)
            {
                Assert.AreEqual(*p, inputBytes[i]);
                p++;
            }

            Marshal.FreeHGlobal(pString);
        }
    }
}
