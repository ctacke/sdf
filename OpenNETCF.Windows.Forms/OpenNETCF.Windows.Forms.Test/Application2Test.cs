using OpenNETCF.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Testing.Support.SmartDevice;
using System.Windows.Forms;
using System.Threading;

namespace OpenNETCF.Windows.Forms.Test
{
  [TestClass()]
  public class Application2Test : TestBase
  {
    [Ignore]
    [TestMethod()]
    [Description("Tests that a basic message filter's PreFilterMessage method gets called when added to Application2's filters")]
    public void AddMessageFilterTest()
    {
      AutoResetEvent filterEvent = new AutoResetEvent(false);
      TestFilter filter = new TestFilter(filterEvent);
      Application2.AddMessageFilter(filter);

      Form f = new Form();
      f.Visible = true;
      Application2.DoEvents();

      Assert.IsTrue(filterEvent.WaitOne(1000, false), "Filter PreFilterMessage was not called");

      filterEvent.Close();
      f.Dispose();
      Application2.Exit();
    }
  }

  public class TestFilter : IMessageFilter
  {
    private AutoResetEvent m_event;

    public TestFilter(AutoResetEvent filterEvent)
    {
      m_event = filterEvent;
    }

    public bool PreFilterMessage(ref Microsoft.WindowsCE.Forms.Message m)
    {
      m_event.Set();

      return false;
    }
  }
}
