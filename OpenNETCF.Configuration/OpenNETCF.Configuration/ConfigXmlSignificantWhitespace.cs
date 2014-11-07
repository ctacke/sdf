using System;
using System.Xml;

namespace OpenNETCF.Configuration
{
	sealed class ConfigXmlSignificantWhitespace : XmlSignificantWhitespace, IConfigXmlNode
	{
		private int _line;
		private string _filename;

		public ConfigXmlSignificantWhitespace(string filename, int line, string strData, XmlDocument doc) : base(strData, doc)
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
			ConfigXmlSignificantWhitespace configXmlSignificantWhitespace = xmlNode as ConfigXmlSignificantWhitespace;
			if (configXmlSignificantWhitespace != null)
			{
				configXmlSignificantWhitespace._line = _line;
				configXmlSignificantWhitespace._filename = _filename;
			}
			return xmlNode;
		}
	}
}
