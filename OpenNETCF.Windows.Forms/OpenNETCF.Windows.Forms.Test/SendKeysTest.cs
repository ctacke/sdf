using OpenNETCF.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Testing.Support.SmartDevice;
using System.Windows.Forms;

namespace OpenNETCF.Windows.Forms.Test
{
  [TestClass()]
  public class SendKeysTest : TestBase
  {
    [Ignore]
    // this test causes failure on the TFS server, though it runs and passe locally
    [TestMethod()]
    [Description("Ensures that SendKeys sends the expected text to a target control")]
    public void SendKeysTestPositive()
    {
      Form f = new Form();
      f.ClientSize = new System.Drawing.Size(240, 268);
      TextBox t = new TextBox();
      t.Multiline = true;
      f.Controls.Add(t);
      f.Show();
      Application2.DoEvents();
      t.Focus();
      Application2.DoEvents();
      SendKeys.Send("hello");

      Application2.DoEvents();
      Application2.DoEvents();

      Assert.AreEqual("hello", t.Text);

      f.Close();
      f.Dispose();
    }
  }
}
