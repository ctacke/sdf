
using System;
using System.Text;

namespace OpenNETCF.Web.Services2
{
	/// <summary>
	/// for stuff that was in bNb.Sec and hid in OpenNET
	/// </summary>
	public class CryptoEx
	{
		public CryptoEx()
		{
		}

		public static string ByteArrayToString(byte [] data)
		{
			StringBuilder sb = new StringBuilder();
			foreach(byte b in data)
			{
				sb.Append(b.ToString());
				sb.Append(" ");
			}
			return sb.ToString().Trim();
		}
	}
}
