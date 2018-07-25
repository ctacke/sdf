using System;
using System.Xml;

namespace OpenNETCF.Configuration
{
	sealed class ConfigXmlCDataSection : XmlCDataSection, IConfigXmlNode
	{
		private int _line;
		private string _filename;

		public ConfigXmlCDataSection(string filename, int line, string data, XmlDocument doc) : base(data, doc)
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
			ConfigXmlCDataSection configXmlCDataSection = xmlNode as ConfigXmlCDataSection;
			if (configXmlCDataSection != null)
			{
				configXmlCDataSection._line = _line;
				configXmlCDataSection._filename = _filename;
			}
			return xmlNode;
		}
	}
}
