
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System;

namespace OpenNETCF.Drawing {

    /// <summary>
    /// 
    /// </summary>
	public static class SystemColorsHelper {

		#region SetSchemeByName

		#region SchemeName
    /// <summary>
    /// System Color scheme names
    /// </summary>
		public enum SchemeName {
      /// <summary>
      /// Brick
      /// </summary>
			Brick = 1,
      /// <summary>
      /// Desert
      /// </summary>
			Desert,
      /// <summary>
      /// Eggplant
      /// </summary>
			Eggplant,
      /// <summary>
      /// High-Contrast Black
      /// </summary>
			HighContrastBlack,
      /// <summary>
      /// High-Contrast White
      /// </summary>
			HighContrastWhite,
      /// <summary>
      /// Lilac
      /// </summary>
			Lilac = 6,
			//Maple
			//Marine
			//Rose
      /// <summary>
      /// Spruce
      /// </summary>
			Spruce = 10,
      /// <summary>
      /// Storm
      /// </summary>
			Storm = 11,
			//Wheat
      /// <summary>
      /// Windows Standard
      /// </summary>
			WindowsStandard = 13
		}
		#endregion

    /// <summary>
    /// Sets the current system color scheme
    /// </summary>
    /// <param name="aSchemeName"></param>
		public static void SetSchemeByName(SchemeName aSchemeName) {
			if ((int)aSchemeName < 1 || (int)aSchemeName > 13) {
				throw new ApplicationException("Please use the enum and don't make up values");
			}
			Int32[] el = {0 | SYS_COLOR_INDEX_FLAG, 1 | SYS_COLOR_INDEX_FLAG, 2 | SYS_COLOR_INDEX_FLAG, 3 | SYS_COLOR_INDEX_FLAG, 4 | SYS_COLOR_INDEX_FLAG, 5 | SYS_COLOR_INDEX_FLAG, 6 | SYS_COLOR_INDEX_FLAG, 7 | SYS_COLOR_INDEX_FLAG, 8 | SYS_COLOR_INDEX_FLAG, 9 | SYS_COLOR_INDEX_FLAG, 10 | SYS_COLOR_INDEX_FLAG, 11 | SYS_COLOR_INDEX_FLAG, 12 | SYS_COLOR_INDEX_FLAG, 13 | SYS_COLOR_INDEX_FLAG, 14 | SYS_COLOR_INDEX_FLAG, 15 | SYS_COLOR_INDEX_FLAG, 16 | SYS_COLOR_INDEX_FLAG, 17 | SYS_COLOR_INDEX_FLAG, 18 | SYS_COLOR_INDEX_FLAG, 19 | SYS_COLOR_INDEX_FLAG, 20 | SYS_COLOR_INDEX_FLAG, 21 | SYS_COLOR_INDEX_FLAG, 22 | SYS_COLOR_INDEX_FLAG, 23 | SYS_COLOR_INDEX_FLAG, 24 | SYS_COLOR_INDEX_FLAG, 25 | SYS_COLOR_INDEX_FLAG, 26 | SYS_COLOR_INDEX_FLAG, 27 | SYS_COLOR_INDEX_FLAG, 28 | SYS_COLOR_INDEX_FLAG};
			Int32[] rgbs = GetArrayOfRGBs(aSchemeName);
			if (SetSysColors(29, el, rgbs) == false) {
				throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
			}

			// write name to registry
			string n;
			if (aSchemeName == SchemeName.HighContrastBlack) {
				n = "High Contrast Black";
			} else if (aSchemeName == SchemeName.HighContrastWhite) {
				n = "High Contrast White";
			} else if (aSchemeName == SchemeName.WindowsStandard) {
				n = "Windows Standard";
			} else {
				n = aSchemeName.ToString();
			}
			//	TODO write to registry the name (key: HKEY_CURRENT_USER\ControlPanel\Appearance name:Current)
			
		}

		private static Int32[] GetArrayOfRGBs(SchemeName aSchemeName) {
			if (aSchemeName == SchemeName.Brick) {
				return GetBrick();
			} else if (aSchemeName == SchemeName.Desert) {
				return GetDesert();
			} else if (aSchemeName == SchemeName.Eggplant) {
				return GetEggplant();
			} else if (aSchemeName == SchemeName.HighContrastBlack) {
				return GetHighContrastBlack();
			} else if (aSchemeName == SchemeName.HighContrastWhite) {
				return GetHighContrastWhite();
			} else if (aSchemeName == SchemeName.Lilac) {
				return GetLilac();
			} else if (aSchemeName == SchemeName.Spruce) {
				return GetSpruce();
			} else if (aSchemeName == SchemeName.Storm) {
				return GetStorm();
			} else if (aSchemeName == SchemeName.WindowsStandard) {
				return GetWindowsStandard();
			}else{
				throw new NotSupportedException();
			}
			//Case SchemeName.Maple, SchemeName.Marine, SchemeName.Rose, SchemeName.Wheat
			//	Throw New NotSupportedException("Maple, Marine, Rose requires non-zero alpha")

		}

		private static Int32[] GetEggplant() {
			return new Int32[]{BitConverter.ToInt32(new byte[]{213, 231, 211, 0}, 0), BitConverter.ToInt32(new byte[]{64, 0, 64, 0}, 0), BitConverter.ToInt32(new byte[]{77, 0, 77, 0}, 0), BitConverter.ToInt32(new byte[]{165, 80, 141, 0}, 0), BitConverter.ToInt32(new byte[]{91, 152, 86, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{91, 152, 86, 0}, 0), BitConverter.ToInt32(new byte[]{91, 152, 86, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{55, 92, 52, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{91, 152, 86, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{120, 82, 109, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{79, 79, 79, 0}, 0), BitConverter.ToInt32(new byte[]{160, 201, 156, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{128, 0, 128, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{91, 152, 86, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{16, 132, 208, 0}, 0), BitConverter.ToInt32(new byte[]{181, 181, 181, 0}, 0)};
		}

		private static Int32[] GetHighContrastBlack() {
			return new Int32[]{BitConverter.ToInt32(new byte[]{192, 192, 192, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{128, 0, 128, 0}, 0), BitConverter.ToInt32(new byte[]{0, 128, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{192, 192, 192, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{192, 192, 192, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{192, 192, 192, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{16, 132, 208, 0}, 0), BitConverter.ToInt32(new byte[]{181, 181, 181, 0}, 0)};
		}

		private static Int32[] GetHighContrastWhite() {
			return new Int32[]{BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{192, 192, 192, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{192, 192, 192, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{192, 192, 192, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{16, 132, 208, 0}, 0), BitConverter.ToInt32(new byte[]{181, 181, 181, 0}, 0)};
		}

		private static Int32[] GetLilac() {
			return new Int32[]{BitConverter.ToInt32(new byte[]{163, 168, 217, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{90, 78, 177, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{163, 168, 217, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{163, 168, 217, 0}, 0), BitConverter.ToInt32(new byte[]{163, 168, 217, 0}, 0), BitConverter.ToInt32(new byte[]{90, 78, 177, 0}, 0), BitConverter.ToInt32(new byte[]{90, 78, 177, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{163, 168, 217, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{105, 105, 105, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{216, 168, 236, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{163, 168, 217, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{16, 132, 208, 0}, 0), BitConverter.ToInt32(new byte[]{181, 181, 181, 0}, 0)};
		}

		private static Int32[] GetSpruce() {
			return new Int32[]{BitConverter.ToInt32(new byte[]{208, 227, 211, 0}, 0), BitConverter.ToInt32(new byte[]{46, 88, 46, 0}, 0), BitConverter.ToInt32(new byte[]{89, 151, 100, 0}, 0), BitConverter.ToInt32(new byte[]{127, 127, 127, 0}, 0), BitConverter.ToInt32(new byte[]{162, 200, 169, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{162, 200, 169, 0}, 0), BitConverter.ToInt32(new byte[]{162, 200, 169, 0}, 0), BitConverter.ToInt32(new byte[]{208, 227, 211, 0}, 0), BitConverter.ToInt32(new byte[]{89, 151, 100, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{162, 200, 169, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{99, 165, 99, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{208, 227, 211, 0}, 0), BitConverter.ToInt32(new byte[]{208, 227, 211, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{162, 200, 169, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{16, 132, 208, 0}, 0), BitConverter.ToInt32(new byte[]{181, 181, 181, 0}, 0)};
		}

		private static Int32[] GetStorm() {
			return new Int32[]{BitConverter.ToInt32(new byte[]{231, 231, 231, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 47, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{192, 192, 192, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{160, 160, 160, 0}, 0), BitConverter.ToInt32(new byte[]{160, 160, 160, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 102, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{160, 160, 160, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{36, 36, 36, 0}, 0), BitConverter.ToInt32(new byte[]{231, 231, 231, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{128, 0, 128, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{160, 160, 160, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{16, 132, 208, 0}, 0), BitConverter.ToInt32(new byte[]{181, 181, 181, 0}, 0)};
		}

		private static Int32[] GetWindowsStandard() {
			return new Int32[]{BitConverter.ToInt32(new byte[]{224, 224, 224, 0}, 0), BitConverter.ToInt32(new byte[]{58, 110, 165, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 128, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{192, 192, 192, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{192, 192, 192, 0}, 0), BitConverter.ToInt32(new byte[]{192, 192, 192, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 128, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{192, 192, 192, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{192, 192, 192, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{223, 223, 223, 0}, 0), BitConverter.ToInt32(new byte[]{128, 0, 128, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{192, 192, 192, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{16, 132, 208, 0}, 0), BitConverter.ToInt32(new byte[]{181, 181, 181, 0}, 0)};
		}

		private static Int32[] GetBrick() {
			return new Int32[]{BitConverter.ToInt32(new byte[]{232, 231, 221, 0}, 0), BitConverter.ToInt32(new byte[]{155, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{128, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{141, 137, 97, 0}, 0), BitConverter.ToInt32(new byte[]{162, 157, 117, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{225, 224, 210, 0}, 0), BitConverter.ToInt32(new byte[]{163, 158, 118, 0}, 0), BitConverter.ToInt32(new byte[]{163, 158, 118, 0}, 0), BitConverter.ToInt32(new byte[]{225, 224, 210, 0}, 0), BitConverter.ToInt32(new byte[]{141, 137, 97, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{163, 158, 118, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{225, 224, 210, 0}, 0), BitConverter.ToInt32(new byte[]{232, 231, 221, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{128, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{225, 224, 210, 0}, 0), BitConverter.ToInt32(new byte[]{159, 155, 115, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{16, 132, 208, 0}, 0), BitConverter.ToInt32(new byte[]{181, 181, 181, 0}, 0)};
		}

		private static Int32[] GetDesert() {
			return new Int32[]{BitConverter.ToInt32(new byte[]{234, 230, 221, 0}, 0), BitConverter.ToInt32(new byte[]{162, 141, 104, 0}, 0), BitConverter.ToInt32(new byte[]{0, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{162, 141, 104, 0}, 0), BitConverter.ToInt32(new byte[]{204, 194, 173, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{204, 194, 173, 0}, 0), BitConverter.ToInt32(new byte[]{204, 194, 173, 0}, 0), BitConverter.ToInt32(new byte[]{162, 141, 104, 0}, 0), BitConverter.ToInt32(new byte[]{0, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{204, 194, 173, 0}, 0), BitConverter.ToInt32(new byte[]{128, 128, 128, 0}, 0), BitConverter.ToInt32(new byte[]{162, 141, 104, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{234, 230, 221, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{255, 255, 255, 0}, 0), BitConverter.ToInt32(new byte[]{204, 194, 173, 0}, 0), BitConverter.ToInt32(new byte[]{0, 0, 0, 0}, 0), BitConverter.ToInt32(new byte[]{16, 132, 208, 0}, 0), BitConverter.ToInt32(new byte[]{181, 181, 181, 0}, 0)};
		}
		#endregion

		#region Consts
		private const Int32 SYS_COLOR_INDEX_FLAG = 0x40000000;

		private const Int32 COLOR_SCROLLBAR = 0 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_BACKGROUND = 1 | SYS_COLOR_INDEX_FLAG; //COLOR_DESKTOP
		private const Int32 COLOR_ACTIVECAPTION = 2 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_INACTIVECAPTION = 3 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_MENU = 4 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_WINDOW = 5 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_WINDOWFRAME = 6 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_MENUTEXT = 7 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_WINDOWTEXT = 8 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_CAPTIONTEXT = 9 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_ACTIVEBORDER = 10 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_INACTIVEBORDER = 11 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_APPWORKSPACE = 12 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_HIGHLIGHT = 13 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_HIGHLIGHTTEXT = 14 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_BTNFACE = 15 | SYS_COLOR_INDEX_FLAG; //COLOR_3DFACE
		private const Int32 COLOR_BTNSHADOW = 16 | SYS_COLOR_INDEX_FLAG; //COLOR_3DSHADOW
		private const Int32 COLOR_GRAYTEXT = 17 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_BTNTEXT = 18 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_INACTIVECAPTIONTEXT = 19 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_BTNHIGHLIGHT = 20 | SYS_COLOR_INDEX_FLAG; //COLOR_3DHIGHLIGHT, COLOR_3DHILIGHT, COLOR_BTNHILIGHT
		private const Int32 COLOR_3DDKSHADOW = 21 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_3DLIGHT = 22 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_INFOTEXT = 23 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_INFOBK = 24 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_STATIC = 25 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_STATICTEXT = 26 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_GRADIENTACTIVECAPTION = 27 | SYS_COLOR_INDEX_FLAG;
		private const Int32 COLOR_GRADIENTINACTIVECAPTION = 28 | SYS_COLOR_INDEX_FLAG;
		#endregion

		#region pinvokes
		[DllImport("coredll.dll", SetLastError=true)]
		private static extern bool SetSysColors(Int32 cElements, Int32[] lpaElements, Int32[] lpaRgbValues);

		[DllImport("coredll.dll", SetLastError=true)]
		private static extern Int32 GetSysColor(Int32 nIndex);
		#endregion

		#region Properties
    /// <summary>
    /// Gets a Color structure that is the lightest color in the color gradient of an active window's title bar.
    /// </summary>
		public static Color GradientActiveCaption {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_GRADIENTACTIVECAPTION));
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_GRADIENTACTIVECAPTION}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the lightest color in the color gradient of an inactive window's title bar.
    /// </summary>
		public static Color GradientInactiveCaption {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_GRADIENTINACTIVECAPTION));
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_GRADIENTINACTIVECAPTION}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of the active window's border.
    /// </summary>
		public static Color ActiveBorder {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_ACTIVEBORDER));
				Trace.Assert(SystemColors.ActiveBorder.R == c.R && SystemColors.ActiveBorder.G == c.G && SystemColors.ActiveBorder.B == c.B && SystemColors.ActiveBorder.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_ACTIVEBORDER}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of the application workspace.
    /// </summary>
		public static Color AppWorkspace {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_APPWORKSPACE));
				Trace.Assert(SystemColors.AppWorkspace.R == c.R && SystemColors.AppWorkspace.G == c.G && SystemColors.AppWorkspace.B == c.B && SystemColors.AppWorkspace.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_APPWORKSPACE}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the face color of a 3-D element.
    /// </summary>
		public static Color Control {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_BTNFACE));
				Trace.Assert(SystemColors.Control.R == c.R && SystemColors.Control.G == c.G && SystemColors.Control.B == c.B && SystemColors.Control.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_BTNFACE}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the light color of a 3-D element.
    /// </summary>
		public static Color ControlLight {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_3DLIGHT));
				Trace.Assert(SystemColors.ControlLight.R == c.R && SystemColors.ControlLight.G == c.G && SystemColors.ControlLight.B == c.B && SystemColors.ControlLight.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_3DLIGHT}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the highlight color of a 3-D element.
    /// </summary>
		public static Color ControlLightLight {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_BTNHIGHLIGHT));
				Trace.Assert(SystemColors.ControlLightLight.R == c.R && SystemColors.ControlLightLight.G == c.G && SystemColors.ControlLightLight.B == c.B && SystemColors.ControlLightLight.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_BTNHIGHLIGHT}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the shadow color of a 3-D element.
    /// </summary>
		public static Color ControlDark {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_BTNSHADOW));
				Trace.Assert(SystemColors.ControlDark.R == c.R && SystemColors.ControlDark.G == c.G && SystemColors.ControlDark.B == c.B && SystemColors.ControlDark.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_BTNSHADOW}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the dark shadow color of a 3-D element.
    /// </summary>
		public static Color ControlDarkDark {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_3DDKSHADOW));
				Trace.Assert(SystemColors.ControlDarkDark.R == c.R && SystemColors.ControlDarkDark.G == c.G && SystemColors.ControlDarkDark.B == c.B && SystemColors.ControlDarkDark.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_3DDKSHADOW}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of the background of a scroll bar.
    /// </summary>
		public static Color ScrollBar {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_SCROLLBAR));
				Trace.Assert(SystemColors.ScrollBar.R == c.R && SystemColors.ScrollBar.G == c.G && SystemColors.ScrollBar.B == c.B && SystemColors.ScrollBar.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_SCROLLBAR}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of a window frame.
    /// </summary>
		public static Color WindowFrame {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_WINDOWFRAME));
				Trace.Assert(SystemColors.WindowFrame.R == c.R && SystemColors.WindowFrame.G == c.G && SystemColors.WindowFrame.B == c.B && SystemColors.WindowFrame.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_WINDOWFRAME}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color used to designate a hot-tracked item.
    /// </summary>
		public static Color HotTrack {
			get {
				Color c = DialogBoxText;
				Trace.Assert(SystemColors.HotTrack.R == c.R && SystemColors.HotTrack.G == c.G && SystemColors.HotTrack.B == c.B && SystemColors.HotTrack.A == c.A);
				return c;
			}
			set {
				DialogBoxText = value;
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of an inactive window's border.
    /// </summary>
		public static Color InactiveBorder {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_INACTIVEBORDER));
				Trace.Assert(SystemColors.InactiveBorder.R == c.R && SystemColors.InactiveBorder.G == c.G && SystemColors.InactiveBorder.B == c.B && SystemColors.InactiveBorder.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_INACTIVEBORDER}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of the background of a ToolTip.
    /// </summary>
		public static Color Info {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_INFOBK));
				Trace.Assert(SystemColors.Info.R == c.R && SystemColors.Info.G == c.G && SystemColors.Info.B == c.B && SystemColors.Info.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_INFOBK}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of the text of a ToolTip.
    /// </summary>
		public static Color InfoText {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_INFOTEXT));
				Trace.Assert(SystemColors.InfoText.R == c.R && SystemColors.InfoText.G == c.G && SystemColors.InfoText.B == c.B && SystemColors.InfoText.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_INFOTEXT}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of the background of the active window's title bar.
    /// </summary>
		public static Color ActiveCaption {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_ACTIVECAPTION));
				Trace.Assert(SystemColors.ActiveCaption.R == c.R && SystemColors.ActiveCaption.G == c.G && SystemColors.ActiveCaption.B == c.B && SystemColors.ActiveCaption.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_ACTIVECAPTION}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of the text in the active window's title bar.
    /// </summary>
		public static Color ActiveCaptionText {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_CAPTIONTEXT));
				Trace.Assert(SystemColors.ActiveCaptionText.R == c.R && SystemColors.ActiveCaptionText.G == c.G && SystemColors.ActiveCaptionText.B == c.B && SystemColors.ActiveCaptionText.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_CAPTIONTEXT}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// 	Gets a Color structure that is the color of text in a 3-D element.
    /// </summary>
		public static Color ControlText {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_BTNTEXT));
				Trace.Assert(SystemColors.ControlText.R == c.R && SystemColors.ControlText.G == c.G && SystemColors.ControlText.B == c.B && SystemColors.ControlText.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_BTNTEXT}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of the desktop.
    /// </summary>
		public static Color Desktop {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_BACKGROUND));
				Trace.Assert(SystemColors.Desktop.R == c.R && SystemColors.Desktop.G == c.G && SystemColors.Desktop.B == c.B && SystemColors.Desktop.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_BACKGROUND}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of the background of a Dialog.
    /// </summary>
		public static Color DialogBackground {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_STATIC));
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_STATIC}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of text on a Dialog.
    /// </summary>
		public static Color DialogBoxText {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_STATICTEXT));
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_STATICTEXT}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of dimmed text.
    /// </summary>
		public static Color GrayText {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_GRAYTEXT));
				Trace.Assert(SystemColors.GrayText.R == c.R && SystemColors.GrayText.G == c.G && SystemColors.GrayText.B == c.B && SystemColors.GrayText.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_GRAYTEXT}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of the background of an inactive window's title bar.
    /// </summary>
		public static Color InactiveCaption {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_INACTIVECAPTION));
				Trace.Assert(SystemColors.InactiveCaption.R == c.R && SystemColors.InactiveCaption.G == c.G && SystemColors.InactiveCaption.B == c.B && SystemColors.InactiveCaption.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_INACTIVECAPTION}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of the text in an inactive window's title bar.
    /// </summary>
		public static Color InactiveCaptionText {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_INACTIVECAPTIONTEXT));
				Trace.Assert(SystemColors.InactiveCaptionText.R == c.R && SystemColors.InactiveCaptionText.G == c.G && SystemColors.InactiveCaptionText.B == c.B && SystemColors.InactiveCaptionText.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_INACTIVECAPTIONTEXT}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of a menu's background.
    /// </summary>
		public static Color Menu {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_MENU));
				Trace.Assert(SystemColors.Menu.R == c.R && SystemColors.Menu.G == c.G && SystemColors.Menu.B == c.B && SystemColors.Menu.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_MENU}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of a menu's text.
    /// </summary>
		public static Color MenuText {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_MENUTEXT));
				Trace.Assert(SystemColors.MenuText.R == c.R && SystemColors.MenuText.G == c.G && SystemColors.MenuText.B == c.B && SystemColors.MenuText.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_MENUTEXT}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of the background of selected items.
    /// </summary>
		public static Color Highlight {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_HIGHLIGHT));
				Trace.Assert(SystemColors.Highlight.R == c.R && SystemColors.Highlight.G == c.G && SystemColors.Highlight.B == c.B && SystemColors.Highlight.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_HIGHLIGHT}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of the text of selected items.
    /// </summary>
		public static Color HighlightText {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_HIGHLIGHTTEXT));
				Trace.Assert(SystemColors.HighlightText.R == c.R && SystemColors.HighlightText.G == c.G && SystemColors.HighlightText.B == c.B && SystemColors.HighlightText.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_HIGHLIGHTTEXT}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of the background in the client area of a window.
    /// </summary>
		public static Color Window {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_WINDOW));
				Trace.Assert(SystemColors.Window.R == c.R && SystemColors.Window.G == c.G && SystemColors.Window.B == c.B && SystemColors.Window.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_WINDOW}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

    /// <summary>
    /// Gets a Color structure that is the color of the text in the client area of a window.
    /// </summary>
		public static Color WindowText {
			get {
				Color c = ColorTranslator.FromWin32(GetSysColor(COLOR_WINDOWTEXT));
				Trace.Assert(SystemColors.WindowText.R == c.R && SystemColors.WindowText.G == c.G && SystemColors.WindowText.B == c.B && SystemColors.WindowText.A == c.A);
				return c;
			}
			set {
				if (SetSysColors(1, new Int32[]{COLOR_WINDOWTEXT}, new Int32[]{ColorTranslator.ToWin32(value)}) == false) {
					throw new ApplicationException(Marshal.GetLastWin32Error().ToString());
				}
			}
		}

		#endregion
	}
}