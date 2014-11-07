using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.Windows.Forms;
using OpenNETCF.Win32;

namespace KeyRecorder
{
  public partial class Form1 : Form
  {
    private KeyboardHook m_keyHook;
    private int m_hideshift = 0;
    private bool m_hidden = false;

    
    public Form1()
    {
      m_keyHook = new KeyboardHook();

      InitializeComponent();

      clear.Click += clear_Click;
      hideShow.Click += hideShow_Click;

      m_keyHook.KeyDetected += OnKeyDetected;
      m_keyHook.Enabled = true;

      inputPanel.EnabledChanged += inputPanel_EnabledChanged;
    }

    void hideShow_Click(object sender, EventArgs e)
    {
      m_hidden = !m_hidden;

      if (m_hidden)
      {
        hideShow.BackColor = SystemColors.ActiveBorder;
        hideShow.Top = 0;
      }
      else
      {
        hideShow.BackColor = SystemColors.Control;
        hideShow.Top = 4;
      }

      m_hideshift = m_hideshift == 0 ? 27 : 0;
      Move();
    }

    void inputPanel_EnabledChanged(object sender, EventArgs e)
    {
      Move();
    }

    private void Move()
    {
      if (inputPanel.Enabled)
      {
        this.Top = inputPanel.Bounds.Top - 30 + m_hideshift;
      }
      else
      {
        this.Top = Screen.PrimaryScreen.WorkingArea.Height - 30 + m_hideshift;
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      this.Width = Screen.PrimaryScreen.WorkingArea.Width;
      this.Height = 29;
      Move();

      Win32Window keysWindow = new Win32Window(keys.Handle);
      keysWindow.ExtendedStyle |= WS_EX.NOACTIVATE;

      base.OnLoad(e);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      e.Graphics.FillRectangle(new SolidBrush(SystemColors.ControlDark), 0, 0, this.Width, this.Height);
    }

    void clear_Click(object sender, EventArgs e)
    {
      keys.Text = string.Empty;
    }

    void OnKeyDetected(OpenNETCF.Win32.WM keyMessage, KeyData keyData)
    {
      if (this.InvokeRequired)
      {
        this.Invoke(new KeyHookEventHandler(OnKeyDetected), new object[] { keyMessage, keyData });
        return;
      }
      char c = (char)keyData.KeyCode;
      if (keyMessage == WM.KEYUP)
      {
        if (char.IsLetterOrDigit(c))
        {
          keys.Text += c;
        }
        else
        {
          switch ((Keys)c)
          {
            case Keys.Up:
              keys.Text += "{up}";
              break;
            case Keys.Down:
              keys.Text += "{dn}";
              break;
            case Keys.Left:
              keys.Text += "{lt}";
              break;
            case Keys.Right:
              keys.Text += "{rt}";
              break;
          }
        }
      }
      keys.SelectionStart = keys.Text.Length;
      keys.ScrollToCaret();
    }
  }
}