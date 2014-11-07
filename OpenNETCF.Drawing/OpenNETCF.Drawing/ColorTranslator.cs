using System;
using System.Drawing;

namespace OpenNETCF.Drawing
{
	/// <summary>
	/// Translates colors to and from <see cref="Color"/> structures.
	/// </summary>
	/// <seealso cref="T:System.Drawing.ColorTranslator">System.Drawing.ColorTranslator Class</seealso>
	public static class ColorTranslator
	{
		#region To Html
		/// <summary>
		/// Translates the specified <see cref="T:System.Drawing.Color"/> structure to an HTML string color representation.
		/// </summary>
		/// <param name="c">The <see cref="T:System.Drawing.Color"/> structure to translate.</param>
		/// <returns>The string that represents the HTML color.</returns>
		/// <remarks>Unlike the desktop version of this function it does not check for named colors but instead always returns the hex notation values - e.g. Color.Red = "#FF0000"</remarks>
		/// <seealso cref="M:System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color)">System.Drawing.ColorTranslator.ToHtml Method</seealso>
		public static string ToHtml(Color c)
		{		
			return string.Format("#{0:X6}", (c.R << 16) + (c.G << 8) + c.B);
		}
		#endregion

		#region To Win32
		/// <summary>
		/// Translates the specified <see cref="Color"/> structure to a Windows color.
		/// </summary>
		/// <param name="c">The <see cref="Color"/> structure to translate.</param>
		/// <returns>The Windows color value.</returns>
		/// <seealso cref="M:System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color)">System.Drawing.ColorTranslator.ToWin32 Method</seealso>
		public static int ToWin32(System.Drawing.Color c)
		{
			return c.R + (c.G << 8) + (c.B << 16);
		}
		#endregion

		#region From Html
		/// <summary>
		/// Translates an HTML color representation to a <see cref="T:System.Drawing.Color"/> structure.
		/// </summary>
		/// <param name="htmlColor">The string representation of the Html color to translate.</param>
		/// <returns>The <see cref="T:System.Drawing.Color"/> structure that represents the translated HTML color.</returns>
		/// <seealso cref="M:System.Drawing.ColorTranslator.FromHtml(System.String)">System.Drawing.ColorTranslator.FromHtml Method</seealso>
		public static System.Drawing.Color FromHtml(string htmlColor)
		{
			Color c = Color.Empty;
			if ((htmlColor != null) && (htmlColor.Length != 0))
			{
				if ((htmlColor[0] == '#') && (htmlColor.Length == 7 || htmlColor.Length == 4))
				{
					if (htmlColor.Length == 7) // #rrggbb format
					{
						c = Color.FromArgb(Convert.ToInt32(htmlColor.Substring(1, 2), 0x10), Convert.ToInt32(htmlColor.Substring(3, 2), 0x10), Convert.ToInt32(htmlColor.Substring(5, 2), 0x10));
					}
					else // #rgb format
					{
						string r = char.ToString(htmlColor[1]);
						string g = char.ToString(htmlColor[2]);
						string b = char.ToString(htmlColor[3]);
						c = Color.FromArgb(Convert.ToInt32(r + r, 16), Convert.ToInt32(g + g, 16), Convert.ToInt32(b + b, 16));
					}
				}
			}
			if (c.IsEmpty)
			{
				//a string
				try
				{
					c = (Color)typeof(System.Drawing.Color).GetProperty(htmlColor).GetValue(null,null);
				}
				catch
				{
					throw new ArgumentException("Unable to convert color name");
				}
			}

			return c;
		}
		#endregion

		#region From Win32
		/// <summary>
		/// Translates a Windows color value to a <see cref="Color"/> structure.
		/// </summary>
		/// <param name="win32Color">The Windows color to translate.</param>
		/// <returns>The <see cref="Color"/> structure that represents the translated Windows color.</returns>
		/// <seealso cref="M:System.Drawing.ColorTranslator.FromWin32(System.Int32)">System.Drawing.ColorTranslator.FromWin32 Method</seealso>
		public static Color FromWin32(int win32Color)
		{
			return Color.FromArgb( win32Color & 0xff, (win32Color >> 8) & 0xff,(win32Color >> 16) & 0xff);
		}
		#endregion

        #region From OLE
        /// <summary>
        /// Translates an OLE color value to a <see cref="Color"/> structure.
        /// </summary>
        /// <param name="oleColor">The OLE color to translate.</param>
        /// <returns>The <see cref="Color"/> structure that represents the translated OLE color.</returns>
        public static Color FromOle(int oleColor)
        {
            if (((oleColor & (-16777216)) == -2147483648) && ((oleColor & 0xffffff) <= 0x18))
            {
                switch (oleColor)
                {
                    case -2147483648:
                        {
                            return (SystemColors.ScrollBar);
                        }
                    case -2147483647:
                        {
                            return (SystemColors.Desktop);
                        }
                    case -2147483646:
                        {
                            return (SystemColors.ActiveCaption);
                        }
                    case -2147483645:
                        {
                            return (SystemColors.InactiveCaption);
                        }
                    case -2147483644:
                        {
                            return (SystemColors.Menu);
                        }
                    case -2147483643:
                        {
                            return (SystemColors.Window);
                        }
                    case -2147483642:
                        {
                            return (SystemColors.WindowFrame);
                        }
                    case -2147483641:
                        {
                            return (SystemColors.MenuText);
                        }
                    case -2147483640:
                        {
                            return (SystemColors.WindowText);
                        }
                    case -2147483639:
                        {
                            return (SystemColors.ActiveCaptionText);
                        }
                    case -2147483638:
                        {
                            return (SystemColors.ActiveBorder);
                        }
                    case -2147483637:
                        {
                            return (SystemColors.InactiveBorder);
                        }
                    case -2147483636:
                        {
                            return (SystemColors.AppWorkspace);
                        }
                    case -2147483635:
                        {
                            return (SystemColors.Highlight);
                        }
                    case -2147483634:
                        {
                            return (SystemColors.HighlightText);
                        }
                    case -2147483633:
                        {
                            return (SystemColors.Control);
                        }
                    case -2147483632:
                        {
                            return (SystemColors.ControlDark);
                        }
                    case -2147483631:
                        {
                            return (SystemColors.GrayText);
                        }
                    case -2147483630:
                        {
                            return (SystemColors.ControlText);
                        }
                    case -2147483629:
                        {
                            return (SystemColors.InactiveCaptionText);
                        }
                    case -2147483628:
                        {
                            return (SystemColors.ControlLightLight);
                        }
                    case -2147483627:
                        {
                            return (SystemColors.ControlDarkDark);
                        }
                    case -2147483626:
                        {
                            return (SystemColors.ControlLight);
                        }
                    case -2147483625:
                        {
                            return (SystemColors.InfoText);
                        }
                    case -2147483624:
                        {
                            return (SystemColors.Info);
                        }
                }
            }
            Color color = Color.FromArgb((byte)(oleColor & 0xff), (byte)((oleColor >> 8) & 0xff), (byte)((oleColor >> 0x10) & 0xff));
            return color;
        }
        #endregion

        #region To OLE
        /// <summary>
        /// Translates the specified <see cref="Color"/> structure to an OLE color.
        /// </summary>
        /// <param name="c">The <see cref="Color"/> structure to translate.</param>
        /// <returns>The OLE color value.</returns>
        public static int ToOle(Color c)
        {
            if (c.IsSystemColor)
            {
                if (c.ToArgb() == SystemColors.ActiveBorder.ToArgb())
                {
                    return -2147483638;
                }
                else if (c.ToArgb() == SystemColors.ActiveCaption.ToArgb())
                {
                    return -2147483646;
                }
                else if (c.ToArgb() == SystemColors.ActiveCaptionText.ToArgb())
                {
                    return -2147483639;
                }
                else if (c.ToArgb() == SystemColors.AppWorkspace.ToArgb())
                {
                    return -2147483636;
                }
                else if (c.ToArgb() == SystemColors.Control.ToArgb())
                {
                    return -2147483633;
                }
                else if (c.ToArgb() == SystemColors.ControlDark.ToArgb())
                {
                    return -2147483632;
                }
                else if (c.ToArgb() == SystemColors.ControlDarkDark.ToArgb())
                {
                    return -2147483627;
                }
                else if (c.ToArgb() == SystemColors.ControlLight.ToArgb())
                {
                    return -2147483626;
                }
                else if (c.ToArgb() == SystemColors.ControlLightLight.ToArgb())
                {
                    return -2147483628;
                }
                else if (c.ToArgb() == SystemColors.ControlText.ToArgb())
                {
                    return -2147483630;
                }
                else if (c.ToArgb() == SystemColors.Desktop.ToArgb())
                {
                    return -2147483647;
                }
                else if (c.ToArgb() == SystemColors.GrayText.ToArgb())
                {
                    return -2147483631;
                }
                else if (c.ToArgb() == SystemColors.Highlight.ToArgb())
                {
                    return -2147483635;
                }
                else if (c.ToArgb() == SystemColors.HighlightText.ToArgb())
                {
                    return -2147483634;
                }
                else if (c.ToArgb() == SystemColors.HotTrack.ToArgb())
                {
                    return -2147483635;
                }
                else if (c.ToArgb() == SystemColors.InactiveBorder.ToArgb())
                {
                    return -2147483637;
                }
                else if (c.ToArgb() == SystemColors.InactiveCaption.ToArgb())
                {
                    return -2147483645;
                }
                else if (c.ToArgb() == SystemColors.InactiveCaptionText.ToArgb())
                {
                    return -2147483629;
                }
                else if (c.ToArgb() == SystemColors.Info.ToArgb())
                {
                    return -2147483624;
                }
                else if (c.ToArgb() == SystemColors.InfoText.ToArgb())
                {
                    return -2147483625;
                }
                else if (c.ToArgb() == SystemColors.Menu.ToArgb())
                {
                    return -2147483644;
                }
                else if (c.ToArgb() == SystemColors.MenuText.ToArgb())
                {
                    return -2147483641;
                }
                else if (c.ToArgb() == SystemColors.ScrollBar.ToArgb())
                {
                    return -2147483648;
                }
                else if (c.ToArgb() == SystemColors.Window.ToArgb())
                {
                    return -2147483643;
                }
                else if (c.ToArgb() == SystemColors.WindowFrame.ToArgb())
                {
                    return -2147483642;
                }
                else if (c.ToArgb() == SystemColors.WindowText.ToArgb())
                {
                    return -2147483640;
                }
            }
            return ColorTranslator.ToWin32(c);
        }
        #endregion
	}
}
