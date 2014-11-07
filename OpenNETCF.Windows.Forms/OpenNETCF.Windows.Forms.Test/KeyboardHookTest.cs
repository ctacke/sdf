using OpenNETCF.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenNETCF.Testing.Support.SmartDevice;
using System.Windows.Forms;

namespace OpenNETCF.Windows.Forms.Test
{
    [TestClass()]
    public class KeyboardHookTest : TestBase
    {
        [Ignore]
        // this test causes failure on the TFS server, though it runs and passe locally
        [TestMethod()]
        [Description("Ensures that the keyboard hook is working")]
        public void KeyboardHookEnabledDisabledPositive()
        {
            int pressedkey = -1;
            bool fired = false;

            KeyboardHook hook = new KeyboardHook();
            hook.KeyDetected += new KeyHookEventHandler(
                delegate(OpenNETCF.Win32.WM keyMessage, KeyData keyData)
                {
                    pressedkey = keyData.KeyCode;
                    fired = true;
                });

            Form f = new Form();
            f.ClientSize = new System.Drawing.Size(240, 268);
            TextBox t = new TextBox();
            t.Multiline = true;
            f.Controls.Add(t);
            f.Show();
            Application2.DoEvents();
            t.Focus();
            Application2.DoEvents();

            SendKeys.Send("a");
            Application2.DoEvents();

            Assert.IsFalse(fired, "hook fired when disabled");

            hook.Enabled = true;
            SendKeys.Send("h");
            Application2.DoEvents();

            Assert.IsTrue(fired, "hook did not fire when enabled");
            Assert.AreEqual((int)Keys.H, pressedkey);
            Assert.AreEqual("ah", t.Text);

            hook.Dispose();
            f.Close();
            f.Dispose();
        }

        [Ignore]
        // this test causes failure on the TFS server, though it runs and passe locally
        [TestMethod()]
        [Description("Ensures that the keyboard hook is working")]
        public void KeyboardHookFilterPositive()
        {
            int pressedkey = -1;
            bool fired = false;

            KeyboardHook hook = new KeyboardHook();
            hook.KeyDetected += new KeyHookEventHandler(
                delegate(OpenNETCF.Win32.WM keyMessage, KeyData keyData)
                {
                    pressedkey = keyData.KeyCode;
                    fired = true;
                });

            Form f = new Form();
            f.ClientSize = new System.Drawing.Size(240, 268);
            TextBox t = new TextBox();
            t.Multiline = true;
            f.Controls.Add(t);
            f.Show();
            Application2.DoEvents();
            t.Focus();
            Application2.DoEvents();

            SendKeys.Send("a");
            Application2.DoEvents();
            
            Assert.IsFalse(fired, "hook fired when disabled");

            hook.Enabled = true;
            SendKeys.Send("h");
            Application2.DoEvents();

            Assert.IsTrue(fired, "hook did not fire when enabled");
            Assert.AreEqual((int)Keys.H, pressedkey);
            Application2.DoEvents();
            Assert.AreEqual("ah", t.Text);

            hook.PassOnKeys = false;
            fired = false;
            SendKeys.Send("a");
            Application2.DoEvents();

            // hook should fire, get the key data, but the textbox should *not* get the character
            Assert.IsTrue(fired, "hook did not fire");
            Assert.AreEqual((int)Keys.A, pressedkey);
            Application2.DoEvents();
            Assert.AreEqual("ah", t.Text);

            hook.Dispose();
            f.Close();
            f.Dispose();
        }
    }
}
