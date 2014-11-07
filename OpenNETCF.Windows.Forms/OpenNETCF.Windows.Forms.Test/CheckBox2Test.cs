using OpenNETCF.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenNETCF.Testing.Support.SmartDevice;
using System;
using System.Windows.Forms;
using System.Threading;

namespace OpenNETCF.Windows.Forms.Test
{
  [TestClass()]
  public class CheckBox2Test : TestBase
  {
    [TestMethod()]
    [Description("Ensures that when the Text property is set, the same value is read back with a get")]
    public void TextTestPositive()
    {
      CheckBox2 target = new CheckBox2();
      string expected = "Test Text";
      string actual;
      target.Text = expected;
      actual = target.Text;
      Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    [Description("Ensures that the control can be created without throwing an exception")]
    public void CheckBox2ConstructorTest()
    {
      CheckBox2 target = new CheckBox2();
      Assert.IsInstanceOfType(target, typeof(Control));
    }

    [Ignore]
    [TestMethod()]
    [Description("Ensures that changing the CheckState property of the control fires the CheckStateChanged event")]
    public void CheckedStateEventPositiveTest()
    {
      bool eventFired = false;
      CheckBox2 target = new CheckBox2();
      target.CheckState = CheckState.Unchecked;

      target.CheckStateChanged += new EventHandler(
        delegate (object sender, EventArgs e)
        {
          eventFired = true;
        });

      target.CheckState = CheckState.Checked;
      System.Windows.Forms.Application.DoEvents();
      Thread.Sleep(10);

      Assert.IsTrue(eventFired, "Event did not fire");
    }

    [Ignore]
    [TestMethod()]
    [Description("Ensures that giving focus to the control fires the GotFocus event")]
    public void GotFocusEventPositiveTest()
    {
      bool eventFired = false;
      CheckBox2 target = new CheckBox2();
      Form container = new Form();
      
      container.Controls.Add(target);
      container.Focus();

      target.GotFocus += new EventHandler(
        delegate(object sender, EventArgs e)
        {
          eventFired = true;
        });

      target.Focus();
      System.Windows.Forms.Application.DoEvents();
      Thread.Sleep(10);

      Assert.IsTrue(eventFired, "Event did not fire");

      container.Dispose();
    }

    [Ignore]
    [TestMethod()]
    [Description("Ensures that giving focus away from the control fires the LostFocus event")]
    public void LostFocusEventPositiveTest()
    {
      bool eventFired = false;
      CheckBox2 target = new CheckBox2();
      Form container = new Form();

      container.Controls.Add(target);
      target.Focus();

      target.LostFocus += new EventHandler(
        delegate(object sender, EventArgs e)
        {
          eventFired = true;
        });

      container.Focus();
      System.Windows.Forms.Application.DoEvents();
      Thread.Sleep(10);

      Assert.IsTrue(eventFired, "Event did not fire");

      container.Dispose();
    }
  }
}
