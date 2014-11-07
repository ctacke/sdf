using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Diagnostics;

namespace OpenNETCF.Unit.Test.Diagnostics
{
    [TestClass]
    public class TextWriterTraceListenerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var target = new StringBuilder();
            using (var stream = new StringWriter(target))
            {
                var testOutput = "Hello World";
                var listener = new TextWriterTraceListener(stream);

                Trace2.Write(testOutput);
                Assert.AreEqual(string.Empty, target.ToString());

                Trace2.Listeners.Add(listener);
                Trace2.Write(testOutput);
                Assert.AreEqual(testOutput, target.ToString());
            }
        }
    }
}
