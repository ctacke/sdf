namespace OpenNETCF.Core.Test
{
  using System;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using OpenNETCF.Testing.Support.SmartDevice;
  using OpenNETCF;

  [TestClass]
  public class TypeExtensionsTests : TestBase
  {
    [TestMethod]
    public void TestImplementsIsTrue()
    {
      DisposableClass mock = new DisposableClass();
      Assert.IsTrue(mock.GetType().Implements<IDisposable>());
    }

    [TestMethod]
    public void TestImplementsIsFalse()
    {
      DisposableClass mock = new DisposableClass();
      Assert.IsFalse(mock.GetType().Implements<IComparable>());
    }

    [TestMethod]
    [ExpectedException(typeof(System.ArgumentException))]
    public void TestImplementsWithNonInterfaceTypeThrowsException()
    {
      DisposableClass mock = new DisposableClass();
      Assert.IsFalse(mock.GetType().Implements<System.String>());
    }
  }
}