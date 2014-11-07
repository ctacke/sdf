using OpenNETCF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;
using System.Text;
using System;

namespace OpenNETCF.Test
{
  /// <summary>
  ///This is a test class for CRCTest and is intended
  ///to contain all CRCTest Unit Tests
  ///</summary>
  [TestClass()]
  public class CRCTest : TestBase
  {
    [Ignore]
    [TestMethod()]
    public void GenerateCRCTableZeroBitsTest()
    {
      ArgumentOutOfRangeException expected = null;

      try
      {
        // CRC_Accessor.GenerateCRCTable(0, 1);
      }
      catch (ArgumentOutOfRangeException ex)
      {
        expected = ex;
      }

      Assert.IsNotNull(expected);
    }
  }
}
