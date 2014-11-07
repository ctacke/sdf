using System;

using System.Collections.Generic;
using System.Text;
using OpenNETCF.Windows.Forms;
using OpenNETCF.Net.NetworkInformation;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Drawing.Imaging;

namespace WiFinder
{
  public partial class APView : Form, IAPView
  {
    private Pen m_blackPen = new Pen(Color.Black);
    private Font m_itemTitleFont = new Font(FontFamily.GenericSansSerif, 10F, FontStyle.Regular);
    private Brush m_itemTitleBrush = new SolidBrush(Color.DarkBlue);

    private Font m_itemDetailFont = new Font(FontFamily.GenericSansSerif, 8F, FontStyle.Regular);
    private Font m_itemDetailBoldFont = new Font(FontFamily.GenericSansSerif, 8F, FontStyle.Bold);
    private Brush m_itemDetailBrush = new SolidBrush(Color.Black);
    private int m_titleHeight = 0;

    void nearbyAPList_DrawItem(object sender, DrawItemEventArgs e)
    {
      // custom draw the item
      e.Graphics.Clip = new Region(e.Bounds);

      // draw a box around the item
      e.Graphics.DrawRectangle(m_blackPen, e.Bounds);

      // the back color of the attached AP is different
      if ((e.Index == 0) && (m_attachedAPItem != null))
      {
        e.DrawBackground(Color.LightGoldenrodYellow);
      }
      else
      {
        e.DrawBackground(Color.LightSteelBlue);
      }

      AccessPoint ap = nearbyAPList.Items[e.Index].Tag as AccessPoint;

      if (ap == null)
      {
        // TODO: figure out what to do here until the AP comes back with the nearby list
        return;
      }

      /*
      // "select" the item if it should be
      if (itemList.SelectedIndex == e.Index)
      {
        e.DrawBackground(Color.Goldenrod);
      }
      else
      {
        e.DrawBackground(Color.White);
      }

      AccessPoint ap = m_lastAPList[e.Index];

      if (m_titleHeight == 0)
      {
        m_titleHeight = ((int)(e.Graphics.MeasureString(ap.Name, m_itemTitleFont).Height));
      }

      string properties1 = MACToString(ap.MacAddress);
      

      // Begin bitmap section
      int bars = 0;
      if (ap.SignalStrengthInDecibels < -80)
        bars = 5;
      else if (ap.SignalStrengthInDecibels < -70)
        bars = 4;
      else if (ap.SignalStrengthInDecibels < -60)
        bars = 3;
      else if (ap.SignalStrengthInDecibels < -50)
        bars = 2;
      else
        bars = 1;

      string bmpName = string.Format("NetUI.Resource.{0}Bars{1}.bmp", bars, ap.Privacy != 0 ? "L" : "");

      Bitmap strength = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(bmpName));

      // TODO: cache these instead of calculating every time
      int bmpWidth = strength.Width;
      ImageAttributes transparentAttributes = new ImageAttributes();
      Color transparentColor = strength.GetPixel(0, 0);
      transparentAttributes.SetColorKey(transparentColor, transparentColor);

      Rectangle destinationRectangle = new Rectangle(e.Bounds.Left, e.Bounds.Top, strength.Width, strength.Height);
      e.Graphics.DrawImage(strength, destinationRectangle, 0, 0, strength.Width, strength.Height, GraphicsUnit.Pixel, transparentAttributes);

      // End bitmap section
      */
      // Begin bitmap section
      int bars = 0;
      
      if (ap.SignalStrength.Decibels < -80)
        bars = 5;
      else if (ap.SignalStrength.Decibels < -70)
        bars = 4;
      else if (ap.SignalStrength.Decibels < -60)
        bars = 3;
      else if (ap.SignalStrength.Decibels < -50)
        bars = 2;
      else
        bars = 1;

      string bmpName = string.Format("WiFinder.Resources.{0}Bars{1}.bmp", bars, ap.Privacy != 0 ? "L" : "");
      Bitmap strength = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(bmpName));

      // TODO: cache these instead of calculating every time
      int bmpWidth = strength.Width;
      ImageAttributes transparentAttributes = new ImageAttributes();
      Color transparentColor = strength.GetPixel(0, 0);
      transparentAttributes.SetColorKey(transparentColor, transparentColor);

      Rectangle destinationRectangle = new Rectangle(e.Bounds.Left + 2, e.Bounds.Top, e.Bounds.Height, e.Bounds.Height);
      e.Graphics.DrawImage(strength, destinationRectangle, 0, 0, strength.Width, strength.Height, GraphicsUnit.Pixel, transparentAttributes);

      // End bitmap section
      
      if (m_titleHeight == 0)
      {
        m_titleHeight = ((int)(e.Graphics.MeasureString(ap.Name, m_itemTitleFont).Height));
      }

      string properties1 = ap.PhysicalAddress.ToString();

      string properties2 = string.Format(
          "{0} mode - Privacy {1}",
          ap.InfrastructureMode.ToString(),
          ap.Privacy != 0 ? "Enabled" : "Disabled");

      if (e.Index <= nearbyAPList.Items.Count - 1)
      {
        e.Graphics.DrawString(string.Format("{0} ({1}dB)", ap.Name, ap.SignalStrength.Decibels), m_itemTitleFont, m_itemTitleBrush, bmpWidth, e.Bounds.Top);
        e.Graphics.DrawString(properties1, m_itemDetailFont, m_itemDetailBrush, bmpWidth, e.Bounds.Top + m_titleHeight);
        e.Graphics.DrawString(properties2, m_itemDetailFont, m_itemDetailBrush, bmpWidth, e.Bounds.Top + (2 * m_titleHeight));
      }

      e.Graphics.ResetClip();
    }
  }
}
