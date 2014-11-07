using System;
using System.Xml;

namespace OpenNETCF.Configuration
{
	sealed class ConfigXmlWhitespace : XmlWhitespace, IConfigXmlNode
	{
		private int _line;
		private string _filename;

		public ConfigXmlWhitespace(string filename, int line, string comment, XmlDocument doc) : base(comment, doc)
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
			ConfigXmlWhitespace configXmlWhitespace = xmlNode as ConfigXmlWhitespace;
			if (configXmlWhitespace != null)
			{
				configXmlWhitespace._line = _line;
				configXmlWhitespace._filename = _filename;
			}
			return xmlNode;
		}
	}
}
