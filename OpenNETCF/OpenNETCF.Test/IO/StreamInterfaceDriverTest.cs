using OpenNETCF.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;
using System;

namespace OpenNETCF.Test
{
  [TestClass()]
  public class StreamInterfaceDriverTest : TestBase
  {
    [TestMethod()]
    [DeploymentItem("OpenNETCF.dll")]
    [Description("Ensures the SID ctor throws a ArgumentNullException of the portName is null")]
    public void NullPortNameCTorTest()
    {
      ArgumentNullException expected = null;

      try
      {
        SampleSID sid = new SampleSID(null);
      }
      catch (ArgumentNullException ex)
      {
        expected = ex;
      }

      Assert.IsNotNull(expected, "ArgumentNullException not thrown as expected");
    }

    [TestMethod()]
    [DeploymentItem("OpenNETCF.dll")]
    [Description("Ensures the SID ctor throws a ArgumentException of the portName is empty")]
    public void EmptyPortNameCTorTest()
    {
      ArgumentException expected = null;

      try
      {
        SampleSID sid = new SampleSID(string.Empty);
      }
      catch (ArgumentException ex)
      {
        expected = ex;
      }

      Assert.IsNotNull(expected, "ArgumentException  not thrown as expected");
    }

  }

  public class SampleSID : StreamInterfaceDriver
  {
    public SampleSID(string portName)
      : base(portName)
    {
    }

    public void Open(System.IO.FileAccess access, System.IO.FileShare share)
    {
      base.Open(access, share);
    }

    public void DeviceIoControl(uint controlCode, byte[] inData, byte[] outData)
    {
      base.DeviceIoControl(controlCode, inData, outData);
    }
    
  }
}
