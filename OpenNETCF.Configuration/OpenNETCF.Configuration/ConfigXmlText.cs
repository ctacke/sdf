
using System;
using System.Xml;

namespace OpenNETCF.Configuration
{
	sealed class ConfigXmlText : XmlText, IConfigXmlNode
	{
		private int _line;

		private string _filename;


		public ConfigXmlText(string filename, int line, string strData, XmlDocument doc) : base(strData, doc)
		{
			_line = line;
			_filename = filename;
		}

		int IConfigXmlNode.LineNumber
		{
			get { return _line; }
		}

		string IConfigXmlNode.Filename
		{
			get { return _filename; }
		}

		public override XmlNode CloneNode(bool deep)
		{
			XmlNode xmlNode = base.CloneNode(deep);
			ConfigXmlText configXmlText = xmlNode as ConfigXmlText;
			if (configXmlText != null)
			{
				configXmlText._line = _line;
				configXmlText._filename = _filename;
			}
			return xmlNode;
		}
	}

}
