using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;

namespace OpenNETCF.Reflection.Test
{
    [TestClass]
    public class DeviceFixture : TestBase
    {
        [TestMethod]
        [Description("Tests for the existence of a known library with a valid extension")]
        public void LibraryWithValidExtension()
        {
            NativeLibrary lib = Device.Win32Library("coredll.dll");
            Assert.IsNotNull(lib);
        }

        [TestMethod]
        [Description("Tests for the existence of a known library with no extension")]
        public void LibraryWithNoExtension()
        {
            NativeLibrary lib = Device.Win32Library("coredll");
            Assert.IsNotNull(lib);
        }

        [TestMethod]
        [Description("Tests for the existence of a library with an invalid extension")]
        [ExpectedException(typeof(ArgumentException), "library must be a DLL.")]
        public void LibraryWithInvalidExtensionRaisesException()
        {
            Device.Win32Library("coredll.exe");
        }

        [TestMethod]
        [Description("Tests for the non-existence of a fictional library")]
        public void LibraryDoesNotExist()
        {
            NativeLibrary lib = Device.Win32Library("fakeLibName");
            Assert.IsNotNull(lib, "lib should never be null.");
            Assert.IsFalse(lib.Exists, "Library does exist");
        }

        [TestMethod]
        [Description("Tests for the existence of a well-known library")]
        public void LibraryDoesExist()
        {
            NativeLibrary lib = Device.Win32Library("coredll");
            Assert.IsNotNull(lib);
            Assert.IsTrue(lib.Exists, "The library does not exist.");
        }

        [TestMethod]
        [Description("Tests for the non-existence of a fictional method")]
        public void MethodDoesNotExist()
        {
            NativeLibrary lib = Device.Win32Library("coredll");
            Assert.IsNotNull(lib);
            Assert.IsFalse(lib.HasMethod("ThisMethodDoesNotExist"), "The method does exist.");
        }

        [TestMethod]
        [Description("Tests for the existence of a well-known method")]
        public void MethodDoesExist()
        {
            NativeLibrary lib = Device.Win32Library("coredll");
            Assert.IsNotNull(lib);
            Assert.IsTrue(lib.HasMethod("FindWindowW"), "The method does not exist.");
        }
    }
}
