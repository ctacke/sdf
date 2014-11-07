using OpenNETCF.WindowsCE.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;

namespace OpenNETCF.WindowsCE.Services.Test
{
    /// <summary>
    ///This is a test class for ServiceCollectionTest and is intended
    ///to contain all ServiceCollectionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceCollectionTest : TestBase
    {
        /// <summary>
        ///A test for GetServices
        ///</summary>
        [TestMethod()]
        public void GetServicesTest()
        {
            ServiceCollection services = new ServiceCollection();
            Assert.IsNotNull(services);
        }
    }
}
